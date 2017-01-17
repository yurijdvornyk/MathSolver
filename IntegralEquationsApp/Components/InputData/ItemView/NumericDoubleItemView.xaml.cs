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
    /// Interaction logic for NumericDoubleItemView.xaml
    /// </summary>
    public partial class NumericDoubleItemView
    {

        public NumericDoubleItemView(object defaultValue)
        {
            InitializeComponent();
            if (defaultValue != null)
            {
                SetItemValue(defaultValue);
            }
        }

        public override object GetItemValue()
        {
            double result = 0;
            if (double.TryParse(textBox.Text, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public override void SetItemValue(object value)
        {
            if (value is double)
            {
                textBox.Text = value.ToString();
            }
            else
            {
                textBox.Text = "";
            }
        }
    }
}
