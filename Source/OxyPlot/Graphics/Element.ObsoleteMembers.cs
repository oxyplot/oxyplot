// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UIElement.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an abstract base class for elements that handle mouse events.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// Provides an abstract base class for graphics elements.
    /// </summary>
    public abstract partial class Element
    {
        /// <summary>
        /// Occurs when a key is pressed down when the plot view is in focus.
        /// </summary>
        [Obsolete("Will be removed in v4.0 (#111)")]
        public event EventHandler<OxyKeyEventArgs> KeyDown;

        /// <summary>
        /// Occurs when a mouse button is pressed down on the model.
        /// </summary>
        [Obsolete("Will be removed in v4.0 (#111)")]
        public event EventHandler<OxyMouseDownEventArgs> MouseDown;

        /// <summary>
        /// Occurs when the mouse is moved on the plot element (only occurs after MouseDown).
        /// </summary>
        [Obsolete("Will be removed in v4.0 (#111)")]
        public event EventHandler<OxyMouseEventArgs> MouseMove;

        /// <summary>
        /// Occurs when the mouse button is released on the plot element.
        /// </summary>
        [Obsolete("Will be removed in v4.0 (#111)")]
        public event EventHandler<OxyMouseEventArgs> MouseUp;

        /// <summary>
        /// Occurs when a touch gesture starts.
        /// </summary>
        [Obsolete("Will be removed in v4.0 (#111)")]
        public event EventHandler<OxyTouchEventArgs> TouchStarted;

        /// <summary>
        /// Occurs when a touch gesture is changed.
        /// </summary>
        [Obsolete("Will be removed in v4.0 (#111)")]
        public event EventHandler<OxyTouchEventArgs> TouchDelta;

        /// <summary>
        /// Occurs when the touch gesture is completed.
        /// </summary>
        [Obsolete("Will be removed in v4.0 (#111)")]
        public event EventHandler<OxyTouchEventArgs> TouchCompleted;

        /// <summary>
        /// Raises the <see cref="MouseDown" /> event.
        /// </summary>
        /// <param name="e">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        [Obsolete("Will be removed in v4.0 (#111)")]
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
        [Obsolete("Will be removed in v4.0 (#111)")]
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
        [Obsolete("Will be removed in v4.0 (#111)")]
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
        [Obsolete("Will be removed in v4.0 (#111)")]
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
        [Obsolete("Will be removed in v4.0 (#111)")]
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
        [Obsolete("Will be removed in v4.0 (#111)")]
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
        [Obsolete("Will be removed in v4.0 (#111)")]
        protected internal virtual void OnTouchCompleted(OxyTouchEventArgs e)
        {
            var handler = this.TouchCompleted;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
