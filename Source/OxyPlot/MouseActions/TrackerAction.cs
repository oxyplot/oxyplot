//-----------------------------------------------------------------------
// <copyright file="TrackerAction.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Tracker mouseaction
    /// </summary>
    public class TrackerAction : OxyMouseAction
    {
        #region Constants and Fields

        /// <summary>
        ///   The current series.
        /// </summary>
        private ITrackableSeries currentSeries;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackerAction"/> class.
        /// </summary>
        /// <param name="pc">
        /// The pc.
        /// </param>
        public TrackerAction(IPlotControl pc)
            : base(pc)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The on mouse down.
        /// </summary>
        /// <param name="pt">
        /// The pt.
        /// </param>
        /// <param name="button">
        /// The button.
        /// </param>
        /// <param name="clickCount">
        /// The click count.
        /// </param>
        /// <param name="control">
        /// The control.
        /// </param>
        /// <param name="shift">
        /// The shift.
        /// </param>
        /// <param name="alt">
        /// The alt.
        /// </param>
        public override void OnMouseDown(
            ScreenPoint pt, OxyMouseButton button, int clickCount, bool control, bool shift, bool alt)
        {
            base.OnMouseDown(pt, button, clickCount, control, shift, alt);

            if (alt)
            {
                return;
            }

            if (button != OxyMouseButton.Left)
            {
                return;
            }

            // Left button double click adds an annotation
            if (clickCount == 2)
            {
                // todo
                // pc.Annotations.Add
                this.pc.InvalidatePlot();
            }

            this.currentSeries = this.pc.GetSeriesFromPoint(pt) as ITrackableSeries;

            this.OnMouseMove(pt, control, shift, alt);

            // pc.CaptureMouse();
            // pc.Cursor = Cursors.Cross;
        }

        /// <summary>
        /// The on mouse move.
        /// </summary>
        /// <param name="pt">
        /// The pt.
        /// </param>
        /// <param name="control">
        /// The control.
        /// </param>
        /// <param name="shift">
        /// The shift.
        /// </param>
        /// <param name="alt">
        /// The alt.
        /// </param>
        public override void OnMouseMove(ScreenPoint pt, bool control, bool shift, bool alt)
        {
            if (this.currentSeries == null)
            {
                return;
            }

            if (!this.pc.ActualModel.PlotArea.Contains(pt.X, pt.Y))
            {
                return;
            }

            TrackerHitResult result = GetNearestPoint(this.currentSeries, pt, !control, shift);
            if (result != null)
            {
                result.PlotModel = this.pc.ActualModel;
                this.pc.ShowTracker(result);
            }
        }

        /// <summary>
        /// The on mouse up.
        /// </summary>
        public override void OnMouseUp()
        {
            base.OnMouseUp();
            if (this.currentSeries == null)
            {
                return;
            }

            this.currentSeries = null;
            this.pc.HideTracker();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get nearest point.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <param name="point">
        /// The point.
        /// </param>
        /// <param name="snap">
        /// The snap.
        /// </param>
        /// <param name="pointsOnly">
        /// The points only.
        /// </param>
        /// <returns>
        /// </returns>
        private static TrackerHitResult GetNearestPoint(
            ITrackableSeries s, ScreenPoint point, bool snap, bool pointsOnly)
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
