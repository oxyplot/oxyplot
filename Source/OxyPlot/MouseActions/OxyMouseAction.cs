namespace OxyPlot
{
    public abstract class OxyMouseAction : IMouseAction
    {
        protected IPlotControl pc;

        protected OxyMouseAction(IPlotControl pc)
        {
            this.pc = pc;
        }

        public virtual void OnMouseDown(ScreenPoint pt, OxyMouseButton button, int clickCount, bool control, bool shift, bool alt)
        {
        }

        public virtual void OnMouseMove(ScreenPoint pt, bool control, bool shift, bool alt)
        {
        }

        public virtual void OnMouseUp()
        {
        }

        public virtual void OnMouseWheel(ScreenPoint pt, double delta, bool control, bool shift, bool alt)
        {
        }

        protected DataPoint InverseTransform(double x, double y, IAxis xaxis, IAxis yaxis)
        {
            if (xaxis != null) return xaxis.InverseTransform(x, y, yaxis);
            if (yaxis != null) return new DataPoint(0, yaxis.InverseTransform(y));
            return new DataPoint();
        }
    }
}