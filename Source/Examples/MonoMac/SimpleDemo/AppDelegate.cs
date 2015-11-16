using System;
using MonoMac;

namespace SimpleDemo
{
    using MonoMac.Foundation;
    using MonoMac.AppKit;

    public partial class AppDelegate : NSApplicationDelegate
    {
        MainWindowController mainWindowController;

        public AppDelegate ()
        {
        }

        public override void DidFinishLaunching (NSNotification notification)
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
