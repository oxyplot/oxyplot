// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UIPlotElement.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
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
//   Provides an abstract base class for plot elements that handle mouse events.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// Provides an abstract base class for plot elements that handle mouse events.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public abstract class UIPlotElement : SelectablePlotElement
    {
        /// <summary>
        /// Occurs when a key is pressed down when the plot view is in focus.
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
        /// Occurs when a touch gesture starts.
        /// </summary>
        public event EventHandler<OxyTouchEventArgs> TouchStarted;

        /// <summary>
        /// Occurs when a touch gesture is changed.
        /// </summary>
        public event EventHandler<OxyTouchEventArgs> TouchDelta;

        /// <summary>
        /// Occurs when the touch gesture is completed.
        /// </summary>
        public event EventHandler<OxyTouchEventArgs> TouchCompleted;

        /// <summary>
        /// Tests if the plot element is hit by the specified point.
        /// </summary>
        /// <param name="args">The hit test arguments.</param>
        /// <returns>
        /// A hit test result.
        /// </returns>
        public HitTestResult HitTest(HitTestArguments args)
        {
            return this.HitTestOverride(args);
        }

        /// <summary>
        /// Raises the <see cref="MouseDown" /> event.
        /// </summary>
        /// <param name="e">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        protected internal virtual void OnMouseDown(OxyMouseDownEventArgs e)
        {
            var handler = this.MouseDown;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseMove" /> event.
        /// </summary>
        /// <param name="e">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        protected internal virtual void OnMouseMove(OxyMouseEventArgs e)
        {
            var handler = this.MouseMove;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="KeyDown" /> event.
        /// </summary>
        /// <param name="e">The <see cref="OxyKeyEventArgs" /> instance containing the event data.</param>
        protected internal virtual void OnKeyDown(OxyKeyEventArgs e)
        {
            var handler = this.KeyDown;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseUp" /> event.
        /// </summary>
        /// <param name="e">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        protected internal virtual void OnMouseUp(OxyMouseEventArgs e)
        {
            var handler = this.MouseUp;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="TouchStarted" /> event.
        /// </summary>
        /// <param name="e">The <see cref="OxyTouchEventArgs" /> instance containing the event data.</param>
        protected internal virtual void OnTouchStarted(OxyTouchEventArgs e)
        {
            var handler = this.TouchStarted;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="TouchDelta" /> event.
        /// </summary>
        /// <param name="e">The <see cref="OxyTouchEventArgs" /> instance containing the event data.</param>
        protected internal virtual void OnTouchDelta(OxyTouchEventArgs e)
        {
            var handler = this.TouchDelta;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseUp" /> event.
        /// </summary>
        /// <param name="e">The <see cref="OxyTouchEventArgs" /> instance containing the event data.</param>
        protected internal virtual void OnTouchCompleted(OxyTouchEventArgs e)
        {
            var handler = this.TouchCompleted;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// When overridden in a derived class, tests if the plot element is hit by the specified point.
        /// </summary>
        /// <param name="args">The hit test arguments.</param>
        /// <returns>
        /// The result of the hit test.
        /// </returns>
        protected abstract HitTestResult HitTestOverride(HitTestArguments args);
    }
}