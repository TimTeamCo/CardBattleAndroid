using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using UnityEngine;

namespace NetCodeTT.Lobby
{
    using Unity.Services.Lobbies.Models;
    using Unity.Services.Lobbies;
    
    public class LobbyManager : MonoBehaviour, ILobby
    {
        public string _lobbyID;
        public bool isClient;
        private IEnumerator _heartbeatLobbyCoroutine;
        private ConcurrentQueue<string> _createdLobbyIds = new ConcurrentQueue<string>();
        private Lobby _currentLobby;
        private LobbyEventCallbacks m_LobbyEventCallbacks = new LobbyEventCallbacks();
        private const string key_RelayCode = nameof(LocalLobby.RelayCode);
        private const string key_LobbyState = nameof(LocalLobby.LocalLobbyState);

        public async Task<Lobby> QuickJoin(LocalPlayer localUser)
        {
            //We dont want to queue a quickjoin
            if (m_QuickJoinCooldown.IsCoolingDown)
            {
                Debug.LogWarning("Quick Join Lobby hit the rate limit.");
                return null;
            }

            await m_QuickJoinCooldown.QueueUntilCooldown();
            string uasId = AuthenticationService.Instance.PlayerId;
            
            var joinRequest = new QuickJoinLobbyOptions
            {
                Player = new Player(id: uasId, data: CreateInitialPlayerData(localUser))
            };

            Lobby loby;
            try
            {
                loby = await LobbyService.Instance.QuickJoinLobbyAsync(joinRequest);
                if (loby != null)
                {
                    return _currentLobby = loby;
                }
            }
            catch (Exception e)
            {
                loby = await CreateLobby(localUser);
                return _currentLobby = loby;
                throw;
            }
            
            return _currentLobby;
        }

        public async Task<Lobby> CreateLobby(LocalPlayer localUser)
        {
            if (m_CreateCooldown.IsCoolingDown)
            {
                Debug.LogWarning("Create Lobby hit the rate limit.");
                return null;
            }
            
            await m_CreateCooldown.QueueUntilCooldown();
            
            try
            {
                string uasId = AuthenticationService.Instance.PlayerId;

                CreateLobbyOptions createOptions = new CreateLobbyOptions
                {
                    IsPrivate = false,
                    Player = new Player(id: uasId, data: CreateInitialPlayerData(localUser))
                };
                
                string lobbyName = await GenerateLobbyName();
                int maxPlayers = 2;

                try
                {
                    _currentLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createOptions);
                    StartHeartBeat();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    throw;
                }

                return _currentLobby;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Lobby Create failed:\n{ex}");
                return null;
            }
        }

        private async Task<string> GenerateLobbyName()
        {
            var lobbyName = String.Empty;
            try
            {
                QueryLobbiesOptions options = new QueryLobbiesOptions();

                QueryResponse lobbies = await Lobbies.Instance.QueryLobbiesAsync(options);

                int id = 0;
                var lobbieses = lobbies.Results;
                if (lobbieses.Count > 0)
                {
                    for (int i = 0; i >= id; i++)
                    {
                        if (i >= lobbieses.Count)
                        {
                            return lobbyName = $"Lobby{i}";
                        }
                        
                        var lobby = lobbies.Results[i];
                        await GetLobby(lobby.Id, result =>
                        {
                            if (result != null) 
                                return;
                        });
                    }
                }
                else
                {
                    lobbyName = $"Lobby0";
                }
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError(e);
            }

            return lobbyName;
        }

