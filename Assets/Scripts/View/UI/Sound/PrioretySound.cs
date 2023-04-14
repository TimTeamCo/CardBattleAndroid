using UnityEngine;

public class PrioretySound : MonoBehaviour
{
    public AudioClip SoundAudioClip;
    private int _index;
    
    public void Play()
    {
        ApplicationController.Instance.AudioController.PlayPriorityClip(ref _index, SoundAudioClip);
    }

    public void Stop()
    {
        ApplicationController.Instance.AudioController.StopPriorityClip(_index, SoundAudioClip);
    }
}