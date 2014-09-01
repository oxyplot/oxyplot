// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrottledMouseEvent.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Creates a 'throttled' MouseMove event which ensures that the UI
//   rendering is not starved.
//   see: http://www.scottlogic.co.uk/blog/colin/2010/06/throttling-silverlight-mouse-events-to-keep-the-ui-responsive/
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HistogramDemo
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Creates a 'throttled' MouseMove event which ensures that the UI
    /// rendering is not starved.
    /// see: http://www.scottlogic.co.uk/blog/colin/2010/06/throttling-silverlight-mouse-events-to-keep-the-ui-responsive/
    /// </summary>
    public class ThrottledMouseMoveEvent
    {
        private bool awaitingRender;

        private readonly UIElement element;

        public ThrottledMouseMoveEvent(UIElement element)
        {
            this.element = element;
            element.MouseMove += this.ElementMouseMove;
        }

        public event MouseEventHandler ThrottledMouseMove;

        private void ElementMouseMove(object sender, MouseEventArgs e)
        {
            if (!this.awaitingRender)
            {
                // if we are not awaiting a render as a result of a previously handled event
                // raise a ThrottledMouseMove event, and add a Rendering handler so that
                // we can determine when this event has been acted upon.
                this.OnThrottledMouseMove(e);
                this.awaitingRender = true;
                CompositionTarget.Rendering += this.CompositionTargetRendering;
            }
        }

        private void CompositionTargetRendering(object sender, EventArgs e)
        {
            this.awaitingRender = false;
            CompositionTarget.Rendering -= this.CompositionTargetRendering;
        }

        /// <summary>
        /// Raises the ThrottledMouseMove event
        /// </summary>
        protected void OnThrottledMouseMove(MouseEventArgs args)
        {
            if (this.ThrottledMouseMove != null)
            {
                this.ThrottledMouseMove(this.element, args);
            }
        }
    }
}