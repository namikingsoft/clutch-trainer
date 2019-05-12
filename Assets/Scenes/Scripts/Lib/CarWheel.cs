using System;
using UnityEngine;

namespace Lib
{
    public class CarWheel
    {
        //---- Constants
        private Vector3 extendedPosition = new Vector3(); // Position when suspension is fully extended (zero g)
        private float rollHeight = 0.9f; // How far off the road lateral forces are applied to the chassis
        private float mass = 18.1f;
        private RotationalFrame rotFr = new RotationalFrame(); // Simulation of wheel rotation

        private float linRollRes = 1.3e-2f; // Linear rolling resistance on hard surface
        private float quadRollRes = 6.5e-6f; // Quadratic rolling resistance on hard surface

        //---- Variables
        private float inertiaCache = 10.0f;
        private float steerAngle = 0; // Negative values = steering left!
        private float radius = 0.3f; // Total radius of wheel
        private float feedback = 0; // Effect value of force feedback

        //---- For info only
        private float angVel = 0;
        private float camberDeg = 0;

        private SlideSlip slips = new SlideSlip();

        public CarWheel()
        {
            SetInertia(10.0f);
        }

        public float GetRollingResistance(float vel, float rollResFactor)
        {
            // Surface influence on rolling resistance
            float rollRes = linRollRes * rollResFactor;

            // Heat due to tire deformation increases rolling resistance
            rollRes += vel * vel * quadRollRes;

            float res = rollRes;
            // Determine direction
            if (vel < 0) { res = -res; }

            return res;
        }

        public void SetRollingResistance(float lin, float quad)
        {
            linRollRes = lin;
            quadRollRes = quad;
        }

        public Vector3 GetExtendedPosition() { return extendedPosition; }
        public void SetExtendedPosition(Vector3 ep) { extendedPosition = ep; }

        public float GetRPM() { return rotFr.GetAngularVelocity()[0] * 30.0f / (float)Math.PI; }

        // Used for telemetry only
        public float GetAngVelInfo() { return angVel; }

        public float GetAngularVelocity() { return rotFr.GetAngularVelocity()[1]; }
        public void SetAngularVelocity(float angVel)
        {
            Vector3 v = new Vector3(0, angVel, 0);
            rotFr.SetAngularVelocity(v);
        }

        public float GetSteerAngle() { return steerAngle; }
        public void SetSteerAngle(float sa) { steerAngle = sa; }

        public void SetRadius(float value) { radius = value; }
        public float GetRadius() { return radius; }

        public void SetRollHeight(float value) { rollHeight = value; }
        public float GetRollHeight() { return rollHeight; }

        public void SetMass(float value) { mass = value; }
        public float GetMass() { return mass; }

        public void SetInertia(float i)
        {
            inertiaCache = i;

            Matrix3x3 inMat = new Matrix3x3();
            inMat.Scale(i);
            rotFr.SetInertia(inMat);
        }
        public float GetInertia() { return inertiaCache; }

        public void SetFeedback(float fb) { feedback = fb; }
        public float GetFeedback() { return feedback; }

        public void SetInitialConditions()
        {
            Vector3 v = new Vector3();
            rotFr.SetInitialTorque(v);
        }
        public void ZeroForces()
        {
            Vector3 v = new Vector3();
            rotFr.SetTorque(v);
        }

        public void IntegrateStep1(float dt) { rotFr.IntegrateStep1(dt); }
        public void IntegrateStep2(float dt) { rotFr.IntegrateStep2(dt); }

        public void SetTorque(float t)
        {
            Vector3 tv = new Vector3(0, t, 0);
            rotFr.SetTorque(tv);

            angVel = GetAngularVelocity();
        }
        public float GetTorque() { return rotFr.GetTorque()[1]; }

        public float GetLockUpTorque(float dt) { return rotFr.GetLockUpTorque(dt)[1]; }

        public Quaternion GetOrientation() { return rotFr.GetOrientation(); }

        public void SetCamberDeg(float cd) { camberDeg = cd; }
        public float GetCamberDeg() { return camberDeg; }

        internal SlideSlip Slips { get => slips; set => slips = value; }

        public float fluidRes = 0;
    }

    class SlideSlip
    {
        float slide = 0; // Ratio of tire contact patch speed to road speed, minus one
        float slip = 0; // Angle in degrees between wheel heading and actual wheel velocity
        float slideRatio = 0; // Ratio of slide to tire's optimum slide
        float slipRatio = 0; // Ratio of slip to tire's optimum slip
        float fxSr = 0, fxRsr = 0, fyAr = 0, fyRar = 0;
        float frict = 0, gamma = 0, fx = 0, fxm = 0, preFx = 0, fy = 0, fym = 0, preFy = 0, fz = 0;
    }
}
