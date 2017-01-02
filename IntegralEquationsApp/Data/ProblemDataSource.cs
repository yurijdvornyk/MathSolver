using ProblemSdk;
using System.Collections.Generic;

namespace IntegralEquationsApp.Data
{
    public class ProblemDataSource
    {
        private static ProblemDataSource problemDataSource;

        public List<IProblem> Problems { get; private set; }
        public IProblem CurrentProblem
        {
            get
            {
                return currentProblem;
            }
            set
            {
                currentProblem = value;
                currentProblemListeners.ForEach(listener => listener.OnCurrentProblemChanged(currentProblem));
            }
        }

        private IProblem currentProblem;

        private List<ICurrentProblemListener> currentProblemListeners;

        private ProblemDataSource()
        {
            Problems = new List<IProblem>();
            currentProblemListeners = new List<ICurrentProblemListener>();
        }

        public void AddCurrentProblemListener(ICurrentProblemListener listener)
        {
            if (!currentProblemListeners.Contains(listener))
            {
                currentProblemListeners.Add(listener);
            }
        }

        public void RemoveCurrentProblemListener(ICurrentProblemListener listener)
        {
            if (currentProblemListeners.Contains(listener))
            {
                currentProblemListeners.Remove(listener);
            }
        }

        public static ProblemDataSource GetInstance()
        {
            if (problemDataSource == null)
            {
                problemDataSource = new ProblemDataSource();
            }
            return problemDataSource;
        }

        internal void setProblems(List<IProblem> problems)
        {
            Problems = problems;
        }
    }
}
