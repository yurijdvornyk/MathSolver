using IntegralEquationsApp.Data;
using System;
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
            try
            {
                SolutionManager.GetInstance().StartProblemSolving();
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
