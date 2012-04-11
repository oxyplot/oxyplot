// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackerManipulator.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// The tracker manipulator.
    /// </summary>
    public class TrackerManipulator : ManipulatorBase
    {
        #region Constants and Fields

        /// <summary>
        ///   The current series.
        /// </summary>
        private ITrackableSeries currentSeries;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackerManipulator"/> class.
        /// </summary>
        /// <param name="plotControl">
        /// The plot control.
        /// </param>
        public TrackerManipulator(IPlotControl plotControl)
            : base(plotControl)
        {
            this.Snap = true;
            this.PointsOnly = false;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets a value indicating whether to show tracker on points only (not interpolating).
        /// </summary>
        public bool PointsOnly { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to snap to the nearest point.
        /// </summary>
        public bool Snap { get; set; }

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

            if (this.currentSeries == null)
            {
                return;
            }

            this.currentSeries = null;
            this.PlotControl.HideTracker();
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
            if (this.currentSeries == null)
            {
                return;
            }

            if (!this.PlotControl.ActualModel.PlotArea.Contains(e.CurrentPosition.X, e.CurrentPosition.Y))
            {
                return;
            }

            TrackerHitResult result = GetNearestHit(this.currentSeries, e.CurrentPosition, this.Snap, this.PointsOnly);
            if (result != null)
            {
                result.PlotModel = this.PlotControl.ActualModel;
                this.PlotControl.ShowTracker(result);
            }
        }

        /// <summary>
        /// Gets the cursor for the manipulation.
        /// </summary>
        /// <returns>
        /// The cursor.
        /// </returns>
        public override CursorType GetCursorType()
        {
            return CursorType.Default;
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
            this.currentSeries = this.PlotControl.GetSeriesFromPoint(e.CurrentPosition);
            this.Delta(e);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the nearest tracker hit.
        /// </summary>
        /// <param name="s">
        /// The series.
        /// </param>
        /// <param name="point">
        /// The point.
        /// </param>
        /// <param name="snap">
        /// Snap to points.
        /// </param>
        /// <param name="pointsOnly">
        /// Check points only (no interpolation).
        /// </param>
        /// <returns>
        /// A tracker hit result.
        /// </returns>
        private static TrackerHitResult GetNearestHit(ITrackableSeries s, ScreenPoint point, bool snap, bool pointsOnly)
        {
            if (s == null)
            {
                return null;
            }

            // Check data points only
            if (snap || pointsOnly)
            {
                TrackerHitResult result = s.GetNearestPoint(point, false);
                if (result != null)
                {
                    if (result.Position.DistanceTo(point) < 20)
                    {
                        return result;
                    }
                }
            }

            // Check between data points (if possible)
            if (!pointsOnly)
            {
                TrackerHitResult result = s.GetNearestPoint(point, true);
                return result;
            }

            return null;
        }

        #endregion
    }
}