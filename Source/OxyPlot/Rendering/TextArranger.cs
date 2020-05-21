// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextArranger.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// <summary>
//   Arranges text.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Rendering
{
    using System;
    using System.Linq;
    using System.Xml.Linq;

    /// <summary>
    /// Arranges text.
    /// </summary>
    public class TextArranger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextArranger" /> class.
        /// </summary>
        /// <param name="textMeasurer">The <see cref="ITextMeasurer"/> to be used by this instance.</param>
        /// <param name="textTrimmer">The <see cref="ITextTrimmer"/> to be used by this instance.</param>
        public TextArranger(ITextMeasurer textMeasurer, ITextTrimmer textTrimmer)
        {
            this.TextMeasurer = textMeasurer ?? throw new ArgumentNullException(nameof(textMeasurer));
            this.TextTrimmer = textTrimmer ?? throw new ArgumentNullException(nameof(textTrimmer));

            this.SquashTrimmedTextBounds = false;
        }

        /// <summary>
        /// Gets or sets the <see cref="ITextMeasurer"/> used by this instance.
        /// </summary>
        public ITextMeasurer TextMeasurer { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ITextTrimmer"/> used by this instance.
        /// </summary>
        public ITextTrimmer TextTrimmer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to squash the bounds of trimmed text to the size the text.
        /// </summary>
        public bool SquashTrimmedTextBounds { get; set; }

        /// <summary>
        /// Splits the text into multiple lines, and indicates where they should be rendered given the given target alignment.
        /// </summary>
        /// <param name="p">The position.</param>
        /// <param name="text">The text.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">Size of the font (in device independent units, 1/96 inch).</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <param name="rotation">The rotation angle.</param>
        /// <param name="horizontalAlignment">The horizontal alignment.</param>
        /// <param name="verticalAlignment">The vertical alignment.</param>
        /// <param name="maxSize">The maximum size of the text (in device independent units, 1/96 inch). If set to <c>null</c>, the text will not be clipped.</param>
        /// <param name="targetHorizontalAlignment">The horixontal alignment used to render the text.</param>
        /// <param name="targetVerticalAlignment">The vertical alignment used to render the text.</param>
        /// <param name="lines">The separate lines of text.</param>
        /// <param name="linePositions">The point at which to render each line.</param>
        /// <remarks>
        /// Non-null <paramref name="maxSize"/> is not supported.
        /// </remarks>
        public void ArrangeText(
            ScreenPoint p,
            string text,
            string fontFamily,
            double fontSize,
            double fontWeight,
            double rotation,
            HorizontalAlignment horizontalAlignment,
            VerticalAlignment verticalAlignment,
            OxySize? maxSize,
            HorizontalAlignment targetHorizontalAlignment,
            TextVerticalAlignment targetVerticalAlignment,
            out string[] lines,
            out ScreenPoint[] linePositions)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            // measure the size of the whole text
            var wholeSize = this.MeasureText(text, fontFamily, fontSize, fontWeight);

            // infer font metrics from wholeSize
            var metrics = this.TextMeasurer.GetFontMetrics(fontFamily, fontSize, fontWeight);
            var lineHeight = metrics.CellHeight;
            var leading = metrics.Leading;
            var descender = metrics.Descender;

            // get the rendering bounds from maxSize if necessary
            var bounds = wholeSize;
            if (maxSize != null)
            {
                bounds = new OxySize(Math.Ceiling(Math.Min(bounds.Width, maxSize.Value.Width)), Math.Ceiling(Math.Min(bounds.Height, maxSize.Value.Height)));
            }

            // split into lines
            lines = StringHelper.SplitLines(text);

            // if the text is too tall, we need to remove some lines
            if (bounds.Height < wholeSize.Height && lines.Length > 1)
            {
                var clippedLineCount = 1 + (int)((bounds.Height - lineHeight) / (lineHeight + leading));
                lines = lines.Take(clippedLineCount).ToArray();

                if (this.SquashTrimmedTextBounds)
                {
                    bounds = new OxySize(bounds.Width, ((lineHeight + leading) * lines.Length) - leading);
                }
            }

            // if the text is too wide, we need to trim the lines down a bit
            if (bounds.Width < wholeSize.Width)
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    lines[i] = this.TextTrimmer.Trim(this.TextMeasurer, lines[i], bounds.Width, fontFamily, fontSize, fontWeight);
                }

                if (this.SquashTrimmedTextBounds)
                {
                    bounds = new OxySize(lines.Max(l => this.TextMeasurer.MeasureTextWidth(l, fontFamily, fontSize, fontWeight)), bounds.Height);
                }
            }

            // do these once
            var sin = Math.Sin(rotation / 180.0 * Math.PI);
            var cos = Math.Cos(rotation / 180.0 * Math.PI);

            // turn metrics into vectors
            var offsetBoundsWidth = new ScreenVector(cos * bounds.Width, sin * bounds.Width);
            var offsetBoundsHeight = new ScreenVector(-sin * bounds.Height, cos * bounds.Height);

            var offsetLineHeight = new ScreenVector(-sin * lineHeight, cos * lineHeight);
            var offsetLeading = new ScreenVector(-sin * leading, cos * leading);
            var offsetDescender = new ScreenVector(-sin * descender, cos * descender);

            // align to bounds
            var offsetBoundsX = offsetBoundsWidth * 0.0; // keep centerline
            var offsetBoundsY = offsetBoundsHeight * (ResolveOffset(verticalAlignment) - 0.5); // find the top of the top line

            p += offsetBoundsX + offsetBoundsY;

            // align lines within bounds
            bool useBaselineOffset = targetVerticalAlignment == TextVerticalAlignment.Baseline;
            if (useBaselineOffset)
            {
                targetVerticalAlignment = TextVerticalAlignment.Bottom;
            }

            var offsetLineXRelative = ResolveOffset(targetHorizontalAlignment) - ResolveOffset(horizontalAlignment); // multiply later when we know the line width
            var offsetLineY = offsetLineHeight * (0.5 - ResolveOffset((VerticalAlignment)targetVerticalAlignment));

            if (useBaselineOffset)
            {
                offsetLineY -= offsetDescender;
            }

            // position the lines
            linePositions = new ScreenPoint[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var lineWidth = this.TextMeasurer.MeasureTextWidth(line, fontFamily, fontSize, fontWeight);

                var offsetLineWidth = new ScreenVector(cos * lineWidth, sin * lineWidth);

                var offsetLineX = offsetLineWidth * offsetLineXRelative;

                linePositions[i] = p + ((offsetLineHeight + offsetLeading) * i) + offsetLineX + offsetLineY;
            }
        }

        /// <summary>
        /// Measures the size of the text as it would be when arranged by the ArrangeText method."/>.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">Size of the font (in device independent units, 1/96 inch).</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <returns>The size of the text.</returns>
        /// <remarks>Returns an <see cref="OxySize"/> with no width and height if the text is <c>null</c> or empty.</remarks>
        public OxySize MeasureText(
            string text,
            string fontFamily,
            double fontSize,
            double fontWeight)
        {
            if (string.IsNullOrEmpty(text))
            {
                // It is a bit of a lie to do this here, but otherwise everyone else has to do it
                return OxySize.Empty;
            }

            var lines = StringHelper.SplitLines(text);

            var width = lines.Max(l => this.TextMeasurer.MeasureTextWidth(l, fontFamily, fontSize, fontWeight));

            var metrics = this.TextMeasurer.GetFontMetrics(fontFamily, fontSize, fontWeight);

            var cellHeight = metrics.CellHeight;
            var leading = metrics.Leading;
            var lineCount = lines.Length;

            var height = ((cellHeight  + leading) * lineCount) - leading;

            return new OxySize(width, height);
        }

        /// <summary>
        /// Translates a <see cref="HorizontalAlignment"/> into a relative offset.
        /// </summary>
        /// <param name="horizontalAlignment">The horizontal alignent.</param>
        /// <returns>The offset.</returns>
        /// <remarks>
        /// Left   -> -0.5
        /// Center ->  0.0
        /// Right  -> +0.5
        /// </remarks>
        private static double ResolveOffset(HorizontalAlignment horizontalAlignment)
        {
            return horizontalAlignment switch
            {
                HorizontalAlignment.Left => -0.5,
                HorizontalAlignment.Center => 0.0,
                HorizontalAlignment.Right => 0.5,
                _ => throw new ArgumentException(nameof(horizontalAlignment)),
            };
        }

        /// <summary>
        /// Translates a <see cref="VerticalAlignment"/> into a relative offset.
        /// </summary>
        /// <param name="verticalAlignment">The vertical alignment.</param>
        /// <returns>The offset.</returns>
        /// <remarks>
        /// Top    -> +0.5
        /// Middle ->  0.0
        /// Bottom -> -0.5
        /// </remarks>
        private static double ResolveOffset(VerticalAlignment verticalAlignment)
        {
            return verticalAlignment switch
            {
                VerticalAlignment.Top => 0.5,
                VerticalAlignment.Middle => 0.0,
                VerticalAlignment.Bottom => -0.5,
                _ => throw new ArgumentException(nameof(verticalAlignment)),
            };
        }
    }
}
