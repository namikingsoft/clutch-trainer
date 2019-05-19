using System.Collections;
using UnityEngine;

public class CustomAudio : MonoBehaviour
{
    private AudioSource engine;
    private AudioSource engineStart;
    private AudioSource gearChange;
    private AudioSource gearNoise;

    private bool isPlayingEngineStart = false;
    private bool isPlayingGearChange = false;
    private bool isPlayingGearNoise = false;

    private void Start()
    {
        engine = GetComponents<AudioSource>()[0];
        engineStart = GetComponents<AudioSource>()[1];
        gearChange = GetComponents<AudioSource>()[2];
        gearNoise = GetComponents<AudioSource>()[3];
    }

    public bool PlayEngineStart()
    {
        if (isPlayingEngineStart) return false;
        StartCoroutine(DoPlayEngineStart());
        return true;
    }
    public IEnumerator DoPlayEngineStart()
    {
        if (isPlayingEngineStart) yield break;
        isPlayingEngineStart = true;
        engineStart.Play();
        yield return new WaitForSeconds(0.5f);
        isPlayingEngineStart = false;
    }

    public bool PlayGearChange()
    {
        if (isPlayingGearChange) return false;
        StartCoroutine(DoPlayGearChange());
        return true;
    }
    public IEnumerator DoPlayGearChange()
    {
        if (isPlayingGearChange) yield break;
        isPlayingGearChange = true;
        gearChange.Play();
        yield return new WaitForSeconds(0.2f);
        isPlayingGearChange = false;
    }

    public bool PlayGearNoise()
    {
        if (isPlayingGearChange) return false;
        StartCoroutine(DoPlayGearNoise());
        return true;
    }
    public IEnumerator DoPlayGearNoise()
    {
        if (isPlayingGearNoise) yield break;
        isPlayingGearNoise = true;
        gearNoise.time = 0.2f;
        gearNoise.Play();
        yield return new WaitForSeconds(0.5f);
        gearNoise.Stop();
        isPlayingGearNoise = false;
    }

    public void PitchEngine(float pitch)
    {
        if (pitch < 0.01f)
        {
            if (engine.isPlaying) engine.Stop();
        }
        else
        {
            if (!engine.isPlaying)
            {
                engine.Play();
            }
        }
        engine.pitch = pitch;
    }
}
