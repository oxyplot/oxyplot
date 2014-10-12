using System;

using Foundation;
using AppKit;
using System.Collections.Generic;
using System.Linq;

namespace ExampleBrowser
{
    using ExampleLibrary;

    public partial class AppDelegate : NSApplicationDelegate
    {
        PlotWindowController plotWindowController;
        ExampleInfo currentExample;

        public AppDelegate ()
        {
        }

        List<ExampleInfo> exampleList;

        public override void FinishedLaunching (NSObject notification)
        {
            plotWindowController = new PlotWindowController ();

            var menu = new NSMenu ();

            var appMenu = new NSMenu ();
            appMenu.AddItem (new NSMenuItem ("Next example", "n", (s, e) => this.NextExample (1)));
            appMenu.AddItem (new NSMenuItem ("Previous example", "p", (s, e) => this.NextExample (-1)));
            appMenu.AddItem (NSMenuItem.SeparatorItem);
            appMenu.AddItem (new NSMenuItem ("Quit", "q", (s, e) => NSApplication.SharedApplication.Terminate (menu)));
            menu.AddItem (new NSMenuItem { Submenu = appMenu });

            var fileMenu = new NSMenu ("File");
            fileMenu.AddItem (new NSMenuItem ("Export", "e"));
            menu.AddItem (new NSMenuItem { Submenu = fileMenu });

            var editMenu = new NSMenu ("Edit");
            editMenu.AddItem (new NSMenuItem ("Copy", "c"));
            menu.AddItem (new NSMenuItem { Submenu = editMenu });

            var examplesMenu = new NSMenu ("Examples");
            exampleList = Examples.GetList ();

            var categories = exampleList.Select (e => e.Category).Distinct ().OrderBy (c => c).ToArray ();
            var categoryMenus = new Dictionary<string,NSMenu> ();
            foreach (var category in categories) {
                var categoryMenu = new NSMenu (category);
                examplesMenu.AddItem (new NSMenuItem (category) { Submenu = categoryMenu });
                categoryMenus.Add (category, categoryMenu);
            }

            foreach (var example in exampleList) {
                var item = new NSMenuItem (example.Title, (s, e) => this.SetExample (example));
                var categoryMenu = categoryMenus [example.Category];
                categoryMenu.AddItem (item);
            }
            menu.AddItem (new NSMenuItem { Submenu = examplesMenu });
            this.SetExample (exampleList.First ());

            plotWindowController.Window.MakeKeyAndOrderFront (this);
            NSApplication.SharedApplication.MainMenu = menu;
        }

        public void SetExample (ExampleInfo example)
        {
            this.plotWindowController.SetExample (example);
            this.currentExample = example;
        }

        public void NextExample (int delta)
        {
            var index = this.exampleList.IndexOf (this.currentExample);
            index += delta;
            if (index < 0)
                index = this.exampleList.Count - 1;
            if (index >= this.exampleList.Count)
                index = 0;
            this.SetExample (this.exampleList [index]);
        }

        public void Export ()
        {
        }
    }
}
