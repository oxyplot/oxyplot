// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphicsRenderContext.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using Xwt;
using Xwt.Drawing;

namespace OxyPlot.Xwt
{
    /// <summary>
    /// The Xwt graphics render context.
    /// </summary>
    public class GraphicsRenderContext : RenderContextBase, IDisposable
    {
        /// <summary>
        /// The image cache.
        /// </summary>
        readonly Dictionary<OxyImage, Image> imageCache = new Dictionary<OxyImage, Image> ();

        /// <summary>
        /// The images in use.
        /// </summary>
        readonly HashSet<OxyImage> imagesInUse = new HashSet<OxyImage> ();

        /// <summary>
        /// The fonts cache.
        /// </summary>
        readonly Dictionary<string, Font> fonts = new Dictionary<string, Font> ();

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public Context Context {
            get;
            set;
        }

        /// <summary>
        /// Draws an ellipse.
        /// </summary>
        /// <param name="rect">The rectangle defining the extents of the ellipse.</param>
        /// <param name="fill">The fill color. If set to <c>OxyColors.Undefined</c>, the extents will not be filled.</param>
        /// <param name="stroke">The stroke color. If set to <c>OxyColors.Undefined</c>, the extents will not be stroked.</param>
        /// <param name="thickness">The thickness (in device independent units, 1/96 inch).</param>
        public override void DrawEllipse (OxyRect rect,
                                    OxyColor fill,
                                    OxyColor stroke,
                                    double thickness)
        {
            var ex = rect.Left + (rect.Width / 2.0);
            var ey = rect.Top + (rect.Height / 2.0);


            if (fill.IsVisible ()) {
                Context.Save ();
                Context.SetColor (fill.ToXwtColor ());

                Context.Translate (ex, ey);
                Context.Scale (1.0, -rect.Height / rect.Width);
                Context.Arc (0.0, 0.0, rect.Width / 2.0, 0, 360);

                Context.ClosePath ();
                Context.Fill ();
                Context.Restore ();
            }

            if (stroke.IsVisible () && thickness > 0) {
                Context.Save ();
                Context.SetColor (stroke.ToXwtColor ());
                Context.SetLineWidth (thickness);

                Context.Translate (ex, ey);
                Context.Scale (1.0, -rect.Height / rect.Width);
                Context.Arc (0.0, 0.0, rect.Width / 2.0, 0, 360);

                Context.ClosePath ();
                Context.Stroke ();
                Context.Restore ();
            }
        }

        /// <summary>
        /// Draws a polyline.
        /// </summary>
        /// <param name="points">The points defining the polyline.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The stroke thickness (in device independent units, 1/96 inch).</param>
        /// <param name="dashArray">The dash array (in device independent units, 1/96 inch). Use <c>null</c> to get a solid line.</param>
        /// <param name="lineJoin">The line join type.</param>
        /// <param name="aliased">if set to <c>true</c> the shape will be aliased.</param>
        public override void DrawLine (IList<ScreenPoint> points,
                                 OxyColor stroke,
                                 double thickness,
                                 double[] dashArray,
                                 LineJoin lineJoin,
                                 bool aliased)
        {
            if (stroke.IsVisible () && thickness > 0 && points.Count >= 2) {
                // g.SmoothingMode = aliased ? SmoothingMode.None : SmoothingMode.HighQuality; // TODO: Smoothing modes
                Context.Save ();
                Context.SetColor (stroke.ToXwtColor ());
                //Context.Li = lineJoin.ToLineJoin();
                Context.SetLineWidth (thickness);
                if (dashArray != null)
					Context.SetLineDash (0, Array.ConvertAll (dashArray, x => x * thickness));

                Context.MoveTo (points [0].ToXwtPoint (aliased));
                foreach (var point in points.Skip(1)) {
                    Context.LineTo (point.ToXwtPoint (aliased));
                }

                Context.Stroke ();
                Context.Restore ();
            }
        }

