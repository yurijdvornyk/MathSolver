using IntegralEquationsApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProblemSdk;

namespace IntegralEquationsApp.Components.InputData
{
    public class InputDataPresenter : Presenter<IInputDataView>, ICurrentProblemListener
    {
        public InputDataPresenter(IInputDataView view) : base(view)
        {
            DataManager.GetInstance().AddCurrentProblemListener(this);
        }

        public void OnCurrentProblemChanged(IProblem currentProblem)
        {
            view.BuildLayoutForProblem(currentProblem != null ? currentProblem.InputData : null);
        }
    }
}
