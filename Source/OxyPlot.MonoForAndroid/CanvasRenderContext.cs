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
        /// <param name="canvas">The canvas.</param>
        public CanvasRenderContext(Canvas canvas)
        {
            this.canvas = canvas;
            this.PaintBackground = true;
            using (var bounds = new Rect())
            {
                this.canvas.GetClipBounds(bounds);
                this.Width = bounds.Right - bounds.Left;
                this.Height = bounds.Bottom - bounds.Top;
            }
        }

        /// <summary>
        /// Gets the height of the rendering area.
        /// </summary>
        /// <value></value>
        public double Height { get; set; }

        public bool PaintBackground { get; set; }

        public double Width { get; set; }

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

        public void DrawText(ScreenPoint p, string text, OxyColor fill, string fontFamily = null, double fontSize = 10, double fontWeight = 500, double rotate = 0, HorizontalTextAlign halign = HorizontalTextAlign.Left, VerticalTextAlign valign = VerticalTextAlign.Top, OxySize? maxSize = new OxySize?())
        {
            using (var paint = new Paint())
            {
                paint.AntiAlias = true;
                paint.TextSize = (float)fontSize;
                paint.Color = fill.ToColor();
                var bounds = new Rect();
                paint.GetTextBounds(text, 0, text.Length, bounds);

                float dx = 0;
                if (halign == HorizontalTextAlign.Center)
                {
                    dx = -bounds.Width() / 2;
                }

                if (halign == HorizontalTextAlign.Right)
                {
                    dx = -bounds.Width();
                }

                float dy = 0;
                if (valign == VerticalTextAlign.Middle)
                {
                    dy = +bounds.Height() / 2;
                }

                if (valign == VerticalTextAlign.Top)
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

        public void SetToolTip(string text)
        {
        }
    }
}