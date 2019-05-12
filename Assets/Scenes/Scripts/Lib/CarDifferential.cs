using System;
using UnityEngine;

namespace Lib
{
    public class CarDifferential
    {
        //---- Constants
        // Gear ratio
        private float finalDrive = 4.1f;

        // For modeling of speed-sensitive limited-slip differentials.
        // The maximum anti_slip torque that will be applied.
        // For speed-sensitive limited-slip differentials,
        // the anti-slip multiplier that's always applied.
        private float antiSlip = 600.0f;

        // For modeling of speed-sensitive limited-slip differentials.
        // Anti-slip dependence on torque.
        private float antiSlipTorque = 0;

        // For modeling of speed-sensitive limited-slip differentials
        // that are 1.5 or 2-way. Set to 0.0 for 1-way LSD.
        private float antiSlipTorqueDecelFactor = 0;

        // For modeling of epicyclic differentials.
        // Ranges from 0.0 to 1.0, where 0.0 applies all torque to side1
        private float torqueSplit = 0.5f;

        //---- Variables
        // side1 is left/front, side2 is right/back
        private float side1Speed = 0; 
        private float side2Speed = 0;
        private float side1Torque = 0; 
        private float side2Torque = 0;

        public void ComputeWheelTorques(float driveshaftTorque)
        {
            // Determine torque from anti-slip mechanism
            float cas = antiSlip;

            // If torque-sensitive
            if (antiSlipTorque > 0)
            {
                cas = antiSlipTorque * driveshaftTorque;
            }
            // TODO: Dev wanted to add minimum anti-slip

            // Determine deceleration behavior
            if (cas < 0) cas *= -antiSlipTorqueDecelFactor;

            cas = Math.Max(0.0f, cas);
            float drag = Mathf.Clamp(cas * (side1Speed - side2Speed), -antiSlip, antiSlip);

            float torque = driveshaftTorque * finalDrive;
            side1Torque = torque * (1.0f - torqueSplit) - drag;
            side2Torque = torque * torqueSplit + drag;
        }

        public void SetAntiSlip(float _as, float ast, float astdf)
        {
            antiSlip = _as;
            antiSlipTorque = ast;
            antiSlipTorqueDecelFactor = astdf;
        }

        public float CalculateDriveshaftSpeed(float newSide1Speed, float newSide2Speed)
        {
            side1Speed = newSide1Speed;
            side2Speed = newSide2Speed;

            return GetDriveshaftSpeed();
        }

        public float GetDriveshaftSpeed() 
        {
            return finalDrive * (side1Speed + side2Speed) * 0.5f;
        }

        public float GetFinalDrive() { return finalDrive; }
        public void SetFinalDrive(float fd) { finalDrive = fd; }

        public float GetSide1Speed() { return side1Speed; }
        public float GetSide2Speed() { return side2Speed; }
        public float GetSide1Torque() { return side1Torque; }
        public float GetSide2Torque() { return side2Torque; }
    }
}
