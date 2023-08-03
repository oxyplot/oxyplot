// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PortableDocumentTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Text;

    using NUnit.Framework;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class PortableDocumentTests
    {
        private const string Folder = @"PortableDocumentTests\";

        [Test]
        public void AddPage_EmptyDocument()
        {
            var doc = new PortableDocument();
            doc.AddPage(PageSize.A4);
            doc.Save(Folder + "EmptyDocument.pdf");
        }

        [Test]
        public void AddProperties_EmptyDocument()
        {
            var doc = new PortableDocument
            {
                Title = "The title",
                Author = "the author",
                Subject = "the subject",
                Keywords = "key;words",
                Creator = "the creator",
                Producer = "the producer"
            };
            doc.AddPage(PageSize.A4);
            doc.Save(Folder + "Properties.pdf");
        }

        [Test]
        public void DrawText_HelloWorld()
        {
            var doc = new PortableDocument();
            doc.AddPage(PageSize.A4);
            doc.SetFont("Arial", 96);
            doc.DrawText(50, 400, "Hello world!");
            doc.Save(Folder + "DrawText.pdf");
        }

        [Test, Ignore("Not supported")]
        public void DrawText_SpecialCharacters()
        {
            var doc = new PortableDocument();
            doc.AddPage(PageSize.A4);
            doc.SetFont("Arial", 96);
            var s = "π";
            doc.DrawText(50, 400, s);
            doc.Save(Folder + "DrawText_SpecialCharacters.pdf");
            Assert.IsTrue(s[0] > 255);
        }

        [Test]
        public void DrawText_TopLeftCoordinateSystem()
        {
            var doc = new PortableDocument();
            doc.AddPage(PageSize.A4);
            doc.Transform(1, 0, 0, -1, 0, doc.PageHeight);
            doc.SetHorizontalTextScaling(-100);

            // Note: negative font size
            doc.SetFont("Arial", -20);
            doc.DrawText(5, 25, "Hello world!");

            doc.SetColor(OxyColors.Blue);
            doc.DrawCross(5, 25);

            doc.SetColor(OxyColors.Blue);
            doc.SetFillColor(OxyColors.LightBlue);
            doc.DrawEllipse(50, 100, 50, 40, true);

            doc.Save(Folder + "DrawText_TopLeftCoordinateSystem.pdf");
        }

        [Test]
        public void DrawText_CharacterMap()
        {
            var doc = new PortableDocument();
            doc.AddPage(20 * 17, 20 * 17);
            doc.SetFont("Arial", 18);
            var sb = new StringBuilder();
            for (int i = 32; i < 256; i++)
            {
                double x = 10 + (20 * (i % 16));
                double y = doc.PageHeight - 10 - (20 * (i / 16));
                var s = ((char)i).ToString(CultureInfo.InvariantCulture);
                doc.DrawText(x, y, s);
                sb.Append(s);
                if (i % 16 == 15)
                {
                    sb.AppendLine();
                }
            }

            doc.Save(Folder + "DrawText_CharacterMap.pdf");
            File.WriteAllText(Folder + "DrawText_CharacterMap.txt", sb.ToString(), Encoding.UTF8);
        }

        [Test]
        public void MeasureText()
        {
            var doc = new PortableDocument();
            doc.AddPage(PageSize.A4);

            doc.SetFont("Arial", 96);

            var text = "qjQJKæ";
            double width, height;
            doc.MeasureText(text, out width, out height);
            double y = doc.PageHeight - 400 - height;
            doc.SetColor(0, 0, 1);
            doc.DrawRectangle(50, y, width, height);
            doc.SetFillColor(0, 0, 0);
            doc.DrawText(50, y, text);
            doc.Save(Folder + "MeasureText.pdf");
        }

        [Test]
        public void DrawText_Rotated()
        {
            var doc = new PortableDocument();
            doc.AddPage(200, 200);
            doc.SetFont("Arial", 12);
            for (int i = 0; i <= 360; i += 30)
            {
                doc.SaveState();
                doc.RotateAt(100, 100, i);
                doc.DrawText(100, 100, "Hello world!");
                doc.RestoreState();
            }

            doc.Save(Folder + "DrawText_Rotated.pdf");
        }

        [Test]
        public void DrawText_Rotated2()
        {
            var doc = new PortableDocument();
            doc.AddPage(200, 200);
            doc.SetFont("Arial", 12);
            for (int i = 0; i <= 360; i += 30)
            {
                doc.SaveState();
                doc.Translate(100, 100);
                doc.Rotate(i);
                doc.DrawText(0, 0, "Hello world!");
                doc.RestoreState();
            }

            doc.Save(Folder + "DrawText_Rotated2.pdf");
        }

        [Test]
        public void DrawCircle()
        {
            var doc = new PortableDocument();
            doc.AddPage(200, 200);
            doc.SetColor(OxyColors.Blue);
            doc.SetFillColor(OxyColors.LightBlue);
            doc.DrawCircle(100, 100, 95, true);
            doc.DrawCircle(185, 185, 5);
            doc.Save(Folder + "DrawCircle.pdf");
        }

        [Test]
        public void FillCircle()
        {
            var doc = new PortableDocument();
            doc.AddPage(200, 200);
            doc.SetColor(OxyColors.Blue);
            doc.SetFillColor(OxyColors.LightBlue);
            doc.FillCircle(100, 100, 95);
            doc.Save(Folder + "FillCircle.pdf");
        }

        [Test]
        public void DrawEllipse()
        {
            var doc = new PortableDocument();
            doc.AddPage(200, 100);
            doc.SetColor(OxyColors.Orange);
            doc.SetFillColor(OxyColors.LightGreen);
            doc.DrawEllipse(5, 5, 190, 90, true);
            doc.DrawEllipse(175, 85, 20, 10);
            doc.Save(Folder + "DrawEllipse.pdf");
        }

        [Test]
        public void FillEllipse()
        {
            var doc = new PortableDocument();
            doc.AddPage(200, 100);
            doc.SetColor(OxyColors.Orange);
            doc.SetFillColor(OxyColors.LightGreen);
            doc.FillEllipse(5, 5, 190, 90);
            doc.Save(Folder + "FillEllipse.pdf");
        }

        [Test]
        public void DrawLine()
        {
            var doc = new PortableDocument();
            doc.AddPage(200, 100);
            doc.DrawLine(50, 50, 100, 70);
            doc.Save(Folder + "DrawLine.pdf");
        }

        [Test]
        public void DrawLine_Colors()
        {
            var doc = new PortableDocument();
            doc.AddPage(100, 100);
            double x = 0;
            double y0 = 78;
            double y1 = 10;

            doc.DrawLine(10, 95, 10, 80);

            doc.SetColor(0, 0, 0);
            doc.DrawLine(x += 10, y0, x, y1);

            doc.SetColor(1, 1, 1);
            doc.DrawLine(x += 10, y0, x, y1);

            doc.SetColor(1, 0, 0);
            doc.DrawLine(x += 10, y0, x, y1);

            doc.SetColor(0, 1, 0);
            doc.DrawLine(x += 10, y0, x, y1);

            doc.SetColor(0, 0, 1);
            doc.DrawLine(x += 10, y0, x, y1);

            doc.SetColor(1, 1, 0);
            doc.DrawLine(x += 10, y0, x, y1);

            doc.Save(Folder + "DrawLine_Colors.pdf");
        }

        [Test]
        public void DrawLine_LineWidths()
        {
            var doc = new PortableDocument();
            doc.AddPage(100, 100);
            double x = 0;
            double y0 = 78;
            double y1 = 10;

            doc.DrawLine(20, 95, 20, 80);

            doc.SetLineWidth(0.1);
            doc.DrawLine(x += 10, y0, x, y1);

            doc.SetLineWidth(1);
            doc.DrawLine(x += 10, y0, x, y1);

            doc.SetLineWidth(2);
            doc.DrawLine(x += 10, y0, x, y1);

            doc.SetLineWidth(3);
            doc.DrawLine(x += 10, y0, x, y1);

            doc.Save(Folder + "DrawLine_LineWidths.pdf");
        }

        [Test]
        public void DrawLine_LineDashPatterns()
        {
            var doc = new PortableDocument();
            doc.AddPage(100, 100);
            double x0 = 10;
            double x1 = 40;
            double y = 100;
            double dy = -5;

            doc.SetLineWidth(1);

            // default dash pattern
            doc.DrawLine(x0, y += dy, x1, y);

            doc.SetLineDashPattern(new double[] { }, 0);
            doc.DrawLine(x0, y += dy, x1, y);

            doc.SetLineDashPattern(new[] { 3d }, 0);
            doc.DrawLine(x0, y += dy, x1, y);

            doc.SetLineDashPattern(new[] { 2d }, 1);
            doc.DrawLine(x0, y += dy, x1, y);

            doc.SetLineDashPattern(new[] { 2d, 1 }, 0);
            doc.DrawLine(x0, y += dy, x1, y);

            doc.SetLineDashPattern(new[] { 3d, 5 }, 6);
            doc.DrawLine(x0, y += dy, x1, y);

            doc.SetLineDashPattern(new[] { 2d, 3 }, 11);
            doc.DrawLine(x0, y += dy, x1, y);

            doc.Save(Folder + "DrawLine_LineDashPatterns.pdf");
        }

        [Test]
        public void Stroke_LineJoins()
        {
            var doc = new PortableDocument();
            doc.AddPage(100, 100);

            doc.SetLineWidth(3);
            doc.MoveTo(10, 10);
            doc.LineTo(50, 60);
            doc.LineTo(90, 10);
            doc.Stroke(false);

            doc.SetColor(1, 0, 0);
            doc.SetLineJoin(LineJoin.Bevel);
            doc.MoveTo(10, 20);
            doc.LineTo(50, 70);
            doc.LineTo(90, 20);
            doc.Stroke(false);

            doc.SetColor(0, 1, 0);
            doc.SetLineJoin(LineJoin.Miter);
            doc.MoveTo(10, 30);
            doc.LineTo(50, 80);
            doc.LineTo(90, 30);
            doc.Stroke(false);

            doc.SetColor(0, 0, 1);
            doc.SetLineJoin(LineJoin.Round);
            doc.MoveTo(10, 40);
            doc.LineTo(50, 90);
            doc.LineTo(90, 40);
            doc.Stroke(false);

            doc.Save(Folder + "Stroke_LineJoins.pdf");
        }

        [Test]
        public void Stroke_LineCaps()
        {
            var doc = new PortableDocument();
            doc.AddPage(100, 100);

            doc.SetColor(0.5, 0.5, 0.5);
            doc.SetLineWidth(3);
            doc.MoveTo(10, 10);
            doc.LineTo(50, 60);
            doc.LineTo(90, 10);
            doc.Stroke(false);

            doc.SetColor(1, 0, 0);
            doc.SetLineCap(LineCap.Butt);
            doc.MoveTo(10, 20);
            doc.LineTo(50, 70);
            doc.LineTo(90, 20);
            doc.Stroke(false);

            doc.SetColor(0, 1, 0);
            doc.SetLineCap(LineCap.ProjectingSquare);
            doc.MoveTo(10, 30);
            doc.LineTo(50, 80);
            doc.LineTo(90, 30);
            doc.Stroke(false);

            doc.SetColor(0, 0, 1);
            doc.SetLineCap(LineCap.Round);
            doc.MoveTo(10, 40);
            doc.LineTo(50, 90);
            doc.LineTo(90, 40);
            doc.Stroke(false);

            doc.SetColor(0, 0, 0);
            doc.SetLineWidth(0.1);
            doc.MoveTo(10, 10);
            doc.LineTo(50, 60);
            doc.LineTo(90, 10);
            doc.MoveTo(10, 20);
            doc.LineTo(50, 70);
            doc.LineTo(90, 20);
            doc.MoveTo(10, 30);
            doc.LineTo(50, 80);
            doc.LineTo(90, 30);
            doc.MoveTo(10, 40);
            doc.LineTo(50, 90);
            doc.LineTo(90, 40);
            doc.Stroke(false);

            doc.Save(Folder + "Stroke_LineCaps.pdf");
        }

        [Test]
        public void DrawPolygon()
        {
            var doc = new PortableDocument();
            doc.AddPage(200, 100);

            doc.MoveTo(50, 30);
            doc.LineTo(170, 30);
            doc.LineTo(100, 70);
            doc.SetColor(OxyColors.Orange);
            doc.SetFillColor(OxyColors.LightGreen);
            doc.FillAndStroke();

            doc.MoveTo(5, 5);
            doc.LineTo(5, 25);
            doc.LineTo(25, 5);
            doc.Fill();

            doc.MoveTo(195, 95);
            doc.LineTo(175, 95);
            doc.LineTo(195, 75);
            doc.Stroke();

            doc.Save(Folder + "DrawPolygon.pdf");
        }

        [Test]
        public void DrawRectangle()
        {
            var doc = new PortableDocument();
            doc.AddPage(200, 100);
            doc.SetColor(OxyColors.Navy);
            doc.SetFillColor(OxyColors.Gainsboro);
            doc.DrawRectangle(5, 5, 100, 70, true);
            doc.DrawRectangle(185, 85, 10, 10);
            doc.Save(Folder + "DrawRectangle.pdf");
        }

        [Test]
        public void FillRectangle()
        {
            var doc = new PortableDocument();
            doc.AddPage(200, 100);
            doc.SetColor(OxyColors.Gainsboro);
            doc.SetFillColor(OxyColors.Navy);
            doc.FillRectangle(5, 5, 100, 70);
            doc.Save(Folder + "FillRectangle.pdf");
        }

        [Test, Ignore("Not implemented")]
        public void DrawImage()
        {
            var doc = new PortableDocument();
            doc.AddPage(200, 100);
            //// var image = new PortableDocument.Image() { };
            //// doc.DrawImage(image);
            doc.Save(Folder + "DrawImage.pdf");
        }

        [Test]
        public void SetClippingRectangle()
        {
            var doc = new PortableDocument();
            doc.AddPage(200, 200);
            doc.SetColor(OxyColors.Blue);
            doc.SetFillColor(OxyColors.LightBlue);
            doc.SaveState();
            doc.SetClippingRectangle(5, 5, 50, 50);
            doc.DrawCircle(100, 100, 95, true);
            doc.RestoreState();
            doc.DrawCircle(120, 120, 70);
            doc.Save(Folder + "SetClippingRectangle.pdf");
        }

        [Test]
        public void Translate()
        {
            var doc = new PortableDocument();
            doc.AddPage(200, 200);

            doc.SaveState();
            doc.SetColor(1, 0, 0);
            doc.Translate(20, 10);
            doc.DrawRectangle(10, 10, 100, 70);
            doc.RestoreState();

            doc.DrawRectangle(10, 10, 100, 70);

            doc.Save(Folder + "Translate.pdf");
        }

        [Test]
        public void Rotate()
        {
            var doc = new PortableDocument();
            doc.AddPage(200, 200);

            doc.SaveState();
            doc.SetColor(1, 0, 0);
            doc.Rotate(30);
            doc.DrawRectangle(50, 20, 100, 15);
            doc.RestoreState();

            doc.DrawRectangle(50, 20, 100, 15);

            doc.Save(Folder + "Rotate.pdf");
        }

        [Test]
        public void RotateAt()
        {
            var doc = new PortableDocument();
            doc.AddPage(200, 200);

            doc.SaveState();
            doc.SetColor(1, 0, 0);
            doc.RotateAt(50, 20, 30);
            doc.DrawRectangle(50, 20, 100, 15);
            doc.RestoreState();

            doc.DrawRectangle(50, 20, 100, 15);

            doc.Save(Folder + "RotateAt.pdf");
        }

        [Test]
        public void FontFaces()
        {
            var doc = new PortableDocument();
            doc.AddPage(PageSize.A4);
            double x = 20 / 25.4 * 72;
            double dy = 10 / 25.4 * 72;
            double y = doc.PageHeight - dy;
            doc.DrawText(x, y -= dy, "This is the default font.");
            doc.SetFont("Courier", 12);
            doc.DrawText(x, y -= dy, "This is courier normal.");
            doc.SetFont("Times", 12, false, true);
            doc.DrawText(x, y -= dy, "This is times italic.");
            doc.SetFont("Helvetica", 12, true);
            doc.DrawText(x, y -= dy, "This is helvetica bold.");
            doc.SetFont("Courier", 12, true, true);
            doc.DrawText(x, y, "This is courier bolditalic.");
            doc.Save(Folder + "FontFaces.pdf");
        }

        [Test]
        [TestCase("Helvetica")]
        [TestCase("Times")]
        [TestCase("Courier")]
        public void FontFace(string fontName)
        {
            var doc = new PortableDocument();
            doc.AddPage(PageSize.A4);
            doc.SetFont(fontName, 12);
            double x = 20 / 25.4 * 72;
            double dy = 10 / 25.4 * 72;
            double y = doc.PageHeight - dy;
            doc.DrawText(x, y -= dy, "This is 12pt " + fontName + " regular.");
            doc.SetFont(fontName, 12, true);
            doc.DrawText(x, y -= dy, "This is 12pt " + fontName + " bold.");
            doc.SetFont(fontName, 12, false, true);
            doc.DrawText(x, y -= dy, "This is 12pt " + fontName + " italic.");
            doc.SetFont(fontName, 12, true, true);
            doc.DrawText(x, y, "This is 12pt " + fontName + " bold and italic.");
            doc.Save(Folder + "FontFace_" + fontName + ".pdf");
        }

        [Test]
        public void Transparency()
        {
            var doc = new PortableDocument();
            doc.AddPage(220, 100);
            doc.SetFillColor(OxyColors.Black);
            doc.FillRectangle(0, 45, 220, 10);
            for (int i = 0; i <= 10; i++)
            {
                doc.SetFillColor(OxyColor.FromAColor((byte)(255d * i / 10), OxyColors.Gold));
                doc.FillEllipse((i * 20) + 1, 41, 18, 18);
            }

            doc.Save(Folder + "Transparency.pdf");
        }
    }
}