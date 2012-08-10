// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResetManipulator.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// The reset manipulator.
    /// </summary>
    public class ResetManipulator : ManipulatorBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ResetManipulator"/> class.
        /// </summary>
        /// <param name="plotControl">
        /// The plot control.
        /// </param>
        public ResetManipulator(IPlotControl plotControl)
            : base(plotControl)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Occurs when an input device begins a manipulation on the plot.
        /// </summary>
        /// <param name="e">
        /// The <see cref="OxyPlot.ManipulationEventArgs"/> instance containing the event data.
        /// </param>
        public override void Started(ManipulationEventArgs e)
        {
            base.Started(e);
            if (this.XAxis != null)
            {
                this.PlotControl.Reset(this.XAxis);
            }

            if (this.YAxis != null)
            {
                this.PlotControl.Reset(this.YAxis);
            }

            this.PlotControl.InvalidatePlot();
        }

        #endregion
    }
}