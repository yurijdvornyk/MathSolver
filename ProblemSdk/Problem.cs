using ProblemSdk.Data;
using ProblemSdk.Result;

namespace ProblemSdk
{
    public abstract class Problem : IProblem
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public string Equation { get; protected set; }
        public ProblemData InputData { get; private set; }
        public ProblemResult Result { get; private set; }
        public SolutionNotifier SolutionNotifier { get; private set; }

        public Problem()
        {
            InputData = new ProblemData();
            SolutionNotifier = SolutionNotifier.GetInstance();
        }

        public void Solve(params object[] args)
        {
            setInputData(args);
            updateData();
            SolutionNotifier.NotifyStartProblemSolving(this);
            Result = execute(); // TODO: Do it with Rx.
            SolutionNotifier.NotifyProblemSolved(this);
        }

        public void CancelSolution()
        {
            // TODO: Add code after implementing solution with Rx.
        }

        public override string ToString()
        {
            return Name;
        }

        protected virtual void setInputData(params object[] args)
        {
            for (int i = 0; i < args.Length; ++i)
            {
                InputData.SetValue(i, args[i]);
            }
        }

        protected abstract ProblemResult execute();
        protected abstract void updateData();
    }
}
