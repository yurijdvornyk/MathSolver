using ProblemSdk;
using System;
using ProblemSdk.Result;
using System.Threading;

/// <summary>
/// This is the problem for testing purposes!
/// </summary>
namespace Problems
{
    public class SampleProblem : Problem
    {
        private double a;
        private double b;
        private double c;

        public SampleProblem(): base()
        {
            Name = "Square roots problem";
            InputData.AddDataItem<double>("a");
            InputData.AddDataItem<double>("b");
            InputData.AddDataItem<double>("c");
        }

        protected override ProblemResult execute()
        {
            ProblemResult problemResult = new ProblemResult("Result", "", "");
            checkIfCanSolve(() => {
                double d = b * b - 4 * a * c;
                if (d >= 0)
                {
                    Thread.Sleep(2000);
                    double x1 = (-b - Math.Sqrt(d)) / (2 * a);
                    double x2 = (-b + Math.Sqrt(d)) / (2 * a);
                    problemResult.ResultData.Items.Add(
                        ResultDataItem.Builder.Create().ColumnTitles("x").Matrix(new object[,] { { x1 }, { x2 } }).Build());
                }
                problemResult.ResultData.Items.Add(
                    ResultDataItem.Builder.Create().ColumnTitles("x").Matrix(new object[,] { { "No real roots." } }).Build());
                });
            return problemResult;
        }

        protected override void updateData()
        {
            a = InputData.GetValue<double>(0);
            b = InputData.GetValue<double>(1);
            c = InputData.GetValue<double>(2);
        }
    }
}
