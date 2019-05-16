using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lib
{
    public class CarDynamics
    {
        private static float nonClutchRPMDiff = 100;
        private static float permitShiftGearClutch = 0.1f;

        private CarEngine engine = new CarEngine();
        private CarClutch clutch = new CarClutch();
        private CarBrake brake = new CarBrake();
        private CarWheel wheel = new CarWheel();
        private CarTransmission transmission = new CarTransmission();

        public float DriveShaftRPM { get; private set; }
        public float ClutchShaftRPM { get; private set; }
        public float EngineShaftRPM { get; private set; }
        public float DriveMPS { get; private set; }

        public CarDynamics()
        {
            float torqueValMul = 0.88f;
            List<Tuple<float, float>> torqueCurve = new List<Tuple<float, float>>();
            torqueCurve.Add(new Tuple<float, float>(1000, torqueValMul * 325));
            torqueCurve.Add(new Tuple<float, float>(1500, torqueValMul * 360));
            torqueCurve.Add(new Tuple<float, float>(2200, torqueValMul * 388));
            torqueCurve.Add(new Tuple<float, float>(2700, torqueValMul * 426));
            torqueCurve.Add(new Tuple<float, float>(3200, torqueValMul * 452));
            torqueCurve.Add(new Tuple<float, float>(3800, torqueValMul * 482));
            torqueCurve.Add(new Tuple<float, float>(4200, torqueValMul * 511));
            torqueCurve.Add(new Tuple<float, float>(4800, torqueValMul * 555));
            torqueCurve.Add(new Tuple<float, float>(5200, torqueValMul * 576));
            torqueCurve.Add(new Tuple<float, float>(5600, torqueValMul * 580));
            torqueCurve.Add(new Tuple<float, float>(6000, torqueValMul * 576));
            torqueCurve.Add(new Tuple<float, float>(6500, torqueValMul * 554));
            torqueCurve.Add(new Tuple<float, float>(7000, torqueValMul * 526));
            torqueCurve.Add(new Tuple<float, float>(7500, torqueValMul * 498));
            torqueCurve.Add(new Tuple<float, float>(8000, torqueValMul * 475));
            torqueCurve.Add(new Tuple<float, float>(8500, torqueValMul * 454));
            torqueCurve.Add(new Tuple<float, float>(9000, torqueValMul * 409));
            engine.SetTorqueCurve(8000, torqueCurve);
            engine.SetMaxRPM(8000);
            engine.SetInertia(0.3f);
            engine.SetStartRPM(1000);
            engine.SetInitialConditions();

            wheel.SetRadius(0.2f);
            wheel.SetInertia(6);
            wheel.SetInitialConditions();

            transmission.SetGearRatio(-1, -3.29f);
            transmission.SetGearRatio(1, 3.29f);
            transmission.SetGearRatio(2, 2.16f);
            transmission.SetGearRatio(3, 1.61f);
            transmission.SetGearRatio(4, 1.27f);
            transmission.SetGearRatio(5, 1.03f);
            transmission.SetGearRatio(6, 0.85f);

            DriveShaftRPM = 0;
            ClutchShaftRPM = 0;
            EngineShaftRPM = engine.GetRPM();
        }

        public float GetMaxEnginePRM() { return engine.GetMaxRPM(); }

        public void SetThrottle(float value) { engine.SetThrottle(value); }
        public void SetClutch(float value) { clutch.SetClutch(value); }
        public void SetBrake(float value) { brake.SetBrakeFactor(value); }

        public int GetGear() { return transmission.GetGear(); }
        public bool ShiftGear(int value)
        {
            if (value == transmission.GetGear())
            {
                return true;
            }
            float ratio = transmission.GetGearRatio(value);
            float nextClutchShaftRPM = DriveShaftRPM * ratio;
            Debug.Log(EngineShaftRPM - nextClutchShaftRPM);
            if (clutch.GetClutch() < permitShiftGearClutch || Math.Abs(EngineShaftRPM - nextClutchShaftRPM) < nonClutchRPMDiff)
            {
                transmission.Shift(value);
                return true;
            }
            transmission.Shift(0);
            return false;
        }

        public void StartEngine()
        {
            engine.SetStallRPM(350);
            engine.StartEngine();
        }

        public void StopEngine()
        {
            engine.SetStallRPM(float.MaxValue);
        }

        public float CalculateMaxMPS()
        {
            float maxEngineAngVel = engine.GetMaxRPM() * (float)Math.PI / 30.0f;
            float maxGearRatio = transmission.GetGearRatio(transmission.GetForwardGears());
            float maxDriveShaftSpeed = maxEngineAngVel / maxGearRatio; 
            return maxDriveShaftSpeed * wheel.GetRadius();
        }

        public void Tick(float dt)
        {
            int numReps = 60; // Going off of Stuntrally's game-default.cfg
            float internalDt = dt / numReps;
            for (int i = 0; i < numReps; i++)
            {
                float driveshaftTorque = UpdateDriveline(internalDt);

                // TODO:
                float tireFrictionMock = 1;
                ApplyWheelTorque(internalDt, driveshaftTorque, i, tireFrictionMock);
            }
        }

        private float UpdateDriveline(float dt)
        {
            engine.IntegrateStep1(dt);

            float driveshaftSpeed = CalculateDriveshaftSpeed();
            float clutchSpeed = transmission.CalculateClutchSpeed(driveshaftSpeed);
            float crankshaftSpeed = engine.GetAngularVelocity();
            float engineDrag = clutch.GetTorque(crankshaftSpeed, clutchSpeed);
            engineDrag += 0.1f;
            // Fixes clutch stall bug when car velocity is 0 and all wheels are in the air,
            // according to the Stuntrally dev

            engine.ComputeForces();
            engine.SetClutchTorque(transmission.GetGear() == 0 ? 0.0f : engineDrag);
            engine.ApplyForces();
            float driveshaftTorque = transmission.GetTorque(engineDrag);
            Debug.Assert(!float.IsNaN(driveshaftTorque));

            engine.IntegrateStep2(dt);

            // Update RPM
            DriveShaftRPM = driveshaftSpeed * 30.0f / (float)Math.PI;
            ClutchShaftRPM = clutchSpeed * 30.0f / (float)Math.PI;
            EngineShaftRPM = crankshaftSpeed * 30.0f / (float)Math.PI;
            DriveMPS = driveshaftSpeed * wheel.GetRadius();

            return driveshaftTorque;
        }

        private float CalculateDriveshaftSpeed()
        {
            Debug.Assert(!float.IsNaN(wheel.GetAngularVelocity()));
            return wheel.GetAngularVelocity();
        }

        private void ApplyWheelTorque(float dt, float driveTorque, int i, float tireFriction)
        {
            wheel.IntegrateStep1(dt);

            // Torques acting on wheel
            float frictionTorque = tireFriction * wheel.GetRadius();
            float wheelTorque = driveTorque - frictionTorque;
            float lockUpTorque = wheel.GetLockUpTorque(dt) - wheelTorque;
            float angVel = Math.Abs(wheel.GetAngularVelocity());
            float brakeTorque = brake.GetTorque() + wheel.fluidRes * angVel; // Fluid resistance

            // Brake and rolling resistance torque should never exceed lock up torque
            if (lockUpTorque >= 0 && lockUpTorque > brakeTorque) {
                brake.SetWillLock(false); wheelTorque += brakeTorque; // Brake torque in same dir as lock up torque
            } else if (lockUpTorque< 0 && lockUpTorque< -brakeTorque) {
                brake.SetWillLock(false); wheelTorque -= brakeTorque;
            } else {
                brake.SetWillLock(true);  wheelTorque = wheel.GetLockUpTorque(dt);
            }

            // Set wheel torque due to tire rolling resistance
            // TODO:
            // double rollRes = wheel.GetRollingResistance(wheel.GetAngularVelocity(),
            //                                             wheelContact[i].getSurface()->rollingResist);
            float rollingRegistMock = 2000;
            float rollRes = wheel.GetRollingResistance(wheel.GetAngularVelocity(), rollingRegistMock);
            float tireRollResTorque = -rollRes * wheel.GetRadius();

            wheel.SetTorque(wheelTorque * 0.5f + tireRollResTorque);
            wheel.IntegrateStep2(dt);
        }
    }
}
