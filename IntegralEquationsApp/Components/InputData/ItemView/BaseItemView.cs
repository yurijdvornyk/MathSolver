using System;
using System.Windows.Controls;

namespace IntegralEquationsApp.Components.InputData.ItemView
{
    /// <summary>
    /// General interface for input data item views.
    /// </summary>
    public abstract class BaseItemView : UserControl
    {
        public abstract void SetItemValue(object value);
        public abstract object GetItemValue();
    }
}