// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ZoomManipulator.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// The zoom manipulator.
    /// </summary>
    public class ZoomManipulator : ManipulatorBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoomManipulator"/> class.
        /// </summary>
        /// <param name="plotControl">
        /// The plot control.
        /// </param>
        public ZoomManipulator(IPlotControl plotControl)
            : base(plotControl)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Occurs when the input device changes position during a manipulation.
        /// </summary>
        /// <param name="e">The <see cref="OxyPlot.ManipulationEventArgs"/> instance containing the event data.</param>
        public override void Delta(ManipulationEventArgs e)
        {
            base.Delta(e);

            DataPoint current = this.InverseTransform(e.CurrentPosition.X, e.CurrentPosition.Y);

            if (this.XAxis != null)
            {
                this.PlotControl.ZoomAt(this.XAxis, e.ScaleX, current.X);
            }

            if (this.YAxis != null)
            {
                this.PlotControl.ZoomAt(this.YAxis, e.ScaleY, current.Y);
            }

            this.PlotControl.InvalidatePlot();
        }

        #endregion
    }
}