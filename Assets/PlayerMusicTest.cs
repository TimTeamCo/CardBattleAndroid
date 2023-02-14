using UnityEngine;
using UnityEngine.Audio;

public class PlayerMusicTest : MonoBehaviour
{
    [Range(1,16)]
    [SerializeField] public int zone;
    
    [SerializeField] public AudioMixer mixer;
    private AudioMixerSnapshot fullAudio;
    private AudioMixerSnapshot Perc80;
    private AudioMixerSnapshot Perc60;
    private AudioMixerSnapshot Perc30;
    private AudioMixerSnapshot Shut;

    private void Start()
    {
        fullAudio = mixer.FindSnapshot("FullAudio");
        Perc80 = mixer.FindSnapshot("80%");
        Perc60 = mixer.FindSnapshot("60%");
        Perc30 = mixer.FindSnapshot("30%");
        Shut = mixer.FindSnapshot("Shut");
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        switch (zone)
        {
            case 1:
                Shut.TransitionTo(0.1f);
                break;
            case 2:
                Perc30.TransitionTo(2f);
                break;
            case 4:
                Perc30.TransitionTo(2f);
                break;
            case 5:
                Perc60.TransitionTo(2f);
                break;
            case 7:
                Perc60.TransitionTo(2f);
                break;
            case 6:
                Perc80.TransitionTo(2f);
                break;
            case 8:
                fullAudio.TransitionTo(2f);
                break;
        }
    }
}
