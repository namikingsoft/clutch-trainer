using NUnit.Framework;

namespace Lib
{
    public class CarTransmissionTest
    {
        [Test]
        public void Functions()
        {
            CarTransmission transmission = new CarTransmission();

            transmission.SetGearRatio(-1, -3.29f);
            transmission.SetGearRatio(1, 3.29f);
            transmission.SetGearRatio(2, 2.16f);
            transmission.SetGearRatio(3, 1.61f);
            transmission.SetGearRatio(4, 1.27f);
            transmission.SetGearRatio(5, 1.03f);
            transmission.SetGearRatio(6, 0.85f);

            Assert.AreEqual(transmission.GetForwardGears(), 6);
            Assert.AreEqual(transmission.GetReverseGears(), 1);
            Assert.AreEqual(transmission.GetGear(), 0);
            Assert.AreEqual(transmission.GetCurrentGearRatio(), 0);

            transmission.Shift(4);
            Assert.AreEqual(transmission.GetGear(), 4);
            Assert.AreEqual(transmission.GetCurrentGearRatio(), 1.27f, 0.0001f);
            Assert.AreEqual(transmission.CalculateClutchSpeed(100), 127, 0.0001f);
            Assert.AreEqual(transmission.GetClutchSpeed(50), 63.5f, 0.0001f);
            Assert.AreEqual(transmission.GetTorque(20), 25.4f, 0.0001f);

            transmission.Shift(-2);
            Assert.AreEqual(transmission.GetGear(), 4);
            Assert.AreEqual(transmission.GetCurrentGearRatio(), 1.27f, 0.0001f);

            transmission.Shift(-1);
            Assert.AreEqual(transmission.GetGear(), -1);
            Assert.AreEqual(transmission.GetCurrentGearRatio(), -3.29f, 0.0001f);
            Assert.AreEqual(transmission.CalculateClutchSpeed(50), -164.5f, 0.0001f);
            Assert.AreEqual(transmission.GetClutchSpeed(100), -329, 0.0001f);
            Assert.AreEqual(transmission.GetTorque(2), -6.58f, 0.0001f);
        }
    }
}
