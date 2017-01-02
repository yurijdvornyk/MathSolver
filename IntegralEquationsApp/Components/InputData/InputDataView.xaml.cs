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
using ProblemSdk.Data;

namespace IntegralEquationsApp.Components.InputData
{
    /// <summary>
    /// Interaction logic for InputDataView.xaml
    /// </summary>
    public partial class InputDataView : UserControl, IInputDataView
    {
        public InputDataView()
        {
            InitializeComponent();
        }

        public void buildLayoutForProblem(ProblemData problemData)
        {
            if (problemData == null)
            {
                dockPanel.Children.Clear();
                return;
            }
            foreach (IDataItem item in problemData.DataItems)
            {
                dockPanel.Children.Add(getItemView(item));
            }
        }

        private UIElement getItemView(IDataItem item)
        {
            throw new NotImplementedException();
        }
    }
}
