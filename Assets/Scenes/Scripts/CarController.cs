using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lib;

public class CarController : MonoBehaviour
{
    public Slider AccelSlider;
    public Slider BrakeSlider;
    public Slider ClutchSlider;

    public GameObject EngineMeter;
    public GameObject SpeedMeter;
    public GameObject shifterHandle;

    private CarDynamics dynamics = new CarDynamics();
    private string rpmLabel = "";

    private void Start()
    {
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 1000), rpmLabel);
    }

    private void Update()
    {
        AccelSlider.value = CustomInput.Accel;
        BrakeSlider.value = CustomInput.Brake;
        ClutchSlider.value = CustomInput.Clutch;
        shifterHandle.GetComponent<ShifterHandle>().Shift(CustomInput.Gear);
    }

    private void FixedUpdate()
    {
        if (CustomInput.StartEngine)
        {
            dynamics.StartEngine();
        }
        if (!dynamics.ShiftGear(CustomInput.Gear))
        {
            CustomInput.Gear = 0;
            shifterHandle.GetComponent<ShifterHandle>().Shift(CustomInput.Gear);
        }
        dynamics.SetThrottle(CustomInput.Accel);
        dynamics.SetBrake(CustomInput.Brake);
        dynamics.SetClutch(CustomInput.Clutch);
        dynamics.Tick(Time.deltaTime); // 0.02 = 1 / 50

        EngineMeter.GetComponent<TachoMeter>().SetValue(dynamics.EngineShaftRPM);
        SpeedMeter.GetComponent<TachoMeter>().SetValue(dynamics.DriveMPS);

        rpmLabel = "";
        rpmLabel += "engine rpm: " + dynamics.EngineShaftRPM + "\n";
        rpmLabel += "drive rpm: " + dynamics.DriveShaftRPM + "\n";
        rpmLabel += "drive speed: " + dynamics.DriveMPS + "\n";
        rpmLabel += "\n";
        rpmLabel += "clutch: " + CustomInput.Clutch + "\n";
        rpmLabel += "brake: " + CustomInput.Brake + "\n";
        rpmLabel += "accel: " + CustomInput.Accel + "\n";
    }
}
