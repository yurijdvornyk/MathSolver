using IntegralEquationsApp.Data;
using System.Collections.Generic;
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

        private void btnSolve_Click(object sender, RoutedEventArgs e)
        {
            SolutionManager.GetInstance().StartProblemSolving();
        }
    }
}
