using System;
using System.Collections.Generic;

namespace ProblemSdk.Result
{
    public class ResultChart<T> : IResultChart where T : IChartPoint
    {
        public string Title { get; set; }
        public List<ResultChartItem<T>> Items { get; private set; }
        public List<string> ChartLabels { get; private set; }

        public ResultChart(string title, List<string> chartLabels)
        {
            Title = title;
            ChartLabels = new List<string>();
            if (chartLabels != null)
            {
                chartLabels.ForEach(label => ChartLabels.Add(label));
            }
            Items = new List<ResultChartItem<T>>();
        }

        public Type GetChartPointType()
        {
            return typeof(T);
        }
    }
}