using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace NetCode.Lobby
{
    public class LobbyManager : MonoBehaviour, ILobby
    {
        private IEnumerator _heartbeatLobbyCoroutine;
        private ConcurrentQueue<string> _createdLobbyIds;

        public async void CreateLobby()
        {
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

        public void DebugSMTH()
        {
            Debug.Log($"BlaBla");
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