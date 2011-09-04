//-----------------------------------------------------------------------
// <copyright file="PanAction.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// The pan action.
    /// </summary>
    public class PanAction : OxyMouseAction
    {
        #region Constants and Fields

        /// <summary>
        /// The is panning.
        /// </summary>
        private bool isPanning;

        /// <summary>
        /// The ppt.
        /// </summary>
        private ScreenPoint ppt;

        /// <summary>
        /// The xaxis.
        /// </summary>
        private IAxis xaxis;

        /// <summary>
        /// The yaxis.
        /// </summary>
        private IAxis yaxis;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PanAction"/> class.
        /// </summary>
        /// <param name="pc">
        /// The pc.
        /// </param>
        public PanAction(IPlotControl pc)
            : base(pc)
        {
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
        public override void OnMouseDown(
            ScreenPoint pt, OxyMouseButton button, int clickCount, bool control, bool shift, bool alt)
        {
            this.pc.GetAxesFromPoint(pt, out this.xaxis, out this.yaxis);

            if (alt)
            {
                button = OxyMouseButton.Right;
            }

            if (button != OxyMouseButton.Right || control)
            {
                return;
            }

            // Right button double click resets the axis
            if (clickCount == 2)
            {
                if (this.xaxis != null)
                {
                    this.xaxis.Reset();
                }

                if (this.yaxis != null)
                {
                    this.yaxis.Reset();
                }

                this.pc.InvalidatePlot();
                return;
            }

            this.ppt = pt;

            // previousPoint = InverseTransform(pt.X, pt.Y, xaxis, yaxis);

            // pc.Cursor = Cursors.SizeAll;
            this.isPanning = true;
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
        public override void OnMouseMove(ScreenPoint pt, bool control, bool shift, bool alt)
        {
            if (!this.isPanning)
            {
                return;
            }

            // previousPoint = InverseTransform(ppt.X,ppt.Y, xaxis, yaxis);
            // var currentPoint = InverseTransform(pt.X, pt.Y, xaxis, yaxis);
            if (this.xaxis != null)
            {
                this.pc.Pan(this.xaxis, this.ppt, pt);
            }

            if (this.yaxis != null)
            {
                this.pc.Pan(this.yaxis, this.ppt, pt);
            }

            this.pc.RefreshPlot(false);
            this.ppt = pt;

            // previousPoint = currentPoint;
        }

        /// <summary>
        /// The on mouse up.
        /// </summary>
        public override void OnMouseUp()
        {
            if (!this.isPanning)
            {
                return;
            }

            // pc.Cursor = Cursors.Arrow;
            this.isPanning = false;
        }

        #endregion
    }
}
