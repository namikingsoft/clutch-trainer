using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineController : MonoBehaviour
{
    private static int MinEngineRpm = 500;
    private static int MaxEngineRpm = 3000;
    
    public GameObject EngineShaft;
    public GameObject InputShaft;

    private float engineRpm;
    private float inputRpm;

    void Start()
    {
        engineRpm = 500;
    }

    void OnGUI()
    {
        string rpmLabel = "";
        rpmLabel += "rpm (Engine): " + engineRpm + "\n";
        rpmLabel += "rpm (Input):  " + inputRpm + "\n";
        GUI.Label(new Rect(10, 10, 200, 1000), rpmLabel);
    }

    void FixedUpdate()
    {
        Calculate();
        Transfer();
    }

    void Calculate()
    {
        engineRpm = engineRpm + CustomInput.Accel * 50;

        if (engineRpm > MaxEngineRpm)
        {
            engineRpm = MaxEngineRpm;
        }
        if (engineRpm > MinEngineRpm)
        {
            engineRpm = engineRpm - 15;
        }
        else
        {
            engineRpm = MinEngineRpm;
        }

        inputRpm = inputRpm + ((engineRpm - inputRpm) * (1 - CustomInput.Clutch));
        if (inputRpm > engineRpm)
        {
            inputRpm = engineRpm;
        }
        if (inputRpm > 0)
        {
            inputRpm = inputRpm - 15;
        }
    }

    private void Transfer()
    {
        EngineShaft.transform.Rotate(new Vector3(0, engineRpm / 100, 0));
        InputShaft.transform.Rotate(new Vector3(0, inputRpm / 100, 0));
    }
}
