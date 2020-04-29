// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XkcdRenderingDecorator.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a <see cref="IRenderContext" /> decorator that distorts the rendered output.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides a <see cref="IRenderContext" /> decorator that distorts the rendered output.
    /// </summary>
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

            this.DistortionFactor = 7;
            this.InterpolationDistance = 10;
            this.ThicknessScale = 2;

            this.FontFamily = "Humor Sans"; // http://antiyawn.com/uploads/humorsans.html
            //// this.FontFamily = "Comic Sans MS";
        }

        /// <summary>
        /// Gets or sets the distortion factor.
        /// </summary>
        public double DistortionFactor { get; set; }

        /// <summary>
        /// Gets or sets the interpolation distance.
        /// </summary>
        public double InterpolationDistance { get; set; }

        /// <summary>
        /// Gets or sets the font family.
        /// </summary>
        /// <value>
        /// The font family.
        /// </value>
        public string FontFamily { get; set; }

        /// <summary>
        /// Gets or sets the thickness scale.
        /// </summary>
        /// <value>
        /// The thickness scale.
        /// </value>
        public double ThicknessScale { get; set; }

        /// <inheritdoc/>
        public override void DrawLine(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray,
            LineJoin lineJoin)
        {
            var xckdPoints = this.Distort(points);
            this.rc.DrawLine(xckdPoints, stroke, thickness * this.ThicknessScale, edgeRenderingMode, dashArray, lineJoin);
        }

        /// <inheritdoc/>
        public override void DrawPolygon(
            IList<ScreenPoint> points,
            OxyColor fill,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray,
            LineJoin lineJoin)
        {
            var p = new List<ScreenPoint>(points);
            p.Add(p[0]);

            var xckdPoints = this.Distort(p);
            this.rc.DrawPolygon(xckdPoints, fill, stroke, thickness * this.ThicknessScale, edgeRenderingMode, dashArray, lineJoin);
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
        /// <param name="text">The text in the tool tip.</param>
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
        /// Gets the transformed font family name.
        /// </summary>
        /// <param name="fontFamily">The original font family.</param>
        /// <returns>The actual font family.</returns>
        // ReSharper disable once UnusedParameter.Local
        private string GetFontFamily(string fontFamily)
        {
            return this.FontFamily;
        }

        /// <summary>
        /// Distorts the specified points.
        /// </summary>
        /// <param name="points">The input points.</param>
        /// <returns>
        /// The distorted points.
        /// </returns>
        private ScreenPoint[] Distort(IEnumerable<ScreenPoint> points)
        {
            // See the Mathematica / Matplotlib solutions
            // http://jakevdp.github.io/blog/2012/10/07/xkcd-style-plots-in-matplotlib/
            // http://jakevdp.github.io/blog/2013/07/10/XKCD-plots-in-matplotlib/
            // http://mathematica.stackexchange.com/questions/11350/xkcd-style-graphs
            // http://www.mail-archive.com/matplotlib-users@lists.sourceforge.net/msg25499.html
            // http://nbviewer.ipython.org/gist/anonymous/3835181
            // http://matplotlib.org/xkcd/
            // http://xkcd.com/

            // The following code is just to show that some randomness is working - this should be improved
            IList<ScreenPoint> interpolated = this.Interpolate(points, this.InterpolationDistance).ToArray();
            var result = new ScreenPoint[interpolated.Count];
            var randomNumbers = this.GenerateRandomNumbers(interpolated.Count);
            randomNumbers = this.ApplyMovingAverage(randomNumbers, 5);

            var d = this.DistortionFactor;
            double d2 = d / 2;
            for (int i = 0; i < interpolated.Count; i++)
            {
                if (i == 0 || i == interpolated.Count - 1)
                {
                    result[i] = interpolated[i];
                    continue;
                }

                var tangent = interpolated[i + 1] - interpolated[i - 1];
                tangent.Normalize();
                var normal = new ScreenVector(tangent.Y, -tangent.X);

                var delta = normal * ((randomNumbers[i] * d) - d2);
                result[i] = interpolated[i] + delta;
            }

            return result;
        }

        /// <summary>
        /// Generates an array of random numbers.
        /// </summary>
        /// <param name="n">The number of numbers to generate.</param>
        /// <returns>The random numbers.</returns>
        private double[] GenerateRandomNumbers(int n)
        {
            var result = new double[n];
            for (int i = 0; i < n; i++)
            {
                result[i] = this.r.NextDouble();
            }

            return result;
        }

        /// <summary>
        /// Applies a moving average filter to the input values.
        /// </summary>
        /// <param name="input">The input values.</param>
        /// <param name="m">The number of values to average.</param>
        /// <returns>The filtered values.</returns>
        private double[] ApplyMovingAverage(IList<double> input, int m)
        {
            // http://en.wikipedia.org/wiki/Moving_average
            int n = input.Count;
            var result = new double[n];
            var m2 = m / 2;
            for (int i = 0; i < n; i++)
            {
                var j0 = Math.Max(0, i - m2);
                var j1 = Math.Min(n - 1, i + m2);
                for (int j = j0; j <= j1; j++)
                {
                    result[i] += input[j];
                }

                result[i] /= m;
            }

            return result;
        }

        /// <summary>
        /// Interpolates the input points.
        /// </summary>
        /// <param name="input">The input points.</param>
        /// <param name="dist">The interpolation distance.</param>
        /// <returns>The interpolated points.</returns>
        private IEnumerable<ScreenPoint> Interpolate(IEnumerable<ScreenPoint> input, double dist)
        {
            var p0 = default(ScreenPoint);
            double l = -1;
            double nl = dist;
            foreach (var p1 in input)
            {
                if (l < 0)
                {
                    yield return p1;
                    p0 = p1;
                    l = 0;
                    continue;
                }

                var dp = p1 - p0;
                var l1 = dp.Length;

                if (l1 > 0)
                {
                    while (nl >= l && nl <= l + l1)
                    {
                        var f = (nl - l) / l1;
                        yield return new ScreenPoint((p0.X * (1 - f)) + (p1.X * f), (p0.Y * (1 - f)) + (p1.Y * f));

                        nl += dist;
                    }
                }

                l += l1;
                p0 = p1;
            }

            yield return p0;
        }

        /*
        private double[] KaiserWindow(int m, double beta)
        {
            // http://en.wikipedia.org/wiki/Kaiser_window
            // http://docs.scipy.org/doc/numpy/reference/generated/numpy.kaiser.html
            // http://docs.scipy.org/doc/numpy/reference/routines.window.html
            return null;
        }

        private double[] FirWin(int nunmtaps, double cutoff)
        {
            // http://docs.scipy.org/doc/scipy-0.13.0/reference/generated/scipy.signal.firwin.html
            // http://www.labbookpages.co.uk/audio/firWindowing.html
            return null;
        }

        private static double[] FIR(double[] b, double[] x)
        {
            // http://stackoverflow.com/questions/2472093/implementation-of-fir-filter-in-c-sharp
            int M = b.Length;
            int n = x.Length;
            //y[n]=b0x[n]+b1x[n-1]+....bmx[n-M]
            var y = new double[n];
            for (int yi = 0; yi < n; yi++)
            {
                double t = 0.0;
                for (int bi = M - 1; bi >= 0; bi--)
                {
                    if (yi - bi < 0)
                    {
                        continue;
                    }

                    t += b[bi] * x[yi - bi];
                }

                y[yi] = t;
            }

            return y;
        }*/
    }
}
