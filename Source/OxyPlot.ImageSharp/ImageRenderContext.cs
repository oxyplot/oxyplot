// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageRenderContext.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an implementation of IRenderContext which draws to an ImageSharp Image.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.ImageSharp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using SixLabors.Fonts;
    using SixLabors.Fonts.Exceptions;
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.PixelFormats;
    using SixLabors.ImageSharp.Processing;
    using SixLabors.Primitives;
    using SixLabors.Shapes;

    /// <summary>
    /// Provides an implementation of IRenderContext which draws to a <see cref="Image"/>.
    /// </summary>
    public class ImageRenderContext : RenderContextBase, IDisposable
    {
        /// <summary>
        /// The default font to use when a request font cannot be found.
        /// </summary>
        private static readonly string FallbackFontFamily = "Arial";

        /// <summary>
        /// Image to which the the <see cref="ImageRenderContext"/> will render.
        /// </summary>
        private readonly Image<Rgba32> image;

        /// <summary>
        /// Image to which we will render when clipping.
        /// </summary>
        private readonly Image<Rgba32> clipImage;

        /// <summary>
        /// Whether or not the ImageRenderContext has been disposed.
        /// </summary>
        private bool disposedValue = false;

        /// <summary>
        /// The current clipping rectangle.
        /// </summary>
        private Rectangle clippingRectangle;

        /// <summary>
        /// A value indicating whether we are currently clipping.
        /// </summary>
        private bool clipping;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageRenderContext"/> class.
        /// </summary>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="background">The background color of the image.</param>
        /// <param name="dpi">The number of dots per inch (DPI).</param>
        public ImageRenderContext(int width, int height, OxyColor background, double dpi = 96)
        {
            this.image = new Image<Rgba32>(width, height);
            this.clipImage = new Image<Rgba32>(width, height);

            this.image.Metadata.HorizontalResolution = dpi;
            this.image.Metadata.VerticalResolution = dpi;
            this.clipImage.Metadata.HorizontalResolution = dpi;
            this.clipImage.Metadata.VerticalResolution = dpi;

            this.Dpi = (float)dpi;
            this.DpiScale = (float)(dpi / 96.0);

            this.image.Mutate(img => img.BackgroundColor(ToRgba32(background)));

            this.RendersToScreen = false;

            this.clipping = false;
        }

        /// <summary>
        /// Gets the DPI scaling factor. A value of 1 corresponds to 96 DPI (dots per inch).
        /// </summary>
        private float DpiScale { get; }

        /// <summary>
        /// Gets the number of dots per inch (DPI).
        /// </summary>
        private float Dpi { get; }

        /// <summary>
        /// Gets the current target image.
        /// </summary>
        private Image Target => this.clipping ? this.clipImage : this.image;

        /// <summary>
        /// Gets a copy of the image.
        /// </summary>
        /// <returns>A copy of the internal image.</returns>
        public Image GetImageCopy()
        {
            this.EnsureClippedRegion();

            return this.image.Clone();
        }

        /// <summary>
        /// Saves the image to the specified stream as a png.
        /// </summary>
        /// <param name="output">The output stream.</param>
        public void SaveAsPng(Stream output)
        {
            this.EnsureClippedRegion();

            this.image.SaveAsPng(output);
        }

        /// <summary>
        /// Saves the image to the specified stream as a bmp.
        /// </summary>
        /// <param name="output">The output stream.</param>
        public void SaveAsBmp(Stream output)
        {
            this.EnsureClippedRegion();

            // TODO: investigate bmp encoder options
            this.image.SaveAsBmp(output);
        }

        /// <summary>
        /// Saves the image to the specified stream as a gif.
        /// </summary>
        /// <param name="output">The output stream.</param>
        public void SaveAsGif(Stream output)
        {
            this.EnsureClippedRegion();

            // TODO: investigate gif encoder options
            this.image.SaveAsGif(output);
        }

        /// <summary>
        /// Saves the image to the specified stream as a jpeg.
        /// </summary>
        /// <param name="output">The output stream.</param>
        /// <param name="quality">The quality of the exported jpeg, a value between 0 and 100.</param>
        public void SaveAsJpeg(Stream output, int quality = 75)
        {
            this.EnsureClippedRegion();

            this.image.SaveAsJpeg(output, new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder() { Quality = quality });
        }

        /// <inheritdoc/>
        public override void DrawText(ScreenPoint p, string text, OxyColor fill, string fontFamily = null, double fontSize = 10, double fontWeight = 400, double rotation = 0, OxyPlot.HorizontalAlignment horizontalAlignment = OxyPlot.HorizontalAlignment.Left, OxyPlot.VerticalAlignment verticalAlignment = OxyPlot.VerticalAlignment.Top, OxySize? maxSize = null)
        {
            if (text == null || !fill.IsVisible())
            {
                return;
            }

            var font = this.GetFontOrThrow(fontFamily, fontSize, this.ToFontStyle(fontWeight));
            var actualFontSize = this.NominalFontSizeToPoints(fontSize);

            var outputX = this.Convert(p.X);
            var outputY = this.Convert(p.Y);
            var outputPosition = new PointF(outputX, outputY);

            var cos = (float)Math.Cos(rotation * Math.PI / 180.0);
            var sin = (float)Math.Sin(rotation * Math.PI / 180.0);

            // measure bounds of the whole text (we only need the height)
            var bounds = this.MeasureTextLoose(text, fontFamily, fontSize, fontWeight);
            var boundsHeight = this.Convert(bounds.Height);
            var offsetHeight = new PointF(boundsHeight * -sin, boundsHeight * cos);

            // determine the font metrids for this font size at 96 DPI
            var actualDescent = this.Convert(actualFontSize * this.MilliPointsToNominalResolution(font.Descender));
            var offsetDescent = new PointF(actualDescent * -sin, actualDescent * cos);

            var actualLineHeight = this.Convert(actualFontSize * this.MilliPointsToNominalResolution(font.LineHeight));
            var offsetLineHeight = new PointF(actualLineHeight * -sin, actualLineHeight * cos);

            var actualLineGap = this.Convert(actualFontSize * this.MilliPointsToNominalResolution(font.LineGap));
            var offsetLineGap = new PointF(actualLineGap * -sin, actualLineGap * cos);

            // find top of the whole text
            var deltaY = verticalAlignment switch
            {
                OxyPlot.VerticalAlignment.Top => 1.0f,
                OxyPlot.VerticalAlignment.Middle => 0.5f,
                OxyPlot.VerticalAlignment.Bottom => 0.0f,
                _ => throw new ArgumentOutOfRangeException(nameof(verticalAlignment)),
            };

            // this is the top of the top line
            var topPosition = outputPosition + (offsetHeight * deltaY) - offsetHeight;

            // need this later
            var deltaX = horizontalAlignment switch
            {
                OxyPlot.HorizontalAlignment.Left => -0.0f,
                OxyPlot.HorizontalAlignment.Center => -0.5f,
                OxyPlot.HorizontalAlignment.Right => -1.0f,
                _ => throw new ArgumentOutOfRangeException(nameof(horizontalAlignment)),
            };

            var lines = StringHelper.SplitLines(text);
            for (int li = 0; li < lines.Length; li++)
            {
                var line = lines[li];

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                
                // measure bounds of just the line (we only need the width)
                var lineBounds = this.MeasureTextLoose(line, fontFamily, fontSize, fontWeight);
                var lineBoundsWidth = this.Convert(lineBounds.Width);
                var offsetLineWidth = new PointF(lineBoundsWidth * cos, lineBoundsWidth * sin);

                // find the left baseline position
                var lineTop = topPosition + (offsetLineGap * li) + (offsetLineHeight * li);
                var lineBaseLineLeft = lineTop + offsetLineWidth * deltaX + offsetLineHeight + offsetDescent;

                // this seems to produce consistent and correct results, but we have to rotate it manually, so render it at the origin for simplicity
                var glyphsAtOrigin = TextBuilder.GenerateGlyphs(line, new PointF(0f, 0f), new RendererOptions(font, this.Dpi, this.Dpi)
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Bottom, // sit on the line (baseline)
                    ApplyKerning = true,
                });

                // translate and rotate into possition
                var transform = Matrix3x2Extensions.CreateRotationDegrees((float)rotation);
                transform.Translation = lineBaseLineLeft;
                var glyphs = glyphsAtOrigin.Transform(transform);

                // draw the glyphs
                this.Target.Mutate(img =>
                {
                    img.Fill(ToRgba32(fill), glyphs);
                });
            }
        }

        /// <inheritdoc/>
        public override OxySize MeasureText(string text, string fontFamily = null, double fontSize = 10, double fontWeight = 500)
        {
            return this.MeasureTextLoose(text, fontFamily, fontSize, fontWeight);
        }

        /// <inheritdoc/>
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
            if (source == null)
            {
                return;
            }

            var dest = new RectangleF((float)this.Convert(destX), (float)this.Convert(destY), (float)this.Convert(destWidth), (float)this.Convert(destHeight));
            var src = new RectangleF((float)srcX, (float)srcY, (float)srcWidth, (float)srcHeight);
            var scale = new SizeF(dest.Width / src.Width, dest.Height / src.Height);

            // if we are outside the image, quit now
            if (dest.Right < 0 || dest.Left >= this.image.Width || dest.Bottom < 0 || dest.Top >= this.image.Height)
            {
                return;
            }

            // crop the bounds so that they are within the image bounds (this is necessary because we have to create a resized version of the cropped source image)
            var cropLeft = dest.Left < 0 ? -dest.Left : 0;
            var cropTop = dest.Top < 0 ? -dest.Top : 0;
            var cropRight = dest.Right >= this.image.Width ? dest.Right - this.image.Width : 0;
            var cropBottom = dest.Bottom >= this.image.Height ? dest.Bottom - this.image.Height : 0;

            dest = RectangleF.FromLTRB(dest.Left + cropLeft, dest.Top + cropTop, dest.Right - cropRight, dest.Bottom - cropBottom);
            src = RectangleF.FromLTRB(src.Left + (cropLeft / scale.Width), src.Top + (cropTop / scale.Height), src.Right - (cropRight / scale.Width), src.Bottom - (cropBottom / scale.Height));

            var bytes = source.GetData();
            var sourceImage = SixLabors.ImageSharp.Image.Load(bytes);

            var resampler = interpolate ? KnownResamplers.Triangle : KnownResamplers.NearestNeighbor;

            /* The idea now is to roughly crop the source before we resize and then precisely crop it, before drawing it onto the target
             * The steps required are:
             *   - Crop the source image to -1/+2 pixel bounds (may need to increase these bounds depending on the resampler)
             *   - Add a one pixel 'mirror' border, so that we can a clamped edge when interpolating
             *   - Resize the source image by the appropriate scale with the appropriate resampler, simultaneously offseting by the non-integer parts of dest and src
             *   - Crop to exactly what we want
             *   - Draw the source image onto the destination image
             */

            var doPad = interpolate;

            var srcRough = new Rectangle((int)Math.Floor(src.X), (int)Math.Floor(src.Y), (int)Math.Ceiling(src.Width + 3), (int)Math.Ceiling(src.Height + 3));
            srcRough.Intersect(sourceImage.Bounds());
            var srcOffset = new PointF(srcRough.X - src.X, srcRough.Y - src.Y);
            srcOffset.Offset(0.5f, 0.5f); // texel alignment for resampler
            if (doPad)
            {
                srcOffset.Offset(-1f, -1f); // offset from padding
            }

            var destOffset = new PointF(dest.X - (float)Math.Floor(dest.X), dest.Y - (float)Math.Floor(dest.Y));
            var destRough = new Rectangle(0, 0, (int)Math.Ceiling(dest.Width), (int)Math.Ceiling(dest.Height));

            var rescale = new AffineTransformBuilder().AppendTranslation(srcOffset).AppendScale(scale);

            try
            {
                sourceImage.Mutate(img =>
                {
                    img.Crop(srcRough);

                    if (doPad)
                    {
                        img.Pad(srcRough.Width + 2, srcRough.Height + 2);
                    }
                });

                if (doPad)
                {
                    sourceImage[0, 0] = sourceImage[1, 1];
                    sourceImage[sourceImage.Width - 1, 0] = sourceImage[sourceImage.Width - 2, 1];
                    sourceImage[0, sourceImage.Height - 1] = sourceImage[1, sourceImage.Height - 2];
                    sourceImage[sourceImage.Width - 1, sourceImage.Height - 1] = sourceImage[sourceImage.Width - 2, sourceImage.Height - 2];

                    for (int x = 1; x < sourceImage.Width - 1; x++)
                    {
                        sourceImage[x, 0] = sourceImage[x, 1];
                        sourceImage[x, sourceImage.Height - 1] = sourceImage[x, sourceImage.Height - 2];
                    }

                    for (int y = 1; y < sourceImage.Height - 1; y++)
                    {
                        sourceImage[0, y] = sourceImage[1, y];
                        sourceImage[sourceImage.Width - 1, y] = sourceImage[sourceImage.Width - 2, y];
                    }
                }

                sourceImage.Mutate(img =>
                {
                    img.Transform(rescale, resampler);
                    destRough.Intersect(sourceImage.Bounds());
                    img.Crop(destRough);
                });

                this.Target.Mutate(img =>
                {
                    img.DrawImage(sourceImage, new Point((int)dest.X, (int)dest.Y), new GraphicsOptions(interpolate, (float)opacity));
                });
            }
            catch (ImageProcessingException)
            {
                // Swallow: it's probably because we are trying to render outside of the image: https://github.com/SixLabors/ImageSharp/pull/877
                // TODO: verify that we are trying to render outside of the image... somehow
                //  - I don't think this can be done without having to track the ImageSharp code unhealthily closely
            }
            finally
            {
                sourceImage.Dispose();
            }
        }

        /// <inheritdoc/>
        public override void DrawLine(IList<ScreenPoint> points, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode, double[] dashArray, LineJoin lineJoin)
        {
            if (points.Count < 2 || !stroke.IsVisible() || thickness <= 0)
            {
                return;
            }

            var actualThickness = this.GetActualThickness(thickness, edgeRenderingMode);
            var actualDashArray = dashArray != null
                ? this.ConvertDashArray(dashArray, actualThickness)
                : null;

            var pen = actualDashArray != null
                ? new Pen(ToRgba32(stroke), actualThickness, actualDashArray)
                : new Pen(ToRgba32(stroke), actualThickness);
            var actualPoints = this.GetActualPoints(points, thickness, edgeRenderingMode).ToArray();
            var options = new GraphicsOptions(this.ShouldUseAntiAliasingForLine(edgeRenderingMode, points));

            this.Target.Mutate(img =>
            {
                img.DrawLines(options, pen, actualPoints);
            });
        }

        /// <inheritdoc/>
        public override void DrawPolygon(IList<ScreenPoint> points, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode, double[] dashArray, LineJoin lineJoin)
        {
            if ((!fill.IsVisible() && !(stroke.IsVisible() || thickness <= 0)) || points.Count < 2)
            {
                return;
            }

            var actualThickness = this.GetActualThickness(thickness, edgeRenderingMode);
            var actualDashArray = dashArray != null
                ? this.ConvertDashArray(dashArray, actualThickness)
                : null;

            var pen = actualDashArray != null
                ? new Pen(ToRgba32(stroke), actualThickness, actualDashArray)
                : new Pen(ToRgba32(stroke), actualThickness);
            var actualPoints = this.GetActualPoints(points, thickness, edgeRenderingMode).ToArray();
            var options = new GraphicsOptions(this.ShouldUseAntiAliasingForLine(edgeRenderingMode, points));

            var brush = Brushes.Solid(ToRgba32(fill));

            this.Target.Mutate(img =>
            {
                img.FillPolygon(options, brush, actualPoints);
                img.DrawPolygon(options, pen, actualPoints);
            });
        }

        /// <inheritdoc/>
        public override void ResetClip()
        {
            this.EnsureClippedRegion();

            this.clipping = false;
        }

        /// <inheritdoc/>
        public override bool SetClip(OxyRect clippingRectangle)
        {
            var actualRectangle = this.ConvertSnap(clippingRectangle, 0);
            var provisonal = Rectangle.FromLTRB((int)actualRectangle.Left, (int)actualRectangle.Top, (int)actualRectangle.Right, (int)actualRectangle.Bottom);

            if (this.clipping && this.clippingRectangle.Equals(provisonal))
            {
                // don't perform unnecessary work
                return true;
            }
            else
            {
                this.EnsureClippedRegion();

                this.clippingRectangle = provisonal;
                this.clipping = true;
                this.Blit(this.image, this.clipImage, this.clippingRectangle);

                return true;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">Whether this method is being called from Dispose.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.image.Dispose();
                    this.clipImage.Dispose();
                }

                this.disposedValue = true;
            }
        }

        /// <summary>
        /// Translates an <see cref="OxyColor"/> to a <see cref="Rgba32"/>.
        /// </summary>
        /// <param name="color">The <see cref="OxyColor"/>.</param>
        /// <returns>The resulting <see cref="Rgba32"/>.</returns>
        private static Rgba32 ToRgba32(OxyColor color)
        {
            return new Rgba32(color.R, color.G, color.B, color.A);
        }

        /// <summary>
        /// Gets the pixel offset that a line with the specified thickness should snap to.
        /// </summary>
        /// <remarks>
        /// This takes into account that lines with even stroke thickness should be snapped to the border between two pixels while lines with odd stroke thickness should be snapped to the middle of a pixel.
        /// </remarks>
        /// <param name="thickness">The line thickness.</param>
        /// <returns>The snap offset.</returns>
        private static float GetSnapOffset(float thickness)
        {
            var mod = thickness % 2;
            var isOdd = mod >= 0.5 && mod < 1.5;
            return isOdd ? 0.5f : 0;
        }

        /// <summary>
        /// Snaps a value to a pixel with the specified offset.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>The snapped value.</returns>
        private static float Snap(float value, float offset)
        {
            return (float)Math.Round(value + offset, MidpointRounding.AwayFromZero) - offset;
        }

        /// <summary>
        /// Counts the number of lines in the text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The number of lines in the text.</returns>
        private static int CountLines(string text)
        {
            return StringHelper.SplitLines(text).Length;
        }

        /// <summary>
        /// Copies the current clipping rectangle from the <see cref="clipImage"/> to the <see cref="image" />.
        /// </summary>
        private void EnsureClippedRegion()
        {
            if (this.clipping)
            {
                this.Blit(this.clipImage, this.image, this.clippingRectangle);
            }
        }

        /// <summary>
        /// Copies pixel values from one image to another within a given rectangle.
        /// </summary>
        /// <param name="source">The <see cref="Image{Rgba32}" /> from which to copy.</param>
        /// <param name="destination">The <see cref="Image{Rgba32}" /> to which to copy.</param>
        /// <param name="rectangle">The region to copy.</param>
        private void Blit(Image<Rgba32> source, Image<Rgba32> destination, Rectangle rectangle)
        {
            rectangle.Intersect(source.Bounds());

            for (int i = rectangle.Left; i < rectangle.Right; i++)
            {
                for (int j = rectangle.Top; j < rectangle.Bottom; j++)
                {
                    destination[i, j] = source[i, j];
                }
            }
        }

        private Font GetFontOrThrow(string fontFamily, double fontSize, FontStyle regular, bool allowFallback = true)
        {
            var family = this.GetFamilyOrFallbackOrThrow(fontFamily, allowFallback);
            var actualFontSize = this.NominalFontSizeToPoints(fontSize);
            return new Font(family, (float)actualFontSize, FontStyle.Regular);
        }

        private FontFamily GetFamilyOrFallbackOrThrow(string fontFamily = null, bool allowFallback = true)
        {
            if (fontFamily == null)
            {
                allowFallback = false;
                fontFamily = FallbackFontFamily;
            }

            FontFamily family;
            try
            {
                family = SixLabors.Fonts.SystemFonts.Find(fontFamily);
            }
            catch (FontFamilyNotFoundException primaryEx)
            {
                if (!allowFallback)
                {
                    throw;
                }

                try
                {
                    family = SixLabors.Fonts.SystemFonts.Find(FallbackFontFamily);
                }
                catch (FontFamilyNotFoundException fallbackEx)
                {
                    throw new AggregateException(primaryEx, fallbackEx);
                }
            }

            return family;
        }

        /// <summary>
        /// Measures the text as it will be arranged out by OxyPlot.
        /// </summary>
        /// <param name="text">The text to render.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">The font size in points.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <returns>An <see cref="OxySize"/>.</returns>
        private OxySize MeasureTextLoose(string text, string fontFamily, double fontSize, double fontWeight)
        {
            text = text ?? string.Empty;

            var font = this.GetFontOrThrow(fontFamily, fontSize, this.ToFontStyle(fontWeight));
            var actualFontSize = this.NominalFontSizeToPoints(fontSize);

            var tight = this.MeasureTextTight(text, fontFamily, fontSize, fontWeight);
            var width = tight.Width;

            var lineHeight = actualFontSize * this.MilliPointsToNominalResolution(font.LineHeight);
            var lineGap = actualFontSize * this.MilliPointsToNominalResolution(font.LineGap);
            var lineCount = CountLines(text);

            var height = (lineHeight * lineCount) + (lineGap * (lineCount - 1));

            return new OxySize(width, height);
        }

        /// <summary>
        /// Measures the text as it will be rendered by ImageSharp.
        /// </summary>
        /// <param name="text">The text to render.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">The font size in points.</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <returns>An <see cref="OxySize"/>.</returns>
        private OxySize MeasureTextTight(string text, string fontFamily, double fontSize, double fontWeight)
        {
            text = text ?? string.Empty;

            var font = this.GetFontOrThrow(fontFamily, fontSize, this.ToFontStyle(fontWeight));
            var actualFontSize = this.NominalFontSizeToPoints(fontSize);

            var result = TextMeasurer.Measure(text, new RendererOptions(font, this.Dpi));
            return new OxySize(this.ConvertBack(result.Width), this.ConvertBack(result.Height));
        }

        /// <summary>
        /// Gets the snapping offset for the specified stroke thickness.
        /// </summary>
        /// <remarks>
        /// This takes into account that lines with even stroke thickness should be snapped to the border between two pixels while lines with odd stroke thickness should be snapped to the middle of a pixel.
        /// </remarks>
        /// <param name="thickness">The stroke thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <returns>The snap offset.</returns>
        private float GetSnapOffset(double thickness, EdgeRenderingMode edgeRenderingMode)
        {
            var actualThickness = this.GetActualThickness(thickness, edgeRenderingMode);
            return GetSnapOffset(actualThickness);
        }

        /// <summary>
        /// Converts millipoints (thousanths of 1/72nds of an inch) to pixels at 96 dots per inch.
        /// </summary>
        /// <param name="milliPoints">The number of milliPoints.</param>
        /// <returns>Pixels at the nominal resolution of 96 dots per inch. </returns>
        private double MilliPointsToNominalResolution(int milliPoints)
        {
            return milliPoints * (0.75 / 1000);
        }

        /// <summary>
        /// Converts nominal font sizes (1/96ths of an inch) to points (1/72nds of an inch).
        /// </summary>
        /// <param name="fontSize">The nominal font size, in units of 1/96th of an inch.</param>
        /// <returns>The font size in points.</returns>
        private double NominalFontSizeToPoints(double fontSize)
        {
            return fontSize * 0.75;
        }

        /// <summary>
        /// Determines an appropriate <see cref="FontStyle"/> to approximate the given font weight.
        /// </summary>
        /// <param name="fontWeight">The font weight.</param>
        /// <returns>The <see cref="FontStyle"/> that approximates the given font weight.</returns>
        private FontStyle ToFontStyle(double fontWeight)
        {
            return fontWeight < 700 ? FontStyle.Regular : FontStyle.Bold;
        }

        /// <summary>
        /// Converts a <see cref="OxyRect"/> to a <see cref="RectangleF"/>, taking into account DPI scaling.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <returns>The converted rectangle.</returns>
        private RectangleF Convert(OxyRect rect)
        {
            var left = this.Convert(rect.Left);
            var right = this.Convert(rect.Right);
            var top = this.Convert(rect.Top);
            var bottom = this.Convert(rect.Bottom);
            return RectangleF.FromLTRB(left, top, right, bottom);
        }

        /// <summary>
        /// Converts a <see cref="double"/> to a <see cref="float"/>, taking into account DPI scaling.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The converted value.</returns>
        private float Convert(double value)
        {
            return (float)value * this.DpiScale;
        }

        /// <summary>
        /// Converts <see cref="ScreenPoint"/> to a <see cref="PointF"/>, taking into account DPI scaling.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>The converted point.</returns>
        private PointF Convert(ScreenPoint point)
        {
            return new PointF(this.Convert(point.X), this.Convert(point.Y));
        }

        /// <summary>
        /// Converts a <see cref="float"/> to a <see cref="double"/>, applying reversed DPI scaling.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The converted value.</returns>
        private double ConvertBack(float value)
        {
            return value / this.DpiScale;
        }

        /// <summary>
        /// Converts <see cref="double"/> dash array to a <see cref="float"/> array, taking into account DPI scaling.
        /// </summary>
        /// <param name="values">The array of values.</param>
        /// <param name="strokeThickness">The stroke thickness.</param>
        /// <returns>The array of converted values.</returns>
        private float[] ConvertDashArray(double[] values, float strokeThickness)
        {
            var ret = new float[values.Length];
            for (var i = 0; i < values.Length; i++)
            {
                ret[i] = this.Convert(values[i]) * strokeThickness;
            }

            return ret;
        }

        /// <summary>
        /// Converts a <see cref="OxyRect"/> to a <see cref="RectangleF"/>, taking into account DPI scaling and snapping the corners to pixels.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <param name="snapOffset">The snapping offset.</param>
        /// <returns>The converted rectangle.</returns>
        private RectangleF ConvertSnap(OxyRect rect, float snapOffset)
        {
            var left = this.ConvertSnap(rect.Left, snapOffset);
            var right = this.ConvertSnap(rect.Right, snapOffset);
            var top = this.ConvertSnap(rect.Top, snapOffset);
            var bottom = this.ConvertSnap(rect.Bottom, snapOffset);
            return RectangleF.FromLTRB(left, top, right, bottom);
        }

        /// <summary>
        /// Converts a <see cref="double"/> to a <see cref="float"/>, taking into account DPI scaling and snapping the value to a pixel.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="snapOffset">The snapping offset.</param>
        /// <returns>The converted value.</returns>
        private float ConvertSnap(double value, float snapOffset)
        {
            return Snap(this.Convert(value), snapOffset);
        }

        /// <summary>
        /// Converts <see cref="ScreenPoint"/> to a <see cref="PointF"/>, taking into account DPI scaling and snapping the point to a pixel.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="snapOffset">The snapping offset.</param>
        /// <returns>The converted point.</returns>
        private PointF ConvertSnap(ScreenPoint point, float snapOffset)
        {
            return new PointF(this.ConvertSnap(point.X, snapOffset), this.ConvertSnap(point.Y, snapOffset));
        }

        /// <summary>
        /// Gets the <see cref="PointF"/>s that should actually be rendered from the list of <see cref="ScreenPoint"/>s, taking into account DPI scaling and snapping if necessary.
        /// </summary>
        /// <param name="screenPoints">The points.</param>
        /// <param name="strokeThickness">The stroke thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <returns>The actual points.</returns>
        private IEnumerable<PointF> GetActualPoints(IList<ScreenPoint> screenPoints, double strokeThickness, EdgeRenderingMode edgeRenderingMode)
        {
            switch (edgeRenderingMode)
            {
                case EdgeRenderingMode.Automatic when RenderContextBase.IsStraightLine(screenPoints):
                case EdgeRenderingMode.Adaptive when RenderContextBase.IsStraightLine(screenPoints):
                case EdgeRenderingMode.PreferSharpness:
                    var snapOffset = this.GetSnapOffset(strokeThickness, edgeRenderingMode);
                    return screenPoints.Select(p => this.ConvertSnap(p, snapOffset));
                default:
                    return screenPoints.Select(this.Convert);
            }
        }

        /// <summary>
        /// Gets the stroke thickness that should actually be used for rendering, taking into account DPI scaling and snapping if necessary.
        /// </summary>
        /// <param name="strokeThickness">The stroke thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <returns>The actual stroke thickness.</returns>
        private float GetActualThickness(double strokeThickness, EdgeRenderingMode edgeRenderingMode)
        {
            var scaledThickness = this.Convert(strokeThickness);
            if (edgeRenderingMode == EdgeRenderingMode.PreferSharpness)
            {
                scaledThickness = Snap(scaledThickness, 0);
            }

            return scaledThickness;
        }
    }
}
