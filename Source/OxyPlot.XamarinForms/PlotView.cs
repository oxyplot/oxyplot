namespace OxyPlot.XamarinForms
{
    using Xamarin.Forms;

    using OxyPlot;

    /// <summary>
    /// Represents a visual element that displays a <see cref="PlotModel" /> .
    /// </summary>
    public class PlotView : View
    {
        /// <summary>
        /// Identifies the <see cref="Model" />  bindable property.
        /// </summary>
        public static readonly BindableProperty ModelProperty = BindableProperty.Create<PlotView,PlotModel> (p => p.Model, null);

        /// <summary>
        /// Gets or sets the <see cref="PlotModel"/> to view.
        /// </summary>
        /// <value>The model.</value>
        public PlotModel Model {
            get { return (PlotModel)this.GetValue (ModelProperty); }
            set { this.SetValue (ModelProperty, value); }
        }
    }
}
