using IntegralEquationsApp.Data;

namespace IntegralEquationsApp.Components.ProblemSelector
{
    public class ProblemSelectorPresenter : MvpPresenter<IProblemSelectorView>
    {
        public ProblemSelectorPresenter(IProblemSelectorView view) : base(view) { }

        public void LoadProblems()
        {
            view.SetProblemList(ProblemDataSource.GetInstance().Problems);
        }
    }
}
