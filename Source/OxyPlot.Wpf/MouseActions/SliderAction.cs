using System.Windows.Input;

namespace OxyPlot.Wpf
{
    // todo: use screen coordinates instead of original points (problem on log axes)
    public class SliderAction : MouseAction
    {

        public SliderAction(PlotControl pc)
            : base(pc)
        {
        }

        private OxyPlot.DataSeries currentSeries;

        public override void OnMouseDown(System.Windows.Point pt, MouseButton button, int clickCount, bool control, bool shift)
        {
            base.OnMouseDown(pt, button, clickCount, control, shift);

            if (button != MouseButton.Left)
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

        public override void OnMouseMove(System.Windows.Point pt, bool control, bool shift)
        {
            if (currentSeries == null)
                return;

            var current = GetNearestPoint(currentSeries, pt, !control, shift);
            if (current != null)
                pc.ShowSlider(currentSeries, current.Value);
        }

        private DataPoint? GetNearestPoint(OxyPlot.DataSeries s, System.Windows.Point pt, bool snap, bool pointsOnly)
        {
            if (s == null)
                return null;
            var dp = pc.InverseTransform(pt, s.XAxis, s.YAxis);

            if (snap || pointsOnly)
            {
                var dpn = s.GetNearestPoint(dp);
                if (dpn != null && snap)
                {
                    var spn = pc.Transform(dpn.Value, s.XAxis, s.YAxis);
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