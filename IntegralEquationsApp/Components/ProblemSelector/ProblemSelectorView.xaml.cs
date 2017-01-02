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

namespace IntegralEquationsApp.Components.ProblemSelector
{
    /// <summary>
    /// Interaction logic for ProblemSelectorView.xaml
    /// </summary>
    public partial class ProblemSelectorView : UserControl, IProblemSelectorView
    {
        private ProblemSelectorPresenter presenter;

        public ProblemSelectorView()
        {
            InitializeComponent();
            presenter = new ProblemSelectorPresenter(this);
            presenter.LoadProblems();
        }

        public void SetProblemList(List<IProblem> problems)
        {
            comboBox.Items.Clear();
            foreach (IProblem problem in problems)
            {
                comboBox.Items.Add(problem);
            }
        }
    }
}
