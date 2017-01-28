using HomericLibrary;
using Problems.Helper;
using Problems.Utils;
using ProblemSdk;
using ProblemSdk.Data;
using ProblemSdk.Result;
using System;
using System.Collections.Generic;

namespace Problems
{
    /// <summary>
    /// K\tau=\frac{1}{2\pi}\int\limits_{-1}^{1}{\tau(y)\ln{\frac{1}{|x-y|}}}dy=g(x), \qquad x\in(-1, 1)
    /// </summary>
    public class RectilinearSegment : Problem
    {
        private const int POSITION_G = 0;
        private const int POSITION_VAR = 1;
        private const int POSITION_N = 2;

        private HomericExpression FunctionG;
        private readonly int A = -1;
        private readonly int B = 1;
        private int N;
        private double H;
        private string Variable;

        public double[,] matrix;

        public RectilinearSegment() : base()
        {
            Name = "Integral equation with logarithmic singularity for rectilinear segment";
            Equation = EquationUtils.GetPathForCurrentProblemEquation(this);
            InputData.AddDataItemAt(POSITION_G, 
                DataItemBuilder<string>.Create().Name("g").Build());
            InputData.AddDataItemAt(POSITION_VAR, 
                DataItemBuilder<string>.Create().Name("Variable").DefValue("x").Build());
            InputData.AddDataItemAt(POSITION_N, 
                DataItemBuilder<int>.Create().Name("N").DefValue(100).Build());
        }

        protected override void setInputData(params object[] args)
        {
            InputData.SetValue(POSITION_G, args[0]);
            InputData.SetValue(POSITION_VAR, args[1]);
            InputData.SetValue(POSITION_N, args[2]);
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

        public List<ProblemChartPoint> GetTx()
        {
            List<ProblemChartPoint> tx = new List<ProblemChartPoint>();
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
                tx.Add(new ProblemChartPoint(x, b[i]));
            }
            return tx;
        }

        protected override ProblemResult execute()
        {
            var result = GetTx();
            object[,] resultMatrix = new object[result.Count, 2];
            for (int i = 0; i < result.Count; ++i)
            {
                resultMatrix[i, 0] = result[i].X;
                resultMatrix[i, 1] = result[i].Y;
            }
            ProblemResult problemResult = new ProblemResult("Result", "x", "tau(x)");
            problemResult.ResultData.Items.Add(ResultDataItem.Builder.Create().ColumnTitles("x", "tau(x)").SetMatrix(resultMatrix).Build());
            problemResult.ResultChart.Items.Add(ResultChartItem.Builder.Create().Points(result).Build());
            return problemResult;
        }

        protected override void updateData()
        {
            InputData.GetValue(POSITION_N, out N);
            InputData.GetValue(POSITION_VAR, out Variable);
            FunctionG = new HomericExpression(InputData.GetValue<string>(POSITION_G), Variable);
            H = (double)(B - A) / N;
        }
    }
}
