using IntegralEquationsApp.Components.Result.Charts.Surface;
using ProblemSdk.Result;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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
            string pageContent = SurfaceHelper.GetPageContent(data);
            browser.NavigateToString(pageContent);
        }
        
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                browser.InvokeScript("resize", e.NewSize.Height);
            }
            catch { }
        }
    }
}
