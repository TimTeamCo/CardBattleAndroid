using System;
using UnityEngine;
using UnityEngine.LowLevel;

public class SoundSFX : MonoBehaviour
{
    public AudioSource myFx;
    public AudioClip ButtonClickSound;
    private AudioSource Circle;
    
    public void StartButton()
    {
        myFx.PlayOneShot(ButtonClickSound);
    }

    private void Start()
    {
        Circle = GetComponent<AudioSource>();
    }
}