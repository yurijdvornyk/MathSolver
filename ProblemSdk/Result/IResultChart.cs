using System;
using System.Collections.Generic;

namespace ProblemSdk.Result
{
    public interface IResultChart
    {
        List<string> ChartLabels { get; }
        Type GetChartPointType();
    }
}