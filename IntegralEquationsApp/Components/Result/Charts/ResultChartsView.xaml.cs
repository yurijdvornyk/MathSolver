using IntegralEquationsApp.Data;
using OxyPlot;
using OxyPlot.Wpf;
using ProblemSdk.Result;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

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

        public void setChartData(ResultChart chartData)
        {
            oxyChart.Series.Clear();
            chartData.Items.ForEach(item => addChart(item));
            foreach (var value in chartData.Items)
            {
                addChart(value);
            }
            oxyChart.IsLegendVisible = chartData.Items.Count > 1;
        }

        private void addChart(ResultChartItem item)
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
        }
    }
}
