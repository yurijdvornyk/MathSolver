using System.Collections.Generic;

namespace ProblemSdk.Result
{
    public class ResultChartItem<T> where T: IChartPoint
    {
        public string Title { get; set; }
        public List<T> ChartPoints { get; private set; }

        public ResultChartItem() : this(string.Empty) { }

        public ResultChartItem(string title)
        {
            Title = title;
            ChartPoints = new List<T>();
        }

        public class Builder
        {
            private ResultChartItem<T> item;

            public static Builder Create()
            {
                return new Builder();
            }

            public ResultChartItem<T> Build()
            {
                return item;
            }

            private Builder()
            {
                item = new ResultChartItem<T>();
            }

            public Builder Title(string title)
            {
                item.Title = title;
                return this;
            }

            public Builder Points(List<T> points)
            {
                item.ChartPoints = points;
                return this;
            }

            public Builder AddPoint(T point)
            {
                item.ChartPoints.Add(point);
                return this;
            }
        }
    }
}