using System.Collections.Generic;

namespace IntegralEquationsApp.Components.Result.Data
{
    public interface IResultDataView : IView
    {
        void SetProblemResult(string title, List<ResultDataTable> dataTables);
    }
}
