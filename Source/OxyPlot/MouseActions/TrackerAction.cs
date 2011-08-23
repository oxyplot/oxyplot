namespace OxyPlot
{
    /// <summary>
    /// Tracker mouseaction
    /// </summary>
    public class TrackerAction : OxyMouseAction
    {
        public TrackerAction(IPlotControl pc)
            : base(pc)
        {
        }

        private ITrackableSeries currentSeries;

        public override void OnMouseDown(ScreenPoint pt, OxyMouseButton button, int clickCount, bool control, bool shift, bool alt)
        {
            base.OnMouseDown(pt, button, clickCount, control, shift, alt);

            if (alt)
                return;

            if (button != OxyMouseButton.Left)
                return;

            // Left button double click adds an annotation
            if (clickCount == 2)
            {
                // todo
                // pc.Annotations.Add
                pc.InvalidatePlot();
            }

            currentSeries = pc.GetSeriesFromPoint(pt) as ITrackableSeries;

            OnMouseMove(pt, control, shift, alt);

            //pc.CaptureMouse();
            // pc.Cursor = Cursors.Cross;
        }

        public override void OnMouseMove(ScreenPoint pt, bool control, bool shift, bool alt)
        {
            if (currentSeries == null)
                return;

            if (!pc.ActualModel.PlotArea.Contains(pt.X,pt.Y)) return;

            var result = GetNearestPoint(currentSeries, pt, !control, shift);
            if (result != null)
            {
                result.PlotModel = pc.ActualModel;
                pc.ShowTracker(result);
            }
        }

        private static TrackerHitResult GetNearestPoint(ITrackableSeries s, ScreenPoint point, bool snap, bool pointsOnly)
        {
            if (s == null)
                return null;

            // Check data points only
            if (snap || pointsOnly)
            {
                var result=s.GetNearestPoint(point, false);
                if (result!=null) {
                    if (result.Position.DistanceTo(point) < 20)
                        return result;
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

        public override void OnMouseUp()
        {
            base.OnMouseUp();
            if (currentSeries == null)
                return;
            currentSeries = null;
            pc.HideTracker();
        }
    }
}