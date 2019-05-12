using NUnit.Framework;

namespace Lib
{
    public class CarClutchTest
    {
        [Test]
        public void Functions()
        {
            CarClutch clutch = new CarClutch();
            Assert.AreEqual(clutch.GetTorque(100, 12312), 0, 0.0001);
            Assert.AreEqual(clutch.GetTorque(1123123100, 1.23f), 0, 0.0001);

            clutch.SetClutch(0.5f);
            Assert.AreEqual(clutch.GetTorque(10012, 6237), 168.265, 0.0001);
            Assert.AreEqual(clutch.GetTorque(0.234231f, 1.233f), -168.057866, 0.0001);

            clutch.SetMaxTorque(50);
            Assert.AreEqual(clutch.GetTorque(65, 4567), -25, 0.0001);
            Assert.AreEqual(clutch.GetTorque(0.1423f, 314159), -25, 0.0001);
        }
    }
}
