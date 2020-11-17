// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SvgExporter.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.SkiaSharp
{
    using global::SkiaSharp;
    using System.IO;

    /// <summary>
    /// Provides functionality to export plots to scalable vector graphics using the SkiaSharp SVG canvas.
    /// </summary>
    public class SvgExporter : IExporter
    {
        /// <summary>
        /// Gets or sets the height (in user units) of the output area.
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// Gets or sets the width (in user units) of the output area.
        /// </summary>
        public float Width { get; set; }

        /// <inheritdoc/>
        public void Export(IPlotModel model, Stream stream)
        {
            using var skStream = new SKManagedWStream(stream);
            using var writer = new SKXmlStreamWriter(skStream);
            using var canvas = SKSvgCanvas.Create(new SKRect(0, 0, this.Width, this.Height), writer);

            if (!model.Background.IsInvisible())
            {
                canvas.Clear(model.Background.ToSKColor());
            }

            // SVG export does not work with UseTextShaping=true. However SVG does text shaping by itself anyway, so we can just disable it
            using var context = new SkiaRenderContext { RenderTarget = RenderTarget.VectorGraphic, SkCanvas = canvas, UseTextShaping = false };
            model.Update(true);
            model.Render(context, new OxyRect(0, 0, this.Width, this.Height));
        }
    }
}
