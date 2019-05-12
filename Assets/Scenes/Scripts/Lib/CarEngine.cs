using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lib
{
    public class CarEngine
    {
        //---- Constants
        private float maxRPM = 7800; // The "red line" in RPM
        private float idle = 0.02f; // Idle throttle percentage; calculated algorithmically
        private float startRPM = 1000; // Initial RPM
        private float stallRPM = 350; // RPM at which engine dies
        private float fuelConsumption = (float)1e-9; // fuelConsumption * RPM * throttle = liters of fuel consumer per second
        private float friction = 0.000328f; // Friction coefficient from engine; calculated algorithmically
        private float fricCoeffB = 230;

        private float mass = 200;
        private Vector3 position = new Vector3();
        private Spline torqueCurve = new Spline();

        //---- Variables
        private float throttlePosition = 0.0f;
        private float clutchTorque = 0.0f;
        private bool outOfFuel = false;
        private bool revLimitExceeded = false;
        private RotationalFrame crankshaft = new RotationalFrame();

        //---- For info only
        private float frictionTorque = 0;
        private float combustionTorque = 0;
        private bool stalled = false;

        public void SetInertia(float i)
        {
            Matrix3x3 inertia = new Matrix3x3();
            inertia.Scale(i);
            crankshaft.SetInertia(inertia);
        }

        public float GetInertia() { return crankshaft.GetInertia()[0]; }

        public void SetFrictionB(float val) { fricCoeffB = val; }
        public float GetFrictionB() { return fricCoeffB; }

        public void SetMaxRPM(float val) { maxRPM = val; }
        public float GetMaxRPM() { return maxRPM; }

        public float GetIdle() { return idle; }

        public void SetStartRPM(float val) { startRPM = val; }
        public float GetStartRPM() { return startRPM; }

        public void SetStallRPM(float val) { stallRPM = val; }
        public float GetStallRPM() { return stallRPM; }

        public void SetFuelConsumption(float val) { fuelConsumption = val; }
        public float GetFuelConsumption() { return fuelConsumption; }

        public void IntegrateStep1(float dt) { crankshaft.IntegrateStep1(dt); }

        public void IntegrateStep2(float dt) { crankshaft.IntegrateStep2(dt); }

        public float GetRPM() { return crankshaft.GetAngularVelocity()[0] * 30 / (float)Math.PI; }

        // 0.0 = no throttle; 1.0 = full throttle;
        public void SetThrottle(float val) { throttlePosition = val; }
        public float GetThrottle() { return throttlePosition; }

        public void SetInitialConditions()
        {
            Vector3 v = new Vector3();
            crankshaft.SetInitialTorque(v);
            // StartEngine();
        }

        public void StartEngine()
        {
            Vector3 v = new Vector3();
            v[0] = startRPM * (float)Math.PI / 30.0f;
            crankshaft.SetAngularVelocity(v);
        }

        // Used to set engine drag from clutch being partially engaged
        public void SetClutchTorque(float val) { clutchTorque = val; }

        public float GetAngularVelocity() { return crankshaft.GetAngularVelocity()[0]; }
        public void SetAngularVelocity(float val)
        {
            Vector3 v = new Vector3(val, 0, 0);
            crankshaft.SetAngularVelocity(v);
        }

        // Sum of all torque (except clutch forces) acting on engine)
        public float GetTorque() { return combustionTorque + frictionTorque; }


        public void SetMass(float val) { mass = val; }
        public float GetMass() { return mass; }

        public void SetPosition(Vector3 value) { position = value; }
        public Vector3 GetPosition() { return position; }

        public float FuelRate() { return fuelConsumption* GetAngularVelocity() * throttlePosition; }

        public void SetOutOfFuel(bool value) { outOfFuel = value; }

        // True if engine is combusting fuel
        public bool IsCombusting() { return !stalled; }

        public float GetTorqueCurve(float curThrottle, float curRPM)
        {
            if (curRPM < 1) { return 0.0f; }

            float torque = torqueCurve.Interpolate(curRPM);
            return torque * curThrottle;
        }

        public float GetFrictionTorque(float curAngVel, float fricFactor, float throttlePos)
        {
            float b = fricCoeffB * friction;
            return (-curAngVel * b) * (1.0f - fricFactor * throttlePos);
        }

        public void ComputeForces()
        {
            stalled = GetRPM() < stallRPM;

            if (throttlePosition < idle) { throttlePosition = idle; } // Must be at least idle

            float curAngVel = crankshaft.GetAngularVelocity()[0];

            float fricFactor = 1.0f; // Used to make sure friction works even when out of fuel
            float revLimit = maxRPM + 500;
            if (revLimitExceeded) { revLimit -= 400; }

            revLimitExceeded = GetRPM() >= revLimit;

            combustionTorque = GetTorqueCurve(throttlePosition, GetRPM());

            if (outOfFuel || revLimitExceeded || stalled)
            {
                fricFactor = 0.0f;
                combustionTorque = 0.0f;
            }

            frictionTorque = GetFrictionTorque(curAngVel, fricFactor, throttlePosition);
            if (stalled) { frictionTorque *= 100.0f; } // Try to mimic engine static friction
        }

        public void ApplyForces()
        {
            Vector3 totalTorque = new Vector3();
            totalTorque[0] += combustionTorque + frictionTorque - clutchTorque;

            crankshaft.SetTorque(totalTorque);
        }


        // Set torque curve using vector of (RPM, torque) pairs.
        // Also recalculate engine friction.
        // maxPowerRPM should be set to engine red line
        public void SetTorqueCurve(float maxPowerRPM, List<Tuple<float, float>> curve)
        {
            torqueCurve.clear();

            // Dyno correction factor removed because it equaled 1

            Debug.Assert(curve.Count > 1);

            //if (curve[0].first != 0)
            if (Math.Abs(curve[0].Item1) < float.Epsilon)
            {
                torqueCurve.addPoint(0, 0); // Want to always start from 0 RPM
            }

            foreach (Tuple<float, float> i in curve)
            {
                torqueCurve.addPoint(i.Item1, i.Item2);
            }

            // Ensure we have a smooth curve for over-revs
            torqueCurve.addPoint(curve[curve.Count - 1].Item1 + 10000, 0);

            float maxPowerAngVel = maxPowerRPM * (float)Math.PI / 30.0f;
            float maxPower = torqueCurve.Interpolate(maxPowerRPM) * maxPowerAngVel;
            friction = maxPower / (maxPowerAngVel * maxPowerAngVel * maxPowerAngVel);

            // Calculate idle throttle position
            for (idle = 0; idle < 1.0f; idle += 0.01f)
            {
                if (GetTorqueCurve(idle, startRPM) >
                    -GetFrictionTorque(startRPM * (float)Math.PI / 30.0f, 1.0f, idle))
                {
                    break;
                }
            }
        }
    }
}
