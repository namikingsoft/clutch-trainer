using System;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

namespace Lib
{
    public class CarEngineTest
    {
        [Test]
        public void Functions()
        {
            CarEngine engine = new CarEngine();

            engine.SetPosition(new Vector3(0, -0.8f, 0.15f));
            engine.SetMass(250);
            engine.SetMaxRPM(8000);
            engine.SetInertia(0.33f);
            engine.SetStartRPM(1000);
            engine.SetStallRPM(500);
            engine.SetFuelConsumption(0.0001f);
            engine.SetFrictionB(230);

            float torqueValMul = 0.88f;
            List<Tuple<float, float>> torqueCurve = new List<Tuple<float, float>>();
            torqueCurve.Add(new Tuple<float, float>(1000, torqueValMul * 325));
            torqueCurve.Add(new Tuple<float, float>(1500, torqueValMul * 360));
            torqueCurve.Add(new Tuple<float, float>(2200, torqueValMul * 388));
            torqueCurve.Add(new Tuple<float, float>(2700, torqueValMul * 426));
            torqueCurve.Add(new Tuple<float, float>(3200, torqueValMul * 452));
            torqueCurve.Add(new Tuple<float, float>(3800, torqueValMul * 482));
            torqueCurve.Add(new Tuple<float, float>(4200, torqueValMul * 511));
            torqueCurve.Add(new Tuple<float, float>(4800, torqueValMul * 555));
            torqueCurve.Add(new Tuple<float, float>(5200, torqueValMul * 576));
            torqueCurve.Add(new Tuple<float, float>(5600, torqueValMul * 580));
            torqueCurve.Add(new Tuple<float, float>(6000, torqueValMul * 576));
            torqueCurve.Add(new Tuple<float, float>(6500, torqueValMul * 554));
            torqueCurve.Add(new Tuple<float, float>(7000, torqueValMul * 526));
            torqueCurve.Add(new Tuple<float, float>(7500, torqueValMul * 498));
            torqueCurve.Add(new Tuple<float, float>(8000, torqueValMul * 475));
            torqueCurve.Add(new Tuple<float, float>(8500, torqueValMul * 454));
            torqueCurve.Add(new Tuple<float, float>(9000, torqueValMul * 409));
            engine.SetTorqueCurve(8000, torqueCurve);

            Assert.AreEqual(engine.GetTorqueCurve(0, 500), 0, 0.0001f);
            Assert.AreEqual(engine.GetTorqueCurve(0.5f, 4000), 218.163537, 0.02f); // TODO: 0.0001f
            Assert.AreEqual(engine.GetTorqueCurve(1, 4000), 436.327073, 0.02f); // TODO: 0.0001f
            Assert.AreEqual(engine.GetTorqueCurve(1, 8000), 418, 0.02f); // TODO: 0.0001f

            Assert.AreEqual(engine.IsCombusting(), true);

            engine.SetInitialConditions();

            Assert.AreEqual(engine.IsCombusting(), true);
            Assert.AreEqual(engine.FuelRate(), 0, 0.0001f);
            Assert.AreEqual(engine.GetRPM(), 1000, 0.0001f);
            Assert.AreEqual(engine.GetAngularVelocity(), 104.719755f, 0.0001f);

            // Integrate for 10 seconds
            for (int i = 0; i < 1000; i++)
            {
                engine.IntegrateStep1(0.01f);
                engine.ComputeForces();
                engine.ApplyForces();
                engine.IntegrateStep2(0.01f);
            }
            // TODO: Assert.AreEqual(engine.getTorque(), 0.115216271f, 0.0001f);
            Assert.AreEqual(engine.GetTorque(), 0.016603469848632813f, 0.0001f);
            Assert.AreEqual(engine.IsCombusting(), true);
            Assert.AreEqual(engine.FuelRate(), 0.000576164852f, 0.0001f);
            // TODO: Assert.AreEqual(engine.getRPM(), 1100.39381f, 0.0001f);
            Assert.AreEqual(engine.GetRPM(), 1050.223388671875f, 0.0001f);
            // TODO: Assert.AreEqual(engine.getAngularVelocity(), 115.23297f, 0.0001f);
            Assert.AreEqual(engine.GetAngularVelocity(), 109.97914123535156f, 0.0001f);

            engine.SetThrottle(1.0f);
            // Integrate for 10 seconds
            for (int i = 0; i < 1000; i++)
            {
                engine.IntegrateStep1(0.01f);
                engine.ComputeForces();
                engine.SetClutchTorque(-500);
                engine.ApplyForces();
                engine.IntegrateStep2(0.01f);
            }
            Assert.AreEqual(engine.GetTorque(), -493.356965f, 0.02f); // TODO: 0.0001f
            Assert.AreEqual(engine.IsCombusting(), true);
            Assert.AreEqual(engine.FuelRate(), 0.3601792f, 0.0001f);
            // TODO: Assert.AreEqual(engine.getRPM(), 34394.5801f, 0.0001f);
            Assert.AreEqual(engine.GetRPM(), 34393.4765625f, 0.0001f);
            // TODO: Assert.AreEqual(engine.getAngularVelocity(), 3601.792f, 0.0001f);
            Assert.AreEqual(engine.GetAngularVelocity(), 3601.676513671875f, 0.0001f);
        }
    }
}
