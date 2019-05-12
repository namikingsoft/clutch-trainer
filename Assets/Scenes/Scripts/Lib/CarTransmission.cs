using System; 
using System.Collections.Generic;

namespace Lib
{
    public class CarTransmission
    {
        private Dictionary<int, float> gearRatios; // Gear number and ratio; reverse gears are negative
        private int forwardGears = 1; // Number of consecutive forward gears
        private int reverseGears; // Number of consecutive reverse gears

        //---- Variables
        private int gear = 0; // Current gear

        //---- For info only;
        private float driveshaftRPM;
        private float crankshaftRPM;

        public CarTransmission()
        {
            gearRatios = new Dictionary<int, float>();
            gearRatios[0] = 0.0f;
        }

        public int GetGear() { return gear; }
        public int GetForwardGears() { return forwardGears; }
        public int GetReverseGears() { return reverseGears; }

        public void Shift(int newGear)
        {
            if (newGear <= forwardGears && newGear >= -reverseGears)
            {
                gear = newGear;
            }
        }

        // Gear ratio is (driveshaft speed / crankshaft speed)
        public void SetGearRatio(int gear, float ratio)
        {
            gearRatios[gear] = ratio;

            // Determine the number of consecutive forward and reverse gears
            forwardGears = 0;
            reverseGears = 0;
            foreach (int key in gearRatios.Keys)
            {
                if (key > 0)
                {
                    forwardGears++;
                }
                else if (key < 0)
                {
                    reverseGears++;
                }

            }
        }

        public float GetGearRatio(int gear)
        {
            return gearRatios.ContainsKey(gear) ? gearRatios[gear] : 1.0f;
        }

        public float GetCurrentGearRatio() { return GetGearRatio(gear); }

        // Get the torque on the driveshaft due to given clutch torque
        public float GetTorque(float clutchTorque) { return clutchTorque * gearRatios[gear]; }

        // Get the rotational speed of the clutch given the rotational speed of the driveshaft
        public float CalculateClutchSpeed(float driveshaftSpeed)
        {
            driveshaftRPM = driveshaftSpeed * 30.0f / (float)Math.PI;
            crankshaftRPM = driveshaftSpeed * gearRatios[gear] * 30.0f / (float)Math.PI;

            return driveshaftSpeed * gearRatios[gear];
        }

        public float GetClutchSpeed(float driveshaftSpeed)
        {
            return driveshaftSpeed * GetGearRatio(gear);
        }
    }
}
