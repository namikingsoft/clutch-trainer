using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreEffect : MonoBehaviour
{
    private Image flushOverlay;
    private bool isFlushing = false;

    private GameObject engineStartedText;
    private bool isDoEngineStarted = false;

    private GameObject stalledText;
    private bool isDoStalled = false;

    private GameObject ouchText;
    private bool isDoOuch = false;

    private GameObject gravitateText;
    private bool isDoGravitate = false;

    private TransformShaker ouchShaker;

    private void Start()
    {
        flushOverlay = transform.Find("Flush Overlay").GetComponent<Image>();
        engineStartedText = transform.Find("Engine Started Text").gameObject;
        stalledText = transform.Find("Stalled Text").gameObject;
        ouchText = transform.Find("Ouch Text").gameObject;
        gravitateText = transform.Find("Gravitate Text").gameObject;

        ouchShaker = new TransformShaker(ouchText.transform);
    }

    public bool EngineStarted()
    {
        if (isDoEngineStarted) return false;
        StartCoroutine(DoEngineStarted());
        return true;
    }
    public IEnumerator DoEngineStarted()
    {
        if (isDoEngineStarted) yield break;
        isDoEngineStarted = true;

        engineStartedText.SetActive(true);
        yield return new WaitForSeconds(2f);
        engineStartedText.SetActive(false);

        isDoEngineStarted = false;
    }

    public bool Stalled()
    {
        if (isDoStalled) return false;
        StartCoroutine(DoStalled());
        return true;
    }
    public IEnumerator DoStalled()
    {
        if (isDoStalled) yield break;
        isDoStalled = true;

        stalledText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        stalledText.SetActive(false);

        isDoStalled = false;
    }

    public bool Ouch()
    {
        if (isDoOuch) return false;
        StartCoroutine(DoOuch());
        return true;
    }
    public IEnumerator DoOuch()
    {
        if (isDoOuch) yield break;
        isDoOuch = true;

        ouchText.SetActive(true);
        StartCoroutine(ouchShaker.Do(1.5f, 2.5f));
        yield return new WaitForSeconds(1.5f);
        ouchText.SetActive(false);

        isDoOuch = false;
    }

    public bool Gravitate()
    {
        if (isFlushing) return false;
        StartCoroutine(DoGravitate());
        return true;
    }
    public IEnumerator DoGravitate()
    {
        if (isDoGravitate) yield break;
        isDoGravitate = true;

        gravitateText.SetActive(true);
        yield return new WaitForSeconds(.5f);
        StartCoroutine(DoFlush(0.5f, 1f));
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(DoFlush(0.25f, 0.5f));
        yield return new WaitForSeconds(0.5f);
        gravitateText.SetActive(false);

        isDoGravitate = false;
    }

    private IEnumerator DoFlush(float duration, float strength = 0.5f)
    {
        if (isFlushing) yield break;
        isFlushing = true;

        float durationForward = (2f / 5f) * duration;
        float durationBack = (3f / 5f) * duration;

        float elapsed = 0f;
        while (elapsed < durationForward)
        {
            flushOverlay.color = new Color(
                flushOverlay.color.r,
                flushOverlay.color.g,
                flushOverlay.color.b,
                elapsed / durationForward * strength);

            elapsed += Time.deltaTime;

            yield return null;
        }

        float baseAlpha = flushOverlay.color.a;
        elapsed = 0f;
        while (elapsed < durationBack)
        {
            flushOverlay.color = new Color(
                flushOverlay.color.r,
                flushOverlay.color.g,
                flushOverlay.color.b,
                baseAlpha - (elapsed / durationBack * strength));

            elapsed += Time.deltaTime;

            yield return null;
        }
        flushOverlay.color = new Color(
           flushOverlay.color.r,
           flushOverlay.color.g,
           flushOverlay.color.b,
           0);

        isFlushing = false;
    }
}
