//-----------------------------------------------------------------------
// <copyright file="RenderContextBase.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

namespace OxyPlot
{
    using System.Collections.Generic;

    /// <summary>
    /// The render context base.
    /// </summary>
    public abstract class RenderContextBase : IRenderContext
    {
        #region Public Properties

        /// <summary>
        ///   Gets or sets Height.
        /// </summary>
        public double Height { get; protected set; }

        /// <summary>
        ///   Gets or sets a value indicating whether PaintBackground.
        /// </summary>
        public bool PaintBackground { get; protected set; }

        /// <summary>
        ///   Gets or sets Width.
        /// </summary>
        public double Width { get; protected set; }

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
        public abstract void DrawEllipse(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness);

        /// <summary>
        /// The draw ellipses.
        /// </summary>
        /// <param name="rectangles">
        /// The rectangles.
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
        public virtual void DrawEllipses(IList<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness)
        {
            foreach (OxyRect r in rectangles)
            {
                this.DrawEllipse(r, fill, stroke, thickness);
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
        public abstract void DrawLine(
            IList<ScreenPoint> points, 
            OxyColor stroke, 
            double thickness, 
            double[] dashArray, 
            OxyPenLineJoin lineJoin, 
            bool aliased);

        /// <summary>
        /// The draw line segments.
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
        public virtual void DrawLineSegments(
            IList<ScreenPoint> points, 
            OxyColor stroke, 
            double thickness, 
            double[] dashArray, 
            OxyPenLineJoin lineJoin, 
            bool aliased)
        {
            for (int i = 0; i + 1 < points.Count; i += 2)
            {
                this.DrawLine(new[] { points[i], points[i + 1] }, stroke, thickness, dashArray, lineJoin, aliased);
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
        public abstract void DrawPolygon(
            IList<ScreenPoint> points, 
            OxyColor fill, 
            OxyColor stroke, 
            double thickness, 
            double[] dashArray, 
            OxyPenLineJoin lineJoin, 
            bool aliased);

        /// <summary>
        /// The draw polygons.
        /// </summary>
        /// <param name="polygons">
        /// The polygons.
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
        public virtual void DrawPolygons(
            IList<IList<ScreenPoint>> polygons, 
            OxyColor fill, 
            OxyColor stroke, 
            double thickness, 
            double[] dashArray, 
            OxyPenLineJoin lineJoin, 
            bool aliased)
        {
            foreach (var polygon in polygons)
            {
                this.DrawPolygon(polygon, fill, stroke, thickness, dashArray, lineJoin, aliased);
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
        public abstract void DrawRectangle(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness);

        /// <summary>
        /// The draw rectangles.
        /// </summary>
        /// <param name="rectangles">
        /// The rectangles.
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
        public virtual void DrawRectangles(IList<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness)
        {
            foreach (OxyRect r in rectangles)
            {
                this.DrawRectangle(r, fill, stroke, thickness);
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
        public abstract void DrawText(
            ScreenPoint p, 
            string text, 
            OxyColor fill, 
            string fontFamily, 
            double fontSize, 
            double fontWeight, 
            double rotate, 
            HorizontalTextAlign halign, 
            VerticalTextAlign valign);

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
        public abstract OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight);

        #endregion
    }
}
