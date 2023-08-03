// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PortableDocument.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a document that can be output to PDF.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Represents a document that can be output to PDF.
    /// </summary>
    public class PortableDocument
    {
        /// <summary>
        /// The objects.
        /// </summary>
        private readonly List<PortableDocumentObject> objects = new List<PortableDocumentObject>();

        /// <summary>
        /// The stroke alpha cache.
        /// </summary>
        private readonly Dictionary<double, string> strokeAlphaCache = new Dictionary<double, string>();

        /// <summary>
        /// The fill alpha cache.
        /// </summary>
        private readonly Dictionary<double, string> fillAlphaCache = new Dictionary<double, string>();

        /// <summary>
        /// The font cache.
        /// </summary>
        private readonly Dictionary<PortableDocumentFont, string> fontCache = new Dictionary<PortableDocumentFont, string>();

        /// <summary>
        /// The image cache.
        /// </summary>
        private readonly Dictionary<PortableDocumentImage, string> imageCache = new Dictionary<PortableDocumentImage, string>();

        /// <summary>
        /// The catalog object.
        /// </summary>
        private readonly PortableDocumentObject catalog;

        /// <summary>
        /// The pages object.
        /// </summary>
        private readonly PortableDocumentObject pages;

        /// <summary>
        /// The metadata object.
        /// </summary>
        private readonly PortableDocumentObject metadata;

        /// <summary>
        /// The resources object.
        /// </summary>
        private readonly PortableDocumentObject resources;

        /// <summary>
        /// The fonts dictionary.
        /// </summary>
        private readonly Dictionary<string, object> fonts;

        /// <summary>
        /// The x objects dictionary.
        /// </summary>
        private readonly Dictionary<string, object> xobjects;

        /// <summary>
        /// The ext g state dictionary.
        /// </summary>
        private readonly Dictionary<string, object> extgstate;

        /// <summary>
        /// The page reference objects.
        /// </summary>
        private readonly IList<PortableDocumentObject> pageReferences = new List<PortableDocumentObject>();

        /// <summary>
        /// The current page contents
        /// </summary>
        private PortableDocumentObject currentPageContents;

        /// <summary>
        /// The current font
        /// </summary>
        private PortableDocumentFont currentFont;

        /// <summary>
        /// The current font size
        /// </summary>
        private double currentFontSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="PortableDocument" /> class.
        /// </summary>
        public PortableDocument()
        {
            this.metadata = this.AddObject();
            this.metadata["/CreationDate"] = DateTime.Now;

            this.catalog = this.AddObject(PdfWriter.ObjectType.Catalog);
            this.pages = this.AddObject(PdfWriter.ObjectType.Pages);
            this.catalog["/Pages"] = this.pages;

            this.fonts = new Dictionary<string, object>();
            this.xobjects = new Dictionary<string, object>();
            this.extgstate = new Dictionary<string, object>();
            this.resources = this.AddObject();

            // See chapter 10.1 - ProcSet is obsolete from version 1.4?
            this.resources["/ProcSet"] = new[] { "/PDF", "/Text", "/ImageB", "/ImageC", "/ImageI" };

            this.resources["/Font"] = this.fonts;
            this.resources["/XObject"] = this.xobjects;
            this.resources["/ExtGState"] = this.extgstate;

            this.currentFont = StandardFonts.Helvetica.GetFont(false, false);
            this.currentFontSize = 12;
        }

        /// <summary>
        /// Gets the width of the current page.
        /// </summary>
        /// <value>The width measured in points (1/72 inch).</value>
        public double PageWidth { get; private set; }

        /// <summary>
        /// Gets the height of the current page.
        /// </summary>
        /// <value>The height measured in points (1/72 inch).</value>
        public double PageHeight { get; private set; }

        /// <summary>
        /// Sets the title property.
        /// </summary>
        public string Title
        {
            set
            {
                this.metadata["/Title"] = EscapeString(value);
            }
        }

        /// <summary>
        /// Sets the author property.
        /// </summary>
        public string Author
        {
            set
            {
                this.metadata["/Author"] = EscapeString(value);
            }
        }

        /// <summary>
        /// Sets the subject property.
        /// </summary>
        public string Subject
        {
            set
            {
                this.metadata["/Subject"] = EscapeString(value);
            }
        }

        /// <summary>
        /// Sets the keywords property.
        /// </summary>
        public string Keywords
        {
            set
            {
                this.metadata["/Keywords"] = EscapeString(value);
            }
        }

        /// <summary>
        /// Sets the creator property.
        /// </summary>
        public string Creator
        {
            set
            {
                this.metadata["/Creator"] = EscapeString(value);
            }
        }

        /// <summary>
        /// Sets the producer property.
        /// </summary>
        public string Producer
        {
            set
            {
                this.metadata["/Producer"] = EscapeString(value);
            }
        }

        /// <summary>
        /// Sets the current line width.
        /// </summary>
        /// <param name="w">The line width in points.</param>
        public void SetLineWidth(double w)
        {
            this.AppendLine("{0:0.####} w", w);
        }

        /// <summary>
        /// Sets the line cap type.
        /// </summary>
        /// <param name="cap">The cap type.</param>
        public void SetLineCap(LineCap cap)
        {
            this.AppendLine("{0} J", (int)cap);
        }

        /// <summary>
        /// Sets the line join type.
        /// </summary>
        /// <param name="lineJoin">The line join.</param>
        public void SetLineJoin(LineJoin lineJoin)
        {
            this.AppendLine("{0} j", (int)lineJoin);
        }

        /// <summary>
        /// Sets the miter limit.
        /// </summary>
        /// <param name="ml">The limit.</param>
        public void SetMiterLimit(double ml)
        {
            this.AppendLine("{0:0.####} M", ml);
        }

        /// <summary>
        /// Sets the line dash pattern.
        /// </summary>
        /// <param name="dashArray">The dash array specifies the lengths of alternating dashes and gaps; the numbers must be nonnegative and not all zero.</param>
        /// <param name="dashPhase">The dash phase specifies the distance into dash pattern at which to start the dash.</param>
        /// <remarks>Before beginning to stroke a path, the dash array is cycled through, adding up the lengths of
        /// dashes and gaps. When the accumulated length equals the value specified by the dash phase, stroking
        /// of the path begins, and the dash array is used cyclically from that point onward.
        /// Table 4.6 shows examples of line dash patterns. As can be seen from the table, an empty dash array
        /// and zero phase can be used to restore the dash pattern to a solid line.</remarks>
        public void SetLineDashPattern(double[] dashArray, double dashPhase)
        {
            this.Append("[");
            for (int i = 0; i < dashArray.Length; i++)
            {
                if (i > 0)
                {
                    this.Append(" ");
                }

                this.Append("{0:0.####}", dashArray[i]);
            }

            this.AppendLine("]{0:0.####} d", dashPhase);
        }

        /// <summary>
        /// Resets the line dash pattern.
        /// </summary>
        public void ResetLineDashPattern()
        {
            this.SetLineDashPattern(new double[] { }, 0);
        }

        /// <summary>
        /// Moves to the specified coordinate.
        /// </summary>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <remarks>Begin a new subpath by moving the current point to coordinates (x, y), omitting any connecting line segment.
        /// If the previous path construction operator in the current path was also m, the new m overrides it;
        /// no vestige of the previous m operation remains in the path.</remarks>
        public void MoveTo(double x1, double y1)
        {
            this.AppendLine("{0:0.####} {1:0.####} m", x1, y1);
        }

        /// <summary>
        /// Appends a straight line segment to the current path.
        /// </summary>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <remarks>Append a straight line segment from the current point to the point (x, y). The new current point is (x, y).</remarks>
        public void LineTo(double x1, double y1)
        {
            this.AppendLine("{0:0.####} {1:0.####} l", x1, y1);
        }

        /// <summary>
        /// Appends a cubic Bézier curve to the current path.
        /// </summary>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        /// <param name="x3">The x3.</param>
        /// <param name="y3">The y3.</param>
        /// <remarks>The curve extends from the current point to the point (x3 , y3 ), using (x1 , y1 ) and (x2 , y2 )
        /// as the Bézier control points (see “Cubic Bézier Curves,” below). The new current point is (x3 , y3 ).</remarks>
        public void AppendCubicBezier(double x1, double y1, double x2, double y2, double x3, double y3)
        {
            this.AppendLine("{0:0.####} {1:0.####} {2:0.####} {3:0.####} {4:0.####} {5:0.####} c", x1, y1, x2, y2, x3, y3);
        }

        /// <summary>
        /// Saves the current graphics state.
        /// </summary>
        public void SaveState()
        {
            this.AppendLine("q");
        }

        /// <summary>
        /// Restores the graphics state.
        /// </summary>
        public void RestoreState()
        {
            this.AppendLine("Q");
        }

        /// <summary>
        /// Translates the current transformation matrix.
        /// </summary>
        /// <param name="x">The x-translation.</param>
        /// <param name="y">The y-translation.</param>
        public void Translate(double x, double y)
        {
            this.Transform(1, 0, 0, 1, x, y);
        }

        /// <summary>
        /// Scales the current transformation matrix.
        /// </summary>
        /// <param name="sx">The x-scale.</param>
        /// <param name="sy">The y-scale.</param>
        public void Scale(double sx, double sy)
        {
            this.Transform(sx, 0, 0, sy, 0, 0);
        }

        /// <summary>
        /// Modifies the current transformation matrix (CTM).
        /// </summary>
        /// <param name="a">The a.</param>
        /// <param name="b">The b.</param>
        /// <param name="c">The c.</param>
        /// <param name="d">The d.</param>
        /// <param name="e">The e.</param>
        /// <param name="f">The f.</param>
        /// <remarks>Modify the current transformation matrix (CTM) by concatenating the specified matrix
        /// (see Section 4.2.1, “Coordinate Spaces”). Although the operands specify a matrix, they
        /// are written as six separate numbers, not as an array.</remarks>
        public void Transform(double a, double b, double c, double d, double e, double f)
        {
            // Modify the current transformation matrix (CTM) by concatenating the specified matrix.
            this.AppendLine("{0:0.#####} {1:0.#####} {2:0.#####} {3:0.#####} {4:0.#####} {5:0.#####} cm", a, b, c, d, e, f);
        }

        /// <summary>
        /// Sets the vertical text scaling.
        /// </summary>
        /// <param name="scale">A number specifying the percentage of the normal height.</param>
        public void SetHorizontalTextScaling(double scale)
        {
            this.AppendLine("{0:0.#####} Tz", scale);
        }

        /// <summary>
        /// Rotates by the specified angle around the specified point.
        /// </summary>
        /// <param name="x">The x-coordinate of the rotation centre.</param>
        /// <param name="y">The y-coordinate of the rotation centre.</param>
        /// <param name="angle">The rotation angle in degrees.</param>
        public void RotateAt(double x, double y, double angle)
        {
            this.Translate(x, y);
            this.Rotate(angle);
            this.Translate(-x, -y);
        }

        /// <summary>
        /// Rotates by the specified angle.
        /// </summary>
        /// <param name="angle">The rotation angle in degrees.</param>
        public void Rotate(double angle)
        {
            double theta = angle / 180 * Math.PI;
            this.Transform(Math.Cos(theta), Math.Sin(theta), -Math.Sin(theta), Math.Cos(theta), 0, 0);
        }

        /// <summary>
        /// Sets the stroke alpha.
        /// </summary>
        /// <param name="alpha">The alpha value [0,1].</param>
        public void SetStrokeAlpha(double alpha)
        {
            var gs = GetCached(alpha, this.strokeAlphaCache, () => this.AddExtGState("/CA", alpha));
            this.AppendLine("{0:0.####} gs", gs);
        }

        /// <summary>
        /// Sets the fill alpha.
        /// </summary>
        /// <param name="alpha">The alpha value [0,1].</param>
        public void SetFillAlpha(double alpha)
        {
            var gs = GetCached(alpha, this.fillAlphaCache, () => this.AddExtGState("/ca", alpha));
            this.AppendLine("{0:0.####} gs", gs);
        }

        /// <summary>
        /// Strokes the path.
        /// </summary>
        /// <param name="close">Closes the path if set to <c>true</c>.</param>
        public void Stroke(bool close = true)
        {
            this.AppendLine(close ? "s" : "S");
        }

        /// <summary>
        /// Fills the path.
        /// </summary>
        /// <param name="evenOddRule">Use the even-odd fill rule if set to <c>true</c>. Use the nonzero winding number rule if set to <c>false</c>.</param>
        public void Fill(bool evenOddRule = false)
        {
            this.AppendLine(evenOddRule ? "f>" : "f");
        }

        /// <summary>
        /// Fills and strokes the path.
        /// </summary>
        /// <param name="close">Closes the path if set to <c>true</c>.</param>
        /// <param name="evenOddRule">Use the even-odd fill rule if set to <c>true</c>. Use the nonzero winding number rule if set to <c>false</c>.</param>
        public void FillAndStroke(bool close = true, bool evenOddRule = false)
        {
            if (evenOddRule)
            {
                this.AppendLine(close ? "b>" : "B>");
            }
            else
            {
                this.AppendLine(close ? "b" : "B");
            }
        }

        /// <summary>
        /// Sets the clipping path.
        /// </summary>
        /// <param name="evenOddRule">Use the even-odd fill rule if set to <c>true</c>. Use the nonzero winding number rule if set to <c>false</c>.</param>
        public void SetClippingPath(bool evenOddRule = false)
        {
            this.AppendLine(evenOddRule ? "W>" : "W");
        }

        /// <summary>
        /// Ends the path.
        /// </summary>
        /// <remarks>End the path object without filling or stroking it. This operator is a path-painting no-op,
        /// used primarily for the side effect of changing the current clipping path (see Section 4.4.3, “Clipping Path Operators”).</remarks>
        public void EndPath()
        {
            this.AppendLine("n");
        }

        /// <summary>
        /// Closes the subpath.
        /// </summary>
        /// <remarks>Close the current subpath by appending a straight line segment from the current point
        /// to the starting point of the subpath. If the current subpath is already closed, h does nothing.
        /// This operator terminates the current subpath. Appending another segment to the current
        /// path begins a new subpath, even if the new segment begins at the endpoint reached by the h operation.</remarks>
        public void CloseSubPath()
        {
            this.AppendLine("h");
        }

        /// <summary>
        /// Appends a rectangle to the current path.
        /// </summary>
        /// <param name="x">The x-coordinate of the lower-left corner.</param>
        /// <param name="y">The y-coordinate of the lower-left corner.</param>
        /// <param name="w">The width.</param>
        /// <param name="h">The height.</param>
        /// <remarks>Append a rectangle to the current path as a complete subpath,
        /// with lower-left corner (x, y) and dimensions width and height in user space.</remarks>
        public void AppendRectangle(double x, double y, double w, double h)
        {
            this.AppendLine("{0:0.####} {1:0.####} {2:0.####} {3:0.####} re", x, y, w, h);
        }

        /// <summary>
        /// Draws a line connecting the two points specified by the coordinate pairs.
        /// </summary>
        /// <param name="x1">The x-coordinate of the first point.</param>
        /// <param name="y1">The y-coordinate of the first point.</param>
        /// <param name="x2">The x-coordinate of the second point.</param>
        /// <param name="y2">The y-coordinate of the second point.</param>
        public void DrawLine(double x1, double y1, double x2, double y2)
        {
            this.AppendLine("{0:0.####} {1:0.####} m {2:0.####} {3:0.####} l S", x1, y1, x2, y2);
        }

        /// <summary>
        /// Draws a rectangle.
        /// </summary>
        /// <param name="x">The x-coordinate of the lower-left corner.</param>
        /// <param name="y">The y-coordinate of the lower-left corner.</param>
        /// <param name="w">The width.</param>
        /// <param name="h">The height.</param>
        /// <param name="fill">Fill the rectangle if set to <c>true</c>.</param>
        public void DrawRectangle(double x, double y, double w, double h, bool fill = false)
        {
            this.AppendLine("{0:0.####} {1:0.####} {2:0.####} {3:0.####} re {4}", x, y, w, h, fill ? "B" : "S");
        }

        /// <summary>
        /// Sets the clipping rectangle.
        /// </summary>
        /// <param name="x">The x-coordinate of the lower-left corner.</param>
        /// <param name="y">The y-coordinate of the lower-left corner.</param>
        /// <param name="w">The width.</param>
        /// <param name="h">The height.</param>
        /// <param name="evenOddRule">Use the even-odd region rule if set to <c>true</c>.</param>
        public void SetClippingRectangle(double x, double y, double w, double h, bool evenOddRule = false)
        {
            // TODO: not working?
            return;

            // Set clipping path using non-zero rule (W)
            // Set clipping path using even-odd rule (W*)
            // End path without filling or stroking (n)
            // TODO: this.AppendLine("{0} {1} {2} {3} re {4} n", x, y, w, h, evenOddRule ? "W*" : "W");
        }

        /// <summary>
        /// Fills a rectangle.
        /// </summary>
        /// <param name="x">The x-coordinate of the lower-left corner.</param>
        /// <param name="y">The y-coordinate of the lower-left corner.</param>
        /// <param name="w">The width.</param>
        /// <param name="h">The height.</param>
        public void FillRectangle(double x, double y, double w, double h)
        {
            this.AppendLine("{0:0.####} {1:0.####} {2:0.####} {3:0.####} re f", x, y, w, h);
        }

        /// <summary>
        /// Draws a circle.
        /// </summary>
        /// <param name="x">The x-coordinate of the center.</param>
        /// <param name="y">The y-coordinate of the center.</param>
        /// <param name="r">The radius.</param>
        /// <param name="fill">Fill the circle if set to <c>true</c>.</param>
        public void DrawCircle(double x, double y, double r, bool fill = false)
        {
            this.DrawEllipse(x - r, y - r, r * 2, r * 2, fill);
        }

        /// <summary>
        /// Fills a circle.
        /// </summary>
        /// <param name="x">The x-coordinate of the center.</param>
        /// <param name="y">The y-coordinate of the center.</param>
        /// <param name="r">The radius.</param>
        public void FillCircle(double x, double y, double r)
        {
            this.FillEllipse(x - r, y - r, r * 2, r * 2);
        }

        /// <summary>
        /// Draws an ellipse.
        /// </summary>
        /// <param name="x">The x-coordinate of the lower-left corner.</param>
        /// <param name="y">The y-coordinate of the lower-left corner.</param>
        /// <param name="w">The width.</param>
        /// <param name="h">The height.</param>
        /// <param name="fill">Fill the ellipse if set to <c>true</c>.</param>
        public void DrawEllipse(double x, double y, double w, double h, bool fill = false)
        {
            this.AppendEllipse(x, y, w, h);
            if (!fill)
            {
                this.Stroke();
            }
            else
            {
                this.FillAndStroke();
            }
        }

        /// <summary>
        /// Fills an ellipse.
        /// </summary>
        /// <param name="x">The x-coordinate of the lower-left corner.</param>
        /// <param name="y">The y-coordinate of the lower-left corner.</param>
        /// <param name="w">The width.</param>
        /// <param name="h">The height.</param>
        public void FillEllipse(double x, double y, double w, double h)
        {
            this.AppendEllipse(x, y, w, h);
            this.Fill();
        }

        /// <summary>
        /// Appends an ellipse to the current path.
        /// </summary>
        /// <param name="x">The x-coordinate of the lower-left corner.</param>
        /// <param name="y">The y-coordinate of the lower-left corner.</param>
        /// <param name="w">The width.</param>
        /// <param name="h">The height.</param>
        public void AppendEllipse(double x, double y, double w, double h)
        {
            const double Kappa = 0.5522848;
            var ox = w * 0.5 * Kappa; // control point offset horizontal
            var oy = h * 0.5 * Kappa; // control point offset vertical
            var xe = x + w; // x-end
            var ye = y + h; // y-end
            var xm = x + (w * 0.5); // x-middle
            var ym = y + (h * 0.5); // y-middle

            this.MoveTo(x, ym);
            this.AppendCubicBezier(x, ym - oy, xm - ox, y, xm, y);
            this.AppendCubicBezier(xm + ox, y, xe, ym - oy, xe, ym);
            this.AppendCubicBezier(xe, ym + oy, xm + ox, ye, xm, ye);
            this.AppendCubicBezier(xm - ox, ye, x, ym + oy, x, ym);
        }

        /// <summary>
        /// Sets the current font.
        /// </summary>
        /// <param name="fontName">The font name.</param>
        /// <param name="fontSize">The font size in points.</param>
        /// <param name="bold">Use bold font weight if set to <c>true</c>.</param>
        /// <param name="italic">Use italic style if set to <c>true</c>.</param>
        public void SetFont(string fontName, double fontSize, bool bold = false, bool italic = false)
        {
            this.currentFont = GetFont(fontName, bold, italic);
            this.currentFontSize = fontSize;
        }

        /// <summary>
        /// Draws the text at the specified coordinate.
        /// </summary>
        /// <param name="x">The left x-coordinate.</param>
        /// <param name="y">The bottom (!) y-coordinate.</param>
        /// <param name="text">The text.</param>
        public void DrawText(double x, double y, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            var fontId = GetCached(this.currentFont, this.fontCache, () => this.AddFont(this.currentFont));
            this.AppendLine("BT"); // Begin text object
            this.AppendLine("{0} {1:0.####} Tf", fontId, this.currentFontSize);
            text = EncodeString(text, this.currentFont.Encoding);
            text = EscapeString(text);

            y = y - (this.currentFont.Descent * this.currentFontSize / 1000);
            this.AppendLine("{0:0.####} {1:0.####} Td", x, y); // Move to the start of the next line, offset from the start of the current line by (tx , ty ). tx and ty are numbers expressed in unscaled text space units.
            this.AppendLine("{0} Tj", text); // Show text string
            this.AppendLine("ET"); // End text object
        }

        /// <summary>
        /// Measures the size of the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public void MeasureText(string text, out double width, out double height)
        {
            if (string.IsNullOrEmpty(text))
            {
                width = height = 0;
                return;
            }

            this.currentFont.Measure(text, this.currentFontSize, out width, out height);
        }

        /// <summary>
        /// Draws an image.
        /// </summary>
        /// <param name="image">The image to draw.</param>
        public void DrawImage(PortableDocumentImage image)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }

            var imageId = GetCached(image, this.imageCache, () => this.AddImage(image));
            this.AppendLine("{0} Do", imageId);
        }

        /// <summary>
        /// Sets the color in Device RGB color space.
        /// </summary>
        /// <param name="r">The red value.</param>
        /// <param name="g">The green value.</param>
        /// <param name="b">The blue value.</param>
        public void SetColor(double r, double g, double b)
        {
            this.AppendLine("{0:0.####} {1:0.####} {2:0.####} RG", r, g, b);
        }

        /// <summary>
        /// Sets the color in CMYK color space.
        /// </summary>
        /// <param name="c">The cyan value.</param>
        /// <param name="m">The magenta value.</param>
        /// <param name="y">The yellow value.</param>
        /// <param name="k">The black value.</param>
        public void SetColor(double c, double m, double y, double k)
        {
            this.AppendLine("{0:0.####} {1:0.####} {2:0.####} {3:0.####} K", c, m, y, k);
        }

        /// <summary>
        /// Sets the fill color in Device RGB color space.
        /// </summary>
        /// <param name="r">The red value.</param>
        /// <param name="g">The green value.</param>
        /// <param name="b">The blue value.</param>
        public void SetFillColor(double r, double g, double b)
        {
            this.AppendLine("{0:0.####} {1:0.####} {2:0.####} rg", r, g, b);
        }

        /// <summary>
        /// Adds a page.
        /// </summary>
        /// <param name="pageSize">The page size.</param>
        /// <param name="pageOrientation">The page orientation.</param>
        public void AddPage(PageSize pageSize, PageOrientation pageOrientation = PageOrientation.Portrait)
        {
            double shortLength = double.NaN, longLength = double.NaN;
            switch (pageSize)
            {
                case PageSize.A4:
                    shortLength = 595;
                    longLength = 842;
                    break;
                case PageSize.A3:
                    shortLength = 842;
                    longLength = 1190;
                    break;
                case PageSize.Letter:
                    shortLength = 612;
                    longLength = 792;
                    break;
            }

            if (pageOrientation == PageOrientation.Portrait)
            {
                this.AddPage(shortLength, longLength);
            }
            else
            {
                this.AddPage(longLength, shortLength);
            }
        }

        /// <summary>
        /// Adds a page specified by width and height.
        /// </summary>
        /// <param name="width">The page width in points.</param>
        /// <param name="height">The page height in points.</param>
        public void AddPage(double width = 595, double height = 842)
        {
            this.PageWidth = width;
            this.PageHeight = height;
            this.currentPageContents = this.AddObject();

            var page1 = this.AddObject(PdfWriter.ObjectType.Page);
            page1["/Parent"] = this.pages;
            page1["/MediaBox"] = new[] { 0d, 0d, width, height };
            page1["/Contents"] = this.currentPageContents;
            page1["/Resources"] = this.resources;
            this.pageReferences.Add(page1);
        }

        /// <summary>
        /// Saves the document to the specified stream.
        /// </summary>
        /// <param name="s">The output stream.</param>
        public void Save(Stream s)
        {
            using (var w = new PdfWriter(s))
            {
                // update the Pages dictionary
                this.pages["/Count"] = this.pageReferences.Count;
                this.pages["/Kids"] = this.pageReferences;

                // HEADER
                w.WriteLine("%PDF-1.3");

                // BODY
                var objectPosition = new Dictionary<PortableDocumentObject, long>();
                foreach (var o in this.objects)
                {
                    objectPosition.Add(o, w.Position);
                    o.Write(w);
                }

                // CROSS-REFERENCE TABLE
                var xrefPosition = w.Position;
                w.WriteLine("xref");
                w.WriteLine("0 {0}", this.objects.Count + 1);
                w.WriteLine("0000000000 65535 f ");
                foreach (var o in this.objects)
                {
                    w.WriteLine("{0:0000000000} 00000 n ", objectPosition[o]);
                }

                // TRAILER
                w.WriteLine("trailer");
                var trailer = new Dictionary<string, object>
                                  {
                                      { "/Size", this.objects.Count + 1 },
                                      { "/Root", this.catalog },
                                      { "/Info", this.metadata }
                                  };
                w.Write(trailer);
                w.WriteLine();

                // Start of cross-reference table
                w.WriteLine("startxref");
                w.WriteLine("{0}", xrefPosition);

                // write PDF end of file marker
                w.Write("%%EOF");
            }
        }

        /// <summary>
        /// Encodes the specified string.
        /// </summary>
        /// <param name="text">The text to encode.</param>
        /// <param name="encoding">The target encoding.</param>
        /// <returns>The encoded text</returns>
        private static string EncodeString(string text, FontEncoding encoding)
        {
            // TODO
            return text;
        }

        /// <summary>
        /// Escapes the specified string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The encoded string.</returns>
        private static string EscapeString(string text)
        {
            // Apply escape
            text = text.Replace("\\", "\\\\");
            text = text.Replace("(", "\\(");
            text = text.Replace(")", "\\)");

            // Enclose
            return "(" + text + ")";
        }

        /// <summary>
        /// Encodes binary bits into a plaintext ASCII85 format string
        /// </summary>
        /// <param name="ba">binary bits to encode</param>
        /// <returns>ASCII85 encoded string</returns>
        private static string Ascii85Encode(byte[] ba)
        {
            // http://en.wikipedia.org/wiki/Ascii85
            // http://www.codinghorror.com/blog/2005/10/c-implementation-of-ascii85.html
            // PDF reference section 3.3.2
            var encodedBlock = new byte[5];
            var sb = new StringBuilder(ba.Length * 5 / 4);
            const int AsciiOffset = 33;

            Action<int, uint> encodeBlock = (length, t) =>
                {
                    for (var i = encodedBlock.Length - 1; i >= 0; i--)
                    {
                        encodedBlock[i] = (byte)((t % 85) + AsciiOffset);
                        t /= 85;
                    }

                    for (var i = 0; i < length; i++)
                    {
                        sb.Append((char)encodedBlock[i]);
                    }
                };

            uint tuple = 0;
            int count = 0;
            foreach (byte b in ba)
            {
                if (count >= 4 - 1)
                {
                    tuple |= b;
                    if (tuple == 0)
                    {
                        sb.Append('z');
                    }
                    else
                    {
                        encodeBlock(encodedBlock.Length, tuple);
                    }

                    tuple = 0;
                    count = 0;
                }
                else
                {
                    tuple |= (uint)(b << (24 - (count * 8)));
                    count++;
                }
            }

            // if we have some bytes left over at the end..
            if (count > 0)
            {
                encodeBlock(count + 1, tuple);
            }

            // EOD marker
            sb.Append("~>");

            return sb.ToString();
        }

        /// <summary>
        /// Gets the font.
        /// </summary>
        /// <param name="fontName">Name of the font.</param>
        /// <param name="bold">Use bold if set to <c>true</c>.</param>
        /// <param name="italic">Use italic if set to <c>true</c>.</param>
        /// <returns>The font.</returns>
        private static PortableDocumentFont GetFont(string fontName, bool bold, bool italic)
        {
            if (fontName != null)
            {
                fontName = fontName.ToLower();
            }

            switch (fontName)
            {
                case "arial":
                case "helvetica":
                    return StandardFonts.Helvetica.GetFont(bold, italic);
                case "times":
                case "times new roman":
                    return StandardFonts.Times.GetFont(bold, italic);
                case "courier":
                case "courier new":
                    return StandardFonts.Courier.GetFont(bold, italic);
                default:
                    // Use Arial/Helvetica as default
                    return StandardFonts.Helvetica.GetFont(bold, italic);
            }
        }

        /// <summary>
        /// Gets a cached value.
        /// </summary>
        /// <typeparam name="T1">The type of the key.</typeparam>
        /// <typeparam name="T2">The type of the value.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="cache">The cache dictionary.</param>
        /// <param name="create">The create value function.</param>
        /// <returns>The cached or created value.</returns>
        private static T2 GetCached<T1, T2>(T1 key, Dictionary<T1, T2> cache, Func<T2> create)
        {
            T2 value;
            if (cache.TryGetValue(key, out value))
            {
                return value;
            }

            value = create();
            cache[key] = value;
            return value;
        }

        /// <summary>
        /// Adds an object to the document.
        /// </summary>
        /// <returns>The added object.</returns>
        private PortableDocumentObject AddObject()
        {
            var obj = new PortableDocumentObject(this.objects.Count + 1);
            this.objects.Add(obj);
            return obj;
        }

        /// <summary>
        /// Adds an object of the specified type.
        /// </summary>
        /// <param name="type">The object type.</param>
        /// <returns>The added object.</returns>
        private PortableDocumentObject AddObject(PdfWriter.ObjectType type)
        {
            var obj = this.AddObject();
            obj["/Type"] = type;
            return obj;
        }

        /// <summary>
        /// Adds an ExtGState object.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The added object.</returns>
        private string AddExtGState(string key, object value)
        {
            var gs = this.AddObject(PdfWriter.ObjectType.ExtGState);
            gs[key] = value;
            string statekey = "/GS" + this.extgstate.Count;
            this.extgstate.Add(statekey, gs);
            return statekey;
        }

        /// <summary>
        /// Adds an image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>The added object.</returns>
        private string AddImage(PortableDocumentImage image)
        {
            int i = this.xobjects.Count + 1;
            var imageObject = this.AddObject(PdfWriter.ObjectType.XObject);
            imageObject["/Subtype"] = "/Image";
            imageObject["/Width"] = image.Width;
            imageObject["/Height"] = image.Height;
            imageObject["/ColorSpace"] = "/" + image.ColorSpace;
            imageObject["/Interpolate"] = image.Interpolate;
            imageObject["/BitsPerComponent"] = image.BitsPerComponent;

            ////var flateData = Flate(bits);
            ////var encodedData = Ascii85Encode(flateData);
            ////image["/Length"] = encodedData.Length;
            ////image["/Filter"] = new[] { "/ASCII85Decode", "/FlateDecode" };

            var encodedData = Ascii85Encode(image.Bits);
            imageObject["/Length"] = encodedData.Length;
            imageObject["/Filter"] = "/ASCII85Decode";

            imageObject.Append(encodedData);
            string imageId = "/Image" + i;
            this.xobjects.Add(imageId, imageObject);

            if (image.MaskBits != null)
            {
                var maskImage = this.AddObject(PdfWriter.ObjectType.XObject);
                maskImage["/Subtype"] = "/Image";
                maskImage["/Width"] = image.Width;
                maskImage["/Height"] = image.Height;
                maskImage["/ColorSpace"] = "/DeviceGray";
                maskImage["/Interpolate"] = image.Interpolate;
                maskImage["/BitsPerComponent"] = image.BitsPerComponent;
                var encodedMaskData = Ascii85Encode(image.MaskBits);
                maskImage["/Length"] = encodedMaskData.Length;
                maskImage["/Filter"] = "/ASCII85Decode";
                maskImage.Append(encodedMaskData);
                imageObject["/SMask"] = maskImage;
            }

            return imageId;
        }

        /// <summary>
        /// Adds a font.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <returns>The added object.</returns>
        private string AddFont(PortableDocumentFont font)
        {
            //// For the standard 14 fonts, the entries FirstChar, LastChar, Widths, and FontDescriptor must either all be
            //// present or all be absent. Ordinarily, they are absent; specifying them enables a standard font to be overridden

            PortableDocumentObject fd = null;
            if (font.SubType != FontSubType.Type1)
            {
                fd = this.AddObject(PdfWriter.ObjectType.FontDescriptor);
                fd["/Ascent"] = font.Ascent;
                fd["/CapHeight"] = font.CapHeight;
                fd["/Descent"] = font.Descent;
                fd["/Flags"] = font.Flags;
                fd["/FontBBox"] = font.FontBoundingBox;
                fd["/ItalicAngle"] = font.ItalicAngle;
                fd["/StemV"] = font.StemV;
                fd["/XHeight"] = font.XHeight;
                fd["/FontName"] = "/" + font.FontName;
            }

            var f = this.AddObject(PdfWriter.ObjectType.Font);
            f["/Subtype"] = "/" + font.SubType;
            f["/Encoding"] = "/" + font.Encoding;
            f["/BaseFont"] = "/" + font.BaseFont;
            if (fd != null)
            {
                f["/FontDescriptor"] = fd;
            }

            if (font.SubType != FontSubType.Type1)
            {
                // Optional for Type 1 fonts
                f["/FirstChar"] = font.FirstChar;
                f["/LastChar"] = font.FirstChar + font.Widths.Length - 1;
                f["/Widths"] = font.Widths;
            }

            int i = this.fonts.Count + 1;
            string fontId = "/F" + i;
            this.fonts.Add(fontId, f);
            return fontId;
        }

        /// <summary>
        /// Appends a line to the current page contents.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments.</param>
        /// <exception cref="System.InvalidOperationException">Cannot add content before a page has been added.</exception>
        private void AppendLine(string format, params object[] args)
        {
            if (this.currentPageContents == null)
            {
                throw new InvalidOperationException("Cannot add content before a page has been added.");
            }

            this.currentPageContents.AppendLine(format, args);
        }

        /// <summary>
        /// Appends text to the current page contents.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="args">The arguments.</param>
        /// <exception cref="System.InvalidOperationException">Cannot add content before a page has been added.</exception>
        private void Append(string format, params object[] args)
        {
            if (this.currentPageContents == null)
            {
                throw new InvalidOperationException("Cannot add content before a page has been added.");
            }

            this.currentPageContents.Append(format, args);
        }

        /// <summary>
        /// Represents an object in the <see cref="PortableDocument" />.
        /// </summary>
        /// <remarks>The object contains a dictionary and text content.</remarks>
        internal class PortableDocumentObject : PdfWriter.IPortableDocumentObject
        {
            /// <summary>
            /// The dictionary
            /// </summary>
            private readonly Dictionary<string, object> dictionary;

            /// <summary>
            /// The object number
            /// </summary>
            private readonly int objectNumber;

            /// <summary>
            /// The contents
            /// </summary>
            private readonly StringBuilder contents;

            /// <summary>
            /// Initializes a new instance of the <see cref="PortableDocumentObject" /> class.
            /// </summary>
            /// <param name="objectNumber">The object number.</param>
            public PortableDocumentObject(int objectNumber)
            {
                this.objectNumber = objectNumber;
                this.contents = new StringBuilder();
                this.dictionary = new Dictionary<string, object>();
            }

            /// <summary>
            /// Gets the object number.
            /// </summary>
            /// <value>The object number.</value>
            public int ObjectNumber
            {
                get
                {
                    return this.objectNumber;
                }
            }

            /// <summary>
            /// Sets the dictionary value for the specified key.
            /// </summary>
            /// <value>The <see cref="System.Object" />.</value>
            /// <param name="key">The key.</param>
            /// <returns>The object.</returns>
            public object this[string key]
            {
                set
                {
                    this.dictionary[key] = value;
                }
            }

            /// <summary>
            /// Appends text to the content of the object.
            /// </summary>
            /// <param name="format">The format string.</param>
            /// <param name="args">The arguments.</param>
            public void Append(string format, params object[] args)
            {
                this.contents.Append(string.Format(CultureInfo.InvariantCulture, format, args));
            }

            /// <summary>
            /// Appends a line to the content of the object.
            /// </summary>
            /// <param name="format">The format string.</param>
            /// <param name="args">The arguments.</param>
            public void AppendLine(string format, params object[] args)
            {
                this.contents.AppendLine(string.Format(CultureInfo.InvariantCulture, format, args));
            }

            /// <summary>
            /// Writes the object to the specified <see cref="PdfWriter" />.
            /// </summary>
            /// <param name="w">The writer.</param>
            public void Write(PdfWriter w)
            {
                w.WriteLine("{0} 0 obj", this.ObjectNumber);

                byte[] streamBytes = null;

                if (this.contents != null && this.contents.Length > 0)
                {
                    var c = this.contents.ToString().Trim();

                    // convert to a byte[] buffer
                    streamBytes = new byte[c.Length];
                    for (int i = 0; i < c.Length; i++)
                    {
                        streamBytes[i] = (byte)c[i];
                    }

                    this.dictionary["/Length"] = streamBytes.Length;
                }

                w.Write(this.dictionary);
                w.WriteLine();

                if (streamBytes != null)
                {
                    w.WriteLine("stream");
                    w.Write(streamBytes);
                    w.WriteLine();
                    w.WriteLine("endstream");
                }

                w.WriteLine("endobj");
            }
        }
    }
}