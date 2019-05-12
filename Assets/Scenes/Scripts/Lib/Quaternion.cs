using System;
using UnityEngine;

namespace Lib
{
    public class Quaternion
    {
        private float[] v = new float[4];

        public Quaternion()
        {
            LoadIdentity();
        }

        public Quaternion(float x, float y, float z, float w) {
            v[0] = x;
            v[1] = y;
            v[2] = z;
            v[3] = w;
        }

        public float this[int i]
        {
            get { return v[i]; }
            set { v[i] = value; }
        }

        public static Quaternion operator +(Quaternion a, Quaternion b)
        {
            return new Quaternion(a[0] + b[0], a[1] + b[1], a[2] + b[2], a[3] + b[3]);
        }

        // The conjugate
        public static Quaternion operator -(Quaternion a)
        {
            return new Quaternion(-a[0], -a[1], -a[2], a[3]);
        }

        public static Quaternion operator *(Quaternion a, float b)
        {
            return new Quaternion(a[0] * b, a[1] * b, a[2] * b, a[3] * b);
        }

        public static Quaternion operator *(Quaternion a, Quaternion b)
        {
            float A, B, C, D, E, F, G, H;

            A = (a[3] + a[0]) * (b[3] + b[0]);
            B = (a[2] - a[1]) * (b[1] - b[2]);
            C = (a[3] - a[0]) * (b[1] + b[2]);
            D = (a[1] + a[2]) * (b[3] - b[0]);
            E = (a[0] + a[2]) * (b[0] + b[1]);
            F = (a[0] - a[2]) * (b[0] - b[1]);
            G = (a[3] + a[1]) * (b[3] - b[2]);
            H = (a[3] - a[1]) * (b[3] + b[2]);

            return new Quaternion(
                A - (E + F + G + H) * 0.5f,
                C + (E - F + G - H) * 0.5f,
                D + (E - F - G + H) * 0.5f,
                B + (-E - F + G + H) * 0.5f);
        }

        public float X() { return v[0]; }
        public float Y() { return v[1]; }
        public float Z() { return v[2]; }
        public float W() { return v[3]; }

        public void LoadIdentity()
        {
            v[3] = 1;
            v[0] = v[1] = v[2] = 0;
        }

        public void SetAxisAngle(float a, float ax, float ay, float az)
        {
            float sina2 = (float)Math.Sin(a / 2);

            v[3] = (float)Math.Cos(a / 2);
            v[0] = ax * sina2;
            v[1] = ay * sina2;
            v[2] = az * sina2;
        }

        public void Rotate(float a, float ax, float ay, float az)
        {
            Quaternion output = new Quaternion();
            output.SetAxisAngle(a, ax, ay, az);

            Quaternion result = output * this;
            v[0] = result[0];
            v[1] = result[1];
            v[2] = result[2];
            v[3] = result[3];
            Normalize();
        }

        public Vector3 RotateVector(Vector3 vec)
        {
            Quaternion dirconj = -this;
            Quaternion qtemp = new Quaternion();
            qtemp[3] = 0;
            for (int i = 0; i < 3; ++i)
            {
                qtemp[i] = vec[i];
            }

            Quaternion qout = this * qtemp * dirconj;

            // TODO: change to mutable?
            // for (size_t i = 0; i < 3; ++i) { vec[i] = qout.v[i]; }
            return new Vector3(qout[0], qout[1], qout[2]);

        }

        public void RepresentAsMatrix3(Matrix3x3 destMat)
        {
            float xx = v[0] * v[0];
            float xy = v[0] * v[1];
            float xz = v[0] * v[2];
            float xw = v[0] * v[3];

            float yy = v[1] * v[1];
            float yz = v[1] * v[2];
            float yw = v[1] * v[3];

            float zz = v[2] * v[2];
            float zw = v[2] * v[3];

            destMat[0] = 1.0f - 2.0f * (yy + zz);
            destMat[1] = 2.0f * (xy + zw);
            destMat[2] = 2.0f * (xz - yw);

            destMat[3] = 2.0f * (xy - zw);
            destMat[4] = 1.0f - 2.0f * (xx + zz);
            destMat[5] = 2.0f * (yz + xw);

            destMat[6] = 2.0f * (xz + yw);
            destMat[7] = 2.0f * (yz - xw);
            destMat[8] = 1.0f - 2.0f * (xx + yy);
        }

        public float Magnitude()
        {
            return (float)Math.Sqrt((double)(v[3] * v[3] + v[0] * v[0] + v[1] * v[1] + v[2] * v[2]));
        }

        public void Normalize()
        {
            float len = Magnitude();
            for (int i = 0; i < 4; i++)
            {
                v[i] /= len;
            }
        }
    }
}
