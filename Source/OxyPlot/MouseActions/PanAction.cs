namespace OxyPlot
{
    public class PanAction : OxyMouseAction
    {
        public PanAction(IPlotControl pc)
            : base(pc)
        {
        }

        private IAxis xaxis;
        private IAxis yaxis;
        private ScreenPoint ppt;
        private bool isPanning;

        public override void OnMouseDown(ScreenPoint pt, OxyMouseButton button, int clickCount, bool control, bool shift, bool alt)
        {
            pc.GetAxesFromPoint(pt, out xaxis, out yaxis);
           
            if (alt)
                button = OxyMouseButton.Right;

            if (button != OxyMouseButton.Right || control)
                return;

            // Right button double click resets the axis
            if (clickCount == 2)
            {
                if (xaxis != null) xaxis.Reset();
                if (yaxis != null) yaxis.Reset();
                pc.InvalidatePlot();
                return;
            }

            ppt = pt;
            //   pc.Cursor = Cursors.SizeAll;
            isPanning = true;
        }

        public override void OnMouseMove(ScreenPoint pt, bool control, bool shift, bool alt)
        {
            if (!isPanning)
                return;
            var previousPoint = InverseTransform(ppt.X,ppt.Y, xaxis, yaxis);
            var currentPoint = InverseTransform(pt.X, pt.Y, xaxis, yaxis);
            if (xaxis != null)
                pc.Pan(xaxis, previousPoint.X,currentPoint.X);
            if (yaxis != null)
                pc.Pan(yaxis, previousPoint.Y,currentPoint.Y);

            pc.RefreshPlot(false);
            //pc.InvalidatePlot();
            ppt = pt;
        }

        public override void OnMouseUp()
        {
            if (!isPanning)
                return;

            //   pc.Cursor = Cursors.Arrow;
            isPanning = false;
        }
    }
}