// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockTextMeasurer.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a predicatble implementation of ITextMeasurer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using OxyPlot.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace OxyPlot.Tests
{
    public class MockTextMeasurer : ITextMeasurer
    {
        public MockTextMeasurer()
        {
        }

        public double Ascent { get; set; } = 0.6;
        public double Descent { get; set; } = 0.3;
        public double Leading { get; set; } = 0.1;
        public double CharacterWidth { get; set; } = 0.8;

        public HashSet<char> CharsWithWidth { get; } = new HashSet<char>();

        public FontMetrics GetFontMetrics(string fontFamily, double fontSize, double fontWeight)
        {
            return new FontMetrics(fontSize * this.Ascent, fontSize * this.Descent, fontSize * this.Leading);
        }

        public double MeasureTextWidth(string text, string fontFamily, double fontSize, double fontWeight)
        {
            return fontSize * this.CharacterWidth * text.Count(CharsWithWidth.Contains);
        }

        public void AddBasicAlphabet()
        {
            for (char c = 'a'; c <= 'z'; c++)
            {
                this.CharsWithWidth.Add(c);
                this.CharsWithWidth.Add(char.ToUpperInvariant(c));
            }
        }

        public void AddBasicWhitespace()
        {
            this.CharsWithWidth.Add(' ');
        }

        public void AddEllipsisChars()
        {
            this.CharsWithWidth.Add(SimpleTextTrimmer.AsciiEllipsis[0]);
            this.CharsWithWidth.Add(SimpleTextTrimmer.UnicodeEllipsis[0]);
        }
    }
}
