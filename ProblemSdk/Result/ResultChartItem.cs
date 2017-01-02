using System.Collections.Generic;

namespace ProblemSdk.Result
{
    public class ResultChartItem
    {
        public string Title { get; set; }
        public List<Point> ChartPoints { get; set; }

        public ResultChartItem() : this(string.Empty) { }

        public ResultChartItem(string title)
        {
            Title = title;
            ChartPoints = new List<Point>();
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

            public Builder Points(List<Point> points)
            {
                item.ChartPoints = points;
                return this;
            }

            public Builder AddPoint(Point point)
            {
                item.ChartPoints.Add(point);
                return this;
            }

            public Builder AddPoint(double x, double y)
            {
                item.ChartPoints.Add(new Point(x, y));
                return this;
            }
        }
    }
}