        public async Task BindLocalLobbyToRemote(string lobbyID, LocalLobby localLobby)
        {
            m_LobbyEventCallbacks.LobbyChanged += async changes =>
            {
                if (changes.LobbyDeleted)
                {
                    await LeaveLobbyAsync();
                    return;
                }

                //Lobby Fields
                if (changes.Name.Changed)
                    localLobby.LobbyName.Value = changes.Name.Value;
                if (changes.HostId.Changed)
                    localLobby.HostID.Value = changes.HostId.Value;
                if (changes.IsPrivate.Changed)
                    localLobby.Private.Value = changes.IsPrivate.Value;
                if (changes.IsLocked.Changed)
                    localLobby.Locked.Value = changes.IsLocked.Value;
                if (changes.AvailableSlots.Changed)
                    localLobby.AvailableSlots.Value = changes.AvailableSlots.Value;
                if (changes.MaxPlayers.Changed)
                    localLobby.MaxPlayerCount.Value = changes.MaxPlayers.Value;

                if (changes.LastUpdated.Changed)
                    localLobby.LastUpdated.Value = changes.LastUpdated.Value.ToFileTimeUtc();

                //Custom Lobby Fields
                if (changes.Data.Changed)
                    LobbyChanged();

                if (changes.PlayerJoined.Changed)
                    PlayersJoined();

                if (changes.PlayerLeft.Changed)
                    PlayersLeft();

                if (changes.PlayerData.Changed)
                    PlayerDataChanged();

                void LobbyChanged()
                {
                    foreach (var change in changes.Data.Value)
                    {
                        var changedValue = change.Value;
                        var changedKey = change.Key;

                        if (changedValue.Removed)
                        {
                            RemoveCustomLobbyData(changedKey);
                        }

                        if (changedValue.Changed)
                        {
                            ParseCustomLobbyData(changedKey, changedValue.Value);
                        }
                    }

                    void RemoveCustomLobbyData(string changedKey)
                    {
                        if (changedKey == key_RelayCode)
                            localLobby.RelayCode.Value = "";
                    }

                    void ParseCustomLobbyData(string changedKey, DataObject playerDataObject)
                    {
                        if (changedKey == key_RelayCode)
                            localLobby.RelayCode.Value = playerDataObject.Value;

                        if (changedKey == key_LobbyState)
                            localLobby.LocalLobbyState.Value = (LobbyState)int.Parse(playerDataObject.Value);

                        // if (changedKey == key_LobbyColor)
                            // localLobby.LocalLobbyColor.Value = (LobbyColor)int.Parse(playerDataObject.Value);
                    }
                }

                void PlayersJoined()
                {
                    foreach (var playerChanges in changes.PlayerJoined.Value)
                    {
                        Player joinedPlayer = playerChanges.Player;

                        var id = joinedPlayer.Id;
                        var index = playerChanges.PlayerIndex;
                        var isHost = localLobby.HostID.Value == id;

                        var newPlayer = new LocalPlayer(id, index, isHost);

                        foreach (var dataEntry in joinedPlayer.Data)
                        {
                            var dataObject = dataEntry.Value;
                            ParseCustomPlayerData(newPlayer, dataEntry.Key, dataObject.Value);
                        }

                        localLobby.AddPlayer(index, newPlayer);
                    }
                }

                void PlayersLeft()
                {
                    foreach (var leftPlayerIndex in changes.PlayerLeft.Value)
                    {
                        localLobby.RemovePlayer(leftPlayerIndex);
                    }
                }

                void PlayerDataChanged()
                {
                    foreach (var lobbyPlayerChanges in changes.PlayerData.Value)
                    {
                        var playerIndex = lobbyPlayerChanges.Key;
                        var localPlayer = localLobby.GetLocalPlayer(playerIndex);
                        if (localPlayer == null)
                            continue;
                        var playerChanges = lobbyPlayerChanges.Value;
                        if (playerChanges.ConnectionInfoChanged.Changed)
                        {
                            var connectionInfo = playerChanges.ConnectionInfoChanged.Value;
                            Debug.Log(
                                $"ConnectionInfo for player {playerIndex} changed to {connectionInfo}");
                        }

                        if (playerChanges.LastUpdatedChanged.Changed) { }

                        //There are changes on the Player
                        if (playerChanges.ChangedData.Changed)
                        {
                            foreach (var playerChange in playerChanges.ChangedData.Value)
                            {
                                var changedValue = playerChange.Value;

                                //There are changes on some of the changes in the player list of changes

                                if (changedValue.Changed)
                                {
                                    if (changedValue.Removed)
                                    {
                                        Debug.LogWarning("This Sample does not remove Player Values currently.");
                                        continue;
                                    }

                                    var playerDataObject = changedValue.Value;
                                    ParseCustomPlayerData(localPlayer, playerChange.Key, playerDataObject.Value);
                                }
                            }
                        }
                    }
                }
            };

            m_LobbyEventCallbacks.LobbyEventConnectionStateChanged += lobbyEventConnectionState =>
            {
                Debug.Log($"Lobby ConnectionState Changed to {lobbyEventConnectionState}");
            };

            m_LobbyEventCallbacks.KickedFromLobby += () =>
            {
                Debug.Log("Left Lobby");
                Dispose();
            };
            await LobbyService.Instance.SubscribeToLobbyEventsAsync(lobbyID, m_LobbyEventCallbacks);
        }
        
