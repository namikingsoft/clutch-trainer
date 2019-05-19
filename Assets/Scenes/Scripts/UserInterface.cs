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

    private TransformShaker shaker;

    private bool isShaking = false;

    private void Start()
    {
        shifterHandle = transform.Find("Shifter").GetComponent<ShifterHandle>();
        engineMeter = transform.Find("Engine Meter").GetComponent<TachoMeter>();
        speedMeter = transform.Find("Speed Meter").GetComponent<TachoMeter>();

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
