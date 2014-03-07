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

namespace OxyPlot.XamarinAndroid
{
    using System.Collections.Generic;
    using System.Linq;

    using Android.Graphics;

    /// <summary>
    /// Provides a render context for Android.Graphics.Canvas.
    /// </summary>
    public class CanvasRenderContext : RenderContextBase
    {
        /// <summary>
        /// The images in use
        /// </summary>
        private readonly HashSet<OxyImage> imagesInUse = new HashSet<OxyImage>();

        /// <summary>
        /// The image cache
        /// </summary>
        private readonly Dictionary<OxyImage, Bitmap> imageCache = new Dictionary<OxyImage, Bitmap>();

        /// <summary>
        /// The current paint.
        /// </summary>
        private readonly Paint paint;

        /// <summary>
        /// A reusable path object.
        /// </summary>
        private readonly Path path;

        /// <summary>
        /// A reusable bounds rectangle.
        /// </summary>
        private readonly Rect bounds;

        /// <summary>
        /// A reusable list of points.
        /// </summary>
        private readonly List<float> pts;

        /// <summary>
        /// The canvas to draw on.
        /// </summary>
        private Canvas canvas;

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasRenderContext"/> class.
        /// </summary>
        public CanvasRenderContext()
        {
            this.paint = new Paint();
            this.path = new Path();
            this.bounds = new Rect();
            this.pts = new List<float>();
        }

        /// <summary>
        /// Sets the target.
        /// </summary>
        /// <param name="c">The canvas.</param>
        public void SetTarget(Canvas c)
        {
            this.canvas = c;
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
            this.paint.Reset();
            {
                if (fill.IsVisible())
                {
                    this.SetFill(fill);
                    this.canvas.DrawOval(rect.ToRectF(), this.paint);
                }

                if (stroke.IsVisible())
                {
                    this.SetStroke(stroke, thickness);
                    this.canvas.DrawOval(rect.ToRectF(), this.paint);
                }
            }
        }

        /// <summary>
        /// Draws the collection of ellipses, where all have the same stroke and fill.
        /// This performs better than calling DrawEllipse multiple times.
        /// </summary>
        /// <param name="rectangles">The rectangles.</param>
        /// <param name="fill">The fill color.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The stroke thickness.</param>
        public override void DrawEllipses(IList<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness)
        {
            this.paint.Reset();
            {
                this.paint.AntiAlias = true;
                this.paint.StrokeWidth = (float)thickness;
                foreach (var rect in rectangles)
                {
                    if (fill.IsVisible())
                    {
                        this.paint.SetStyle(Paint.Style.Fill);
                        this.paint.Color = fill.ToColor();
                        this.canvas.DrawOval(rect.ToRectF(), this.paint);
                    }

                    if (stroke.IsVisible())
                    {
                        this.paint.SetStyle(Paint.Style.Stroke);
                        this.paint.Color = stroke.ToColor();
                        this.canvas.DrawOval(rect.ToRectF(), this.paint);
                    }
                }
            }
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
            this.paint.Reset();
            {
                this.SetStroke(stroke, thickness, dashArray, lineJoin, aliased);
                this.pts.Clear();
                for (int i = 0; i + 1 < points.Count; i++)
                {
                    this.pts.Add((float)points[i].X);
                    this.pts.Add((float)points[i].Y);
                    this.pts.Add((float)points[i + 1].X);
                    this.pts.Add((float)points[i + 1].Y);
                }

                this.canvas.DrawLines(this.pts.ToArray(), this.paint);
            }
        }

