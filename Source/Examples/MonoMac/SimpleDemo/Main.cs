using System;

using MonoMac.AppKit;

namespace SimpleDemo
{
    static class MainClass
    {
        static void Main (string[] args)
        {
            NSApplication.Init ();

            var application = NSApplication.SharedApplication;
            application.Delegate = new AppDelegate ();
            application.Run ();
        }
    }
}
