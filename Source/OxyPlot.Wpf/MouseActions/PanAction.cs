using System.Windows.Input;

namespace OxyPlot.Wpf
{
    public class PanAction : MouseAction
    {
        public PanAction(PlotControl pc)
            : base(pc)
        {
        }

        private OxyPlot.AxisBase xaxis;
        private OxyPlot.AxisBase yaxis;
        private DataPoint previousPoint;

        private bool isPanning;

        public override void OnMouseDown(System.Windows.Point pt, MouseButton button, int clickCount, bool control, bool shift)
        {
            if (button != MouseButton.Right || control)
                return;
            pc.GetAxesFromPoint(pt, out xaxis, out yaxis);

            // Right button double click resets the axis
            if (clickCount == 2)
            {
                if (xaxis != null) xaxis.Reset();
                if (yaxis != null) yaxis.Reset();
                pc.Refresh();
            }

            previousPoint = pc.InverseTransform(pt, xaxis, yaxis);
            pc.CaptureMouse();
            pc.Cursor = Cursors.SizeAll;
            isPanning = true;
        }

        public override void OnMouseMove(System.Windows.Point pt, bool control, bool shift)
        {
            if (!isPanning)
                return;
            var currentPoint = pc.InverseTransform(pt, xaxis, yaxis);
            double dx = currentPoint.X - previousPoint.X;
            double dy = currentPoint.Y - previousPoint.Y;
            if (xaxis != null)
                pc.Pan(xaxis, -dx);
            if (yaxis != null)
                pc.Pan(yaxis, -dy);
            pc.Refresh();
            previousPoint = pc.InverseTransform(pt, xaxis, yaxis);
        }

        public override void OnMouseUp()
        {
            if (!isPanning)
                return;

            pc.Cursor = Cursors.Arrow;
            pc.ReleaseMouseCapture();
            isPanning = false;
        }

    }
}