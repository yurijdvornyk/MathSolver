using ProblemSdk;
using System.Collections.Generic;

namespace IntegralEquationsApp.Components.ProblemSelector
{
    public interface IProblemSelectorView : IView
    {
        void SetProblemList(List<IProblem> problems);
    }
}
