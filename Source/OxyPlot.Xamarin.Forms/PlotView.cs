// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotView.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a visual element that displays a PlotModel.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Xamarin.Forms
{
    using OxyPlot;

    using global::Xamarin.Forms;

    /// <summary>
    /// Represents a visual element that displays a <see cref="PlotModel" />.
    /// </summary>
    public class PlotView : View
    {
        /// <summary>
        /// Identifies the <see cref="Controller" />  bindable property.
        /// </summary>
        public static readonly BindableProperty ControllerProperty = BindableProperty.Create<PlotView, PlotController>(p => p.Controller, null);

        /// <summary>
        /// Identifies the <see cref="Model" />  bindable property.
        /// </summary>
        public static readonly BindableProperty ModelProperty = BindableProperty.Create<PlotView, PlotModel>(p => p.Model, null);

        /// <summary>
        /// Gets or sets the <see cref="PlotModel"/> to view.
        /// </summary>
        /// <value>The model.</value>
        public PlotModel Model
        {
            get { return (PlotModel)this.GetValue(ModelProperty); }
            set { this.SetValue(ModelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="PlotController"/> for the view.
        /// </summary>
        /// <value>The controller.</value>
        public PlotController Controller
        {
            get { return (PlotController)this.GetValue(ControllerProperty); }
            set { this.SetValue(ControllerProperty, value); }
        }
    }
}
