using System.Windows;
using System.Windows.Controls;

namespace IntegralEquationsApp.Components.Solution
{
    /// <summary>
    /// Interaction logic for SolutionView.xaml
    /// </summary>
    public partial class SolutionView : UserControl, ISolutionView
    {
        private readonly string ERROR_TITLE = "Error";

        private SolutionPresenter presenter;

        public SolutionView()
        {
            InitializeComponent();
            presenter = new SolutionPresenter(this);
            btnSolve.Content = "\u25BA";
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

        public void ShowError(string message)
        {
            MessageBox.Show(message, ERROR_TITLE, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
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
