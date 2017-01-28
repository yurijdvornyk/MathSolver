using System.Collections.Generic;
using ProblemSdk;

namespace IntegralEquationsApp.Components.InputData
{
    public interface IInputDataView : IView
    {
        void BuildLayoutForProblem(IProblem problem);
        List<object> GetItemValues();
    }
}
