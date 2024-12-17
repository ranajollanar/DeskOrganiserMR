using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBGM : MonoBehaviour
{
    public void SetBGM(AudioClip bgmClip)
    {
        AudioSource audioSource = GetComponent<AudioSource>();

        if (audioSource.isPlaying)
        {
            // Preserve current playback times
            float currentTime = audioSource.time;

            // Assign the new clip and resume from the same point if possible
            audioSource.clip = bgmClip;
            audioSource.time = Mathf.Min(currentTime, bgmClip.length); // Ensure the time doesn't exceed the new clip's length
            audioSource.Play();
        }
        else
        {
            audioSource.clip = bgmClip;
        }
    }
}