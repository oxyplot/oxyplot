using System.Diagnostics;
using System.Windows.Input;

namespace OxyPlot.Wpf
{
    public abstract class MouseAction : IMouseAction
    {
        protected PlotControl pc;

        protected MouseAction(PlotControl pc)
        {
            this.pc = pc;
            SubscribeEvents(pc);
        }

        private void SubscribeEvents(PlotControl pc)
        {
            pc.MouseDown += PlotControl_MouseDown;
            pc.MouseMove += PlotControl_MouseMove;
            pc.MouseUp += PlotControl_MouseUp;
            pc.MouseWheel += PlotControl_MouseWheel;
        }

        void PlotControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            bool control = (Keyboard.IsKeyDown(Key.LeftCtrl));
            bool shift = (Keyboard.IsKeyDown(Key.LeftShift));

            OnMouseWheel(e.GetPosition(pc), e.Delta, control, shift);
        }

        private void PlotControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            OnMouseUp();
        }

        private void PlotControl_MouseMove(object sender, MouseEventArgs e)
        {
            bool control = (Keyboard.IsKeyDown(Key.LeftCtrl));
            bool shift = (Keyboard.IsKeyDown(Key.LeftShift));
            OnMouseMove(e.GetPosition(pc), control, shift);
        }

        private void PlotControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            bool control = (Keyboard.IsKeyDown(Key.LeftCtrl));
            bool shift = (Keyboard.IsKeyDown(Key.LeftShift));

            MouseButton button = MouseButton.Left;
            if (e.LeftButton == MouseButtonState.Pressed)
                button = MouseButton.Left;
            if (e.MiddleButton == MouseButtonState.Pressed)
                button = MouseButton.Middle;
            if (e.RightButton == MouseButtonState.Pressed)
                button = MouseButton.Right;
            if (e.XButton1 == MouseButtonState.Pressed)
                button = MouseButton.XButton1;
            if (e.XButton2 == MouseButtonState.Pressed)
                button = MouseButton.XButton2;
            OnMouseDown(e.GetPosition(pc), button, e.ClickCount, control, shift);
        }

        public virtual void OnMouseDown(System.Windows.Point pt, MouseButton button, int clickCount, bool control, bool shift)
        {
        }

        public virtual void OnMouseMove(System.Windows.Point pt, bool control, bool shift)
        {
        }

        public virtual void OnMouseUp()
        {
        }

        public virtual void OnMouseWheel(System.Windows.Point pt, double delta, bool control, bool shift)
        {
        }

    }
}