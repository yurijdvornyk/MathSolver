using ProblemSdk;
using System.Collections.Generic;

namespace IntegralEquationsApp.Data
{
    public class ProblemDataSource
    {
        private static ProblemDataSource problemDataSource;

        public List<IProblem> Problems { get; private set; }
        public IProblem CurrentProblem { get; set; }

        private ProblemDataSource()
        {
            Problems = new List<IProblem>();
        }

        public static ProblemDataSource GetInstance()
        {
            if (problemDataSource == null)
            {
                problemDataSource = new ProblemDataSource();
            }
            return problemDataSource;
        }
    }
}
