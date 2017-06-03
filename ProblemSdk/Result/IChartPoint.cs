using System.Collections.Generic;

namespace ProblemSdk.Result
{
    public interface IChartPoint
    {
        int Dimensions { get; }
        List<double> GetValues();
    }
}