        /// <summary>
        /// Draws multiple line segments defined by points (0,1) (2,3) (4,5) etc.
        /// This should have better performance than calling DrawLine for each segment.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The stroke thickness.</param>
        /// <param name="dashArray">The dash array.</param>
        /// <param name="lineJoin">The line join type.</param>
        /// <param name="aliased">If set to <c>true</c> the shape will be aliased.</param>
        public override void DrawLineSegments(IList<ScreenPoint> points, OxyColor stroke, double thickness, double[] dashArray, OxyPenLineJoin lineJoin, bool aliased)
        {
            this.paint.Reset();
            {
                this.SetStroke(stroke, thickness, dashArray, lineJoin, aliased);
                this.pts.Clear();
                foreach (var p in points)
                {
                    this.pts.Add((float)p.X);
                    this.pts.Add((float)p.Y);
                }

                this.canvas.DrawLines(this.pts.ToArray(), this.paint);
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
            this.paint.Reset();
            {
                this.paint.AntiAlias = !aliased;
                this.path.Reset();
                {
                    this.path.MoveTo((float)points[0].X, (float)points[0].Y);
                    for (int i = 1; i <= points.Count; i++)
                    {
                        this.path.LineTo((float)points[i % points.Count].X, (float)points[i % points.Count].Y);
                    }

                    if (fill.IsVisible())
                    {
                        this.SetFill(fill);
                        this.canvas.DrawPath(this.path, this.paint);
                    }

                    if (stroke.IsVisible())
                    {
                        this.SetStroke(stroke, thickness, dashArray, lineJoin, aliased);
                        this.canvas.DrawPath(this.path, this.paint);
                    }
                }
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
            this.paint.Reset();
            {
                this.paint.AntiAlias = false;
                if (fill.IsVisible())
                {
                    this.SetFill(fill);
                    this.canvas.DrawRect((float)rect.Left, (float)rect.Top, (float)rect.Right, (float)rect.Bottom, paint);
                }

                if (stroke.IsVisible())
                {
                    this.paint.SetStyle(Paint.Style.Stroke);
                    this.paint.Color = stroke.ToColor();
                    this.paint.StrokeWidth = (float)thickness;
                    this.canvas.DrawRect((float)rect.Left, (float)rect.Top, (float)rect.Right, (float)rect.Bottom, paint);
                }
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
            this.paint.Reset();
            {
                this.paint.AntiAlias = true;
                this.paint.TextSize = (float)fontSize;
                this.paint.Color = fill.ToColor();
                this.paint.GetTextBounds(text, 0, text.Length, this.bounds);

                double dx = 0;
                if (halign == HorizontalAlignment.Center)
                {
                    dx = -this.bounds.Width() * 0.5;
                }

                if (halign == HorizontalAlignment.Right)
                {
                    dx = -this.bounds.Width();
                }

                double dy = 0;
                if (valign == VerticalAlignment.Middle)
                {
                    dy = this.bounds.Height() * 0.5;
                }

                if (valign == VerticalAlignment.Top)
                {
                    dy = this.bounds.Height();
                }

                this.canvas.Save();
				this.canvas.Translate((float)p.X, (float)p.Y);
				this.canvas.Rotate((float)rotate);
                this.canvas.Translate((float)dx, (float)dy);
                this.canvas.DrawText(text, 0, 0, this.paint);
                this.canvas.Restore();
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
        /// The text size.
        /// </returns>
        public override OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight)
        {
            if (string.IsNullOrEmpty(text))
            {
                return OxySize.Empty;
            }

            this.paint.Reset();
            {
                this.paint.AntiAlias = true;
                this.paint.TextSize = (float)fontSize;
                this.paint.GetTextBounds(text, 0, text.Length, this.bounds);
                return new OxySize(this.bounds.Width(), this.bounds.Height());
            }
        }

        /// <summary>
        /// Sets the clip rectangle.
        /// </summary>
        /// <param name="rect">The clip rectangle.</param>
        /// <returns>
        /// True if the clip rectangle was set.
        /// </returns>
        public override bool SetClip(OxyRect rect)
        {
            this.canvas.Save();
            this.canvas.ClipRect(rect.ToRectF());
            return true;
        }

        /// <summary>
        /// Resets the clip rectangle.
        /// </summary>
        public override void ResetClip()
        {
            this.canvas.Restore();
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
            var image = this.GetImage(source);
            if (image == null)
            {
                return;
            }

            var src = new Rect((int)srcX, (int)srcY, (int)(srcX + srcWidth), (int)(srcY + srcHeight));
            var dest = new RectF((float)destX, (float)destY, (float)(destX + destWidth), (float)(destY + destHeight));

            this.paint.Reset();

            // TODO: support opacity
            this.canvas.DrawBitmap(image, src, dest, this.paint);
        }

        /// <summary>
        /// Cleans up resources not in use.
        /// </summary>
        /// <remarks>
        /// This method is called at the end of each rendering.
        /// </remarks>
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
        /// Sets the fill style.
        /// </summary>
        /// <param name="fill">The fill color.</param>
        private void SetFill(OxyColor fill)
        {
            this.paint.SetStyle(Paint.Style.Fill);
            this.paint.Color = fill.ToColor();
            this.paint.AntiAlias = true;
        }

        /// <summary>
        /// Sets the stroke style.
        /// </summary>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The stroke thickness.</param>
        /// <param name="dashArray">The dash array.</param>
        /// <param name="lineJoin">The line join.</param>
        /// <param name="aliased">Use aliased strokes if set to <c>true</c>.</param>
        private void SetStroke(OxyColor stroke, double thickness, double[] dashArray = null, OxyPenLineJoin lineJoin = OxyPenLineJoin.Miter, bool aliased = false)
        {
            this.paint.SetStyle(Paint.Style.Stroke);
            this.paint.Color = stroke.ToColor();
            this.paint.StrokeWidth = (float)thickness;
            this.paint.StrokeJoin = lineJoin.Convert();
            if (dashArray != null)
            {
                var dashArrayF = dashArray.Select(x => (float)x).ToArray();
                this.paint.SetPathEffect(new DashPathEffect(dashArrayF, 0f));
            }

            this.paint.AntiAlias = !aliased;
        }

        /// <summary>
        /// Gets the image from cache or creates a new <see cref="Bitmap"/>.
        /// </summary>
        /// <param name="source">The source image.</param>
        /// <returns>A <see cref="Bitmap"/>.</returns>
        private Bitmap GetImage(OxyImage source)
        {
            if (source == null)
            {
                return null;
            }

            if (!this.imagesInUse.Contains(source))
            {
                this.imagesInUse.Add(source);
            }

            Bitmap bitmap;
            if (!this.imageCache.TryGetValue(source, out bitmap))
            {
                var bytes = source.GetData();
                bitmap = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length);
                this.imageCache.Add(source, bitmap);
            }

            return bitmap;
        }
    }
}