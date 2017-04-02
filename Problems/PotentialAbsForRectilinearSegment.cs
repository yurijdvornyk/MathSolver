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

        private const double a = -1;
        private const double b = 1;
        private const double potentialDeltaX = 0.3;
        private const double potentialDeltaY = 0.3;
        private const int potentialDivisions = 50;
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
            ProblemResult result = new ProblemResult();
            List<Tuple<double, double, double>> potentials = getPotentialPoints();
            object[,] resultMatrix = new object[potentials.Count, 3];
            List<Chart3dPoint> points = new List<Chart3dPoint>();
            for (int i = 0; i < potentials.Count; ++i)
            {
                resultMatrix[i, 0] = potentials[i].Item1;
                resultMatrix[i, 1] = potentials[i].Item2;
                resultMatrix[i, 2] = potentials[i].Item3;
                points.Add(new Chart3dPoint(potentials[i].Item1, potentials[i].Item2, potentials[i].Item3));
            }
            result.ResultData.Items.Add(ResultDataItem.Builder.Create().ColumnTitles("x1", "x2", "u(x1, x2)").Matrix(resultMatrix).Build());
            ResultChart<Chart3dPoint> chart = new ResultChart<Chart3dPoint>("Result", new List<string>() { "x1", "x2", "u(x)" });
            chart.Items.Add(ResultChartItem<Chart3dPoint>.Builder.Create().Points(points).Build());
            result.ResultPlot.Charts.Add(chart);

            ResultChart<Chart2dPoint> projectionXZ = new ResultChart<Chart2dPoint>("X1_U projection", new List<string>() { "x1", "u(x1, x2)" });
            projectionXZ.Items.Add(ResultChartItem<Chart2dPoint>.Builder.Create().Points(getXZProjection()).Build());
            result.ResultPlot.Charts.Add(projectionXZ);

            ResultChart<Chart2dPoint> projectionYZ = new ResultChart<Chart2dPoint>("X2_U projection", new List<string>() { "x2", "u(x1, x2)" });
            projectionYZ.Items.Add(ResultChartItem<Chart2dPoint>.Builder.Create().Points(getYZProjection()).Build());
            result.ResultPlot.Charts.Add(projectionYZ);
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
            List<Chart2dPoint> tx = new List<Chart2dPoint>();
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
            List<Tuple<double, double, double>> result = new List<Tuple<double, double, double>>();
            double stepX = 2 * potentialDeltaX / potentialDivisions;
            double stepY = 2 * potentialDeltaY / potentialDivisions;
            for (int i = 0; i < potentialDivisions; ++i)
            {
                for (int j = 0; j < potentialDivisions; ++j)
                {
                    double x = b - potentialDeltaX + i * stepX;
                    double y = -potentialDeltaY + i * stepY;
                    result.Add(new Tuple<double, double, double>(x, y, getPotentialAbsolute(x, y)));
                }
            }
            return result;
        }

        private List<Chart2dPoint> getXZProjection()
        {
            List<Chart2dPoint> result = new List<Chart2dPoint>();
            double stepX = 2 * potentialDeltaX / potentialDivisions;
            for (int i = 0; i < potentialDivisions; ++i)
            {
                double x = b - potentialDeltaX + i * stepX;
                result.Add(new Chart2dPoint(x, getPotentialAbsolute(x, 0)));
            }
            return result;
        }

        private List<Chart2dPoint> getYZProjection()
        {
            List<Chart2dPoint> result = new List<Chart2dPoint>();
            double stepY = 2 * potentialDeltaY / potentialDivisions;
            for (int i = 0; i < potentialDivisions; ++i)
            {
                double y = -potentialDeltaY + i * stepY;
                result.Add(new Chart2dPoint(y, getPotentialAbsolute(0, y)));
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