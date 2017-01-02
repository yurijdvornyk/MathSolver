using System.Collections.Generic;

namespace ProblemSdk.Result
{
    public class ResultChart
    {
        public string Title { get; set; }
        public string XAxisTitle { get; set; }
        public string YAxisTitle { get; set; }
        public List<ResultChartItem> Items { get; private set; }

        public ResultChart(string title, string xAxisTitle, string yAxisTitle)
        {
            Title = title;
            XAxisTitle = xAxisTitle;
            YAxisTitle = yAxisTitle;
            Items = new List<ResultChartItem>();
        }
    }
}