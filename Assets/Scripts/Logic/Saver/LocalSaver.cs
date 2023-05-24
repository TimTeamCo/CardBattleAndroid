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
        
        public static void SetPlayerPet(PetType pet)
        {
            PlayerPrefs.SetString("Pet", pet.ToString());
        }

        public static string GetPlayerPet()
        {
            if (PlayerPrefs.HasKey("Pet"))
            {
                return PlayerPrefs.GetString("Pet");
            }

            Debug.LogError("Pet doesnt set");
            return String.Empty;
        }
    }
}