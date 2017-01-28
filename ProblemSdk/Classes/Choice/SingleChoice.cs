using System;
using System.Collections.Generic;

namespace ProblemSdk.Classes.Choice
{
    public class SingleChoice<T> : Choice<T, T>, ISingleChoice
    {
        public SingleChoice(IEnumerable<T> options, T defValue = default(T)) : base(options, defValue) { }

        public IEnumerable<object> GetOptions()
        {
            List<object> result = new List<object>();
            Options.ForEach(option => result.Add(option));
            return result;
        }

        public object GetValue()
        {
            return Value;
        }

        public void SetValue(object value)
        {
            base.SetValue((T)value);
        }
    }
}
