namespace OxyPlotSample.iOS
{
	using System;
	using System.Drawing;

	using MonoTouch.UIKit;

	using OxyPlot.XamarinIOS;
	using OxyPlotSample;

	/// <summary>
	/// Represents the main view controller.
	/// </summary>
	public partial class OxyPlotSample_iOSViewController : UIViewController
	{
		/// <summary>
		/// The plot view.
		/// </summary>
		private PlotView plotView;

		/// <summary>
		/// The object that contains the plot model.
		/// </summary>
		private readonly MyClass myClass = new MyClass();

		/// <summary>
		/// Initializes a new instance of the <see cref="OxyPlotSample.iOS.OxyPlotSample_iOSViewController"/> class.
		/// </summary>
		/// <param name="handle">Handle.</param>
		public OxyPlotSample_iOSViewController (IntPtr handle) : base (handle)
		{
		}

		/// <summary>
		/// Called after the controller’s view is loaded into memory.
		/// </summary>
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Create the plot view and set the model
			this.plotView = new PlotView ();
			this.plotView.Model = myClass.MyModel;

			// Set the frame and add the plot view to the view
			this.plotView.Frame = new RectangleF (0, 20, this.View.Frame.Width, this.View.Frame.Height - 20);
			this.plotView.AutoresizingMask = UIViewAutoresizing.All;
			this.View.AddSubview (this.plotView);
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
	}
}

