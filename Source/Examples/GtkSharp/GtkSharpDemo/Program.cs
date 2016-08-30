// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   The demo program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GtkSharpDemo
{
    using System;

    using Gtk;

    using OxyPlot;
    using OxyPlot.Series;

    /// <summary>
    /// The demo program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        private static void Main()
        {
            Application.Init();

            using (var window = new Window("GtkSharp Demo"))
            {
                var plotModel = new PlotModel
                {
                    Title = "Trigonometric functions",
                    Subtitle = "Example using the FunctionSeries",
                    PlotType = PlotType.Cartesian,
                    Background = OxyColors.White
                };
                plotModel.Series.Add(new FunctionSeries(Math.Sin, -10, 10, 0.1, "sin(x)"));
                plotModel.Series.Add(new FunctionSeries(Math.Cos, -10, 10, 0.1, "cos(x)"));
                plotModel.Series.Add(new FunctionSeries(t => 5 * Math.Cos(t), t => 5 * Math.Sin(t), 0, 2 * Math.PI, 0.1,
                    "cos(t),sin(t)"));

                using (var plotView = new OxyPlot.GtkSharp.PlotView { Model = plotModel, Visible = true })
                {
                    window.SetSizeRequest(800, 600);
                    window.Add(plotView);
                    window.Focus = plotView;
                    window.Show();
                    window.DeleteEvent += (s, a) =>
                    {
                        Application.Quit();
                        a.RetVal = true;
                    };

                    Application.Run();
                }
            }
        }
    }
}