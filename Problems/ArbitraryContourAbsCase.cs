using HomericLibrary;
using Problems.Helper;
using ProblemSdk;
using ProblemSdk.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problems
{
    public class ArbitraryContourAbsCase : Problem
    {
        private readonly int POSITION_G = 0;
        private readonly int POSITION_VAR = 1;
        private readonly int POSITION_A = 2;
        private readonly int POSITION_B = 3;
        private readonly int POSITION_N = 4;

        private HomericExpression FunctionG;

        private HomericExpression L1x1;
        private HomericExpression L1x2;
        private HomericExpression L1x1Der;
        private HomericExpression L1x2Der;

        private HomericExpression L2x1;
        private HomericExpression L2x2;
        private HomericExpression L2x1Der;
        private HomericExpression L2x2Der;

        private double A;
        private double B;
        private double middle { get { return (B - A) / 2; } }
        private int N;
        private double H { get { return (B - A) / N; } }
        private string variable;

        public override string Name { get { return "Integral equation with logarithmic singularity for open-circuited parameterized contour that can be divided into two parts"; } }
        public override string Equation { get { return @"K\tau=\frac{1}{2\pi}\int\limits_{\alpha}^{\beta}{\tau(t)\ln{\left(\frac{1}{\sqrt{(\varphi_1(t)-\varphi_1(s))^2+(\varphi_2(t)-\varphi_2(s))^2}}\right)}F(t)}dt=g(s)"; } }

        private SortedDictionary<double, double> Tx = new SortedDictionary<double, double>();
        public double[,] matrix;

        public ArbitraryContourAbsCase() : base()
        {
            InputData.AddDataItemAt<string>(POSITION_G, "g");
            InputData.AddDataItemAt(POSITION_VAR, "Variable", "t");
            InputData.AddDataItemAt<double>(POSITION_A, "a", 0);
            InputData.AddDataItemAt(POSITION_B, "b", 6.28);
            InputData.AddDataItemAt(POSITION_N, "N", 100);

            L1x1Der = new HomericExpression("1");
            L1x2Der = new HomericExpression("-1");
            L2x1Der = new HomericExpression("1");
            L2x2Der = new HomericExpression("1");
        }

        private double jacobian(HomericExpression f1, HomericExpression f2, double t)
        {
            double dt1 = f1.Calculate(t);
            double dt2 = f2.Calculate(t);
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
                        if (t < middle)
                        {
                            string UnderLog = "1/(( ((" + L1x1.Expression + ")-(" + L1x1.Calculate(t) + "))^2 + ((" + L1x2.Expression + ")-(" + L1x2.Calculate(t) + "))^2 )^0.5)";
                            string jacobianHere = "( (" + L1x1Der.Expression + ")^2 + (" + L1x2Der.Expression + ")^2 )^0.5";
                            string toIntegrate = "(" + jacobianHere + ") * Ln(" + UnderLog + ")";
                            double integral = IntegrationHelper.GaussMethod(toIntegrate, ti, ti1, variable);
                            M[i, j] = (1 / (2 * Math.PI)) * integral;
                        }
                        if (t > middle)
                        {
                            string UnderLog = "1/(( ((" + L2x1.Expression + ")-(" + L2x1.Calculate(t) + "))^2 + ((" + L2x2.Expression + ")-(" + L2x2.Calculate(t) + "))^2 )^0.5)";
                            string jacobianHere = "( (" + L2x1Der.Expression + ")^2 + (" + L2x2Der.Expression + ")^2 )^0.5";
                            string toIntegrate = "(" + jacobianHere + ") * Ln(" + UnderLog + ")";
                            double integral = IntegrationHelper.GaussMethod(toIntegrate, ti, ti1, variable);
                            M[i, j] = (1 / (2 * Math.PI)) * integral;
                        }
                    }
                    else
                    {
                        double mid = (ti + ti1) / 2;
                        if (t < middle)
                        {
                            M[i, j] = (1 / (2 * Math.PI)) * jacobian(L1x1Der, L1x2Der, mid) * (ti1 - ti + ti * Math.Log(t - ti) - ti1 * Math.Log(ti1 - t) + t * Math.Log((ti1 - t) / (t - ti)));
                            M[i, j] += (1 / (2 * Math.PI)) * jacobian(L1x1Der, L1x2Der, mid) * Math.Log(jacobian(L1x1Der, L1x2Der, mid));
                        }
                        if (t > middle)
                        {
                            M[i, j] = (1 / (2 * Math.PI)) * jacobian(L2x1Der, L2x2Der, mid) * (ti1 - ti + ti * Math.Log(t - ti) - ti1 * Math.Log(ti1 - t) + t * Math.Log((ti1 - t) / (t - ti)));
                            M[i, j] += (1 / (2 * Math.PI)) * jacobian(L2x1Der, L2x2Der, mid) * Math.Log(jacobian(L2x1Der, L2x2Der, mid));
                        }
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

                if (t < middle)
                {
                    b[i] = jacobian(L1x1Der, L1x2Der, t);
                }
                if (t > middle)
                {
                    b[i] = jacobian(L2x1Der, L2x2Der, t);
                }
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
                chartPoints.Add(new ProblemChartPoint((double)matrix[i, 0], (double)matrix[i, 1]));
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
            InputData.GetValue(POSITION_A, out A);
            InputData.GetValue(POSITION_B, out B);
            InputData.GetValue(POSITION_N, out N);
            L1x1 = new HomericExpression(variable, variable);
            L1x2 = new HomericExpression("-" + variable, variable);
            L2x1 = new HomericExpression(variable, variable);
            L2x2 = new HomericExpression(variable, variable);
        }
    }
}
