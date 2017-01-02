namespace ProblemSdk.Result
{
    public class ProblemResult
    {
        public ResultData ResultData { get; private set; }
        public ResultChart ResultChart { get; private set; }

        public ProblemResult(string xChartAxisTitle, string yChartAxisTitle) : this(string.Empty, xChartAxisTitle, yChartAxisTitle) { }

        public ProblemResult(string chartTitle, string xChartAxisTitle, string yChartAxisTitle)
        {
            ResultData = new ResultData();
            ResultChart = new ResultChart(chartTitle, xChartAxisTitle, yChartAxisTitle);
        }
    }
}
