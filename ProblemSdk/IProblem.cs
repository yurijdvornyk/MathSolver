using ProblemSdk.Data;
using ProblemSdk.Result;

namespace ProblemSdk
{
    public interface IProblem
    {
        string Name { get; }
        string Description { get; }
        string Equation { get; }
        ProblemData InputData { get; }
        ProblemResult Result { get; }
        void Solve(params object[] args);
        void CancelSolution();
    }
}