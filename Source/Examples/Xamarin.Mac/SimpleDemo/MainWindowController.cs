
namespace SimpleDemo
{
    using System;

    using AppKit;
    using CoreGraphics;
    using Foundation;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    using OxyPlot.Xamarin.Mac;

    public partial class MainWindowController : NSWindowController
    {
        // Called when created from unmanaged code
        public MainWindowController (IntPtr handle) : base (handle)
        {
            Initialize ();
        }

        // Called when created directly from a XIB file
        [Export ("initWithCoder:")]
        public MainWindowController (NSCoder coder) : base (coder)
        {
            Initialize ();
        }

        // Call to load from the XIB/NIB file
        public MainWindowController () : base ("MainWindow")
        {
            Initialize ();
        }

        void Initialize ()
        {
        }

        public override void WindowDidLoad ()
        {
            base.WindowDidLoad ();
            var frame = new CGRect (10, 10, 300, 300);
            var plotView = new PlotView (frame);
            var model = new PlotModel{ Title = "Hello Mac!" };
            model.Axes.Add (new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add (new LinearAxis { Position = AxisPosition.Left });
            model.Series.Add (new FunctionSeries (Math.Sin, 0, 10, 200));
            plotView.Model = model;
            Window.ContentView = plotView;
            Window.Title = "OxyPlot on Xamarin.Mac (Unified API)";
        }

        public new MainWindow Window {
            get {
                return (MainWindow)base.Window;
            }
        }
    }
}

