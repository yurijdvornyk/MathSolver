using ProblemSdk;

namespace IntegralEquationsApp.Data
{
    public interface ICurrentProblemListener
    {
        void OnCurrentProblemChanged(IProblem currentProblem);
    }
}
