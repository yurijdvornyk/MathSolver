using HomericLibrary;
using System;

namespace Problems.Helper
{
    public class IntegrationHelper
    {
        public static double GaussMethod(string fx, double a, double b, string Var, double eps = 0.001)
        {
            HomericExpression expr = new HomericExpression(fx, Var);

            bool flag = true;
            int iter = 0;

            double c1 = 0.3478548;
            double c2 = 0.6521452;
            double c3 = c2;
            double c4 = c1;

            double result0 = a - eps;
            double result1 = b + eps;
            int n = 1;

            while (flag == true)
            {
                ++iter;
                result0 = 0;
                double h = (b - a) / n;

                for (int i = 0; i < n; ++i)
                {
                    double aa = a + i * h;
                    double bb = a + (i + 1) * h;
                    double hh = (bb - aa) / 3;

                    double x1 = aa;
                    double x2 = aa + hh;
                    double x3 = aa + 2 * hh;
                    double x4 = bb;

                    result0 += ((bb - aa) / 2) * (c1 * expr.Calculate(x1) + c2 * expr.Calculate(x2) + c3 * expr.Calculate(x3) + c4 * expr.Calculate(x4));
                }

                n *= 2;

                result1 = 0;
                h = (b - a) / n;

                for (int i = 0; i < n; ++i)
                {
                    double aa = a + i * h;
                    double bb = a + (i + 1) * h;
                    double hh = (bb - aa) / 3;

                    double x1 = aa;
                    double x2 = aa + hh;
                    double x3 = aa + 2 * hh;
                    double x4 = bb;

                    result1 += ((bb - aa) / 2) * (c1 * expr.Calculate(x1) + c2 * expr.Calculate(x2) + c3 * expr.Calculate(x3) + c4 * expr.Calculate(x4));
                }

                if (Math.Abs(result1 - result0) <= eps || iter > 100)
                {
                    n /= 2;
                    flag = false;
                }
            }
            return result1;
        }
    }
}
