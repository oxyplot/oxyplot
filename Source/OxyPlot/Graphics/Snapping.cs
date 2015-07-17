// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Snapping.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Snapping data.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Snapping data.
    /// </summary>
    public class Snapping
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Snapping"/> class.
        /// </summary>
        public Snapping()
        {
            this.IsEnabled = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether snapping is enabled.
        /// </summary>
        /// <value><c>true</c> if snapping is enabled; otherwise, <c>false</c>.</value>
        public bool IsEnabled { get; set; }
    }
}