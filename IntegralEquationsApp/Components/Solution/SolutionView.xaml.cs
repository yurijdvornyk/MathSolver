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

namespace IntegralEquationsApp.Components.Solution
{
    /// <summary>
    /// Interaction logic for SolutionView.xaml
    /// </summary>
    public partial class SolutionView : UserControl, ISolutionView
    {
        private SolutionPresenter presenter;

        public SolutionView()
        {
            InitializeComponent();
            presenter = new SolutionPresenter(this);
        }

        public void FinishProgress()
        {
            progressBar.IsIndeterminate = false;
            progressBar.Value = 0;
        }

        public void SetProgress(double progress)
        {
            progressBar.IsIndeterminate = false;
            progressBar.Value = progress;
        }

        public void SetSolveButtonEnabled(bool enabled)
        {
            btnSolve.IsEnabled = enabled;
        }

        public void StartProgress()
        {
            progressBar.IsIndeterminate = true;
        }

        private void btnSolve_Click(object sender, RoutedEventArgs e)
        {
            presenter.StartSolving();
        }
    }
}
