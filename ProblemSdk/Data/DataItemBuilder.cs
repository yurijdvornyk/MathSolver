using System;

namespace ProblemSdk.Data
{
    public class DataItemBuilder<T>
    {
        private DataItem<T> dataItem;

        private DataItemBuilder()
        {
            dataItem = new DataItem<T>(null);
        }

        public static DataItemBuilder<T> Create()
        {
            return new DataItemBuilder<T>();
        }

        public DataItemBuilder<T> Name(string name)
        {
            dataItem.Name = name;
            return this;
        }

        public DataItemBuilder<T> Hint(string hint)
        {
            dataItem.Hint = hint;
            return this;
        }

        public DataItemBuilder<T> DefValue(T defaultValue)
        {
            dataItem.DefaultValue = defaultValue;
            return this;
        }

        public DataItemBuilder<T> Required(bool isRequired)
        {
            dataItem.IsRequired = isRequired;
            return this;
        }

        public DataItemBuilder<T> Validation(Predicate<T> validationPredicate)
        {
            dataItem.ValidationPredicate = validationPredicate;
            return this;
        }

        public DataItem<T> Build()
        {
            return dataItem;
        }
    }
}
