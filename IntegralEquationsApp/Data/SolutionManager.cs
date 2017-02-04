using ProblemSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegralEquationsApp.Data
{
    public class SolutionManager: ISolutionListener
    {
        private static SolutionManager solutionManager;

        private DataManager dataManager;
        private List<IProblemSolutionListener> solutionListeners;

        private SolutionManager()
        {
            dataManager = DataManager.GetInstance();
            solutionListeners = new List<IProblemSolutionListener>();
            SolutionNotifier.GetInstance().AddListener(this);
        }

        public static SolutionManager GetInstance()
        {
            if (solutionManager == null)
            {
                solutionManager = new SolutionManager();
            }
            return solutionManager;
        }

        public void AddSolutionListener(IProblemSolutionListener listener)
        {
            solutionListeners.Add(listener);
        }

        public void OnError(IProblem problem, Exception error)
        {
            solutionListeners.ForEach(listener => listener.OnError(error));
        }

        public void OnProgressChanged(IProblem problem, double progress)
        {
        }

        public void OnProblemSolved(IProblem problem)
        {
            solutionListeners.ForEach(listener => listener.OnProblemSolved(dataManager.CurrentProblem.Result));
        }

        public void OnStartSolving(IProblem problem)
        {
            solutionListeners.ForEach(listener => listener.OnStartProblemSolving(dataManager.CurrentProblem));
        }

        public void RemoveSolutionListener(IProblemSolutionListener listener)
        {
            solutionListeners.Remove(listener);
        }

        public void StartProblemSolving()
        {
            dataManager.SolveProblem();
        }
    }
}
