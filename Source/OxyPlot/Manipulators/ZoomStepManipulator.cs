// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ZoomStepManipulator.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// The step manipulator.
    /// </summary>
    public class ZoomStepManipulator : ManipulatorBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoomStepManipulator"/> class.
        /// </summary>
        /// <param name="plotControl">
        /// The plot control.
        /// </param>
        /// <param name="step">
        /// The step.
        /// </param>
        /// <param name="fineControl">
        /// The fine Control.
        /// </param>
        public ZoomStepManipulator(IPlotControl plotControl, double step, bool fineControl)
            : base(plotControl)
        {
            this.Step = step;
            this.FineControl = fineControl;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets a value indicating whether FineControl.
        /// </summary>
        public bool FineControl { get; set; }

        /// <summary>
        ///   Gets or sets Step.
        /// </summary>
        public double Step { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The started.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        public override void Started(ManipulationEventArgs e)
        {
            base.Started(e);

            DataPoint current = this.InverseTransform(e.CurrentPosition.X, e.CurrentPosition.Y);

            double scale = this.Step;
            if (this.FineControl)
            {
                scale *= 3;
            }

            scale = 1 + scale;
            if (this.XAxis != null)
            {
                this.PlotControl.ZoomAt(this.XAxis, scale, current.X);
            }

            if (this.YAxis != null)
            {
                this.PlotControl.ZoomAt(this.YAxis, scale, current.Y);
            }

            this.PlotControl.InvalidatePlot();
        }

        #endregion
    }
}