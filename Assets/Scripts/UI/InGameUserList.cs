using System.Collections.Generic;
using TTGame;
using TTLobbyLogic;
using UnityEngine;

namespace TTUI
{
    // Contains the InLobbyUserUI instances while showing the UI for a lobby.
    [RequireComponent(typeof(LocalLobbyObserver))]
    public class InGameUserList : ObserverPanel<LocalLobby>
    {
        [SerializeField] List<InGameUserUI> m_UserUIObjects = new List<InGameUserUI>();
        // Just for keeping track more easily of which users are already displayed.
        List<string> m_CurrentUsers = new List<string>();
        
        // When the observed data updates, we need to detect changes to the list of players.
        public override void ObservedUpdated(LocalLobby observed)
        {
            // We might remove users if they aren't in the new data, so iterate backwards.
            for (int id = m_CurrentUsers.Count - 1; id >= 0; id--)
            {
                string userId = m_CurrentUsers[id];
                if (observed.LobbyUsers.ContainsKey(userId) == false)
                {
                    foreach (var ui in m_UserUIObjects)
                    {
                        if (ui.UserId == userId)
                        {
                            ui.OnUserLeft();
                            OnUserLeft(userId);
                        }
                    }
                }
            }

            // If there are new players, we need to hook them into the UI.
            foreach (var lobbyUserKvp in observed.LobbyUsers)
            {
                if (m_CurrentUsers.Contains(lobbyUserKvp.Key))
                {
                    continue;
                }
                m_CurrentUsers.Add(lobbyUserKvp.Key);

                foreach (var pcu in m_UserUIObjects)
                {
                    if (pcu.IsAssigned)
                    {
                        continue;
                    }
                    pcu.SetUser(lobbyUserKvp.Value);
                    break;
                }
            }
        }

        void OnUserLeft(string userID)
        {
            if (!m_CurrentUsers.Contains(userID))
                return;
            m_CurrentUsers.Remove(userID);
        }
    }
}