using System;
using System.Collections.Generic;

namespace ProblemSdk.Data
{
    /// <summary>
    /// Defines Problem input data item.
    /// </summary>
    /// <typeparam name="V">Value type (string, number, etc.)</typeparam>
    /// <typeparam name="T">Item Type (object or the list of objects)</typeparam>
    public /*abstract*/ class DataItem<T> : IDataItem
    {
        private const string INVALID_CAST_EXCEPTION_MESSAGE = "Cannot cast given object to {0}";

        public string Name { get; set; }
        public T DefaultValue { get; set; }
        public T Value { get; set; }
        public bool IsRequired { get; set; }
        public Predicate<T> ValidationPredicate { get; set; }

        public DataItem(
            string name, 
            T defaultValue = default(T),
            bool isRequired = true, 
            Predicate<T> validationPredicate = null)
        {
            Name = name;
            DefaultValue = defaultValue;
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
            return typeof(T);
        }

        public object GetValue()
        {
            return Value;
        }

        public object GetDefaultValue()
        {
            return DefaultValue;
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

        public bool IsSet()
        {
            return IsRequired && DefaultValue == null && Value == null;
        }

        public void Reset()
        {
            Value = DefaultValue;
        }
    }
}