// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMouseAction.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   The i mouse action.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// The i mouse action.
    /// </summary>
    public interface IMouseAction
    {
        #region Public Methods

        /// <summary>
        /// The on mouse down.
        /// </summary>
        /// <param name="pt">
        /// The pt.
        /// </param>
        /// <param name="button">
        /// The button.
        /// </param>
        /// <param name="clickCount">
        /// The click count.
        /// </param>
        /// <param name="control">
        /// The control.
        /// </param>
        /// <param name="shift">
        /// The shift.
        /// </param>
        /// <param name="alt">
        /// The alt.
        /// </param>
        void OnMouseDown(ScreenPoint pt, OxyMouseButton button, int clickCount, bool control, bool shift, bool alt);

        /// <summary>
        /// The on mouse move.
        /// </summary>
        /// <param name="pt">
        /// The pt.
        /// </param>
        /// <param name="control">
        /// The control.
        /// </param>
        /// <param name="shift">
        /// The shift.
        /// </param>
        /// <param name="alt">
        /// The alt.
        /// </param>
        void OnMouseMove(ScreenPoint pt, bool control, bool shift, bool alt);

        /// <summary>
        /// The on mouse up.
        /// </summary>
        void OnMouseUp();

        /// <summary>
        /// The on mouse wheel.
        /// </summary>
        /// <param name="pt">
        /// The pt.
        /// </param>
        /// <param name="delta">
        /// The delta.
        /// </param>
        /// <param name="control">
        /// The control.
        /// </param>
        /// <param name="shift">
        /// The shift.
        /// </param>
        /// <param name="alt">
        /// The alt.
        /// </param>
        void OnMouseWheel(ScreenPoint pt, double delta, bool control, bool shift, bool alt);

        #endregion
    }
}