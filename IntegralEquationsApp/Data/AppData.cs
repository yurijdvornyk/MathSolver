using Problems;
using ProblemSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegralEquationsApp.Data
{
    public class AppData
    {
        private static readonly List<IProblem> problems = new List<IProblem>()
        {
            new RectilinearSegment()
        };

        public static void Initialize()
        {
            DataManager.GetInstance().setProblems(problems);
        }
    }
}
