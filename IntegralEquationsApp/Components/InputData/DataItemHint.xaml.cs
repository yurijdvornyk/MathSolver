using System.Windows;
using System.Windows.Controls;

namespace IntegralEquationsApp.Components.InputData
{
    /// <summary>
    /// Interaction logic for DataItemHint.xaml
    /// </summary>
    public partial class DataItemHint : UserControl
    {
        private string hint;

        public DataItemHint(bool isRequired, string hint)
        {
            InitializeComponent();
            this.hint = hint;
            buttonHint.IsEnabled = !string.IsNullOrEmpty(hint);
            buttonHint.ToolTip = string.IsNullOrEmpty(hint) ? null : hint;
            buttonHint.Content = isRequired ? "*" : null;
        }

        private void buttonHint_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(hint, "Hint");
        }
    }
}