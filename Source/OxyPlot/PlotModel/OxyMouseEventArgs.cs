// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyMouseEventArgs.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// Represents event arguments for 3D mouse events events.
    /// </summary>
    public class OxyMouseEventArgs : EventArgs
    {
        #region Public Properties

        /// <summary>
        ///   Gets or sets the mouse button that has changed.
        /// </summary>
        public OxyMouseButton ChangedButton { get; set; }

        /// <summary>
        ///   Gets or sets the click count.
        /// </summary>
        /// <value> The click count. </value>
        public int ClickCount { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether Handled.
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether the alt key was pressed when the event was raised.
        /// </summary>
        public bool IsAltDown { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether the control key was pressed when the event was raised.
        /// </summary>
        public bool IsControlDown { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether the shift key was pressed when the event was raised.
        /// </summary>
        public bool IsShiftDown { get; set; }

        /// <summary>
        ///   Gets or sets the hit test result.
        /// </summary>
        public HitTestResult HitTestResult { get; set; }

        /// <summary>
        ///   Gets or sets the plot control.
        /// </summary>
        /// <value> The plot control. </value>
        public IPlotControl PlotControl { get; set; }

        /// <summary>
        ///   Gets or sets the position.
        /// </summary>
        public ScreenPoint Position { get; set; }

        #endregion
    }
}