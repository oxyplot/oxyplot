using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using OxyPlot.XamarinForms;
using OxyPlot.XamarinFormsIOS;

// Exports the renderer.
[assembly: ExportRenderer(typeof(PlotView), typeof(PlotViewRenderer))]

namespace OxyPlot.XamarinFormsIOS
{
    using System.ComponentModel;

    /// <summary>
    /// Provides a custom <see cref="OxyPlot.XamarinForms.PlotView" /> renderer for Xamarin.iOS. 
    /// </summary>
    public class PlotViewRenderer : ViewRenderer<PlotView, OxyPlot.XamarinIOS.PlotView>
    {
        /// <summary>
        /// Raises the element changed event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnElementChanged (ElementChangedEventArgs<PlotView> e){
            base.OnElementChanged (e);
            if (e.OldElement != null || this.Element == null) {
                return;
            }

            var plotView = new OxyPlot.XamarinIOS.PlotView {
                Model = this.Element.Model
            };

            this.SetNativeControl (plotView);
        }

        /// <summary>
        /// Raises the element property changed event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        protected override void OnElementPropertyChanged (object sender, PropertyChangedEventArgs e) {
            base.OnElementPropertyChanged (sender, e);
            if (this.Element == null || this.Control == null) {
                return;
            }

            if (e.PropertyName == PlotView.ModelProperty.PropertyName) {
                this.Control.Model = Element.Model;
            } 
       }
    }
}
