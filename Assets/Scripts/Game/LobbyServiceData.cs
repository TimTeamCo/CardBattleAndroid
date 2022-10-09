using System;
using System.Collections.Generic;
using TTLobbyLogic;

namespace TTGame
{
    /// Holds data related to the Lobby service itself - The latest retrieved lobby list, the state of retrieval.
    [Serializable]
    public class LobbyServiceData : Observed<LobbyServiceData>
    {
        LobbyQueryState m_CurrentState = LobbyQueryState.Empty;
        
        public LobbyQueryState State
        {
            get { return m_CurrentState; }
            set
            {
                m_CurrentState = value;
                OnChanged(this);
            }
        }
        
        Dictionary<string, LocalLobby> m_currentLobbies = new Dictionary<string, LocalLobby>();
        
        /// Maps from a lobby's ID to the local representation of it. This allows us to remember which remote lobbies are which LocalLobbies.
        /// Will only trigger if the dictionary is set wholesale. Changes in the size or contents will not trigger OnChanged.
        public Dictionary<string, LocalLobby> CurrentLobbies
        {
            get { return m_currentLobbies; }
            set
            {
                m_currentLobbies = value;
                OnChanged(this);
            }
        }
        
        public override void CopyObserved(LobbyServiceData oldObserved)
        {
            m_currentLobbies = oldObserved.CurrentLobbies;
            OnChanged(this);
        }
    }
}