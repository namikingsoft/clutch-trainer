using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    private ShifterHandle shifterHandle;
    private TachoMeter engineMeter;
    private TachoMeter speedMeter;
    private Slider clutchSlider;
    private Slider brakeSlider;
    private Slider accelSlider;
    private Image speedOverlay;

    private TachoMeter expectedEngineMeter1;
    private TachoMeter expectedEngineMeter2;
    private TachoMeter expectedEngineMeter3;
    private TachoMeter expectedEngineMeter4;
    private TachoMeter expectedEngineMeter5;
    private TachoMeter expectedEngineMeter6;

    private TransformShaker shaker;

    private bool isShaking = false;

    private void Start()
    {
        shifterHandle = transform.Find("Shifter").GetComponent<ShifterHandle>();
        engineMeter = transform.Find("Engine Meter").GetComponent<TachoMeter>();
        speedMeter = transform.Find("Speed Meter").GetComponent<TachoMeter>();

        expectedEngineMeter1 = transform.Find("Expected Engine Meter1").GetComponent<TachoMeter>();
        expectedEngineMeter2 = transform.Find("Expected Engine Meter2").GetComponent<TachoMeter>();
        expectedEngineMeter3 = transform.Find("Expected Engine Meter3").GetComponent<TachoMeter>();
        expectedEngineMeter4 = transform.Find("Expected Engine Meter4").GetComponent<TachoMeter>();
        expectedEngineMeter5 = transform.Find("Expected Engine Meter5").GetComponent<TachoMeter>();
        expectedEngineMeter6 = transform.Find("Expected Engine Meter6").GetComponent<TachoMeter>();

        clutchSlider = transform.Find("Clutch Slider").GetComponent<Slider>();
        brakeSlider = transform.Find("Brake Slider").GetComponent<Slider>();
        accelSlider = transform.Find("Accel Slider").GetComponent<Slider>(); 
        speedOverlay = transform.Find("Speed Overlay").GetComponent<Image>();

        shaker = new TransformShaker(transform);
    }

    public void ShiftGear(int gear)
    {
        shifterHandle.Shift(gear);
    }

    public void SetEngineValue(float value)
    {
        engineMeter.SetValue(value);
    }

    public void SetSpeedValue(float value)
    {
        speedMeter.SetValue(value);
        speedMeter.SetValueText(((int)value).ToString().PadLeft(3, '0'));
    }

    public void SetExpectedEngineValue1(float value)
    {
        expectedEngineMeter1.SetValue(value);
    }

    public void SetExpectedEngineValue2(float value)
    {
        expectedEngineMeter2.SetValue(value);
    }

    public void SetExpectedEngineValue3(float value)
    {
        expectedEngineMeter3.SetValue(value);
    }

    public void SetExpectedEngineValue4(float value)
    {
        expectedEngineMeter4.SetValue(value);
    }

    public void SetExpectedEngineValue5(float value)
    {
        expectedEngineMeter5.SetValue(value);
    }

    public void SetExpectedEngineValue6(float value)
    {
        expectedEngineMeter6.SetValue(value);
    }

    public void SetClutchValue(float value)
    {
        clutchSlider.value = 1f - value; 
    }

    public void SetBrakeValue(float value)
    {
        brakeSlider.value = value;
    }

    public void SetAccelValue(float value)
    {
        accelSlider.value = value;
    }

    public void Shake(float duration, float magnitude = 1)
    {
        StartCoroutine(shaker.Do(duration, magnitude));
    }

    public void ApplySpeedOverlay(float rate)
    {
        speedOverlay.color = new Color(
            speedOverlay.color.r,
            speedOverlay.color.g,
            speedOverlay.color.b,
            rate);
    }
}
