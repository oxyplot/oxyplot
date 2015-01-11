namespace SimpleDemo.iOS
{
    using MonoTouch.Foundation;
    using MonoTouch.UIKit;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.iOS;

    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            OxyPlot.XamarinFormsIOS.Forms.Init();
            Forms.Init();

            this.LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}

