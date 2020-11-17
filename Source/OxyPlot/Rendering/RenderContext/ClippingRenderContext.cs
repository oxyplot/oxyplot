// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClippingRenderContext.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides an abstract base class for rendering contexts that implements a clipping stack.
    /// </summary>
    public abstract class ClippingRenderContext : RenderContextBase
    {
        private readonly Stack<OxyRect> clipStack = new Stack<OxyRect>();

        /// <inheritdoc/>
        public sealed override void PopClip()
        {
            if (this.clipStack.Count == 0)
            {
                throw new InvalidOperationException($"Unbalanced call to {nameof(PopClip)}.");
            }

            var currentClippingRectangle = this.clipStack.Pop();
            if (this.clipStack.Count > 0)
            {
                var newClippingRectangle = this.clipStack.Peek();
                if (!newClippingRectangle.Equals(currentClippingRectangle))
                {
                    this.ResetClip();
                    this.SetClip(newClippingRectangle);
                }
            }
            else
            {
                this.ResetClip();
            }
        }

        /// <inheritdoc/>
        public sealed override void PushClip(OxyRect clippingRectangle)
        {
            if (this.clipStack.Count > 0)
            {
                var currentClippingRectangle = this.clipStack.Peek();
                var newClippingRectangle = clippingRectangle.Intersect(currentClippingRectangle);
                if (!currentClippingRectangle.Equals(newClippingRectangle))
                {
                    this.ResetClip();
                    this.SetClip(newClippingRectangle);
                }

                this.clipStack.Push(newClippingRectangle);
            }
            else
            {
                this.SetClip(clippingRectangle);
                this.clipStack.Push(clippingRectangle);
            }
        }

        /// <inheritdoc/>
        public sealed override int ClipCount => this.clipStack.Count;

        /// <summary>
        /// Resets the clipping area.
        /// </summary>
        protected abstract void ResetClip();

        /// <summary>
        /// Sets the clipping area to the specified rectangle.
        /// </summary>
        /// <param name="clippingRectangle">The rectangle.</param>
        /// <remarks>
        /// Calls to this method must always be balanced by a call to <see cref="ResetClip"/> before calling <see cref="SetClip(OxyRect)"/> again.
        /// </remarks>
        protected abstract void SetClip(OxyRect clippingRectangle);
    }
}
