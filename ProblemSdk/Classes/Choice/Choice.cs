using System.Collections.Generic;

namespace ProblemSdk.Classes.Choice
{
    public abstract class Choice<T, V> : IChoice<V>
    {
        public List<T> Options { get; protected set; }
        public V Value { get; set; }

        public Choice(IEnumerable<T> options, V defValue = default(V))
        {
            Options = new List<T>(options);
            Value = defValue;
        }

        public IEnumerable<object> GetOptions()
        {
            List<object> result = new List<object>();
            Options.ForEach(option => result.Add(option));
            return result;
        }

        public V GetValue()
        {
            return Value;
        }

        public void SetValue(V value)
        {
            Value = value;
        }
    }
}