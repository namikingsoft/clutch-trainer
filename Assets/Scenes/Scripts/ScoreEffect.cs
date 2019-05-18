using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreEffect : MonoBehaviour
{
    private Image flushOverlay;
    private bool isFlushing = false;

    private GameObject gravitateText;
    private bool isDoGravitate = false;

    private void Start()
    {
        flushOverlay = transform.Find("Flush Overlay").GetComponent<Image>();
        gravitateText = transform.Find("Gravitate Text").gameObject;
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
        yield return new WaitForSeconds(2f);
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
