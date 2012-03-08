// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManipulatorBase.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// The manipulator base.
    /// </summary>
    public class ManipulatorBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ManipulatorBase"/> class.
        /// </summary>
        /// <param name="plotControl">
        /// The plot control.
        /// </param>
        protected ManipulatorBase(IPlotControl plotControl)
        {
            this.PlotControl = plotControl;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the first position of the manipulation.
        /// </summary>
        public ScreenPoint StartPosition { get; private set; }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the plot control.
        /// </summary>
        protected IPlotControl PlotControl { get; private set; }

        /// <summary>
        ///   Gets or sets the X axis.
        /// </summary>
        /// <value>The X axis.</value>
        protected Axis XAxis { get; set; }

        /// <summary>
        ///   Gets or sets the Y axis.
        /// </summary>
        /// <value>The Y axis.</value>
        protected Axis YAxis { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Occurs when a manipulation is complete.
        /// </summary>
        /// <param name="e">
        /// The <see cref="OxyPlot.ManipulationEventArgs"/> instance containing the event data.
        /// </param>
        public virtual void Completed(ManipulationEventArgs e)
        {
            this.PlotControl.SetCursorType(CursorType.Default);
        }

        /// <summary>
        /// Occurs when the input device changes position during a manipulation.
        /// </summary>
        /// <param name="e">
        /// The <see cref="OxyPlot.ManipulationEventArgs"/> instance containing the event data.
        /// </param>
        public virtual void Delta(ManipulationEventArgs e)
        {
        }

        /// <summary>
        /// Gets the cursor for the manipulation.
        /// </summary>
        /// <returns>
        /// The cursor.
        /// </returns>
        public virtual CursorType GetCursorType()
        {
            return CursorType.Default;
        }

        /// <summary>
        /// Occurs when an input device begins a manipulation on the plot.
        /// </summary>
        /// <param name="e">
        /// The <see cref="OxyPlot.ManipulationEventArgs"/> instance containing the event data.
        /// </param>
        public virtual void Started(ManipulationEventArgs e)
        {
            Axis xaxis;
            Axis yaxis;
            this.PlotControl.GetAxesFromPoint(e.CurrentPosition, out xaxis, out yaxis);
            this.StartPosition = e.CurrentPosition;

            this.XAxis = xaxis;
            this.YAxis = yaxis;

            this.PlotControl.SetCursorType(this.GetCursorType());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Transforms a point from screen coordinates to data coordinates.
        /// </summary>
        /// <param name="x">
        /// The x coordinate.
        /// </param>
        /// <param name="y">
        /// The y coordinate.
        /// </param>
        /// <returns>
        /// A data point.
        /// </returns>
        protected DataPoint InverseTransform(double x, double y)
        {
            if (this.XAxis != null)
            {
                return this.XAxis.InverseTransform(x, y, this.YAxis);
            }

            if (this.YAxis != null)
            {
                return new DataPoint(0, this.YAxis.InverseTransform(y));
            }

            return new DataPoint();
        }

        #endregion
    }
}