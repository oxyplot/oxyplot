namespace SimpleDemo.iOS
{
    using MonoTouch.Foundation;
    using MonoTouch.UIKit;
    using Xamarin.Forms;

    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register ("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        UIWindow window;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();

            this.window = new UIWindow(UIScreen.MainScreen.Bounds);

            this.window.RootViewController =  App.GetMainPage().CreateViewController();

            this.window.MakeKeyAndVisible();

            return true;
        }
    }
}

