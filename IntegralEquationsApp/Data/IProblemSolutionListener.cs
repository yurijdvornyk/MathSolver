using ProblemSdk;
using ProblemSdk.Result;

namespace IntegralEquationsApp.Data
{
    public interface IProblemSolutionListener
    {
        void OnStartProblemSolving(IProblem problem);

        void OnProblemSolved(ProblemResult result);
    }
}
