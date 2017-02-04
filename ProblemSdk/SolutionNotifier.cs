using System;
using System.Collections.Generic;

namespace ProblemSdk
{
    public class SolutionNotifier
    {
        private static SolutionNotifier solutionNotifier;

        private List<ISolutionListener> solutionListeners;

        private SolutionNotifier()
        {
            solutionListeners = new List<ISolutionListener>();
        }

        public static SolutionNotifier GetInstance()
        {
            if (solutionNotifier == null)
            {
                solutionNotifier = new SolutionNotifier();
            }
            return solutionNotifier;
        }

        public void AddListener(ISolutionListener listener)
        {
            solutionListeners.Add(listener);
        }

        public void RemoveListener(ISolutionListener listener)
        {
            solutionListeners.Remove(listener);
        }

        internal void NotifyStartProblemSolving(IProblem problem)
        {
            solutionListeners.ForEach(listener => listener.OnStartSolving(problem));
        }

        internal void NotifySolvingProgressChanged(IProblem problem, double progress)
        {
            solutionListeners.ForEach(listener => listener.OnProgressChanged(problem, progress));
        }

        internal void NotifyProblemSolved(IProblem problem)
        {
            solutionListeners.ForEach(listener => listener.OnProblemSolved(problem));
        }

        internal void NotifyError(IProblem problem, Exception error)
        {
            solutionListeners.ForEach(listener => listener.OnError(problem, error));
        }
    }
}
