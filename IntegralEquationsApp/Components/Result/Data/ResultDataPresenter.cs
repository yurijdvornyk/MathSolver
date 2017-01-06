using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegralEquationsApp.Components.Result.Data
{
    public class ResultDataPresenter : Presenter<IResultDataView>
    {
        public ResultDataPresenter(IResultDataView view) : base(view) { }
    }
}
