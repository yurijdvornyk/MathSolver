using ProblemSdk.Error;
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
            try
            {
                return (T)DataItems[itemPosition].GetValue();
            }
            catch (IndexOutOfRangeException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throwDataItemTypeMismatchException<T>(itemPosition, ex);
            }
            return default(T);
        }

        public void GetValue<T>(int itemPosition, out T outerObject)
        {
            outerObject = GetValue<T>(itemPosition);
        }

        public void AddDataItem<T>(
            string name, 
            T defaultValue = default(T), 
            bool isRequired = true, 
            Predicate<T> validationPredicate = null)
        {
            DataItems.Add(new DataItem<T>(name, defaultValue, isRequired, validationPredicate));
        }

        public void AddDataItemAtPosition<T>(
            int position,
            string name,
            T defaultValue = default(T),
            bool isRequired = true,
            Predicate<T> validationPredicate = null)
        {
            addEmptyItemsIfNeed(position);
            DataItems.Insert(position, new DataItem<T>(name, defaultValue, isRequired, validationPredicate));
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

        private void throwDataItemTypeMismatchException<T>(int itemPosition, Exception originalException)
        {
            string validType = typeof(T).Name;
            string invalidType = DataItems[itemPosition].GetDataItemType().Name;
            throw new DataItemTypeMismatchException(DataItems[itemPosition].Name, typeof(T).Name, invalidType, originalException.Message);
        }
    }
}