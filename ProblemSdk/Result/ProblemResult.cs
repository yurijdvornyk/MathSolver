namespace ProblemSdk.Result
{
    public class ProblemResult
    {
        public ResultData ResultData { get; private set; }
        public ResultPlot ResultPlot { get; private set; }

        public ProblemResult()
        {
            ResultData = new ResultData();
            ResultPlot = new ResultPlot();
        }
    }
}