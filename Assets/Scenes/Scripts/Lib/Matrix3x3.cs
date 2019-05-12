using System;
using UnityEngine;

namespace Lib
{
    public class Matrix3x3
    {
        private float[] data = new float[9];

        public Matrix3x3()
        {
            LoadIdentity();
        }

        public float this[int i]
        {
            get { return data[i]; }
            set { data[i] = value; }
        }

        public void LoadIdentity()
        {
            data[0] = data[4] = data[8] = 1;
            data[1] = data[2] = data[3] = data[5] = data[6] = data[7] = 0;
        }

        public Matrix3x3 Transpose()
        {
            Matrix3x3 mat = new Matrix3x3();

            mat[0] = data[0];
            mat[1] = data[3];
            mat[2] = data[6];
            mat[3] = data[1];
            mat[4] = data[4];
            mat[5] = data[7];
            mat[6] = data[2];
            mat[7] = data[5];
            mat[8] = data[8];

            return mat;
        }

        public Vector3 Multiply(Vector3 v)
        {
            return new Vector3(
                v[0] * data[0] + v[1] * data[3] + v[2] * data[6],
                v[0] * data[1] + v[1] * data[4] + v[2] * data[7],
                v[0] * data[2] + v[1] * data[5] + v[2] * data[8]);
        }

        public Matrix3x3 Multiply(Matrix3x3 other)
        {
            Matrix3x3 mat = new Matrix3x3();

            for (int i = 0, i3 = 0; i < 3; i++, i3 += 3)
            {
                for (int j = 0; j < 3; j++)
                {
                    mat[i3 + j] = 0;

                    for (int k = 0, k3 = 0; k < 3; k++, k3 += 3)
                    {
                        mat[i3 + j] += data[i3 + k] * other[k3 + j];
                    }
                }
            }

            return mat;
        }

        public void Scale(float scalar)
        {
            Matrix3x3 scaleMat = new Matrix3x3();
            scaleMat[0] = scaleMat[4] = scaleMat[8] = scalar;
            scaleMat[1] = scaleMat[2] = scaleMat[3] = scaleMat[5] = scaleMat[6] = scaleMat[7] = 0;

            Matrix3x3 scaledMat = Multiply(scaleMat);
            data[0] = scaledMat[0];
            data[1] = scaledMat[1];
            data[2] = scaledMat[2];
            data[3] = scaledMat[3];
            data[4] = scaledMat[4];
            data[5] = scaledMat[5];
            data[6] = scaledMat[6];
            data[7] = scaledMat[7];
            data[8] = scaledMat[8];
        }

        public Matrix3x3 Inverse()
        {
            float a = data[0];
            float b = data[1];
            float c = data[2];
            float d = data[3];
            float e = data[4];
            float f = data[5];
            float g = data[6];
            float h = data[7];
            float i = data[8];
            float div = -c * e * g + b * f * g + c * d * h - a * f * h - b * d * i + a * e * i;

            // const float EPSILON(1e-10);
            // TODO: http://d.hatena.ne.jp/nakamura001/20150117/1421501942
            Debug.Assert(Math.Abs(div) > float.Epsilon);

            float invDiv = 1.0f / div;

            Matrix3x3 m = new Matrix3x3();
            m[0] = -f * h + e * i;
            m[1] =  c * h - b * i;
            m[2] = -c * e + b * f;
            m[3] =  f * g - d * i;
            m[4] = -c * g + a * i;
            m[5] =  c * d - a * f;
            m[6] = -e * g + d * h;
            m[7] =  b * g - a * h;
            m[8] = -b * d + a * e;

            for (int idx = 0; idx< 9; idx++) { m.data[idx] *= invDiv; }

            return m;
        }
    }
}
