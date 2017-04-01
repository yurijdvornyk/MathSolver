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

        public void Set2dChart(ResultChart<Chart2dPoint> chartData)
        {
            oxyChart.Visibility = System.Windows.Visibility.Visible;
            surfaceView.Visibility = System.Windows.Visibility.Collapsed;

            oxyChart.Series.Clear();
            chartData.Items.ForEach(item =>
            {
                LineSeries lineSeries = new LineSeries();
                lineSeries.StrokeThickness = 2;
                lineSeries.Color = Colors.DarkGreen;
                List<DataPoint> linePoints = new List<DataPoint>();
                item.ChartPoints.ForEach(point =>
                {
                    linePoints.Add(new DataPoint(point.X, point.Y));
                });
                lineSeries.ItemsSource = linePoints;
                oxyChart.Series.Add(lineSeries);
            });
            oxyChart.IsLegendVisible = chartData.Items.Count > 1;
        }

        public void Set3dChart(ResultChart<Chart3dPoint> resultChart)
        {
            oxyChart.Visibility = System.Windows.Visibility.Collapsed;
            surfaceView.Visibility = System.Windows.Visibility.Visible;
            if (resultChart.Items.Count > 0)
            {
                surfaceView.Update(SurfaceUtils.GetValueMatrixFromPoints(resultChart.Items[0].ChartPoints));
            }
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if (surfaceView.Visibility == System.Windows.Visibility.Visible)
            {
                surfaceView.OnKeyDown(e.Key);
            }
        }
    }
}