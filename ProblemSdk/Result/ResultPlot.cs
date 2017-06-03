using System.Collections.Generic;

namespace ProblemSdk.Result
{
    public class ResultPlot
    {
        public string Title { get; set; }
        public List<IResultChart> Charts { get; private set; }

        public ResultPlot() : this(string.Empty) { }

        public ResultPlot(string title)
        {
            Title = title;
            Charts = new List<IResultChart>();
        }
    }
}