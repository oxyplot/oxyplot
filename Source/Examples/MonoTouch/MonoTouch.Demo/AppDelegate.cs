using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using ExampleLibrary;

namespace MonoTouch.Demo
{
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
			
			var root = new RootElement ("OxyPlot Demo") {
				new Section() {
					from e in exampleInfoList
					group e by e.Category into g
					orderby g.Key
					select (Element)new StyledStringElement (g.Key, delegate {
							DisplayCategory(g.Key);
						}) { Accessory = UITableViewCellAccessory.DisclosureIndicator }
				}
			};
				
			var dvc = new DialogViewController (root, true);
			
			navigation.PushViewController(dvc, true);
			
			window.RootViewController = navigation;
			
			// make the window visible
			window.MakeKeyAndVisible ();
			
			return true;
		}
		
		private void DisplayCategory(string category)
		{
			var root = new RootElement (category) {
				new Section() {
					from e in exampleInfoList
					where e.Category == category
					orderby e.Title
					select (Element)new StyledStringElement (e.Title, delegate { GraphView(e); }) { Accessory = UITableViewCellAccessory.DisclosureIndicator }
				}
			};
				
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