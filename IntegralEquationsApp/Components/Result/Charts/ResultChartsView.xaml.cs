using IntegralEquationsApp.Data;
using OxyPlot;
using OxyPlot.Wpf;
using ProblemSdk.Result;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System;

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

        public void set2dChart(ResultChart<Chart2dPoint> chartData)
        {
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

        public void set3dChart(ResultChart<Chart3dPoint> resultChart)
        {
            // TODO: Add code
        }
    }
}
