using System;
using IntegralEquationsApp.Data;
using ProblemSdk;
using ProblemSdk.Result;

namespace IntegralEquationsApp.Components.Result.Charts
{
    public class ResultChartsPresenter : Presenter<IResultChartsView>, IProblemSolutionListener
    {
        public ResultChartsPresenter(IResultChartsView view) : base(view)
        {
            SolutionManager.GetInstance().AddSolutionListener(this);
        }

        public void OnError(object erorr)
        {
            // TODO: Add error handling
        }

        public void OnProblemSolved(ProblemResult result)
        {
            IResultChart chart = result.ResultChart;
            if (chart.GetChartPointType() == typeof(Chart2dPoint))
            {
                view.set2dChart(chart as ResultChart<Chart2dPoint>);
            } else if (chart.GetChartPointType() == typeof(Chart3dPoint))
            {
                view.set3dChart(chart as ResultChart<Chart3dPoint>);
            }
        }

        public void OnStartProblemSolving(IProblem problem) { }
    }
}
