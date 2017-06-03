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
            view.SetCharts(result.ResultPlot.Charts);
        }

        public void OnStartProblemSolving(IProblem problem) { }
    }
}
