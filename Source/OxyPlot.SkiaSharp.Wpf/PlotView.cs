// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotView.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.SkiaSharp.Wpf
{
    using global::SkiaSharp;
    using global::SkiaSharp.Views.Desktop;
    using OxyPlot;
    using OxyPlot.SkiaSharp;
    using OxyPlot.Wpf;
    using System.Windows;

    /// <summary>
    /// Represents a control that displays a <see cref="PlotModel" />. This <see cref="IPlotView"/> is based on <see cref="SkiaSharp.SkiaRenderContext"/>.
    /// </summary>
    public partial class PlotView : PlotViewBase
    {
        /// <summary>
        /// Gets the SkiaRenderContext.
        /// </summary>
        private SkiaRenderContext SkiaRenderContext => (SkiaRenderContext)this.renderContext;

        /// <summary>
        /// Gets the OxySKElement.
        /// </summary>
        private OxySKElement OxySKElement => (OxySKElement)this.plotPresenter;

        /// <inheritdoc/>
        protected override void ClearBackground()
        {
            var color = this.ActualModel?.Background.IsVisible() == true
                ? this.ActualModel.Background.ToSKColor()
                : SKColors.Empty;

            this.SkiaRenderContext.SkCanvas.Clear(color);
        }

        /// <inheritdoc/>
        protected override FrameworkElement CreatePlotPresenter()
        {
            var skElement = new OxySKElement();
            skElement.PaintSurface += this.SkElement_PaintSurface;
            return skElement;
        }

        /// <inheritdoc/>
        protected override IRenderContext CreateRenderContext()
        {
            return new SkiaRenderContext();
        }

        /// <inheritdoc/>
        protected override void RenderOverride()
        {
            // Instead of rendering directly, invalidate the plot presenter.
            // Actual rendering is done in SkElement_PaintSurface.
            this.plotPresenter.InvalidateVisual();
        }

        /// <inheritdoc/>
        protected override double UpdateDpi()
        {
            var dpiScale = base.UpdateDpi();
            var renderScale = this.OxySKElement.GetRenderScale();
            var skiaScale = (float)(dpiScale * renderScale);

            this.SkiaRenderContext.DpiScale = skiaScale;

            return dpiScale;
        }

        /// <summary>
        /// This is called when the SKElement paints its surface.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The surface paint event args.</param>
        private void SkElement_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            if (this.plotPresenter == null || this.renderContext == null)
            {
                return;
            }
            this.SkiaRenderContext.SkCanvas = e.Surface.Canvas;
            base.RenderOverride();
            this.SkiaRenderContext.SkCanvas = null;
        }
    }
}
