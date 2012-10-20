// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderingControl.cs" company="OxyPlot">
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
//   Provides an abstract base class for Control objects that listens to the CompositionTarget.Rendering event.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Wpf
{
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Provides an abstract base class for Control objects that listens to the CompositionTarget.Rendering event.
    /// </summary>
    public abstract class RenderingControl : Control
    {
        /// <summary>
        /// The renderingEventListener
        /// </summary>
        private readonly RenderingEventListener renderingEventListener;

        /// <summary>
         /// Initializes a new instance of the <see cref="RenderingControl"/> class.
        /// </summary>
        protected RenderingControl()
        {
            this.renderingEventListener = new RenderingEventListener(this.OnCompositionTargetRendering);
        }

        /// <summary>
        /// Subscribes to CompositionTarget.Rendering event.
        /// </summary>
        protected void SubscribeToRenderingEvent()
        {
            RenderingEventManager.AddListener(this.renderingEventListener);
        }

        /// <summary>
        /// Unsubscribes the CompositionTarget.Rendering event.
        /// </summary>
        protected void UnsubscribeRenderingEvent()
        {
            RenderingEventManager.RemoveListener(this.renderingEventListener);
        }

        /// <summary>
        /// Handles the CompositionTarget.Rendering event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="System.Windows.Media.RenderingEventArgs"/> instance containing the event data.</param>
        protected abstract void OnCompositionTargetRendering(object sender, RenderingEventArgs eventArgs);
    }
}