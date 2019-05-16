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

    private void Start()
    {
        shifterHandle = transform.Find("Shifter").GetComponent<ShifterHandle>();
        engineMeter = transform.Find("EngineMeter").GetComponent<TachoMeter>();
        speedMeter = transform.Find("SpeedMeter").GetComponent<TachoMeter>();

        clutchSlider = transform.Find("ClutchSlider").GetComponent<Slider>();
        brakeSlider = transform.Find("BrakeSlider").GetComponent<Slider>();
        accelSlider = transform.Find("AccelSlider").GetComponent<Slider>();
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
        clutchSlider.value = value; 
    }

    public void SetBrakeValue(float value)
    {
        brakeSlider.value = value;
    }

    public void SetAccelValue(float value)
    {
        accelSlider.value = value;
    }
}
