using System;

using Foundation;
using AppKit;

namespace ExampleBrowser
{
    public partial class PlotWindow : NSWindow
    {
        public PlotWindow (IntPtr handle) : base (handle)
        {
        }

        [Export ("initWithCoder:")]
        public PlotWindow (NSCoder coder) : base (coder)
        {
        }

        public override void AwakeFromNib ()
        {
            base.AwakeFromNib ();
        }
    }
}
