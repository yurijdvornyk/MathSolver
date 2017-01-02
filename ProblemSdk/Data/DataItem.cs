using System;
using System.Collections.Generic;

namespace ProblemSdk.Data
{
    /// <summary>
    /// Defines Problem input data item.
    /// </summary>
    /// <typeparam name="V">Value type (string, number, etc.)</typeparam>
    /// <typeparam name="T">Item Type (object or the list of objects)</typeparam>
    public abstract class DataItem<V, T> : IDataItem
    {
        private const string INVALID_CAST_EXCEPTION_MESSAGE = "Cannot cast given object to {0}";

        public string Name { get; set; }
        public T DefaultValue { get; set; }
        public T Value { get; set; }
        public List<V> ValueOptions { get; set; }
        public bool IsRequired { get; set; }
        public Predicate<T> ValidationPredicate { get; set; }

        public DataItem(
            string name, 
            T defaultValue = default(T),
            bool isRequired = true, 
            List<V> valueOptions = null, 
            Predicate<T> validationPredicate = null)
        {
            Name = name;
            DefaultValue = defaultValue;
            ValueOptions = valueOptions != null ? valueOptions : new List<V>();
            IsRequired = isRequired;
            Value = default(T);
            ValidationPredicate = validationPredicate;
        }

        public void Apply(out T outerValue)
        {
            outerValue = Value;
        }

        public Type GetDataItemType()
        {
            return typeof(T).GetType();
        }

        public object GetValue()
        {
            return Value;
        }

        public void SetValue(object value)
        {
            if (value is T)
            {
                Value = (T)value;
            }
            else
            {
                throw new InvalidCastException(string.Format(INVALID_CAST_EXCEPTION_MESSAGE, typeof(T).GetType().Name));
            }
        }

        public bool IsValid()
        {
            return ValidationPredicate.Invoke(Value);
        }

        public abstract bool IsSet();

        public abstract void Reset();
    }
}