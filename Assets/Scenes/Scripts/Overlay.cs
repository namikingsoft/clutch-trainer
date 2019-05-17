using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Overlay : MonoBehaviour
{
    private Image speed;
    private Image flush;

    private bool isFlushing = false;

    // Start is called before the first frame update
    void Start()
    {
        speed = transform.Find("Speed").GetComponent<Image>();
        flush = transform.Find("Flush").GetComponent<Image>();
    }

    public void SetSpeedFactor(float factor)
    {
        speed.color = new Color(
            speed.color.r,
            speed.color.g,
            speed.color.b,
            factor);
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
            flush.color = new Color(
                flush.color.r,
                flush.color.g,
                flush.color.b,
                elapsed / durationForward * strength);

            elapsed += Time.deltaTime;

            yield return null;
        }

        float baseAlpha = flush.color.a;
        elapsed = 0f;
        while (elapsed < durationBack)
        {
            flush.color = new Color(
                flush.color.r,
                flush.color.g,
                flush.color.b,
                baseAlpha - (elapsed / durationBack * strength));

            elapsed += Time.deltaTime;

            yield return null;
        }
        flush.color = new Color(
           flush.color.r,
           flush.color.g,
           flush.color.b,
           0);

        isFlushing = false;
    }
}
