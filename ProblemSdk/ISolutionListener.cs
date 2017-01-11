namespace ProblemSdk
{
    public interface ISolutionListener
    {
        void OnStartSolving(IProblem problem);
        void OnError(IProblem problem);
        void OnProgressChanged(IProblem problem, double progress);
        void OnProblemSolved(IProblem problem);
    }
}
