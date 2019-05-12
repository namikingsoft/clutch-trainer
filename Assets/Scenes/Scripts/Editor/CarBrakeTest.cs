using NUnit.Framework;

namespace Lib
{
    public class CarBrakeTest
    {
        [Test]
        public void Functions()
        {
            CarBrake brake = new CarBrake();
            Assert.AreEqual(brake.GetTorque(), 0);

            brake.SetBrakeFactor(1.0f);
            Assert.AreEqual(brake.GetTorque(), 3066, 0.0001f);

            brake.SetHandbrakeFactor(1.0f);
            Assert.AreEqual(brake.GetTorque(), 3066, 0.0001f);

            brake.SetHandbrake(2.6f);
            Assert.AreEqual(brake.GetTorque(), 7971.59971f, 0.01f); // TODO: 0.0001f

            brake.SetBrakeFactor(0);
            Assert.AreEqual(brake.GetTorque(), 7971.59971f, 0.01f); // TODO: 0.0001f

            brake.SetHandbrakeFactor(0);
            Assert.AreEqual(brake.GetTorque(), 0);
        }
    }
}
