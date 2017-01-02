using System;
using System.Collections.Generic;

namespace ProblemSdk.Data
{
    public class ProblemData
    {
        public List<IDataItem> DataItems { get; set; }

        public ProblemData()
        {
            DataItems = new List<IDataItem>();
        }

        public bool IsSet()
        {
            return DataItems.TrueForAll(dataItem => dataItem.IsSet());
        }

        public void Reset()
        {
            DataItems.ForEach(dataItem => dataItem.Reset());
        }

        public void SetValue(int itemPosition, object value)
        {
            DataItems[itemPosition].SetValue(value);
        }

        public T GetValue<T>(int itemPosition)
        {
            return (T) DataItems[itemPosition].GetValue();
        }

        public void GetValue<T>(int itemPosition, out T outerObject)
        {
            IDataItem dataItem = DataItems[itemPosition];
            if (typeof(T).Equals(dataItem.GetValue().GetType()))
            {
                outerObject = (T) dataItem.GetValue();
            }
            else
            {
                throw new ArgumentException(string.Format("Type mismatch! Type of {0} is {1}.", dataItem.Name, dataItem.GetType().Name));
            }
        }

        public void AddSingleDataItem<T>(
            string name, 
            T defaultValue = default(T), 
            bool isRequired = true, 
            List<T> valueOptions = null,
            Predicate<T> validationPredicate = null)
        {
            DataItems.Add(new SingleDataItem<T>(name, defaultValue, isRequired, valueOptions, validationPredicate));
        }

        public void AddMultipleDataItem<T>(
            string name, 
            List<T> defaultValues = default(List<T>), 
            bool isRequired = true, 
            List<T> valueOptions = null,
            Predicate<List<T>> validationPredicate = null)
        {
            DataItems.Add(new MultipleDataItem<T>(name, defaultValues, isRequired, valueOptions, validationPredicate));
        }

        public void AddSingleDataItemAtPosition<T>(
            int position,
            string name,
            T defaultValue = default(T),
            bool isRequired = true,
            List<T> valueOptions = null,
            Predicate<T> validationPredicate = null)
        {
            addEmptyItemsIfNeed(position);
            DataItems.Insert(position, new SingleDataItem<T>(name, defaultValue, isRequired, valueOptions, validationPredicate));
        }

        public void AddMultipleDataItemAtPosition<T>(
            int position,
            string name,
            List<T> defaultValues = default(List<T>),
            bool isRequired = true,
            List<T> valueOptions = null,
            Predicate<List<T>> validationPredicate = null)
        {
            addEmptyItemsIfNeed(position);
            DataItems.Insert(position, new MultipleDataItem<T>(name, defaultValues, isRequired, valueOptions, validationPredicate));
        }

        private void addEmptyItemsIfNeed(int position)
        {
            if (position > DataItems.Count - 1)
            {
                while (position == DataItems.Count - 1)
                {
                    DataItems.Add(null);
                }
            }
        }
    }
}