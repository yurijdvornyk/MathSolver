using IntegralEquationsApp.Components.Result.Charts.Surface;
using Microsoft.Win32;
using ProblemSdk.Result;
using System;
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
        private string pageContent;

        public SurfaceWebView()
        {
            InitializeComponent();
        }

        public void SetData(List<Chart3dPoint> data)
        {
            Data = data;
            pageContent = SurfaceHelper.GetPageContent(data);
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

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "HTML file (*.html)|*.html";
            saveFileDialog.Title = "Export chart as HTML page";
            bool? result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                SurfaceHelper.SavePageToFile(saveFileDialog.FileName, pageContent);
            }
        }
    }
}
