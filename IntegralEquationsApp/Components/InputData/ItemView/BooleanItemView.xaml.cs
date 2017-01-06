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
    /// Interaction logic for BooleanItemView.xaml
    /// </summary>
    public partial class BooleanItemView
    {
        public BooleanItemView()
        {
            InitializeComponent();
        }

        public override object GetValue()
        {
            return checkBox.IsChecked.Value;
        }

        public override void SetValue(object value)
        {
            checkBox.IsChecked = (bool)value;
        }
    }
}
