using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace OxyPlot.Reporting
{
    public enum Alignment { Left, Right, Center };

    public class TableColumn
    {
        public Alignment Alignment { get; set; }
        public string Header { get; set; }
        public string StringFormat { get; set; }
        public string Path { get; set; }
        public double Width { get; set; }
        // public Collection<TableColumn> SubColumns { get; set; }

        public TableColumn(string header, string path, string stringFormat=null, Alignment alignment=Alignment.Center)
        {
            Header = header;
            Path = path;
            StringFormat = stringFormat;
            Alignment = alignment;
            // SubColumns = new Collection<TableColumn>();
        }

        public string GetText(object item)
        {
            var pi = item.GetType().GetProperty(Path);
            object o = pi.GetValue(item, null);
            var of = o as IFormattable;
            if (of != null)
                return of.ToString(StringFormat, CultureInfo.InvariantCulture);
            return o!=null ? o.ToString():null;
        }
    }
}