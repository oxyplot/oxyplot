// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyKeyEventArgs.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides data for key events.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Provides data for key events.
    /// </summary>
    public class OxyKeyEventArgs : OxyInputEventArgs
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public OxyKey Key { get; set; }
    }
}