        public async Task LeaveLobbyAsync()
        {
            await m_LeaveLobbyOrRemovePlayer.QueueUntilCooldown();
            if (InLobby() == false)
                return;
            string playerId = AuthenticationService.Instance.PlayerId;

            await LobbyService.Instance.RemovePlayerAsync(_currentLobby.Id, playerId);
            _currentLobby = null;
        }
        
        #region Rate Limiting

        public enum RequestType
        {
            Query = 0,
            Join,
            QuickJoin,
            Host
        }

        public bool InLobby()
        {
            if (_currentLobby == null)
            {
                Debug.LogWarning("Player not currently in a lobby.");
                return false;
            }

            return true;
        }

        //TODO not use GetRateLimit?
        public ServiceRateLimiter GetRateLimit(RequestType type)
        {
            if (type == RequestType.Join)
                return m_JoinCooldown;
            else if (type == RequestType.QuickJoin)
                return m_QuickJoinCooldown;
            else if (type == RequestType.Host)
                return m_CreateCooldown;
            return m_QueryCooldown;
        }
        
        // Rate Limits are posted here: https://docs.unity.com/lobby/rate-limits.html
        ServiceRateLimiter m_QueryCooldown = new ServiceRateLimiter(1, 1f);
        ServiceRateLimiter m_CreateCooldown = new ServiceRateLimiter(2, 6f);
        ServiceRateLimiter m_JoinCooldown = new ServiceRateLimiter(2, 6f);
        ServiceRateLimiter m_QuickJoinCooldown = new ServiceRateLimiter(1, 10f);
        ServiceRateLimiter m_GetLobbyCooldown = new ServiceRateLimiter(1, 1f);
        ServiceRateLimiter m_DeleteLobbyCooldown = new ServiceRateLimiter(2, 1f);
        ServiceRateLimiter m_UpdateLobbyCooldown = new ServiceRateLimiter(5, 5f);
        ServiceRateLimiter m_UpdatePlayerCooldown = new ServiceRateLimiter(5, 5f);
        ServiceRateLimiter m_LeaveLobbyOrRemovePlayer = new ServiceRateLimiter(5, 1);
        ServiceRateLimiter _heartBeatCooldown = new ServiceRateLimiter(5, 30);

        #endregion
        
        #region HeartBeat

        //Since the LobbyManager maintains the "connection" to the lobby, we will continue to heartbeat until host leaves.
        async Task SendHeartbeatPingAsync()
        {
            if (InLobby() == false) return;
            
            if (_heartBeatCooldown.IsCoolingDown) return;
            
            await _heartBeatCooldown.QueueUntilCooldown();

            await LobbyService.Instance.SendHeartbeatPingAsync(_currentLobby.Id);
        }

        void StartHeartBeat()
        {
#pragma warning disable 4014
            _heartBeatTask = HeartBeatLoop();
#pragma warning restore 4014
        }

        async Task HeartBeatLoop()
        {
            while (_currentLobby != null)
            {
                await SendHeartbeatPingAsync();
                await Task.Delay(8000);
            }
        }

        #endregion
        
        public void Dispose()
        {
            _currentLobby = null;
            m_LobbyEventCallbacks = new LobbyEventCallbacks();
        }
        
        //TODO Unused down
        #region UnUsed

        public async void JoinLobbyByID(string lobbyID)
        {
            try
            {
                await LobbyService.Instance.JoinLobbyByIdAsync(lobbyID);
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError(e);
            }
        }

        public async void JoinLobbyByLobbyCode(string lobbyCode)
        {
            try
            {
                await LobbyService.Instance.JoinLobbyByCodeAsync("lobbyCode");
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError(e);
            }
        }

