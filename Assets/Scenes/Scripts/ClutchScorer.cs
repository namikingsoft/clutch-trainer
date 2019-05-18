using System;
using System.Collections.Generic;
using Lib;

public class ClutchScorer
{
    private Dictionary<int, float> gearRatios;
    private float nonClutchRPMDiff = 100;
    private float permitShiftGearClutch = 0.1f;

    public ClutchScorer(Dictionary<int, float> gearRatios)
    {
        this.gearRatios = gearRatios;
    }

    public bool Calculate(int currentGear, int nextGear, float clutchRate, float engineShaftRPM, float driveShaftRPM)
    {
        if (currentGear == nextGear)
        {
            return true;
        }
        float ratio = gearRatios[nextGear];
        float expectedClutchShaftRPM = driveShaftRPM * ratio;
        if (clutchRate < permitShiftGearClutch || Math.Abs(engineShaftRPM - expectedClutchShaftRPM) < nonClutchRPMDiff)
        {
            return true;
        }
        return false;
    }
}
