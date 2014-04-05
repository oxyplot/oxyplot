namespace SimpleDemo
{
	using System;
	using MonoTouch.UIKit;

	public partial class SimpleDemoViewController : UIViewController
	{
		/// <summary>
		/// The plot view.
		/// </summary>
		private OxyPlot.XamarinIOS.PlotView plotView;

		/// <summary>
		/// Initializes a new instance of the <see cref="SimpleDemo.SimpleDemoViewController"/> class.
		/// </summary>
		/// <param name="handle">Handle.</param>
		public SimpleDemoViewController (IntPtr handle) : base (handle)
		{
		}

		/// <summary>
		/// Called after the controller’s view is loaded into memory.
		/// </summary>
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.plotView = new OxyPlot.XamarinIOS.PlotView ();
			this.plotView.Model = this.CreatePlotModel ();
			this.EdgesForExtendedLayout = UIRectEdge.None;
			this.View = plotView;
		}

		/// <summary>
		/// Handles device orientation changes.
		/// </summary>
		/// <param name="fromInterfaceOrientation">The previous interface orientation.</param>
		public override void DidRotate (UIInterfaceOrientation fromInterfaceOrientation)
		{
			base.DidRotate (fromInterfaceOrientation);
			this.plotView.InvalidatePlot (false);
		}

		/// <summary>
		/// Creates an example plot model.
		/// </summary>
		/// <returns>The plot model.</returns>
		private OxyPlot.PlotModel CreatePlotModel() {
			var plotModel = new OxyPlot.PlotModel {
				Title = "Simple OxyPlot Demo",
				Background = OxyPlot.OxyColors.White
			};

			var xaxis = new OxyPlot.Axes.LinearAxis {
				Position = OxyPlot.Axes.AxisPosition.Bottom
			};

			var yaxis = new OxyPlot.Axes.LinearAxis {
				Position = OxyPlot.Axes.AxisPosition.Left,
				Maximum = 10,
				Minimum = 0
			};

			plotModel.Axes.Add (xaxis);
			plotModel.Axes.Add (yaxis);

			var series1 = new OxyPlot.Series.LineSeries {
				MarkerType = OxyPlot.MarkerType.Circle,
				MarkerSize = 4,
				MarkerStroke = OxyPlot.OxyColors.White

			};

			series1.Points.Add (new OxyPlot.DataPoint (0.0, 6.0));
			series1.Points.Add (new OxyPlot.DataPoint (1.4, 2.1));
			series1.Points.Add (new OxyPlot.DataPoint (2.0, 4.2));
			series1.Points.Add (new OxyPlot.DataPoint (3.3, 2.3));
			series1.Points.Add (new OxyPlot.DataPoint (4.7, 7.4));
			series1.Points.Add (new OxyPlot.DataPoint (6.0, 6.2));
			series1.Points.Add (new OxyPlot.DataPoint (8.9, 8.9));

			plotModel.Series.Add (series1);

			return plotModel;
		}
	}
}