        /// <summary>
        /// Draws a polygon.
        /// </summary>
        /// <param name="points">The points defining the polygon.</param>
        /// <param name="fill">The fill color. If set to <c>OxyColors.Undefined</c>, the polygon will not be filled.</param>
        /// <param name="stroke">The stroke color. If set to <c>OxyColors.Undefined</c>, the polygon will not be stroked.</param>
        /// <param name="thickness">The stroke thickness (in device independent units, 1/96 inch).</param>
        /// <param name="dashArray">The dash array (in device independent units, 1/96 inch).</param>
        /// <param name="lineJoin">The line join type.</param>
        /// <param name="aliased">If set to <c>true</c> the polygon will be aliased.</param>
        public override void DrawPolygon (IList<ScreenPoint> points,
                                    OxyColor fill,
                                    OxyColor stroke,
                                    double thickness,
                                    double[] dashArray,
                                    LineJoin lineJoin,
                                    bool aliased)
        {
            if (fill.IsVisible () && points.Count >= 2) {
                // g.SmoothingMode = aliased ? SmoothingMode.None : SmoothingMode.HighQuality; // TODO: Smoothing modes
                Context.Save ();
                Context.SetColor (fill.ToXwtColor ());
                //Context.LineJoin = lineJoin.ToLineJoin();
				Context.SetLineWidth (thickness);
                if (dashArray != null)
					Context.SetLineDash (0, Array.ConvertAll (dashArray, x => x * thickness));

                Context.MoveTo (points [0].ToXwtPoint (aliased));
                foreach (var point in points.Skip(1)) {
                    Context.LineTo (point.ToXwtPoint (aliased));
                }

                // g.LineTo(points[0].ToPointD(aliased));
                Context.ClosePath ();
                Context.Fill ();
                Context.Restore ();
            }

            if (stroke.IsVisible () && thickness > 0 && points.Count >= 2) {
                // g.SmoothingMode = aliased ? SmoothingMode.None : SmoothingMode.HighQuality; // TODO: Smoothing modes
                Context.Save ();
                Context.SetColor (stroke.ToXwtColor ());
                //Context.LineJoin = lineJoin.ToLineJoin();
                Context.SetLineWidth (thickness);
                if (dashArray != null)
					Context.SetLineDash (0, Array.ConvertAll (dashArray, x => x * thickness));

                Context.MoveTo (points [0].ToXwtPoint (aliased));
                foreach (var point in points.Skip(1)) {
                    Context.LineTo (point.ToXwtPoint (aliased));
                }

                Context.ClosePath ();
                Context.Stroke ();
                Context.Restore ();
            }
        }

        /// <summary>
        /// Draws a rectangle.
        /// </summary>
        /// <param name="rect">The rectangle to draw.</param>
        /// <param name="fill">The fill color. If set to <c>OxyColors.Undefined</c>, the rectangle will not be filled.</param>
        /// <param name="stroke">The stroke color. If set to <c>OxyColors.Undefined</c>, the rectangle will not be stroked.</param>
        /// <param name="thickness">The stroke thickness (in device independent units, 1/96 inch).</param>
        public override void DrawRectangle (OxyRect rect,
                                      OxyColor fill,
                                      OxyColor stroke,
                                      double thickness)
        {
            if (fill.IsVisible ()) {
                Context.Save ();
                Context.Rectangle (rect.ToXwtRect (false));
                Context.SetColor (fill.ToXwtColor ());
                Context.Fill ();
                Context.Restore ();
            }

            if (stroke.IsVisible () && thickness > 0) {
                Context.Save ();
                Context.SetColor (stroke.ToXwtColor ());
                Context.SetLineWidth (thickness);
                Context.Rectangle (rect.ToXwtRect (false));
                Context.Stroke ();
                Context.Restore ();
            }
        }

        /// <summary>
        /// Draws text.
        /// </summary>
        /// <param name="p">The position.</param>
        /// <param name="text">The text.</param>
        /// <param name="fill">The text color.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">Size of the font (in device independent units, 1/96 inch).</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <param name="rotate">The rotation angle.</param>
        /// <param name="halign">The horizontal alignment.</param>
        /// <param name="valign">The vertical alignment.</param>
        /// <param name="maxSize">The maximum size of the text (in device independent units, 1/96 inch).</param>
        public override void DrawText (ScreenPoint p,
                                 string text,
                                 OxyColor fill,
                                 string fontFamily,
                                 double fontSize,
                                 double fontWeight,
                                 double rotate,
                                 HorizontalAlignment halign,
                                 VerticalAlignment valign,
                                 OxySize? maxSize)
        {
            Context.Save ();

            var layout = new TextLayout ();
            layout.Font = GetCachedFont (fontFamily, fontSize, fontWeight);
            layout.Text = text;

            var size = layout.GetSize ();
            if (maxSize != null) {
                size.Width = Math.Min (size.Width, maxSize.Value.Width);
                size.Height = Math.Min (size.Height, maxSize.Value.Height);
            }

            double dx = 0;
            if (halign == HorizontalAlignment.Center) {
                dx = -size.Width / 2;
            }

            if (halign == HorizontalAlignment.Right) {
                dx = -size.Width;
            }

            double dy = 0;
            if (valign == VerticalAlignment.Middle) {
                dy = -size.Height / 2;
            }

            if (valign == VerticalAlignment.Bottom) {
                dy = -size.Height;
            }

            Context.Translate (p.X, p.Y);
            if (Math.Abs (rotate) > double.Epsilon) {
                Context.Rotate (rotate);
            }

            Context.Translate (dx, dy);

            Context.SetColor (fill.ToXwtColor ());
            Context.DrawTextLayout (layout, 0, 0);

            Context.Restore ();
        }

