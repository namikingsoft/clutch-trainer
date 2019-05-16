using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CustomInput : MonoBehaviour
{
    public int Gear { get; set; }
    public float Accel { get; private set; }
    public float Brake { get; private set; }
    public float Clutch { get; private set; }
    public bool StartEngine { get; private set; }

    private int handleIndex = -1;
    private int dirtRemainTickCount = 0;
    private int bumpyRemainTickCount = 0;

    private void Start()
    {
        Gear = 0;
        Accel = 0;
        Brake = 0;
        Clutch = 1;

        if (LogitechGSDK.LogiSteeringInitialize(false))
        {
            Debug.Log("LogiSteering Initialized");
            LogitechGSDK.LogiControllerPropertiesData properties = new LogitechGSDK.LogiControllerPropertiesData();
            properties.forceEnable = true;
            properties.overallGain = 100;
            properties.springGain = 100;
            properties.damperGain = 100;
            properties.defaultSpringEnabled = true;
            properties.defaultSpringGain = 100;
            properties.combinePedals = false;
            properties.wheelRange = 900;
            properties.gameSettingsEnabled = false;
            properties.allowGameSettings = false;
            LogitechGSDK.LogiSetPreferredControllerProperties(properties);
            for (int i = 0; ; i++)
            {
                if (!LogitechGSDK.LogiIsConnected(i)) break;
                if (LogitechGSDK.LogiIsDeviceConnected(i, LogitechGSDK.LOGI_DEVICE_TYPE_WHEEL))
                {
                    handleIndex = i;
                    StringBuilder deviceName = new StringBuilder(256);
                    LogitechGSDK.LogiGetFriendlyProductName(0, deviceName, 256);
                    Debug.Log(deviceName);
                    break;
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        Debug.Log("SteeringShutdown:" + LogitechGSDK.LogiSteeringShutdown());
    }

    private void Update()
    {

        if (handleIndex >= 0 && LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(handleIndex))
        {
            LogitechGSDK.DIJOYSTATE2ENGINES rec;
            rec = LogitechGSDK.LogiGetStateUnity(handleIndex);

            Accel = (-rec.lY + 32768f) / 65535f * 0.65f;
            Brake = (-rec.lRz + 32768f) / 65535f;
            Clutch = (rec.rglSlider[1] + 32768f) / 65535f;
            if (rec.rgbButtons[8] == 128) Gear = 1;
            else if (rec.rgbButtons[9] == 128) Gear = 2;
            else if (rec.rgbButtons[10] == 128) Gear = 3;
            else if (rec.rgbButtons[11] == 128) Gear = 4;
            else if (rec.rgbButtons[12] == 128) Gear = 5;
            else if (rec.rgbButtons[13] == 128) Gear = 6;
            else Gear = 0;
            StartEngine = rec.rgbButtons[18] == 128;
            return;
        }
        UpdateGear();
        UpdateAccel();
        UpdateBrake();
        UpdateClutch();
        UpdateStartEngine();
    }

    private void FixedUpdate()
    {
        if (handleIndex <= -1) return;
        if (dirtRemainTickCount > 0)
        {
            dirtRemainTickCount--;
        } else
        {
            if (LogitechGSDK.LogiIsPlaying(handleIndex, LogitechGSDK.LOGI_FORCE_DIRT_ROAD))
            {
                LogitechGSDK.LogiStopDirtRoadEffect(handleIndex);
            }
        }
        if (bumpyRemainTickCount > 0)
        {
            bumpyRemainTickCount--;
        }
        else
        {
            if (LogitechGSDK.LogiIsPlaying(handleIndex, LogitechGSDK.LOGI_FORCE_BUMPY_ROAD))
            {
                LogitechGSDK.LogiStopBumpyRoadEffect(handleIndex);
            }
        }
    }

    public void DirtTickCount(int count, int strength)
    {
        dirtRemainTickCount = count;
        if (!LogitechGSDK.LogiIsPlaying(handleIndex, LogitechGSDK.LOGI_FORCE_BUMPY_ROAD))
        {
            LogitechGSDK.LogiPlayDirtRoadEffect(handleIndex, strength);
        }
    }

    public void BumpyTickCount(int count, int strength)
    {
        bumpyRemainTickCount = count;
        if (!LogitechGSDK.LogiIsPlaying(handleIndex, LogitechGSDK.LOGI_FORCE_BUMPY_ROAD))
        {
            LogitechGSDK.LogiPlayBumpyRoadEffect(handleIndex, strength);
        }
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
