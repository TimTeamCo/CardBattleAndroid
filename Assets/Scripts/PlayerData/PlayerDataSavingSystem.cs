using System;
using UnityEngine;

public static class PlayerDataSavingSystem
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