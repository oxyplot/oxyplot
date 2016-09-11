// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents the view-model for the main window.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ControllerDemo
{
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    /// <summary>
    /// Represents the view-model for the main window.
    /// </summary>
    public class MainViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel" /> class.
        /// </summary>
        public MainViewModel()
        {
            // Set the Model property, the INotifyPropertyChanged event will make the WPF Plot control update its content
            this.Model = CreatePlotModel("Custom PlotController", "Supports left/right keys only");
            this.Controller = new CustomPlotController();

            this.Model1 = CreatePlotModel("Default controller", null);
            this.Model2 = CreatePlotModel("Default controller", "UnbindAll()");
        }

        private static PlotModel CreatePlotModel(string title, string subtitle)
        {
            // Create the plot model
            var tmp = new PlotModel { Title = title, Subtitle = subtitle };

            tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Left });

            // Create two line series (markers are hidden by default)
            var series1 = new LineSeries { Title = "Series 1", MarkerType = MarkerType.Circle };
            series1.Points.Add(new DataPoint(0, 0));
            series1.Points.Add(new DataPoint(10, 18));
            series1.Points.Add(new DataPoint(20, 12));
            series1.Points.Add(new DataPoint(30, 8));
            series1.Points.Add(new DataPoint(40, 15));

            var series2 = new LineSeries { Title = "Series 2", MarkerType = MarkerType.Square };
            series2.Points.Add(new DataPoint(0, 4));
            series2.Points.Add(new DataPoint(10, 12));
            series2.Points.Add(new DataPoint(20, 16));
            series2.Points.Add(new DataPoint(30, 25));
            series2.Points.Add(new DataPoint(40, 5));

            // Add the series to the plot model
            tmp.Series.Add(series1);
            tmp.Series.Add(series2);

            // Axes are created automatically if they are not defined
            return tmp;
        }

        /// <summary>
        /// Gets the plot model.
        /// </summary>
        public PlotModel Model { get; private set; }
        public PlotModel Model1 { get; private set; }
        public PlotModel Model2 { get; private set; }

        /// <summary>
        /// Gets the plot controller.
        /// </summary>
        public IPlotController Controller { get; private set; }
    }
}