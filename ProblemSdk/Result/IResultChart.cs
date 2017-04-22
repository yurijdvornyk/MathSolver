using System;
using System.Collections.Generic;

namespace ProblemSdk.Result
{
    public interface IResultChart
    {
        string Title { get; }
        List<string> ChartLabels { get; }
        Type GetChartPointType();
    }
}