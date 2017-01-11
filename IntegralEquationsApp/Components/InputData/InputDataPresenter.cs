using IntegralEquationsApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            view.BuildLayoutForProblem(currentProblem != null ? currentProblem.InputData : null);
        }

        public void OnStartProblemSolving(IProblem problem) { }

        public void OnError(IProblem problem) { }

        public void OnProgressChanged(IProblem problem, double progress) { }

        public void OnProblemSolved(ProblemResult result) { }

        public void UpdateArguments()
        {
            DataManager.GetInstance().CurrentProblemArguments = view.GetItemValues();
        }
    }
}
