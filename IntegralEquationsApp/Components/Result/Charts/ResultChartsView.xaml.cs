using IntegralEquationsApp.Data;
using OxyPlot;
using OxyPlot.Wpf;
using ProblemSdk.Result;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System;
using IntegralEquationsApp.Utils;
using System.Windows.Input;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using System.Windows;

namespace IntegralEquationsApp.Components.Result.Charts
{
    /// <summary>
    /// Interaction logic for ResultChartsView.xaml
    /// </summary>
    public partial class ResultChartsView : UserControl, IResultChartsView
    {
        private ResultChartsPresenter presenter;

        public ResultChartsView()
        {
            InitializeComponent();
            presenter = new ResultChartsPresenter(this);
        }

        public void Set2dChart(ResultChart<Chart2dPoint> chartData, ChartPlotter plotter)
        {
            chartData.Items.ForEach(chart =>
            {
                List<double> x = chart.ChartPoints.Select(point => point.X).ToList();
                List<double> y = chart.ChartPoints.Select(point => point.Y).ToList();
                plotter.Children.Add(new LineGraph(x.AsXDataSource().Join(y.AsYDataSource())));
            });
        }

        public void Set3dChart(ResultChart<Chart3dPoint> resultChart, SurfaceWebView surfaceView)
        {
            if (resultChart.Items.Count > 0)
            {
                surfaceView.SetData(resultChart.Items[0].ChartPoints);
                //surfaceView.Update(SurfaceUtils.GetValueMatrixFromPoints(resultChart.Items[0].ChartPoints));
            }
        }

        public void SetCharts(List<IResultChart> charts)
        {
            tcResultsTabs.Items.Clear();
            charts.ForEach(chart =>
            {
                TabItem tabItem = new TabItem();
                tabItem.Style = new Style(typeof(TabItem));
                tabItem.Header = getTabHeader(chart.Title);
                if (chart.GetChartPointType() == typeof(Chart2dPoint))
                {
                    ChartPlotter plotter = new ChartPlotter();
                    Set2dChart(chart as ResultChart<Chart2dPoint>, plotter);
                    tabItem.Content = plotter;
                    tcResultsTabs.Items.Add(tabItem);
                } else if (chart.GetChartPointType() == typeof(Chart3dPoint))
                {
                    SurfaceWebView surfaceView = new SurfaceWebView();
                    Set3dChart(chart as ResultChart<Chart3dPoint>, surfaceView);
                    tabItem.Content = surfaceView;
                    tcResultsTabs.Items.Add(tabItem);
                }
            });
        }

        private string getTabHeader(string title)
        {

            return string.IsNullOrEmpty(title) ? (tcResultsTabs.Items.Count + 1).ToString() : title;
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            //if (surfaceView.Visibility == System.Windows.Visibility.Visible)
            //{
            //    surfaceView.OnKeyDown(e.Key);
            //}
        }
    }
}