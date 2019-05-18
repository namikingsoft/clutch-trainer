using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreEffect : MonoBehaviour
{
    private Image flushOverlay;
    private bool isFlushing = false;

    private GameObject greatText;
    private bool isDoGreat = false;

    private void Start()
    {
        flushOverlay = transform.Find("FlushOverlay").GetComponent<Image>();
        greatText = transform.Find("Great").gameObject;
    }

    public bool Great()
    {
        if (isFlushing) return false;
        StartCoroutine(DoGreat());
        return true;
    }
    public IEnumerator DoGreat()
    {
        if (isDoGreat) yield break;
        isDoGreat = true;

        greatText.SetActive(true);
        StartCoroutine(DoFlush(0.4f, 0.4f));
        yield return new WaitForSeconds(1f);
        greatText.SetActive(false);

        isDoGreat = false;
    }

    private IEnumerator DoFlush(float duration, float strength = 0.5f)
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