        /// <summary>
        /// Measures the size of the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">Size of the font (in device independent units, 1/96 inch).</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <returns>The size of the text (in device independent units, 1/96 inch).</returns>
        public override OxySize MeasureText (string text,
                                       string fontFamily,
                                       double fontSize,
                                       double fontWeight)
        {
            if (text == null)
                return OxySize.Empty;

            var layout = new TextLayout ();
            layout.Font = GetCachedFont (fontFamily, fontSize, fontWeight);
            layout.Text = text;

            var size = layout.GetSize ();
            return new OxySize (size.Width, size.Height);
        }

        /// <summary>
        /// Cleans up resources not in use.
        /// </summary>
        /// <remarks>This method is called at the end of each rendering.</remarks>
        public override void CleanUp ()
        {
            var imagesToRelease = imageCache.Keys.Where (i => !imagesInUse.Contains (i)).ToList ();
            foreach (var i in imagesToRelease) {
                var image = GetImage (i);
                image.Dispose ();
                imageCache.Remove (i);
            }

            imagesInUse.Clear ();
        }

        /// <summary>
        /// Draws a portion of the specified <see cref="OxyImage" />.
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
        public override void DrawImage (OxyImage source,
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
            var image = GetImage (source);
            if (image != null) {
                Context.Save ();
                var sourceRect = new Rectangle (srcX, srcY, srcWidth, srcHeight);
                var destRect = new Rectangle (destX, destY, destWidth, destHeight);
                Context.DrawImage (image, sourceRect, destRect, opacity);
                Context.Fill ();
                Context.Restore ();
            }
        }

        /// <summary>
        /// Sets the clipping rectangle.
        /// </summary>
        /// <param name="rect">The clipping rectangle.</param>
        /// <returns><c>true</c> if the clip rectangle was set.</returns>
        public override bool SetClip (OxyRect rect)
        {
            Context.Save ();
            Context.Rectangle (rect.Left, rect.Top, rect.Width, rect.Height);
            Context.Clip ();
            return true;
        }

        /// <summary>
        /// Resets the clipping rectangle.
        /// </summary>
        public override void ResetClip ()
        {
            Context.Restore ();
        }

        /// <summary>
        /// Gets the specified font from cache.
        /// </summary>
        /// <returns>The font.</returns>
        /// <param name="fontFamily">Font family name.</param>
        /// <param name="fontSize">Font size.</param>
        /// <param name="fontWeight">Font weight.</param>
        Font GetCachedFont (string fontFamily, double fontSize, double fontWeight)
        {
			if (String.IsNullOrEmpty (fontFamily))
				fontFamily = Font.SystemFont.Family;

			var key = fontFamily + ' ' + GetFontWeight(fontWeight) + ' ' + fontSize.ToString ("0.###");

            Font font;
			if (!fonts.TryGetValue (key, out font))
				fonts [key] = Font.FromName (fontFamily).WithWeight (GetFontWeight (fontWeight)).WithSize (fontSize);

			return fonts [key];
        }

		/// <summary>
		/// Gets the Xwt font weight.
		/// </summary>
		/// <returns>The font weight.</returns>
		/// <param name="weight">the numeric font weight.</param>
		FontWeight GetFontWeight (double weight)
		{
			if (weight < 300)
				return FontWeight.Ultralight;
			if (weight < 400)
				return FontWeight.Light;
			if (weight < 600)
				return FontWeight.Normal;
			if (weight < 700)
				return FontWeight.Semibold;
			if (weight < 800)
				return FontWeight.Bold;
			return FontWeight.Ultrabold;
		}

        /// <summary>
        /// Gets the cached <see cref="Image" /> of the specified <see cref="OxyImage" />.
        /// </summary>
        /// <param name="source">The source image.</param>
        /// <returns>The <see cref="Image" />.</returns>
        private Image GetImage (OxyImage source)
        {
            if (source == null)
                return null;

            if (!this.imagesInUse.Contains (source))
                this.imagesInUse.Add (source);

            Image src;
            if (this.imageCache.TryGetValue (source, out src))
                return src;


            Image btm;
            using (var ms = new System.IO.MemoryStream (source.GetData ())) {
                btm = Image.FromStream (ms);
            }

            imageCache.Add (source, btm);
            return btm;
        }

        /// <summary>
        /// Releases all resource used by the <see cref="OxyPlot.Xwt.GraphicsRenderContext"/> object.
        /// </summary>
        public void Dispose ()
        {
            foreach (var image in this.imageCache.Values) {
                image.Dispose ();
            }
        }
    }
}

