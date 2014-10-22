using System;

namespace SimpleDemo
{
    using Foundation;
    using AppKit;

    public partial class AppDelegate : NSApplicationDelegate
    {
        MainWindowController mainWindowController;

        public AppDelegate ()
        {
        }

        public override void FinishedLaunching (NSObject notification)
        {
            var menu = new NSMenu ();

            var menuItem = new NSMenuItem ();
            menu.AddItem (menuItem);

            var appMenu = new NSMenu ();
            var quitItem = new NSMenuItem ("Quit", "q", (s, e) => NSApplication.SharedApplication.Terminate (menu));
            appMenu.AddItem (quitItem);

            menuItem.Submenu = appMenu;
            NSApplication.SharedApplication.MainMenu = menu;

            mainWindowController = new MainWindowController ();
            mainWindowController.Window.MakeKeyAndOrderFront (this);
        }
    }
}
