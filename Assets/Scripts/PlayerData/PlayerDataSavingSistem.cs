using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerDataSavingSistem
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

        Debug.Log("Nickname doesnt set");
        return null;
    }
}
