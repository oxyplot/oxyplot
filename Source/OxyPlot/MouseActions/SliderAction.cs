namespace OxyPlot
{
    // todo: use screen coordinates instead of original points (problem on log axes)
    public class SliderAction : MouseAction
    {
        public SliderAction(IPlotControl pc)
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

            currentSeries = pc.GetSeriesFromPoint(pt);

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

        private DataPoint? GetNearestPoint(DataSeries s, ScreenPoint pt, bool snap, bool pointsOnly)
        {
            if (s == null)
                return null;
            var dp = AxisBase.InverseTransform(pt, s.XAxis, s.YAxis);

            if (snap || pointsOnly)
            {
                var dpn = s.GetNearestPoint(dp);
                if (dpn != null && snap)
                {
                    var spn = s.XAxis.Transform(dpn.Value.X, dpn.Value.Y, s.YAxis);
                    if (spn.DistanceTo(pt) < 20)
                        return dpn;
                }
            }

            if (!pointsOnly)
                return s.GetNearestPointOnLine(dp);

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