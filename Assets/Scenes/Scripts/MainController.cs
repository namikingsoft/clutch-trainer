using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lib;

public class MainController : MonoBehaviour
{
    public UserInterface ui;
    public ScoreEffect scoreEffect;

    public GameObject Camera;
    public GameObject Particle;

    private CustomInput input;
    private CustomAudio sound; // TODO: Base class has `audio` field 

    private Vector3 carPosition;
    private float maxSpeed;

    private CarDynamics dynamics = new CarDynamics();
    private ClutchScorer clutchScorer;
    private EngineScorer engineScorer;

    private bool isStartingEngine = false;

    private bool prevEngineStoped = true;

    private string rpmLabel = "";

    private void Start()
    {
        clutchScorer = new ClutchScorer(dynamics.GetGearRatios());
        engineScorer = new EngineScorer();

        input = transform.Find("IO").GetComponent<CustomInput>();
        sound = transform.Find("IO").GetComponent<CustomAudio>();
        carPosition = Camera.transform.position;
        maxSpeed = dynamics.CalculateMaxMPS();
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 1000), rpmLabel);
    }

    private void Update()
    {
        ui.SetClutchValue(input.Clutch);
        ui.SetBrakeValue(input.Brake);
        ui.SetAccelValue(input.Accel);
        ui.ShiftGear(input.Gear);
    }

    private void FixedUpdate()
    {
        if (input.EngineToggle)
        {
            if (dynamics.IsEngineStoped()) StartCoroutine(StartEngine());
            else dynamics.StopEngine();
        }

        scoreEffect.SetVisibleOfPushEnter(dynamics.IsEngineStoped());

        if (prevEngineStoped != dynamics.IsEngineStoped() && dynamics.IsEngineStoped())
        {
            scoreEffect.Stalled();
        }
        prevEngineStoped = dynamics.IsEngineStoped();

        if (dynamics.GetGear() != input.Gear) // TODO: not garigari?
        {
            bool canGearChange = clutchScorer.Calculate(
                dynamics.GetGear(),
                input.Gear,
                input.Clutch,
                dynamics.EngineShaftRPM,
                dynamics.DriveShaftRPM);
            if (canGearChange)
            {
                scoreEffect.Gravitate();
                dynamics.ShiftGear(input.Gear);
                sound.PlayGearChange();
            }
            else
            {
                input.Gear = 0;
                input.BumpyTickCount(15, 50);
            }
        }
        dynamics.SetThrottle(input.Accel);
        dynamics.SetBrake(input.Brake);
        dynamics.SetClutch(input.Clutch);
        dynamics.Tick(Time.deltaTime); // 0.02 = 1 / 50

        float engineShaftRPMPerMax = dynamics.EngineShaftRPM / dynamics.GetMaxEnginePRM();
        float driveSpeedPerMax = dynamics.DriveMPS / maxSpeed;

        ui.SetEngineValue(dynamics.EngineShaftRPM);
        ui.SetSpeedValue(dynamics.DriveMPS);

        float uiShakeFactor = (engineShaftRPMPerMax - 0.5f) / 0.5f;
        if (uiShakeFactor > 0) ui.Shake(0.1f, uiShakeFactor * 12.5f);

        sound.PitchEngine(dynamics.EngineShaftRPM * 0.0003f);
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
        particleMain.startSpeed = 0.1f + driveSpeedPerMax * 200;
        particleTrails.ratio = driveSpeedPerMax;
        particleEmission.rateOverTime = 10 + driveSpeedPerMax * 300;
        ui.ApplySpeedOverlay(driveSpeedPerMax - 0.7f);

        float engineDiffScore = engineScorer.CalcEngineShaftRPMScore(
            dynamics.GetGear(),
            input.Clutch,
            dynamics.EngineShaftRPM);

        if (engineDiffScore > 1f) scoreEffect.Ouch();

        rpmLabel = "";
        rpmLabel += "engine rpm: " + dynamics.EngineShaftRPM + "\n";
        rpmLabel += "drive rpm: " + dynamics.DriveShaftRPM + "\n";
        rpmLabel += "drive speed: " + dynamics.DriveMPS + "\n";
        rpmLabel += "\n";
        rpmLabel += "engine score: " + engineDiffScore + "\n";
        rpmLabel += "position: " + Camera.transform.position + "\n";
    }

    private IEnumerator StartEngine()
    {
        if (isStartingEngine) yield break;
        isStartingEngine = true;
        yield return sound.DoPlayEngineStart();
        dynamics.StartEngine();
        scoreEffect.EngineStarted();
        isStartingEngine = false;
    }
}
