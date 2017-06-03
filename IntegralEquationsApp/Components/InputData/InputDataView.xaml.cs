using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ProblemSdk.Data;
using IntegralEquationsApp.Components.InputData.ItemView;
using ProblemSdk.Classes.Choice;
using ProblemSdk;
using System.Windows.Media.Imaging;

namespace IntegralEquationsApp.Components.InputData
{
    /// <summary>
    /// Interaction logic for InputDataView.xaml
    /// </summary>
    public partial class InputDataView : UserControl, IInputDataView
    {
        private readonly int INPUT_DATA_ITEM_MARGIN = 5;
        private readonly int TITLE_COLUMN_INDEX = 0;
        private readonly int VALUE_COLUMN_INDEX = 1;
        private readonly int HINT_COLUMN_INDEX = 2;

        private InputDataPresenter presenter;

        public InputDataView()
        {
            InitializeComponent();
            presenter = new InputDataPresenter(this);
            tbDefaultText.Text = "Select the problem from the drop down list to see the list of input parameters.";
        }

        public void BuildLayoutForProblem(IProblem problem)
        {
            clearContentArea();
            if (problem == null || problem.InputData == null)
            {
                return;
            }
            setUpColumns();
            if (problem.Equation != null)
            {
                BitmapImage bitmap = new BitmapImage(new Uri(problem.Equation));
                imageEquation.Source = bitmap;
                Image imageTooltip = new Image();
                imageTooltip.Source = bitmap;
                imageEquation.ToolTip = imageTooltip;
                imageEquation.Visibility = Visibility.Visible;
            }
            else
            {
                imageEquation.Visibility = Visibility.Collapsed;
            }
            problem.InputData.DataItems.ForEach(item =>
            {
                createNewRow();
                addItemTitle(item);
                addItemValue(item);
                addItemHint(item);
            });
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
            if (item.GetDefaultValue() != null)
            {
                defaultValue = item.GetDefaultValue();
            }
            else if (item.GetValue() != null)
            {
                defaultValue = item.GetValue();
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
            } else if (itemType == typeof(ISingleChoice))
            {
                return new SingleChoiceItemView(defaultValue as ISingleChoice);
            }
            else
            {
                return new ErrorItemView();
            }
        }

        private void addItemHint(IDataItem item)
        {
            DataItemHint hint = new DataItemHint(item.IsRequired, item.Hint);
            hint.HorizontalAlignment = HorizontalAlignment.Center;
            hint.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetColumn(hint, HINT_COLUMN_INDEX);
            Grid.SetRow(hint, getLastRowIndex());
            contentArea.Children.Add(hint);
        }

        private void addItemValue(IDataItem item)
        {
            FrameworkElement itemValue = getItemValueView(item);
            itemValue.Margin = new Thickness(INPUT_DATA_ITEM_MARGIN);
            Grid.SetColumn(itemValue, VALUE_COLUMN_INDEX);
            Grid.SetRow(itemValue, getLastRowIndex());
            contentArea.Children.Add(itemValue);
        }

        private void addItemTitle(IDataItem item)
        {
            FrameworkElement itemTitle = getItemTitleView(item);
            itemTitle.Margin = new Thickness(INPUT_DATA_ITEM_MARGIN);
            itemTitle.HorizontalAlignment = HorizontalAlignment.Left;
            itemTitle.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetColumn(itemTitle, TITLE_COLUMN_INDEX);
            Grid.SetRow(itemTitle, getLastRowIndex());
            contentArea.Children.Add(itemTitle);
        }

        private void createNewRow()
        {
            RowDefinition row = new RowDefinition();
            row.Height = GridLength.Auto;
            contentArea.RowDefinitions.Add(row);
        }

        private int getLastRowIndex()
        {
            return contentArea.RowDefinitions.Count - 1;
        }

        private void setUpColumns()
        {
            // Title column
            ColumnDefinition titleColumn = new ColumnDefinition();
            titleColumn.Width = GridLength.Auto;
            contentArea.ColumnDefinitions.Add(titleColumn);
            // Value column
            contentArea.ColumnDefinitions.Add(new ColumnDefinition());
            // Hint column
            ColumnDefinition hintColumn = new ColumnDefinition();
            hintColumn.Width = GridLength.Auto;
            contentArea.ColumnDefinitions.Add(hintColumn);
        }

        private void clearContentArea()
        {
            tbDefaultText.Visibility = Visibility.Collapsed;
            dpInputData.Visibility = Visibility.Visible;
            contentArea.Children.Clear();
            contentArea.ColumnDefinitions.Clear();
            contentArea.RowDefinitions.Clear();
        }
    }
}