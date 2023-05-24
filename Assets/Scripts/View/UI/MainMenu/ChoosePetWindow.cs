using Saver;
using UnityEngine;

public class ChoosePetWindow : MonoBehaviour
{
    private PetType _userPet = PetType.Cat1;
    
    public async void SetPet(int petType)
    {
        switch (petType)
        {
            case 0:
                _userPet = PetType.Cat1;
                ApplicationController.Instance.GameManager.SetLocalPet(PetType.Cat1);
                break;
            case 1:
                _userPet = PetType.Cat2;
                ApplicationController.Instance.GameManager.SetLocalPet(PetType.Cat2);
                break;
            case 2:
                _userPet = PetType.Dog1;
                ApplicationController.Instance.GameManager.SetLocalPet(PetType.Dog1);
                break;
            case 3:
                _userPet = PetType.Dog2;
                ApplicationController.Instance.GameManager.SetLocalPet(PetType.Dog2);
                break;
            case 4:
                _userPet = PetType.Dog3;
                ApplicationController.Instance.GameManager.SetLocalPet(PetType.Dog3);
                break;
            case 5:
                _userPet = PetType.Frog;
                ApplicationController.Instance.GameManager.SetLocalPet(PetType.Frog);
                break;
        }

        LocalSaver.SetPlayerPet(_userPet);

        if (ApplicationController.Instance.LobbyManager.InLobby())
        {
            await ApplicationController.Instance.GameLobbyManager.SetNewPet(_userPet);
        }
        
        gameObject.SetActive(false);
    }
}
