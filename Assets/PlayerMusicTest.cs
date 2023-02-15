using UnityEngine;
using UnityEngine.Audio;

public class PlayerMusicTest : MonoBehaviour
{
    [Range(1,16)]
    [SerializeField] public int zone;
    
    [SerializeField] public AudioMixer mixer;
    private AudioMixerSnapshot fullAudio;
    private AudioMixerSnapshot MixRedForestMistic;
    private AudioMixerSnapshot MixRedForestMagic;
    private AudioMixerSnapshot MixMisticMagic;
    private AudioMixerSnapshot Perc80;
    private AudioMixerSnapshot Perc60;
    private AudioMixerSnapshot Perc30;
    private AudioMixerSnapshot Shut;
    private AudioMixerSnapshot Magic_Swamp_fullAudio;
    private AudioMixerSnapshot Magic_Swamp_Perc80;
    private AudioMixerSnapshot Magic_Swamp_Perc60;
    private AudioMixerSnapshot Magic_Swamp_Perc30;
    private AudioMixerSnapshot Magic_Swamp_Shut;
    private AudioMixerSnapshot Mystic_FullAudio;
    private AudioMixerSnapshot Mystic80;
    private AudioMixerSnapshot Mystic60;
    private AudioMixerSnapshot Mystic30;
    private AudioMixerSnapshot Mystic_Shut;
    

    private void Start()
    {
        fullAudio = mixer.FindSnapshot("FullAudio");
        Perc80 = mixer.FindSnapshot("80%");
        Perc60 = mixer.FindSnapshot("60%");
        Perc30 = mixer.FindSnapshot("30%");
        Shut = mixer.FindSnapshot("Shut"); 
        Magic_Swamp_fullAudio = mixer.FindSnapshot("Magic_Swamp_FullAudio");
        Magic_Swamp_Perc80 = mixer.FindSnapshot("Magic_Swamp_80%");
        Magic_Swamp_Perc60 = mixer.FindSnapshot("Magic_Swamp_60%");
        Magic_Swamp_Perc30 = mixer.FindSnapshot("Magic_Swamp_30%");
        Magic_Swamp_Shut = mixer.FindSnapshot("Magic_Swamp_Shut");
        Mystic_FullAudio = mixer.FindSnapshot("Mystic_FullAudio");
        Mystic80 = mixer.FindSnapshot("Mystic_80%");
        Mystic60 = mixer.FindSnapshot("Mystic_60%");
        Mystic30 = mixer.FindSnapshot("Mystic_30%");
        Mystic_Shut = mixer.FindSnapshot("Mystic_Shut");
        MixRedForestMistic = mixer.FindSnapshot("Mix RedForesMistic");
        MixRedForestMagic = mixer.FindSnapshot("Mix RedForesMagic");
        MixMisticMagic = mixer.FindSnapshot("Mix MisticMagic");
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
                MixRedForestMistic.TransitionTo(2f);
                break;
            case 3:
                MixMisticMagic.TransitionTo(2f);
                break;
            case 4:
                MixRedForestMagic.TransitionTo(2f);
                break;
            case 5:
                Perc60.TransitionTo(2f);
                break;
            case 13:
                Magic_Swamp_Perc60.TransitionTo(2f);
                break;
            case 14:
                Magic_Swamp_Perc80.TransitionTo(2f);
                break;
            case 15:
                Magic_Swamp_Perc60.TransitionTo(2f);
                break;
            case 16:
                Magic_Swamp_fullAudio.TransitionTo(2f);
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
            case 9:
                Mystic60.TransitionTo(2f);
                break;
            case 11:
                Mystic60.TransitionTo(2f);
                break;
            case 10:
                Mystic80.TransitionTo(2f);
                break;
            case 12:
                Mystic_FullAudio.TransitionTo(2f);
                break;
        }
    }
}
