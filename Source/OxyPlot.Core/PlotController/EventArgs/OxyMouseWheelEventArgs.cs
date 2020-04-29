// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyMouseWheelEventArgs.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides data for mouse wheel events.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Provides data for mouse wheel events.
    /// </summary>
    public class OxyMouseWheelEventArgs : OxyMouseEventArgs
    {
        /// <summary>
        /// Gets or sets the change.
        /// </summary>
        public int Delta { get; set; }
    }
}