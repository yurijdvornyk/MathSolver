using System;
using System.Collections.Generic;

namespace ProblemSdk.Data
{
    public class SingleDataItem<T> : DataItem<T, T>
    {
        public SingleDataItem(
            string name,
            T defaultValue = default(T),
            bool isRequired = true,
            List<T> valueOptions = null,
            Predicate<T> validationPredicate = null) :
            base(name, defaultValue, isRequired, valueOptions, validationPredicate)
        { }

        public override bool IsSet()
        {
            return IsRequired && DefaultValue == null && Value == null;
        }

        public override void Reset()
        {
            Value = DefaultValue;
        }
    }
}