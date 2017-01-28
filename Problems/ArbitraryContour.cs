using HomericLibrary;
using Problems.Helper;
using Problems.Utils;
using ProblemSdk;
using ProblemSdk.Result;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Problems
{
    /// <summary>
    /// K\tau=\frac{1}{2\pi}\int\limits_{\alpha}^{\beta}{\tau(t)\ln{\left(\frac{1}{\sqrt{(\varphi_1(t)-\varphi_1(s))^2+(\varphi_2(t)-\varphi_2(s))^2}}\right)}F(t)}dt=g(s)
    /// </summary>
    public class ArbitraryContour : Problem
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

        public ArbitraryContour() : base()
        {
            Name = "Integral equation with logarithmic singularity for arbitrary open-circuited parameterized contour";
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

        private double jacobian(double t)
        {
            double dt1 = Phi1Der.Calculate(t);
            double dt2 = Phi2Der.Calculate(t);
            double jacobian = Math.Sqrt(dt1 * dt1 + dt2 * dt2);
            return jacobian;
        }

        private void fillMatrix()
        {
            double[,] M = new double[N, N]; // M: N x N

            for (int i = 0; i < N; ++i)
            {
                double ti = A + i * H;
                double ti1 = A + (i + 1) * H;

                for (int j = 0; j < N; ++j)
                {
                    double tj = A + j * H;
                    double tj1 = A + (j + 1) * H;
                    double t = (tj + tj1) / 2;

                    if (i != j)
                    {
                        string UnderLog = "1/(( ((" + Phi1.Expression + ")-(" + Phi1.Calculate(t) + "))^2 + ((" + Phi2.Expression + ")-(" + Phi2.Calculate(t) + "))^2 )^0.5)";
                        string jacobianHere = "( (" + Phi1Der.Expression + ")^2 + (" + Phi2Der.Expression + ")^2 )^0.5";
                        string toIntegrate = "(" + jacobianHere + ") * Ln(" + UnderLog + ")";
                        double integral = IntegrationHelper.GaussMethod(toIntegrate, ti, ti1, variable);
                        M[i, j] = (1 / (2 * Math.PI)) * integral;
                    }
                    else
                    {
                        double middle = (ti + ti1) / 2;
                        M[i, j] = (1 / (2 * Math.PI)) * jacobian(middle) * (ti1 - ti + ti * Math.Log(t - ti) - ti1 * Math.Log(ti1 - t) + t * Math.Log((ti1 - t) / (t - ti)));
                        M[i, j] += (1 / (2 * Math.PI)) * jacobian(middle) * Math.Log(jacobian(middle));
                    }
                }
            }
            matrix = M;
        }

        public SortedDictionary<double, double> GetTx()
        {
            Tx.Clear();
            fillMatrix();

            double[] b = new double[N];
            // Fill b in Ac=b
            for (int i = 0; i < N; ++i)
            {
                double ti = A + i * H;
                double ti1 = A + (i + 1) * H;
                double t = (ti + ti1) / 2;
                b[i] = jacobian(t);
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
            var chartPoints = new List<ProblemChartPoint>();
            for (int i = 0; i < resultRaw.Count; ++i)
            {
                matrix[i, 0] = resultRaw.Keys.ToList()[i];
                matrix[i, 1] = resultRaw.Values.ToList()[i];
                chartPoints.Add(new ProblemChartPoint((double) matrix[i, 0], (double) matrix[i, 1]));
            }
            ProblemResult problemResult = new ProblemResult("Result", "t", "tau(t)");
            problemResult.ResultData.Items.Add(ResultDataItem.Builder.Create().ColumnTitles("t", "tau(t)").SetMatrix(matrix).Build());
            problemResult.ResultChart.Items.Add(ResultChartItem.Builder.Create().Points(chartPoints).Build());
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