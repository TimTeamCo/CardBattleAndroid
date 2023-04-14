using UnityEngine;

public class SoundSFX : MonoBehaviour
{
    public AudioClip SoundAudioClipSFX;
    private AudioSource _clipSource;
    
    public void PlaySFX()
    {
        ApplicationController.Instance.AudioController.SetSFX(SoundAudioClipSFX);
    }

    public void StopSFX()
    {
        ApplicationController.Instance.AudioController.StopSFX(SoundAudioClipSFX);
    }
}