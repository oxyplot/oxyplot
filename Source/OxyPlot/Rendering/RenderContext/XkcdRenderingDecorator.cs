// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XkcdRenderingDecorator.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
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
//   Provides 'XKCD style' to an IRenderContext.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides 'XKCD style' to an <see cref="IRenderContext" />.
    /// </summary>
    /// <remarks>See <a href="http://matplotlib.org/xkcd/">Matplotlib</a> and <a href="http://xkcd.com/">xkcd</a>.</remarks>
    public class XkcdRenderingDecorator : RenderContextBase
    {
        /// <summary>
        /// The decorated <see cref="IRenderContext" />. This is the one that does the actual rendering.
        /// </summary>
        private readonly IRenderContext rc;

        /// <summary>
        /// The random number generator.
        /// </summary>
        private readonly Random r = new Random(0);

        /// <summary>
        /// Initializes a new instance of the <see cref="XkcdRenderingDecorator"/> class.
        /// </summary>
        /// <param name="rc">The decorated render context.</param>
        public XkcdRenderingDecorator(IRenderContext rc)
        {
            this.rc = rc;
            this.RendersToScreen = this.rc.RendersToScreen;
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
        public override void DrawLine(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness,
            double[] dashArray,
            OxyPenLineJoin lineJoin,
            bool aliased)
        {
            var xckdPoints = this.Xkcdify(points);
            this.rc.DrawLine(xckdPoints, stroke, thickness, dashArray, lineJoin);
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
        public override void DrawPolygon(
            IList<ScreenPoint> points,
            OxyColor fill,
            OxyColor stroke,
            double thickness,
            double[] dashArray,
            OxyPenLineJoin lineJoin,
            bool aliased)
        {
            var xckdPoints = this.Xkcdify(points);
            this.rc.DrawPolygon(xckdPoints, fill, stroke, thickness, dashArray, lineJoin, aliased);
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
        public override void DrawText(
            ScreenPoint p,
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
            this.rc.DrawText(p, text, fill, this.GetFontFamily(fontFamily), fontSize, fontWeight, rotate, halign, valign, maxSize);
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
            return this.rc.MeasureText(text, this.GetFontFamily(fontFamily), fontSize, fontWeight);
        }

        /// <summary>
        /// Sets the tool tip for the following items.
        /// </summary>
        /// <param name="text">The text in the tooltip.</param>
        public override void SetToolTip(string text)
        {
            this.rc.SetToolTip(text);
        }

        /// <summary>
        /// Cleans up resources not in use.
        /// </summary>
        /// <remarks>
        /// This method is called at the end of each rendering.
        /// </remarks>
        public override void CleanUp()
        {
            this.rc.CleanUp();
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
        /// <param name="interpolate">Interpolate if set to <c>true</c>.</param>
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
            this.rc.DrawImage(source, srcX, srcY, srcWidth, srcHeight, destX, destY, destWidth, destHeight, opacity, interpolate);
        }

        /// <summary>
        /// Sets the clipping rectangle.
        /// </summary>
        /// <param name="clippingRect">The clipping rectangle.</param>
        /// <returns>
        ///   <c>true</c> if the clip rectangle was set.
        /// </returns>
        public override bool SetClip(OxyRect clippingRect)
        {
            return this.rc.SetClip(clippingRect);
        }

        /// <summary>
        /// Resets the clip rectangle.
        /// </summary>
        public override void ResetClip()
        {
            this.rc.ResetClip();
        }

        /// <summary>
        /// Gets the xkcdified font family name.
        /// </summary>
        /// <param name="fontFamily">The original font family.</param>
        /// <returns>The actual font family.</returns>
        private string GetFontFamily(string fontFamily)
        {
            // google for 'humor-sans.ttf'
            return "Humor Sans";
        }

        /// <summary>
        /// Xkcdifies the specified points.
        /// </summary>
        /// <param name="points">The points to xkcdify.</param>
        /// <returns>The xkcdified points.</returns>
        private ScreenPoint[] Xkcdify(IList<ScreenPoint> points)
        {
            // See the Mathematica / Matplotlib solutions
            // http://jakevdp.github.io/blog/2012/10/07/xkcd-style-plots-in-matplotlib/
            // http://jakevdp.github.io/blog/2013/07/10/XKCD-plots-in-matplotlib/
            // http://mathematica.stackexchange.com/questions/11350/xkcd-style-graphs
            // http://www.mail-archive.com/matplotlib-users@lists.sourceforge.net/msg25499.html
            // http://nbviewer.ipython.org/gist/anonymous/3835181

            // TODO: this is where the points should be xkcdified
            // The following code is just to show that some randomness is working - this must be improved
            var result = new ScreenPoint[points.Count];
            double d = 5;
            double d2 = d / 2;
            for (int i = 0; i < points.Count; i++)
            {
                var delta = new ScreenVector((r.NextDouble() * d) - d2, (r.NextDouble() * d) - d2);
                result[i] = points[i] + delta;
            }

            return result;
        }
    }
}