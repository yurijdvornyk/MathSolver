using ProblemSdk;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public void OnProblemError(IProblem problem)
        {
            throw new NotImplementedException();
        }

        public void OnProblemProgressChanged(IProblem problem, double progress)
        {
            throw new NotImplementedException();
        }

        public void OnProblemSolved(IProblem problem)
        {
            solutionListeners.ForEach(listener => listener.OnProblemSolved(dataManager.CurrentProblem.Result));
        }

        public void OnStartProblemSolving(IProblem problem)
        {
            solutionListeners.ForEach(listener => listener.OnStartProblemSolving(dataManager.CurrentProblem));
        }

        public void RemoveSolutionListener(IProblemSolutionListener listener)
        {
            solutionListeners.Remove(listener);
        }

        public void StartProblemSolving()
        {
            IProblem problem = dataManager.CurrentProblem;
            solutionListeners.ForEach(listener => listener.OnStartProblemSolving(problem));
            problem.Solve();
        }
    }
}
