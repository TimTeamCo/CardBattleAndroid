using System.Collections.Generic;
using Unity.Services.Authentication;
using UnityEngine;

namespace NetCodeTT.Lobbys
{
    using Unity.Services.Lobbies.Models;
    
    /// <summary>
    /// QueryToLocalList the lobby resulting from a request into a LocalLobby for use in the game logic.
    /// </summary>
    public static class LobbyConverters
    {
        const string key_RelayCode = nameof(LocalLobby.RelayCode);
        const string key_LobbyState = nameof(LocalLobby.LocalLobbyState);
        // const string key_LobbyColor = nameof(LocalLobby.LocalLobbyColor);
        const string key_LastEdit = nameof(LocalLobby.LastUpdated);

        const string key_Playername = nameof(LocalPlayer.PlayerName);
        const string key_Userstatus = nameof(LocalPlayer.UserStatus);
        const string key_Pet = nameof(LocalPlayer.Pet);

        public static Dictionary<string, string> LocalToRemoteLobbyData(LocalLobby lobby)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(key_RelayCode, lobby.RelayCode.Value);
            data.Add(key_LobbyState, ((int)lobby.LocalLobbyState.Value).ToString());
            data.Add(key_LastEdit, lobby.LastUpdated.Value.ToString());

            return data;
        }

        public static Dictionary<string, string> LocalToRemoteUserData(LocalPlayer user)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            if (user == null || string.IsNullOrEmpty(user.ID.Value))
                return data;
            data.Add(key_Playername, user.PlayerName.Value);
            data.Add(key_Userstatus, ((int)user.UserStatus.Value).ToString());
            data.Add(key_Pet, ((int)user.Pet.Value).ToString());
            return data;
        }

        /// <summary>
        /// Create a new LocalLobby from the content of a retrieved lobby. Its data can be copied into an existing LocalLobby for use.
        /// </summary>
        public static void RemoteToLocal(Lobby remoteLobby, LocalLobby localLobby)
        {
            if (remoteLobby == null)
            {
                Debug.LogError("Remote lobby is null, cannot convert.");
                return;
            }

            if (localLobby == null)
            {
                Debug.LogError("Local Lobby is null, cannot convert");
                return;
            }

            localLobby.LobbyID.Value = remoteLobby.Id;
            localLobby.HostID.Value = remoteLobby.HostId;
            localLobby.LobbyName.Value = remoteLobby.Name;
            localLobby.LobbyCode.Value = remoteLobby.LobbyCode;
            localLobby.Private.Value = remoteLobby.IsPrivate;
            localLobby.AvailableSlots.Value = remoteLobby.AvailableSlots;
            localLobby.MaxPlayerCount.Value = remoteLobby.MaxPlayers;
            localLobby.LastUpdated.Value = remoteLobby.LastUpdated.ToFileTimeUtc();

            //Custom Lobby Data Conversions
            localLobby.RelayCode.Value = remoteLobby.Data?.ContainsKey(key_RelayCode) == true
                ? remoteLobby.Data[key_RelayCode].Value
                : localLobby.RelayCode.Value;
            localLobby.LocalLobbyState.Value = remoteLobby.Data?.ContainsKey(key_LobbyState) == true
                ? (LobbyState)int.Parse(remoteLobby.Data[key_LobbyState].Value)
                : LobbyState.Lobby;
            // localLobby.LocalLobbyColor.Value = remoteLobby.Data?.ContainsKey(key_LobbyColor) == true
            //     ? (LobbyColor)int.Parse(remoteLobby.Data[key_LobbyColor].Value)
            //     : LobbyColor.None;

            //Custom User Data Conversions
            List<string> remotePlayerIDs = new List<string>();
            int index = 0;
            foreach (var player in remoteLobby.Players)
            {
                var id = player.Id;
                remotePlayerIDs.Add(id);
                var isHost = remoteLobby.HostId.Equals(player.Id);
                var displayName = player.Data?.ContainsKey(key_Playername) == true
                    ? player.Data[key_Playername].Value
                    : default;
                var pet = player.Data?.ContainsKey(key_Pet) == true
                    ? (PetType)int.Parse(player.Data[key_Pet].Value)
                    : PetType.Cat1;
                var userStatus = player.Data?.ContainsKey(key_Userstatus) == true
                    ? (PlayerStatus)int.Parse(player.Data[key_Userstatus].Value)
                    : PlayerStatus.Lobby;

                LocalPlayer user = localLobby.GetLocalPlayer(index);

                if (user == null)
                {
                    user = new LocalPlayer(id, isHost, displayName, pet, userStatus);
                    localLobby.AddPlayer(user);
                }
                else
                {
                    user.ID.Value = id;
                    user.IsHost.Value = isHost;
                    user.PlayerName.Value = displayName;
                    user.Pet.Value = pet;
                    user.UserStatus.Value = userStatus;
                }

                index++;
            }
        }

        /// <summary>
        /// Create a list of new LocalLobbies from the result of a lobby list query.
        /// </summary>
        public static List<LocalLobby> QueryToLocalList(QueryResponse response)
        {
            List<LocalLobby> retLst = new List<LocalLobby>();
            foreach (var lobby in response.Results)
                retLst.Add(RemoteToNewLocal(lobby));
            return retLst;
        }

        //This might be heavy handed,
        static LocalLobby RemoteToNewLocal(Lobby lobby)
        {
            LocalLobby data = new LocalLobby();
            RemoteToLocal(lobby, data);
            return data;
        }
    }
}