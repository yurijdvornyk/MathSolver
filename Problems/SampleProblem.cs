using ProblemSdk;
using System;
using ProblemSdk.Result;
using System.Threading;
using ProblemSdk.Data;
using System.Collections.Generic;
using ProblemSdk.Classes.Choice;

/// <summary>
/// This is the problem for testing purposes!
/// </summary>
namespace Problems
{
    public class SampleProblem : Problem
    {
        private string function;
        private double a;
        private double b;
        private int n;

        private Dictionary<string, Func<double, double>> functions = new Dictionary<string, Func<double, double>>()
        {
            { "Sin", x => Math.Sin(x) },
            { "Cos", x => Math.Cos(x) }
        };
        
        public SampleProblem(): base()
        {
            Name = "Function Tabulation";
            Equation = null; // You can add path to equation image in PNG, GIF or JPG

            IDataItem function = DataItemBuilder<ISingleChoice>
                .Create()
                .Name("function")
                .DefValue(new SingleChoice<string>(functions.Keys, new List<string>(functions.Keys)[0]))
                .Build();
            IDataItem a = DataItemBuilder<double>.Create().Name("a").Hint("Left bound").Build();
            IDataItem b = DataItemBuilder<double>.Create().Name("b").Hint("Right bound").Build();

            InputData.AddDataItem(function);
            InputData.AddDataItemAt(1, a);
            InputData.AddDataItemAt(2, b);
            InputData.AddDataItem("n", 100, true, x => x > 0);
        }

        protected override ProblemResult execute()
        {
            double step = (b - a) / n;
            int pointsCount = n + 1;
            object[,] dataPoints = new object[pointsCount, 2];
            List<Chart2dPoint> chartPoints = new List<Chart2dPoint>();
            // Calculate values and save them to dataPoints matrix and chartPoints list
            for (int i = 0; i < pointsCount; i++)
            {
                double x = a + i * step;
                double y = functions[function](x);
                dataPoints[i, 0] = x;
                dataPoints[i, 1] = y;
                chartPoints.Add(new Chart2dPoint(x, y));
            }

            // Create ProblemResult and set values to it
            ProblemResult result = new ProblemResult();
            result.ResultData.Items.Add(ResultDataItem.Builder.Create().ColumnTitles("x", "y").Matrix(dataPoints).Build());

            ResultChart<Chart2dPoint> chart = new ResultChart<Chart2dPoint>("f(x)", new List<string>() { "x", "y" });
            chart.Items.Add(ResultChartItem<Chart2dPoint>.Builder.Create().Points(chartPoints).Build());
            result.ResultPlot.Charts.Add(chart);

            return result;
        }

        protected override void updateData()
        {
            function = InputData.GetValue<ISingleChoice>(0).GetValue().ToString();
            InputData.GetValue(1, out a);
            InputData.GetValue(2, out b);
            n = InputData.GetValue<int>(3);
        }
    }
}
