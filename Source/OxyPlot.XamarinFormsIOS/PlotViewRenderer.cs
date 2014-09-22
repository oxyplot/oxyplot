namespace OxyPlot.XamarinFormsIOS
{
    using System.ComponentModel;

    using Xamarin.Forms.Platform.iOS;

    using OxyPlot.XamarinForms;

    public class PlotViewRenderer : ViewRenderer<PlotView, OxyPlot.XamarinIOS.PlotView>
    {
        public PlotViewRenderer()
        {
        }

        protected override void OnElementChanged (ElementChangedEventArgs<PlotView> e){
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

            if (e.PropertyName == PlotView.ModelProperty.PropertyName) {
                Control.Model = Element.Model;
            } 
       }
    }
}
