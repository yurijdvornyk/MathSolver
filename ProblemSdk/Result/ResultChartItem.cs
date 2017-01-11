using System.Collections.Generic;

namespace ProblemSdk.Result
{
    public class ResultChartItem
    {
        public string Title { get; set; }
        public List<ProblemChartPoint> ChartPoints { get; set; }

        public ResultChartItem() : this(string.Empty) { }

        public ResultChartItem(string title)
        {
            Title = title;
            ChartPoints = new List<ProblemChartPoint>();
        }

        public class Builder
        {
            private ResultChartItem item;

            public static Builder Create()
            {
                return new Builder();
            }

            public ResultChartItem Build()
            {
                return item;
            }

            private Builder()
            {
                item = new ResultChartItem();
            }

            public Builder Title(string title)
            {
                item.Title = title;
                return this;
            }

            public Builder Points(List<ProblemChartPoint> points)
            {
                item.ChartPoints = points;
                return this;
            }

            public Builder AddPoint(ProblemChartPoint point)
            {
                item.ChartPoints.Add(point);
                return this;
            }

            public Builder AddPoint(double x, double y)
            {
                item.ChartPoints.Add(new ProblemChartPoint(x, y));
                return this;
            }
        }
    }
}