// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PanManipulator.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// The pan manipulator.
    /// </summary>
    public class PanManipulator : ManipulatorBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PanManipulator"/> class.
        /// </summary>
        /// <param name="plotControl">
        /// The plot control.
        /// </param>
        public PanManipulator(IPlotControl plotControl)
            : base(plotControl)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the previous position.
        /// </summary>
        private ScreenPoint PreviousPosition { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Occurs when the input device changes position during a manipulation.
        /// </summary>
        /// <param name="e">
        /// The <see cref="OxyPlot.ManipulationEventArgs"/> instance containing the event data.
        /// </param>
        public override void Delta(ManipulationEventArgs e)
        {
            base.Delta(e);
            if (this.XAxis != null)
            {
                this.PlotControl.Pan(this.XAxis, this.PreviousPosition, e.CurrentPosition);
            }

            if (this.YAxis != null)
            {
                this.PlotControl.Pan(this.YAxis, this.PreviousPosition, e.CurrentPosition);
            }

            this.PlotControl.RefreshPlot(false);
            this.PreviousPosition = e.CurrentPosition;
        }

        /// <summary>
        /// Gets the cursor for the manipulation.
        /// </summary>
        /// <returns>
        /// The cursor.
        /// </returns>
        public override CursorType GetCursorType()
        {
            return CursorType.Pan;
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
            this.PreviousPosition = e.CurrentPosition;
        }

        #endregion
    }
}