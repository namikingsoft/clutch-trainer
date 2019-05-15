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

    public GameObject shifterHandle;
    public GameObject EngineMeter;
    public GameObject SpeedMeter;
    public Image SpeedOverlay;

    public GameObject Camera;
    public GameObject Particle;

    private Vector3 carPosition;

    private CarDynamics dynamics = new CarDynamics();
    private AudioSource engineAudioSource;
    private AudioSource engineStartAudioSource;
    private int engineStartCountDown = -1;
    private string rpmLabel = "";

    private void Start()
    {
        engineAudioSource = GetComponents<AudioSource>()[0];
        engineStartAudioSource = GetComponents<AudioSource>()[1];
        carPosition = Camera.transform.position; 
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
            if (engineStartCountDown < 0 && dynamics.EngineShaftRPM < 1)
            {
                engineStartAudioSource.Play();
                engineStartCountDown = 25;
            }
            else dynamics.StopEngine();
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
        carPosition += new Vector3(0, 0, 1) * dynamics.DriveMPS * Time.deltaTime;
        float magnitude = dynamics.DriveMPS / 175f;
        Camera.transform.position = carPosition + new Vector3(
            Random.Range(-1f, 1f) * magnitude,
            Random.Range(-1f, 1f) * magnitude,
            0);
        Particle.transform.position = carPosition + new Vector3(0, 0, 30);
        var particle = Particle.GetComponent<ParticleSystem>();
        var particleMain = particle.main;
        var particleTrails = particle.trails;
        var particleEmission = particle.emission;
        particleMain.startSpeed = 0.1f + dynamics.DriveMPS / 200 * 200;
        particleTrails.ratio = dynamics.DriveMPS / 200;
        particleEmission.rateOverTime = 10 + dynamics.DriveMPS / 200 * 300;
        SpeedOverlay.color = new Color(
            SpeedOverlay.color.r,
            SpeedOverlay.color.g,
            SpeedOverlay.color.b,
            dynamics.DriveMPS / 200 - 0.7f);

        rpmLabel = "";
        rpmLabel += "engine rpm: " + dynamics.EngineShaftRPM + "\n";
        rpmLabel += "drive rpm: " + dynamics.DriveShaftRPM + "\n";
        rpmLabel += "drive speed: " + dynamics.DriveMPS + "\n";
        rpmLabel += "\n";
        rpmLabel += "clutch: " + CustomInput.Clutch + "\n";
        rpmLabel += "brake: " + CustomInput.Brake + "\n";
        rpmLabel += "accel: " + CustomInput.Accel + "\n";
        rpmLabel += "adsf: " + Camera.transform.position + "\n";
    }
}
