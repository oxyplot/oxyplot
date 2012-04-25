// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ZoomRectangleManipulator.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// The zoom manipulator.
    /// </summary>
    public class ZoomRectangleManipulator : ManipulatorBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoomRectangleManipulator"/> class.
        /// </summary>
        /// <param name="plotControl">
        /// The plot control.
        /// </param>
        public ZoomRectangleManipulator(IPlotControl plotControl)
            : base(plotControl)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the zoom rectangle.
        /// </summary>
        private OxyRect ZoomRectangle { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Occurs when a manipulation is complete.
        /// </summary>
        /// <param name="e">
        /// The <see cref="OxyPlot.ManipulationEventArgs"/> instance containing the event data.
        /// </param>
        public override void Completed(ManipulationEventArgs e)
        {
            base.Completed(e);

            this.PlotControl.HideZoomRectangle();

            if (this.ZoomRectangle.Width > 10 && this.ZoomRectangle.Height > 10)
            {
                DataPoint p0 = this.InverseTransform(this.ZoomRectangle.Left, this.ZoomRectangle.Top);
                DataPoint p1 = this.InverseTransform(this.ZoomRectangle.Right, this.ZoomRectangle.Bottom);

                if (this.XAxis != null)
                {
                    this.PlotControl.Zoom(this.XAxis, p0.X, p1.X);
                }

                if (this.YAxis != null)
                {
                    this.PlotControl.Zoom(this.YAxis, p0.Y, p1.Y);
                }

                this.PlotControl.InvalidatePlot();
            }
        }

        /// <summary>
        /// Occurs when the input device changes position during a manipulation.
        /// </summary>
        /// <param name="e">
        /// The <see cref="OxyPlot.ManipulationEventArgs"/> instance containing the event data.
        /// </param>
        public override void Delta(ManipulationEventArgs e)
        {
            base.Delta(e);

            OxyRect plotArea = this.PlotControl.ActualModel.PlotArea;

            double x = Math.Min(this.StartPosition.X, e.CurrentPosition.X);
            double w = Math.Abs(this.StartPosition.X - e.CurrentPosition.X);
            double y = Math.Min(this.StartPosition.Y, e.CurrentPosition.Y);
            double h = Math.Abs(this.StartPosition.Y - e.CurrentPosition.Y);

            if (this.XAxis == null)
            {
                x = plotArea.Left;
                w = plotArea.Width;
            }

            if (this.YAxis == null)
            {
                y = plotArea.Top;
                h = plotArea.Height;
            }

            this.ZoomRectangle = new OxyRect(x, y, w, h);
            this.PlotControl.ShowZoomRectangle(this.ZoomRectangle);
        }

        /// <summary>
        /// Gets the cursor for the manipulation.
        /// </summary>
        /// <returns>
        /// The cursor.
        /// </returns>
        public override CursorType GetCursorType()
        {
            if (this.XAxis == null)
            {
                return CursorType.ZoomVertical;
            }

            if (this.YAxis == null)
            {
                return CursorType.ZoomHorizontal;
            }

            return CursorType.ZoomRectangle;
        }

        /// <summary>
        /// Occurs when an input device begins a manipulation on the plot.
        /// </summary>
        /// <param name="e">
        /// The <see cref="OxyPlot.ManipulationEventArgs"/> instance containing the event data.
        /// </param>
        public override void Started(ManipulationEventArgs e)
        {
            base.Started(e);
            this.ZoomRectangle = new OxyRect(this.StartPosition.X, this.StartPosition.Y, 0, 0);
            this.PlotControl.ShowZoomRectangle(this.ZoomRectangle);
        }

        #endregion
    }
}