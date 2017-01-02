using System.Collections.Generic;

namespace ProblemSdk.Result
{
    public class ResultData
    {
        public string Title { get; set; }
        public List<ResultDataItem> Items { get; private set; }

        public ResultData() : this(string.Empty) { }

        public ResultData(string title)
        {
            Title = title;
            Items = new List<ResultDataItem>();
        }
    }
}