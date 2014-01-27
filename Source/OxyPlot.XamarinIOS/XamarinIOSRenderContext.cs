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
    using MonoTouch.UIKit;
    using MonoTouch.CoreGraphics;
    using MonoTouch.Foundation;

	/// <summary>
	/// Implements a <see cref="IRenderContext"/> for MonoTouch CoreGraphics.
	/// </summary>
    public class MonoTouchRenderContext : RenderContextBase
    {
        private CGContext gctx;

        public MonoTouchRenderContext(CGContext context)
        {
            gctx = context;

			// Set rendering quality
            gctx.SetAllowsFontSmoothing(true);
            gctx.SetAllowsFontSubpixelQuantization(true);
            gctx.SetAllowsAntialiasing(true);
            gctx.SetShouldSmoothFonts(true);
            gctx.SetShouldAntialias(true);
            gctx.InterpolationQuality = CGInterpolationQuality.High;
        }

        private UIColor ToColor(OxyColor c)
        {
            return UIColor.FromRGBA(c.R, c.G, c.B, c.A);
        }

        private RectangleF ToRectangle(OxyRect rect)
        {
            return new RectangleF((int)rect.Left, (int)rect.Top, (int)rect.Width, (int)rect.Height);
        }

        private PointF ToPoint(ScreenPoint p)
        {
            return new PointF((float)p.X, (float)p.Y);
        }

		private void SetAlias(bool alias) 
		{
			gctx.SetShouldAntialias(!alias);
		}

        public override void DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
        {
			this.SetAlias(false);

            if (fill.IsVisible())
            {
                ToColor(fill).SetFill();
                var path = new CGPath();
                path.AddElipseInRect(ToRectangle(rect));

                gctx.AddPath(path);
                gctx.DrawPath(CGPathDrawingMode.Fill);
            }

            if (stroke.IsVisible() && thickness > 0)
            {
                ToColor(stroke).SetStroke();
                gctx.SetLineWidth((float)thickness);

                var path = new CGPath();
                path.AddElipseInRect(ToRectangle(rect));

                gctx.AddPath(path);
                gctx.DrawPath(CGPathDrawingMode.Stroke);
            }
        }

        public override void DrawLine(IList<ScreenPoint> points, OxyColor stroke, double thickness, double[] dashArray, OxyPenLineJoin lineJoin, bool aliased)
        {
            if (stroke.IsVisible() && thickness > 0)
            {
				this.SetAlias(aliased);
                gctx.SetLineCap(ToLine(lineJoin));

                ToColor(stroke).SetStroke();
                gctx.SetLineWidth((float)thickness);

				var path = new CGPath();

                path.AddLines(points.Select(p => ToPoint(p)).ToArray());

                gctx.AddPath(path);
                gctx.DrawPath(CGPathDrawingMode.Stroke);
            }
        }

        private CGLineCap ToLine(OxyPenLineJoin lineJoin)
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

        public override void DrawPolygon(IList<ScreenPoint> points, OxyColor fill, OxyColor stroke, double thickness, double[] dashArray, OxyPenLineJoin lineJoin, bool aliased)
        {
			this.SetAlias(aliased);

            if (fill.IsVisible())
            {
                ToColor(fill).SetFill();
                var path = new CGPath();
                path.AddLines(points.Select(p => ToPoint(p)).ToArray());
                path.CloseSubpath();
                gctx.AddPath(path);
                gctx.DrawPath(CGPathDrawingMode.Fill);
            }

            if (stroke.IsVisible() && thickness > 0)
            {
                ToColor(stroke).SetStroke();
                gctx.SetLineWidth((float)thickness);
                gctx.SetLineCap(ToLine(lineJoin));

                var path = new CGPath();
                path.AddLines(points.Select(p => ToPoint(p)).ToArray());
                path.CloseSubpath();
                gctx.AddPath(path);
                gctx.DrawPath(CGPathDrawingMode.Stroke);
            }
        }

        public override void DrawRectangle(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
        {
			this.SetAlias(true);

            if (fill.IsVisible())
            {
                ToColor(fill).SetFill();
                var path = new CGPath();
                path.AddRect(ToRectangle(rect));
                gctx.AddPath(path);
                gctx.DrawPath(CGPathDrawingMode.Fill);
            }

            if (stroke.IsVisible() && thickness > 0)
            {
                ToColor(stroke).SetStroke();
                gctx.SetLineWidth((float)thickness);

                var path2 = new CGPath();
                path2.AddRect(ToRectangle(rect));
                gctx.AddPath(path2);
                gctx.DrawPath(CGPathDrawingMode.Stroke);
            }
        }

        public override void DrawText(ScreenPoint p, string text, OxyColor fill, string fontFamily, double fontSize, double fontWeight, double rotate, HorizontalAlignment halign, VerticalAlignment valign, OxySize? maxSize)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            fontFamily = GetDefaultFont(fontFamily);

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

            gctx.SaveState();

            gctx.SelectFont(fontFamily, (float)fontSize, CGTextEncoding.MacRoman);
            ToColor(fill).SetFill();

			this.SetAlias(false);

            gctx.SetTextDrawingMode(CGTextDrawingMode.Fill);

            var tfont = UIFont.FromName(fontFamily, (float)fontSize);
            NSString nsstr = new NSString(text);
            SizeF sz = nsstr.StringSize(tfont);

            float y = (float)(p.Y);
            float x = (float)(p.X);

            // Rotate the text here.
            var m = CGAffineTransform.MakeTranslation(-x, -y);
			m.Multiply(CGAffineTransform.MakeRotation((float)(Math.PI / 180d * rotate)));
            m.Multiply(CGAffineTransform.MakeTranslation(x, y));

            gctx.ConcatCTM(m);

            switch (halign)
            {
                case HorizontalAlignment.Left:
                    x = (float)(p.X);
                    break;
                case HorizontalAlignment.Right:
                    x = (float)(p.X - sz.Width);
                    break;
                case HorizontalAlignment.Center:
                    x = (float)(p.X - (sz.Width / 2));
                    break;
            }

            switch (valign)
            {
                case VerticalAlignment.Bottom:
                    y -= (float)sz.Height;
                    break;
                case VerticalAlignment.Top:
                    //y += (float)fontSize;
                    break;
                case VerticalAlignment.Middle:
                    y -= (float)(sz.Height / 2);
                    break;
            }

            RectangleF rect = new RectangleF(x, y, sz.Width, sz.Height);
            nsstr.DrawString(rect, tfont);

            gctx.RestoreState();
        }

        public override OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight)
        {
            if (text == null)
            {
                return OxySize.Empty;
            }

            fontFamily = GetDefaultFont(fontFamily);

            gctx.SelectFont(fontFamily, (float)fontSize, CGTextEncoding.MacRoman);

            UIFont tfont = UIFont.FromName(fontFamily, (float)fontSize);

            NSString nsstr = new NSString(text);
            SizeF sz = nsstr.StringSize(tfont);

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
    }
}