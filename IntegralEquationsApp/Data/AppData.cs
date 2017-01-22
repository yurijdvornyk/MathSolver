using Problems;
using ProblemSdk;
using System.Collections.Generic;

namespace IntegralEquationsApp.Data
{
    public class AppData
    {
        private static readonly List<IProblem> problems = new List<IProblem>()
        {
            new RectilinearSegment(),
            new ParameterizedRectilinearSegment(),
            new ArbitraryContour(),
            new ArbitraryContourAbsCase(),
            new ArbitraryContourExtendedCase(),
            new LinearSingularEquation()
        };

        public static void Initialize()
        {
            DataManager.GetInstance().setProblems(problems);
        }
    }
}
