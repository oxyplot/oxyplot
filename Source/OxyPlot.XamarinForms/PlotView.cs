namespace OxyPlot.XamarinForms
{
    using Xamarin.Forms;

    using OxyPlot;

    /// <summary>
    /// Represents a visual element that displays a <see cref="OxyPlot.PlotModel" /> .
    /// </summary>
    public class PlotView : View
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OxyPlot.XamarinForms.PlotView"/> class.
        /// </summary>
        public PlotView ()
        {
        }

        /// <summary>
        /// The model bindable property.
        /// </summary>
        public static readonly BindableProperty ModelProperty = BindableProperty.Create<PlotView,PlotModel> (p => p.Model, null);

        /// <summary>
        /// Gets or sets the model to view.
        /// </summary>
        /// <value>The model.</value>
        public PlotModel Model {
            get { return (PlotModel)this.GetValue (ModelProperty); }
            set { this.SetValue (ModelProperty, value); }
        }
    }
}
