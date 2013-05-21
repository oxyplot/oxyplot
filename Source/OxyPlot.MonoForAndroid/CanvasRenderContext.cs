// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CanvasRenderContext.cs" company="OxyPlot">
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
// <summary>
//   Provides a render context for Android.Graphics.Canvas.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System.Collections.Generic;

namespace OxyPlot.MonoForAndroid
{
    using Android.Graphics;

    /// <summary>
    /// Provides a render context for Android.Graphics.Canvas.
    /// </summary>
    public class CanvasRenderContext : IRenderContext
    {
        private Canvas canvas;

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasRenderContext"/> class.
        /// </summary>
        public CanvasRenderContext()
        {
        }

        /// <summary>
        /// Sets the target.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        public void SetTarget(Canvas canvas)
        {
            this.canvas = canvas;
        }

        /// <summary>
        /// Gets a value indicating whether the context renders to screen.
        /// </summary>
        /// <value>
        /// <c>true</c> if the context renders to screen; otherwise, <c>false</c>.
        /// </value>
        public bool RendersToScreen
        {
            get
            {
                return true;
            }
        }

        public void DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness = 1)
        {
            using (var paint = new Paint())
            {
                paint.AntiAlias = true;
                paint.StrokeWidth = (float)thickness;
                if (fill != null)
                {
                    paint.SetStyle(Paint.Style.Fill);
                    paint.Color = stroke.ToColor();
                    canvas.DrawOval(rect.ToRectF(), paint);
                }
                if (stroke != null)
                {
                    paint.SetStyle(Paint.Style.Stroke);
                    paint.Color = stroke.ToColor();
                    canvas.DrawOval(rect.ToRectF(), paint);
                }
            }
        }

        public void DrawEllipses(IList<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness = 1)
        {
            using (var paint = new Paint())
            {
                paint.AntiAlias = true;
                paint.StrokeWidth = (float)thickness;
                foreach (var rect in rectangles)
                {
                    if (fill != null)
                    {
                        paint.SetStyle(Paint.Style.Fill);
                        paint.Color = fill.ToColor();
                        canvas.DrawOval(rect.ToRectF(), paint);
                    }

                    if (stroke != null)
                    {
                        paint.SetStyle(Paint.Style.Stroke);
                        paint.Color = stroke.ToColor();
                        canvas.DrawOval(rect.ToRectF(), paint);
                    }
                }

            }
        }

        public void DrawLine(IList<ScreenPoint> points, OxyColor stroke, double thickness = 1, double[] dashArray = null, OxyPenLineJoin lineJoin = OxyPenLineJoin.Miter, bool aliased = false)
        {
            using (var paint = new Paint())
            {
                paint.StrokeWidth = (float)thickness;
                paint.Color = stroke.ToColor();
                paint.AntiAlias = !aliased;
                var pts = new float[(points.Count - 1) * 4];
                int j = 0;
                for (int i = 0; i + 1 < points.Count; i++)
                {
                    pts[j++] = (float)points[i].X;
                    pts[j++] = (float)points[i].Y;
                    pts[j++] = (float)points[i + 1].X;
                    pts[j++] = (float)points[i + 1].Y;
                }

                canvas.DrawLines(pts, paint);
            }
        }

        public void DrawLineSegments(IList<ScreenPoint> points, OxyColor stroke, double thickness = 1, double[] dashArray = null, OxyPenLineJoin lineJoin = OxyPenLineJoin.Miter, bool aliased = false)
        {
            using (var paint = new Paint())
            {
                paint.StrokeWidth = (float)thickness;
                paint.Color = stroke.ToColor();
                paint.AntiAlias = !aliased;
                var pts = new float[points.Count * 2];
                int i = 0;
                foreach (var p in points)
                {
                    pts[i++] = (float)p.X;
                    pts[i++] = (float)p.Y;
                }

                canvas.DrawLines(pts, paint);
            }
        }

        public void DrawPolygon(IList<ScreenPoint> points, OxyColor fill, OxyColor stroke, double thickness = 1, double[] dashArray = null, OxyPenLineJoin lineJoin = OxyPenLineJoin.Miter, bool aliased = false)
        {
            using (var paint = new Paint())
            {
                paint.AntiAlias = !aliased;
                paint.StrokeWidth = (float)thickness;
                using (var path = new Path())
                {
                    path.MoveTo((float)points[0].X, (float)points[0].Y);
                    for (int i = 1; i <= points.Count; i++)
                    {
                        path.LineTo((float)points[i % points.Count].X, (float)points[i % points.Count].Y);
                    }

                    if (fill != null)
                    {
                        paint.SetStyle(Paint.Style.Fill);
                        paint.Color = fill.ToColor();
                        canvas.DrawPath(path, paint);
                    }

                    if (stroke != null)
                    {
                        paint.SetStyle(Paint.Style.Stroke);
                        paint.Color = stroke.ToColor();
                        canvas.DrawPath(path, paint);
                    }
                }
            }
        }

