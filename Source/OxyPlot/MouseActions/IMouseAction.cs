namespace OxyPlot
{
    public interface IMouseAction
    {
        void OnMouseDown(ScreenPoint pt, OxyMouseButton button, int clickCount, bool control, bool shift, bool alt);
        void OnMouseMove(ScreenPoint pt, bool control, bool shift, bool alt);
        void OnMouseUp();
        void OnMouseWheel(ScreenPoint pt, double delta, bool control, bool shift, bool alt);
    }
}