        public void DeleteAllCreatedLobbies()
        {
            if (_heartbeatLobbyCoroutine != null)
            {
                StopCoroutine(_heartbeatLobbyCoroutine);
            }
            
            while (_createdLobbyIds.TryDequeue(out var lobbyId))
            {
                LobbyService.Instance.DeleteLobbyAsync(lobbyId);
            }
        }

        public async void DeleteLobby(string lobbyId)
        {
            try
            {
                await LobbyService.Instance.DeleteLobbyAsync("lobbyId");
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError(e);
            }
        }

        public async Task<List<string>> GetJoinedLobbies()
        {
            try
            {
                var lobbyIds = await LobbyService.Instance.GetJoinedLobbiesAsync();
                return lobbyIds;
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError(e);
            }

            return null;
        }

        public async void UpdateLobbyData()
        {
            //example
            try
            {
                UpdateLobbyOptions options = new UpdateLobbyOptions();
                options.Name = "testLobbyName";
                options.MaxPlayers = 2;
                options.IsPrivate = false;

                //Ensure you sign-in before calling Authentication Instance
                //See IAuthenticationService interface
                options.HostId = AuthenticationService.Instance.PlayerId;

                options.Data = new Dictionary<string, DataObject>()
                {
                    {
                        "ExamplePrivateData", new DataObject(
                            visibility: DataObject.VisibilityOptions.Private,
                            value: "PrivateData")
                    },
                    {
                        "ExamplePublicData", new DataObject(
                            visibility: DataObject.VisibilityOptions.Public,
                            value: "PublicData",
                            index: DataObject.IndexOptions.S1)
                    },
                };

                var lobby = await LobbyService.Instance.UpdateLobbyAsync("lobbyId", options);

                //...
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError(e);
            }
        }

        public async void UpdatePlayerData()
        {
            //example

            try
            {
                UpdatePlayerOptions options = new UpdatePlayerOptions();

                options.Data = new Dictionary<string, PlayerDataObject>()
                {
                    {
                        "existing data key", new PlayerDataObject(
                            visibility: PlayerDataObject.VisibilityOptions.Private,
                            value: "updated data value")
                    },
                    {
                        "new data key", new PlayerDataObject(
                            visibility: PlayerDataObject.VisibilityOptions.Public,
                            value: "new data value")
                    }
                };

                //Ensure you sign-in before calling Authentication Instance
                //See IAuthenticationService interface
                string playerId = AuthenticationService.Instance.PlayerId;

                var lobby = await LobbyService.Instance.UpdatePlayerAsync("lobbyId", playerId, options);

                //...
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError(e);
            }
        }

        public async void FilteringLobbySample()
        {
            try
            {
                QueryLobbiesOptions options = new QueryLobbiesOptions();
                options.Count = 25;

                // Filter for open lobbies only
                options.Filters = new List<QueryFilter>()
                {
                    new QueryFilter(
                        field: QueryFilter.FieldOptions.AvailableSlots,
                        op: QueryFilter.OpOptions.GT,
                        value: "0")
                };

                // Order by newest lobbies first
                options.Order = new List<QueryOrder>()
                {
                    new QueryOrder(
                        asc: false,
                        field: QueryOrder.FieldOptions.Created)
                };

                QueryResponse lobbies = await Lobbies.Instance.QueryLobbiesAsync(options);

                //...
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError(e);
            }

            #region Filtering with custom data
            // Lobbies have five custom string fields (S1-S5) and five custom number fields (N1-N5) that you can use for querying
            
            // Creating a custom indexed string property to be used as lobby data
            var lobbyData = new Dictionary<string, DataObject>
            {
                {
                    "GameMode", new DataObject(
                        visibility: DataObject.VisibilityOptions.Public,
                        value: "Conquest",
                        index: DataObject.IndexOptions.S1)
                }
            };
            // ... set the lobby data ...
            
            // Create query filter for the custom data that was set above
            var queryFilter = new List<QueryFilter>()
            {
                new QueryFilter(
                    field: QueryFilter.FieldOptions.S1,
                    op: QueryFilter.OpOptions.EQ,
                    value: "Conquest")
            };
            #endregion
        }

        public async Task Reconnect(string lobbyId)
        {
            await LobbyService.Instance.ReconnectToLobbyAsync(lobbyId);
        }

