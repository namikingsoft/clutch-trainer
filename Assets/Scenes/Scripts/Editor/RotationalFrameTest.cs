using UnityEngine;
using NUnit.Framework;

namespace Lib
{
    public class RotationalFrameTest
    {
        [Test]
        public void Integration1()
        {
            RotationalFrame frame = new RotationalFrame();

            Quaternion initOrient = new Quaternion();
            frame.SetOrientation(initOrient);
            Vector3 initV = new Vector3(0, 0, 0);
            frame.SetAngularVelocity(initV);
            Vector3 torque = new Vector3(0, 0, 0);
            frame.SetInitialTorque(torque);

            // Integrate for 10 seconds
            for (int i = 0; i < 1000; i++)
            {
                frame.IntegrateStep1(0.01f);

                torque.Set(0, 1, 0);
                torque = torque - frame.GetAngularVelocity() * 10.0f;
                frame.ApplyTorque(torque);

                frame.IntegrateStep2(0.01f);
            }

            Assert.AreEqual(frame.GetAngularVelocity()[1], 0.1f, 0.0001f);
        }

        [Test]
        public void Integration2()
        {
            RotationalFrame frame = new RotationalFrame();

            Quaternion initOrient = new Quaternion();
            frame.SetOrientation(initOrient);
            Vector3 initV = new Vector3(0, 0, 0);
            frame.SetAngularVelocity(initV);
            Vector3 torque = new Vector3(0, 1, 0);
            frame.SetInitialTorque(torque);
            Matrix3x3 inertia = new Matrix3x3(); 
            inertia.Scale(0.1f);
            frame.SetInertia(inertia);

            // Integrate for 10 seconds
            for (int i = 0; i < 1000; i++)
            {
                frame.IntegrateStep1(0.01f);
                frame.ApplyTorque(torque);
                frame.IntegrateStep2(0.01f);
            }

            Assert.AreEqual(frame.GetAngularVelocity()[1], 100.0f, 0.002f); // TODO: 0.0001f
        }
    }
}