using HomericLibrary;
using Problems.Helper;
using Problems.Utils;
using ProblemSdk;
using ProblemSdk.Result;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Problems
{
    /// <summary>
    /// K\tau=\frac{1}{2\pi}\int\limits_{\alpha}^{\beta}{\tau(t)\ln{\frac{1}{|t-s|}}}dt=g(s), \qquad s\in(\alpha, \beta)
    /// </summary>
    public class ParameterizedRectilinearSegment : Problem
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

        private HomericExpression FunctionG;
        private HomericExpression Phi1;
        private HomericExpression Phi2;
        private HomericExpression Phi1Der;
        private HomericExpression Phi2Der;

        private double A;
        private double B;
        private int N;
        private double H { get { return (B - A) / N; } }
        private string variable;

        private SortedDictionary<double, double> Tx = new SortedDictionary<double, double>();
        public double[,] matrix;

        public ParameterizedRectilinearSegment() : base()
        {
            Name = "Integral equation with logarithmic singularity for rectilinear parameterized segment";
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
        }

        private void fillParameterizedMatrix()
        {
            double[,] M = new double[N, N];

            for (int i = 0; i < N; ++i)
            {
                double ti = A + i * H;
                double ti1 = A + (i + 1) * H;
                for (int j = 0; j < N; ++j)
                {
                    double tj = A + j * H;
                    double tj1 = A + (j + 1) * H;
                    double t = (tj + tj1) / 2;

                    if (t > ti1 || t < ti)
                    {
                        string UnderLog = "((" + Phi1.Expression + ") - (" + Phi1.Calculate(t) + "))^2"
                            + " + ((" + Phi2.Expression + ") - (" + Phi2.Calculate(t) + "))^2";

                        double dt1 = Phi1Der.Calculate(t);
                        double dt2 = Phi2Der.Calculate(t);
                        double jacobian = Math.Sqrt(dt1 * dt1 + dt2 * dt2);
                        string functionStr = "(-0.5/(2*pi)) * Ln(" + UnderLog + ") * (" + jacobian.ToString() + ")";
                        M[i, j] = IntegrationHelper.GaussMethod(functionStr, ti, ti1, variable);
                    }
                    else
                    {
                        M[i, j] = 1;
                    }
                }
            }
            matrix = M;
        }

        public SortedDictionary<double, double> GetTx()
        {
            Tx.Clear();

            fillParameterizedMatrix();

            double[] b = new double[N];
            // Fill b in Ac=b
            for (int i = 0; i < N; ++i)
            {
                double ti = A + i * H;
                double ti1 = A + (i + 1) * H;
                double t = (ti + ti1) / 2;

                double dt1;
                double dt2;

                try
                {
                    dt1 = Phi1Der.Calculate(t);
                    dt2 = Phi2Der.Calculate(t);
                }
                catch
                {
                    throw new ArgumentException("Phi derivatives are in wrong format!");
                }

                string jacobianStr = "(" + (dt1 * dt1 + dt2 * dt2).ToString() + ")^0.5";
                HomericExpression jacobian = new HomericExpression(jacobianStr, variable);
                b[i] = jacobian.Calculate(t);
            }

            b = LinearEquationHelper.SolveWithGaussMethod(matrix, b);

            // Save c[] as Dictionary(x, y)
            for (int i = 0; i < N; ++i)
            {
                double ti = A + i * H;
                double ti1 = A + (i + 1) * H;
                double x = (ti + ti1) / 2;
                Tx.Add(x, b[i]);
            }

            return Tx;
        }

        protected override ProblemResult execute()
        {
            var resultRaw = GetTx();
            var matrix = new object[resultRaw.Count, 2];
            var chartPoints = new List<Chart2dPoint>();
            for (int i = 0; i < resultRaw.Count; ++i)
            {
                matrix[i, 0] = resultRaw.Keys.ToList()[i];
                matrix[i, 1] = resultRaw.Values.ToList()[i];
                chartPoints.Add(new Chart2dPoint((double)matrix[i, 0], (double)matrix[i, 1]));
            }
            ProblemResult problemResult = new ProblemResult(); // "Result", "t", "tau(t)");
            problemResult.ResultData.Items.Add(ResultDataItem.Builder.Create().ColumnTitles("t", "tau(t)").Matrix(matrix).Build());
            ResultChart<Chart2dPoint> chart = new ResultChart<Chart2dPoint>("Result", new List<string>() { "t", "tau(t)" });
            chart.Items.Add(ResultChartItem<Chart2dPoint>.Builder.Create().Points(chartPoints).Build());
            problemResult.ResultPlot.Charts.Add(chart);
            return problemResult;
        }

        protected override void updateData()
        {
            InputData.GetValue(POSITION_VAR, out variable);
            FunctionG = new HomericExpression(InputData.GetValue<string>(POSITION_G), variable);
            Phi1 = new HomericExpression(InputData.GetValue<string>(POSITION_PHI1), variable);
            Phi2 = new HomericExpression(InputData.GetValue<string>(POSITION_PHI2), variable);
            Phi1Der = new HomericExpression(InputData.GetValue<string>(POSITION_PHI1_DER), variable);
            Phi2Der = new HomericExpression(InputData.GetValue<string>(POSITION_PHI2_DER), variable);
            InputData.GetValue(POSITION_A, out A);
            InputData.GetValue(POSITION_B, out B);
            InputData.GetValue(POSITION_N, out N);
        }
    }
}
