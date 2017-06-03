using System;
using ProblemSdk;
using IntegralEquationsApp.Data;
using ProblemSdk.Result;

namespace IntegralEquationsApp.Components.Result
{
    public class ResultPresenter : Presenter<IResultView>, IProblemSolutionListener
    {
        public ResultPresenter(IResultView view) : base(view)
        {
            SolutionManager.GetInstance().AddSolutionListener(this);
        }

        public void OnError(object erorr) { }

        public void OnProblemSolved(ProblemResult result)
        {
            view.ShowResultTabs();
        }

        public void OnStartProblemSolving(IProblem problem) { }
    }
}
