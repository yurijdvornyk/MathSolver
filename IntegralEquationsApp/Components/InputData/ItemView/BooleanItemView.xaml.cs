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
    public partial class BooleanItemView : UserControl, IBaseItemView<bool>
    {
        public BooleanItemView()
        {
            InitializeComponent();
        }

        public bool getValue()
        {
            return checkBox.IsChecked.Value;
        }

        public void setValue(bool value)
        {
            checkBox.IsChecked = value;
        }
    }
}
