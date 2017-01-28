using HomericLibrary;
using Problems.Helper;
using ProblemSdk;
using ProblemSdk.Classes.Choice;
using ProblemSdk.Data;
using ProblemSdk.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problems
{
    public class LinearSingularEquation : Problem
    {
        private readonly int POSITION_G = 0;
        private readonly int POSITION_PHI1 = 1;
        private readonly int POSITION_PHI2 = 2;
        private readonly int POSITION_VAR = 3;
        private readonly int POSITION_A = 4;
        private readonly int POSITION_B = 5;
        private readonly int POSITION_N = 6;

        public override string Name { get { return "(Nameless) uniform equation"; } }
        public override string Equation { get { return @"\sum\limits_{i=1}^{n}c_{i}a_{ij}=\begin{cases}\quad 0,\\ \quad 1\end{cases}"; } }

        private double A;
        private double B;
        private int G;
        private string variable;
        private HomericExpression Phi1;
        private HomericExpression Phi2;
        private int N;
        private double H { get { return (double)Math.Abs(B - A) / N; } }

        public LinearSingularEquation() : base()
        {
            //InputData.AddDataItemAt(POSITION_G, "g", 0, true, x => x == 0 || x == 1);
            InputData.AddDataItemAt(POSITION_G, DataItemBuilder<ISingleChoice>
                .Create()
                .Name("G")
                .DefValue(new SingleChoice<int>(new List<int>() { 0, 1 }, 0))
                .Build());
            InputData.AddDataItemAt(POSITION_PHI1, "Phi1", "t");
            InputData.AddDataItemAt(POSITION_PHI2, "Phi2", "1");
            InputData.AddDataItemAt(POSITION_VAR, "Variable", "t");
            InputData.AddDataItemAt<double>(POSITION_A, "a", -1);
            InputData.AddDataItemAt<double>(POSITION_B, "b", 1);
            InputData.AddDataItemAt(POSITION_N, "N", 100);
        }

        private double[,] GetMatrix()
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
                    double middle = (tj + tj1) / 2;
                    M[i, j] = 0.5 * Math.Log(
                        Math.Pow(Phi1.Calculate(ti1) - Phi1.Calculate(middle), 2)
                        + Math.Pow(Phi2.Calculate(ti1) - Phi2.Calculate(middle), 2))
                        - 0.5 * Math.Log(
                        Math.Pow(Phi1.Calculate(ti) - Phi1.Calculate(middle), 2)
                        + Math.Pow(Phi2.Calculate(ti) - Phi2.Calculate(middle), 2));
                }
            }
            return M;
        }

        private double[] SolveMatrix()
        {
            double[] right = new double[N];
            for (int i = 0; i < N; ++i) { right[i] = G; }
            var matrix = GetMatrix();
            return LinearEquationHelper.SolveWithGaussMethod(matrix, right);
        }

        protected override ProblemResult execute()
        {
            object[,] result = new object[N, 2];
            var resArray = SolveMatrix();
            List<ProblemChartPoint> chartPoints = new List<ProblemChartPoint>();
            for (int i = 0; i < N; ++i)
            {
                double x = A + i * H;
                result[i, 0] = x;
                result[i, 1] = resArray[i];
                chartPoints.Add(new ProblemChartPoint(x, resArray[i]));
            }
            ProblemResult problemResult = new ProblemResult("Result", "x", "f(x)");
            problemResult.ResultData.Items.Add(ResultDataItem.Builder.Create().ColumnTitles("x", "f(x)").SetMatrix(result).Build());
            problemResult.ResultChart.Items.Add(ResultChartItem.Builder.Create().Points(chartPoints).Build());
            return problemResult;
        }

        protected override void updateData()
        {
            InputData.GetValue(POSITION_VAR, out variable);
            SingleChoice<int> g;
            InputData.GetValue(POSITION_G, out g);
            G = g.Value;
            Phi1 = new HomericExpression(InputData.GetValue<string>(POSITION_PHI1), variable);
            Phi2 = new HomericExpression(InputData.GetValue<string>(POSITION_PHI2), variable);
            InputData.GetValue(POSITION_A, out A);
            InputData.GetValue(POSITION_B, out B);
            InputData.GetValue(POSITION_N, out N);
        }
    }
}
