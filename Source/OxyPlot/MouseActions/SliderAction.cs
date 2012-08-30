namespace OxyPlot
{
    // todo: use screen coordinates instead of original points (problem on log axes)
    public class SliderAction : MouseAction
    {
        public SliderAction(IPlot pc)
            : base(pc)
        {
        }

        private DataSeries currentSeries;

        public override void OnMouseDown(ScreenPoint pt, OxyMouseButton button, int clickCount, bool control, bool shift)
        {
            base.OnMouseDown(pt, button, clickCount, control, shift);

            if (button != OxyMouseButton.Left)
                return;

            // Middle button double click adds an annotation
            if (clickCount == 2)
            {
                // pc.Annotations.
                pc.Refresh();
            }

            currentSeries = pc.GetSeriesFromPoint(pt) as DataSeries;

            OnMouseMove(pt, control, shift);

            //pc.CaptureMouse();
            // pc.Cursor = Cursors.Cross;
        }

        public override void OnMouseMove(ScreenPoint pt, bool control, bool shift)
        {
            if (currentSeries == null)
                return;

            var current = GetNearestPoint(currentSeries, pt, !control, shift);
            if (current != null)
                pc.ShowSlider(currentSeries, current.Value);
        }

        private static DataPoint? GetNearestPoint(ISeries s, ScreenPoint point, bool snap, bool pointsOnly)
        {
            if (s == null)
                return null;

            if (snap || pointsOnly)
            {
                ScreenPoint spn;
                DataPoint dpn;
                if (s.GetNearestPoint(point, out dpn, out spn) && snap)
                {
                    if (spn.DistanceTo(point) < 20)
                        return dpn;
                }
            }

            ScreenPoint sp;
            DataPoint dp;

            if (!pointsOnly)
                if (s.GetNearestInterpolatedPoint(point, out dp, out sp))
                    return dp;

            return null;
        }

        public override void OnMouseUp()
        {
            base.OnMouseUp();
            if (currentSeries == null)
                return;
            currentSeries = null;
            pc.HideSlider();
        }
    }
}