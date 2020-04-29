// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AxisChangedEventArgs.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides additional data for the <see cref="Axis.AxisChanged" /> event.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes
{
    using System;

    /// <summary>
    /// Provides additional data for the <see cref="Axis.AxisChanged" /> event.
    /// </summary>
    public class AxisChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AxisChangedEventArgs" /> class.
        /// </summary>
        /// <param name="changeType">Type of the change.</param>
        /// <param name="deltaMinimum">The delta minimum.</param>
        /// <param name="deltaMaximum">The delta maximum.</param>
        public AxisChangedEventArgs(AxisChangeTypes changeType, double deltaMinimum, double deltaMaximum)
        {
            this.ChangeType = changeType;
            this.DeltaMinimum = deltaMinimum;
            this.DeltaMaximum = deltaMaximum;
        }

        /// <summary>
        /// Gets the type of the change.
        /// </summary>
        /// <value>The type of the change.</value>
        public AxisChangeTypes ChangeType { get; private set; }

        /// <summary>
        /// Gets the delta for the minimum.
        /// </summary>
        /// <value>The delta.</value>
        public double DeltaMinimum { get; private set; }

        /// <summary>
        /// Gets the delta for the maximum.
        /// </summary>
        /// <value>The delta.</value>
        public double DeltaMaximum { get; private set; }
    }
}