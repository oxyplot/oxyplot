// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MouseAction.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
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