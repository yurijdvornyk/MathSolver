using ProblemSdk.Result;

namespace IntegralEquationsApp.Components.Result.Charts
{
    public interface IResultChartsView : IView
    {
        void set2dChart(ResultChart<Chart2dPoint> chartData);
        void set3dChart(ResultChart<Chart3dPoint> resultChart);
    }
}
