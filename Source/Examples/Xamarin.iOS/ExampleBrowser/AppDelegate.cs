// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppDelegate.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleBrowser
{
  using System.Collections.Generic;
  using System.Linq;

  using Foundation;
  using UIKit;

  using MonoTouch.Dialog;

  using ExampleLibrary;

    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register ("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations
        UIWindow window;

        UINavigationController navigation;

        List<ExampleInfo> exampleInfoList;

        //
        // This method is invoked when the application has loaded and is ready to run. In this
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching (UIApplication app, NSDictionary options)
        {
            exampleInfoList = ExampleLibrary.Examples.GetList();

            // create a new window instance based on the screen size
            window = new UIWindow (UIScreen.MainScreen.Bounds);

            navigation = new UINavigationController();

            var root = new RootElement ("OxyPlot Example Browser");
            var section = new Section ();
            section.AddAll (exampleInfoList
                .GroupBy (e => e.Category)
                .OrderBy (g => g.Key)
                .Select (g =>
                    (Element)new StyledStringElement (g.Key, delegate {
                        DisplayCategory (g.Key);
                    }) { Accessory = UITableViewCellAccessory.DisclosureIndicator }));
            root.Add (section);

            var dvc = new DialogViewController (root, true);

            navigation.PushViewController(dvc, true);

            window.RootViewController = navigation;

            // make the window visible
            window.MakeKeyAndVisible ();

            return true;
        }

        private void DisplayCategory(string category)
        {
            var root = new RootElement (category);
            var section = new Section ();
            section.AddAll (exampleInfoList
                .Where (e => e.Category == category)
                .OrderBy (e => e.Title)
                .Select (e => (Element)new StyledStringElement (e.Title, delegate {
                GraphView (e);
            }) { Accessory = UITableViewCellAccessory.DisclosureIndicator }
            ));
            root.Add (section);

            var dvc = new DialogViewController (root, true);

            navigation.PushViewController (dvc, true);
        }

        private void GraphView(ExampleInfo exampleInfo)
        {
            var dvc = new GraphViewController (exampleInfo);
            navigation.PushViewController (dvc, true);
        }
    }
}