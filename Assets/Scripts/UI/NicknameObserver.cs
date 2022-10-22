using TMPro;
using TTGame;
using UnityEngine;

namespace TTUI
{
    // Displays the player's name.
    public class NicknameObserver : ObserverPanel<LobbyUser>
    {
        [SerializeField] private TMP_Text m_TextField;
        
        public override void ObservedUpdated(LobbyUser observed)
        {
            m_TextField.SetText(observed.DisplayName);
        }
    }
}

