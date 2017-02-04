using ProblemSdk.Data;
using ProblemSdk.Result;
using ProblemSdk.Utils;
using System;
using System.ComponentModel;
using System.Threading;

namespace ProblemSdk
{
    public abstract class Problem : IProblem
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public string Equation { get; protected set; }
        public ProblemData InputData { get; private set; }
        public ProblemResult Result { get; private set; }
        protected SolutionNotifier solutionNotifier { get; private set; }
        private CancellationTokenSource cancellationTokenSource;
        private BackgroundWorker backgroundWorker;

        public Problem()
        {
            InputData = new ProblemData();
            solutionNotifier = SolutionNotifier.GetInstance();
        }

        public void Solve(params object[] args)
        {
            setInputData(args);
            updateData();
            cancellationTokenSource = new CancellationTokenSource();
            solutionNotifier.NotifyStartProblemSolving(this);
            Result = execute();
            cancellationTokenSource = null;
            solutionNotifier.NotifyProblemSolved(this);
        }

        public void SolveAsync(params object[] args)
        {
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            backgroundWorker.RunWorkerAsync();
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

        protected void checkIfCanSolve(Action action)
        {
            if (ValidationUtils.NotNull(cancellationTokenSource) && 
                ValidationUtils.NotNull(cancellationTokenSource.Token) && 
                !cancellationTokenSource.Token.IsCancellationRequested)
            {
                action.Invoke();
            }
        }

        protected abstract ProblemResult execute();
        protected abstract void updateData();

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Solve(e);
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                solutionNotifier.NotifyError(this, e.Error);
            }
            else if (e.Error != null)
            {
                SolutionNotifier.GetInstance().NotifyError(this, e.Error);
            }
        }
    }
}
