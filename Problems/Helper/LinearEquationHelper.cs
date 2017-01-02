using System;

namespace Problems.Helper
{
    public class LinearEquationHelper
    {
        public static double[] SolveWithGaussMethod(double[,] aa, double[] bb)
        {
            double[,] a = (double[,])aa.Clone();
            double[] b = (double[])bb.Clone();

            int n = b.Length;
            double[] c = new double[n];
            int i, j, l = 0;
            double c1, c2, c3;

            for (i = 0; i < n; ++i)
            {
                c1 = 0;
                for (j = i; j < n; ++j)
                {
                    c2 = a[j, i];
                    if (Math.Abs(c2) > Math.Abs(c1))
                    {
                        l = j;
                        c1 = c2;
                    }
                }

                for (j = i; j < n; ++j)
                {
                    c3 = a[l, j] / c1;
                    a[l, j] = a[i, j];
                    a[i, j] = c3;
                }

                c3 = b[l] / c1;
                b[l] = b[i];
                b[i] = c3;

                for (j = 0; j < n; ++j)
                {
                    if (j == i)
                    {
                        continue;
                    }
                    for (l = i + 1; l < n; ++l)
                    {
                        a[j, l] = a[j, l] - a[i, l] * a[j, i];
                    }
                    b[j] = b[j] - b[i] * a[j, i];
                }
            }
            return b;
        }
    }
}
