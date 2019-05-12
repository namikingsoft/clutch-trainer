using UnityEngine;
using NUnit.Framework;

namespace Lib
{
    public class LinearFrameTest
    {
        [Test]
        public void Functions()
        {
            LinearFrame frame = new LinearFrame();
            frame.SetMass(1.0f);
            Vector3 initPos = new Vector3();
            frame.SetPosition(initPos);
            Vector3 initV = new Vector3(0, 65, 0);
            frame.SetVelocity(initV);
            Vector3 gravity = new Vector3(0, -9.81f, 0);
            frame.SetInitialForce(gravity);

            float t = 0.0f;
            // Integrate for 10 seconds
            for (int i = 0; i < 1000; i++)
            {
                frame.IntegrateStep1(0.01f);
                frame.ApplyForce(gravity);
                frame.IntegrateStep2(0.01f);
                t += 0.01f;
            }
            Assert.AreEqual(frame.GetPosition()[1], (initV * t + gravity * t * t * 0.5f)[1], 0.02f); // TODO: 0.0001f

            frame.SetMass(1.0f);
            initPos.Set(0, 0, 0);
            frame.SetPosition(initPos);
            initV.Set(0, 0, 0);
            frame.SetVelocity(initV);
            Vector3 force = new Vector3();
            frame.SetInitialForce(force);

            // Integrate for 10 seconds
            for (int i = 0; i < 1000; i++)
            {
                frame.IntegrateStep1(0.01f);

                force.Set(0, 1, 0);
                force = force - frame.GetVelocity() * 10.0f;
                frame.ApplyForce(force);

                frame.IntegrateStep2(0.01f);
            }

            Assert.AreEqual(frame.GetVelocity()[1], 0.1f, 0.0001f);
        }
    }
}
