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
using ProblemSdk;
using System.Collections.ObjectModel;

namespace IntegralEquationsApp.Components.ProblemSelector
{
    /// <summary>
    /// Interaction logic for ProblemSelectorView.xaml
    /// </summary>
    public partial class ProblemSelectorView : UserControl, IProblemSelectorView
    {
        public ObservableCollection<IProblem> Problems
        {
            get { return (ObservableCollection<IProblem>)GetValue(ProblemsProperty); }
            set { SetValue(ProblemsProperty, value); }
        }

        public static readonly DependencyProperty ProblemsProperty =
            DependencyProperty.Register("Problems", typeof(ObservableCollection<IProblem>), 
                typeof(ProblemSelectorView), new PropertyMetadata(new ObservableCollection<IProblem>()));

        private ProblemSelectorPresenter presenter;

        public ProblemSelectorView()
        {
            InitializeComponent();
            presenter = new ProblemSelectorPresenter(this);
            presenter.LoadProblems();
        }

        public void SetProblemList(List<IProblem> problems)
        {
            Problems.Clear();
            foreach (IProblem problem in problems)
            {
                Problems.Add(problem);
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            presenter.ChangeCurrentProblem(comboBox.SelectedItem as IProblem);
        }
    }
}