        public void DrawPolygons(IList<IList<ScreenPoint>> polygons, OxyColor fill, OxyColor stroke, double thickness = 1, double[] dashArray = null, OxyPenLineJoin lineJoin = OxyPenLineJoin.Miter, bool aliased = false)
        {
            foreach (var p in polygons)
                this.DrawPolygon(p, fill, stroke, thickness, dashArray, lineJoin, aliased);
        }

        public void DrawRectangle(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness = 1)
        {
            using (var paint = new Paint())
            {
                paint.AntiAlias = false;
                paint.StrokeWidth = (float)thickness;
                if (fill != null)
                {
                    paint.SetStyle(Paint.Style.Fill);
                    paint.Color = fill.ToColor();
                    canvas.DrawRect((float)rect.Left, (float)rect.Top, (float)rect.Right, (float)rect.Bottom, paint);
                }
                if (stroke != null)
                {
                    paint.SetStyle(Paint.Style.Stroke);
                    paint.Color = stroke.ToColor();
                    canvas.DrawRect((float)rect.Left, (float)rect.Top, (float)rect.Right, (float)rect.Bottom, paint);
                }
            }
        }

        public void DrawRectangles(IList<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness = 1)
        {
            foreach (var r in rectangles)
            {
                this.DrawRectangle(r, fill, stroke, thickness);
            }
        }

        public void DrawText(ScreenPoint p, string text, OxyColor fill, string fontFamily = null, double fontSize = 10, double fontWeight = 500, double rotate = 0, OxyPlot.HorizontalAlignment halign = HorizontalAlignment.Left, VerticalAlignment valign = VerticalAlignment.Top, OxySize? maxSize = new OxySize?())
        {
            using (var paint = new Paint())
            {
                paint.AntiAlias = true;
                paint.TextSize = (float)fontSize;
                paint.Color = fill.ToColor();
                var bounds = new Rect();
                paint.GetTextBounds(text, 0, text.Length, bounds);

                float dx = 0;
                if (halign == HorizontalAlignment.Center)
                {
                    dx = -bounds.Width() / 2;
                }

                if (halign == HorizontalAlignment.Right)
                {
                    dx = -bounds.Width();
                }

                float dy = 0;
                if (valign == VerticalAlignment.Middle)
                {
                    dy = +bounds.Height() / 2;
                }

                if (valign == VerticalAlignment.Top)
                {
                    dy = bounds.Height();
                }

                canvas.Save();
                canvas.Translate(dx, dy);
                canvas.Rotate((float)rotate);
                canvas.Translate((float)p.X, (float)p.Y);
                canvas.DrawText(text, 0, 0, paint);
                canvas.Restore();
            }
        }

        public OxySize MeasureText(string text, string fontFamily = null, double fontSize = 10, double fontWeight = 500)
        {
            if (string.IsNullOrEmpty(text))
            {
                return OxySize.Empty;
            }

            using (var paint = new Paint())
            {
                paint.AntiAlias = true;
                paint.TextSize = (float)fontSize;
                var bounds = new Rect();
                paint.GetTextBounds(text, 0, text.Length, bounds);
                // var width = paint.MeasureText(text);
                return new OxySize(bounds.Width(), bounds.Height());
            }
        }

        /// <summary>
        /// Sets the tool tip for the following items.
        /// </summary>
        /// <param name="text">The text in the tooltip.</param>
        /// <params>
        /// This is only used in the plot controls.
        /// </params>
        public void SetToolTip(string text)
        {
            // TODO
        }

        /// <summary>
        /// Cleans up resources not in use.
        /// </summary>
        /// <remarks>
        /// This method is called at the end of each rendering.
        /// </remarks>
        public void CleanUp()
        {
            // TODO
        }

        /// <summary>
        /// Gets the size of the specified image.
        /// </summary>
        /// <param name="source">The image source.</param>
        /// <returns>
        /// The image info.
        /// </returns>
        public OxyImageInfo GetImageInfo(OxyImage source)
        {
            // TODO
            return null;
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
        /// <param name="interpolate">interpolate if set to <c>true</c>.</param>
        public void DrawImage(
            OxyImage source,
            uint srcX,
            uint srcY,
            uint srcWidth,
            uint srcHeight,
            double destX,
            double destY,
            double destWidth,
            double destHeight,
            double opacity,
            bool interpolate)
        {
            // TODO
        }

        /// <summary>
        /// Sets the clip rectangle.
        /// </summary>
        /// <param name="rect">The clip rectangle.</param>
        /// <returns>
        /// True if the clip rectangle was set.
        /// </returns>
        public bool SetClip(OxyRect rect)
        {
            // TODO
            return false;
        }

        /// <summary>
        /// Resets the clip rectangle.
        /// </summary>
        public void ResetClip()
        {
            // TODO
        }
    }
}