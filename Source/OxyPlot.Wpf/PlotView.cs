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
    /// Represents a control that displays a <see cref="PlotModel" />. This <see cref="IPlotView"/> is based on <see cref="CanvasRenderContext"/>.
    /// </summary>
    public partial class PlotView : PlotViewBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlotView" /> class.
        /// </summary>
        public PlotView()
        {
            this.DisconnectCanvasWhileUpdating = true;
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, this.DoCopy));
        }

        /// <summary>
        /// Gets or sets a value indicating whether to disconnect the canvas while updating.
        /// </summary>
        /// <value><c>true</c> if canvas should be disconnected while updating; otherwise, <c>false</c>.</value>
        public bool DisconnectCanvasWhileUpdating { get; set; }

        /// <summary>
        /// Gets the Canvas.
        /// </summary>
        private Canvas Canvas => (Canvas)this.plotPresenter;

        /// <summary>
        /// Gets the CanvasRenderContext.
        /// </summary>
        private CanvasRenderContext RenderContext => (CanvasRenderContext)this.renderContext;

        /// <inheritdoc/>
        protected override void ClearBackground()
        {
            this.Canvas.Children.Clear();

            if (this.ActualModel != null && this.ActualModel.Background.IsVisible())
            {
                this.Canvas.Background = this.ActualModel.Background.ToBrush();
            }
            else
            {
                this.Canvas.Background = null;
            }
        }

        /// <inheritdoc/>
        protected override FrameworkElement CreatePlotPresenter()
        {
            return new Canvas();
        }

        /// <inheritdoc/>
        protected override IRenderContext CreateRenderContext()
        {
            return new CanvasRenderContext(this.Canvas);
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
            if (this.DisconnectCanvasWhileUpdating)
            {
                // TODO: profile... not sure if this makes any difference
                var idx = this.grid.Children.IndexOf(this.plotPresenter);
                if (idx != -1)
                {
                    this.grid.Children.RemoveAt(idx);
                }

                base.RenderOverride();

                if (idx != -1)
                {
                    // reinsert the canvas again
                    this.grid.Children.Insert(idx, this.plotPresenter);
                }
            }
            else
            {
                base.RenderOverride();
            }
        }

        /// <inheritdoc/>
        protected override double UpdateDpi()
        {
            var scale = base.UpdateDpi();
            this.RenderContext.DpiScale = scale;
            this.RenderContext.VisualOffset = this.TransformToAncestor(Window.GetWindow(this)).Transform(default);
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
    }
}
