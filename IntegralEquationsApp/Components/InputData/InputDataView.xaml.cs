using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProblemSdk;
using ProblemSdk.Data;
using IntegralEquationsApp.Components.InputData.ItemView;

namespace IntegralEquationsApp.Components.InputData
{
    /// <summary>
    /// Interaction logic for InputDataView.xaml
    /// </summary>
    public partial class InputDataView : UserControl, IInputDataView
    {
        private readonly int INPUT_DATA_ITEM_MARGIN = 5;

        private InputDataPresenter presenter;

        public InputDataView()
        {
            InitializeComponent();
            presenter = new InputDataPresenter(this);
        }

        public void BuildLayoutForProblem(ProblemData problemData)
        {
            contentArea.Children.Clear();
            contentArea.ColumnDefinitions.Clear();
            contentArea.RowDefinitions.Clear();
            if (problemData == null)
            {
                return;
            }
            ColumnDefinition titleColumn = new ColumnDefinition();
            titleColumn.Width = GridLength.Auto;
            contentArea.ColumnDefinitions.Add(titleColumn);
            ColumnDefinition valueColumn = new ColumnDefinition();
            contentArea.ColumnDefinitions.Add(valueColumn);
            foreach (IDataItem item in problemData.DataItems)
            {
                RowDefinition row = new RowDefinition();
                row.Height = GridLength.Auto;
                contentArea.RowDefinitions.Add(row);

                FrameworkElement itemTitle = getItemTitleView(item);
                itemTitle.Margin = new Thickness(INPUT_DATA_ITEM_MARGIN);
                itemTitle.HorizontalAlignment = HorizontalAlignment.Left;
                itemTitle.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetColumn(itemTitle, 0);
                Grid.SetRow(itemTitle, contentArea.RowDefinitions.Count - 1);
                contentArea.Children.Add(itemTitle);

                FrameworkElement itemValue = getItemValueView(item);
                itemValue.Margin = new Thickness(INPUT_DATA_ITEM_MARGIN);
                itemValue.HorizontalAlignment = HorizontalAlignment.Stretch;
                itemValue.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetColumn(itemValue, 1);
                Grid.SetRow(itemValue, contentArea.RowDefinitions.Count - 1);
                contentArea.Children.Add(itemValue);
            }
        }

        public List<object> GetItemValues()
        {
            List<object> result = new List<object>();
            for (int i = 0; i < contentArea.Children.Count; ++i)
            {
                if (contentArea.Children[i] is BaseItemView)
                {
                    result.Add((contentArea.Children[i] as BaseItemView).GetItemValue());
                }
            }
            return result;
        }

        private FrameworkElement getItemTitleView(IDataItem item)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = item.Name;
            return textBlock;
        }

        private BaseItemView getItemValueView(IDataItem item)
        {
            object defaultValue = null;
            if (item.GetValue() != null)
            {
                defaultValue = item.GetValue();
            }
            else if (item.GetDefaultValue() != null)
            {
                defaultValue = item.GetDefaultValue();
            }
            Type itemType = item.GetDataItemType();
            if (itemType == typeof(int))
            {
                return new NumericIntegerItemView(defaultValue);
            }
            else if (itemType == typeof(double))
            {
                return new NumericDoubleItemView(defaultValue);
            }
            else if (itemType == typeof(bool))
            {
                return new BooleanItemView(defaultValue);
            }
            else if (itemType == typeof(string))
            {
                return new StringItemView(defaultValue);
            }
            else
            {
                return new ErrorItemView();
            }
        }
    }
}