using UnityEngine;
using UnityEngine.UI;

public class MapButton : MonoBehaviour
{
    [SerializeField] public GameObject Panel;
    private void OnMouseDown()
    {
        Panel.SetActive(false);
    }
}
