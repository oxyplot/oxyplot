using System;

using Foundation;
using AppKit;

namespace ExampleBrowser
{
    using ExampleLibrary;
    using OxyPlot;
    using OxyPlot.Xamarin.Mac;

    public partial class PlotWindowController : NSWindowController
    {
        private PlotView plotView;

        public PlotWindowController (IntPtr handle) : base (handle)
        {
        }

        [Export ("initWithCoder:")]
        public PlotWindowController (NSCoder coder) : base (coder)
        {
        }

        public PlotWindowController () : base ("PlotWindow")
        {
        }

        public override void AwakeFromNib ()
        {
            base.AwakeFromNib ();
        }

        public new PlotWindow Window {
            get { return (PlotWindow)base.Window; }
        }

        public override void WindowDidLoad ()
        {
            base.WindowDidLoad ();
            plotView = new PlotView (this.Window.Frame);
            Window.ContentView = plotView;
        }

        public void SetExample(ExampleInfo example){
            this.Window.Title = example.Title;
            plotView.Model = example.PlotModel;
            plotView.Controller = example.PlotController;
            plotView.InvalidatePlot (true);
        }
           }
}
