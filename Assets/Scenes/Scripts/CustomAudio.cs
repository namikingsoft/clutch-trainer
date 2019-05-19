using System.Collections;
using UnityEngine;

public class CustomAudio : MonoBehaviour
{
    private AudioSource engine;
    private AudioSource engineStart;
    private AudioSource gearChange;
    private AudioSource gearShock;
    private AudioSource gearNoise;
    private AudioSource gearFit;

    private bool isPlayingEngineStart = false;
    private bool isPlayingGearChange = false;
    private bool isPlayingGearShock = false;
    private bool isPlayingGearNoise = false;
    private bool isPlayingGearFit = false;

    private void Start()
    {
        engine = GetComponents<AudioSource>()[0];
        engineStart = GetComponents<AudioSource>()[1];
        gearChange = GetComponents<AudioSource>()[2];
        gearShock = GetComponents<AudioSource>()[3];
        gearNoise = GetComponents<AudioSource>()[4];
        gearFit = GetComponents<AudioSource>()[5];
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

    public bool PlayGearShock()
    {
        if (isPlayingGearShock) return false;
        StartCoroutine(DoPlayGearShock());
        return true;
    }
    public IEnumerator DoPlayGearShock()
    {
        if (isPlayingGearShock) yield break;
        isPlayingGearShock = true;
        gearShock.time = 0.2f;
        gearShock.Play();
        yield return new WaitForSeconds(0.5f);
        gearShock.Stop();
        isPlayingGearShock = false;
    }

    public bool PlayGearNoise()
    {
        if (isPlayingGearNoise) return false;
        StartCoroutine(DoPlayGearNoise());
        return true;
    }
    public IEnumerator DoPlayGearNoise()
    {
        if (isPlayingGearNoise) yield break;
        isPlayingGearNoise = true;
        gearNoise.time = 0.5f;
        gearNoise.Play();
        yield return new WaitForSeconds(1f);
        gearNoise.Stop();
        isPlayingGearNoise = false;
    }

    public bool PlayGearFit()
    {
        if (isPlayingGearFit) return false;
        StartCoroutine(DoPlayGearFit());
        return true;
    }
    public IEnumerator DoPlayGearFit()
    {
        if (isPlayingGearFit) yield break;
        isPlayingGearFit = true;
        gearFit.Play();
        yield return new WaitForSeconds(1f);
        gearFit.Stop();
        isPlayingGearFit = false;
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
