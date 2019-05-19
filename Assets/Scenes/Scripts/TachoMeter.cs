using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ref. https://www.youtube.com/watch?v=3xSYkFdQiZ0
public class TachoMeter : MonoBehaviour
{
    private const float ZeroAngle = 210;
    private const float MaxAngle = -20;

    public float MaxValue = 10000;
    public float RedlineValue = 8000;
    public float ValueToLabel = 1;
    public int LabelAmount = 10;
    public int TickAmount = 10;

    private float value = 0;
    private Transform needleTransform;
    private Transform labelTemplateTransform;
    private Transform tickTemplateTransform;

    public void SetValue(float val)
    {
        value = val;
        if (value < 0) value = 0;
        else if (value > MaxValue) value = MaxValue;
        needleTransform.eulerAngles = new Vector3(0, 0, GetRotation());
    }

    private void Start()
    {
        needleTransform = transform.Find("Needle");
        labelTemplateTransform = transform.Find("Label Template");
        tickTemplateTransform = transform.Find("Tick Template");
        if (labelTemplateTransform && labelTemplateTransform.gameObject) labelTemplateTransform.gameObject.SetActive(false);
        if (tickTemplateTransform && tickTemplateTransform.gameObject) tickTemplateTransform.gameObject.SetActive(false);
        CreateLabels();
        CreateTicks();
    }

    private void CreateLabels()
    {
        float totalAngleSize = ZeroAngle - MaxAngle;
        for (int i = 0; i <= LabelAmount; i++)
        {
            Transform labelTransform = Instantiate(labelTemplateTransform, transform);
            float labelNormalized = (float)i / LabelAmount;
            float labelAngle = ZeroAngle - labelNormalized * totalAngleSize;
            labelTransform.eulerAngles = new Vector3(0, 0, labelAngle);
            float val = labelNormalized * MaxValue;
            labelTransform.Find("Text").GetComponent<Text>().text = Mathf.RoundToInt(val * ValueToLabel).ToString();
            labelTransform.Find("Text").eulerAngles = Vector3.zero;
            if (val >= RedlineValue)
            {
                labelTransform.Find("Line").GetComponent<Image>().color = new Color(255, 0, 0);
            }
            labelTransform.gameObject.SetActive(true);
        }
        needleTransform.transform.SetAsLastSibling();
    }

    private void CreateTicks()
    {
        float totalAngleSize = ZeroAngle - MaxAngle;
        int tickAmount = LabelAmount * TickAmount;
        for (int i = 0; i <= tickAmount; i++)
        {
            Transform tickTransform = Instantiate(tickTemplateTransform, transform);
            float tickNormalized = (float)i / tickAmount;
            float tickAngle = ZeroAngle - tickNormalized * totalAngleSize;
            tickTransform.eulerAngles = new Vector3(0, 0, tickAngle);
            float val = tickNormalized * MaxValue;
            if (val >= RedlineValue)
            {
                tickTransform.Find("Line").GetComponent<Image>().color = new Color(255, 0, 0);
            }
            tickTransform.gameObject.SetActive(true);
        }
        needleTransform.transform.SetAsLastSibling();
    }


    private float GetRotation()
    {
        float totalAngleSize = ZeroAngle - MaxAngle;
        float valueNormalized = value / MaxValue;
        return ZeroAngle - valueNormalized * totalAngleSize;
    }
}
