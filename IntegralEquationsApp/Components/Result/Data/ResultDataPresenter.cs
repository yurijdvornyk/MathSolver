using IntegralEquationsApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProblemSdk;
using ProblemSdk.Result;

namespace IntegralEquationsApp.Components.Result.Data
{
    public class ResultDataPresenter : Presenter<IResultDataView>, IProblemSolutionListener
    {
        public ResultDataPresenter(IResultDataView view) : base(view)
        {
            SolutionManager.GetInstance().AddSolutionListener(this);
        }

        public void OnError(object erorr)
        {
            // TODO: Add error handling
        }

        public void OnProblemSolved(ProblemResult result)
        {
            List<ResultDataTable> tables = new List<ResultDataTable>();
            ResultData resultData = result.ResultData;
            result.ResultData.Items.ForEach(item =>
            {
                ResultDataTable table = new ResultDataTable(result.ResultData.Title);
                table.SetDataColumns(item.ColumnTitles);
                table.SetData(item.GetResultItemMatrix());
                tables.Add(table);
            });
            view.SetProblemResult(result.ResultData.Title, tables);
        }

        public void OnStartProblemSolving(IProblem problem)
        {
            // TODO: Add code
        }
    }
}
