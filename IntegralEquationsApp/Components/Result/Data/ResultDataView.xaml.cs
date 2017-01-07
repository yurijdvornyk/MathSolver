using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace IntegralEquationsApp.Components.Result.Data
{
    /// <summary>
    /// Interaction logic for ResultDataView.xaml
    /// </summary>
    public partial class ResultDataView : UserControl, IResultDataView
    {
        public ObservableCollection<TabItem> ResultTabs
        {
            get { return (ObservableCollection<TabItem>)GetValue(ResultTabsProperty); }
            set { SetValue(ResultTabsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ResultTabs.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResultTabsProperty =
            DependencyProperty.Register("ResultTabs", typeof(ObservableCollection<TabItem>),
                typeof(ResultDataView), new PropertyMetadata(new ObservableCollection<TabItem>()));

        private ResultDataPresenter presenter;

        public ResultDataView()
        {
            InitializeComponent();
            presenter = new ResultDataPresenter(this);
        }

        public void SetProblemResult(string title, List<ResultDataTable> dataTables)
        {
            ResultTabs.Clear();
            dataTables.ForEach(dataTable =>
            {
                DataGrid dataGrid = new DataGrid();
                dataGrid.IsReadOnly = true;
                dataGrid.CanUserSortColumns = false;
                dataGrid.CanUserAddRows = false;
                dataGrid.CanUserDeleteRows = false;
                dataGrid.CanUserResizeRows = false;
                dataGrid.CanUserResizeColumns = false;
                dataGrid.SelectionUnit = DataGridSelectionUnit.Cell;
                dataGrid.ItemsSource = dataTable.AsDataView();

                TabItem tabItem = new TabItem();
                tabItem.Header = getTabHeader(dataTable, tabItem);
                tabItem.Content = dataGrid;
                ResultTabs.Add(tabItem);
            });
        }

        private string getTabHeader(ResultDataTable dataTable, TabItem tabItem)
        {
            return string.IsNullOrEmpty(dataTable.TableName) ? 
                (ResultTabs.Count + 1).ToString() : dataTable.TableName;
        }
    }
}
