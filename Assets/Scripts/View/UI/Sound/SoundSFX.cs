using UnityEngine;

public class SoundSFX : MonoBehaviour
{
    public AudioClip SoundAudioClipSFX;
    
    public void PlaySFX()
    {
        ApplicationController.Instance.AudioController.SetSFX(SoundAudioClipSFX);
    }
}