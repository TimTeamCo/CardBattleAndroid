using UnityEngine;

public class SoundSFX : MonoBehaviour
{
    public AudioSource myFx;
    public AudioClip ButtonClickSound;
    
    public void StartButton()
    {
        myFx.PlayOneShot(ButtonClickSound);
    }
}