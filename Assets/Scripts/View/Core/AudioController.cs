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

    public void PlayPriorityClip(ref int index, AudioClip audioClip, bool startToPlay = true, bool loop = false)
    {
        bool clipSeted = false;

        if (_audioSources.Count == 0)
        {
            AddNewAudioSource(audioClip, startToPlay, loop);
            index = 0;
        }
        else
        {
            for (int i = 0; i < _audioSources.Count; i++)
            {
                if (_audioSources[i].isPlaying)
                {
                    continue;
                }
                else
                {
                    SetClip(_audioSources[i], audioClip, startToPlay, loop);
                    clipSeted = true;
                    index = i;
                    break;
                }
            }
        }

        if (clipSeted == false)
        {
            AddNewAudioSource(audioClip, startToPlay, loop);
            index = _audioSources.Count-1;
        }
    }

    public void StopPriorityClip(int index, AudioClip audioClip)
    {
        if (_audioSources[index].clip = audioClip)
        {
            _audioSources[index].Stop();
        }
        else
        {
            Debug.Log($"_audioSources is not playing AudioClip - {audioClip.name}");
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

    public void StopSFX(AudioClip audioClip)
    {
        if (_SFXMainSource.clip = audioClip)
        {
            _SFXMainSource.Stop();
        }
        else if (_SFXAdditionalSource.clip = audioClip)
        {
            _SFXAdditionalSource.Stop();
        }
        else
        {
            Debug.Log($"SFX is not playing AudioClip - {audioClip.name}");
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
