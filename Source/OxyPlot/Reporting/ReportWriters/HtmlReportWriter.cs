using System;
using System.IO;
using System.Linq;
using System.Text;

namespace OxyPlot.Reporting
{
    /// <summary>
    /// HTML5 report writer.
    /// </summary>
    public class HtmlReportWriter : XmlWriterBase, IReportWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlReportWriter"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public HtmlReportWriter(string path)
            : base(path)
        {
            WriteStartElement("html", "http://www.w3.org/1999/xhtml");
        }

        static string CreateCss(ReportStyle style)
        {
            var css = new StringBuilder();
            css.AppendLine("body { " + ParagraphStyleToCss(style.BodyTextStyle) + " }");
            for (int i = 0; i < style.HeaderStyles.Length; i++)
                css.AppendLine("h" + (i + 1) + " {" + ParagraphStyleToCss(style.HeaderStyles[i]) + " }");

            css.AppendLine("table caption { " + ParagraphStyleToCss(style.TableCaptionStyle) + " }");
            css.AppendLine("thead { " + ParagraphStyleToCss(style.TableHeaderStyle) + " }");
            css.AppendLine("td { " + ParagraphStyleToCss(style.TableTextStyle) + " }");
            css.AppendLine("td.header { " + ParagraphStyleToCss(style.TableHeaderStyle) + " }");
            css.AppendLine("figuretext { " + ParagraphStyleToCss(style.FigureTextStyle) + " }");

            css.Append(
                @"body { margin:20pt; }
            table { border: solid 1px black; margin: 8pt; border-collapse:collapse; }
            td { padding: 0 2pt 0 2pt; border-left: solid 1px black; border-right: solid 1px black;}
            thead { border:solid 1px black; }
            .content, .content td { border: none; }
            .figure { margin: 8pt;}
            .table { margin: 8pt;}
            .table caption { margin: 4pt;}
            .table thead td { padding: 2pt;}");
            return css.ToString();
        }

        private static string ParagraphStyleToCss(ParagraphStyle s)
        {
            var css = new StringBuilder();
            if (s.FontFamily != null)
                css.Append(String.Format("font-family:{0};", s.FontFamily));
            css.Append(String.Format("font-size:{0}pt;", s.FontSize));
            if (s.Bold)
                css.Append(String.Format("font-weight:bold;"));
            return css.ToString();
        }

        private void WriteHtmlHeader(string title, string cssPath, string style)
        {
            WriteStartElement("head");

            if (title != null)
                WriteElementString("title", title);

            if (cssPath != null)
            {
                WriteStartElement("link");
                WriteAttributeString("href", cssPath);
                WriteAttributeString("rel", "stylesheet");
                WriteAttributeString("type", "text/css");
                WriteEndElement(); // link
            }
            if (style != null)
            {
                WriteStartElement("style");
                WriteAttributeString("type", "text/css");
                WriteRaw(style);
                WriteEndElement();
            }

            WriteEndElement(); // head
            WriteStartElement("body");
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public override void Close()
        {
            WriteEndElement();
            WriteEndElement();
            base.Close();
        }

        /// <summary>
        /// Writes the header.
        /// </summary>
        /// <param name="h">The h.</param>
        public void WriteHeader(Header h)
        {
            if (h.Text == null)
                return;
            WriteStartElement("h" + h.Level);
            WriteString(h.ToString());
            WriteEndElement();
        }

        /// <summary>
        /// Writes the paragraph.
        /// </summary>
        /// <param name="p">The p.</param>
        public void WriteParagraph(Paragraph p)
        {
            WriteElementString("p", p.Text);
        }

        /// <summary>
        /// Writes the class ID.
        /// </summary>
        /// <param name="ri">The ri.</param>
        public void WriteClassID(string _class, string id = null)
        {
            if (_class != null)
                WriteAttributeString("class", _class);
            if (id != null)
                WriteAttributeString("id", id);
        }

        /// <summary>
        /// Writes the table.
        /// </summary>
        /// <param name="t">The t.</param>
        public void WriteTable(Table t)
        {
            if (t.Rows == null || t.Columns == null)
                return;
            WriteStartElement("table");
            // WriteAttributeString("border", "1");
            // WriteAttributeString("width", "60%");

            if (t.Caption != null)
            {
                WriteStartElement("caption");
                WriteString(t.GetFullCaption(style));
                WriteEndElement();
            }
            WriteRows(t);

            WriteEndElement(); // table
        }

        /// <summary>
        /// Writes the items.
        /// </summary>
        /// <param name="t">The t.</param>
        public void WriteRows(Table t)
        {
            var columns = t.Columns;

            foreach (var c in columns)
            {
                WriteStartElement("col");
                WriteAttributeString("align", GetAlignmentString(c.Alignment));
                if (double.IsNaN(c.Width))
                    WriteAttributeString("width", c.Width + "pt");
                WriteEndElement();
            }

            foreach (var row in t.Rows)
            {
                if (row.IsHeader)
                    WriteStartElement("thead");
                WriteStartElement("tr");
                int j = 0;
                foreach (var c in row.Cells)
                {
                    bool isHeader = row.IsHeader || t.Columns[j++].IsHeader;

                    WriteStartElement("td");
                    if (isHeader)
                        WriteAttributeString("class", "header");
                    WriteString(c.Content);
                    WriteEndElement();
                }

                WriteEndElement(); // tr
                if (row.IsHeader)
                    WriteEndElement(); // thead
            }
        }

        private static string GetAlignmentString(Alignment a)
        {
            return a.ToString().ToLower();
        }

        private int figureCounter;

        private void WriteStartFigure(Figure f)
        {
            figureCounter++;
            WriteStartElement("p");
            WriteClassID("figure");
        }

        private void WriteEndFigure(string text)
        {
            WriteDiv("figuretext", String.Format("Fig {0}. {1}", figureCounter, text));
            WriteEndElement();
        }

        /// <summary>
        /// Writes the image.
        /// </summary>
        /// <param name="i">The i.</param>
        public void WriteImage(Image i)
        {
            // this requires the image to be located in the same folder as the html
            var localFileName = Path.GetFileName(i.Source);
            WriteStartFigure(i);
            WriteStartElement("img");
            WriteAttributeString("src", localFileName);
            WriteAttributeString("alt", i.FigureText);
            WriteEndElement();
            WriteEndFigure(i.FigureText);
        }

        /// <summary>
        /// Writes the drawing.
        /// </summary>
        /// <param name="d">The d.</param>
        public void WriteDrawing(DrawingFigure d)
        {
            WriteStartFigure(d);
            WriteRaw(d.Content);
            WriteEndFigure(d.FigureText);
        }

        /// <summary>
        /// Writes the plot.
        /// </summary>
        /// <param name="plot">The plot.</param>
        public void WritePlot(PlotFigure plot)
        {
            WriteStartFigure(plot);
            WriteRaw(plot.PlotModel.ToSvg(plot.Width, plot.Height));
            WriteEndFigure(plot.FigureText);
        }

        /// <summary>
        /// Writes the equation.
        /// </summary>
        /// <param name="equation">The equation.</param>
        public void WriteEquation(Equation equation)
        {
            // todo: MathML?
        }

        private void WriteDiv(string style, string content)
        {
            WriteStartElement("div");
            WriteAttributeString("class", style);
            WriteString(content);
            WriteEndElement();
        }

        private ReportStyle style;
        public void WriteReport(Report report, ReportStyle style)
        {
            this.style = style;
            WriteHtmlHeader(report.Title, null, CreateCss(style));
            report.Write(this);
        }
    }
}