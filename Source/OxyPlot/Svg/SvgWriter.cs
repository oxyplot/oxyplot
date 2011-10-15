//-----------------------------------------------------------------------
// <copyright file="SvgWriter.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

namespace OxyPlot
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Scalable Vector Graphics writer.
    /// </summary>
    public class SvgWriter : XmlWriterBase
    {
        #region Constants and Fields

        /// <summary>
        ///   The end is written.
        /// </summary>
        private bool endIsWritten;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgWriter"/> class.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        /// <param name="isDocument">
        /// if set to <c>true</c> [is document].
        /// </param>
        public SvgWriter(Stream stream, double width, double height, bool isDocument = true)
            : base(stream)
        {
            this.IsDocument = isDocument;
            this.NumberFormat = "0.####";
            this.WriteHeader(width, height);
        }

#if !METRO

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgWriter"/> class.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        public SvgWriter(string path, double width, double height)
            : base(path)
        {
            this.IsDocument = true;
            this.NumberFormat = "0.####";
            this.WriteHeader(width, height);
        }

#endif

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets a value indicating whether this writer should produce a stand-alone document.
        /// </summary>
        public bool IsDocument { get; set; }

        /// <summary>
        ///   Gets or sets the number format.
        /// </summary>
        /// <value>The number format.</value>
        public string NumberFormat { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The close.
        /// </summary>
        public override void Close()
        {
            if (!this.endIsWritten)
            {
                this.Complete();
            }

            base.Close();
        }

        /// <summary>
        /// The complete.
        /// </summary>
        public void Complete()
        {
            this.WriteEndElement();
            if (this.IsDocument)
            {
                this.WriteEndDocument();
            }

            this.endIsWritten = true;
        }

        /// <summary>
        /// The create style.
        /// </summary>
        /// <param name="fill">
        /// The fill.
        /// </param>
        /// <param name="stroke">
        /// The stroke.
        /// </param>
        /// <param name="thickness">
        /// The thickness.
        /// </param>
        /// <param name="dashArray">
        /// The dash array.
        /// </param>
        /// <param name="lineJoin">
        /// The line join.
        /// </param>
        /// <returns>
        /// The create style.
        /// </returns>
        public string CreateStyle(
            OxyColor fill, 
            OxyColor stroke, 
            double thickness, 
            double[] dashArray, 
            OxyPenLineJoin lineJoin = OxyPenLineJoin.Miter)
        {
            // http://oreilly.com/catalog/svgess/chapter/ch03.html
            var style = new StringBuilder();
            if (fill == null)
            {
                style.AppendFormat("fill:none;");
            }
            else
            {
                style.AppendFormat("fill:{0};", this.ColorToString(fill));
                if (fill.A != 0xFF)
                {
                    style.AppendFormat(CultureInfo.InvariantCulture, "fill-opacity:{0};", fill.A / 255.0);
                }
            }

            if (stroke == null)
            {
                style.AppendFormat("stroke:none;");
            }
            else
            {
                style.AppendFormat(
                    "stroke:{0};stroke-width:{1:" + this.NumberFormat + "}", this.ColorToString(stroke), thickness);
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
                {
                    style.AppendFormat(CultureInfo.InvariantCulture, ";stroke-opacity:{0}", stroke.A / 255.0);
                }

                if (dashArray != null && dashArray.Length > 0)
                {
                    style.Append(";stroke-dasharray:");
                    for (int i = 0; i < dashArray.Length; i++)
                    {
                        style.AppendFormat(
                            CultureInfo.InvariantCulture, "{0}{1}", i > 0 ? "," : string.Empty, dashArray[i]);
                    }
                }
            }

            return style.ToString();
        }

        /// <summary>
        /// The write ellipse.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="y">
        /// The y.
        /// </param>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        /// <param name="style">
        /// The style.
        /// </param>
        public void WriteEllipse(double x, double y, double width, double height, string style)
        {
            // http://www.w3.org/TR/SVG/shapes.html#EllipseElement
            this.WriteStartElement("ellipse");
            this.WriteAttributeString("cx", x + width / 2);
            this.WriteAttributeString("cy", y + height / 2);
            this.WriteAttributeString("rx", width / 2);
            this.WriteAttributeString("ry", height / 2);
            WriteAttributeString("style", style);
            this.WriteEndElement();
        }

        /// <summary>
        /// The write line.
        /// </summary>
        /// <param name="p1">
        /// The p 1.
        /// </param>
        /// <param name="p2">
        /// The p 2.
        /// </param>
        /// <param name="style">
        /// The style.
        /// </param>
        public void WriteLine(ScreenPoint p1, ScreenPoint p2, string style)
        {
            // http://www.w3.org/TR/SVG/shapes.html#LineElement
            // http://www.w3schools.com/svg/svg_line.asp
            this.WriteStartElement("line");
            this.WriteAttributeString("x1", p1.X);
            this.WriteAttributeString("y1", p1.Y);
            this.WriteAttributeString("x2", p2.X);
            this.WriteAttributeString("y2", p2.Y);
            WriteAttributeString("style", style);
            this.WriteEndElement();
        }

        /// <summary>
        /// The write polygon.
        /// </summary>
        /// <param name="pts">
        /// The pts.
        /// </param>
        /// <param name="style">
        /// The style.
        /// </param>
        public void WritePolygon(IEnumerable<ScreenPoint> pts, string style)
        {
            // http://www.w3.org/TR/SVG/shapes.html#PolygonElement
            this.WriteStartElement("polygon");
            this.WriteAttributeString("points", this.PointsToString(pts));
            WriteAttributeString("style", style);
            this.WriteEndElement();
        }

        /// <summary>
        /// The write polyline.
        /// </summary>
        /// <param name="pts">
        /// The pts.
        /// </param>
        /// <param name="style">
        /// The style.
        /// </param>
        public void WritePolyline(IEnumerable<ScreenPoint> pts, string style)
        {
            // http://www.w3.org/TR/SVG/shapes.html#PolylineElement
            this.WriteStartElement("polyline");
            this.WriteAttributeString("points", this.PointsToString(pts));
            WriteAttributeString("style", style);
            this.WriteEndElement();
        }

        /// <summary>
        /// The write rectangle.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="y">
        /// The y.
        /// </param>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        /// <param name="style">
        /// The style.
        /// </param>
        public void WriteRectangle(double x, double y, double width, double height, string style)
        {
            // http://www.w3.org/TR/SVG/shapes.html#RectangleElement
            this.WriteStartElement("rect");
            this.WriteAttributeString("x", x);
            this.WriteAttributeString("y", y);
            this.WriteAttributeString("width", width);
            this.WriteAttributeString("height", height);
            WriteAttributeString("style", style);
            this.WriteEndElement();
        }

        /// <summary>
        /// The write text.
        /// </summary>
        /// <param name="pt">
        /// The pt.
        /// </param>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <param name="fill">
        /// The fill.
        /// </param>
        /// <param name="fontFamily">
        /// The font family.
        /// </param>
        /// <param name="fontSize">
        /// The font size.
        /// </param>
        /// <param name="fontWeight">
        /// The font weight.
        /// </param>
        /// <param name="rotate">
        /// The rotate.
        /// </param>
        /// <param name="halign">
        /// The halign.
        /// </param>
        /// <param name="valign">
        /// The valign.
        /// </param>
        public void WriteText(
            ScreenPoint pt, 
            string text, 
            OxyColor fill, 
            string fontFamily = null, 
            double fontSize = 10, 
            double fontWeight = FontWeights.Normal, 
            double rotate = 0, 
            HorizontalTextAlign halign = HorizontalTextAlign.Left, 
            VerticalTextAlign valign = VerticalTextAlign.Top)
        {
            // http://www.w3.org/TR/SVG/text.html
            this.WriteStartElement("text");

            // WriteAttributeString("x", pt.X);
            // WriteAttributeString("y", pt.Y);
            string baselineAlignment = "hanging";
            if (valign == VerticalTextAlign.Middle)
            {
                baselineAlignment = "middle";
            }

            if (valign == VerticalTextAlign.Bottom)
            {
                baselineAlignment = "baseline";
            }

            WriteAttributeString("dominant-baseline", baselineAlignment);

            string textAnchor = "start";
            if (halign == HorizontalTextAlign.Center)
            {
                textAnchor = "middle";
            }

            if (halign == HorizontalTextAlign.Right)
            {
                textAnchor = "end";
            }

            WriteAttributeString("text-anchor", textAnchor);

            string fmt = "translate({0:" + this.NumberFormat + "},{1:" + this.NumberFormat + "})";
            string transform = string.Format(CultureInfo.InvariantCulture, fmt, pt.X, pt.Y);
            if (rotate != 0)
            {
                transform += string.Format(CultureInfo.InvariantCulture, " rotate({0})", rotate);
            }

            WriteAttributeString("transform", transform);

            if (fontFamily != null)
            {
                WriteAttributeString("font-family", fontFamily);
            }

            if (fontSize > 0)
            {
                this.WriteAttributeString("font-size", fontSize);
            }

            if (fontWeight > 0)
            {
                this.WriteAttributeString("font-weight", fontWeight);
            }

            this.WriteAttributeString("fill", this.ColorToString(fill));

            // WriteAttributeString("style", style);
            this.WriteString(text);
            this.WriteEndElement();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The color to string.
        /// </summary>
        /// <param name="color">
        /// The color.
        /// </param>
        /// <returns>
        /// The color to string.
        /// </returns>
        protected string ColorToString(OxyColor color)
        {
            if (color == OxyColors.Black)
            {
                return "black";
            }

            return
                string.Format(
                    "rgb({0:" + this.NumberFormat + "},{1:" + this.NumberFormat + "},{2:" + this.NumberFormat + "})", 
                    color.R, 
                    color.G, 
                    color.B);
        }

        /// <summary>
        /// The write attribute string.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        protected void WriteAttributeString(string name, double value)
        {
            WriteAttributeString(name, value.ToString(this.NumberFormat, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// The get auto value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="auto">
        /// The auto.
        /// </param>
        /// <returns>
        /// The get auto value.
        /// </returns>
        private string GetAutoValue(double value, string auto)
        {
            if (double.IsNaN(value))
            {
                return auto;
            }

            return value.ToString(this.NumberFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// The points to string.
        /// </summary>
        /// <param name="pts">
        /// The pts.
        /// </param>
        /// <returns>
        /// The points to string.
        /// </returns>
        private string PointsToString(IEnumerable<ScreenPoint> pts)
        {
            var sb = new StringBuilder();
            string fmt = "{0:" + this.NumberFormat + "},{1:" + this.NumberFormat + "} ";
            foreach (var p in pts)
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, fmt, p.X, p.Y);
            }

            return sb.ToString().Trim();
        }

        /// <summary>
        /// The write header.
        /// </summary>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        private void WriteHeader(double width, double height)
        {
            // http://www.w3.org/TR/SVG/struct.html#SVGElement
            if (this.IsDocument)
            {
                this.WriteStartDocument(false);
                this.WriteDocType(
                    "svg", "-//W3C//DTD SVG 1.1//EN", "http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd", null);
            }

            this.WriteStartElement("svg", "http://www.w3.org/2000/svg");
            this.WriteAttributeString("width", this.GetAutoValue(width, "100%"));
            this.WriteAttributeString("height", this.GetAutoValue(height, "100%"));
            this.WriteAttributeString("version", "1.1");
        }

        #endregion
    }
}
