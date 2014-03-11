// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotModel.MouseEvents.cs" company="OxyPlot">
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
// <summary>
//   Partial PlotModel class - this file contains mouse events and handlers.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Linq;

    /// <summary>
    /// Partial PlotModel class - this file contains mouse events and handlers.
    /// </summary>
    public partial class PlotModel
    {
        /// <summary>
        /// The mouse hit tolerance.
        /// </summary>
        private const double MouseHitTolerance = 10;

        /// <summary>
        /// The element that receives mouse move events.
        /// </summary>
        private UIPlotElement currentMouseEventElement;

        /// <summary>
        /// The element that receives touch delta events.
        /// </summary>
        private UIPlotElement currentTouchEventElement;

        /// <summary>
        /// Occurs when a key is pressed down when the plot view is focused.
        /// </summary>
        public event EventHandler<OxyKeyEventArgs> KeyDown;

        /// <summary>
        /// Occurs when a mouse button is pressed down on the model.
        /// </summary>
        public event EventHandler<OxyMouseDownEventArgs> MouseDown;

        /// <summary>
        /// Occurs when the mouse is moved on the plot element (only occurs after MouseDown).
        /// </summary>
        public event EventHandler<OxyMouseEventArgs> MouseMove;

        /// <summary>
        /// Occurs when the mouse button is released on the plot element.
        /// </summary>
        public event EventHandler<OxyMouseEventArgs> MouseUp;

        /// <summary>
        /// Occurs when the mouse cursor enters the plot area.
        /// </summary>
        public event EventHandler<OxyMouseEventArgs> MouseEnter;

        /// <summary>
        /// Occurs when the mouse cursor leaves the plot area.
        /// </summary>
        public event EventHandler<OxyMouseEventArgs> MouseLeave;

        /// <summary>
        /// Occurs when a touch gesture is started.
        /// </summary>
        public event EventHandler<OxyTouchEventArgs> TouchStarted;

        /// <summary>
        /// Occurs when a touch gesture is changed.
        /// </summary>
        public event EventHandler<OxyTouchEventArgs> TouchDelta;

        /// <summary>
        /// Occurs when a touch gesture is completed.
        /// </summary>
        public event EventHandler<OxyTouchEventArgs> TouchCompleted;

        /// <summary>
        /// Handles the mouse down event.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="OxyPlot.OxyMouseEventArgs"/> instance containing the event data.
        /// </param>
        public void HandleMouseDown(object sender, OxyMouseDownEventArgs e)
        {
            // Revert the order to handle the top-level elements first
            foreach (var element in this.GetElements().Reverse())
            {
                var uiElement = element as UIPlotElement;
                if (uiElement == null)
                {
                    continue;
                }

                var result = uiElement.HitTest(e.Position, MouseHitTolerance);
                if (result != null)
                {
                    e.HitTestResult = result;
                    uiElement.OnMouseDown(sender, e);
                    if (e.Handled)
                    {
                        this.currentMouseEventElement = uiElement;
                    }
                }

                if (e.Handled)
                {
                    break;
                }
            }

            if (!e.Handled)
            {
                this.OnMouseDown(sender, e);
            }
        }

        /// <summary>
        /// Handles the mouse move event.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="OxyPlot.OxyMouseEventArgs"/> instance containing the event data.
        /// </param>
        public void HandleMouseMove(object sender, OxyMouseEventArgs e)
        {
            if (this.currentMouseEventElement != null)
            {
                this.currentMouseEventElement.OnMouseMove(sender, e);
            }

            if (!e.Handled)
            {
                this.OnMouseMove(sender, e);
            }
        }

        /// <summary>
        /// Handles the mouse up event.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="OxyPlot.OxyMouseEventArgs"/> instance containing the event data.
        /// </param>
        public void HandleMouseUp(object sender, OxyMouseEventArgs e)
        {
            if (this.currentMouseEventElement != null)
            {
                this.currentMouseEventElement.OnMouseUp(sender, e);
                this.currentMouseEventElement = null;
            }

            if (!e.Handled)
            {
                this.OnMouseUp(sender, e);
            }
        }

        /// <summary>
        /// Handles the mouse enter event.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="OxyPlot.OxyMouseEventArgs"/> instance containing the event data.
        /// </param>
        public void HandleMouseEnter(object sender, OxyMouseEventArgs e)
        {
            if (!e.Handled)
            {
                this.OnMouseEnter(sender, e);
            }
        }

        /// <summary>
        /// Handles the mouse leave event.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="OxyPlot.OxyMouseEventArgs"/> instance containing the event data.
        /// </param>
        public void HandleMouseLeave(object sender, OxyMouseEventArgs e)
        {
            if (!e.Handled)
            {
                this.OnMouseLeave(sender, e);
            }
        }

        /// <summary>
        /// Handles the touch started event.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// A <see cref="OxyPlot.OxyTouchEventArgs"/> instance containing the event data.
        /// </param>
        public void HandleTouchStarted(object sender, OxyTouchEventArgs e)
        {
            // Revert the order to handle the top-level elements first
            foreach (var element in this.GetElements().Reverse())
            {
                var uiElement = element as UIPlotElement;
                if (uiElement == null)
                {
                    continue;
                }

                var result = uiElement.HitTest(e.Position, MouseHitTolerance);
                if (result != null)
                {
                    uiElement.OnTouchStarted(sender, e);
                    if (e.Handled)
                    {
                        this.currentTouchEventElement = uiElement;
                    }
                }

                if (e.Handled)
                {
                    break;
                }
            }

            if (!e.Handled)
            {
                this.OnTouchStarted(sender, e);
            }
        }

        /// <summary>
        /// Handles the touch delta event.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// A <see cref="OxyPlot.OxyTouchEventArgs"/> instance containing the event data.
        /// </param>
        public void HandleTouchDelta(object sender, OxyTouchEventArgs e)
        {
            if (this.currentTouchEventElement != null)
            {
                this.currentTouchEventElement.OnTouchDelta(sender, e);
            }

            if (!e.Handled)
            {
                this.OnTouchDelta(sender, e);
            }
        }

        /// <summary>
        /// Handles the touch completed event.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// A <see cref="OxyPlot.OxyTouchEventArgs"/> instance containing the event data.
        /// </param>
        public void HandleTouchCompleted(object sender, OxyTouchEventArgs e)
        {
            if (this.currentTouchEventElement != null)
            {
                this.currentTouchEventElement.OnTouchCompleted(sender, e);
                this.currentTouchEventElement = null;
            }

            if (!e.Handled)
            {
                this.OnTouchCompleted(sender, e);
            }
        }

        /// <summary>
        /// Handles key down events.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="OxyKeyEventArgs"/> instance containing the event data.</param>
        public void HandleKeyDown(object sender, OxyKeyEventArgs e)
        {
            // Revert the order to handle the top-level elements first
            foreach (var element in this.GetElements().Reverse())
            {
                var uiElement = element as UIPlotElement;
                if (uiElement == null)
                {
                    continue;
                }

                uiElement.OnKeyDown(sender, e);

                if (e.Handled)
                {
                    break;
                }
            }

            if (!e.Handled)
            {
                this.OnKeyDown(sender, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="KeyDown"/> event.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="OxyMouseEventArgs"/> instance containing the event data.
        /// </param>
        protected virtual void OnKeyDown(object sender, OxyKeyEventArgs e)
        {
            var handler = this.KeyDown;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseDown"/> event.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="OxyMouseEventArgs"/> instance containing the event data.
        /// </param>
        protected virtual void OnMouseDown(object sender, OxyMouseDownEventArgs e)
        {
            var handler = this.MouseDown;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseMove"/> event.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="OxyMouseEventArgs"/> instance containing the event data.
        /// </param>
        protected virtual void OnMouseMove(object sender, OxyMouseEventArgs e)
        {
            var handler = this.MouseMove;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseUp"/> event.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="OxyMouseEventArgs"/> instance containing the event data.
        /// </param>
        protected virtual void OnMouseUp(object sender, OxyMouseEventArgs e)
        {
            var handler = this.MouseUp;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseEnter"/> event.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="OxyMouseEventArgs"/> instance containing the event data.
        /// </param>
        protected virtual void OnMouseEnter(object sender, OxyMouseEventArgs e)
        {
            var handler = this.MouseEnter;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseLeave"/> event.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="OxyMouseEventArgs"/> instance containing the event data.
        /// </param>
        protected virtual void OnMouseLeave(object sender, OxyMouseEventArgs e)
        {
            var handler = this.MouseLeave;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseDown"/> event.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="OxyMouseEventArgs"/> instance containing the event data.
        /// </param>
        protected virtual void OnTouchStarted(object sender, OxyTouchEventArgs e)
        {
            var handler = this.TouchStarted;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseMove"/> event.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="OxyMouseEventArgs"/> instance containing the event data.
        /// </param>
        protected virtual void OnTouchDelta(object sender, OxyTouchEventArgs e)
        {
            var handler = this.TouchDelta;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseUp"/> event.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="OxyMouseEventArgs"/> instance containing the event data.
        /// </param>
        protected virtual void OnTouchCompleted(object sender, OxyTouchEventArgs e)
        {
            var handler = this.TouchCompleted;
            if (handler != null)
            {
                handler(sender, e);
            }
        }
    }
}