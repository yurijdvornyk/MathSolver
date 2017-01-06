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

namespace IntegralEquationsApp.Components.InputData.ItemView
{
    /// <summary>
    /// Interaction logic for ErrorItemView.xaml
    /// </summary>
    public partial class ErrorItemView
    {
        private readonly string ERROR_CANNOT_HANDLE_INPUT_DATA_ITEM_TYPE = "This data type is not supported.\nData item can not be shown.";

        public ErrorItemView()
        {
            InitializeComponent();
            textBox.Text = ERROR_CANNOT_HANDLE_INPUT_DATA_ITEM_TYPE;
        }

        public override void SetValue(object value) { }

        public override object GetValue()
        {
            return null;
        }
    }
}
