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
    public partial class NumericDoubleItemView : UserControl, IBaseItemView<double>
    {
        public NumericDoubleItemView()
        {
            InitializeComponent();
        }

        public double getValue()
        {
            double result = 0;
            if (!double.TryParse(textBox.Text, out result))
            {
                throw new ItemValueParseException(GetType());
            }
            return result;
        }

        public void setValue(double value)
        {
            textBox.Text = value.ToString();
        }
    }
}
