using ProblemSdk.Data;
using ProblemSdk.Result;

namespace ProblemSdk
{
    public interface IProblem
    {
        string Name { get; }
        string Description { get; }
        /// <summary>
        /// Path to problem equation image.
        /// NOTE: Probably the easiest way to convert LaTeX equation into PNG is http://latex2png.com/.
        /// </summary>
        string Equation { get; }
        ProblemData InputData { get; }
        ProblemResult Result { get; }
        void Solve(params object[] args);
        void SolveAsync(params object[] args);
        void CancelSolution();
    }
}