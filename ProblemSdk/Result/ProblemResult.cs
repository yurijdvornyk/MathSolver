namespace ProblemSdk.Result
{
    public class ProblemResult
    {
        public ResultData ResultData { get; private set; }
        public IResultChart ResultChart { get; private set; }

        public ProblemResult()
        {
            ResultData = new ResultData();
        }

        public void SetChart<T>(T resultChart) where T : IResultChart
        {
            ResultChart = resultChart;
        }
    }
}