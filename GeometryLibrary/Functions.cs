using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometryLibrary
{
    public static class Functions
    {
        public static double DeBoorCoxAlgorithm(int j, int n, double t, double[] ts)
        {
            if (n == 0)
            {
                if (0 <= j && j <= ts.Count() - 2)
                {
                    return (ts[j] <= t && t < ts[j + 1]) ? 1 : 0;
                }
                else {
                    throw new IndexOutOfRangeException();
                }
            }
            else
            {
                if (0 <= j && j <= ts.Count() - n - 2)
                {
                    return (t - ts[j]) / (ts[j + n] - ts[j]) * DeBoorCoxAlgorithm(j, n - 1, t, ts) + (ts[j + n + 1] - t) / (ts[j + n + 1] - ts[j + 1]) * DeBoorCoxAlgorithm(j + 1, n - 1, t, ts);
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public static double BernsteinBasisPolynomials(int n, int i, double t)
        {
            return BinomialCoefficients(n, i) * Math.Pow(t, i) * Math.Pow(1 - t, n - i);
        }

        public static double BinomialCoefficients(int n, int k)
        {
            double result = 1;
            for (int i = n; i > n - k; i--)
            {
                result *= i;
            }
            for (int i = 1; i <= k; i++)
            {
                result /= i;
            }
            return result;
        }
    }
}
