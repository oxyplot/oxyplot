using System;
using System.Linq;

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

        private int tableCounter;

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
            tableCounter++;
            WriteStartElement("table");
            // WriteAttributeString("border", "1");
            // WriteAttributeString("width", "60%");
            WriteClassID(t);

            if (t.Caption != null)
            {
                WriteStartElement("caption");
                WriteString(String.Format("Table {0}. {1}", tableCounter, t.Caption));
                WriteEndElement();
            }
            if (t.Transposed)
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

            var hasHead = false;
            foreach (var c in columns)
                if (c.Header != null) hasHead = true;

            if (hasHead)
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
            WriteStartFigure(i);
            WriteStartElement("img");
            WriteAttributeString("src", i.Source);
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
            // todo: convert to MathML?
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