using System;
using System.Collections.Generic;

namespace ProblemSdk.Classes.Choice
{
    public class MultipleChoice<T> : Choice<T, IEnumerable<T>>, IMultipleChoice
    {
        public MultipleChoice(IEnumerable<T> options, List<T> defValue = null) : base(options, defValue) { }

        public IEnumerable<object> GetOptions()
        {
            return base.GetOptions();
        }

        public IEnumerable<object> GetValue()
        {
            List<object> result = new List<object>();
            new List<T>(base.GetValue()).ForEach(v => result.Add(v));
            return result;
        }

        public void SetValue(IEnumerable<object> value)
        {
            Value = new List<object>(value).ConvertAll(v => (T)v);
        }
    }
}
