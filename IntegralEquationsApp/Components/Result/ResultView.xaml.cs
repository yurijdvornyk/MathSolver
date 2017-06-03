using System.Windows;
using System.Windows.Controls;

namespace IntegralEquationsApp.Components.Result
{
    /// <summary>
    /// Interaction logic for ResultView.xaml
    /// </summary>
    public partial class ResultView : UserControl, IResultView
    {
        private readonly string DEFAULT_TEXT = "In this area the result of the problem solution will be displayed.\n\nSelect Data or Charts on the top of this window to switch between text and graphic representation of the problem solution.";

        private ResultPresenter presenter;

        public ResultView()
        {
            InitializeComponent();
            presenter = new ResultPresenter(this);
            tbDefaultText.Text = DEFAULT_TEXT;
        }

        public void ShowResultTabs()
        {
            tcResult.Visibility = Visibility.Visible;
        }
    }
}
