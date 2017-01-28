using System.Collections.Generic;

namespace ProblemSdk.Classes.Choice
{
    public interface IChoice<T>
    {
        IEnumerable<object> GetOptions();
        T GetValue();
        void SetValue(T value);
    }
}
