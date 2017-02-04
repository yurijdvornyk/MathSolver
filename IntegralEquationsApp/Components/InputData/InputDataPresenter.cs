using System;
using IntegralEquationsApp.Data;
using ProblemSdk;
using ProblemSdk.Result;

namespace IntegralEquationsApp.Components.InputData
{
    public class InputDataPresenter : Presenter<IInputDataView>, ICurrentProblemListener, IProblemSolutionListener, IProblemArgumentsUpdater
    {
        public InputDataPresenter(IInputDataView view) : base(view)
        {
            DataManager.GetInstance().AddCurrentProblemListener(this);
            DataManager.GetInstance().ArgumentsUpdater = this;
            SolutionManager.GetInstance().AddSolutionListener(this);
        }

        public void OnCurrentProblemChanged(IProblem currentProblem)
        {
            view.BuildLayoutForProblem(currentProblem);
        }

        public void OnStartProblemSolving(IProblem problem) { }

        public void OnError(IProblem problem) { }

        public void OnProgressChanged(IProblem problem, double progress) { }

        public void OnProblemSolved(ProblemResult result) { }

        public void UpdateArguments()
        {
            DataManager.GetInstance().CurrentProblemArguments = view.GetItemValues();
        }

        public void OnError(object erorr)
        {
            // TODO: Add error handling
        }
    }
}
