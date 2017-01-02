using System;
using System.Collections.Generic;

namespace ProblemSdk.Data
{
    public class MultipleDataItem<T> : DataItem<T, List<T>>
    {
        public MultipleDataItem(
            string name,
            List<T> defaultValue = default(List<T>),
            bool isRequired = true,
            List<T> valueOptions = null,
            Predicate<List<T>> validationPredicate = null) :
            base(name, defaultValue, isRequired, valueOptions, validationPredicate)
        { }

        public override bool IsSet()
        {
            return IsRequired && Value.Count == 0;
        }

        public override void Reset()
        {
            Value = DefaultValue;
        }
    }
}