using System;
using ProblemSdk.Data;
using ProblemSdk.Result;

namespace ProblemSdk
{
    public abstract class Problem : IProblem
    {
        public virtual string Name { get; protected set; }
        public virtual string Description { get; protected set; }
        public virtual string Equation { get; protected set; }
        public ProblemData InputData { get; private set; }
        public ProblemResult Result { get; private set; }

        public Problem()
        {
            InputData = new ProblemData();
        }

        public void Solve()
        {
            SetInputData();
            UpdateData();
            Result = Execute(); // TODO: Do it with Rx.
        }

        public void CancelSolution()
        {
            // TODO: Add code after implementing solution with Rx.
        }

        public abstract void SetInputData();
        protected abstract ProblemResult Execute();
        protected abstract void UpdateData();

    }
}
