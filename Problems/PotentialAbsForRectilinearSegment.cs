using ProblemSdk;
using System;
using System.Collections.Generic;
using ProblemSdk.Result;
using HomericLibrary;
using ProblemSdk.Data;
using Problems.Helper;

namespace Problems
{
    public class PotentialAbsForRectilinearSegment : Problem
    {
        private static readonly int POSITION_G = 0;
        private static readonly int POSITION_VAR = 1;
        private static readonly int POSITION_N = 2;

        private readonly double a = -1;
        private readonly double b = 1;
        private HomericExpression g;
        private string variable;
        private int n;
        private double h;

        private List<double> tau;

        public PotentialAbsForRectilinearSegment(): base()
        {
            Name = "Potential absolute for Rectilinear segment problem";
            InputData.AddDataItemAt(POSITION_G, DataItemBuilder<string>.Create().Name("g").Build());
            InputData.AddDataItemAt(POSITION_VAR, DataItemBuilder<string>.Create().Name("variable").DefValue("t").Build());
            InputData.AddDataItemAt(POSITION_N, DataItemBuilder<int>.Create().Name("n").Validation(number => number > 0).Build());
        }

        protected override ProblemResult execute()
        {
            ProblemResult result = new ProblemResult("x", "u(x)");
            List<Tuple<double, double, double>> potentials = getPotentialPoints();
            object[,] resultMatrix = new object[potentials.Count, 3];
            List<ProblemChartPoint> points = new List<ProblemChartPoint>();
            for (int i = 0; i < potentials.Count; ++i)
            {
                resultMatrix[i, 0] = potentials[i].Item1;
                resultMatrix[i, 1] = potentials[i].Item2;
                resultMatrix[i, 2] = potentials[i].Item3;
                points.Add(new ProblemChartPoint(potentials[i].Item2, potentials[i].Item3));
            }
            result.ResultData.Items.Add(ResultDataItem.Builder.Create().ColumnTitles("x1", "x2", "u(x1, x2)").Matrix(resultMatrix).Build());
            result.ResultChart.Items.Add(ResultChartItem.Builder.Create().Points(points).Build());
            return result;
        }

        protected override void updateData()
        {
            InputData.GetValue(POSITION_VAR, out variable);
            g = new HomericExpression(InputData.GetValue<string>(POSITION_G), variable);
            InputData.GetValue(POSITION_N, out n);
            h = (b - a) / n;
        }

        private double[,] getMatrix()
        {
            double[,] matrix = new double[n, n]; // M: n x n

            for (int i = 0; i < n; ++i)
            {
                double ti = a + i * h;
                double ti1 = a + (i + 1) * h;
                for (int j = 0; j < n; ++j)
                {
                    double tj = a + j * h;
                    double tj1 = a + (j + 1) * h;
                    double x = (tj + tj1) / 2;
                    if (x > ti1)
                    {
                        matrix[i, j] = ti * Math.Log(x - ti) - ti1 * Math.Log(x - ti1) + x * Math.Log((x - ti1) / (x - ti)) + ti1 - ti;
                    }
                    if (x < ti)
                    {
                        matrix[i, j] = ti * Math.Log(ti - x) - ti1 * Math.Log(ti1 - x) + x * Math.Log((ti1 - x) / (ti - x)) + ti1 - ti;
                    }
                    if (x >= ti && x <= ti1)
                    {
                        matrix[i, j] = ti1 - ti + ti * Math.Log(x - ti) - ti1 * Math.Log(ti1 - x) + x * Math.Log((ti1 - x) / (x - ti));
                    }
                }
            }
            return matrix;
        }

        private List<double> getTauFunc()
        {
            List<ProblemChartPoint> tx = new List<ProblemChartPoint>();
            double[] b = new double[n];
            for (int i = 0; i < n; ++i)
            {
                double ti = a + i * h;
                double ti1 = a + (i + 1) * h;
                double x = (ti + ti1) / 2;
                b[i] = g.Calculate(x);
            }
            return new List<double>(LinearEquationHelper.SolveWithGaussMethod(getMatrix(), b));
        }

        private List<Tuple<double, double, double>> getPotentialPoints()
        {
            double bottom = -1;
            double top = 1;
            int steps = 100;
            double step = (top - bottom) / steps;
            List<Tuple<double, double, double>> result = new List<Tuple<double, double, double>>();
            for (int i = 0; i < steps; ++i)
            {
                double y = bottom + i * step;
                if (y != 0)
                {
                    result.Add(new Tuple<double, double, double>(0.5, y, getPotentialAbsolute(0.5, bottom + i * step)));
                }
                //else
                //{
                //    result.Add(new Tuple<double, double, double>(0.5, -1, getPotentialAbsolute(0.5, bottom + i * step)));
                //}
            }
            return result;
        }

        private double getPotentialAbsolute(double x1, double x2)
        {
            if (tau == null)
            {
                tau = getTauFunc();
            }
            return Math.Sqrt(Math.Pow(getDuDx1(x1, x2), 2) + Math.Pow(getDuDx2(x1, x2), 2));
        }

        private double getDuDx1(double x1, double x2)
        {
            double sum = 0;
            for (int i = 0; i < n - 1; ++i)
            {
                sum += tau[i] * Math.Log((Math.Pow(tau[i + 1] - x1, 2) + Math.Pow(x2, 2))
                    / (Math.Pow(tau[i] - x1, 2) + Math.Pow(x2, 2)));
            }
            return (-0.25 / Math.PI) * sum;
        }

        private double getDuDx2(double x1, double x2)
        {
            double sum = 0;
            for (int i = 0; i < n - 1; ++i)
            {
                sum += tau[i] * (Math.Atan(Math.Abs(tau[i + 1] - x1) / Math.Abs(x2)) - Math.Atan(Math.Abs(tau[i] - x1) / Math.Abs(x2)));
            }
            return (0.5 / Math.PI) * sum;
        }
    }
}