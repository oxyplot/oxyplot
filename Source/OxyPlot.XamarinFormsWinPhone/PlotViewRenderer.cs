using OxyPlot.XamarinForms;
using OxyPlot.XamarinFormsIOS;

using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;

// Exports the renderer.
[assembly: ExportRenderer(typeof(PlotView), typeof(PlotViewRenderer))]

namespace OxyPlot.XamarinFormsIOS
{
    using System.ComponentModel;

    using OxyPlot.WP8;

    /// <summary>
    /// Provides a custom <see cref="OxyPlot.XamarinForms.PlotView" /> renderer for Xamarin.iOS. 
    /// </summary>
    public class PlotViewRenderer : ViewRenderer<XamarinForms.PlotView, OxyPlot.WP8.PlotView>
    {
        /// <summary>
        /// Raises the element changed event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<XamarinForms.PlotView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || this.Element == null)
            {
                return;
            }

            var plotView = new PlotView
            {
                Model = this.Element.Model,
                Controller = this.Element.Controller
            };

            if (this.Element.Model.Background.IsVisible())
            {
                plotView.Background = this.Element.Model.Background.ToBrush();
            }

            this.SetNativeControl(plotView);
        }

        /// <summary>
        /// Raises the element property changed event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (this.Element == null || this.Control == null)
            {
                return;
            }

            if (e.PropertyName == XamarinForms.PlotView.ModelProperty.PropertyName)
            {
                this.Control.Model = Element.Model;
            }

            if (e.PropertyName == XamarinForms.PlotView.ControllerProperty.PropertyName)
            {
                this.Control.Controller = Element.Controller;
            }
        }
    }
}
