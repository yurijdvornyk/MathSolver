using ProblemSdk;
using System.Collections.Generic;

namespace IntegralEquationsApp.Data
{
    public class DataManager
    {
        private static DataManager problemDataManager;

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

        private DataManager()
        {
            Problems = new List<IProblem>();
            currentProblemListeners = new List<ICurrentProblemListener>();
        }

        public static DataManager GetInstance()
        {
            if (problemDataManager == null)
            {
                problemDataManager = new DataManager();
            }
            return problemDataManager;
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

        internal void setProblems(List<IProblem> problems)
        {
            Problems = problems;
        }
    }
}
