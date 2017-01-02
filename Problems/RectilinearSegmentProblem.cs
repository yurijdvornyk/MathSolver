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
    public class RectilinearSegment : Problem
    {
        private const int POSITION_G = 0;
        private const int POSITION_VAR = 1;
        private const int POSITION_N = 2;

        public override string Name { get { return "Integral equation with logarithmic singularity for rectilinear segment"; } }
        public override string Equation { get { return @"K\tau=\frac{1}{2\pi}\int\limits_{-1}^{1}{\tau(y)\ln{\frac{1}{|x-y|}}}dy=g(x)"; } }

        private HomericExpression FunctionG;
        private readonly int A = -1;
        private readonly int B = 1;
        private int N;
        private double H;
        private string variable;

        public double[,] matrix;

        public RectilinearSegment() : base()
        {
            InputData.AddSingleDataItemAtPosition<string>(POSITION_G, "g");
            InputData.AddSingleDataItemAtPosition(POSITION_VAR, "Variable", "x");
            InputData.AddSingleDataItemAtPosition(POSITION_N, "N", 100);
        }

        public override void SetInputData()
        {
            // TODO: Dummy data
            InputData.SetValue(POSITION_G, "1");
            InputData.SetValue(POSITION_VAR, "x");
            InputData.SetValue(POSITION_N, 100);
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
                    double x = (tj + tj1) / 2;

                    if (x > ti1)
                    {
                        M[i, j] = ti * Math.Log(x - ti) - ti1 * Math.Log(x - ti1) + x * Math.Log((x - ti1) / (x - ti)) + ti1 - ti;
                    }
                    if (x < ti)
                    {
                        M[i, j] = ti * Math.Log(ti - x) - ti1 * Math.Log(ti1 - x) + x * Math.Log((ti1 - x) / (ti - x)) + ti1 - ti;
                    }
                    if (x >= ti && x <= ti1)
                    {
                        M[i, j] = ti1 - ti + ti * Math.Log(x - ti) - ti1 * Math.Log(ti1 - x) + x * Math.Log((ti1 - x) / (x - ti));
                    }
                }
            }
            matrix = M;
        }

        public List<Point> GetTx()
        {
            List<Point> tx = new List<Point>();
            fillMatrix();

            double[] b = new double[N];
            // Fill b in Ac=b
            for (int i = 0; i < N; ++i)
            {
                double ti = A + i * H;
                double ti1 = A + (i + 1) * H;
                double x = (ti + ti1) / 2;
                b[i] = FunctionG.Calculate(x);
            }
            // Solve Ac=b equation
            b = LinearEquationHelper.SolveWithGaussMethod(matrix, b);

            // Save c[] as Dictionary(x, y)
            for (int i = 0; i < N; ++i)
            {
                double ti = A + i * H;
                double ti1 = A + (i + 1) * H;
                double x = (ti + ti1) / 2;
                tx.Add(new Point(x, b[i]));
            }
            return tx;
        }

        protected override ProblemResult Execute()
        {
            var result = GetTx();
            object[,] resultMatrix = new object[result.Count, 2];
            for (int i = 0; i < result.Count; ++i)
            {
                resultMatrix[i, 0] = result[i].X;
                resultMatrix[i, 1] = result[i].Y;
            }
            ProblemResult problemResult = new ProblemResult("Result", "x", "tau(x)");
            problemResult.ResultData.Items.Add(ResultDataItem.Builder.Create().SetMatrix(resultMatrix).Build());
            problemResult.ResultChart.Items.Add(ResultChartItem.Builder.Create().Points(result).Build());
            return problemResult;
        }

        protected override void UpdateData()
        {
            InputData.GetValue(POSITION_N, out N);
            InputData.GetValue(POSITION_VAR, out variable);
            FunctionG = new HomericExpression(InputData.GetValue<string>(POSITION_G), variable);
            H = (double)(B - A) / N;
        }
    }
}
