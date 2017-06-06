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
            new PotentialAbsForRectilinearSegment(),
            //new ParameterizedRectilinearSegment(),
            new ArbitraryContour(),
            new ArbitraryContourAbsCase(),
            new ArbitraryContourExtendedCase(),
            new LinearSingularEquation()
            //new SampleProblem()
        };

        public static void Initialize()
        {
            DataManager.GetInstance().setProblems(problems);
        }
    }
}
