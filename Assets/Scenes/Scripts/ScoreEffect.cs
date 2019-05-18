using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreEffect : MonoBehaviour
{
    private Image flushOverlay;
    private bool isFlushing = false;

    private void Start()
    {
        flushOverlay = transform.Find("FlushOverlay").GetComponent<Image>();
    }

    public bool Flush(float duration, float strength = 0.5f)
    {
        if (isFlushing) return false;
        StartCoroutine(DoFlush(duration, strength));
        return true;
    }
    public IEnumerator DoFlush(float duration, float strength = 0.5f)
    {
        if (isFlushing) yield break;
        isFlushing = true;

        float durationForward = (1f / 4f) * duration;
        float durationBack = (3f / 4f) * duration;

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
