using System.Windows.Input;

namespace OxyPlot.Wpf
{
    public interface IMouseAction
    {
        void OnMouseDown(System.Windows.Point pt, MouseButton button, int clickCount, bool control, bool shift);
        void OnMouseMove(System.Windows.Point pt, bool control, bool shift);
        void OnMouseUp();
        void OnMouseWheel(System.Windows.Point pt, double delta, bool control, bool shift);
    }
}