using System.Collections.Generic;
using ProblemSdk.Result;
using Microsoft.Research.DynamicDataDisplay;

namespace IntegralEquationsApp.Components.Result.Charts
{
    public interface IResultChartsView : IView
    {
        void Set2dChart(ResultChart<Chart2dPoint> chartData, ChartPlotter plotter);
        void Set3dChart(ResultChart<Chart3dPoint> resultChart, SurfaceWebView surfaceView);
        void SetCharts(List<IResultChart> charts);
    }
}