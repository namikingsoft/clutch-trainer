using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAudio : MonoBehaviour
{
    private AudioSource engineAudioSource;
    private AudioSource engineStartAudioSource;

    private void Start()
    {
        engineAudioSource = GetComponents<AudioSource>()[0];
        engineStartAudioSource = GetComponents<AudioSource>()[1];
    }

    public void PlayEngineStart()
    {
        engineStartAudioSource.Play();
    }

    public void PitchEngine(float pitch)
    {
        engineAudioSource.pitch = pitch;
    }
}
