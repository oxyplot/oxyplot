using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using OxyPlot.XamarinForms;
using OxyPlot.XamarinFormsAndroid;

// Exports the renderer.
[assembly: ExportRenderer(typeof(PlotView), typeof(PlotViewRenderer))]

namespace OxyPlot.XamarinFormsAndroid
{
    using System.ComponentModel;

    /// <summary>
    /// Provides a custom <see cref="OxyPlot.XamarinForms.PlotView" /> renderer for Xamarin.Android. 
    /// </summary>
    public class PlotViewRenderer : ViewRenderer<PlotView, OxyPlot.XamarinAndroid.PlotView>
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

            var plotView = new OxyPlot.XamarinAndroid.PlotView(this.Context) {
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
