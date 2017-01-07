using ProblemSdk.Result;

namespace IntegralEquationsApp.Components.Result.Charts
{
    public interface IResultChartsView : IView
    {
        void setChartData(ResultChart chartData);
    }
}
