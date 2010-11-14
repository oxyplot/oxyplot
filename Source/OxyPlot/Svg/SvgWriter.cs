using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace OxyPlot
{
    public class SvgWriter : XmlTextWriter
    {
        public bool IsDocument { get; set; }

        public SvgWriter(Stream s, double width, double height, bool isDocument = true) :
            base(s, Encoding.UTF8)
        {
            IsDocument = isDocument;
            WriteHeader(width, height);
        }
        public SvgWriter(string path, double width, double height)
            : base(path, Encoding.UTF8)
        {
            IsDocument = true;
            WriteHeader(width, height);
        }

        private void WriteHeader(double width, double height)
        {
            this.Formatting = Formatting.Indented;

            // http://www.w3.org/TR/SVG/struct.html#SVGElement

            if (IsDocument)
            {
                WriteStartDocument(false);
                WriteDocType("svg", "-//W3C//DTD SVG 1.1//EN", "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd", null);
            }
            WriteStartElement("svg");
            WriteAttributeString("width", GetAutoValue(width, "100%"));
            WriteAttributeString("height", GetAutoValue(height, "100%"));
            WriteAttributeString("version", "1.1");
            WriteAttributeString("xmlns", "http://www.w3.org/2000/svg");
        }

        private string GetAutoValue(double value, string auto)
        {
            if (double.IsNaN(value))
                return auto;
            return value.ToString(CultureInfo.InvariantCulture);
        }

        protected static string ColorToString(Color color)
        {
            return String.Format("rgb({0},{1},{2})", color.R, color.G, color.B);
        }

        public static string CreateStyle(Color fill, Color stroke, double thickness, double[] dashArray)
        {
            // http://oreilly.com/catalog/svgess/chapter/ch03.html
            var style = new StringBuilder();
            if (fill == null)
                style.AppendFormat("fill:none;");
            else
            {
                style.AppendFormat("fill:{0};", ColorToString(fill));
                if (fill.A != 0xFF)
                    style.AppendFormat(CultureInfo.InvariantCulture, ";fill-opacity:{0}", fill.A / 255.0);

            }
            if (stroke == null)
            {
                style.AppendFormat("stroke:none;");

            }
            else
                style.AppendFormat("stroke:{0};stroke-width:{1}", ColorToString(stroke), thickness);

            if (stroke.A != 0xFF)
                style.AppendFormat(CultureInfo.InvariantCulture, ";stroke-opacity:{0}", stroke.A / 255.0);

            if (dashArray != null && dashArray.Length > 0)
            {
                style.Append(";stroke-dasharray:");
                for (int i = 0; i < dashArray.Length; i++)
                    style.AppendFormat(CultureInfo.InvariantCulture, "{0}{1}", i > 0 ? "," : "", dashArray[i]);
            }
            return style.ToString();
        }

        /*        public void WriteStrokeStyle(string name, Color color, double thickness, double[] dashArray)
                {
                    WriteStyle(name, CreateStrokeStyle(color, thickness, dashArray));
                }

                private void WriteStyle(string name, string createStrokeStyle)
                {
                }*/

        protected void WriteAttributeString(string name, double value)
        {
            WriteAttributeString(name, value.ToString(CultureInfo.InvariantCulture));
        }

        public void WriteLine(Point p1, Point p2, string style)
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

        public void WritePolyline(IEnumerable<Point> pts, string style)
        {
            // http://www.w3.org/TR/SVG/shapes.html#PolylineElement
            WriteStartElement("polyline");
            var sb = new StringBuilder();
            foreach (var p in pts)
                sb.AppendFormat(CultureInfo.InvariantCulture, "{0},{1} ", p.X, p.Y);

            WriteAttributeString("points", sb.ToString().Trim());
            WriteAttributeString("style", style);
            WriteEndElement();
        }

        public void WritePolygon(IEnumerable<Point> pts, string style)
        {
            // http://www.w3.org/TR/SVG/shapes.html#PolygonElement
            WriteStartElement("polygon");
            var sb = new StringBuilder();
            foreach (var p in pts)
                sb.AppendFormat(CultureInfo.InvariantCulture, "{0},{1} ", p.X, p.Y);

            WriteAttributeString("points", sb.ToString().Trim());
            WriteAttributeString("style", style);
            WriteEndElement();
        }

        public void WriteText(Point pt, string text, Color fill, string fontFamily = null, double fontSize = 10, double fontWeight = 500, double rotate = 0, HorizontalTextAlign halign = HorizontalTextAlign.Left, VerticalTextAlign valign = VerticalTextAlign.Top)
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
            string transform = string.Format(CultureInfo.InvariantCulture, "translate({0},{1})", pt.X, pt.Y);
            Debug.WriteLine(transform);
            if (rotate != 0)
                transform += string.Format(CultureInfo.InvariantCulture, " rotate({0})", rotate);
            WriteAttributeString("transform", transform);
            //            if (rotate != 0)
            //              WriteAttributeString("transform", "rotate("+rotate+")");
            if (fontFamily != null)
                WriteAttributeString("font-family", fontFamily);
            if (fontSize > 0)
                WriteAttributeString("font-size", fontSize);
            if (fontWeight > 0)
                WriteAttributeString("font-weight", fontWeight);
            WriteAttributeString("fill", ColorToString(fill));
            WriteString(text);
            // WriteAttributeString("style", style);
            WriteEndElement();
        }

        public override void Close()
        {
            if (IsDocument)
                WriteEndDocument();
            base.Close();
        }
    }
}