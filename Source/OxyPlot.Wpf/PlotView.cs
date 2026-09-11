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
    using OxyPlot;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Represents a control that displays a <see cref="PlotModel" />. This <see cref="IPlotView"/> is based on <see cref="DrawingRenderContext"/>.
    /// </summary>
    public partial class PlotView : PlotViewBase
    {
        /// <summary>
        /// Identifies the <see cref="TextMeasurementMethod"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextMeasurementMethodProperty =
            DependencyProperty.Register(
                nameof(TextMeasurementMethod), typeof(TextMeasurementMethod), typeof(PlotViewBase), new PropertyMetadata(TextMeasurementMethod.TextBlock));

        /// <summary>
        /// Initializes a new instance of the <see cref="PlotView" /> class.
        /// </summary>
        public PlotView()
        {
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, this.DoCopy));
        }

        /// <summary>
        /// Gets or sets a value indicating whether to disconnect the canvas while updating.
        /// </summary>
        /// <value><c>true</c> if canvas should be disconnected while updating; otherwise, <c>false</c>.</value>
        public bool DisconnectCanvasWhileUpdating { get; set; }

        /// <summary>
        /// Gets or sets the text measurement method.
        /// </summary>
        /// <value>The text measurement method.</value>
        public TextMeasurementMethod TextMeasurementMethod
        {
            get => (TextMeasurementMethod)this.GetValue(TextMeasurementMethodProperty);
            set => this.SetValue(TextMeasurementMethodProperty, value);
        }

        /// <summary>
        /// Gets the Canvas.
        /// </summary>
        protected Panel RenderSurface => (Panel)this.plotPresenter;

        /// <summary>
        /// Gets the WpfRenderContext.
        /// </summary>
        private WpfRenderContext RenderContext => this.renderContext as WpfRenderContext;

        /// <inheritdoc/>
        protected override void ClearBackground()
        {
            this.RenderSurface.Children.Clear();

            if (this.ActualModel != null && this.ActualModel.Background.IsVisible())
            {
                this.RenderSurface.Background = this.ActualModel.Background.ToBrush();
            }
            else
            {
                this.RenderSurface.Background = Brushes.Transparent;
            }
        }

        /// <inheritdoc/>
        protected override FrameworkElement CreatePlotPresenter()
        {
            return new RenderSurface() { Background = Brushes.Transparent };
        }

        /// <inheritdoc/>
        protected override IRenderContext CreateRenderContext()
        {
            return new DrawingRenderContext((RenderSurface)this.RenderSurface);
        }

        /// <inheritdoc/>
        protected override void OnRender(DrawingContext drawingContext)
        {
            this.Render();
            base.OnRender(drawingContext);
        }

        /// <inheritdoc/>
        protected override void RenderOverride()
        {
            this.RenderContext.TextMeasurementMethod = this.TextMeasurementMethod;
            var renderSurface = this.RenderSurface as RenderSurface;

            try
            {
                renderSurface?.BeginRender();

                base.RenderOverride();
            }
            finally
            {
                renderSurface?.EndRender();
            }
        }

        /// <inheritdoc/>
        protected override double UpdateDpi()
        {
            var scale = base.UpdateDpi();
            if (this.RenderContext != null)
            {
                this.RenderContext.DpiScale = scale;
                var ancestor = this.GetAncestorVisualFromVisualTree(this);
                this.RenderContext.VisualOffset = ancestor != null ? this.TransformToAncestor(ancestor).Transform(default) : default;
            }

            return scale;
        }

        /// <summary>
        /// Performs the copy operation.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Input.ExecutedRoutedEventArgs" /> instance containing the event data.</param>
        private void DoCopy(object sender, ExecutedRoutedEventArgs e)
        {
            var exporter = new PngExporter() { Width = (int)this.ActualWidth, Height = (int)this.ActualHeight };
            var bitmap = exporter.ExportToBitmap(this.ActualModel);
            Clipboard.SetImage(bitmap);
        }


        /// <summary>
        /// Returns a reference to the visual object that hosts the dependency object in the visual tree.
        /// </summary>
        /// <returns> The host window from the visual tree.</returns>
        private Visual GetAncestorVisualFromVisualTree(DependencyObject startElement)
        {

            DependencyObject child = startElement;
            DependencyObject parent = VisualTreeHelper.GetParent(child);
            while (parent != null)
            {
                child = parent;
                parent = VisualTreeHelper.GetParent(child);
            }

            return child as Visual ?? Window.GetWindow(this);
        }
    }
}
