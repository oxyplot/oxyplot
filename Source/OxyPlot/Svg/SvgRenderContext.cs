// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SvgRenderContext.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   The svg render context.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// The svg render context.
    /// </summary>
    public class SvgRenderContext : RenderContextBase, IDisposable
    {
        #region Constants and Fields

        /// <summary>
        /// The w.
        /// </summary>
        private readonly SvgWriter w;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgRenderContext"/> class.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        /// <param name="isDocument">
        /// The is document.
        /// </param>
        public SvgRenderContext(Stream s, double width, double height, bool isDocument)
        {
            this.w = new SvgWriter(s, width, height, isDocument);
            this.Width = width;
            this.Height = height;
            this.PaintBackground = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgRenderContext"/> class.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        public SvgRenderContext(string path, double width, double height)
        {
            this.w = new SvgWriter(path, width, height);
            this.Width = width;
            this.Height = height;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The close.
        /// </summary>
        public void Close()
        {
            this.w.Close();
        }

        /// <summary>
        /// The complete.
        /// </summary>
        public void Complete()
        {
            this.w.Complete();
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            this.w.Dispose();
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
            this.w.WriteEllipse(
                rect.Left, rect.Top, rect.Width, rect.Height, this.w.CreateStyle(fill, stroke, thickness, null));
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
            this.w.WritePolyline(points, this.w.CreateStyle(null, stroke, thickness, dashArray, lineJoin));
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
            this.w.WritePolygon(points, this.w.CreateStyle(fill, stroke, thickness, dashArray, lineJoin));
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
            this.w.WriteRectangle(
                rect.Left, rect.Top, rect.Width, rect.Height, this.w.CreateStyle(fill, stroke, thickness, null));
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
        /// <param name="c">
        /// The c.
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
        public override void DrawText(
            ScreenPoint p, 
            string text, 
            OxyColor c, 
            string fontFamily, 
            double fontSize, 
            double fontWeight, 
            double rotate, 
            HorizontalTextAlign halign, 
            VerticalTextAlign valign)
        {
            this.w.WriteText(p, text, c, fontFamily, fontSize, fontWeight, rotate, halign, valign);
        }

        /// <summary>
        /// The flush.
        /// </summary>
        public void Flush()
        {
            this.w.Flush();
        }

        /// <summary>
        /// The measure text.
        /// </summary>
        /// <param name="text">
        /// The text.
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
        /// <returns>
        /// </returns>
        public override OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight)
        {
            if (string.IsNullOrEmpty(text))
            {
                return OxySize.Empty;
            }

            // todo: should improve text measuring, currently using p/invoke on GDI32
            // is it better to use winforms or wpf text measuring?
            return Gdi32.MeasureString(fontFamily, (int)fontSize, (int)fontWeight, text);
        }

        #endregion
    }
}