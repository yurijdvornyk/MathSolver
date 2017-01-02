using ProblemSdk;
using ProblemSdk.Data;

namespace IntegralEquationsApp.Components.InputData
{
    public interface IInputDataView : IView
    {
        void buildLayoutForProblem(ProblemData problemData);
    }
}
