// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MonoTouchRenderContext.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Implements a <see cref="IRenderContext"/> for MonoTouch CoreGraphics.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.XamarinIOS
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using MonoTouch.CoreGraphics;
    using MonoTouch.CoreText;
    using MonoTouch.Foundation;
    using MonoTouch.UIKit;

    /// <summary>
    /// Implements a <see cref="IRenderContext"/> for MonoTouch CoreGraphics.
    /// </summary>
    public class MonoTouchRenderContext : RenderContextBase
    {
        /// <summary>
        /// The images in use.
        /// </summary>
        private readonly HashSet<OxyImage> imagesInUse = new HashSet<OxyImage>();

        /// <summary>
        /// The image cache.
        /// </summary>
        private readonly Dictionary<OxyImage, UIImage> imageCache = new Dictionary<OxyImage, UIImage>();

        /// <summary>
        /// The graphics context.
        /// </summary>
        private readonly CGContext gctx;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoTouchRenderContext"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public MonoTouchRenderContext(CGContext context)
        {
            this.gctx = context;

            // Set rendering quality
            this.gctx.SetAllowsFontSmoothing(true);
            this.gctx.SetAllowsFontSubpixelQuantization(true);
            this.gctx.SetAllowsAntialiasing(true);
            this.gctx.SetShouldSmoothFonts(true);
            this.gctx.SetShouldAntialias(true);
            this.gctx.InterpolationQuality = CGInterpolationQuality.High;
        }

        /// <summary>
        /// Draws an ellipse.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The thickness.</param>
        public override void DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
        {
            this.SetAlias(false);
            var convertedRectangle = rect.Convert();
            if (fill.IsVisible())
            {
                this.SetFill(fill);
                var path = new CGPath();
                path.AddEllipseInRect(convertedRectangle);

                this.gctx.AddPath(path);
                this.gctx.DrawPath(CGPathDrawingMode.Fill);
            }

            if (stroke.IsVisible() && thickness > 0)
            {
                this.SetStroke(stroke, thickness);

                var path = new CGPath();
                path.AddEllipseInRect(convertedRectangle);

                this.gctx.AddPath(path);
                this.gctx.DrawPath(CGPathDrawingMode.Stroke);
            }
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
        public override void DrawImage(OxyImage source, double srcX, double srcY, double srcWidth, double srcHeight, double destX, double destY, double destWidth, double destHeight, double opacity, bool interpolate)
        {
            var image = this.GetImage(source);
            if (image == null)
            {
                return;
            }

            this.gctx.SaveState();

            double x = destX - (srcX / srcWidth * destWidth);
            double y = destY - (srcY / srcHeight * destHeight);
            this.gctx.ScaleCTM(1, -1);
            this.gctx.TranslateCTM((float)x, -(float)(y + destHeight));
            this.gctx.SetAlpha((float)opacity);
            this.gctx.InterpolationQuality = interpolate ? CGInterpolationQuality.High : CGInterpolationQuality.None;
            var destRect = new RectangleF(0f, 0f, (float)destWidth, (float)destHeight);
            this.gctx.DrawImage(destRect, image.CGImage);
            this.gctx.RestoreState();
        }

        /// <summary>
        /// Cleans up resources not in use.
        /// </summary>
        /// <remarks>This method is called at the end of each rendering.</remarks>
        public override void CleanUp()
        {
            var imagesToRelease = this.imageCache.Keys.Where(i => !this.imagesInUse.Contains(i)).ToList();
            foreach (var i in imagesToRelease)
            {
                var image = this.GetImage(i);
                image.Dispose();
                this.imageCache.Remove(i);
            }

            this.imagesInUse.Clear();
        }

        /// <summary>
        /// Sets the clip rectangle.
        /// </summary>
        /// <param name="rect">The clip rectangle.</param>
        /// <returns>True if the clip rectangle was set.</returns>
        public override bool SetClip(OxyRect rect)
        {
            this.gctx.SaveState();
            this.gctx.ClipToRect(rect.Convert());
            return true;
        }

        /// <summary>
        /// Resets the clip rectangle.
        /// </summary>
        public override void ResetClip()
        {
            this.gctx.RestoreState();
        }

        /// <summary>
        /// Draws a polyline.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The stroke thickness.</param>
        /// <param name="dashArray">The dash array.</param>
        /// <param name="lineJoin">The line join type.</param>
        /// <param name="aliased">if set to <c>true</c> the shape will be aliased.</param>
        public override void DrawLine(IList<ScreenPoint> points, OxyColor stroke, double thickness, double[] dashArray, OxyPenLineJoin lineJoin, bool aliased)
        {
            if (stroke.IsVisible() && thickness > 0)
            {
                this.SetAlias(aliased);
                this.SetStroke(stroke, thickness, dashArray, lineJoin);

                var path = new CGPath();
                var convertedPoints = (aliased ? points.Select(p => p.ConvertAliased()) : points.Select(p => p.Convert())).ToArray();
                path.AddLines(convertedPoints);

                this.gctx.AddPath(path);
                this.gctx.DrawPath(CGPathDrawingMode.Stroke);
            }
        }

        /// <summary>
        /// Draws a polygon. The polygon can have stroke and/or fill.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The stroke thickness.</param>
        /// <param name="dashArray">The dash array.</param>
        /// <param name="lineJoin">The line join type.</param>
        /// <param name="aliased">If set to <c>true</c> the shape will be aliased.</param>
        public override void DrawPolygon(IList<ScreenPoint> points, OxyColor fill, OxyColor stroke, double thickness, double[] dashArray, OxyPenLineJoin lineJoin, bool aliased)
        {
            this.SetAlias(aliased);
            var convertedPoints = (aliased ? points.Select(p => p.ConvertAliased()) : points.Select(p => p.Convert())).ToArray();
            if (fill.IsVisible())
            {
                this.SetFill(fill);
                var path = new CGPath();
                path.AddLines(convertedPoints);
                path.CloseSubpath();
                this.gctx.AddPath(path);
                this.gctx.DrawPath(CGPathDrawingMode.Fill);
            }

            if (stroke.IsVisible() && thickness > 0)
            {
                this.SetStroke(stroke, thickness, dashArray, lineJoin);

                var path = new CGPath();
                path.AddLines(convertedPoints);
                path.CloseSubpath();
                this.gctx.AddPath(path);
                this.gctx.DrawPath(CGPathDrawingMode.Stroke);
            }
        }

        /// <summary>
        /// Draws a rectangle.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The stroke thickness.</param>
        public override void DrawRectangle(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
        {
            this.SetAlias(true);
            var convertedRect = rect.ConvertAliased();

            if (fill.IsVisible())
            {
                this.SetFill(fill);
                var path = new CGPath();
                path.AddRect(convertedRect);
                this.gctx.AddPath(path);
                this.gctx.DrawPath(CGPathDrawingMode.Fill);
            }

            if (stroke.IsVisible() && thickness > 0)
            {
                this.SetStroke(stroke, thickness);

                var path = new CGPath();
                path.AddRect(convertedRect);
                this.gctx.AddPath(path);
                this.gctx.DrawPath(CGPathDrawingMode.Stroke);
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
        public override void DrawText(ScreenPoint p, string text, OxyColor fill, string fontFamily, double fontSize, double fontWeight, double rotate, HorizontalAlignment halign, VerticalAlignment valign, OxySize? maxSize)
        {
            if (string.IsNullOrEmpty(text) || fontFamily == null)
            {
                return;
            }

            var fontName = GetActualFontName(fontFamily, fontWeight);

            using (var attributedString = new NSAttributedString(text, new CTStringAttributes { ForegroundColorFromContext = true, Font = new CTFont(fontName, (float)fontSize) }))
            {
                using (var textLine = new CTLine(attributedString))
                {
                    float width;
                    float height;
                    if (maxSize.HasValue || halign != HorizontalAlignment.Left || valign != VerticalAlignment.Bottom)
                    {
                        var bounds = textLine.GetImageBounds(this.gctx);
                        width = bounds.Width;
                        height = bounds.Height;
                    }
                    else
                    {
                        width = height = 0f;
                    }

                    if (maxSize.HasValue)
                    {
                        if (width > maxSize.Value.Width)
                        {
                            width = (float)maxSize.Value.Width;
                        }

                        if (height > maxSize.Value.Height)
                        {
                            height = (float)maxSize.Value.Height;
                        }
                    }

                    var x = p.X;
                    var y = p.Y;

                    this.gctx.SaveState();

                    this.SetFill(fill);
                    this.SetAlias(false);

                    this.gctx.SetTextDrawingMode(CGTextDrawingMode.Fill);
                    this.gctx.TextPosition = new PointF(0, 0);

                    var dx = halign == HorizontalAlignment.Left ? 0d : (halign == HorizontalAlignment.Center ? -width * 0.5 : -width);
                    var dy = valign == VerticalAlignment.Bottom ? 0d : (valign == VerticalAlignment.Middle ? height * 0.5 : height);

                    this.gctx.TranslateCTM((float)x, (float)y);
                    this.gctx.RotateCTM((float)(rotate / 180 * Math.PI));
                    this.gctx.TranslateCTM((float)dx, (float)dy);
                    this.gctx.ScaleCTM(1f, -1f);

                    if (maxSize.HasValue)
                    {
                        this.gctx.ClipToRect(new RectangleF(0, 0, width, height));
                    }

                    textLine.Draw(this.gctx);
                    this.gctx.RestoreState();
                }
            }
        }

        /// <summary>
        /// Measures the text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">Size of the font.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <returns>
        /// The size of the text.
        /// </returns>
        public override OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight)
        {
            if (string.IsNullOrEmpty(text) || fontFamily == null)
            {
                return OxySize.Empty;
            }

            var fontName = GetActualFontName(fontFamily, fontWeight);

            using (var attributedString = new NSAttributedString(text, new CTStringAttributes { ForegroundColorFromContext = true, Font = new CTFont(fontName, (float)fontSize) }))
            {
                using (var textLine = new CTLine(attributedString))
                {
                    var bounds = textLine.GetImageBounds(this.gctx);
                    return new OxySize(bounds.Width, bounds.Height);
                }
            }
        }

        /// <summary>
        /// Gets the actual font for iOS.
        /// </summary>
        /// <param name="fontFamily">The font family.</param> 
        /// <param name="fontWeight">The font weight.</param> 
        /// <returns>The actual font name.</returns>
        private static string GetActualFontName(string fontFamily, double fontWeight)
        {
            string fontName;
            switch (fontFamily)
            {
                case "Segoe UI":
                case "Arial":
                    fontName = "HelveticaNeue";
                    break;
                case "Times New Roman":
                    fontName = "Times";
                    break;
                default:
                    fontName = fontFamily;
                    break;
            }

            if (fontWeight >= 700)
            {
                fontName += "-Bold";
            }

            return fontName;
        }

        /// <summary>
        /// Sets the alias state.
        /// </summary>
        /// <param name="alias">alias if set to <c>true</c>.</param>
        private void SetAlias(bool alias)
        {
            this.gctx.SetShouldAntialias(!alias);
        }

        /// <summary>
        /// Sets the fill color.
        /// </summary>
        /// <param name="c">The color.</param>
        private void SetFill(OxyColor c)
        {
            this.gctx.SetFillColor(c.ToCGColor());
        }

        /// <summary>
        /// Sets the stroke style.
        /// </summary>
        /// <param name="c">The stroke color.</param>
        /// <param name="thickness">The stroke thickness.</param>
        /// <param name="dashArray">The dash array.</param>
        /// <param name="lineJoin">The line join.</param>
        private void SetStroke(OxyColor c, double thickness, double[] dashArray = null, OxyPenLineJoin lineJoin = OxyPenLineJoin.Miter)
        {
            this.gctx.SetStrokeColor(c.ToCGColor());
            this.gctx.SetLineWidth((float)thickness);
            this.gctx.SetLineJoin(lineJoin.Convert());
            if (dashArray != null)
            {
                var lengths = dashArray.Select(d => (float)d).ToArray();
                this.gctx.SetLineDash(0f, lengths);
            }
            else
            {
                this.gctx.SetLineDash(0, null);
            }
        }

        /// <summary>
        /// Gets the image from cache or converts the specified <paramref name="source"/> <see cref="OxyImage"/>.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>The image.</returns>
        private UIImage GetImage(OxyImage source)
        {
            if (source == null)
            {
                return null;
            }

            if (!this.imagesInUse.Contains(source))
            {
                this.imagesInUse.Add(source);
            }

            UIImage src;
            if (!this.imageCache.TryGetValue(source, out src))
            {
                using (var data = NSData.FromArray(source.GetData()))
                {
                    src = UIImage.LoadFromData(data);
                }

                this.imageCache.Add(source, src);
            }

            return src;
        }
    }
}