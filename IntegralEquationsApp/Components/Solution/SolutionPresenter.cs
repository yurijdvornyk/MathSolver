using System;
using IntegralEquationsApp.Data;
using ProblemSdk;
using ProblemSdk.Result;

namespace IntegralEquationsApp.Components.Solution
{
    public class SolutionPresenter : Presenter<ISolutionView>, ICurrentProblemListener, IProblemSolutionListener
    {
        public SolutionPresenter(ISolutionView view) : base(view)
        {
            DataManager.GetInstance().AddCurrentProblemListener(this);
            SolutionManager.GetInstance().AddSolutionListener(this);
        }

        public void OnCurrentProblemChanged(IProblem currentProblem)
        {
            view.SetSolveButtonEnabled(currentProblem != null);
        }

        public void OnError(object erorr)
        {
            view.SetSolveButtonEnabled(true);
            view.SetProgress(0);
        }

        public void OnProblemSolved(ProblemResult result)
        {
            view.SetSolveButtonEnabled(true);
            view.FinishProgress();
        }

        public void OnStartProblemSolving(IProblem problem)
        {
            view.SetSolveButtonEnabled(false);
            view.StartProgress();
        }

        public void StartSolving()
        {
            SolutionManager.GetInstance().StartProblemSolving();
        }
    }
}
