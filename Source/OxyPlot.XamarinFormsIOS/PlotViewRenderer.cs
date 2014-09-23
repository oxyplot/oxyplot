[assembly: Xamarin.Forms.ExportRenderer(typeof(OxyPlot.XamarinForms.PlotView), typeof(OxyPlot.XamarinFormsIOS.PlotViewRenderer))]

namespace OxyPlot.XamarinFormsIOS
{
    using System.ComponentModel;

    using Xamarin.Forms.Platform.iOS;

    public class PlotViewRenderer : ViewRenderer<OxyPlot.XamarinForms.PlotView, OxyPlot.XamarinIOS.PlotView>
    {
        public PlotViewRenderer()
        {
        }

        protected override void OnElementChanged (ElementChangedEventArgs<OxyPlot.XamarinForms.PlotView> e){
            base.OnElementChanged (e);
            if (e.OldElement != null || this.Element == null) {
                return;
            }

            var plotView = new OxyPlot.XamarinIOS.PlotView() {
                Model = this.Element.Model
            };

            SetNativeControl (plotView);
        }

        protected override void OnElementPropertyChanged (object sender, PropertyChangedEventArgs e) {
            base.OnElementPropertyChanged (sender, e);
            if (this.Element == null || this.Control == null) {
                return;
            }

            if (e.PropertyName == OxyPlot.XamarinForms.PlotView.ModelProperty.PropertyName) {
                Control.Model = Element.Model;
            } 
       }
    }
}
