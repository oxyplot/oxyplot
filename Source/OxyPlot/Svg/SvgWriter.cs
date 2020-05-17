// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SvgWriter.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a writer that provides easy generation of Scalable Vector Graphics files.
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
    /// Represents a writer that provides easy generation of Scalable Vector Graphics files.
    /// </summary>
    public class SvgWriter : XmlWriterBase
    {
        /// <summary>
        /// The end is written.
        /// </summary>
        private bool endIsWritten;

        /// <summary>
        /// The clip path number
        /// </summary>
        private int clipPathNumber = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgWriter" /> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="width">The width (in user units).</param>
        /// <param name="height">The height (in user units).</param>
        /// <param name="isDocument">if set to <c>true</c>, the writer will write the xml headers (?xml and !DOCTYPE).</param>
        public SvgWriter(Stream stream, double width, double height, bool isDocument = true)
            : base(stream)
        {
            this.IsDocument = isDocument;
            this.NumberFormat = "0.####";
            this.WriteHeader(width, height);
        }

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
        /// Closes the svg document.
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
        /// Writes the end of the document.
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
        /// Creates a style.
        /// </summary>
        /// <param name="fill">The fill color.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The stroke thickness (in user units).</param>
        /// <param name="dashArray">The line dash array.</param>
        /// <param name="lineJoin">The line join type.</param>
        /// <returns>A style string.</returns>
        public string CreateStyle(
            OxyColor fill,
            OxyColor stroke,
            double thickness,
            double[] dashArray = null,
            LineJoin lineJoin = LineJoin.Miter)
        {
            // http://oreilly.com/catalog/svgess/chapter/ch03.html
            var style = new StringBuilder();
            if (fill.IsInvisible())
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

            if (stroke.IsInvisible())
            {
                style.AppendFormat("stroke:none;");
            }
            else
            {
                string formatString = "stroke:{0};stroke-width:{1:" + this.NumberFormat + "}";
                style.AppendFormat(CultureInfo.InvariantCulture, formatString, this.ColorToString(stroke), thickness);
                switch (lineJoin)
                {
                    case LineJoin.Round:
                        style.AppendFormat(";stroke-linejoin:round");
                        break;
                    case LineJoin.Bevel:
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
        /// Writes an ellipse.
        /// </summary>
        /// <param name="x">The x-coordinate of the center.</param>
        /// <param name="y">The y-coordinate of the center.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="style">The style.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        public void WriteEllipse(double x, double y, double width, double height, string style, EdgeRenderingMode edgeRenderingMode)
        {
            // http://www.w3.org/TR/SVG/shapes.html#EllipseElement
            this.WriteStartElement("ellipse");
            this.WriteAttributeString("cx", x + (width / 2));
            this.WriteAttributeString("cy", y + (height / 2));
            this.WriteAttributeString("rx", width / 2);
            this.WriteAttributeString("ry", height / 2);
            this.WriteAttributeString("style", style);
            this.WriteEdgeRenderingModeAttribute(edgeRenderingMode);
            this.WriteEndElement();
        }

        /// <summary>
        /// Sets a clipping rectangle.
        /// </summary>
        /// <param name="x">The x coordinate of the clipping rectangle.</param>
        /// <param name="y">The y coordinate of the clipping rectangle.</param>
        /// <param name="width">The width of the clipping rectangle.</param>
        /// <param name="height">The height of the clipping rectangle.</param>
        public void BeginClip(double x, double y, double width, double height)
        {
            // http://www.w3.org/TR/SVG/masking.html
            // https://developer.mozilla.org/en-US/docs/Web/SVG/Element/clipPath
            // http://www.svgbasics.com/clipping.html
            var clipPath = "clipPath" + this.clipPathNumber++;

            this.WriteStartElement("defs");
            this.WriteStartElement("clipPath");
            this.WriteAttributeString("id", clipPath);
            this.WriteStartElement("rect");
            this.WriteAttributeString("x", x);
            this.WriteAttributeString("y", y);
            this.WriteAttributeString("width", width);
            this.WriteAttributeString("height", height);
            this.WriteEndElement(); // rect
            this.WriteEndElement(); // clipPath
            this.WriteEndElement(); // defs

            this.WriteStartElement("g");
            this.WriteAttributeString("clip-path", $"url(#{clipPath})");
        }

        /// <summary>
        /// Resets the clipping rectangle.
        /// </summary>
        public void EndClip()
        {
            this.WriteEndElement(); // g
        }

        /// <summary>
        /// Writes a portion of the specified image.
        /// </summary>
        /// <param name="srcX">The x-coordinate of the upper-left corner of the portion of the source image to draw.</param>
        /// <param name="srcY">The y-coordinate of the upper-left corner of the portion of the source image to draw.</param>
        /// <param name="srcWidth">Width of the portion of the source image to draw.</param>
        /// <param name="srcHeight">Height of the portion of the source image to draw.</param>
        /// <param name="destX">The destination x-coordinate.</param>
        /// <param name="destY">The destination y-coordinate.</param>
        /// <param name="destWidth">Width of the destination rectangle.</param>
        /// <param name="destHeight">Height of the destination rectangle.</param>
        /// <param name="image">The image.</param>
        public void WriteImage(
            double srcX,
            double srcY,
            double srcWidth,
            double srcHeight,
            double destX,
            double destY,
            double destWidth,
            double destHeight,
            OxyImage image)
        {
            double x = destX - (srcX / srcWidth * destWidth);
            double width = image.Width / srcWidth * destWidth;
            double y = destY - (srcY / srcHeight * destHeight);
            double height = image.Height / srcHeight * destHeight;
            this.BeginClip(destX, destY, destWidth, destHeight);
            this.WriteImage(x, y, width, height, image);
            this.EndClip();
        }

        /// <summary>
        /// Writes the specified image.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="image">The image.</param>
        public void WriteImage(double x, double y, double width, double height, OxyImage image)
        {
            // http://www.w3.org/TR/SVG/shapes.html#ImageElement
            this.WriteStartElement("image");
            this.WriteAttributeString("x", x);
            this.WriteAttributeString("y", y);
            this.WriteAttributeString("width", width);
            this.WriteAttributeString("height", height);
            this.WriteAttributeString("preserveAspectRatio", "none");
            var imageData = image.GetData();
            var encodedImage = new StringBuilder();
            encodedImage.Append("data:");
            encodedImage.Append("image/png");
            encodedImage.Append(";base64,");
            encodedImage.Append(Convert.ToBase64String(imageData));
            this.WriteAttributeString("xlink", "href", null, encodedImage.ToString());
            this.WriteEndElement();
        }

        /// <summary>
        /// Writes a line.
        /// </summary>
        /// <param name="p1">The first point.</param>
        /// <param name="p2">The second point.</param>
        /// <param name="style">The style.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        public void WriteLine(ScreenPoint p1, ScreenPoint p2, string style, EdgeRenderingMode edgeRenderingMode)
        {
            // http://www.w3.org/TR/SVG/shapes.html#LineElement
            // http://www.w3schools.com/svg/svg_line.asp
            this.WriteStartElement("line");
            this.WriteAttributeString("x1", p1.X);
            this.WriteAttributeString("y1", p1.Y);
            this.WriteAttributeString("x2", p2.X);
            this.WriteAttributeString("y2", p2.Y);
            this.WriteAttributeString("style", style);
            this.WriteEdgeRenderingModeAttribute(edgeRenderingMode);
            this.WriteEndElement();
        }

        /// <summary>
        /// Writes a polygon.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="style">The style.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        public void WritePolygon(IEnumerable<ScreenPoint> points, string style, EdgeRenderingMode edgeRenderingMode)
        {
            // http://www.w3.org/TR/SVG/shapes.html#PolygonElement
            this.WriteStartElement("polygon");
            this.WriteAttributeString("points", this.PointsToString(points));
            this.WriteAttributeString("style", style);
            this.WriteEdgeRenderingModeAttribute(edgeRenderingMode);
            this.WriteEndElement();
        }

        /// <summary>
        /// Writes a polyline.
        /// </summary>
        /// <param name="pts">The points.</param>
        /// <param name="style">The style.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        public void WritePolyline(IEnumerable<ScreenPoint> pts, string style, EdgeRenderingMode edgeRenderingMode)
        {
            // http://www.w3.org/TR/SVG/shapes.html#PolylineElement
            this.WriteStartElement("polyline");
            this.WriteAttributeString("points", this.PointsToString(pts));
            this.WriteAttributeString("style", style);
            this.WriteEdgeRenderingModeAttribute(edgeRenderingMode);
            this.WriteEndElement();
        }

        /// <summary>
        /// Writes a rectangle.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="style">The style.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        public void WriteRectangle(double x, double y, double width, double height, string style, EdgeRenderingMode edgeRenderingMode)
        {
            // http://www.w3.org/TR/SVG/shapes.html#RectangleElement
            this.WriteStartElement("rect");
            this.WriteAttributeString("x", x);
            this.WriteAttributeString("y", y);
            this.WriteAttributeString("width", width);
            this.WriteAttributeString("height", height);
            this.WriteAttributeString("style", style);
            this.WriteEdgeRenderingModeAttribute(edgeRenderingMode);
            this.WriteEndElement();
        }

        /// <summary>
        /// Writes text.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="text">The text.</param>
        /// <param name="fill">The text color.</param>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">The font size (in user units).</param>
        /// <param name="fontWeight">The font weight.</param>
        /// <param name="rotate">The rotation angle.</param>
        /// <param name="halign">The horizontal alignment.</param>
        /// <param name="valign">The vertical alignment.</param>
        public void WriteText(
            ScreenPoint position,
            string text,
            OxyColor fill,
            string fontFamily = null,
            double fontSize = 10,
            double fontWeight = FontWeights.Normal,
            double rotate = 0,
            HorizontalAlignment halign = HorizontalAlignment.Left,
            VerticalAlignment valign = VerticalAlignment.Top)
        {
            // http://www.w3.org/TR/SVG/text.html
            this.WriteStartElement("text");

            // WriteAttributeString("x", position.X);
            // WriteAttributeString("y", position.Y);
            string baselineAlignment = "hanging";
            if (valign == VerticalAlignment.Middle)
            {
                baselineAlignment = "middle";
            }

            if (valign == VerticalAlignment.Bottom)
            {
                baselineAlignment = "baseline";
            }

            this.WriteAttributeString("dominant-baseline", baselineAlignment);

            string textAnchor = "start";
            if (halign == HorizontalAlignment.Center)
            {
                textAnchor = "middle";
            }

            if (halign == HorizontalAlignment.Right)
            {
                textAnchor = "end";
            }

            this.WriteAttributeString("text-anchor", textAnchor);

            string fmt = "translate({0:" + this.NumberFormat + "},{1:" + this.NumberFormat + "})";
            string transform = string.Format(CultureInfo.InvariantCulture, fmt, position.X, position.Y);
            if (Math.Abs(rotate) > 0)
            {
                transform += string.Format(CultureInfo.InvariantCulture, " rotate({0})", rotate);
            }

            this.WriteAttributeString("transform", transform);

            if (fontFamily != null)
            {
                this.WriteAttributeString("font-family", fontFamily);
            }

            if (fontSize > 0)
            {
                this.WriteAttributeString("font-size", fontSize);
            }

            if (fontWeight > 0)
            {
                this.WriteAttributeString("font-weight", fontWeight);
            }

            if (fill.IsInvisible())
            {
                this.WriteAttributeString("fill", "none");
            }
            else
            {
                this.WriteAttributeString("fill", this.ColorToString(fill));
                if (fill.A != 0xFF)
                {
                    this.WriteAttributeString("fill-opacity", fill.A / 255.0);
                }
            }

            // WriteAttributeString("style", style);
            this.WriteString(text);
            this.WriteEndElement();
        }

        /// <summary>
        /// Converts a color to a svg color string.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The color string.</returns>
        protected string ColorToString(OxyColor color)
        {
            if (color.Equals(OxyColors.Black))
            {
                return "black";
            }

            var formatString = "rgb({0:" + this.NumberFormat + "},{1:" + this.NumberFormat + "},{2:" + this.NumberFormat + "})";
            return string.Format(formatString, color.R, color.G, color.B);
        }

        /// <summary>
        /// Writes an double attribute.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        protected void WriteAttributeString(string name, double value)
        {
            this.WriteAttributeString(name, value.ToString(this.NumberFormat, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Writes the edge rendering mode attribute if necessary.
        /// </summary>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        private void WriteEdgeRenderingModeAttribute(EdgeRenderingMode edgeRenderingMode)
        {
            string value;
            switch (edgeRenderingMode)
            {
                case EdgeRenderingMode.PreferSharpness:
                    value = "crispEdges";
                    break;
                case EdgeRenderingMode.PreferSpeed:
                    value = "optimizeSpeed";
                    break;
                case EdgeRenderingMode.PreferGeometricAccuracy:
                    value = "geometricPrecision";
                    break;
                default:
                    return;
            }

            this.WriteAttributeString("shape-rendering", value);
        }

        /// <summary>
        /// Converts a value to a string or to the specified "auto" string if the value is NaN.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="auto">The string to return if value is NaN.</param>
        /// <returns>A string.</returns>
        private string GetAutoValue(double value, string auto)
        {
            if (double.IsNaN(value))
            {
                return auto;
            }

            return value.ToString(this.NumberFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a list of points to a string.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <returns>A string.</returns>
        private string PointsToString(IEnumerable<ScreenPoint> points)
        {
            var sb = new StringBuilder();
            string fmt = "{0:" + this.NumberFormat + "},{1:" + this.NumberFormat + "} ";
            foreach (var p in points)
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, fmt, p.X, p.Y);
            }

            return sb.ToString().Trim();
        }

        /// <summary>
        /// Writes the header.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
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
            this.WriteAttributeString("xmlns", "xlink", null, "http://www.w3.org/1999/xlink");
        }
    }
}
