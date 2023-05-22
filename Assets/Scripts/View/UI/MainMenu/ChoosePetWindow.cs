using UnityEngine;

public class ChoosePetWindow : MonoBehaviour
{
    public void SetPet(int petType)
    {
        switch (petType)
        {
            case 0:
                ApplicationController.Instance.GameManager.SetLocalPet(PetType.Cat1);
                break;
            case 1:
                ApplicationController.Instance.GameManager.SetLocalPet(PetType.Cat2);
                break;
            case 2:
                ApplicationController.Instance.GameManager.SetLocalPet(PetType.Dog1);
                break;
            case 3:
                ApplicationController.Instance.GameManager.SetLocalPet(PetType.Dog2);
                break;
            case 4:
                ApplicationController.Instance.GameManager.SetLocalPet(PetType.Dog3);
                break;
            case 5:
                ApplicationController.Instance.GameManager.SetLocalPet(PetType.Frog);
                break;
            default:
                ApplicationController.Instance.GameManager.SetLocalPet(PetType.Cat1);
                break;
        }

        gameObject.SetActive(false);
    }
}