        public async void LeaveLobby()
        {
            try
            {
                //Ensure you sign-in before calling Authentication Instance
                //See IAuthenticationService interface
                string playerId = AuthenticationService.Instance.PlayerId;
                if (playerId == null || _lobbyID == null)
                {
                    return;
                }
                await LobbyService.Instance.RemovePlayerAsync(_lobbyID, playerId);
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError(e);
            }
        }

        public async Task GetLobby(string lobbyId, Action<Unity.Services.Lobbies.Models.Lobby> lobbyRes)
        {
            try
            {
                var lobby = await LobbyService.Instance.GetLobbyAsync(lobbyId);
                lobbyRes.Invoke(lobby);
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError(e);
                lobbyRes.Invoke(null);
            }
        }


        //copy realization from sample
        
        public Lobby CurrentLobby => _currentLobby;
        // private Lobby _currentLobby;
        private Task _heartBeatTask;

        Dictionary<string, PlayerDataObject> CreateInitialPlayerData(LocalPlayer user)
        {
            Dictionary<string, PlayerDataObject> data = new Dictionary<string, PlayerDataObject>();

            var displayNameObject = new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, user.DisplayName.Value);
            data.Add("DisplayName", displayNameObject);
            return data;
        }
        
        public async Task<Lobby> CreateLobbyAsync(int maxPlayers, bool isPrivate, LocalPlayer localUser)
        {
            if (m_CreateCooldown.IsCoolingDown)
            {
                Debug.LogWarning("Create Lobby hit the rate limit.");
                return null;
            }

            await m_CreateCooldown.QueueUntilCooldown();

            try
            {
                string uasId = AuthenticationService.Instance.PlayerId;

                CreateLobbyOptions createOptions = new CreateLobbyOptions
                {
                    IsPrivate = isPrivate,
                    Player = new Player(uasId, data: CreateInitialPlayerData(localUser))
                };
                
                string lobbyName = await GenerateLobbyName();
                _currentLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createOptions);
                Debug.Log($"[Lobby] Lobby created {_currentLobby.Name}");
                StartHeartBeat();

                return _currentLobby;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Lobby Create failed:\n{ex}");
                return null;
            }
        }

        ///<summary>
        /// sample show differents for lobby filter
        /// List<QueryFilter> LobbyColorToFilters(LobbyColor limitToColor)
        /// {
        ///     List<QueryFilter> filters = new List<QueryFilter>();
        ///     if (limitToColor == LobbyColor.Orange)
        ///         filters.Add(new QueryFilter(QueryFilter.FieldOptions.N1, ((int)LobbyColor.Orange).ToString(),
        ///             QueryFilter.OpOptions.EQ));
        ///     else if (limitToColor == LobbyColor.Green)
        ///         filters.Add(new QueryFilter(QueryFilter.FieldOptions.N1, ((int)LobbyColor.Green).ToString(),
        ///             QueryFilter.OpOptions.EQ));
        ///     else if (limitToColor == LobbyColor.Blue)
        ///         filters.Add(new QueryFilter(QueryFilter.FieldOptions.N1, ((int)LobbyColor.Blue).ToString(),
        ///             QueryFilter.OpOptions.EQ));
        ///     return filters;
        /// }
        /// </summary>
        
        // List<QueryFilter> LobbyColorToFilters(LobbyColor limitToColor)
        // {
        //     List<QueryFilter> filters = new List<QueryFilter>();
        //     if (limitToColor == LobbyColor.Orange)
        //         filters.Add(new QueryFilter(QueryFilter.FieldOptions.N1, ((int)LobbyColor.Orange).ToString(),
        //             QueryFilter.OpOptions.EQ));
        //     else if (limitToColor == LobbyColor.Green)
        //         filters.Add(new QueryFilter(QueryFilter.FieldOptions.N1, ((int)LobbyColor.Green).ToString(),
        //             QueryFilter.OpOptions.EQ));
        //     else if (limitToColor == LobbyColor.Blue)
        //         filters.Add(new QueryFilter(QueryFilter.FieldOptions.N1, ((int)LobbyColor.Blue).ToString(),
        //             QueryFilter.OpOptions.EQ));
        //     return filters;
        // }

        #endregion
    }
}