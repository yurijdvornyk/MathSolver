using System.Windows;

namespace IntegralEquationsApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string res = "";
            foreach (var item in inputDataView.GetItemValues())
            {
                res += item.ToString() + "\n";
            }
            MessageBox.Show(res);
        }
    }
}
