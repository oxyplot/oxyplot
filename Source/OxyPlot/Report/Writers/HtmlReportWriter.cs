using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace OxyReporter
{
    public class HtmlReportWriter : XmlTextWriter, IReportWriter
    {
        public HtmlReportWriter(string filename, string title, string css, string style)
            : base(filename, Encoding.UTF8)
        {
            Formatting = Formatting.Indented;
            WriteStartElement("html");
            WriteAttributeString("xmlns", "http://www.w3.org/1999/xhtml");
            WriteHtmlHeader(title, css, style);
        }

        private void WriteHtmlHeader(string title, string css, string style)
        {
            WriteStartElement("head");

            if (title != null)
                WriteElementString("title", title);

            if (css != null)
            {
                WriteStartElement("link");
                WriteAttributeString("href", css);
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

        public override void Close()
        {
            WriteEndElement();
            WriteEndElement();
            base.Close();
        }

        public void WriteHeader(Header h)
        {
            if (h.Text == null)
                return;
            WriteStartElement("h" + h.Level);
            WriteClassID(h);
            WriteString(h.ToString());
            WriteEndElement();
        }

        public void WriteParagraph(Paragraph p)
        {
            WriteClassID(p);
            WriteElementString("p", p.Text);
        }

        int tableCounter = 0;

        public void WriteClassID(ReportItem ri)
        {
            if (ri.Class != null)
                WriteAttributeString("class", ri.Class);
            if (ri.ID != null)
                WriteAttributeString("id", ri.ID);
        }
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
            if (t.Flipped)
                WriteFlippedItems(t);
            else
                WriteItems(t);
            WriteEndElement(); // table
        }

        public void WriteItems(Table t)
        {
            IEnumerable items = t.Items;
            IList<TableColumn> columns = t.Fields;

            for (int i = 0; i < columns.Count; i++)
            {
                var c = columns[i];
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

                for (int i = 0; i < columns.Count; i++)
                {
                    var c = columns[i];
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
                for (int i = 0; i < columns.Count; i++)
                {
                    var c = columns[i];
                    var text = c.GetText(item);
                    WriteStartElement("td");
                    WriteString(text);
                    WriteEndElement();
                }
                WriteEndElement(); // tr
            }


        }

        public void WriteFlippedItems(Table t)
        {
            IEnumerable items = t.Items;
            IList<TableColumn> rows = t.Fields;

            var columns = items.Cast<object>().ToList();

            for (int i = 0; i < columns.Count; i++)
            {
                var c = columns[i];
                WriteStartElement("col");
                // WriteAttributeString("align", GetAlignmentString(c.Alignment));
                //WriteAttributeString("width", c.Width + "pt");
                // WriteAttributeString("style", c.Style);
                WriteEndElement();
            }

            var hasHead = false;
            foreach (var c in columns)
                if (c.ToString() != null) hasHead = true;

            if (hasHead)
            {
                WriteStartElement("thead");
                WriteStartElement("tr");

                WriteStartElement("td");
                WriteString("");
                WriteEndElement();

                for (int i = 0; i < columns.Count; i++)
                {
                    var c = columns[i];
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

                for (int i = 0; i < columns.Count; i++)
                {
                    var c = columns[i];
                    var text = row.GetText(c);
                    WriteStartElement("td");
                    WriteString(text);
                    WriteEndElement();
                }
                WriteEndElement(); // tr
            }
        }

        private string GetAlignmentString(Alignment a)
        {
            return a.ToString().ToLower();
        }

        int FigureCounter;

        public void WriteStartFigure()
        {
            FigureCounter++;
            WriteStartElement("p");
        }
        public void WriteEndFigure(string text)
        {
            WriteDiv("figuretext", String.Format("Fig {0}. {1}", FigureCounter, text));
            WriteEndElement();
        }

        public void WriteImage(Image i)
        {
            WriteStartFigure();
            WriteClassID(i);
            WriteStartElement("img");
            WriteAttributeString("src", i.Source);
            WriteAttributeString("alt", i.FigureText);
            WriteEndElement();
            WriteEndFigure(i.FigureText);
        }

        public void WriteDrawing(Drawing d)
        {
            WriteStartFigure();
            WriteClassID(d);
            WriteRaw(d.Content);
            WriteEndFigure(d.FigureText);
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