using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreEffect : MonoBehaviour
{

    private GameObject pushEnterKeyText;

    private GameObject engineStartedText;
    private bool isDoEngineStarted = false;

    private GameObject stalledText;
    private bool isDoStalled = false;

    private GameObject gearShockText;
    private bool isDoGearShock = false;
    private TransformShaker gearShockShaker;

    private GameObject ouchText;
    private bool isDoOuch = false;
    private TransformShaker ouchShaker;

    private GameObject pleaseNoText;
    private bool isDoPleaseNo = false;
    private TransformShaker pleaseNoShaker;

    private GameObject gravitateText;
    private bool isDoGravitate = false;

    private GameObject awesomeText;
    private bool isDoAwesome = false;

    private GameObject doubleClutchText;
    private bool isDoDoubleClutch = false;

    private GameObject noviceText;
    private bool isDoNovice = false;
    private TransformShaker noviceShaker;

    private Image flushOverlay;
    private bool isFlushing = false;

    private void Start()
    {
        pushEnterKeyText = transform.Find("Push Enter Text").gameObject;
        engineStartedText = transform.Find("Engine Started Text").gameObject;
        stalledText = transform.Find("Stalled Text").gameObject;
        ouchText = transform.Find("Ouch Text").gameObject;
        pleaseNoText = transform.Find("Please No Text").gameObject;
        gearShockText = transform.Find("Gear Shock Text").gameObject;
        gravitateText = transform.Find("Gravitate Text").gameObject;
        awesomeText = transform.Find("Awesome Text").gameObject;
        doubleClutchText = transform.Find("Double Clutch Text").gameObject;
        noviceText = transform.Find("Novice Text").gameObject;
        flushOverlay = transform.Find("Flush Overlay").GetComponent<Image>();

        ouchShaker = new TransformShaker(ouchText.transform);
        pleaseNoShaker = new TransformShaker(pleaseNoText.transform);
        gearShockShaker = new TransformShaker(gearShockText.transform);
        noviceShaker = new TransformShaker(noviceText.transform);
    }

    public void SetVisibleOfPushEnter(bool visible)
    {
        pushEnterKeyText.SetActive(visible);
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

    public bool PleaseNo()
    {
        if (isDoPleaseNo) return false;
        StartCoroutine(DoPleaseNo());
        return true;
    }
    public IEnumerator DoPleaseNo()
    {
        if (isDoPleaseNo) yield break;
        isDoPleaseNo = true;

        pleaseNoText.SetActive(true);
        StartCoroutine(pleaseNoShaker.Do(2.5f, 2.5f));
        yield return new WaitForSeconds(2.75f);
        pleaseNoText.SetActive(false);

        isDoPleaseNo = false;
    }

    public bool GearShock()
    {
        if (isDoGearShock) return false;
        StartCoroutine(DoGearShock());
        return true;
    }
    public IEnumerator DoGearShock()
    {
        if (isDoGearShock) yield break;
        isDoGearShock = true;

        gearShockText.SetActive(true);
        StartCoroutine(gearShockShaker.Do(1f, 1.5f));
        yield return new WaitForSeconds(1f);
        gearShockText.SetActive(false);

        isDoGearShock = false;
    }

    public bool Gravitate()
    {
        if (isDoGravitate) return false;
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

    public bool Awesome()
    {
        if (isDoAwesome) return false;
        StartCoroutine(DoAwesome());
        return true;
    }
    public IEnumerator DoAwesome()
    {
        if (isDoAwesome) yield break;
        isDoAwesome = true;

        awesomeText.SetActive(true);
        StartCoroutine(DoFlush(0.25f, 0.25f));
        yield return new WaitForSeconds(1.5f);
        awesomeText.SetActive(false);

        isDoAwesome = false;
    }

    public bool DoubleClutch()
    {
        if (isDoDoubleClutch) return false;
        StartCoroutine(DoDoubleClutch());
        return true;
    }
    public IEnumerator DoDoubleClutch()
    {
        if (isDoDoubleClutch) yield break;
        isDoDoubleClutch = true;

        yield return new WaitForSeconds(0.4f);
        doubleClutchText.SetActive(true);
        yield return new WaitForSeconds(1f);
        StartCoroutine(DoFlush(0.5f, 0.5f));
        yield return new WaitForSeconds(1.4f);
        doubleClutchText.SetActive(false);

        isDoDoubleClutch = false;
    }

    public bool Novice()
    {
        if (isDoNovice) return false;
        StartCoroutine(DoNovice());
        return true;
    }
    public IEnumerator DoNovice()
    {
        if (isDoNovice) yield break;
        isDoNovice = true;

        noviceText.SetActive(true);
        StartCoroutine(noviceShaker.Do(1.0f, 1f));
        yield return new WaitForSeconds(1.5f);
        noviceText.SetActive(false);

        isDoNovice = false;
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
