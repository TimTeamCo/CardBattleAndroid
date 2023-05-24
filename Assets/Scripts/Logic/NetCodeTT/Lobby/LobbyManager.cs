using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NetCodeTT.Lobbys
{
    using Unity.Services.Lobbies.Models;
    using Unity.Services.Lobbies;
    
    public class LobbyManager : MonoBehaviour, ILobby
    {
        public string LobbyID => _currentLobby?.Id;
        public string LobbyCode => _currentLobby?.LobbyCode;
        
        private Lobby _currentLobby;
        private bool _isHost;
        private IEnumerator _heartbeatLobbyCoroutine;
        private LobbyEventCallbacks m_LobbyEventCallbacks = new LobbyEventCallbacks();
        private const string key_RelayCode = nameof(LocalLobby.RelayCode);
        private const string key_LobbyState = nameof(LocalLobby.LocalLobbyState);
        const string key_Pet = nameof(LocalPlayer.Pet);
        const string key_Userstatus = nameof(LocalPlayer.UserStatus);
        const string key_Displayname = nameof(LocalPlayer.PlayerName);
        private const int MaxPlayers = 2;
        private Coroutine _refreshLobbyCoroutine;

        public async Task<Lobby> QuickJoin(LocalPlayer localUser)
        {
            if (m_QuickJoinCooldown.IsCoolingDown)
            {
                Debug.LogWarning("Quick Join Lobby hit the rate limit.");
                return null;
            }

            await m_QuickJoinCooldown.QueueUntilCooldown();

            try
            {
                QuickJoinLobbyOptions joinRequest = new QuickJoinLobbyOptions
                {
                    Player = GetPlayer(localUser),
                };

                _currentLobby = await LobbyService.Instance.QuickJoinLobbyAsync(joinRequest);
                _isHost = false;
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError(e);
                await CreateLobby(localUser);
                _isHost = true;
            }

            PrintPlayers();
            return _currentLobby;
        }

        public async Task<bool> CreateLobby(Dictionary<string, string> userData, Dictionary<string, string> lobbyData)
        {
            //random name 10 numbers xxxxxxxxxx
            StringBuilder stringBuilder = new StringBuilder();
            while (stringBuilder.Length < 10)
            {
                int num = Random.Range(0, 10);
                stringBuilder.Append(num);
            }
            
            //lobby options
            Dictionary<string, PlayerDataObject> playerData = SerializePlayerData(userData);
            Player player = new Player(AuthenticationService.Instance.PlayerId, null, playerData);
            CreateLobbyOptions lobbyOptions = new CreateLobbyOptions
            {
                Data = SerializeLobbyData(lobbyData),
                IsPrivate = false,
                Player = player,
            };

            try
            {
                _currentLobby = await LobbyService.Instance.CreateLobbyAsync(stringBuilder.ToString(), MaxPlayers, lobbyOptions);
                _isHost = true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
            
            Debug.Log($"Lobby created with lobby code {_currentLobby.LobbyCode}");
            StartHeartBeat();
            _refreshLobbyCoroutine = StartCoroutine(RefreshLobbyCoroutine(_currentLobby.Id, 1));
            return true;
        }

        public async Task<bool> JoinLobby(string lobbyCode, Dictionary<string, string> data)
        {
            JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions
            {
                Player = new Player(AuthenticationService.Instance.PlayerId, null, SerializePlayerData(data))
            };

            try
            {
                _currentLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode, options);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
            
            _refreshLobbyCoroutine = StartCoroutine(RefreshLobbyCoroutine(_currentLobby.Id, 1));
            return true;
        }

        public async Task<bool> UpdatePlayerData(string playerId, Dictionary<string,string> data, string allocationId = default, string conectionData = default)
        {
            Dictionary<string, PlayerDataObject> playerData = SerializePlayerData(data);

            var options = new UpdatePlayerOptions
            {
                Data = playerData,
                AllocationId = allocationId,
                ConnectionInfo = conectionData
            };
            
            try
            {
                _currentLobby = await LobbyService.Instance.UpdatePlayerAsync(_currentLobby.Id, playerId, options);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }

            LobbyEvents.OnLobbyUpdated(_currentLobby);
            return true;
        }
        
        public async Task<bool> UpdateLobbyData(Dictionary<string,string> data)
        {
            Dictionary<string, DataObject> lobbyData = SerializeLobbyData(data);

            var options = new UpdateLobbyOptions
            {
                Data = lobbyData
            };
            
            try
            {
                _currentLobby = await LobbyService.Instance.UpdateLobbyAsync(_currentLobby.Id, options);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }

            LobbyEvents.OnLobbyUpdated(_currentLobby);
            return true;
        }

        public string GetHostId()
        {
            return _currentLobby.HostId;
        }
        
        
        private Dictionary<string, DataObject> SerializeLobbyData(Dictionary<string,string> data)
        {
            Dictionary<string, DataObject> lobbyData = new Dictionary<string, DataObject>();
            foreach (var (key, value) in data)
            {
                lobbyData.Add(key, new DataObject(DataObject.VisibilityOptions.Member, value));
            }

            return lobbyData;
        }
        
        private Dictionary<string,PlayerDataObject> SerializePlayerData(Dictionary<string,string> data)
        {
            Dictionary<string, PlayerDataObject> playerData = new Dictionary<string, PlayerDataObject>();
            foreach (var (key, value) in data)
            {
                playerData.Add(key, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, value));
            }

            return playerData;
        }

        public List<Dictionary<string, PlayerDataObject>> GetPlayersData()
        {
            List<Dictionary<string, PlayerDataObject>> data = new List<Dictionary<string, PlayerDataObject>>();

            foreach (var player in _currentLobby.Players)
            {
                data.Add(player.Data);
            }

            return data;
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
                CreateLobbyOptions createOptions = new CreateLobbyOptions
                {
                    IsPrivate = false,
                    Player = GetPlayer(localUser),
                };
                
                string lobbyName = await GenerateLobbyName();
                int maxPlayers = 2;

                try
                {
                    _currentLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createOptions);
                    StartHeartBeat();
                }
                catch (LobbyServiceException e)
                {
                    Debug.LogError(e);
                }
            }
            catch (LobbyServiceException ex)
            {
                Debug.LogError($"Lobby Create failed:\n{ex}");
                return null;
            }
            
            return _currentLobby;
        }

        public bool IsHostUser()
        {
            return _isHost;
        }

        public void PrintPlayers(Lobby loby = null)
        {
            loby ??= _currentLobby;
            StringBuilder playersInfo = new StringBuilder();
            playersInfo.AppendLine($"Players in {loby.Name}");
            foreach (var player in loby.Players)
            {
                playersInfo.AppendLine($"Player ID - {player.Id} - PlayerName -  {player.Data["PlayerName"].Value}");
            }
            Debug.Log(playersInfo);
        }
        
        private Player GetPlayer(LocalPlayer localUser)
        {
            string uasId = AuthenticationService.Instance.PlayerId;
            return new Player(id: uasId,
                data: CreateInitialPlayerData(localUser));
        }

        private async Task<string> GenerateLobbyName()
        {
            var lobbyName = String.Empty;
            try
            {
                QueryLobbiesOptions options = new QueryLobbiesOptions();

                QueryResponse lobbies = await Lobbies.Instance.QueryLobbiesAsync(options);

                var list = lobbies.Results;
                lobbyName = list.Count > 0 ? $"Lobby{list.Count}" : $"Lobby0";
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
                {
                    localLobby.AvailableSlots.Value = changes.AvailableSlots.Value;
                    if (localLobby.AvailableSlots.Value == 0)
                    {
                    }
                }

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
                            localLobby.LocalLobbyState.Value = (LobbyState) int.Parse(playerDataObject.Value);

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
                        var isHost = localLobby.HostID.Value == id;

                        var newPlayer = new LocalPlayer(id, isHost);

                        foreach (var dataEntry in joinedPlayer.Data)
                        {
                            var dataObject = dataEntry.Value;
                            ParseCustomPlayerData(newPlayer, dataEntry.Key, dataObject.Value);
                        }

                        localLobby.AddPlayer(newPlayer);
                        Debug.Log($"Player join {newPlayer.PlayerName.Value}");
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
                        // localLobby.UpdatePlayer(localPlayer);
                        var playerChanges = lobbyPlayerChanges.Value;
                        if (playerChanges.ConnectionInfoChanged.Changed)
                        {
                            var connectionInfo = playerChanges.ConnectionInfoChanged.Value;
                            Debug.Log($"ConnectionInfo for player {playerIndex} changed to {connectionInfo}");
                        }

                        if (playerChanges.LastUpdatedChanged.Changed)
                        {
                        }

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

        void ParseCustomPlayerData(LocalPlayer player, string dataKey, string playerDataValue)
        {
            switch (dataKey)
            {
                case key_Pet:
                    player.Pet.Value = (PetType) int.Parse(playerDataValue);
                    break;
                case key_Userstatus:
                    player.UserStatus.Value = (PlayerStatus) int.Parse(playerDataValue);
                    break;
                case key_Displayname:
                    player.PlayerName.Value = playerDataValue;
                    break;
            }
        }

        public async Task UpdatePlayerDataAsync(Dictionary<string, string> data)
        {
            if (InLobby() == false)
                return;

            string playerId = AuthenticationService.Instance.PlayerId;
            Dictionary<string, PlayerDataObject> dataCurr = new Dictionary<string, PlayerDataObject>();
            foreach (var dataNew in data)
            {
                PlayerDataObject dataObj = new PlayerDataObject(visibility: PlayerDataObject.VisibilityOptions.Member,
                    value: dataNew.Value);
                if (dataCurr.ContainsKey(dataNew.Key))
                    dataCurr[dataNew.Key] = dataObj;
                else
                    dataCurr.Add(dataNew.Key, dataObj);
            }

            if (m_UpdatePlayerCooldown.TaskQueued)
                return;
            await m_UpdatePlayerCooldown.QueueUntilCooldown();

            UpdatePlayerOptions updateOptions = new UpdatePlayerOptions
            {
                Data = dataCurr,
                AllocationId = null,
                ConnectionInfo = null
            };
            
            _currentLobby = await LobbyService.Instance.UpdatePlayerAsync(_currentLobby.Id, playerId, updateOptions);
        }
        
        public async Task UpdateLobbyDataAsync(Dictionary<string, string> data)
        {
            if (InLobby() == false)
                return;

            Dictionary<string, DataObject> dataCurr = _currentLobby.Data ?? new Dictionary<string, DataObject>();

            var shouldLock = false;
            foreach (var dataNew in data)
            {
                /*
                // Special case: We want to be able to filter on our color data, so we need to supply an arbitrary index to retrieve later. Uses N# for numerics, instead of S# for strings.
                DataObject.IndexOptions index = dataNew.Key == "LocalLobbyColor" ? DataObject.IndexOptions.N1 : 0;
                DataObject dataObj = new DataObject(DataObject.VisibilityOptions.Public, dataNew.Value,
                    index); // Public so that when we request the list of lobbies, we can get info about them for filtering.
                if (dataCurr.ContainsKey(dataNew.Key))
                    dataCurr[dataNew.Key] = dataObj;
                else
                    dataCurr.Add(dataNew.Key, dataObj);
                */

                //Special Use: Get the state of the Local lobby so we can lock it from appearing in queries if it's not in the "Lobby" LocalLobbyState
                if (dataNew.Key == "LocalLobbyState")
                {
                    Enum.TryParse(dataNew.Value, out LobbyState lobbyState);
                    shouldLock = lobbyState != LobbyState.Lobby;
                }
            }

            //We can still update the latest data to send to the service, but we will not send multiple UpdateLobbySyncCalls
            if (m_UpdateLobbyCooldown.TaskQueued)
                return;
            await m_UpdateLobbyCooldown.QueueUntilCooldown();

            UpdateLobbyOptions updateOptions = new UpdateLobbyOptions {Data = dataCurr, IsLocked = shouldLock};
            _currentLobby = await LobbyService.Instance.UpdateLobbyAsync(_currentLobby.Id, updateOptions);
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

        // Rate Limits are posted here: https://docs.unity.com/lobby/rate-limits.html
        ServiceRateLimiter m_CreateCooldown = new ServiceRateLimiter(2, 6f);
        ServiceRateLimiter m_QuickJoinCooldown = new ServiceRateLimiter(1, 10f);
        ServiceRateLimiter m_UpdateLobbyCooldown = new ServiceRateLimiter(5, 5f);
        ServiceRateLimiter m_UpdatePlayerCooldown = new ServiceRateLimiter(5, 5f);
        ServiceRateLimiter m_LeaveLobbyOrRemovePlayer = new ServiceRateLimiter(5, 1);
        ServiceRateLimiter _heartBeatCooldown = new ServiceRateLimiter(5, 30);

        #endregion
        
        #region HeartBeat

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

        //Since the LobbyManager maintains the "connection" to the lobby, we will continue to heartbeat until host leaves.
        async Task SendHeartbeatPingAsync()
        {
            if (InLobby() == false) return;
            
            if (_heartBeatCooldown.IsCoolingDown) return;
            
            await _heartBeatCooldown.QueueUntilCooldown();

            await LobbyService.Instance.SendHeartbeatPingAsync(_currentLobby.Id);
        }

        #endregion

        #region RefreshLobby

        private IEnumerator RefreshLobbyCoroutine(string lobbyId, float waitTimeSeconds)
        {
            while (true)
            {
                Task<Lobby> task = LobbyService.Instance.GetLobbyAsync(lobbyId);
                yield return new WaitUntil(() => task.IsCompleted);
                Lobby newLobby = task.Result;
                if (newLobby.LastUpdated > _currentLobby.LastUpdated)
                {
                    _currentLobby = newLobby;
                    LobbyEvents.OnLobbyUpdated?.Invoke(_currentLobby);
                }

                yield return new WaitForSecondsRealtime(waitTimeSeconds);
            }
        }

        #endregion
        
        public async void LeaveLobby()
        {
            try
            {
                string playerId = AuthenticationService.Instance.PlayerId;
                if (playerId == null || LobbyID == null)
                {
                    return;
                }

                _isHost = false;
                
                //change Lobby Host previous than exit
                await MigrateLobbyHost();
                await LobbyService.Instance.RemovePlayerAsync(LobbyID, playerId);
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError(e);
            }
        }

        private async Task MigrateLobbyHost()
        {
            try
            {
                if (_currentLobby.Players.Count < 2)
                {
                    return;
                }
                
                _currentLobby = await Lobbies.Instance.UpdateLobbyAsync(_currentLobby.Id, new UpdateLobbyOptions
                {
                    HostId = _currentLobby.Players[1].Id,
                });
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError(e);
                throw;
            }
        }
        
        Dictionary<string, PlayerDataObject> CreateInitialPlayerData(LocalPlayer user)
        {
            Dictionary<string, PlayerDataObject> data = new Dictionary<string, PlayerDataObject>();

            var displayNameObject =
                new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, user.PlayerName.Value);
            data.Add("PlayerName", displayNameObject);
            return data;
        }

        public void Dispose()
        {
            _currentLobby = null;
            m_LobbyEventCallbacks = new LobbyEventCallbacks();
        }

        //TODO Unused down
        #region UnUsed
        
        private ConcurrentQueue<string> _createdLobbyIds = new ConcurrentQueue<string>();
        ServiceRateLimiter m_QueryCooldown = new ServiceRateLimiter(1, 1f);
        ServiceRateLimiter m_JoinCooldown = new ServiceRateLimiter(2, 6f);
        ServiceRateLimiter m_GetLobbyCooldown = new ServiceRateLimiter(1, 2f);
        private ServiceRateLimiter m_DeleteLobbyCooldown = new ServiceRateLimiter(2, 1f);

        public async Task<Lobby> GetLobbyAsync(string lobbyId = null)
        {
            if (!InLobby())
                return null;
            await m_GetLobbyCooldown.QueueUntilCooldown();
            if (_currentLobby == null)
                return null;
            lobbyId ??= _currentLobby.Id;
            return _currentLobby = await LobbyService.Instance.GetLobbyAsync(lobbyId);
        }


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


        //copy realization from sample

        public Lobby CurrentLobby => _currentLobby;

        // private Lobby _currentLobby;
        private Task _heartBeatTask;

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

        public void OnApplicationQuit()
        {
            if (_currentLobby != null && _currentLobby.HostId == AuthenticationService.Instance.PlayerId)
            {
                LobbyService.Instance.DeleteLobbyAsync(_currentLobby.Id);
            }
        }
    }
}