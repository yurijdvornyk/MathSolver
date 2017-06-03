namespace IntegralEquationsApp.Components.Solution
{
    public interface ISolutionView: IView
    {
        void SetSolveButtonEnabled(bool enabled);
        void SetProgress(double progress);
        void StartProgress();
        void FinishProgress();
        void ShowError(string message);
    }
}
