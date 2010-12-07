using System;

namespace OxyPlot
{
    public class ZoomAction : MouseAction
    {
        public ScreenPoint DownPoint;
        private bool isZooming;
        private IAxis xaxis;
        private IAxis yaxis;

        private OxyRect zoomRectangle;

        public ZoomAction(IPlot pc)
            : base(pc)
        {
        }

        public override void OnMouseDown(ScreenPoint pt, OxyMouseButton button, int clickCount, bool control, bool shift)
        {
            base.OnMouseDown(pt, button, clickCount, control, shift);

            pc.GetAxesFromPoint(pt, out xaxis, out yaxis);

            if (button == OxyMouseButton.XButton1 || button == OxyMouseButton.XButton2)
            {
                var current = AxisBase.InverseTransform(pt.X, pt.Y, xaxis, yaxis); //pc.InverseTransform(pt, xaxis, yaxis));

                double scale = button == OxyMouseButton.XButton1 ? 0.05 : -0.05;
                if (control) scale *= 3;
                scale = 1 + scale;
                if (xaxis != null)
                    pc.ZoomAt(xaxis, scale, current.X);
                if (yaxis != null)
                    pc.ZoomAt(yaxis, scale, current.Y);
                pc.Refresh();
                return;
            }


            // RMB+Control is the same as MMB
            if (button == OxyMouseButton.Right && control) 
                button = OxyMouseButton.Middle;

            // RMB double-click is the same as MMB
            if (button==OxyMouseButton.Right && clickCount==2)
            {
                button = OxyMouseButton.Middle;
                clickCount = 1;
            }

            if (button != OxyMouseButton.Middle)
                return;

            // Middle button double click resets the axis
            if (clickCount == 2)
            {
                if (xaxis != null) pc.Reset(xaxis);
                if (yaxis != null) pc.Reset(yaxis);
                pc.Refresh();
            }

            DownPoint = pt;
            zoomRectangle = new OxyRect(pt.X, pt.Y, 0, 0);
            pc.ShowZoomRectangle(zoomRectangle);
            // pc.Cursor = Cursors.SizeNWSE;
            isZooming = true;
        }

        public override void OnMouseMove(ScreenPoint pt, bool control, bool shift)
        {
            if (!isZooming)
                return;
            var plotArea = pc.GetPlotArea();

            if (yaxis == null)
            {
                DownPoint.Y = plotArea.Top;
                pt.Y = plotArea.Bottom;
            }
            if (xaxis == null)
            {
                DownPoint.X = plotArea.Left;
                pt.X = plotArea.Right;
            }

            zoomRectangle = CreateRect(DownPoint, pt);
            pc.ShowZoomRectangle(zoomRectangle);
        }

        private static OxyRect CreateRect(ScreenPoint p0, ScreenPoint p1)
        {
            double x = Math.Min(p0.X, p1.X);
            double w = Math.Abs(p0.X - p1.X);
            double y = Math.Min(p0.Y, p1.Y);
            double h = Math.Abs(p0.Y - p1.Y);
            return new OxyRect(x, y, w, h);
        }

        public override void OnMouseUp()
        {
            if (!isZooming)
                return;

            // pc.Cursor = Cursors.Arrow;
            pc.HideZoomRectangle();

            if (zoomRectangle.Width > 10 && zoomRectangle.Height > 10)
            {
                var topLeft = new ScreenPoint(zoomRectangle.Left, zoomRectangle.Top);
                var bottomRight = new ScreenPoint(zoomRectangle.Right, zoomRectangle.Bottom);
                var p0 = AxisBase.InverseTransform(topLeft, xaxis, yaxis);
                var p1 = AxisBase.InverseTransform(bottomRight, xaxis, yaxis);

                if (xaxis != null)
                    pc.Zoom(xaxis, p0.X, p1.X);
                if (yaxis != null)
                    pc.Zoom(yaxis, p0.Y, p1.Y);
                pc.Refresh();
            }
            isZooming = false;
            base.OnMouseUp();
        }

        public override void OnMouseWheel(ScreenPoint pt, double delta, bool control, bool shift)
        {
            IAxis xa, ya;
            pc.GetAxesFromPoint(pt, out xa, out ya);
            var current = AxisBase.InverseTransform(pt, xa, ya);
            double f = 0.001;
            if (control) f *= 0.2;
            double s = 1 + delta*f;
            if (xa != null)
                pc.ZoomAt(xa, s, current.X);
            if (ya != null)
                pc.ZoomAt(ya, s, current.Y);
            pc.Refresh();
        }
    }
}