namespace ProblemSdk
{
    public interface ISolutionListener
    {
        void OnStartProblemSolving(IProblem problem);
        void OnProblemError(IProblem problem);
        void OnProblemProgressChanged(IProblem problem, double progress);
        void OnProblemSolved(IProblem problem);
    }
}
