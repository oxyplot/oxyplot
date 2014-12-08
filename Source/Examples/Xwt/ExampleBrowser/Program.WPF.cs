// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Xwt;
using System;

namespace ExampleBrowser
{
	class MainClass
	{
		[STAThread]
		public static void Main (string[] args)
		{
			Application.Initialize (ToolkitType.Wpf);

			MainWindow w = new MainWindow ();
			w.Show ();

			Application.Run ();

			w.Dispose ();

			Application.Dispose ();
		}
	}
}
