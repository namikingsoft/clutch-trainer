using NUnit.Framework;

namespace Lib
{
    public class CarDynamicsTest
    {
        [Test]
        public void Functions1()
        {
            CarDynamics dynamics = new CarDynamics();
            dynamics.StartEngine();

            Assert.AreEqual(dynamics.EngineShaftRPM, 1000, 0.0001f);

            for (int i = 0; i < 100; i++)
            {
                dynamics.Tick(0.1f);
            }

            Assert.AreEqual(dynamics.EngineShaftRPM, 1036.3673095703125f, 0.0001f);

            dynamics.SetThrottle(1);

            for (int i = 0; i < 10; i++)
            {
                dynamics.Tick(0.1f);
            }

            Assert.AreEqual(dynamics.EngineShaftRPM, 4564.10205078125f, 0.0001f);

            dynamics.ShiftGear(1);

            for (int i = 0; i < 100; i++)
            {
                dynamics.Tick(0.1f);
            }

            Assert.AreEqual(dynamics.EngineShaftRPM, 8000, 100);
        }

        [Test]
        public void Stalled()
        {
            CarDynamics dynamics = new CarDynamics();

            dynamics.SetThrottle(1);
            dynamics.ShiftGear(1);
            dynamics.SetClutch(1);

            for (int i = 0; i < 2; i++)
            {
                dynamics.Tick(0.1f);
            }

            dynamics.SetThrottle(0);

            for (int i = 0; i < 19; i++)
            {
                dynamics.Tick(0.1f);
            }

            Assert.AreEqual(dynamics.EngineShaftRPM, 8000, 100);
            Assert.AreEqual(dynamics.DriveShaftRPM, 4510.36962890625f, 0.0001f);
        }
    }
}
