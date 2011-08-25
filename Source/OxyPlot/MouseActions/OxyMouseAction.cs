// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyMouseAction.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   The oxy mouse action.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// The oxy mouse action.
    /// </summary>
    public abstract class OxyMouseAction : IMouseAction
    {
        #region Constants and Fields

        /// <summary>
        /// The pc.
        /// </summary>
        protected IPlotControl pc;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyMouseAction"/> class.
        /// </summary>
        /// <param name="pc">
        /// The pc.
        /// </param>
        protected OxyMouseAction(IPlotControl pc)
        {
            this.pc = pc;
        }

        #endregion

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
        public virtual void OnMouseDown(
            ScreenPoint pt, OxyMouseButton button, int clickCount, bool control, bool shift, bool alt)
        {
        }

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
        public virtual void OnMouseMove(ScreenPoint pt, bool control, bool shift, bool alt)
        {
        }

        /// <summary>
        /// The on mouse up.
        /// </summary>
        public virtual void OnMouseUp()
        {
        }

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
        public virtual void OnMouseWheel(ScreenPoint pt, double delta, bool control, bool shift, bool alt)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// The inverse transform.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="y">
        /// The y.
        /// </param>
        /// <param name="xaxis">
        /// The xaxis.
        /// </param>
        /// <param name="yaxis">
        /// The yaxis.
        /// </param>
        /// <returns>
        /// </returns>
        protected DataPoint InverseTransform(double x, double y, IAxis xaxis, IAxis yaxis)
        {
            if (xaxis != null)
            {
                return xaxis.InverseTransform(x, y, yaxis);
            }

            if (yaxis != null)
            {
                return new DataPoint(0, yaxis.InverseTransform(y));
            }

            return new DataPoint();
        }

        #endregion
    }
}