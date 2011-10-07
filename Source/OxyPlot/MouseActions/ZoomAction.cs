//-----------------------------------------------------------------------
// <copyright file="ZoomAction.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// The zoom action.
    /// </summary>
    public class ZoomAction : OxyMouseAction
    {
        #region Constants and Fields

        /// <summary>
        ///   The down point.
        /// </summary>
        public ScreenPoint DownPoint;

        /// <summary>
        ///   The is zooming.
        /// </summary>
        private bool isZooming;

        /// <summary>
        ///   The xaxis.
        /// </summary>
        private IAxis xaxis;

        /// <summary>
        ///   The yaxis.
        /// </summary>
        private IAxis yaxis;

        /// <summary>
        ///   The zoom rectangle.
        /// </summary>
        private OxyRect zoomRectangle;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoomAction"/> class.
        /// </summary>
        /// <param name="pc">
        /// The pc.
        /// </param>
        public ZoomAction(IPlotControl pc)
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
            base.OnMouseDown(pt, button, clickCount, control, shift, alt);

            this.pc.GetAxesFromPoint(pt, out this.xaxis, out this.yaxis);

            if (button == OxyMouseButton.XButton1 || button == OxyMouseButton.XButton2)
            {
                DataPoint current = this.InverseTransform(pt.X, pt.Y, this.xaxis, this.yaxis);

                double scale = button == OxyMouseButton.XButton1 ? 0.05 : -0.05;
                if (control)
                {
                    scale *= 3;
                }

                scale = 1 + scale;
                if (this.xaxis != null)
                {
                    this.pc.ZoomAt(this.xaxis, scale, current.X);
                }

                if (this.yaxis != null)
                {
                    this.pc.ZoomAt(this.yaxis, scale, current.Y);
                }

                this.pc.InvalidatePlot();
                return;
            }

            if (alt)
            {
                button = OxyMouseButton.Right;
            }

            // RMB+Control is the same as MMB
            if (button == OxyMouseButton.Right && control)
            {
                button = OxyMouseButton.Middle;
            }

            if (button != OxyMouseButton.Middle)
            {
                return;
            }

            // Middle button double click resets the axis
            if (clickCount == 2)
            {
                if (this.xaxis != null)
                {
                    this.pc.Reset(this.xaxis);
                }

                if (this.yaxis != null)
                {
                    this.pc.Reset(this.yaxis);
                }

                this.pc.InvalidatePlot();
            }

            this.DownPoint = pt;
            this.zoomRectangle = new OxyRect(pt.X, pt.Y, 0, 0);
            this.pc.ShowZoomRectangle(this.zoomRectangle);

            // pc.Cursor = Cursors.SizeNWSE;
            this.isZooming = true;
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
            if (!this.isZooming)
            {
                return;
            }

            OxyRect plotArea = this.pc.ActualModel.PlotArea;

            if (this.yaxis == null)
            {
                this.DownPoint.Y = plotArea.Top;
                pt.Y = plotArea.Bottom;
            }

            if (this.xaxis == null)
            {
                this.DownPoint.X = plotArea.Left;
                pt.X = plotArea.Right;
            }

            this.zoomRectangle = CreateRect(this.DownPoint, pt);
            this.pc.ShowZoomRectangle(this.zoomRectangle);
        }

        /// <summary>
        /// The on mouse up.
        /// </summary>
        public override void OnMouseUp()
        {
            if (!this.isZooming)
            {
                return;
            }

            // pc.Cursor = Cursors.Arrow;
            this.pc.HideZoomRectangle();

            if (this.zoomRectangle.Width > 10 && this.zoomRectangle.Height > 10)
            {
                DataPoint p0 = this.InverseTransform(
                    this.zoomRectangle.Left, this.zoomRectangle.Top, this.xaxis, this.yaxis);
                DataPoint p1 = this.InverseTransform(
                    this.zoomRectangle.Right, this.zoomRectangle.Bottom, this.xaxis, this.yaxis);

                if (this.xaxis != null)
                {
                    this.pc.Zoom(this.xaxis, p0.X, p1.X);
                }

                if (this.yaxis != null)
                {
                    this.pc.Zoom(this.yaxis, p0.Y, p1.Y);
                }

                this.pc.InvalidatePlot();
            }

            this.isZooming = false;
            base.OnMouseUp();
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
        public override void OnMouseWheel(ScreenPoint pt, double delta, bool control, bool shift, bool alt)
        {
            IAxis xa, ya;
            this.pc.GetAxesFromPoint(pt, out xa, out ya);
            DataPoint current = this.InverseTransform(pt.X, pt.Y, xa, ya);

            double f = 0.001;
            if (control)
            {
                f *= 0.2;
            }

            double s = 1 + delta * f;
            if (xa != null)
            {
                this.pc.ZoomAt(xa, s, current.X);
            }

            if (ya != null)
            {
                this.pc.ZoomAt(ya, s, current.Y);
            }

            this.pc.RefreshPlot(false);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The create rect.
        /// </summary>
        /// <param name="p0">
        /// The p 0.
        /// </param>
        /// <param name="p1">
        /// The p 1.
        /// </param>
        /// <returns>
        /// </returns>
        private static OxyRect CreateRect(ScreenPoint p0, ScreenPoint p1)
        {
            double x = Math.Min(p0.X, p1.X);
            double w = Math.Abs(p0.X - p1.X);
            double y = Math.Min(p0.Y, p1.Y);
            double h = Math.Abs(p0.Y - p1.Y);
            return new OxyRect(x, y, w, h);
        }

        #endregion
    }
}
