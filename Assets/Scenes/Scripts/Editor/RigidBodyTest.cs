using System;
using UnityEngine;
using NUnit.Framework;

namespace Lib
{
    public class RigidBodyTest
    {
        [Test]
        public void Functions()
        {
            RigidBody body = new RigidBody();

            Vector3 initPos = new Vector3(0, 0, 10);
            body.SetPosition(initPos);
            Quaternion quat = new Quaternion();
            quat.Rotate(-(float)Math.PI * 0.5f, 1, 0, 0);
            body.SetOrientation(quat);

            Vector3 localCoords = new Vector3(0, 0, 1);
            Vector3 expected = new Vector3(0, 1, 10);
            Vector3 pos = body.TransformLocalToWorld(localCoords);

            Assert.AreEqual(pos[0], expected[0], 0.0001f);
            Assert.AreEqual(pos[1], expected[1], 0.0001f);
            Assert.AreEqual(pos[2], expected[2], 0.0001f);

            Assert.AreEqual(body.TransformWorldToLocal(pos)[0], localCoords[0], 0.0001f);
            Assert.AreEqual(body.TransformWorldToLocal(pos)[1], localCoords[1], 0.0001f);
            Assert.AreEqual(body.TransformWorldToLocal(pos)[2], localCoords[2], 0.0001f);
        }
    }
}
