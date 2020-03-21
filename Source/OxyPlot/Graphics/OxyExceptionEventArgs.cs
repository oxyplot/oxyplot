// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyExceptionEventArgs.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides data for an exception handling.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// Provides data for an exception handling.
    /// </summary>
    public class OxyExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the event was handled.
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// Gets or sets the exception to be handled.
        /// </summary>
        public Exception Exception { get; set; }
    }
}
