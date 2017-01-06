using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegralEquationsApp.Components.Result
{
    public class ResultPresenter : Presenter<IResultView>
    {
        public ResultPresenter(IResultView view) : base(view) { }
    }
}
