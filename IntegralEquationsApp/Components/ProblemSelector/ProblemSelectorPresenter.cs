using IntegralEquationsApp.Data;
using ProblemSdk;

namespace IntegralEquationsApp.Components.ProblemSelector
{
    public class ProblemSelectorPresenter : Presenter<IProblemSelectorView>
    {
        private DataManager dataSource;

        public ProblemSelectorPresenter(IProblemSelectorView view) : base(view)
        {
            dataSource = DataManager.GetInstance();
        }

        public void LoadProblems()
        {
            view.SetProblemList(dataSource.Problems);
        }

        internal void ChangeCurrentProblem(IProblem currentProblem)
        {
            dataSource.CurrentProblem = currentProblem;
        }
    }
}
