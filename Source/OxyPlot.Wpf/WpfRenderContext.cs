// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WpfRenderContext.cs" company="OxyPlot">
//   Copyright (c) 2026 OxyPlot contributors
// </copyright>
// <summary>
//   Implements shared functionality for all WPF render contexts.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using FontWeights = OxyPlot.FontWeights;

    /// <summary>
    /// Base class for all WPF render contexts. 
    /// </summary>
    public abstract class WpfRenderContext : ClippingRenderContext
    {
        /// <summary>
        /// The images in use
        /// </summary>
        protected readonly HashSet<OxyImage> _imagesInUse = new HashSet<OxyImage>();

        /// <summary>
        /// The image cache
        /// </summary>
        protected readonly Dictionary<OxyImage, BitmapSource> _imageCache = new Dictionary<OxyImage, BitmapSource>();

        /// <summary>
        /// The brush cache.
        /// </summary>
        protected readonly Dictionary<OxyColor, Brush> _brushCache = new Dictionary<OxyColor, Brush>();

        /// <summary>
        /// The font family cache
        /// </summary>
        protected readonly Dictionary<string, FontFamily> _fontFamilyCache = new Dictionary<string, FontFamily>();

        /// <summary>
        /// The current tool tip
        /// </summary>
        protected string _currentToolTip;

        /// <summary>
        /// The dpi scale.
        /// </summary>
        public double DpiScale { get; set; } = 1;

        /// <summary>
        /// The visual offset relative to visual root.
        /// </summary>
        public Point VisualOffset { get; set; }
        
        /// <summary>
        /// Gets or sets the text measurement method.
        /// </summary>
        /// <value>The text measurement method.</value>
        public TextMeasurementMethod TextMeasurementMethod { get; set; }

        /// <summary>
        /// Gets or sets the text formatting mode.
        /// </summary>
        /// <value>The text formatting mode. The default value is <see cref="System.Windows.Media.TextFormattingMode.Display"/>.</value>
        public TextFormattingMode TextFormattingMode { get; set; }

        ///<inheritdoc/>
        public override void DrawEllipse(
            OxyRect rect,
            OxyColor fill,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode)
        {
            this.DrawEllipses(new[] { rect }, fill, stroke, thickness, edgeRenderingMode);
        }

        ///<inheritdoc/>
        public override void DrawPolygon(
            IList<ScreenPoint> points,
            OxyColor fill,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray,
            LineJoin lineJoin)
        {
            this.DrawPolygons(new[] { points }, fill, stroke, thickness, edgeRenderingMode, dashArray, lineJoin);
        }

        ///<inheritdoc/>
        public override void DrawRectangle(OxyRect rect, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode)
        {
            this.DrawRectangles(new[] { rect }, fill, stroke, thickness, edgeRenderingMode);
        }

        ///<inheritdoc/>
        public override OxySize MeasureText(string text, string fontFamily, double fontSize, double fontWeight)
        {
            if (string.IsNullOrEmpty(text))
            {
                return OxySize.Empty;
            }

            if (this.TextMeasurementMethod == TextMeasurementMethod.GlyphTypeface)
            {
                return MeasureTextByGlyphTypeface(text, fontFamily, fontSize, fontWeight);
            }

            var tb = new TextBlock { Text = text };

            TextOptions.SetTextFormattingMode(tb, this.TextFormattingMode);

            if (fontFamily != null)
            {
                tb.FontFamily = new FontFamily(fontFamily);
            }

            if (fontSize > 0)
            {
                tb.FontSize = fontSize;
            }

            if (fontWeight > 0)
            {
                tb.FontWeight = GetFontWeight(fontWeight);
            }

            tb.Measure(new Size(1000, 1000));

            return new OxySize(tb.DesiredSize.Width, tb.DesiredSize.Height);
        }

        ///<inheritdoc/>
        public override void SetToolTip(string text)
        {
            this._currentToolTip = text;
        }

        ///<inheritdoc/>
        public override void CleanUp()
        {
            // Find the images in the cache that has not been used since last call to this method
            var imagesToRelease = this._imageCache.Keys.Where(i => !this._imagesInUse.Contains(i)).ToList();

            // Remove the images from the cache
            foreach (var i in imagesToRelease)
            {
                this._imageCache.Remove(i);
            }

            this._imagesInUse.Clear();
        }

        /// <summary>
        /// Measures the size of the specified text by a faster method (using GlyphTypefaces).
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">The font size.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <returns>The size of the text.</returns>
        protected static OxySize MeasureTextByGlyphTypeface(string text, string fontFamily, double fontSize, double fontWeight)
        {
            if (string.IsNullOrEmpty(text))
            {
                return OxySize.Empty;
            }

            var typeface = new Typeface(
                new FontFamily(fontFamily), FontStyles.Normal, GetFontWeight(fontWeight), FontStretches.Normal);

            if (!typeface.TryGetGlyphTypeface(out var glyphTypeface))
            {
                throw new InvalidOperationException("No glyph typeface found");
            }

            return MeasureTextSize(glyphTypeface, fontSize, text);
        }

        /// <summary>
        /// Gets the font weight.
        /// </summary>
        /// <param name="fontWeight">The font weight value.</param>
        /// <returns>The font weight.</returns>
        public static FontWeight GetFontWeight(double fontWeight)
        {
            return fontWeight > FontWeights.Normal ? System.Windows.FontWeights.Bold : System.Windows.FontWeights.Normal;
        }

        /// <summary>
        /// Fast text size calculation
        /// </summary>
        /// <param name="glyphTypeface">The glyph typeface.</param>
        /// <param name="sizeInEm">The size.</param>
        /// <param name="s">The text.</param>
        /// <returns>The text size.</returns>
        private static OxySize MeasureTextSize(GlyphTypeface glyphTypeface, double sizeInEm, string s)
        {
            var width = 0d;
            var lineWidth = 0d;
            var lines = 0;
            foreach (var ch in s)
            {
                switch (ch)
                {
                    case '\n':
                    lines++;
                    if (lineWidth > width)
                    {
                        width = lineWidth;
                    }

                    lineWidth = 0;
                    continue;
                    case '\t':
                    continue;
                }

                var glyph = glyphTypeface.CharacterToGlyphMap[ch];
                var advanceWidth = glyphTypeface.AdvanceWidths[glyph];
                lineWidth += advanceWidth;
            }

            lines++;
            if (lineWidth > width)
            {
                width = lineWidth;
            }

            return new OxySize(Math.Round(width * sizeInEm, 2), Math.Round(lines * glyphTypeface.Height * sizeInEm, 2));
        }

        /// <summary>
        /// Gets the cached brush.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The brush.</returns>
        protected Brush GetCachedBrush(OxyColor color)
        {
            if (color.A == 0)
            {
                return null;
            }

            if (!this._brushCache.TryGetValue(color, out var brush))
            {
                brush = new SolidColorBrush(color.ToColor());
                brush.Freeze();
                this._brushCache.Add(color, brush);
            }

            return brush;
        }

        /// <summary>
        /// Gets the cached font family.
        /// </summary>
        /// <param name="familyName">Name of the family.</param>
        /// <returns>The FontFamily.</returns>
        protected FontFamily GetCachedFontFamily(string familyName)
        {
            if (familyName == null)
            {
                return null;
            }

            if (!this._fontFamilyCache.TryGetValue(familyName, out var ff))
            {
                ff = new FontFamily(familyName);
                this._fontFamilyCache.Add(familyName, ff);
            }

            return ff;
        }

        /// <summary>
        /// Gets the bitmap source.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>The bitmap source.</returns>
        protected BitmapSource GetImageSource(OxyImage image)
        {
            if (image == null)
            {
                return null;
            }

            if (!this._imagesInUse.Contains(image))
            {
                this._imagesInUse.Add(image);
            }

            if (this._imageCache.TryGetValue(image, out var src))
            {
                return src;
            }

            using var ms = new MemoryStream(image.GetData());
            var btm = new BitmapImage();
            btm.BeginInit();
            btm.StreamSource = ms;
            btm.CacheOption = BitmapCacheOption.OnLoad;
            btm.EndInit();
            btm.Freeze();
            this._imageCache.Add(image, btm);
            return btm;
        }

        /// <summary>
        /// Converts an <see cref="OxyRect" /> to a <see cref="Rect" />.
        /// </summary>
        /// <param name="r">The rectangle.</param>
        /// <returns>A <see cref="Rect" />.</returns>
        protected static Rect ToRect(OxyRect r)
        {
            return new Rect(r.Left, r.Top, r.Width, r.Height);
        }

        /// <summary>
        /// Snaps points to pixels if required by the edge rendering mode.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="strokeThickness">The stroke thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <returns>The processed points.</returns>
        protected IEnumerable<Point> GetActualPoints(IList<ScreenPoint> points, double strokeThickness, EdgeRenderingMode edgeRenderingMode)
        {
            switch (edgeRenderingMode)
            {
                case EdgeRenderingMode.Adaptive when IsStraightLine(points):
                case EdgeRenderingMode.Automatic when IsStraightLine(points):
                case EdgeRenderingMode.PreferSharpness:
                return points.Select(p => PixelLayout.Snap(p.X, p.Y, strokeThickness, this.VisualOffset, this.DpiScale));
                default:
                return points.Select(p => new Point(p.X, p.Y));
            }
        }

        /// <summary>
        /// Snaps a rectangle to device pixels if required by the edge rendering mode.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <param name="strokeThickness">The stroke thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <returns>The processed rectangle.</returns>
        protected Rect GetActualRect(OxyRect rect, double strokeThickness, EdgeRenderingMode edgeRenderingMode)
        {
            switch (edgeRenderingMode)
            {
                case EdgeRenderingMode.PreferGeometricAccuracy:
                case EdgeRenderingMode.PreferSpeed:
                return ToRect(rect);
                default:
                return PixelLayout.Snap(ToRect(rect), strokeThickness, this.VisualOffset, this.DpiScale);
            }
        }

        /// <summary>
        /// Snaps a stroke thickness to device pixels if required by the edge rendering mode.
        /// </summary>
        /// <param name="strokeThickness">The stroke thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <returns>The processed stroke thickness.</returns>
        protected double GetActualStrokeThickness(double strokeThickness, EdgeRenderingMode edgeRenderingMode)
        {
            switch (edgeRenderingMode)
            {
                case EdgeRenderingMode.PreferSharpness:
                return PixelLayout.SnapStrokeThickness(strokeThickness, this.DpiScale);
                default:
                return strokeThickness;
            }
        }

    }


}
