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
        /// <param name="title">The title.</param>
        /// <param name="cssPath">The path to a CSS file.</param>
        /// <param name="style">The inline CSS style.</param>
        public HtmlReportWriter(string path, string title, string cssPath, string style)
            : base(path)
        {
            WriteStartElement("html", "http://www.w3.org/1999/xhtml");
            WriteHtmlHeader(title, cssPath, style);
        }

        public HtmlReportWriter(string path, string title, ReportStyle style)
            : this(path, title, null, CreateCss(style))
        {
        }

        static string CreateCss(ReportStyle style)
        {
            var css = new StringBuilder();
            css.AppendLine("body { " + ParagraphStyleToCss(style.BodyTextStyle) + " }");
            for (int i = 0; i < style.HeaderStyles.Length; i++)
                css.AppendLine("h" + (i + 1) + " {" + ParagraphStyleToCss(style.HeaderStyles[i]) + " }");

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
            css.Append(String.Format("font-size:{0};", s.FontSize));
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
            WriteClassID(h);
            WriteString(h.ToString());
            WriteEndElement();
        }

        /// <summary>
        /// Writes the paragraph.
        /// </summary>
        /// <param name="p">The p.</param>
        public void WriteParagraph(Paragraph p)
        {
            WriteClassID(p);
            WriteElementString("p", p.Text);
        }

        /// <summary>
        /// Writes the class ID.
        /// </summary>
        /// <param name="ri">The ri.</param>
        public void WriteClassID(ReportItem ri)
        {
            if (ri.Class != null)
                WriteAttributeString("class", ri.Class);
            if (ri.ID != null)
                WriteAttributeString("id", ri.ID);
        }

        /// <summary>
        /// Writes the table.
        /// </summary>
        /// <param name="t">The t.</param>
        public void WriteTable(Table t)
        {
            if (t.Items == null)
                return;
            WriteStartElement("table");
            // WriteAttributeString("border", "1");
            // WriteAttributeString("width", "60%");
            WriteClassID(t);

            if (t.Caption != null)
            {
                WriteStartElement("caption");
                WriteString(t.FullCaption);
                WriteEndElement();
            }
            if (t.ItemsInColumns)
                WriteFlippedItems(t);
            else
                WriteItems(t);
            WriteEndElement(); // table
        }

        /// <summary>
        /// Writes the items.
        /// </summary>
        /// <param name="t">The t.</param>
        public void WriteItems(Table t)
        {
            var items = t.Items;
            var columns = t.Columns;

            foreach (var c in columns)
            {
                WriteStartElement("col");
                WriteAttributeString("align", GetAlignmentString(c.Alignment));
                WriteAttributeString("width", c.Width + "pt");
                // WriteAttributeString("style", c.Style);
                WriteEndElement();
            }

            if (t.HasHeader())
            {
                WriteStartElement("thead");
                WriteStartElement("tr");

                foreach (var c in columns)
                {
                    WriteStartElement("td");
                    WriteString(c.Header);
                    WriteEndElement();
                }
                WriteEndElement(); // tr
                WriteEndElement(); // thead
            }

            foreach (var item in items)
            {
                WriteStartElement("tr");
                foreach (var c in columns)
                {
                    var text = c.GetText(item);
                    WriteStartElement("td");
                    WriteString(text);
                    WriteEndElement();
                }
                WriteEndElement(); // tr
            }
        }

        private void WriteFlippedItems(Table t)
        {
            var items = t.Items;
            var rows = t.Columns;

            var columns = items.Cast<object>().ToList();

            foreach (var c in columns)
            {
                WriteStartElement("col");
                // WriteAttributeString("align", GetAlignmentString(c.Alignment));
                //WriteAttributeString("width", c.Width + "pt");
                // WriteAttributeString("style", c.Style);
                WriteEndElement();
            }

            var hasHead = false;
            foreach (var c in columns)
                if (!String.IsNullOrEmpty(c.ToString())) hasHead = true;

            if (hasHead)
            {
                WriteStartElement("thead");
                WriteStartElement("tr");

                WriteStartElement("td");
                WriteString("");
                WriteEndElement();

                foreach (var c in columns)
                {
                    WriteStartElement("td");
                    WriteString(c.ToString());
                    WriteEndElement();
                }
                WriteEndElement(); // tr
                WriteEndElement(); // thead
            }

            foreach (var row in rows)
            {
                WriteStartElement("tr");

                WriteStartElement("td");
                WriteString(row.Header);
                WriteEndElement();

                foreach (var c in columns)
                {
                    var text = row.GetText(c);
                    WriteStartElement("td");
                    WriteString(text);
                    WriteEndElement();
                }
                WriteEndElement(); // tr
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
            WriteClassID(f);
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
        public void WriteDrawing(Drawing d)
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
    }
}