// <copyright file="MonoTouchRenderContext.cs" company="OxyPlot">
//   The MIT License (MIT)
// --------------------------------------------------------------------------------------------------------------------
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
    using MonoTouch.Foundation;
    using MonoTouch.UIKit;

    /// <summary>
    /// Implements a <see cref="IRenderContext"/> for MonoTouch CoreGraphics.
    /// </summary>
    public class MonoTouchRenderContext : RenderContextBase
    {
        private CGContext gctx;

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

        public override void DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
        {
            this.SetAlias(false);

            if (fill.IsVisible())
            {
                this.SetFill(fill);
                var path = new CGPath();
                path.AddElipseInRect(this.ConvertRectangle(rect));

                this.gctx.AddPath(path);
                this.gctx.DrawPath(CGPathDrawingMode.Fill);
            }

            if (stroke.IsVisible() && thickness > 0)
            {
                this.SetStroke(stroke, thickness);

                var path = new CGPath();
                path.AddElipseInRect(this.ConvertRectangle(rect));

                this.gctx.AddPath(path);
                this.gctx.DrawPath(CGPathDrawingMode.Stroke);
            }
        }

        public override void DrawLine(IList<ScreenPoint> points, OxyColor stroke, double thickness, double[] dashArray, OxyPenLineJoin lineJoin, bool aliased)
        {
            if (stroke.IsVisible() && thickness > 0)
            {
                this.SetAlias(aliased);
                this.SetStroke(stroke, thickness, dashArray, lineJoin);

                var path = new CGPath();
                path.AddLines(points.Select(this.ToPoint).ToArray());

                this.gctx.AddPath(path);
                this.gctx.DrawPath(CGPathDrawingMode.Stroke);
            }
        }

        public override void DrawPolygon(IList<ScreenPoint> points, OxyColor fill, OxyColor stroke, double thickness, double[] dashArray, OxyPenLineJoin lineJoin, bool aliased)
        {
            this.SetAlias(aliased);

            if (fill.IsVisible())
            {
                this.SetFill(fill);
                var path = new CGPath();
                path.AddLines(points.Select(this.ToPoint).ToArray());
                path.CloseSubpath();
                this.gctx.AddPath(path);
                this.gctx.DrawPath(CGPathDrawingMode.Fill);
            }

            if (stroke.IsVisible() && thickness > 0)
            {
                this.SetStroke(stroke, thickness, dashArray, lineJoin);

                var path = new CGPath();
                path.AddLines(points.Select(this.ToPoint).ToArray());
                path.CloseSubpath();
                this.gctx.AddPath(path);
                this.gctx.DrawPath(CGPathDrawingMode.Stroke);
            }
        }

        public override void DrawRectangle(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
        {
            this.SetAlias(true);

            if (fill.IsVisible())
            {
                this.SetFill(fill);
                var path = new CGPath();
                path.AddRect(this.ConvertRectangle(rect));
                this.gctx.AddPath(path);
                this.gctx.DrawPath(CGPathDrawingMode.Fill);
            }

            if (stroke.IsVisible() && thickness > 0)
            {
                this.SetStroke(stroke, thickness);

                var path = new CGPath();
                path.AddRect(this.ConvertRectangle(rect));
                this.gctx.AddPath(path);
                this.gctx.DrawPath(CGPathDrawingMode.Stroke);
            }
        }

        public override void DrawText(ScreenPoint p, string text, OxyColor fill, string fontFamily, double fontSize, double fontWeight, double rotate, HorizontalAlignment halign, VerticalAlignment valign, OxySize? maxSize)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            fontFamily = this.GetDefaultFont(fontFamily);

            if (fontWeight >= 700)
            {
                //fs = FontStyle.Bold;
            }

            if (maxSize != null)
            {
                //                if (size.Width > maxSize.Value.Width)
                //                {
                //                    size.Width = (float)maxSize.Value.Width;
                //                }
                //
                //                if (size.Height > maxSize.Value.Height)
                //                {
                //                    size.Height = (float)maxSize.Value.Height;
                //                }
            }

            this.gctx.SaveState();

            // TODO: deprecated in iOS 7.0 - replace with core text
            // https://developer.apple.com/library/ios/documentation/GraphicsImaging/Reference/CGContext/DeprecationAppendix/AppendixADeprecatedAPI.html#//apple_ref/c/func/CGContextSelectFont
            this.gctx.SelectFont(fontFamily, (float)fontSize, CGTextEncoding.MacRoman);
            this.SetFill(fill);
            this.SetAlias(false);

            this.gctx.SetTextDrawingMode(CGTextDrawingMode.Fill);

            var tfont = UIFont.FromName(fontFamily, (float)fontSize);
            var nsstr = new NSString(text);
            var sz = nsstr.StringSize(tfont);

            float y = (float)(p.Y);
            float x = (float)(p.X);

            // Rotate the text here.
            var m = CGAffineTransform.MakeTranslation(-x, -y);
            m.Multiply(CGAffineTransform.MakeRotation((float)(Math.PI / 180d * rotate)));
            m.Multiply(CGAffineTransform.MakeTranslation(x, y));

            this.gctx.ConcatCTM(m);

            switch (halign)
            {
                case HorizontalAlignment.Left:
                    x = (float)p.X;
                    break;
                case HorizontalAlignment.Center:
                    x = (float)(p.X - (sz.Width / 2));
                    break;
                case HorizontalAlignment.Right:
                    x = (float)(p.X - sz.Width);
                    break;
            }

            switch (valign)
            {
                case VerticalAlignment.Top:
                    break;
                case VerticalAlignment.Middle:
                    y -= (float)(sz.Height / 2);
                    break;
                case VerticalAlignment.Bottom:
                    y -= (float)sz.Height;
                    break;
            }

            var rect = new RectangleF(x, y, sz.Width, sz.Height);
            nsstr.DrawString(rect, tfont);

            this.gctx.RestoreState();
        }

        public override OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight)
        {
            if (text == null)
            {
                return OxySize.Empty;
            }

            fontFamily = this.GetDefaultFont(fontFamily);

            // TODO: deprecated in iOS 7.0 - replace with core text
            this.gctx.SelectFont(fontFamily, (float)fontSize, CGTextEncoding.MacRoman);

            var tfont = UIFont.FromName(fontFamily, (float)fontSize);

            var nsstr = new NSString(text);
            var sz = nsstr.StringSize(tfont);

            return new OxySize(sz.Width, sz.Height);
        }

        /// <summary>
        /// Gets the default font for iOS if the default font is Segoe UI as this font is not supported by iOS.
        /// </summary>
        /// <returns>
        /// The default font of Helvetica Neue.
        /// </returns>
        /// <param name='fontFamily'>
        /// Font family.
        /// </param>
        private string GetDefaultFont(string fontFamily)
        {
            return fontFamily == "Segoe UI" ? "Helvetica Neue" : fontFamily;
        }

        private void SetAlias(bool alias)
        {
            this.gctx.SetShouldAntialias(!alias);
        }

        private UIColor ToColor(OxyColor c)
        {
            return UIColor.FromRGBA(c.R, c.G, c.B, c.A);
        }

        private CGColor ConvertColor(OxyColor c)
        {
            return new CGColor(c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);
        }

        private void SetFill(OxyColor c)
        {
            // this.gctx.SetFillColor(this.ConvertColor(c));
            this.ToColor(c).SetFill();
        }

        private void SetStroke(OxyColor c, double thickness, double[] dashArray = null, OxyPenLineJoin lineJoin = OxyPenLineJoin.Miter)
        {
            this.ToColor(c).SetStroke();
            // this.gctx.SetStroke(this.ConvertColor(c));
            this.gctx.SetLineWidth((float)thickness);
            this.gctx.SetLineCap(ConvertLineJoin(lineJoin));
        }

        private RectangleF ConvertRectangle(OxyRect rect)
        {
            return new RectangleF((int)rect.Left, (int)rect.Top, (int)rect.Width, (int)rect.Height);
        }

        private PointF ToPoint(ScreenPoint p)
        {
            return new PointF((float)p.X, (float)p.Y);
        }

        private CGLineCap ConvertLineJoin(OxyPenLineJoin lineJoin)
        {
            switch (lineJoin)
            {
                case OxyPenLineJoin.Bevel:
                    return CGLineCap.Butt;
                case OxyPenLineJoin.Miter:
                    return CGLineCap.Square;
                case OxyPenLineJoin.Round:
                    return CGLineCap.Round;
            }

            return CGLineCap.Square;
        }
    }
}