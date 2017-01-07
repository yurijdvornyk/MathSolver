using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void NotifyStartProblemSolving(IProblem problem)
        {
            solutionListeners.ForEach(listener => listener.OnStartProblemSolving(problem));
        }

        public void NotifySolvingProgressChanged(IProblem problem, double progress)
        {
            solutionListeners.ForEach(listener => listener.OnProblemProgressChanged(problem, progress));
        }

        public void NotifyProblemSolved(IProblem problem)
        {
            solutionListeners.ForEach(listener => listener.OnProblemSolved(problem));
        }
    }
}
