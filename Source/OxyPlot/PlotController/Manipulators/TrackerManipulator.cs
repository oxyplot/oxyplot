// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackerManipulator.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Provides a plot control manipulator for tracker functionality.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using OxyPlot.Series;

    /// <summary>
    /// Provides a plot control manipulator for tracker functionality.
    /// </summary>
    public class TrackerManipulator : MouseManipulator
    {
        /// <summary>
        /// The current series.
        /// </summary>
        private ITrackableSeries currentSeries;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackerManipulator" /> class.
        /// </summary>
        /// <param name="plotControl">The plot control.</param>
        public TrackerManipulator(IPlotControl plotControl)
            : base(plotControl)
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
        /// <value>
        ///   <c>true</c> if the tracker should be locked; otherwise, <c>false</c>.
        /// </value>
        public bool LockToInitialSeries { get; set; }

        /// <summary>
        /// Occurs when a manipulation is complete.
        /// </summary>
        /// <param name="e">
        /// The <see cref="OxyPlot.OxyMouseEventArgs"/> instance containing the event data.
        /// </param>
        public override void Completed(OxyMouseEventArgs e)
        {
            base.Completed(e);

            this.currentSeries = null;
            this.PlotControl.HideTracker();
            if (this.PlotControl.ActualModel != null)
            {
                this.PlotControl.ActualModel.OnTrackerChanged(null);
            }
        }

        /// <summary>
        /// Occurs when the input device changes position during a manipulation.
        /// </summary>
        /// <param name="e">
        /// The <see cref="OxyPlot.OxyMouseEventArgs"/> instance containing the event data.
        /// </param>
        public override void Delta(OxyMouseEventArgs e)
        {
            base.Delta(e);

            if (this.currentSeries == null || !this.LockToInitialSeries)
            {
                // get the nearest
                this.currentSeries = this.PlotControl.ActualModel != null ? this.PlotControl.ActualModel.GetSeriesFromPoint(e.Position, 20) : null;
            }

            if (this.currentSeries == null)
            {
                if (!this.LockToInitialSeries)
                {
                    this.PlotControl.HideTracker();
                }

                return;
            }

            var actualModel = this.PlotControl.ActualModel;
            if (actualModel == null)
            {
                return;
            }

            if (!actualModel.PlotArea.Contains(e.Position.X, e.Position.Y))
            {
                return;
            }

            var result = GetNearestHit(this.currentSeries, e.Position, this.Snap, this.PointsOnly);
            if (result != null)
            {
                result.PlotModel = this.PlotControl.ActualModel;
                this.PlotControl.ShowTracker(result);
                this.PlotControl.ActualModel.OnTrackerChanged(result);
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
        /// The <see cref="OxyPlot.OxyMouseEventArgs"/> instance containing the event data.
        /// </param>
        public override void Started(OxyMouseEventArgs e)
        {
            base.Started(e);
            this.currentSeries = this.PlotControl.ActualModel != null ? this.PlotControl.ActualModel.GetSeriesFromPoint(e.Position) : null;
            this.Delta(e);
        }

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
                var result = s.GetNearestPoint(point, false);
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
                var result = s.GetNearestPoint(point, true);
                return result;
            }

            return null;
        }
    }
}