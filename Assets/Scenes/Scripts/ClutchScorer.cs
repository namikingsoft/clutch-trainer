﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class ClutchScorer
{
    public enum Technic
    {
        None,
        Novice,
        Normal,
        Awesome,
        Gravitate,
        DoubleAwesome,
        DoubleGravitate,
    }

    private const float minScoreDriveShaftRPM = 100f;
    private const float normalTechDiffRPM = 400f;
    private const float awesomeTechDiffRPM = 150f;
    private const float gravitateTechDiffRPM = 30f;

    private Dictionary<int, float> gearRatios;
    private float nonClutchRPMDiff = 30f;
    private float permitShiftGearClutch = 0.1f;

    private int prevGear = 0;
    private int movedGear = 0;

    public ClutchScorer(Dictionary<int, float> gearRatios)
    {
        this.gearRatios = gearRatios;
    }

    public bool CanGearShift(
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

    public Technic JudgeTechnic(
        int gear,
        float clutchRate,
        float engineShaftRPM,
        float driveShaftRPM)
    {
        if (gear == 0) return Technic.None;

        int subGear = gear - prevGear;
        prevGear = gear;

        if (subGear != 0 && clutchRate < permitShiftGearClutch)
        {
            movedGear = subGear;
            return Technic.None;
        }

        Technic tech = Technic.None;
        if (movedGear != 0 && clutchRate > permitShiftGearClutch)
        {
            float ratio = gearRatios[gear];
            float expectedClutchShaftRPM = driveShaftRPM * ratio;
            float diffExpectedRPM = Math.Abs(engineShaftRPM - expectedClutchShaftRPM);
            Debug.Log(diffExpectedRPM);

            if ((gear == 1 && movedGear > 0) // easy for talk off
              || driveShaftRPM < minScoreDriveShaftRPM) tech = Technic.Normal; // normal when stop car
            else if (diffExpectedRPM < gravitateTechDiffRPM) tech = Technic.Gravitate;
            else if (diffExpectedRPM < awesomeTechDiffRPM) tech = Technic.Awesome;
            else if (diffExpectedRPM < normalTechDiffRPM) tech = Technic.Normal;
            else tech = Technic.Novice;
            movedGear = 0;
        }

        return tech;
    }

}
