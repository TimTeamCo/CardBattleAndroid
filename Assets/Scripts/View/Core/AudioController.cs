using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource _backgroundMusicSource;
    [SerializeField] private AudioSource _SFXMainSource;
    [SerializeField] private AudioSource _SFXAdditionalSource;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void SetBackgroundMusicSource(AudioClip audioClip, bool startToPlay = true, bool loop = false)
    {
        SetClip(_backgroundMusicSource,audioClip, startToPlay, loop);
    }
    
    public void SetSFXMainSource(AudioClip audioClip, bool startToPlay = true, bool loop = false)
    {
        SetClip(_SFXMainSource,audioClip, startToPlay, loop);
    }
    
    public void SetSFXAdditionalSource(AudioClip audioClip, bool startToPlay = true, bool loop = false)
    {
        SetClip(_SFXAdditionalSource,audioClip, startToPlay, loop);
    }

    private void SetClip(AudioSource source, AudioClip audioClip, bool startToPlay, bool loop)
    {
        source.loop = false;

        source.clip = audioClip;
        if (startToPlay)
        {
            source.Play();
        }

        if (loop)
        {
            source.loop = true;
        }
    }
}
