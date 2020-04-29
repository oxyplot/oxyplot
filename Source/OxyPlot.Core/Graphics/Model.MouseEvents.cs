// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Model.MouseEvents.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an abstract base class for graphics models.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// Provides an abstract base class for graphics models.
    /// </summary>
    public abstract partial class Model
    {
        /// <summary>
        /// The mouse hit tolerance.
        /// </summary>
        private const double MouseHitTolerance = 10;

        /// <summary>
        /// The element that receives mouse move events.
        /// </summary>
        private Element currentMouseEventElement;

        /// <summary>
        /// The element that receives touch delta events.
        /// </summary>
        private Element currentTouchEventElement;

        /// <summary>
        /// Occurs when a key is pressed down when the plot view is focused.
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
        /// Occurs when the mouse cursor enters the plot area.
        /// </summary>
        [Obsolete("Will be removed in v4.0 (#111)")]
        public event EventHandler<OxyMouseEventArgs> MouseEnter;

        /// <summary>
        /// Occurs when the mouse cursor leaves the plot area.
        /// </summary>
        [Obsolete("Will be removed in v4.0 (#111)")]
        public event EventHandler<OxyMouseEventArgs> MouseLeave;

        /// <summary>
        /// Occurs when a touch gesture is started.
        /// </summary>
        [Obsolete("Will be removed in v4.0 (#111)")]
        public event EventHandler<OxyTouchEventArgs> TouchStarted;

        /// <summary>
        /// Occurs when a touch gesture is changed.
        /// </summary>
        [Obsolete("Will be removed in v4.0 (#111)")]
        public event EventHandler<OxyTouchEventArgs> TouchDelta;

        /// <summary>
        /// Occurs when a touch gesture is completed.
        /// </summary>
        [Obsolete("Will be removed in v4.0 (#111)")]
        public event EventHandler<OxyTouchEventArgs> TouchCompleted;

        /// <summary>
        /// Handles the mouse down event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="OxyPlot.OxyMouseEventArgs" /> instance containing the event data.</param>
        [Obsolete("Will be removed in v4.0 (#111)")]
        public virtual void HandleMouseDown(object sender, OxyMouseDownEventArgs e)
        {
            var args = new HitTestArguments(e.Position, MouseHitTolerance);
            foreach (var result in this.HitTest(args))
            {
                e.HitTestResult = result;
                result.Element.OnMouseDown(e);
                if (e.Handled)
                {
                    this.currentMouseEventElement = result.Element;
                    return;
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
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="OxyPlot.OxyMouseEventArgs" /> instance containing the event data.</param>
        [Obsolete("Will be removed in v4.0 (#111)")]
        public virtual void HandleMouseMove(object sender, OxyMouseEventArgs e)
        {
            if (this.currentMouseEventElement != null)
            {
                this.currentMouseEventElement.OnMouseMove(e);
            }

            if (!e.Handled)
            {
                this.OnMouseMove(sender, e);
            }
        }

        /// <summary>
        /// Handles the mouse up event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="OxyPlot.OxyMouseEventArgs" /> instance containing the event data.</param>
        [Obsolete("Will be removed in v4.0 (#111)")]
        public virtual void HandleMouseUp(object sender, OxyMouseEventArgs e)
        {
            if (this.currentMouseEventElement != null)
            {
                this.currentMouseEventElement.OnMouseUp(e);
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
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="OxyPlot.OxyMouseEventArgs" /> instance containing the event data.</param>
        [Obsolete("Will be removed in v4.0 (#111)")]
        public virtual void HandleMouseEnter(object sender, OxyMouseEventArgs e)
        {
            if (!e.Handled)
            {
                this.OnMouseEnter(sender, e);
            }
        }

        /// <summary>
        /// Handles the mouse leave event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="OxyPlot.OxyMouseEventArgs" /> instance containing the event data.</param>
        [Obsolete("Will be removed in v4.0 (#111)")]
        public virtual void HandleMouseLeave(object sender, OxyMouseEventArgs e)
        {
            if (!e.Handled)
            {
                this.OnMouseLeave(sender, e);
            }
        }

        /// <summary>
        /// Handles the touch started event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">A <see cref="OxyPlot.OxyTouchEventArgs" /> instance containing the event data.</param>
        [Obsolete("Will be removed in v4.0 (#111)")]
        public virtual void HandleTouchStarted(object sender, OxyTouchEventArgs e)
        {
            var args = new HitTestArguments(e.Position, MouseHitTolerance);
            foreach (var result in this.HitTest(args))
            {
                result.Element.OnTouchStarted(e);
                if (e.Handled)
                {
                    this.currentTouchEventElement = result.Element;
                    return;
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
        /// <param name="sender">The sender.</param>
        /// <param name="e">A <see cref="OxyPlot.OxyTouchEventArgs" /> instance containing the event data.</param>
        [Obsolete("Will be removed in v4.0 (#111)")]
        public virtual void HandleTouchDelta(object sender, OxyTouchEventArgs e)
        {
            if (this.currentTouchEventElement != null)
            {
                this.currentTouchEventElement.OnTouchDelta(e);
            }

            if (!e.Handled)
            {
                this.OnTouchDelta(sender, e);
            }
        }

        /// <summary>
        /// Handles the touch completed event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">A <see cref="OxyPlot.OxyTouchEventArgs" /> instance containing the event data.</param>
        [Obsolete("Will be removed in v4.0 (#111)")]
        public virtual void HandleTouchCompleted(object sender, OxyTouchEventArgs e)
        {
            if (this.currentTouchEventElement != null)
            {
                this.currentTouchEventElement.OnTouchCompleted(e);
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
        /// <param name="e">The <see cref="OxyKeyEventArgs" /> instance containing the event data.</param>
        [Obsolete("Will be removed in v4.0 (#111)")]
        public virtual void HandleKeyDown(object sender, OxyKeyEventArgs e)
        {
            foreach (var element in this.GetHitTestElements())
            {
                element.OnKeyDown(e);

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
        /// Raises the <see cref="KeyDown" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        [Obsolete("Will be removed in v4.0 (#111)")]
        protected virtual void OnKeyDown(object sender, OxyKeyEventArgs e)
        {
            var handler = this.KeyDown;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseDown" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        [Obsolete("Will be removed in v4.0 (#111)")]
        protected virtual void OnMouseDown(object sender, OxyMouseDownEventArgs e)
        {
            var handler = this.MouseDown;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseMove" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        [Obsolete("Will be removed in v4.0 (#111)")]
        protected virtual void OnMouseMove(object sender, OxyMouseEventArgs e)
        {
            var handler = this.MouseMove;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseUp" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        [Obsolete("Will be removed in v4.0 (#111)")]
        protected virtual void OnMouseUp(object sender, OxyMouseEventArgs e)
        {
            var handler = this.MouseUp;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseEnter" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        [Obsolete("Will be removed in v4.0 (#111)")]
        protected virtual void OnMouseEnter(object sender, OxyMouseEventArgs e)
        {
            var handler = this.MouseEnter;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseLeave" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        [Obsolete("Will be removed in v4.0 (#111)")]
        protected virtual void OnMouseLeave(object sender, OxyMouseEventArgs e)
        {
            var handler = this.MouseLeave;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseDown" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        [Obsolete("Will be removed in v4.0 (#111)")]
        protected virtual void OnTouchStarted(object sender, OxyTouchEventArgs e)
        {
            var handler = this.TouchStarted;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseMove" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        [Obsolete("Will be removed in v4.0 (#111)")]
        protected virtual void OnTouchDelta(object sender, OxyTouchEventArgs e)
        {
            var handler = this.TouchDelta;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="MouseUp" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="OxyMouseEventArgs" /> instance containing the event data.</param>
        [Obsolete("Will be removed in v4.0 (#111)")]
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
