// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackerManipulator.cs" company="OxyPlot">
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
    public class TrackerManipulator : MouseManipulator
    {
        /// <summary>
        /// The current series.
        /// </summary>
        private Series.Series currentSeries;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackerManipulator" /> class.
        /// </summary>
        /// <param name="plotView">The plot view.</param>
        public TrackerManipulator(IPlotView plotView)
            : base(plotView)
        {
            this.Snap = true;
            this.PointsOnly = false;
            this.LockToInitialSeries = true;
            this.FiresDistance = 20.0;
            this.CheckDistanceBetweenPoints = false;
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
        /// Gets or sets the distance from the series at which the tracker fires.
        /// </summary>
        public double FiresDistance { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to check distance when showing tracker between data points.
        /// </summary>
        /// <remarks>This parameter is ignored if <see cref="PointsOnly"/> is equal to <c>False</c>.</remarks>
        public bool CheckDistanceBetweenPoints { get; set; }

        /// <summary>
        /// Occurs when a manipulation is complete.
        /// </summary>
        /// <param name="e">The <see cref="OxyPlot.OxyMouseEventArgs" /> instance containing the event data.</param>
        public override void Completed(OxyMouseEventArgs e)
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
        /// <param name="e">The <see cref="OxyPlot.OxyMouseEventArgs" /> instance containing the event data.</param>
        public override void Delta(OxyMouseEventArgs e)
        {
            base.Delta(e);
            e.Handled = true;

            if (this.currentSeries == null || !this.LockToInitialSeries)
            {
                // get the nearest
                this.currentSeries = this.PlotView.ActualModel?.GetSeriesFromPoint(e.Position, this.FiresDistance);
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

            if (!actualModel.PlotArea.Contains(e.Position.X, e.Position.Y))
            {
                return;
            }

            var result = Utilities.TrackerHelper.GetNearestHit(
                this.currentSeries, e.Position, this.Snap, this.PointsOnly, this.FiresDistance, this.CheckDistanceBetweenPoints);
            if (result != null)
            {
                result.PlotModel = this.PlotView.ActualModel;
                this.PlotView.ShowTracker(result);
                this.PlotView.ActualModel.RaiseTrackerChanged(result);
            }
        }

        /// <summary>
        /// Occurs when an input device begins a manipulation on the plot.
        /// </summary>
        /// <param name="e">The <see cref="OxyPlot.OxyMouseEventArgs" /> instance containing the event data.</param>
        public override void Started(OxyMouseEventArgs e)
        {
            base.Started(e);
            this.currentSeries = this.PlotView.ActualModel?.GetSeriesFromPoint(e.Position, FiresDistance);
            this.Delta(e);
        }
    }
}
