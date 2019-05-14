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
    private AudioSource engineAudioSource;
    private AudioSource engineStartAudioSource;
    private int engineStartCountDown = -1;
    private string rpmLabel = "";

    private void Start()
    {
        engineAudioSource = GetComponents<AudioSource>()[0];
        engineStartAudioSource = GetComponents<AudioSource>()[1];
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
        if (CustomInput.StartEngine && engineStartCountDown < 0 && dynamics.EngineShaftRPM < 1)
        {
            engineStartAudioSource.Play();
            engineStartCountDown = 25;
        }
        if (engineStartCountDown == 0)
        {
            dynamics.StartEngine();
            engineStartCountDown--;
        }
        else if (engineStartCountDown > 0)
        {
            engineStartCountDown--;
        }

        if (!dynamics.ShiftGear(CustomInput.Gear))
        {
            CustomInput.Gear = 0;
            CustomInput.BumpyTickCount(15, 50);
            shifterHandle.GetComponent<ShifterHandle>().Shift(CustomInput.Gear);
        }
        dynamics.SetThrottle(CustomInput.Accel);
        dynamics.SetBrake(CustomInput.Brake);
        dynamics.SetClutch(CustomInput.Clutch);
        dynamics.Tick(Time.deltaTime); // 0.02 = 1 / 50

        EngineMeter.GetComponent<TachoMeter>().SetValue(dynamics.EngineShaftRPM);
        SpeedMeter.GetComponent<TachoMeter>().SetValue(dynamics.DriveMPS);

        engineAudioSource.pitch = dynamics.EngineShaftRPM * 0.0003f;

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
