using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace OxyPlot
{
    /// <summary>
    /// Scalable Vector Graphics writer.
    /// </summary>
    public class SvgWriter : XmlWriterBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether this writer should produce a stand-alone document.
        /// </summary>
        public bool IsDocument { get; set; }

        /// <summary>
        /// Gets or sets the number format.
        /// </summary>
        /// <value>The number format.</value>
        public string NumberFormat { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgWriter"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="isDocument">if set to <c>true</c> [is document].</param>
        public SvgWriter(Stream stream, double width, double height, bool isDocument = true)
            : base(stream)
        {
            IsDocument = isDocument;
            NumberFormat = "0.####";
            WriteHeader(width, height);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgWriter"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public SvgWriter(string path, double width, double height)
            : base(path)
        {
            IsDocument = true;
            NumberFormat = "0.####";
            WriteHeader(width, height);
        }

        private void WriteHeader(double width, double height)
        {

            // http://www.w3.org/TR/SVG/struct.html#SVGElement

            if (IsDocument)
            {
                WriteStartDocument(false);
                WriteDocType("svg", "-//W3C//DTD SVG 1.1//EN", "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd", null);
            }
            WriteStartElement("svg", "http://www.w3.org/2000/svg");
            WriteAttributeString("width", GetAutoValue(width, "100%"));
            WriteAttributeString("height", GetAutoValue(height, "100%"));
            WriteAttributeString("version", "1.1");
        }

        private string GetAutoValue(double value, string auto)
        {
            if (double.IsNaN(value))
                return auto;
            return value.ToString(NumberFormat, CultureInfo.InvariantCulture);
        }

        protected string ColorToString(OxyColor color)
        {
            if (color == OxyColors.Black)
                return "black";
            return String.Format("rgb({0:" + NumberFormat + "},{1:" + NumberFormat + "},{2:" + NumberFormat + "})", color.R, color.G, color.B);
        }

        public string CreateStyle(OxyColor fill, OxyColor stroke, double thickness, double[] dashArray, OxyPenLineJoin lineJoin = OxyPenLineJoin.Miter)
        {
            // http://oreilly.com/catalog/svgess/chapter/ch03.html
            var style = new StringBuilder();
            if (fill == null)
                style.AppendFormat("fill:none;");
            else
            {
                style.AppendFormat("fill:{0};", ColorToString(fill));
                if (fill.A != 0xFF)
                    style.AppendFormat(CultureInfo.InvariantCulture, "fill-opacity:{0};", fill.A / 255.0);

            }
            if (stroke == null)
            {
                style.AppendFormat("stroke:none;");

            }
            else
            {
                style.AppendFormat("stroke:{0};stroke-width:{1:" + NumberFormat + "}", ColorToString(stroke), thickness);
                switch (lineJoin)
                {
                    case OxyPenLineJoin.Round:
                        style.AppendFormat(";stroke-linejoin:round");
                        break;
                    case OxyPenLineJoin.Bevel:
                        style.AppendFormat(";stroke-linejoin:bevel");
                        break;
                }

                if (stroke.A != 0xFF)
                    style.AppendFormat(CultureInfo.InvariantCulture, ";stroke-opacity:{0}", stroke.A/255.0);

                if (dashArray != null && dashArray.Length > 0)
                {
                    style.Append(";stroke-dasharray:");
                    for (int i = 0; i < dashArray.Length; i++)
                        style.AppendFormat(CultureInfo.InvariantCulture, "{0}{1}", i > 0 ? "," : "", dashArray[i]);
                }
            }
            return style.ToString();
        }

        protected void WriteAttributeString(string name, double value)
        {
            WriteAttributeString(name, value.ToString(NumberFormat, CultureInfo.InvariantCulture));
        }

        public void WriteLine(ScreenPoint p1, ScreenPoint p2, string style)
        {
            // http://www.w3.org/TR/SVG/shapes.html#LineElement
            // http://www.w3schools.com/svg/svg_line.asp
            WriteStartElement("line");
            WriteAttributeString("x1", p1.X);
            WriteAttributeString("y1", p1.Y);
            WriteAttributeString("x2", p2.X);
            WriteAttributeString("y2", p2.Y);
            WriteAttributeString("style", style);
            WriteEndElement();
        }

        public void WritePolyline(IEnumerable<ScreenPoint> pts, string style)
        {
            // http://www.w3.org/TR/SVG/shapes.html#PolylineElement
            WriteStartElement("polyline");
            WriteAttributeString("points", PointsToString(pts));
            WriteAttributeString("style", style);
            WriteEndElement();
        }

        private string PointsToString(IEnumerable<ScreenPoint> pts)
        {
            var sb = new StringBuilder();
            string fmt = "{0:" + NumberFormat + "},{1:" + NumberFormat + "} ";
            foreach (var p in pts)
                sb.AppendFormat(CultureInfo.InvariantCulture, fmt, p.X, p.Y);
            return sb.ToString().Trim();
        }

        public void WritePolygon(IEnumerable<ScreenPoint> pts, string style)
        {
            // http://www.w3.org/TR/SVG/shapes.html#PolygonElement
            WriteStartElement("polygon");
            WriteAttributeString("points", PointsToString(pts));
            WriteAttributeString("style", style);
            WriteEndElement();
        }

        public void WriteEllipse(double x, double y, double width, double height, string style)
        {
            // http://www.w3.org/TR/SVG/shapes.html#EllipseElement
            WriteStartElement("ellipse");
            WriteAttributeString("cx", x + width / 2);
            WriteAttributeString("cy", y + height / 2);
            WriteAttributeString("rx", width / 2);
            WriteAttributeString("ry", height / 2);
            WriteAttributeString("style", style);
            WriteEndElement();
        }

        public void WriteRectangle(double x, double y, double width, double height, string style)
        {
            // http://www.w3.org/TR/SVG/shapes.html#RectangleElement
            WriteStartElement("rect");
            WriteAttributeString("x", x);
            WriteAttributeString("y", y);
            WriteAttributeString("width", width);
            WriteAttributeString("height", height);
            WriteAttributeString("style", style);
            WriteEndElement();
        }

        public void WriteText(ScreenPoint pt, string text, OxyColor fill, string fontFamily = null, double fontSize = 10, double fontWeight = FontWeights.Normal, double rotate = 0, HorizontalTextAlign halign = HorizontalTextAlign.Left, VerticalTextAlign valign = VerticalTextAlign.Top)
        {
            // http://www.w3.org/TR/SVG/text.html
            WriteStartElement("text");
            //        WriteAttributeString("x", pt.X);
            //       WriteAttributeString("y", pt.Y);
            var baselineAlignment = "hanging";
            if (valign == VerticalTextAlign.Middle) baselineAlignment = "middle";
            if (valign == VerticalTextAlign.Bottom) baselineAlignment = "baseline";
            WriteAttributeString("dominant-baseline", baselineAlignment);

            var textAnchor = "start";
            if (halign == HorizontalTextAlign.Center) textAnchor = "middle";
            if (halign == HorizontalTextAlign.Right) textAnchor = "end";
            WriteAttributeString("text-anchor", textAnchor);

            string fmt = "translate({0:" + NumberFormat + "},{1:" + NumberFormat + "})";
            string transform = string.Format(CultureInfo.InvariantCulture, fmt, pt.X, pt.Y);
            if (rotate != 0)
                transform += string.Format(CultureInfo.InvariantCulture, " rotate({0})", rotate);
            WriteAttributeString("transform", transform);

            if (fontFamily != null)
                WriteAttributeString("font-family", fontFamily);
            if (fontSize > 0)
                WriteAttributeString("font-size", fontSize);
            if (fontWeight > 0)
                WriteAttributeString("font-weight", fontWeight);
            WriteAttributeString("fill", ColorToString(fill));
            // WriteAttributeString("style", style);
            WriteString(text);
            WriteEndElement();
        }

        private bool endIsWritten;

        public void Complete()
        {
            WriteEndElement();
            if (IsDocument)
                WriteEndDocument();
            endIsWritten = true;
        }

        public override void Close()
        {
            if (!endIsWritten)
                Complete();
            base.Close();
        }
    }
}