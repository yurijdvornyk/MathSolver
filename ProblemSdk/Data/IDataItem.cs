using System;

namespace ProblemSdk.Data
{
    public interface IDataItem
    {
        string Name { get; }
        Type GetDataItemType();
        bool IsSet();
        void Reset();
        object GetValue();
        void SetValue(object value);
        bool IsValid();
    }
}