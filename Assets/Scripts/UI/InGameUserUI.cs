using TMPro;
using TTGame;
using UnityEngine;
using UnityEngine.UI;

namespace TTUI
{
    // When inside a game, this will show information about a player, whether local or remote.
    [RequireComponent(typeof(LobbyUserObserver))]
    public class InGameUserUI : ObserverPanel<LobbyUser>
    {
        [SerializeField]
        TMP_Text m_DisplayNameText;

        // [SerializeField] Image m_HostIcon;
        //
        // [SerializeField] Image m_EmoteImage;
        //
        // [SerializeField] Sprite[] m_EmoteIcons;

        public bool IsAssigned => UserId != null;

        public string UserId { get; private set; }
        
        private LobbyUserObserver m_observer;

        public void SetUser(LobbyUser myLobbyUser)
        {
            Show();
            if (m_observer == null)
            {
                m_observer = GetComponent<LobbyUserObserver>();
            }
            m_observer.BeginObserving(myLobbyUser);
            UserId = myLobbyUser.ID;
        }

        public void OnUserLeft()
        {
            Debug.Log($"UserLeft {m_DisplayNameText}");
            UserId = null;
            Hide();
            m_observer.EndObserving();
        }

        public override void ObservedUpdated(LobbyUser observed)
        {
            m_DisplayNameText.SetText(observed.DisplayName);
            m_DisplayNameText.color = SetColor(observed.IsHost);
            // m_EmoteImage.sprite = EmoteIcon(observed.Emote);
            // m_HostIcon.enabled = observed.IsHost;
        }

        private Color SetColor(bool isHost)
        {
            if (isHost)
            {
                return Color.red;
            }
            else
            {
                return Color.green;
            }
        }

        /// <summary>
        /// EmoteType to Icon Sprite
        /// m_EmoteIcon[0] = Smile
        /// m_EmoteIcon[1] = Frown
        /// m_EmoteIcon[2] = UnAmused
        /// m_EmoteIcon[3] = Tongue
        /// </summary>
        // Sprite EmoteIcon(EmoteType type)
        // {
        //     switch (type)
        //     {
        //         case EmoteType.None:
        //             m_EmoteImage.color = Color.clear;
        //             return null;
        //         case EmoteType.Smile:
        //             m_EmoteImage.color = Color.white;
        //             return m_EmoteIcons[0];
        //         case EmoteType.Frown:
        //             m_EmoteImage.color = Color.white;
        //             return m_EmoteIcons[1];
        //         case EmoteType.Unamused:
        //             m_EmoteImage.color = Color.white;
        //             return m_EmoteIcons[2];
        //         case EmoteType.Tongue:
        //             m_EmoteImage.color = Color.white;
        //             return m_EmoteIcons[3];
        //         default:
        //             return null;
        //     }
        // }

        string SetStatusFancy(UserStatus status)
        {
            switch (status)
            {
                case UserStatus.Lobby:
                    return "<color=#56B4E9>In Lobby</color>"; // Light Blue
                case UserStatus.Ready:
                    return "<color=#009E73>Ready</color>"; // Light Mint
                case UserStatus.Connecting:
                    return "<color=#F0E442>Connecting...</color>"; // Bright Yellow
                case UserStatus.InGame:
                    return "<color=#005500>In Game</color>"; // Green
                default:
                    return "";
            }
        }
    }
}