using IntegralEquationsApp.Data;
using ProblemSdk;

namespace IntegralEquationsApp.Components.Solution
{
    public class SolutionPresenter : Presenter<ISolutionView>, ICurrentProblemListener
    {
        public SolutionPresenter(ISolutionView view) : base(view)
        {
            DataManager.GetInstance().AddCurrentProblemListener(this);
        }

        public void OnCurrentProblemChanged(IProblem currentProblem)
        {
            view.SetSolveButtonEnabled(currentProblem != null);
        }

        public void StartSolving()
        {
            SolutionManager.GetInstance().StartProblemSolving();
        }
    }
}
