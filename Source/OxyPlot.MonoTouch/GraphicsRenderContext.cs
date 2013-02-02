// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphicsRenderContext.cs" company="OxyPlot">
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
//   The graphics render context.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.MonoGame
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using OxyPlot;

	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Content;
	using Microsoft.Xna.Framework.Graphics;
	using Microsoft.Xna.Framework.Input;
	using Microsoft.Xna.Framework.Storage;

    /// <summary>
    /// The graphics render context.
    /// </summary>
    internal class GraphicsRenderContext : RenderContextBase
    {
        /// <summary>
        /// The fontsize factor.
        /// </summary>
        private const float FontsizeFactor = 0.8f;

        /// <summary>
        /// The GDI+ drawing surface.
        /// </summary>
        private readonly GraphicsDeviceManager graphics;

		private MonoGameRenderContext g;

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
        public GraphicsRenderContext()
        {
			this.g = new MonoGameRenderContext();
        }

        public void SetTarget(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
        }

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
                    ToBrush(fill), (float)rect.Left, (float)rect.Top, (float)rect.Width, (float)rect.Height);
            }

            if (stroke == null || thickness <= 0)
            {
                return;
            }

            var pen = new Pen(this.ToColor(stroke), (float)thickness);
            this.g.DrawEllipse(pen, (float)rect.Left, (float)rect.Top, (float)rect.Width, (float)rect.Height);
        }

		private Texture ToBrush(OxyColor fill)
        {
            if (fill != null)
            {
                //return new Texture( SolidBrush(this.ToColor(fill));
            }

            return null;
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
            if (stroke == null || thickness <= 0)
            {
                return;
            }

            this.g.SmoothingMode = aliased ? SmoothingMode.None : SmoothingMode.HighQuality;
            var pen = new Pen(this.ToColor(stroke), (float)thickness);

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
            this.g.SmoothingMode = aliased ? SmoothingMode.None : SmoothingMode.HighQuality;

            PointF[] pts = this.ToPoints(points);
            if (fill != null)
            {
                this.g.FillPolygon(this.ToBrush(fill), pts);
            }

            if (stroke != null && thickness > 0)
            {
                var pen = new Pen(this.ToColor(stroke), (float)thickness);

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

            var pen = new Pen(this.ToColor(stroke), (float)thickness);
            this.g.DrawRectangle(pen, (float)rect.Left, (float)rect.Top, (float)rect.Width, (float)rect.Height);
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
            FontStyle fs = FontStyle.Regular;
            if (fontWeight >= 700)
            {
                fs = FontStyle.Bold;
            }

            var font = new Font(fontFamily, (float)fontSize * FontsizeFactor, fs);

            var sf = new StringFormat { Alignment = StringAlignment.Near };

            SizeF size = this.g.MeasureString(text, font);
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

                // sf.Alignment = StringAlignment.Center;
            }

            if (halign == HorizontalTextAlign.Right)
            {
                dx = -size.Width;

                // sf.Alignment = StringAlignment.Far;
            }

            float dy = 0;
            sf.LineAlignment = StringAlignment.Near;
            if (valign == VerticalTextAlign.Middle)
            {
                // sf.LineAlignment = StringAlignment.Center;
                dy = -size.Height / 2;
            }

            if (valign == VerticalTextAlign.Bottom)
            {
                // sf.LineAlignment = StringAlignment.Far;
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

        /// <summary>
        /// The measure text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">The font size.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <returns></returns>
        public override OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight)
        {
            if (text == null)
            {
                return OxySize.Empty;
            }

            FontStyle fs = FontStyle.Regular;
            if (fontWeight >= 700)
            {
                fs = FontStyle.Bold;
            }

            var font = new Font(fontFamily, (float)fontSize * FontsizeFactor, fs);
            SizeF size = this.g.MeasureString(text, font);
            return new OxySize(size.Width, size.Height);
        }

        /// <summary>
        /// The to color.
        /// </summary>
        /// <param name="c">
        /// The c.
        /// </param>
        /// <returns>
        /// </returns>
        private Color ToColor(OxyColor c)
        {
            return Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        /// <summary>
        /// The to float array.
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <returns>
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
        /// The to points.
        /// </summary>
        /// <param name="points">
        /// The points.
        /// </param>
        /// <returns>
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
   }
}