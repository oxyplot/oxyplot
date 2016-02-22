// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TouchTrackerManipulator.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a plot manipulator for tracker functionality.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Provides a plot manipulator for tracker functionality.
    /// </summary>
    public class TouchTrackerManipulator : TouchManipulator
    {
        /// <summary>
        /// The current series.
        /// </summary>
        private Series.Series currentSeries;

        /// <summary>
        /// Initializes a new instance of the <see cref="TouchTrackerManipulator" /> class.
        /// </summary>
        /// <param name="plotView">The plot view.</param>
        public TouchTrackerManipulator(IPlotView plotView)
            : base(plotView)
        {
            this.Snap = true;
            this.PointsOnly = false;
            this.LockToInitialSeries = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show tracker on points only (not interpolating).
        /// </summary>
        public bool PointsOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to snap to the nearest point.
        /// </summary>
        public bool Snap { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to lock the tracker to the initial series.
        /// </summary>
        /// <value><c>true</c> if the tracker should be locked; otherwise, <c>false</c>.</value>
        public bool LockToInitialSeries { get; set; }

        /// <summary>
        /// Occurs when a manipulation is complete.
        /// </summary>
        /// <param name="e">The <see cref="OxyPlot.OxyTouchEventArgs" /> instance containing the event data.</param>
        public override void Completed(OxyTouchEventArgs e)
        {
            base.Completed(e);
            e.Handled = true;

            this.currentSeries = null;
            this.PlotView.HideTracker();
            if (this.PlotView.ActualModel != null)
            {
                this.PlotView.ActualModel.RaiseTrackerChanged(null);
            }
        }

        /// <summary>
        /// Occurs when the input device changes position during a manipulation.
        /// </summary>
        /// <param name="e">The <see cref="OxyPlot.OxyTouchEventArgs" /> instance containing the event data.</param>
        public override void Delta(OxyTouchEventArgs e)
        {
            // For now don't support delta (it's used for other events)
        }

        /// <summary>
        /// Occurs when an input device begins a manipulation on the plot.
        /// </summary>
        /// <param name="e">The <see cref="OxyPlot.OxyTouchEventArgs" /> instance containing the event data.</param>
        public override void Started(OxyTouchEventArgs e)
        {
            base.Started(e);
            this.currentSeries = this.PlotView.ActualModel != null ? this.PlotView.ActualModel.GetSeriesFromPoint(e.Position) : null;

            UpdateTracker(e.Position);
            e.Handled = true;
        }

        /// <summary>
        /// Updates the tracker to the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        private void UpdateTracker(ScreenPoint position)
        {
            if (this.currentSeries == null || !this.LockToInitialSeries)
            {
                // get the nearest
                this.currentSeries = this.PlotView.ActualModel != null ? this.PlotView.ActualModel.GetSeriesFromPoint(position, 20) : null;
            }

            if (this.currentSeries == null)
            {
                if (!this.LockToInitialSeries)
                {
                    this.PlotView.HideTracker();
                }

                return;
            }

            var actualModel = this.PlotView.ActualModel;
            if (actualModel == null)
            {
                return;
            }

            if (!actualModel.PlotArea.Contains(position.X, position.Y))
            {
                return;
            }

            var result = GetNearestHit(this.currentSeries, position, this.Snap, this.PointsOnly);
            if (result != null)
            {
                result.PlotModel = this.PlotView.ActualModel;
                this.PlotView.ShowTracker(result);
                this.PlotView.ActualModel.RaiseTrackerChanged(result);
            }
        }

        /// <summary>
        /// Gets the nearest tracker hit.
        /// </summary>
        /// <param name="series">The series.</param>
        /// <param name="point">The point.</param>
        /// <param name="snap">Snap to points.</param>
        /// <param name="pointsOnly">Check points only (no interpolation).</param>
        /// <returns>A tracker hit result.</returns>
        private static TrackerHitResult GetNearestHit(Series.Series series, ScreenPoint point, bool snap, bool pointsOnly)
        {
            if (series == null)
            {
                return null;
            }

            // Check data points only
            if (snap || pointsOnly)
            {
                var result = series.GetNearestPoint(point, false);
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
                var result = series.GetNearestPoint(point, true);
                return result;
            }

            return null;
        }
    }
}