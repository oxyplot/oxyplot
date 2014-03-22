// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PdfSharpTests.cs" company="OxyPlot">
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
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Pdf.Tests
{
    using System.IO;

    using NUnit.Framework;

    using PdfSharp.Drawing;
    using PdfSharp.Pdf;

    public class PdfSharpTests
    {
        private const string Folder = @"PdfSharpTests\";

        static PdfSharpTests()
        {
            if (!Directory.Exists(Folder))
            {
                Directory.CreateDirectory(Folder);
            }
        }

        [Test]
        [TestCase("Arial")]
        [TestCase("Times New Roman")]
        [TestCase("Courier New")]
        public void DrawString(string fontName)
        {
            var doc = new PdfDocument();
            doc.Options.NoCompression = true;
            doc.Options.CompressContentStreams = false;
            doc.Options.ColorMode = PdfColorMode.Rgb;
            var page = doc.AddPage();
            var g = XGraphics.FromPdfPage(page);
            var s = 72 / 25.4;
            g.DrawString("This is 12pt " + fontName + " regular.", new XFont(fontName, 12), XBrushes.Black, 20 * s, 20 * s);
            g.DrawString("This is 12pt " + fontName + " bold.", new XFont(fontName, 12, XFontStyle.Bold), XBrushes.Black, 20 * s, 30 * s);
            g.DrawString("This is 12pt " + fontName + " italic.", new XFont(fontName, 12, XFontStyle.Italic), XBrushes.Black, 20 * s, 40 * s);
            g.DrawString("This is 12pt " + fontName + " bold and italic.", new XFont(fontName, 12, XFontStyle.BoldItalic), XBrushes.Black, 20 * s, 50 * s);
            doc.Save(Folder + fontName + ".pdf");
        }

        [Test]
        public void DrawString_CharacterMap()
        {
            var doc = new PdfDocument();
            doc.Options.NoCompression = true;
            doc.Options.CompressContentStreams = false;
            doc.Options.ColorMode = PdfColorMode.Rgb;
            var page = doc.AddPage();
            page.Width = 20 * 17;
            page.Height = 20 * 17;
            var g = XGraphics.FromPdfPage(page);
            var font = new XFont("Arial", 18);
            for (int i = 32; i < 256; i++)
            {
                double x = 10 + 20 * (i % 16);
                double y = 10 + 20 * (i / 16);
                var s = ((char)i).ToString();
                g.DrawString(s, font, XBrushes.Black, x, y);
            }

            doc.Save(Folder + "DrawString_CharacterMap.pdf");
        }

        [Test, Ignore("Not supported?")]
        public void DrawString_SpecialCharacters()
        {
            var doc = new PdfDocument();
            doc.Options.NoCompression = true;
            doc.Options.CompressContentStreams = false;
            doc.Options.ColorMode = PdfColorMode.Rgb;
            var page = doc.AddPage();
            var g = XGraphics.FromPdfPage(page);
            var font = new XFont("Arial", 96);
            g.DrawString("π θ", font, XBrushes.Black, 200, 400);
            doc.Save(Folder + "DrawString_SpecialCharacters.pdf");
        }

        [Test]
        public void MeasureString()
        {
            var doc = new PdfDocument();
            doc.Options.NoCompression = true;
            doc.Options.CompressContentStreams = false;
            doc.Options.ColorMode = PdfColorMode.Rgb;
            var page = doc.AddPage();
            var g = XGraphics.FromPdfPage(page);

            var font = new XFont("Arial", 96);
            var text = "qjQJKæ";
            var size = g.MeasureString(text, font);
            g.DrawRectangle(new XPen(XColors.Blue), 50, 400, size.Width, size.Height);
            var sf = new XStringFormat { Alignment = XStringAlignment.Near, LineAlignment = XLineAlignment.Near };
            g.DrawString(text, font, XBrushes.Black, 50, 400, sf);
            doc.Save(Folder + "MeasureString.pdf");
        }

        [Test]
        public void DrawLine_LineDashPatterns()
        {
            var doc = new PdfDocument();
            doc.Options.NoCompression = true;
            doc.Options.CompressContentStreams = false;
            doc.Options.ColorMode = PdfColorMode.Rgb;
            var page = doc.AddPage();
            page.Width = 100;
            page.Height = 100;
            var g = XGraphics.FromPdfPage(page);

            double x0 = 10;
            double x1 = 40;
            double y = 5;
            double dy = 5;

            g.DrawLine(new XPen(XColors.Black, 1), x0, y += dy, x1, y);

            g.DrawLine(new XPen(XColors.Black, 1) { DashPattern = new double[] { }, DashOffset = 0 }, x0, y += dy, x1, y);

            g.DrawLine(new XPen(XColors.Black, 1) { DashPattern = new[] { 3d, 3d }, DashOffset = 0 }, x0, y += dy, x1, y);

            g.DrawLine(new XPen(XColors.Black, 1) { DashPattern = new[] { 2d, 2d }, DashOffset = 1 }, x0, y += dy, x1, y);

            g.DrawLine(new XPen(XColors.Black, 1) { DashPattern = new[] { 2d, 1d }, DashOffset = 0 }, x0, y += dy, x1, y);

            g.DrawLine(new XPen(XColors.Black, 1) { DashPattern = new[] { 3d, 5d }, DashOffset = 6 }, x0, y += dy, x1, y);
            g.DrawLine(new XPen(XColors.Black, 1) { DashPattern = new[] { 2d, 3d }, DashOffset = 11 }, x0, y += dy, x1, y);

            doc.Save(Folder + @"DrawLine_DashPatterns.pdf");
        }
    }
}