using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSFX : MonoBehaviour
{
   public AudioSource myFx;
   public AudioClip StartButtonsound;

   public void StartButton()
   {
      myFx.PlayOneShot(StartButtonsound);
   }
}
