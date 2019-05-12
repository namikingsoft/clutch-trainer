using NUnit.Framework;

namespace Lib
{
    public class CarDifferentialTest
    {
        [Test]
        public void Functions()
        {
            CarDifferential differential = new CarDifferential();

            Assert.AreEqual(differential.GetDriveshaftSpeed(), 0, 0.0001f);
            Assert.AreEqual(differential.GetFinalDrive(), 4.1f, 0.0001f);
            Assert.AreEqual(differential.CalculateDriveshaftSpeed(50, 40), 184.5f, 0.0001f);

            differential.ComputeWheelTorques(150);
            Assert.AreEqual(differential.GetSide1Speed(), 50, 0.0001f);
            Assert.AreEqual(differential.GetSide2Torque(), 907.5f, 0.0001f);

            differential.SetFinalDrive(4.54f);
            differential.SetAntiSlip(600.0f, 0, 0);

            Assert.AreEqual(differential.GetDriveshaftSpeed(), 204.3f, 0.0001f);
            Assert.AreEqual(differential.GetFinalDrive(), 4.54f, 0.0001f);
            Assert.AreEqual(differential.CalculateDriveshaftSpeed(60, 70), 295.1f, 0.0001f);

            differential.ComputeWheelTorques(200);
            Assert.AreEqual(differential.GetSide2Speed(), 70, 0.0001f);
            Assert.AreEqual(differential.GetSide1Torque(), 1054, 0.0001f);
        }
    }
}
