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
        void SetInputData();
        void Solve();
        void CancelSolution();
    }
}