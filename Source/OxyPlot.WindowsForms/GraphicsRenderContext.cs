// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphicsRenderContext.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.WindowsForms
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Linq;

    using OxyPlot;

    /// <summary>
    /// The graphics render context.
    /// </summary>
    internal class GraphicsRenderContext : RenderContextBase
    {
        #region Constants and Fields

        /// <summary>
        ///   The fontsize factor.
        /// </summary>
        private const float FontsizeFactor = 0.8f;

        /// <summary>
        ///   The GDI+ drawing surface.
        /// </summary>
        private readonly Graphics g;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsRenderContext"/> class.
        /// </summary>
        /// <param name="graphics">
        /// The graphics.
        /// </param>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        public GraphicsRenderContext(Graphics graphics, double width, double height)
        {
            this.Width = width;
            this.Height = height;
            this.PaintBackground = true;
            this.g = graphics;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The draw ellipse.
        /// </summary>
        /// <param name="rect">
        /// The rect.
        /// </param>
        /// <param name="fill">
        /// The fill.
        /// </param>
        /// <param name="stroke">
        /// The stroke.
        /// </param>
        /// <param name="thickness">
        /// The thickness.
        /// </param>
        public override void DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
        {
            if (fill != null)
            {
                this.g.FillEllipse(
                    this.ToBrush(fill), (float)rect.Left, (float)rect.Top, (float)rect.Width, (float)rect.Height);
            }

            if (stroke == null || thickness <= 0)
            {
                return;
            }

            using (var pen = new Pen(this.ToColor(stroke), (float)thickness))
            {
                this.g.DrawEllipse(pen, (float)rect.Left, (float)rect.Top, (float)rect.Width, (float)rect.Height);
            }
        }

        /// <summary>
        /// The draw line.
        /// </summary>
        /// <param name="points">
        /// The points.
        /// </param>
        /// <param name="stroke">
        /// The stroke.
        /// </param>
        /// <param name="thickness">
        /// The thickness.
        /// </param>
        /// <param name="dashArray">
        /// The dash array.
        /// </param>
        /// <param name="lineJoin">
        /// The line join.
        /// </param>
        /// <param name="aliased">
        /// The aliased.
        /// </param>
        public override void DrawLine(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness,
            double[] dashArray,
            OxyPenLineJoin lineJoin,
            bool aliased)
        {
            if (stroke == null || thickness <= 0 || points.Count < 2)
            {
                return;
            }

            this.g.SmoothingMode = aliased ? SmoothingMode.None : SmoothingMode.HighQuality;
            using (var pen = new Pen(this.ToColor(stroke), (float)thickness))
            {

                if (dashArray != null)
                {
                    pen.DashPattern = this.ToFloatArray(dashArray);
                }

                switch (lineJoin)
                {
                    case OxyPenLineJoin.Round:
                        pen.LineJoin = LineJoin.Round;
                        break;
                    case OxyPenLineJoin.Bevel:
                        pen.LineJoin = LineJoin.Bevel;
                        break;

                        // The default LineJoin is Miter
                }

                this.g.DrawLines(pen, this.ToPoints(points));
            }
        }

        /// <summary>
        /// The draw polygon.
        /// </summary>
        /// <param name="points">
        /// The points.
        /// </param>
        /// <param name="fill">
        /// The fill.
        /// </param>
        /// <param name="stroke">
        /// The stroke.
        /// </param>
        /// <param name="thickness">
        /// The thickness.
        /// </param>
        /// <param name="dashArray">
        /// The dash array.
        /// </param>
        /// <param name="lineJoin">
        /// The line join.
        /// </param>
        /// <param name="aliased">
        /// The aliased.
        /// </param>
        public override void DrawPolygon(
            IList<ScreenPoint> points,
            OxyColor fill,
            OxyColor stroke,
            double thickness,
            double[] dashArray,
            OxyPenLineJoin lineJoin,
            bool aliased)
        {
            if (points.Count < 2)
            {
                return;
            }

            this.g.SmoothingMode = aliased ? SmoothingMode.None : SmoothingMode.HighQuality;

            PointF[] pts = this.ToPoints(points);
            if (fill != null)
            {
                this.g.FillPolygon(this.ToBrush(fill), pts);
            }

            if (stroke != null && thickness > 0)
            {
                using (var pen = new Pen(this.ToColor(stroke), (float)thickness))
                {

                    if (dashArray != null)
                    {
                        pen.DashPattern = this.ToFloatArray(dashArray);
                    }

                    switch (lineJoin)
                    {
                        case OxyPenLineJoin.Round:
                            pen.LineJoin = LineJoin.Round;
                            break;
                        case OxyPenLineJoin.Bevel:
                            pen.LineJoin = LineJoin.Bevel;
                            break;

                            // The default LineJoin is Miter
                    }

                    this.g.DrawPolygon(pen, pts);
                }
            }
        }

        /// <summary>
        /// The draw rectangle.
        /// </summary>
        /// <param name="rect">
        /// The rect.
        /// </param>
        /// <param name="fill">
        /// The fill.
        /// </param>
        /// <param name="stroke">
        /// The stroke.
        /// </param>
        /// <param name="thickness">
        /// The thickness.
        /// </param>
        public override void DrawRectangle(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness)
        {
            if (fill != null)
            {
                this.g.FillRectangle(
                    this.ToBrush(fill), (float)rect.Left, (float)rect.Top, (float)rect.Width, (float)rect.Height);
            }

            if (stroke == null || thickness <= 0)
            {
                return;
            }

            using (var pen = new Pen(this.ToColor(stroke), (float)thickness))
            {
                this.g.DrawRectangle(pen, (float)rect.Left, (float)rect.Top, (float)rect.Width, (float)rect.Height);
            }
        }

        /// <summary>
        /// The draw text.
        /// </summary>
        /// <param name="p">
        /// The p.
        /// </param>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <param name="fill">
        /// The fill.
        /// </param>
        /// <param name="fontFamily">
        /// The font family.
        /// </param>
        /// <param name="fontSize">
        /// The font size.
        /// </param>
        /// <param name="fontWeight">
        /// The font weight.
        /// </param>
        /// <param name="rotate">
        /// The rotate.
        /// </param>
        /// <param name="halign">
        /// The halign.
        /// </param>
        /// <param name="valign">
        /// The valign.
        /// </param>
        /// <param name="maxSize">
        /// The maximum size of the text.
        /// </param>
        public override void DrawText(
            ScreenPoint p,
            string text,
            OxyColor fill,
            string fontFamily,
            double fontSize,
            double fontWeight,
            double rotate,
            HorizontalTextAlign halign,
            VerticalTextAlign valign,
            OxySize? maxSize)
        {
            var fs = FontStyle.Regular;
            if (fontWeight >= 700)
            {
                fs = FontStyle.Bold;
            }

            using (var font = new Font(fontFamily, (float)fontSize * FontsizeFactor, fs))
            {
                using (var sf = new StringFormat { Alignment = StringAlignment.Near })
                {
                    var size = this.g.MeasureString(text, font);
                    if (maxSize != null)
                    {
                        if (size.Width > maxSize.Value.Width)
                        {
                            size.Width = (float)maxSize.Value.Width;
                        }

                        if (size.Height > maxSize.Value.Height)
                        {
                            size.Height = (float)maxSize.Value.Height;
                        }
                    }

                    float dx = 0;
                    if (halign == HorizontalTextAlign.Center)
                    {
                        dx = -size.Width / 2;
                    }

                    if (halign == HorizontalTextAlign.Right)
                    {
                        dx = -size.Width;
                    }

                    float dy = 0;
                    sf.LineAlignment = StringAlignment.Near;
                    if (valign == VerticalTextAlign.Middle)
                    {
                        dy = -size.Height / 2;
                    }

                    if (valign == VerticalTextAlign.Bottom)
                    {
                        dy = -size.Height;
                    }

                    this.g.TranslateTransform((float)p.X, (float)p.Y);
                    if (Math.Abs(rotate) > double.Epsilon)
                    {
                        this.g.RotateTransform((float)rotate);
                    }

                    this.g.TranslateTransform(dx, dy);

                    var layoutRectangle = new RectangleF(0, 0, size.Width, size.Height);
                    this.g.DrawString(text, font, this.ToBrush(fill), layoutRectangle, sf);

                    this.g.ResetTransform();
                }
            }
        }

        /// <summary>
        /// The measure text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">The font size.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <returns>The size of the text.</returns>
        public override OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight)
        {
            if (text == null)
            {
                return OxySize.Empty;
            }

            var fs = FontStyle.Regular;
            if (fontWeight >= 700)
            {
                fs = FontStyle.Bold;
            }

            using (var font = new Font(fontFamily, (float)fontSize * FontsizeFactor, fs))
            {
                var size = this.g.MeasureString(text, font);
                return new OxySize(size.Width, size.Height);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts a fill color to a System.Drawing.Brush.
        /// </summary>
        /// <param name="fill">
        /// The fill color.
        /// </param>
        /// <returns>
        /// The brush.
        /// </returns>
        private Brush ToBrush(OxyColor fill)
        {
            if (fill != null)
            {
                return new SolidBrush(this.ToColor(fill));
            }

            return null;
        }

        /// <summary>
        /// Converts a color to a System.Drawing.Color.
        /// </summary>
        /// <param name="c">
        /// The color.
        /// </param>
        /// <returns>
        /// The System.Drawing.Color.
        /// </returns>
        private Color ToColor(OxyColor c)
        {
            return Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        /// <summary>
        /// Converts a double array to a float array.
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <returns>
        /// The float array.
        /// </returns>
        private float[] ToFloatArray(double[] a)
        {
            if (a == null)
            {
                return null;
            }

            var r = new float[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                r[i] = (float)a[i];
            }

            return r;
        }

        /// <summary>
        /// Converts a list of point to an array of PointF.
        /// </summary>
        /// <param name="points">
        /// The points.
        /// </param>
        /// <returns>
        /// An array of points.
        /// </returns>
        private PointF[] ToPoints(IList<ScreenPoint> points)
        {
            if (points == null)
            {
                return null;
            }

            var r = new PointF[points.Count()];
            int i = 0;
            foreach (ScreenPoint p in points)
            {
                r[i++] = new PointF((float)p.X, (float)p.Y);
            }

            return r;
        }

        #endregion
    }
}