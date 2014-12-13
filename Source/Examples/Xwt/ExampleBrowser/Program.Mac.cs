// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Xwt;

namespace ExampleBrowser
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Initialize (ToolkitType.Gtk3);

			MainWindow w = new MainWindow ();
			w.Show ();

			Application.Run ();

			w.Dispose ();

			Application.Dispose ();
		}
	}
}
