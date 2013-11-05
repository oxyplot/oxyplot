namespace OxyPlot.Wpf
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    public partial class Plot
    {
        /// <summary>
        /// The stack of manipulation events. This is used to try to avoid latency of the ManipulationDelta events.
        /// </summary>
        private readonly Stack<ManipulationDeltaEventArgs> manipulationQueue = new Stack<ManipulationDeltaEventArgs>();

        /// <summary>
        /// The last cumulative manipulation scale.
        /// </summary>
        private Vector lastManipulationScale;

        /// <summary>
        /// The touch down point.
        /// </summary>
        private Point touchDownPoint;

        /// <summary>
        /// The touch pan manipulator.
        /// </summary>
        private PanManipulator touchPan;

        /// <summary>
        /// The touch zoom manipulator.
        /// </summary>
        private ZoomManipulator touchZoom;

        /// <summary>
        /// Handles the CompositionTarget.Rendering event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="System.Windows.Media.RenderingEventArgs"/> instance containing the event data.</param>
        protected override void OnCompositionTargetRendering(object sender, RenderingEventArgs eventArgs)
        {
            // TODO: get rid of this?
            this.HandleStackedManipulationEvents();
        }

        /// <summary>
        /// Called when the <see cref="E:System.Windows.UIElement.ManipulationCompleted"/> event occurs.
        /// </summary>
        /// <param name="e">
        /// The data for the event.
        /// </param>
        protected override void OnManipulationCompleted(ManipulationCompletedEventArgs e)
        {
            base.OnManipulationCompleted(e);
            if (e.Handled)
            {
                return;
            }

            var position = this.touchDownPoint + e.TotalManipulation.Translation;
            this.touchPan.Completed(new ManipulationEventArgs(position.ToScreenPoint()));
            e.Handled = true;
        }

        /// <summary>
        /// Called when the <see cref="E:System.Windows.UIElement.ManipulationDelta"/> event occurs.
        /// </summary>
        /// <param name="e">
        /// The data for the event.
        /// </param>
        protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
        {
            base.OnManipulationDelta(e);
            if (e.Handled)
            {
                return;
            }

            lock (this.manipulationQueue)
            {
                this.manipulationQueue.Push(e);
            }

            // this was the original code, but it seems to add latency to the manipulations...
            // var position = this.touchDownPoint + e.CumulativeManipulation.Translation;
            // this.touchPan.Delta(new ManipulationEventArgs(position.ToScreenPoint()));

            // this.touchZoom.Delta(
            // new ManipulationEventArgs(position.ToScreenPoint())
            // {
            // ScaleX = e.DeltaManipulation.Scale.X, ScaleY = e.DeltaManipulation.Scale.Y
            // });
            e.Handled = true;
        }

        /// <summary>
        /// Called when the <see cref="E:System.Windows.UIElement.ManipulationStarted"/> event occurs.
        /// </summary>
        /// <param name="e">
        /// The data for the event.
        /// </param>
        protected override void OnManipulationStarted(ManipulationStartedEventArgs e)
        {
            base.OnManipulationStarted(e);
            if (e.Handled)
            {
                return;
            }

            this.touchPan = new PanManipulator(this);
            this.touchPan.Started(new ManipulationEventArgs(e.ManipulationOrigin.ToScreenPoint()));
            this.touchZoom = new ZoomManipulator(this);
            this.touchZoom.Started(new ManipulationEventArgs(e.ManipulationOrigin.ToScreenPoint()));
            this.touchDownPoint = e.ManipulationOrigin;
            this.lastManipulationScale = new Vector(1, 1);
            e.Handled = true;
        }

        /// <summary>
        /// Handles the stacked manipulation events.
        /// </summary>
        private void HandleStackedManipulationEvents()
        {
            ManipulationDeltaEventArgs e;
            lock (this.manipulationQueue)
            {
                if (this.manipulationQueue.Count == 0)
                {
                    return;
                }

                // Get the last manipulation event from the stack
                e = this.manipulationQueue.Pop();

                // Skip all older events
                this.manipulationQueue.Clear();
            }

            // Apply the last manipulation event to translation (pan) and scaling (zoom)
            var position = this.touchDownPoint + e.CumulativeManipulation.Translation;
            this.touchPan.Delta(new ManipulationEventArgs(position.ToScreenPoint()));

            double scaleX = e.CumulativeManipulation.Scale.X / this.lastManipulationScale.X;
            double scaleY = e.CumulativeManipulation.Scale.Y / this.lastManipulationScale.Y;
            this.touchZoom.Delta(
                new ManipulationEventArgs(position.ToScreenPoint()) { ScaleX = scaleX, ScaleY = scaleY });

            this.lastManipulationScale = e.CumulativeManipulation.Scale;
        }

        
    }
}