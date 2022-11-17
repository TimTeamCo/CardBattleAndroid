using UnityEngine;
using UnityEngine.UI;

public class AlphaTest : MonoBehaviour
{
    private void OnEnable()
    {
        gameObject.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }
}
