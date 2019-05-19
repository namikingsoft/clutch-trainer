using System;
using System.Collections.Generic;
using Lib;

public class ClutchScorer
{
    private Dictionary<int, float> gearRatios;
    private float nonClutchRPMDiff = 50f;
    private float permitShiftGearClutch = 0.1f;

    public ClutchScorer(Dictionary<int, float> gearRatios)
    {
        this.gearRatios = gearRatios;
    }

    public bool Calculate(
        int currentGear,
        int nextGear,
        float clutchRate,
        float accelRate,
        float engineShaftRPM,
        float driveShaftRPM)
    {
        if (
            currentGear == nextGear ||
            // To N if accel is zero
            (nextGear == 0 && accelRate <= float.Epsilon)) {
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
