
namespace OxyPlotSample.iOS
{
	using System;
	using System.Drawing;

	using MonoTouch.Foundation;
	using MonoTouch.UIKit;

	using OxyPlot.XamarinIOS;
	using OxyPlotSample;

	public partial class OxyPlotSample_iOSViewController : UIViewController
	{
		/// <summary>
		/// The plot view.
		/// </summary>
		private PlotView plotView;

		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public OxyPlotSample_iOSViewController (IntPtr handle) : base (handle)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Create the instance that contains the plot model
			var myClass = new MyClass ();

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

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}
	}
}

