using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace NetCodeTT.Lobby
{
    public class LobbyManager : MonoBehaviour, ILobby
    {
        private IEnumerator _heartbeatLobbyCoroutine;
        private ConcurrentQueue<string> _createdLobbyIds = new ConcurrentQueue<string>();
        private string _lobbyID;

        public async void CreateLobby(Action<string> result)
        {
            string lobbyName = await GenerateLobbyName();
            int maxPlayers = 2;
            CreateLobbyOptions options = new CreateLobbyOptions();
            options.IsPrivate = false;
            
            Unity.Services.Lobbies.Models.Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);

            _createdLobbyIds.Enqueue(lobby.Id);
            _lobbyID = lobby.Id;
            _heartbeatLobbyCoroutine = HeartbeatLobbyCoroutine(lobby.Id, 15);
            
            StartCoroutine(_heartbeatLobbyCoroutine);
            result.Invoke($"Lobby created {lobbyName}");
            Debug.Log($"LobbyName {lobbyName} lobby.Id {lobby.Id}");

            /* Sample
            string lobbyName = "new lobby";
            int maxPlayers = 2;
            CreateLobbyOptions options = new CreateLobbyOptions();
            options.IsPrivate = false;

            //Create a lobby with standard, non-indexed data
            options.Data = new Dictionary<string, DataObject>()
            {
                {
                    "ExamplePublicLobbyData", new DataObject(
                        visibility: DataObject.VisibilityOptions.Public, // Visible publicly.
                        value: "ExamplePublicLobbyData")
                },
            };

            // Create a lobby with indexed string data
            options.Data = new Dictionary<string, DataObject>()
            {
                {
                    "GameMode", new DataObject(
                        visibility: DataObject.VisibilityOptions.Public, // Visible publicly.
                        value: "Conquest",
                        index: DataObject.IndexOptions.S1)
                },
            };

            // Create a lobby with indexed numeric data
            options.Data = new Dictionary<string, DataObject>()
            {
                {
                    "MinimumSkillLevel", new DataObject(
                        visibility: DataObject.VisibilityOptions.Public, // Visible publicly.
                        value: "25",
                        index: DataObject.IndexOptions.N1)
                },
            };

            // Create a lobby with player data for the host
            options.Player = new Player(
                id: AuthenticationService.Instance.PlayerId,
                data: new Dictionary<string, PlayerDataObject>()
                {
                    {
                        "ExampleMemberPlayerData", new PlayerDataObject(
                            visibility: PlayerDataObject.VisibilityOptions
                                .Member, // Visible only to members of the lobby.
                            value: "ExampleMemberPlayerData")
                    }
                });

            Unity.Services.Lobbies.Models.Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);

            _createdLobbyIds.Enqueue(lobby.Id);
            _heartbeatLobbyCoroutine = HeartbeatLobbyCoroutine(lobby.Id, 15);
            // Heartbeat the lobby every 15 seconds.
            StartCoroutine(_heartbeatLobbyCoroutine);
            */
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
                            if (result != String.Empty) 
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

        public async void QuickJoin(Action<string> result)
        {
            try
            {
                QuickJoinLobbyOptions options = new QuickJoinLobbyOptions();

                // options.Filter = new List<QueryFilter>()
                // {
                //     new QueryFilter(
                //         field: QueryFilter.FieldOptions.MaxPlayers,
                //         op: QueryFilter.OpOptions.GE,
                //         value: "10")
                // };

                var lobby = await LobbyService.Instance.QuickJoinLobbyAsync(options);
                _lobbyID = lobby.Id;
                result.Invoke($"Joined into lobby {lobby.Name}");
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError(e);
                result.Invoke($"Can't join into lobby :(");
                CreateLobby(res =>
                {
                    result.Invoke($"{res}");
                });
            }
            
            /* Sample
            try
            {
                // Quick-join a random lobby with a maximum capacity of 10 or more players.
                QuickJoinLobbyOptions options = new QuickJoinLobbyOptions();

                options.Filter = new List<QueryFilter>()
                {
                    new QueryFilter(
                        field: QueryFilter.FieldOptions.MaxPlayers,
                        op: QueryFilter.OpOptions.GE,
                        value: "10")
                };

                var lobby = await LobbyService.Instance.QuickJoinLobbyAsync(options);

                // ...
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError(e);
            }
            */
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
                await LobbyService.Instance.RemovePlayerAsync(_lobbyID, playerId);
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError(e);
            }
        }

        public async Task GetLobby(string lobbyId, Action<string> result)
        {
            try
            {
                var lobby = await LobbyService.Instance.GetLobbyAsync(lobbyId);
                result.Invoke(lobby.Id);
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError(e);
                result.Invoke(String.Empty);
            }
        }


        private IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float waitTimeSeconds)
        {
            var delay = new WaitForSecondsRealtime(waitTimeSeconds);

            while (true)
            {
                LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
                yield return delay;
            }
        }
    }
}