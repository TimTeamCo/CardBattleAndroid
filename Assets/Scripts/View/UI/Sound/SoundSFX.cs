using UnityEngine;

public class SoundSFX : MonoBehaviour
{
    [SerializeField] private AudioClip SoundAudioClipSFX;
    
    public void PlaySFX()
    {
        ApplicationController.Instance.AudioController.SetSFX(SoundAudioClipSFX);
    }
}