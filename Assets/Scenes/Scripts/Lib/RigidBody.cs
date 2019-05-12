using System;
using UnityEngine;

namespace Lib
{
    public class RigidBody
    {
        private LinearFrame linFr = new LinearFrame();
        private RotationalFrame rotFr = new RotationalFrame();

        //---- Accessor methods to LinearFrame
        public void SetInitialForce(Vector3 f) { linFr.SetInitialForce(f); }
        public void SetForce(Vector3 f) { linFr.SetForce(f); }
        public Vector3 GetForce() { return linFr.GetForce(); }

        public void SetMass(float mass) { linFr.SetMass(mass); }
        public float GetMass() { return linFr.GetMass(); }

        public void SetPosition(Vector3 pos) { linFr.SetPosition(pos); }
        public Vector3 GetPosition() { return linFr.GetPosition(); }

        public void SetVelocity(Vector3 vel) { linFr.SetVelocity(vel); }
        public Vector3 GetVelocity() { return linFr.GetVelocity(); }
        public Vector3 GetVelocity(Vector3 offset)
        {
            // return linFr.getVelocity() + rotFr.getAngularVelocity().cross(offset);
            Vector3 v = rotFr.GetAngularVelocity();
            return linFr.GetVelocity() + new Vector3(
                v[1] * offset[2] - v[2] * offset[1],
                v[2] * offset[0] - v[0] * offset[2],
                v[0] * offset[1] - v[1] * offset[0]);
        }

        //---- Accessor methods to RotationalFrame
        public void SetInitialTorque(Vector3 t) { rotFr.SetInitialTorque(t); }
        public void SetTorque(Vector3 t) { rotFr.SetTorque(t); }
        public Vector3 GetTorque() { return rotFr.GetTorque(); }

        public void SetInertia(Matrix3x3 i) { rotFr.SetInertia(i); }
        public Matrix3x3 GetInertia() { return rotFr.GetInertia(); }
        public Matrix3x3 GetInertiaLocal() { return rotFr.GetInertiaLocal(); }

        public void SetOrientation(Quaternion o) { rotFr.SetOrientation(o); }
        public Quaternion GetOrientation() { return rotFr.GetOrientation(); }

        public void SetAngularVelocity(Vector3 nav) { rotFr.SetAngularVelocity(nav); }
        public Vector3 GetAngularVelocity() { return rotFr.GetAngularVelocity(); }

        //---- Make sure to set the required forces/torques in between integration steps
        public void IntegrateStep1(float dt)
        {
            linFr.IntegrateStep1(dt);
            rotFr.IntegrateStep1(dt);
        }
        public void IntegrateStep2(float dt)
        {
            linFr.IntegrateStep2(dt);
            rotFr.IntegrateStep2(dt);
        }

        public Vector3 TransformLocalToWorld(Vector3 localPt)
        {
            // TODO: change to mutable?
            Vector3 output = GetOrientation().RotateVector(localPt);
            output = output + GetPosition();

            return output;
        }
        public Vector3 TransformWorldToLocal(Vector3 worldPt)
        {
            Vector3 output = new Vector3(worldPt.x, worldPt.y, worldPt.z);

            output = output - GetPosition();
            return (-GetOrientation()).RotateVector(output);
        }

        // Apply force in world space
        public void ApplyForce(Vector3 force) { linFr.ApplyForce(force); }

        // Apply force at offset from center of mass in world space
        public void ApplyForce(Vector3 force, Vector3 offset)
        {
            linFr.ApplyForce(force);
            // rotFr.applyTorque(offset.cross(force));
            rotFr.ApplyTorque(new Vector3(
                offset[1] * force[2] - offset[2] * force[1],
                offset[2] * force[0] - offset[0] * force[2],
                offset[0] * force[1] - offset[1] * force[0]));
        }

        // Apply torque in world space
        public void ApplyTorque(Vector3 torque) { rotFr.ApplyTorque(torque); }
    }
}
