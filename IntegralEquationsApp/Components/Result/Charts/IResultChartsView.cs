using ProblemSdk.Result;

namespace IntegralEquationsApp.Components.Result.Charts
{
    public interface IResultChartsView : IView
    {
        void Set2dChart(ResultChart<Chart2dPoint> chartData);
        void Set3dChart(ResultChart<Chart3dPoint> resultChart);
    }
}
