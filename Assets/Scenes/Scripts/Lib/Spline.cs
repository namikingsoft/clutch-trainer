using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lib
{
    public class Spline
    {
        List<Tuple<float, float>> points = new List<Tuple<float, float>>();
        List<float> secondDeriv = new List<float>();
        float firstSlope = 0;
        float lastSlope = 0;
        bool derivsCalculated = false;
        float slope = 0;

        public void clear()
        {
            points.Clear();
            derivsCalculated = false;
            slope = 0;
        }

        public void addPoint(float x, float y)
        {
            points.Add(new Tuple<float, float>(x, y));
            derivsCalculated = false;
            points.Sort((a, b) => (a.Item1 - b.Item1 > 0) ? 1 : -1);
        }

        public float Interpolate(float x) 
        {
            if (points.Count == 1) {
                slope = 0.0f;
                return points[0].Item2;
            }

            if (!derivsCalculated) { Calculate(); }

            int low = 0;
            int high = points.Count - 1;
            int index;

            // Bisect to find the interval that the distance is on.
            while ((high - low) > 1 )
            {
                index = (int)((high + low) / 2.0f);
                if (points[index].Item1 > x) { high = index; }
                else                         { low  = index; }
            }

            float diff = points[high].Item1 - points[low].Item1;
            Debug.Assert(diff >= 0.0f);

            // Evaluate the coefficients for the cubic spline equation.
            float a = (points[high].Item1 - x) / diff;
            float b = 1.0f - a;
            float sq = diff * diff / 6.0f;
            float a2 = a * a;
            float b2 = b * b;

            // Find the first derivative.
            slope = (points[high].Item2 - points[low].Item2) /diff -
                ((3.0f * a2) - 1.0f) / 6.0f * diff * secondDeriv[low] +
                ((3.0f * b2) - 1.0f) / 6.0f * diff * secondDeriv[high];

            // Return the interpolated value.
            return a* points[low].Item2 +
                   b* points[high].Item2 +
                   a* (a2 - 1.0f) * sq * secondDeriv[low] +
                   b* (b2 - 1.0f) * sq * secondDeriv[high];
        }

        private void Calculate() 
        {
            int n = points.Count;
            Debug.Assert(n > 1);

            float[] a = new float[n];
            float[] b = new float[n];
            float[] c = new float[n];
            float[] r = new float[n];

            // Fill in the arrays that represent the tridiagonal matrix.
            // a[0] is not used.
            float diff = points[1].Item1 - points[0].Item1;
            b[0] = diff / 3.0f;
            c[0] = diff / 6.0f;
            r[0] = ((points[1].Item2 - points[0].Item2) / diff) - firstSlope;

            for (int i = 1; i<n - 1; i++)
            {
                float diff1 = points[i + 1].Item1 - points[i].Item1;
                float diff2 = points[i].Item1 - points[i - 1].Item1;

                a[i] = diff2 / 6.0f;
                b[i] = (points[i + 1].Item1 - points[i - 1].Item1) / 3.0f;
                c[i] = diff1 / 6.0f;
                r[i] = ((points[i + 1].Item2 - points[i].Item2) / diff1) -
                    ((points[i].Item2 - points[i - 1].Item2) / diff2);
            }

            diff = points[n - 1].Item1 - points[n - 2].Item1;
            a[n - 1] = diff / 6.0f;
            b[n - 1] = diff / 3.0f;
            // c[n-1] is not used.
            r[n - 1] = lastSlope - ((points[n - 1].Item2 - points[n - 2].Item2) / diff);

            // Gauss-Jordan Elimination
            for (int i = 1; i<n; i++)
            {
                // Replace row i with row i - k * row (i-1) such that A_{i,i-1} = 0.0.
                float factor = a[i] / b[i - 1];
                // A_{i,i-1} is not used again, so it need not be calculated.
                b[i] -= factor* c[i - 1];
                // A_{i,i+1} is unchanged because A_{i-1,i+1} = 0.0.
                r[i] -= factor* r[i - 1];
            }

            //---- Back-substitution
            // Solve for y"[N].
            // secondDeriv.resize(n);
            for (int expandNum = n - secondDeriv.Count; expandNum > 0; expandNum--)
            {
                secondDeriv.Add(0);
            }

            secondDeriv[n - 1] = r[n - 1] / b[n - 1];
            for (int i = n - 2; i >= 0; i--)
            {
                // Use the solution for y"[i+1] to find y"[i].
                secondDeriv[i] = (r[i] - (c[i] * secondDeriv[i + 1])) / b[i];
            }

            derivsCalculated = true;
        }
    }
}
