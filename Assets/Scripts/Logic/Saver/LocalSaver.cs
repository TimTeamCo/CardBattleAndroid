using System;
using UnityEngine;

namespace Saver
{
    public static class LocalSaver
    {
        public static void SetPlayerNickname(string nickname)
        {
            PlayerPrefs.SetString("Nickname", nickname);
        }

        public static string GetPlayerNickname()
        {
            if (PlayerPrefs.HasKey("Nickname"))
            {
                return PlayerPrefs.GetString("Nickname");
            }

            Debug.LogError("Nickname doesnt set");
            return String.Empty;
        }
    }
}