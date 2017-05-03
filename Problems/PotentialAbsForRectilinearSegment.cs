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
        private const int POSITION_G = 0;
        private const int POSITION_VAR = 1;
        private const int POSITION_N = 2;
        private const int POSITION_DELTA_X = 3;
        private const int POSITION_DELTA_Y = 4;
        private const int POSITION_POTENTIAL_N = 5;

        private const double a = -1;
        private const double b = 1;
        private double potentialDeltaX;
        private double potentialDeltaY;
        private int potentialN;
        private HomericExpression g;
        private string variable;
        private int n;
        private double h;

        private List<double> tau;
        private List<Tuple<double, double>> t;

        private PotentialHelper potentialHelper;

        public PotentialAbsForRectilinearSegment(): base()
        {
            Name = "Potential absolute for Rectilinear segment problem";
            InputData.AddDataItemAt(POSITION_G, DataItemBuilder<string>.Create().Name("g").Build());
            InputData.AddDataItemAt(POSITION_VAR, DataItemBuilder<string>.Create().Name("variable").DefValue("t").Build());
            InputData.AddDataItemAt(POSITION_N, DataItemBuilder<int>.Create().Name("n").DefValue(100).Validation(number => number > 0).Build());
            InputData.AddDataItemAt(POSITION_DELTA_X, DataItemBuilder<double>.Create().Name("x1 shift for potential").DefValue(0.3).Build());
            InputData.AddDataItemAt(POSITION_DELTA_Y, DataItemBuilder<double>.Create().Name("x2 shift for potential").DefValue(0.3).Build());
            InputData.AddDataItemAt(POSITION_POTENTIAL_N, DataItemBuilder<int>.Create().Name("n for calculating potential").DefValue(100).Build());
        }

        protected override ProblemResult execute()
        {
            calculateTArray();
            potentialHelper = new PotentialHelper(potentialDeltaX, potentialDeltaY, potentialN, b, t, getTauFunc());
            List<Tuple<double, double, double>> potentials = potentialHelper.getPotentialPoints();
            object[,] resultMatrix = new object[potentials.Count, 3];
            List<Chart3dPoint> points = new List<Chart3dPoint>();
            for (int i = 0; i < potentials.Count; ++i)
            {
                resultMatrix[i, 0] = potentials[i].Item1;
                resultMatrix[i, 1] = potentials[i].Item2;
                resultMatrix[i, 2] = potentials[i].Item3;
                points.Add(new Chart3dPoint(potentials[i].Item1, potentials[i].Item2, potentials[i].Item3));
            }
            ProblemResult result = new ProblemResult();
            result.ResultData.Items.Add(ResultDataItem.Builder.Create().ColumnTitles("x1", "x2", "u(x1, x2)").Matrix(resultMatrix).Build());
            ResultChart<Chart3dPoint> chart = new ResultChart<Chart3dPoint>("u(x1, x2)", new List<string>() { "x1", "x2", "u(x)" });
            chart.Items.Add(ResultChartItem<Chart3dPoint>.Builder.Create().Points(points).Build());
            result.ResultPlot.Charts.Add(chart);

            ResultChart<Chart2dPoint> projectionXZ = new ResultChart<Chart2dPoint>("u(x1, 0)", new List<string>() { "x1", "u(x1, x2)" });
            projectionXZ.Items.Add(ResultChartItem<Chart2dPoint>.Builder.Create().Points(potentialHelper.getXZProjection()).Build());
            result.ResultPlot.Charts.Add(projectionXZ);

            ResultChart<Chart2dPoint> projectionYZ = new ResultChart<Chart2dPoint>("u(0, x2)", new List<string>() { "x2", "u(x1, x2)" });
            projectionYZ.Items.Add(ResultChartItem<Chart2dPoint>.Builder.Create().Points(potentialHelper.getYZProjection()).Build());
            result.ResultPlot.Charts.Add(projectionYZ);
            return result;
        }

        private void calculateTArray()
        {
            t = new List<Tuple<double, double>>();
            for (int i = 0; i < n; ++i)
            {
                t.Add(new Tuple<double, double>(a + i * h, a + (i + 1) * h));
            }
        }

        protected override void updateData()
        {
            InputData.GetValue(POSITION_VAR, out variable);
            g = new HomericExpression(InputData.GetValue<string>(POSITION_G), variable);
            InputData.GetValue(POSITION_N, out n);
            h = (b - a) / n;
            potentialDeltaX = InputData.GetValue<double>(POSITION_DELTA_X);
            potentialDeltaY = InputData.GetValue<double>(POSITION_DELTA_Y);
            potentialN = InputData.GetValue<int>(POSITION_POTENTIAL_N);
        }

        private double[,] getMatrix()
        {
            double[,] matrix = new double[n, n]; // M: n x n

            for (int i = 0; i < n; ++i)
            {
                double ti = t[i].Item1;
                double ti1 = t[i].Item2;
                t.Add(new Tuple<double, double>(ti, ti1));
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
                double x = (t[i].Item1 + t[i].Item2) / 2;
                b[i] = g.Calculate(x);
            }
            return new List<double>(LinearEquationHelper.SolveWithGaussMethod(getMatrix(), b));
        }
    }
}