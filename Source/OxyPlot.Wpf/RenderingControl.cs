// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderingControl.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
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
