using UnityEngine;
using NUnit.Framework;

namespace Lib
{
    public class CarWheelTest
    {
        [Test]
        public void Functions()
        {
            CarWheel wheel = new CarWheel();
            wheel.SetInitialConditions();

            // Integrate for 10 seconds
            for (int i = 0; i < 1000; i++)
            {
                wheel.IntegrateStep1(0.01f);
                wheel.SetTorque(100);
                wheel.IntegrateStep2(0.01f);
            }
            Vector3 zero = new Vector3();
            Assert.AreEqual(wheel.GetAngularVelocity(), 100, 0.0001f);
            Assert.AreEqual(wheel.GetExtendedPosition(), zero);
            Assert.AreEqual(wheel.GetInertia(), 10, 0.0001f);
            Assert.AreEqual(wheel.GetMass(), 18.1f, 0.0001f);
            Assert.AreEqual(wheel.GetTorque(), 100, 0.0001f);
            Assert.AreEqual(wheel.GetRPM(), 0, 0.0001f);

            Quaternion orient = wheel.GetOrientation();
            Assert.AreEqual(orient.X(), 0, 0.0001f);
            Assert.AreEqual(orient.Y(), 0.985206093f, 0.0001f);
            Assert.AreEqual(orient.Z(), 0, 0.0001f);
            Assert.AreEqual(orient.W(), -0.171373725f, 0.0001f);

            wheel.SetCamberDeg(2);
            wheel.SetExtendedPosition(new Vector3(0.8345f, 1.12f, -0.37f));
            wheel.SetMass(15.0f);
            wheel.SetRadius(0.32f); wheel.SetRollHeight(0.92f);
            wheel.SetRollingResistance(1.3e-2f, 6.5e-5f);
            wheel.SetSteerAngle(5);

            // Integrate for 10 seconds
            for (int i = 0; i < 1000; i++)
            {
                wheel.IntegrateStep1(0.01f);
                wheel.SetTorque(wheel.GetLockUpTorque(0.01f) * 0.5f);
                wheel.IntegrateStep2(0.01f);
            }
            Vector3 expected = new Vector3(0.8345f, 1.12f, -0.37f);
            Vector3 actual = wheel.GetExtendedPosition();
            Assert.AreEqual(wheel.GetAngularVelocity(), 9.33263619e-300f, 0.0001f);
            Assert.AreEqual(actual[0], expected[0], 0.0001f);
            Assert.AreEqual(actual[1], expected[1], 0.0001f);
            Assert.AreEqual(actual[2], expected[2], 0.0001f);
            Assert.AreEqual(wheel.GetInertia(), 10, 0.0001f);
            Assert.AreEqual(wheel.GetMass(), 15, 0.0001f);
            Assert.AreEqual(wheel.GetTorque(), -9.33263619e-297f, 0.0001f);
            Assert.AreEqual(wheel.GetRPM(), 0, 0.0001f);

            orient = wheel.GetOrientation();
            Assert.AreEqual(orient.X(), 0, 0.0001f);
            Assert.AreEqual(orient.Y(), 0.618840906f, 0.0001f);
            Assert.AreEqual(orient.Z(), 0, 0.0001f);
            Assert.AreEqual(orient.W(), -0.785516348f, 0.0001f);
        }
    }
}
