// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotCommands.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a standard set of commands for the <see cref="PlotView" /> control.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows.Input;

    /// <summary>
    /// Provides a standard set of commands for the <see cref="PlotBase" /> control.
    /// </summary>
    public static class PlotCommands
    {
        /// <summary>
        /// Gets the value that represents the "Reset all axes" command.
        /// </summary>
        public static readonly ICommand ResetAxes = new RoutedUICommand("Reset all axes", "ResetAxes", typeof(PlotViewBase));
    }
}
