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
    /// Interaction logic for StringItemView.xaml
    /// </summary>
    public partial class StringItemView : UserControl, IBaseItemView<string>
    {
        public StringItemView()
        {
            InitializeComponent();
        }

        public string getValue()
        {
            return textBox.Text;
        }

        public void setValue(string value)
        {
            textBox.Text = value;
        }
    }
}
