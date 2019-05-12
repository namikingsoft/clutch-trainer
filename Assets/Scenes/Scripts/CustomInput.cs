using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInput : MonoBehaviour
{
    public static int Gear { get; set; }
    public static float Accel { get; private set; }
    public static float Brake { get; private set; }
    public static float Clutch { get; private set; }
    public static bool StartEngine { get; private set; }

    private void Start()
    {
        Gear = 0;
        Accel = 0;
        Brake = 0;
        Clutch = 1;
    }
    private void Update()
    {
        UpdateGear();
        UpdateAccel();
        UpdateBrake();
        UpdateClutch();
        UpdateStartEngine();
    }

    private void UpdateGear()
    {
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.F)) Gear = 0;
        else if (Input.GetKey(KeyCode.W)) Gear = 1;
        else if (Input.GetKey(KeyCode.X)) Gear = 2;
        else if (Input.GetKey(KeyCode.E)) Gear = 3;
        else if (Input.GetKey(KeyCode.C)) Gear = 4;
        else if (Input.GetKey(KeyCode.R)) Gear = 5;
        else if (Input.GetKey(KeyCode.V)) Gear = 6;
        else if (Input.GetKey(KeyCode.B)) Gear = -1;
    }

    private void UpdateAccel()
    {
        if (Input.GetKey(KeyCode.L))
        {
            Accel += 0.01f;
        }
        else if (Input.GetKey(KeyCode.O))
        {
            Accel += 0.05f;
        }
        else if (!Input.GetKey(KeyCode.Period))
        {
            Accel -= 0.03f;
        }

        if (Accel < 0)
        {
            Accel = 0;
        }
        else if (Accel > 0.65f)
        {
            Accel = 0.65f;
        }
    }

    private void UpdateBrake()
    {
        if (Input.GetKey(KeyCode.K))
        {
            Brake += 0.02f;
        }
        else if (Input.GetKey(KeyCode.I))
        {
            Brake += 0.1f;
        }
        else if (!Input.GetKey(KeyCode.Comma))
        {
            Brake -= 0.03f;
        }

        if (Brake < 0)
        {
            Brake = 0;
        }
        else if (Brake > 1)
        {
            Brake = 1;
        }
    }

    private void UpdateClutch()
    {
        if (Input.GetKey(KeyCode.J))
        {
            Clutch -= 0.1f;
        }
        else if (Input.GetKey(KeyCode.U))
        {
            Clutch -= 0.02f;
        }
        else if (!Input.GetKey(KeyCode.M))
        {
            Clutch += 0.015f;
        }

        if (Clutch < 0)
        {
            Clutch = 0;
        }
        else if (Clutch > 1)
        {
            Clutch = 1;
        }
    }

    private void UpdateStartEngine()
    {
        StartEngine = Input.GetKey(KeyCode.Return);
    }
}
