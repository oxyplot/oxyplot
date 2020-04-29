// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyMouseEventArgs.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides data for the mouse events.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Provides data for the mouse events.
    /// </summary>
    public class OxyMouseEventArgs : OxyInputEventArgs
    {
        /// <summary>
        /// Gets or sets the position of the mouse cursor.
        /// </summary>
        public ScreenPoint Position { get; set; }
    }
}