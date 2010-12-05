using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OxyPlot.Reporting
{
    public class ReportItem
    {
        public string Class { get; set; }
        public string ID { get; set; }

        public Collection<ReportItem> Children { get; private set; }

        public ReportItem()
        {
            Children = new Collection<ReportItem>();
        }

        public void Add(ReportItem child)
        {
            Children.Add(child);
        }

        public virtual void WriteContent(IReportWriter w)
        {
        }

        public virtual void Write(IReportWriter w)
        {
            Update();
            WriteContent(w);
            foreach (var child in Children)
                child.Write(w);
        }

        public virtual void Update()
        {
        }

        public void AddHeader(int level, string header)
        {
            Add(new Header { Level = level, Text = header });
        }

        public void AddTableOfContents(ReportItem b)
        {
            Add(new TableOfContents(b));
        }

        public void AddParagraph(string content)
        {
            Add(new Paragraph { Text = content });
        }

        public void AddEquation(string equation, string caption = null)
        {
            Add(new Equation { Content = equation, Caption = caption });
        }

        public void AddTable(string title, IEnumerable items, IList<TableColumn> fields)
        {
            Add(new Table { Caption = title, Items = items, Columns = fields });
        }

        public void AddPropertyTable(string title, IEnumerable items)
        {
            Add(new PropertyTable { Caption = title, Items = items });
        }

        public void AddImage(string src, string text)
        {
            Add(new Image { Source = src, FigureText = text });
        }

        public void AddDrawing(string content, string text)
        {
            Add(new Drawing { Content = content, FigureText = text });
        }
    }
}