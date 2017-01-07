using IntegralEquationsApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProblemSdk;
using ProblemSdk.Result;
using IntegralEquationsApp.Components.Result.Data;
using IntegralEquationsApp.Components.Result.Charts;

namespace IntegralEquationsApp.Components.Result
{
    public class ResultPresenter : Presenter<IResultView>
    {
        public ResultPresenter(IResultView view) : base(view) { }

        public void OnStartProblemSolving(IProblem problem)
        {
            // TODO: Add code
        }
    }
}
