// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PdfRenderContext.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Implements an <see cref="IRenderContext" /> producing PDF documents by <see cref="PortableDocument" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Implements an <see cref="IRenderContext" /> producing PDF documents by <see cref="PortableDocument" />.
    /// </summary>
    public class PdfRenderContext : RenderContextBase
    {
        /// <summary>
        /// The current document.
        /// </summary>
        private readonly PortableDocument doc;

        /// <summary>
        /// The image cache.
        /// </summary>
        private readonly Dictionary<OxyImage, PortableDocumentImage> images = new Dictionary<OxyImage, PortableDocumentImage>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfRenderContext" /> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="background">The background.</param>
        public PdfRenderContext(double width, double height, OxyColor background)
        {
            this.doc = new PortableDocument();
            this.doc.AddPage(width, height);
            this.RendersToScreen = false;

            if (background.IsVisible())
            {
                this.doc.SetFillColor(background);
                this.doc.FillRectangle(0, 0, width, height);
            }
        }

        /// <summary>
        /// Saves the output to the specified stream.
        /// </summary>
        /// <param name="s">The stream.</param>
        public void Save(Stream s)
        {
            this.doc.Save(s);
        }

        /// <summary>
        /// Draws an ellipse.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode. This is not supported and will be ignored.</param>
        public override void DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode)
        {
            var isStroked = stroke.IsVisible() && thickness > 0;
            var isFilled = fill.IsVisible();
            if (!isStroked && !isFilled)
            {
                return;
            }

            double y = this.doc.PageHeight - rect.Bottom;
            if (isStroked)
            {
                this.SetLineWidth(thickness);
                this.doc.SetColor(stroke);
                if (isFilled)
                {
                    this.doc.SetFillColor(fill);
                    this.doc.DrawEllipse(rect.Left, y, rect.Width, rect.Height, true);
                }
                else
                {
                    this.doc.DrawEllipse(rect.Left, y, rect.Width, rect.Height);
                }
            }
            else
            {
                this.doc.SetFillColor(fill);
                this.doc.FillEllipse(rect.Left, y, rect.Width, rect.Height);
            }
        }

        /// <summary>
        /// Draws a polyline.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The stroke thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode. This is not supported and will be ignored.</param>
        /// <param name="dashArray">The dash array.</param>
        /// <param name="lineJoin">The line join type.</param>
        public override void DrawLine(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray,
            LineJoin lineJoin)
        {
            this.doc.SetColor(stroke);
            this.SetLineWidth(thickness);
            if (dashArray != null)
            {
                this.SetLineDashPattern(dashArray, 0);
            }

            this.doc.SetLineJoin(Convert(lineJoin));
            var h = this.doc.PageHeight;
            this.doc.MoveTo(points[0].X, h - points[0].Y);
            for (int i = 1; i < points.Count; i++)
            {
                this.doc.LineTo(points[i].X, h - points[i].Y);
            }

            this.doc.Stroke(false);
            if (dashArray != null)
            {
                this.doc.ResetLineDashPattern();
            }
        }

        /// <summary>
        /// Draws a polygon. The polygon can have stroke and/or fill.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The stroke thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode. This is not supported and will be ignored.</param>
        /// <param name="dashArray">The dash array.</param>
        /// <param name="lineJoin">The line join type.</param>
        public override void DrawPolygon(
            IList<ScreenPoint> points,
            OxyColor fill,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray,
            LineJoin lineJoin)
        {
            var isStroked = stroke.IsVisible() && thickness > 0;
            var isFilled = fill.IsVisible();
            if (!isStroked && !isFilled)
            {
                return;
            }

            var h = this.doc.PageHeight;
            this.doc.MoveTo(points[0].X, h - points[0].Y);
            for (int i = 1; i < points.Count; i++)
            {
                this.doc.LineTo(points[i].X, h - points[i].Y);
            }

            if (isStroked)
            {
                this.doc.SetColor(stroke);
                this.SetLineWidth(thickness);
                if (dashArray != null)
                {
                    this.SetLineDashPattern(dashArray, 0);
                }

                this.doc.SetLineJoin(Convert(lineJoin));
                if (isFilled)
                {
                    this.doc.SetFillColor(fill);
                    this.doc.FillAndStroke();
                }
                else
                {
                    this.doc.Stroke();
                }

                if (dashArray != null)
                {
                    this.doc.ResetLineDashPattern();
                }
            }
            else
            {
                this.doc.SetFillColor(fill);
                this.doc.Fill();
            }
        }

        /// <summary>
        /// Draws a rectangle.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The stroke thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode. This is not supported and will be ignored.</param>
        public override void DrawRectangle(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode)
        {
            var isStroked = stroke.IsVisible() && thickness > 0;
            var isFilled = fill.IsVisible();
            if (!isStroked && !isFilled)
            {
                return;
            }

            double y = this.doc.PageHeight - rect.Bottom;
            if (isStroked)
            {
                this.SetLineWidth(thickness);
                this.doc.SetColor(stroke);
                if (isFilled)
                {
                    this.doc.SetFillColor(fill);
                    this.doc.DrawRectangle(rect.Left, y, rect.Width, rect.Height, true);
                }
                else
                {
                    this.doc.DrawRectangle(rect.Left, y, rect.Width, rect.Height);
                }
            }
            else
            {
                this.doc.SetFillColor(fill);
                this.doc.FillRectangle(rect.Left, y, rect.Width, rect.Height);
            }
        }

        /// <summary>
        /// Draws the text.
        /// </summary>
        /// <param name="p">The position of the text.</param>
        /// <param name="text">The text.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">Size of the font.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <param name="rotate">The rotation angle.</param>
        /// <param name="halign">The horizontal alignment.</param>
        /// <param name="valign">The vertical alignment.</param>
        /// <param name="maxSize">The maximum size of the text.</param>
        public override void DrawText(
            ScreenPoint p,
            string text,
            OxyColor fill,
            string fontFamily,
            double fontSize,
            double fontWeight,
            double rotate,
            HorizontalAlignment halign,
            VerticalAlignment valign,
            OxySize? maxSize)
        {
            this.doc.SaveState();
            this.doc.SetFont(fontFamily, fontSize / 96 * 72, fontWeight > 500);
            this.doc.SetFillColor(fill);

            double width, height;
            this.doc.MeasureText(text, out width, out height);
            if (maxSize != null)
            {
                if (width > maxSize.Value.Width)
                {
                    width = Math.Max(maxSize.Value.Width, 0);
                }

                if (height > maxSize.Value.Height)
                {
                    height = Math.Max(maxSize.Value.Height, 0);
                }
            }

            double dx = 0;
            if (halign == HorizontalAlignment.Center)
            {
                dx = -width / 2;
            }

            if (halign == HorizontalAlignment.Right)
            {
                dx = -width;
            }

            double dy = 0;

            if (valign == VerticalAlignment.Middle)
            {
                dy = -height / 2;
            }

            if (valign == VerticalAlignment.Top)
            {
                dy = -height;
            }

            double y = this.doc.PageHeight - p.Y;

            this.doc.Translate(p.X, y);
            if (Math.Abs(rotate) > 1e-6)
            {
                this.doc.Rotate(-rotate);
            }

            this.doc.Translate(dx, dy);

            // this.doc.DrawRectangle(0, 0, width, height);
            this.doc.SetClippingRectangle(0, 0, width, height);
            this.doc.DrawText(0, 0, text);
            this.doc.RestoreState();
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
            this.doc.SetFont(fontFamily, fontSize / 96 * 72, fontWeight > 500);
            double width, height;
            this.doc.MeasureText(text, out width, out height);
            return new OxySize(width, height);
        }

        /// <summary>
        /// Sets the clip rectangle.
        /// </summary>
        /// <param name="rect">The clip rectangle.</param>
        /// <returns>True if the clip rectangle was set.</returns>
        public override bool SetClip(OxyRect rect)
        {
            this.doc.SaveState();
            this.doc.SetClippingRectangle(rect.Left, rect.Bottom, rect.Width, rect.Height);
            return true;
        }

        /// <summary>
        /// Resets the clip rectangle.
        /// </summary>
        public override void ResetClip()
        {
            this.doc.RestoreState();
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
            PortableDocumentImage image;
            if (!this.images.TryGetValue(source, out image))
            {
                image = PortableDocumentImageUtilities.Convert(source, interpolate);
                if (image == null)
                {
                    // TODO: remove this when image decoding is working
                    return;
                }

                this.images[source] = image;
            }

            this.doc.SaveState();
            double x = destX - (srcX / srcWidth * destWidth);
            double width = image.Width / srcWidth * destWidth;
            double y = destY - (srcY / srcHeight * destHeight);
            double height = image.Height / srcHeight * destHeight;
            this.doc.SetClippingRectangle(destX, this.doc.PageHeight - (destY - destHeight), destWidth, destHeight);
            this.doc.Translate(x, this.doc.PageHeight - (y + height));
            this.doc.Scale(width, height);
            this.doc.DrawImage(image);
            this.doc.RestoreState();
        }

        /// <summary>
        /// Converts the specified <see cref="OxyPlot.LineJoin" /> to a <see cref="OxyPlot.LineJoin" />.
        /// </summary>
        /// <param name="lineJoin">The value to convert.</param>
        /// <returns>The converted value.</returns>
        private static LineJoin Convert(LineJoin lineJoin)
        {
            switch (lineJoin)
            {
                case LineJoin.Bevel:
                    return LineJoin.Bevel;
                case LineJoin.Miter:
                    return LineJoin.Miter;
                default:
                    return LineJoin.Round;
            }
        }

        /// <summary>
        /// Sets the width of the line.
        /// </summary>
        /// <param name="thickness">The thickness (in 1/96 inch units).</param>
        private void SetLineWidth(double thickness)
        {
            // Convert from 1/96 inch to points
            this.doc.SetLineWidth(thickness / 96 * 72);
        }

        /// <summary>
        /// Sets the line dash pattern.
        /// </summary>
        /// <param name="dashArray">The dash array (in 1/96 inch units).</param>
        /// <param name="dashPhase">The dash phase (in 1/96 inch units).</param>
        private void SetLineDashPattern(double[] dashArray, double dashPhase)
        {
            this.doc.SetLineDashPattern(dashArray.Select(d => d / 96 * 72).ToArray(), dashPhase / 96 * 72);
        }
    }
}
