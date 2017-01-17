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
    /// Interaction logic for NumericIntegerItemView.xaml
    /// </summary>
    public partial class NumericIntegerItemView
    {

        public NumericIntegerItemView(object defaultValue)
        {
            InitializeComponent();
            if (defaultValue != null)
            {
                SetItemValue(defaultValue);
            }
        }

        public override object GetItemValue()
        {
            int result = 0;
            if (int.TryParse(textBox.Text, out result))
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
            if (value is int)
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
