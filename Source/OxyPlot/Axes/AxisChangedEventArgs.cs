// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AxisChangedEventArgs.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// EventArgs for the Axis.AxisChanged event.
    /// </summary>
    public class AxisChangedEventArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AxisChangedEventArgs"/> class.
        /// </summary>
        /// <param name="changeType">
        /// Type of the change.
        /// </param>
        public AxisChangedEventArgs(AxisChangeTypes changeType)
        {
            this.ChangeType = changeType;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the type of the change.
        /// </summary>
        /// <value>The type of the change.</value>
        public AxisChangeTypes ChangeType { get; set; }

        #endregion
    }
}