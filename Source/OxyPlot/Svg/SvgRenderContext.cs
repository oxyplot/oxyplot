// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SvgRenderContext.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a render context for scalable vector graphics output.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Provides a render context for scalable vector graphics output.
    /// </summary>
    public class SvgRenderContext : RenderContextBase, IDisposable
    {
        /// <summary>
        /// The writer.
        /// </summary>
        private readonly SvgWriter w;

        /// <summary>
        /// The disposed flag.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// The current clipping rectangle.
        /// </summary>
        private OxyRect? clipRect;

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgRenderContext" /> class.
        /// </summary>
        /// <param name="s">The stream.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="isDocument">Create an SVG document if set to <c>true</c>.</param>
        /// <param name="textMeasurer">The text measurer.</param>
        /// <param name="background">The background.</param>
        /// <param name="useVerticalTextAlignmentWorkaround">Whether to use the workaround for vertical text alignment.</param>
        public SvgRenderContext(Stream s, double width, double height, bool isDocument, IRenderContext textMeasurer, OxyColor background, bool useVerticalTextAlignmentWorkaround = false)
        {
            if (textMeasurer == null)
            {
                throw new ArgumentNullException("textMeasurer", "A text measuring render context must be provided.");
            }

            this.w = new SvgWriter(s, width, height, isDocument);
            this.TextMeasurer = textMeasurer;
            this.UseVerticalTextAlignmentWorkaround = useVerticalTextAlignmentWorkaround;

            if (background.IsVisible())
            {
                this.w.WriteRectangle(0, 0, width, height, this.w.CreateStyle(background, OxyColors.Undefined, 0), EdgeRenderingMode.Adaptive);
            }
        }

        /// <summary>
        /// Gets or sets the text measurer.
        /// </summary>
        /// <value>The text measurer.</value>
        public IRenderContext TextMeasurer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use a workaround for vertical text alignment to support renderers with limited support for the dominate-baseline attribute.
        /// </summary>
        public bool UseVerticalTextAlignmentWorkaround { get; set; }

        /// <summary>
        /// Closes the svg writer.
        /// </summary>
        public void Close()
        {
            this.w.Close();
        }

        /// <summary>
        /// Completes the svg element.
        /// </summary>
        public void Complete()
        {
            this.w.Complete();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public override void DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode)
        {
            this.w.WriteEllipse(rect.Left, rect.Top, rect.Width, rect.Height, this.w.CreateStyle(fill, stroke, thickness), edgeRenderingMode);
        }

        /// <inheritdoc/>
        public override void DrawLine(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray,
            LineJoin lineJoin)
        {
            this.w.WritePolyline(points, this.w.CreateStyle(OxyColors.Undefined, stroke, thickness, dashArray, lineJoin), edgeRenderingMode);
        }

        /// <inheritdoc/>
        public override void DrawPolygon(
            IList<ScreenPoint> points,
            OxyColor fill,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray,
            LineJoin lineJoin)
        {
            this.w.WritePolygon(points, this.w.CreateStyle(fill, stroke, thickness, dashArray, lineJoin), edgeRenderingMode);
        }

        /// <inheritdoc/>
        public override void DrawRectangle(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode)
        {
            this.w.WriteRectangle(rect.Left, rect.Top, rect.Width, rect.Height, this.w.CreateStyle(fill, stroke, thickness), edgeRenderingMode);
        }

        /// <summary>
        /// Draws the text.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="text">The text.</param>
        /// <param name="c">The c.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">Size of the font.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <param name="rotate">The rotate.</param>
        /// <param name="halign">The horizontal alignment.</param>
        /// <param name="valign">The vertical alignment.</param>
        /// <param name="maxSize">Size of the max.</param>
        public override void DrawText(
            ScreenPoint p,
            string text,
            OxyColor c,
            string fontFamily,
            double fontSize,
            double fontWeight,
            double rotate,
            HorizontalAlignment halign,
            VerticalAlignment valign,
            OxySize? maxSize)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            var lines = StringHelper.SplitLines(text);

            var textSize = this.MeasureText(text, fontFamily, fontSize, fontWeight);
            var lineHeight = textSize.Height / lines.Length;
            var lineOffset = new ScreenVector(-Math.Sin(rotate / 180.0 * Math.PI) * lineHeight, +Math.Cos(rotate / 180.0 * Math.PI) * lineHeight);
            
            if (this.UseVerticalTextAlignmentWorkaround)
            {
                // offset the position, and set the valign to neutral value of `Bottom`
                double offsetRatio = valign == VerticalAlignment.Bottom ? (1.0 - lines.Length) : valign == VerticalAlignment.Top ? 1.0 : (1.0 - (lines.Length / 2.0));
                valign = VerticalAlignment.Bottom;

                p += lineOffset * offsetRatio;

                foreach (var line in lines)
                {
                    var size = this.MeasureText(line, fontFamily, fontSize, fontWeight);
                    this.w.WriteText(p, line, c, fontFamily, fontSize, fontWeight, rotate, halign, valign);

                    p += lineOffset;
                }
            }
            else
            {
                if (valign == VerticalAlignment.Bottom)
                {
                    for (var i = lines.Length - 1; i >= 0; i--)
                    {
                        var line = lines[i];
                        var size = this.MeasureText(line, fontFamily, fontSize, fontWeight);
                        this.w.WriteText(p, line, c, fontFamily, fontSize, fontWeight, rotate, halign, valign);

                        p -= lineOffset;
                    }
                }
                else
                {
                    foreach (var line in lines)
                    {
                        var size = this.MeasureText(line, fontFamily, fontSize, fontWeight);
                        this.w.WriteText(p, line, c, fontFamily, fontSize, fontWeight, rotate, halign, valign);

                        p += lineOffset;
                    }
                }
            }
        }

        /// <summary>
        /// Flushes this instance.
        /// </summary>
        public void Flush()
        {
            this.w.Flush();
        }

        /// <summary>
        /// Measures the text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">Size of the font.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <returns>The text size.</returns>
        public override OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight)
        {
            if (string.IsNullOrEmpty(text))
            {
                return OxySize.Empty;
            }

            return this.TextMeasurer.MeasureText(text, fontFamily, fontSize, fontWeight);
        }

        /// <summary>
        /// Draws the specified portion of the specified <see cref="OxyImage" /> at the specified location and with the specified size.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="srcX">The x-coordinate of the upper-left corner of the portion of the source image to draw.</param>
        /// <param name="srcY">The y-coordinate of the upper-left corner of the portion of the source image to draw.</param>
        /// <param name="srcWidth">Width of the portion of the source image to draw.</param>
        /// <param name="srcHeight">Height of the portion of the source image to draw.</param>
        /// <param name="destX">The x-coordinate of the upper-left corner of drawn image.</param>
        /// <param name="destY">The y-coordinate of the upper-left corner of drawn image.</param>
        /// <param name="destWidth">The width of the drawn image.</param>
        /// <param name="destHeight">The height of the drawn image.</param>
        /// <param name="opacity">The opacity.</param>
        /// <param name="interpolate">Interpolate if set to <c>true</c>.</param>
        public override void DrawImage(
            OxyImage source,
            double srcX,
            double srcY,
            double srcWidth,
            double srcHeight,
            double destX,
            double destY,
            double destWidth,
            double destHeight,
            double opacity,
            bool interpolate)
        {
            this.w.WriteImage(srcX, srcY, srcWidth, srcHeight, destX, destY, destWidth, destHeight, source);
        }

        /// <inheritdoc/>
        public override bool SetClip(OxyRect rect)
        {
            if (this.clipRect != null)
            {
                if (rect.Equals(this.clipRect.Value))
                {
                    return true;
                }
                else
                {
                    this.ResetClip();
                }
            }

            this.clipRect = rect;
            this.w.BeginClip(rect.Left, rect.Top, rect.Width, rect.Height);
            return true;
        }

        /// <inheritdoc/>
        public override void ResetClip()
        {
            if (this.clipRect == null)
            {
                return;
            }

            this.clipRect = null;
            this.w.EndClip();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.w.Dispose();
                }
            }

            this.disposed = true;
        }
    }
}
