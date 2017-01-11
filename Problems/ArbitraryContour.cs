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

        public override string Name { get { return "Integral equation with logarithmic singularity for arbitrary open-circuited parameterized contour"; } }
        public override string Equation { get { return @"K\tau=\frac{1}{2\pi}\int\limits_{\alpha}^{\beta}{\tau(t)\ln{\left(\frac{1}{\sqrt{(\varphi_1(t)-\varphi_1(s))^2+(\varphi_2(t)-\varphi_2(s))^2}}\right)}F(t)}dt=g(s)"; } }

        private HomericExpression FunctionG;
        private HomericExpression Phi1;
        private HomericExpression Phi2;
        private HomericExpression Phi1Der;
        private HomericExpression Phi2Der;

        private double A;
        private double B;
        private int N;
        private double H { get { return (double)(B - A) / N; } }
        private string variable;

        #region Fields

        private SortedDictionary<double, double> Tx = new SortedDictionary<double, double>();
        public double[,] matrix;

        #endregion

        #region Constructor

        public ArbitraryContour() : base()
        {
            InputData.AddSingleDataItemAtPosition<string>(POSITION_G, "g");
            InputData.AddSingleDataItemAtPosition<string>(POSITION_PHI1, "Phi1", "t^2");
            InputData.AddSingleDataItemAtPosition<string>(POSITION_PHI2, "Phi2", "t");
            InputData.AddSingleDataItemAtPosition<string>(POSITION_PHI1_DER, "Phi1'", "2*t");
            InputData.AddSingleDataItemAtPosition<string>(POSITION_PHI2_DER, "Phi2'", "1");
            InputData.AddSingleDataItemAtPosition<string>(POSITION_VAR, "Variable", "t");
            InputData.AddSingleDataItemAtPosition<double>(POSITION_A, "a", 0);
            InputData.AddSingleDataItemAtPosition<double>(POSITION_B, "b", 6.28);
            InputData.AddSingleDataItemAtPosition<double>(POSITION_N, "N", 100);
        }

        #endregion

        #region Methods

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

        //public override void ParseData()
        //{
        //    if (InputData.Count == 9)
        //    {
        //        try
        //        {
        //            variable = InputData[5].Value.ToString();
        //            FunctionG = new HomericExpression(InputData[0].Value.ToString(), variable);
        //            Phi1 = new HomericExpression(InputData[1].Value.ToString(), variable);
        //            Phi2 = new HomericExpression(InputData[2].Value.ToString(), variable);
        //            Phi1Der = new HomericExpression(InputData[3].Value.ToString(), variable);
        //            Phi2Der = new HomericExpression(InputData[4].Value.ToString(), variable);
        //            A = double.Parse(InputData[6].Value.ToString());
        //            B = double.Parse(InputData[7].Value.ToString());
        //            N = int.Parse(InputData[8].Value.ToString());
        //        }
        //        catch
        //        {
        //            throw new ArgumentException("Bad input data!");
        //        }
        //    }
        //    else
        //    {
        //        throw new ArgumentException("Bad argument list!");
        //    }
        //}

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
            ProblemResult problemResult = new ProblemResult("Result", "x", "tau(x)");
            problemResult.ResultData.Items.Add(ResultDataItem.Builder.Create().ColumnTitles("x", "tau(x)").SetMatrix(matrix).Build());
            problemResult.ResultChart.Items.Add(ResultChartItem.Builder.Create().Points(chartPoints).Build());
            return problemResult;
        }

        protected override void setInputData(params object[] args)
        {
            throw new NotImplementedException();
        }

        protected override void updateData()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}