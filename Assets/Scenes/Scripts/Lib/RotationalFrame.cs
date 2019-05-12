using UnityEngine;

namespace Lib
{
    public class RotationalFrame
    {
        // Primary
        private Quaternion orientation = new Quaternion();
        private Vector3 angMom = new Vector3();
        private Vector3 torque = new Vector3();

        // Secondary
        private Vector3 oldTorque = new Vector3();
        private Matrix3x3 orientationMat = new Matrix3x3();
        private Matrix3x3 worldInvInertiaTensor = new Matrix3x3();
        private Matrix3x3 worldInertiaTensor = new Matrix3x3();
        private Vector3 angVel = new Vector3();

        // Constants
        private Matrix3x3 invInertiaTensor = new Matrix3x3();
        private Matrix3x3 inertiaTensor = new Matrix3x3();

        // Housekeeping
        private bool haveOldTorque = false;
        private int integrationStep = 0;

        public void SetInertia(Matrix3x3 inertia) 
        {
            Vector3 avOld = GetAngularVelocityFromMomentum(angMom);

            inertiaTensor = inertia;
            invInertiaTensor = inertiaTensor.Inverse();

            worldInvInertiaTensor = orientationMat
                .Transpose()
                .Multiply(invInertiaTensor)
                .Multiply(orientationMat);
            worldInertiaTensor = orientationMat
                .Transpose()
                .Multiply(inertiaTensor)
                .Multiply(orientationMat);

            angMom = worldInertiaTensor.Multiply(avOld);
            angVel = GetAngularVelocityFromMomentum(angMom);
        }

        public Matrix3x3 GetInertia() { return worldInertiaTensor; }
        public Matrix3x3 GetInertiaLocal() { return inertiaTensor; }

        public void SetOrientation(Quaternion no) { orientation = no; }
        public Quaternion GetOrientation() { return orientation; }

        public void SetAngularVelocity(Vector3 nav)
        {
            angMom = worldInertiaTensor.Multiply(nav);
            angVel = nav;
        }

        public Vector3 GetAngularVelocity() { return angVel; }

        // Modified velocity Verlet integration two-step method
        // Both steps must be called per frame
        // Forces can only be set between steps 1 and 2
        public void IntegrateStep1(float dt)
        {
            Debug.Assert(integrationStep == 0);
            Debug.Assert(haveOldTorque); // Must call setInitialTorque()
            integrationStep++;
        }

        public void IntegrateStep2(float dt)
        {
            Debug.Assert(integrationStep == 1);

            orientation = orientation + GetSpinFromMomentum(angMom + torque * dt * 0.5f) * dt;
            orientation.Normalize();
            angMom = angMom + torque * dt;

            RecalculateSecondary();
            integrationStep = 0;
            torque.Set(0, 0, 0);
        }

        // Must only be called between integration steps 1 and 2
        public Vector3 GetLockUpTorque(float dt)
        {
            Debug.Assert(integrationStep == 1);
            return -angMom / dt;
        }

        public void ApplyTorque(Vector3 t)
        {
            Debug.Assert(integrationStep == 1);
            torque = torque + t;
        }

        public void SetTorque(Vector3 t)
        {
            Debug.Assert(integrationStep == 1);
            torque = t;
        }

        public Vector3 GetTorque() { return oldTorque; }

        // Must be called once when simulation starts to set initial torque
        public void SetInitialTorque(Vector3 t)
        {
            Debug.Assert(integrationStep == 0);

            oldTorque = t;
            haveOldTorque = true;
        }

        private void RecalculateSecondary()
        {
            oldTorque = torque;
            haveOldTorque = true;

            orientation.RepresentAsMatrix3(orientationMat);

            worldInvInertiaTensor = orientationMat
                .Transpose()
                .Multiply(invInertiaTensor)
                .Multiply(orientationMat);
            worldInertiaTensor = orientationMat
                .Transpose()
                .Multiply(inertiaTensor)
                .Multiply(orientationMat);

            angVel = GetAngularVelocityFromMomentum(angMom);
        }

        // orientationMat and worldInvIntertiaTensor must have been calculated
        Vector3 GetAngularVelocityFromMomentum(Vector3 mom)
        {
            return worldInvInertiaTensor.Multiply(mom);
        }

        Quaternion GetSpinFromMomentum(Vector3 am)
        {
            Vector3 av = GetAngularVelocityFromMomentum(am);
            Quaternion qav = new Quaternion(av[0], av[1], av[2], 0);
            return (qav * orientation) * 0.5f;
        }
    }
}