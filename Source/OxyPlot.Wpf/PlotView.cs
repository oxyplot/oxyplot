// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotView.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a control that displays a <see cref="PlotModel" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Represents a control that displays a <see cref="PlotModel" />.
    /// </summary>
    [TemplatePart(Name = PartGrid, Type = typeof(Grid))]
    public class PlotView : PlotBase
    {
        /// <summary>
        /// Identifies the <see cref="Controller"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ControllerProperty =
            DependencyProperty.Register("Controller", typeof(IPlotController), typeof(PlotView));

        /// <summary>
        /// Identifies the <see cref="Model"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register("Model", typeof(PlotModel), typeof(PlotView), new PropertyMetadata(null, ModelChanged));

        /// <summary>
        /// The model lock.
        /// </summary>
        private readonly object modelLock = new object();

        /// <summary>
        /// The current model (synchronized with the <see cref="Model" /> property, but can be accessed from all threads.
        /// </summary>
        private PlotModel currentModel;

        /// <summary>
        /// The default plot controller.
        /// </summary>
        private IPlotController defaultController;

        /// <summary>
        /// Initializes static members of the <see cref="PlotView" /> class.
        /// </summary>
        static PlotView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PlotView), new FrameworkPropertyMetadata(typeof(PlotView)));
            PaddingProperty.OverrideMetadata(typeof(PlotView), new FrameworkPropertyMetadata(new Thickness(8), AppearanceChanged));
        }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>The model.</value>
        public PlotModel Model
        {
            get
            {
                return (PlotModel)this.GetValue(ModelProperty);
            }

            set
            {
                this.SetValue(ModelProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the Plot controller.
        /// </summary>
        /// <value>The Plot controller.</value>
        public IPlotController Controller
        {
            get
            {
                return (IPlotController)this.GetValue(ControllerProperty);
            }

            set
            {
                this.SetValue(ControllerProperty, value);
            }
        }

        /// <summary>
        /// Gets the actual model.
        /// </summary>
        /// <value>The actual model.</value>
        public override PlotModel ActualModel
        {
            get
            {
                return this.currentModel;
            }
        }

        /// <summary>
        /// Gets the actual PlotView controller.
        /// </summary>
        /// <value>The actual PlotView controller.</value>
        public override IPlotController ActualController
        {
            get
            {
                return this.Controller ?? (this.defaultController ?? (this.defaultController = new PlotController()));
            }
        }

        /// <summary>
        /// Called when the visual appearance is changed.
        /// </summary>
        protected void OnAppearanceChanged()
        {
            this.InvalidatePlot(false);
        }

        /// <summary>
        /// Called when the visual appearance is changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void AppearanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PlotView)d).OnAppearanceChanged();
        }

        /// <summary>
        /// Called when the model is changed.
        /// </summary>
        /// <param name="d">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void ModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PlotView)d).OnModelChanged();
        }

        /// <summary>
        /// Called when the model is changed.
        /// </summary>
        private void OnModelChanged()
        {
            lock (this.modelLock)
            {
                if (this.currentModel != null)
                {
                    ((IPlotModel)this.currentModel).AttachPlotView(null);
                    this.currentModel = null;
                }

                if (this.Model != null)
                {
                    ((IPlotModel)this.Model).AttachPlotView(this);
                    this.currentModel = this.Model;
                }
            }

            this.InvalidatePlot();
        }
    }
}
