// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   The main entry point for the application.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleBrowser
{
    using System;

    using Gtk;

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Init();
            var window = new MainWindow();
            window.SetDefaultSize (800, 600);
            window.Visible = true;
            window.ShowAll();
            Application.Run();
        }
    }
}