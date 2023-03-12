using UnityEngine;

public class PrioretySound : MonoBehaviour
{
    public AudioClip SoundAudioClip;
    
    public void Play()
    {
        ApplicationController.Instance.AudioController.PlayPriorityClip(SoundAudioClip);
    }
}