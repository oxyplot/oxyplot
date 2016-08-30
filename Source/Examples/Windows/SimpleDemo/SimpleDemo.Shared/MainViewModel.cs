namespace SimpleDemo
{
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    public class MainViewModel
    {
        public MainViewModel()
        {
            var model = new PlotModel { Title = "Hello Windows" };
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
            var lineSeries = new LineSeries { Title = "LineSeries", MarkerType = MarkerType.Circle };
            lineSeries.Points.Add(new DataPoint(0, 0));
            lineSeries.Points.Add(new DataPoint(10, 18));
            lineSeries.Points.Add(new DataPoint(20, 12));
            lineSeries.Points.Add(new DataPoint(30, 8));
            lineSeries.Points.Add(new DataPoint(40, 15));

            model.Series.Add(lineSeries);
            this.Model = model;
        }

        public PlotModel Model { get; private set; }
    }
}
