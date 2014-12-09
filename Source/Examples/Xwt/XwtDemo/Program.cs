// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   The demo program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using Xwt;
using OxyPlot;
using OxyPlot.Series;

namespace XwtDemo
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Initialize (ToolkitType.Gtk);

			var plotModel = new PlotModel
			{
				Title = "Trigonometric functions",
				Subtitle = "Example using the FunctionSeries",
				PlotType = PlotType.Cartesian,
				Background = OxyColors.White
			};
			plotModel.Series.Add(new FunctionSeries(Math.Sin, -10, 10, 0.1, "sin(x)") { Color = OxyColors.Black });
			plotModel.Series.Add(new FunctionSeries(Math.Cos, -10, 10, 0.1, "cos(x)") { Color = OxyColors.Green });
			plotModel.Series.Add(new FunctionSeries(t => 5 * Math.Cos(t), t => 5 * Math.Sin(t), 0, 2 * Math.PI, 0.1, "cos(t),sin(t)") { Color = OxyColors.Yellow });

			var plotView = new OxyPlot.Xwt.PlotView { Model = plotModel };
			plotView.Visible = true;
			plotView.MinWidth = 400;
			plotView.MinHeight = 400;

			Window w = new Window {
				Title = "OxyPlot Xwt Demo Application",
				Width = 600,
				Height = 600,
				Content = plotView
			};

			w.CloseRequested += (s, a) => Application.Exit ();

			w.Show ();
			Application.Run ();
			w.Dispose ();
			Application.Dispose ();
		}
	}
}
