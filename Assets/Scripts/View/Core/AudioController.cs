using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource _backgroundMusicSource;
    [SerializeField] private AudioSource _SFXMainSource;
    [SerializeField] private AudioSource _SFXAdditionalSource;

    private List<AudioSource> _audioSources = new List<AudioSource>();

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void PlayPriorityClip(AudioClip audioClip, bool startToPlay = true, bool loop = false)
    {
        bool clipSeted = false;

        if (_audioSources.Count == 0)
        {
            AddNewAudioSource(audioClip, startToPlay, loop);
        }
        else
        {
            foreach (var audioSource in _audioSources)
            {
                if (audioSource.isPlaying)
                {
                    continue;
                }
                else
                {
                    SetClip(audioSource, audioClip, startToPlay, loop);
                    clipSeted = true;
                    break;
                }
            }
        }

        if (clipSeted == false)
        {
            AddNewAudioSource(audioClip, startToPlay, loop);
        }
    }
    
    public void SetBackgroundMusicSource(AudioClip audioClip, bool startToPlay = true, bool loop = false)
    {
        SetClip(_backgroundMusicSource,audioClip, startToPlay, loop);
    }

    public void SetSFX(AudioClip audioClip, bool startToPlay = true, bool loop = false)
    {
        if (_SFXMainSource.isPlaying)
        {
            if (_SFXAdditionalSource.isPlaying)
            {
                _SFXAdditionalSource.volume = 0.5f;
                SetSFXMainSource(audioClip, startToPlay,loop);
            }
            else
            {
                _SFXMainSource.volume = 0.5f;
                SetSFXAdditionalSource(audioClip, startToPlay, loop);
            }
        }
        else
        {
            if (_SFXAdditionalSource.isPlaying)
            {
                _SFXAdditionalSource.volume = 0.5f;
            }
            SetSFXMainSource(audioClip, startToPlay,loop);
        }
    }

    private void AddNewAudioSource(AudioClip audioClip, bool startToPlay = true, bool loop = false)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        _audioSources.Add(audioSource);
        SetClip(audioSource, audioClip, startToPlay, loop);
    }
    
    private void SetSFXMainSource(AudioClip audioClip, bool startToPlay = true, bool loop = false)
    {
        _SFXMainSource.volume = 1;
        SetClip(_SFXMainSource,audioClip, startToPlay, loop);
    }
    
    private void SetSFXAdditionalSource(AudioClip audioClip, bool startToPlay = true, bool loop = false)
    {
        _SFXAdditionalSource.volume = 1;
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
