// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UiPlotElement.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// Represents a plot element that handles mouse events.
    /// </summary>
    [Serializable]
    public abstract class UIPlotElement : SelectablePlotElement
    {
        #region Public Events

        /// <summary>
        ///   Occurs when a mouse button is pressed down on the model.
        /// </summary>
        public event EventHandler<OxyMouseEventArgs> MouseDown;

        /// <summary>
        ///   Occurs when the mouse is moved on the plot element (only occurs after MouseDown).
        /// </summary>
        public event EventHandler<OxyMouseEventArgs> MouseMove;

        /// <summary>
        ///   Occurs when the mouse button is released on the plot element.
        /// </summary>
        public event EventHandler<OxyMouseEventArgs> MouseUp;

        #endregion

        #region Public Methods

        /// <summary>
        /// Raises the <see cref="MouseDown"/> event.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The <see cref="OxyMouseEventArgs"/> instance containing the event data. 
        /// </param>
        protected internal virtual void OnMouseDown(object sender, OxyMouseEventArgs e)
        {
            if (this.MouseDown != null)
            {
                this.MouseDown(sender, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseMove"/> event.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The <see cref="OxyMouseEventArgs"/> instance containing the event data. 
        /// </param>
        protected internal virtual void OnMouseMove(object sender, OxyMouseEventArgs e)
        {
            if (this.MouseMove != null)
            {
                this.MouseMove(sender, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseUp"/> event.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The <see cref="OxyMouseEventArgs"/> instance containing the event data. 
        /// </param>
        protected internal virtual void OnMouseUp(object sender, OxyMouseEventArgs e)
        {
            if (this.MouseUp != null)
            {
                this.MouseUp(sender, e);
            }
        }

        /// <summary>
        /// Tests if the plot element is hit by the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <returns>
        /// A hit test result.
        /// </returns>
        protected internal abstract HitTestResult HitTest(ScreenPoint point, double tolerance);

        #endregion
    }
}