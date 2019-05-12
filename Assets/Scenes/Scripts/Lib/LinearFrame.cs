using System;
using UnityEngine;

namespace Lib
{
    public class LinearFrame
    {
        //---- Primary
        private Vector3 pos = new Vector3();
        private Vector3 mom = new Vector3(); // Momentum
        private Vector3 force = new Vector3();

        //---- Secondary
        private Vector3 oldForce = new Vector3();

        //----- Constants
        private float invMass = 1.0f;

        //---- Housekeeping
        private bool haveOldForce = false;
        private int integrationStep = 0;

        public void SetMass(float mass) { invMass = 1.0f / mass; }
        public float GetMass() { return 1.0f / invMass; }

        public void SetPosition(Vector3 newPos) { pos = newPos; }
        public Vector3 GetPosition() { return pos; }

        public void SetVelocity(Vector3 newVel) { mom = newVel / invMass; }
        public Vector3 GetVelocity() { return GetVelocityFromMomentum(mom); }

        //---- Modified velocity Verlet integration two-step method
        //---- Both steps must be run per frame. Forces can only be set between steps 1 and 2
        public void IntegrateStep1(float dt)
        {
            Debug.Assert(integrationStep == 0);
            Debug.Assert(haveOldForce); // Call setInitialForce() on first-time integration

            integrationStep++;
        }

        public void IntegrateStep2(float dt)
        {
            Debug.Assert(integrationStep == 1);

            pos = pos + mom * invMass * dt + force * invMass * dt * dt * 0.5f;
            mom = mom + force * dt;

            RecalculateSecondary();

            integrationStep = 0;
            force.Set(0, 0, 0);
        }

        //---- Must only be called between integration steps 1 and 2
        public void ApplyForce(Vector3 f)
        {
            Debug.Assert(integrationStep == 1);
            force = force + f;
        }
        public void SetForce(Vector3 f)
        {
            Debug.Assert(integrationStep == 1);
            force = f;
        }

        //---- Must be called once at simulation start to set initial force
        public void SetInitialForce(Vector3 f)
        {
            Debug.Assert(integrationStep == 0);
            oldForce = f;
            haveOldForce = true;
        }

        public Vector3 GetForce() { return oldForce; }

        private void RecalculateSecondary()
        {
            oldForce = force;
            haveOldForce = true;
        }

        private Vector3 GetVelocityFromMomentum(Vector3 mom)
        {
            return mom * invMass;
        }
    }
}
