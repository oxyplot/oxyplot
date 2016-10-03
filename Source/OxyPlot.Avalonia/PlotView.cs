// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotView.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a control that displays a <see cref="PlotModel" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{

    /// <summary>
    /// Represents a control that displays a <see cref="PlotModel" />.
    /// </summary>
    public class PlotView : PlotBase
    {
        /// <summary>
        /// Identifies the <see cref="Controller"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<IPlotController> ControllerProperty = AvaloniaProperty.Register<PlotView, IPlotController>(nameof(Controller));

        /// <summary>
        /// Identifies the <see cref="Model"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<PlotModel> ModelProperty = AvaloniaProperty.Register<PlotView, PlotModel>(nameof(Model), null);

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
            PaddingProperty.OverrideMetadata(typeof(PlotView), new StyledPropertyMetadata<Thickness>(new Thickness(8)));
            ModelProperty.Changed.AddClassHandler<PlotView>(ModelChanged);
            PaddingProperty.Changed.AddClassHandler<PlotView>(AppearanceChanged);
        }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>The model.</value>
        public PlotModel Model
        {
            get
            {
                return GetValue(ModelProperty);
            }

            set
            {
                SetValue(ModelProperty, value);
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
                return GetValue(ControllerProperty);
            }

            set
            {
                SetValue(ControllerProperty, value);
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
                return currentModel;
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
                return Controller ?? (defaultController ?? (defaultController = new PlotController()));
            }
        }

        /// <summary>
        /// Called when the visual appearance is changed.
        /// </summary>
        protected void OnAppearanceChanged()
        {
            InvalidatePlot(false);
        }

        /// <summary>
        /// Called when the visual appearance is changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="AvaloniaPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void AppearanceChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e)
        {
            ((PlotView)d).OnAppearanceChanged();
        }

        /// <summary>
        /// Called when the model is changed.
        /// </summary>
        /// <param name="d">The sender.</param>
        /// <param name="e">The <see cref="AvaloniaPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void ModelChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e)
        {
            ((PlotView)d).OnModelChanged();
        }

        /// <summary>
        /// Called when the model is changed.
        /// </summary>
        private void OnModelChanged()
        {
            lock (modelLock)
            {
                if (currentModel != null)
                {
                    ((IPlotModel)currentModel).AttachPlotView(null);
                    currentModel = null;
                }

                if (Model != null)
                {
                    ((IPlotModel)Model).AttachPlotView(this);
                    currentModel = Model;
                }
            }

            InvalidatePlot();
        }
    }
}
