namespace OxyPlotSample
{
	using OxyPlot;
	using OxyPlot.Axes;
	using OxyPlot.Series;

	/// <summary>
	/// Provides a model that is shared between the Android and iOS apps.
	/// </summary>
	public class MyClass
	{
		/// <summary>
		/// Gets or sets the plot model that is shown in the demo apps.
		/// </summary>
		/// <value>My model.</value>
		public PlotModel MyModel { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="OxyPlotSample.MyClass"/> class.
		/// </summary>
		public MyClass ()
		{
			var plotModel = new PlotModel {
				Title = "OxyPlot on the Xamarin platform"
			};

			var xaxis = new LinearAxis {
				Position = AxisPosition.Bottom
			};

			var yaxis = new LinearAxis {
				Position = AxisPosition.Left,
				Minimum = 0,
				Maximum = 10
			};

			plotModel.Axes.Add (xaxis);
			plotModel.Axes.Add (yaxis);

			var series1 = new LineSeries {
				StrokeThickness = 3,
				MarkerType = MarkerType.Circle,
				MarkerSize = 4,
				MarkerStroke = OxyColors.White,
				MarkerStrokeThickness = 1
			};

			series1.Points.Add (new DataPoint (0.0, 6.0));
			series1.Points.Add (new DataPoint (1.4, 2.1));
			series1.Points.Add (new DataPoint (2.0, 4.2));
			series1.Points.Add (new DataPoint (3.3, 2.3));
			series1.Points.Add (new DataPoint (4.7, 7.4));
			series1.Points.Add (new DataPoint (6.0, 6.2));
			series1.Points.Add (new DataPoint (8.0, 8.9));

			plotModel.Series.Add (series1);

			this.MyModel = plotModel;
		}
	}
}
