using IntegralEquationsApp.Components.Result.Charts.Surface;
using ProblemSdk.Result;
using System.Collections.Generic;
using System.IO;
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
            StreamWriter file = new StreamWriter("result.html");
            string pageContent = SurfaceHelper.GetPageContent(data);
            file.WriteLine(pageContent);
            file.Close();

            //HtmlBridge b = new HtmlBridge();
            //browser.ObjectForScripting = b;
            browser.NavigateToString(pageContent);
        }

        internal void Update(double[,] v)
        {

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
