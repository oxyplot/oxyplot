using System.Windows.Input;

namespace OxyPlot.Wpf
{
    public class PanAction : MouseAction
    {
        public PanAction(PlotControl pc)
            : base(pc)
        {
        }
       
        private OxyPlot.Axis xaxis;
        private OxyPlot.Axis yaxis;
        private Point prevPoint;

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

            prevPoint = pc.InverseTransform(pt, xaxis, yaxis);
            pc.CaptureMouse();
            pc.Cursor = Cursors.SizeAll;
            isPanning = true;
        }

        public override void OnMouseMove(System.Windows.Point pt, bool control, bool shift)
        {
            if (!isPanning)
                return;
            var currentPoint = pc.InverseTransform(pt, xaxis, yaxis);
            double dx = currentPoint.X - prevPoint.X;
            double dy = currentPoint.Y - prevPoint.Y;
            if (xaxis != null)
                xaxis.Pan(-dx);
            if (yaxis != null)
                yaxis.Pan(-dy);
            pc.Refresh();
            prevPoint = pc.InverseTransform(pt, xaxis, yaxis);
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