// --------------------------------------------------------------------------------------------------------------------
// <copyright filename="PngRenderingContext.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.ImageSharp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using SixLabors.Fonts;
    using SixLabors.Fonts.Exceptions;
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.PixelFormats;
    using SixLabors.ImageSharp.Processing;
    using SixLabors.Primitives;

    /// <summary>
    /// Implements the rendering context on ImageSharp.
    /// </summary>
    public class PngRenderingContext : RenderContextBase, IDisposable
    {
        static readonly string FallbackFontFamily = "Arial";
        private readonly Image<Rgba32> image;
        private bool disposedValue = false; // To detect redundant calls to dispose

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="background"></param>
        /// <param name="dpi"></param>
        public PngRenderingContext(int width, int height, OxyColor background, double dpi = 72)
        {
            this.image = new Image<Rgba32>(width, height);
            image.Metadata.HorizontalResolution = dpi;
            image.Metadata.VerticalResolution = dpi;

            image.Mutate(img => img.BackgroundColor(
                new Rgba32(background.R, background.G, background.B, background.A)));

            RendersToScreen = false;
        }

        /// <summary>
        /// Saves the image to the specified stream.
        /// </summary>
        /// <param name="output"></param>
        public void Save(Stream output)
        {
            image.SaveAsPng(output);
        }

        /// <inheritdoc/>
        public override void DrawText(ScreenPoint p, string text, OxyColor fill, string fontFamily = null, double fontSize = 10, double fontWeight = 400, double rotation = 0, OxyPlot.HorizontalAlignment horizontalAlignment = OxyPlot.HorizontalAlignment.Left, OxyPlot.VerticalAlignment verticalAlignment = OxyPlot.VerticalAlignment.Top, OxySize? maxSize = null)
        {
            var font = GetFontOrThrow(fontFamily, fontSize, FontStyle.Regular);

            float outputX = (float)p.X;
            float outputY = (float)p.Y;

            var bounds = MeasureText(text, fontFamily, fontSize, fontWeight);

            switch (horizontalAlignment)
            {
                case OxyPlot.HorizontalAlignment.Center:
                    outputX = (float)(outputX - bounds.Width / 2.0);
                    break;
                case OxyPlot.HorizontalAlignment.Right:
                    outputX = (float)(outputX - bounds.Width);
                    break;
            }

            switch (verticalAlignment)
            {
                case OxyPlot.VerticalAlignment.Bottom:
                    outputY = (float)(outputY - bounds.Height);
                    break;
                case OxyPlot.VerticalAlignment.Middle:
                    outputY = (float)(outputY - bounds.Height / 2.0);
                    break;
            }

            var outputPosition = new PointF(outputX, outputY);

            image.Mutate(img =>
            {
                img.DrawText(text, font, new Rgba32(fill.R, fill.G, fill.B, fill.A), outputPosition);
            });
        }

        /// <inheritdoc/>
        public override OxySize MeasureText(string text, string fontFamily = null, double fontSize = 10, double fontWeight = 500)
        {
            var font = GetFontOrThrow(fontFamily, fontSize, FontStyle.Regular);
            text = text ?? string.Empty;

            /*
                We use DPI 72 for measurements on purpose. See: 
                    https://github.com/SixLabors/ImageSharp/issues/421
                When this issue is resolved in ImageSharp, we should
                pass the correct dpi here.
            */
            var dpiOverride = 72;
            var result = TextMeasurer.Measure(text, new RendererOptions(font, dpiOverride));
            return new OxySize(result.Width, result.Height);
        }

        /// <inheritdoc/>
        public override void DrawLine(IList<ScreenPoint> points, OxyColor stroke, double thickness, double[] dashArray, LineJoin lineJoin, bool aliased)
        {
            image.Mutate(img =>
            {
                var brush = Brushes.Solid(new Rgba32(stroke.R, stroke.G, stroke.B, stroke.A));
                PointF[] mappedPoints = points.Select(p => new PointF((float)p.X, (float)p.Y)).ToArray();
                var options = new GraphicsOptions(true);

                img.DrawLines(options, brush, (float)thickness, mappedPoints);
            });
        }

        /// <inheritdoc/>
        public override void DrawPolygon(IList<ScreenPoint> points, OxyColor fill, OxyColor stroke, double thickness, double[] dashArray, LineJoin lineJoin, bool aliased)
        {
            image.Mutate(img =>
            {
                var mappedPoints = points.Select(point => new PointF((float)point.X, (float)point.Y)).ToArray();

                var brush = Brushes.Solid(new Rgba32(fill.R, fill.G, fill.B, fill.A));
                img.FillPolygon(brush, mappedPoints);

                var pen = Pens.Solid(new Rgba32(stroke.R, stroke.G, stroke.B, stroke.A), (float)thickness);
                img.DrawPolygon(pen, mappedPoints);
            });
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.image.Dispose();
                }

                disposedValue = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
        }

        Font GetFontOrThrow(string fontFamily, double fontSize, FontStyle regular, bool allowFallback = true)
        {
            var family = GetFamilyOrFallbackOrThrow(fontFamily, allowFallback);
            return new Font(family, (float)fontSize, FontStyle.Regular);
        }

        FontFamily GetFamilyOrFallbackOrThrow(string fontFamily = null, bool allowFallback = true)
        {
            if (fontFamily == null)
            {
                allowFallback = false;
                fontFamily = FallbackFontFamily;
            }

            FontFamily family;
            try
            {
                family = SixLabors.Fonts.SystemFonts.Find(fontFamily);
            }
            catch (FontFamilyNotFoundException primaryEx)
            {
                if (!allowFallback)
                {
                    throw;
                }

                try
                {
                    family = SixLabors.Fonts.SystemFonts.Find(FallbackFontFamily);
                }
                catch (FontFamilyNotFoundException fallbackEx)
                {
                    throw new AggregateException(primaryEx, fallbackEx);
                }
            }
            return family;
        }
    }
}
