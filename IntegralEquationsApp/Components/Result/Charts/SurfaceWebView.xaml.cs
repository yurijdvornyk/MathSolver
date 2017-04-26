using IntegralEquationsApp.Components.Result.Charts.Surface;
using ProblemSdk.Result;
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

namespace IntegralEquationsApp.Components.Result.Charts
{
    /// <summary>
    /// Interaction logic for SurfaceWebView.xaml
    /// </summary>
    public partial class SurfaceWebView : UserControl
    {
        public List<Chart3dPoint> Data { get; private set; }

        public SurfaceWebView()
        {
            InitializeComponent();
        }

        public void SetData(List<Chart3dPoint> data)
        {
            Data = data;
            broswer.NavigateToString(SurfaceHelper.GetPageContent(data));
        }

        internal void Update(double[,] v)
        {

        }
    }
}
