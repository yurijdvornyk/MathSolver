using HomericLibrary;
using Problems.Helper;
using Problems.Utils;
using ProblemSdk;
using ProblemSdk.Data;
using ProblemSdk.Result;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Problems
{
    /// <summary>
    /// K\tau\equiv\frac{1}{2\pi}\int\limits_a^b{\ln\frac{1}{|t-s|}\tau(t)}dt + \frac{1}{2\pi}\int\limits_a^b{W(t,s)\tau(t)}dt=g(s)
    /// </summary>
    public class ArbitraryContourExtendedCase : Problem
    {
        private readonly int POSITION_G = 0;
        private readonly int POSITION_PHI1 = 1;
        private readonly int POSITION_PHI2 = 2;
        private readonly int POSITION_PHI1_DER = 3;
        private readonly int POSITION_PHI2_DER = 4;
        private readonly int POSITION_VAR = 5;
        private readonly int POSITION_A = 6;
        private readonly int POSITION_B = 7;
        private readonly int POSITION_N = 8;
        private const int POSITION_DELTA_X = 9;
        private const int POSITION_DELTA_Y = 10;
        private const int POSITION_POTENTIAL_N = 11;

        private HomericExpression FunctionG;
        private HomericExpression Phi1;
        private HomericExpression Phi2;
        private HomericExpression Phi1Der;
        private HomericExpression Phi2Der;
        private double a;
        private double b;
        private int n;
        private double h { get { return (b - a) / n; } }
        private string variable;

        public double[,] matrix;
        private List<Tuple<double, double>> t;
        private PotentialHelper potentialHelper;
        private double potentialDeltaX;
        private double potentialDeltaY;
        private double potentialN;

        public ArbitraryContourExtendedCase() : base()
        {
            Name = "Integral equation with logarithmic singularity for arbitrary open-circuited parameterized contour - extended form";
            Equation = EquationUtils.GetPathForCurrentProblemEquation(this);
            InputData.AddDataItemAt<string>(POSITION_G, "g");
            InputData.AddDataItemAt(POSITION_PHI1, "Phi1", "t^2");
            InputData.AddDataItemAt(POSITION_PHI2, "Phi2", "t");
            InputData.AddDataItemAt(POSITION_PHI1_DER, "Phi1'", "2*t");
            InputData.AddDataItemAt(POSITION_PHI2_DER, "Phi2'", "1");
            InputData.AddDataItemAt(POSITION_VAR, "Variable", "t");
            InputData.AddDataItemAt<double>(POSITION_A, "a", 0);
            InputData.AddDataItemAt(POSITION_B, "b", 6.28);
            InputData.AddDataItemAt(POSITION_N, "N", 100);
            InputData.AddDataItemAt(POSITION_DELTA_X, DataItemBuilder<double>.Create().Name("x1 shift for potential").DefValue(0.3).Build());
            InputData.AddDataItemAt(POSITION_DELTA_Y, DataItemBuilder<double>.Create().Name("x2 shift for potential").DefValue(0.3).Build());
            InputData.AddDataItemAt(POSITION_POTENTIAL_N, DataItemBuilder<int>.Create().Name("n for calculating potential").DefValue(100).Build());
        }

        private double jacobian(double t)
        {
            double dt1 = Phi1Der.Calculate(t);
            double dt2 = Phi2Der.Calculate(t);
            double jacobian = Math.Sqrt(dt1 * dt1 + dt2 * dt2);
            return jacobian;
        }

        private void fillMatrix()
        {
            double[,] M = new double[n, n];

            for (int i = 0; i < n; ++i)
            {
                double ti = a + i * h;
                double ti1 = a + (i + 1) * h;

                for (int j = 0; j < n; ++j)
                {
                    double tj = a + j * h;
                    double tj1 = a + (j + 1) * h;
                    double t = (tj + tj1) / 2;

                    if (i != j)
                    {
                        string UnderLog = "1/(( ((" + Phi1.Expression + ")-(" + Phi1.Calculate(t) + "))^2 + ((" + Phi2.Expression + ")-(" + Phi2.Calculate(t) + "))^2 )^0.5)";
                        string jacobianHere = "( (" + Phi1Der.Expression + ")^2 + (" + Phi2Der.Expression + ")^2 )^0.5";
                        string toIntegrate = "(" + jacobianHere + ") * Ln(" + UnderLog + ")";
                        double integral = IntegrationHelper.GaussMethod(toIntegrate, ti, ti1, variable);
                        M[i, j] = (1 / (2 * Math.PI)) * integral;

                        string UnderLog2 = "((" + Phi1.Expression + ") - (" + Phi1.Calculate(t) + "))^2"
                            + " + ((" + Phi2.Expression + ") - (" + Phi2.Calculate(t) + "))^2";
                        double dt1 = Phi1Der.Calculate(t);
                        double dt2 = Phi2Der.Calculate(t);
                        double jacobian = Math.Sqrt(dt1 * dt1 + dt2 * dt2);
                        string functionStr = "(-0.5/(2*pi)) * Ln(" + UnderLog2 + ") * (" + jacobian.ToString() + ")";

                        M[i, j] += IntegrationHelper.GaussMethod(functionStr, ti, ti1, variable);
                    }
                    if (i == j)
                    {
                        double middle = (ti + ti1) / 2;
                        M[i, j] = (1 / (2 * Math.PI)) * jacobian(middle) * (ti1 - ti + ti * Math.Log(t - ti) - ti1 * Math.Log(ti1 - t) + t * Math.Log((ti1 - t) / (t - ti)));
                        M[i, j] += (1 / (2 * Math.PI)) * jacobian(middle) * Math.Log(jacobian(middle));

                        M[i, j] += 1;
                    }
                }
            }
            matrix = M;
        }

        private List<double> getTauFunc()
        {
            fillMatrix();
            double[] b = new double[n];
            // Fill b in Ac=b
            for (int i = 0; i < n; ++i)
            {
                b[i] = jacobian((t[i].Item1 + t[i].Item2) / 2);
            }
            return LinearEquationHelper.SolveWithGaussMethod(matrix, b).ToList();
        }

        protected override ProblemResult execute()
        {
            calculateTArray();
            List<double> tau = getTauFunc();
            object[,] matrix = new object[tau.Count, 2];
            List<Chart2dPoint> chartPoints = new List<Chart2dPoint>();
            for (int i = 0; i < tau.Count; ++i)
            {
                matrix[i, 0] = (t[i].Item1 + t[i].Item2) / 2;
                matrix[i, 1] = tau[i];
                chartPoints.Add(new Chart2dPoint((double)matrix[i, 0], (double)matrix[i, 1]));
            }
            ProblemResult problemResult = new ProblemResult();
            problemResult.ResultData.Items.Add(ResultDataItem.Builder.Create().ColumnTitles("t", "tau(t)").Matrix(matrix).Build());
            ResultChart<Chart2dPoint> chart = new ResultChart<Chart2dPoint>("Result", new List<string>() { "t", "tau(t)" });
            chart.Items.Add(ResultChartItem<Chart2dPoint>.Builder.Create().Points(chartPoints).Build());
            problemResult.ResultPlot.Charts.Add(chart);
            addPotentialToResult(problemResult, tau);
            return problemResult;
        }

        private void addPotentialToResult(ProblemResult result, List<double> tau)
        {
            potentialHelper = new PotentialHelper(potentialDeltaX, potentialDeltaY, potentialN, b, t, tau);
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
        }

        protected override void updateData()
        {
            InputData.GetValue(POSITION_VAR, out variable);
            FunctionG = new HomericExpression(InputData.GetValue<string>(POSITION_G), variable);
            Phi1 = new HomericExpression(InputData.GetValue<string>(POSITION_PHI1), variable);
            Phi2 = new HomericExpression(InputData.GetValue<string>(POSITION_PHI2), variable);
            Phi1Der = new HomericExpression(InputData.GetValue<string>(POSITION_PHI1_DER), variable);
            Phi2Der = new HomericExpression(InputData.GetValue<string>(POSITION_PHI2_DER), variable);
            InputData.GetValue(POSITION_A, out a);
            InputData.GetValue(POSITION_B, out b);
            InputData.GetValue(POSITION_N, out n);
            potentialDeltaX = InputData.GetValue<double>(POSITION_DELTA_X);
            potentialDeltaY = InputData.GetValue<double>(POSITION_DELTA_Y);
            potentialN = InputData.GetValue<int>(POSITION_POTENTIAL_N);
        }

        private void calculateTArray()
        {
            t = new List<Tuple<double, double>>();
            for (int i = 0; i < n; ++i)
            {
                t.Add(new Tuple<double, double>(a + i * h, a + (i + 1) * h));
            }
        }
    }
}
