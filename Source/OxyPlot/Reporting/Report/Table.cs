using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OxyPlot.Reporting
{
    public class Table : ReportItem
    {
        private const string CAPTION_FORMAT_STRING = "Table {0}. {1}";

        public IList<TableColumn> Columns { get; set; }
        public string Caption { get; set; }
        public IEnumerable Items { get; set; }
        public bool Transposed { get; set; }

        public int TableNumber { get; set;}

        public string FullCaption
        {
            get { return String.Format(CAPTION_FORMAT_STRING, TableNumber, Caption); }
        }

        public Table()
        {
            Columns = new List<TableColumn>();
            Class = "table";
        }

        public override void Update()
        {
        }

        public override void WriteContent(IReportWriter w)
        {
            w.WriteTable(this);
        }

        public string[,] ToArray()
        {
            var hasHeader = false;
            foreach (var c in Columns)
                if (c.Header != null) hasHeader = true;

            var rows = Items.Cast<object>().ToList();
            int nrows = rows.Count;
            if (hasHeader) nrows++;
            var result = new string[nrows,Columns.Count];


            int row = 0;
            if (hasHeader)
            {
                for (int i = 0; i < Columns.Count; i++)
                {
                    var c = Columns[i];
                    result[row, i] = c.Header;
                }
                row++;
            }

            foreach (var item in rows)
            {
                for (int i = 0; i < Columns.Count; i++)
                {
                    var c = Columns[i];
                    var text = c.GetText(item);
                    result[row, i] = text;
                }
                row++;
            }
            if (Transposed)
                result = Transpose(result);
            return result;
        }

        private static string[,] Transpose(string[,] input)
        {
            int rows = input.GetUpperBound(0) + 1;
            int cols = input.GetUpperBound(1) + 1;
            var result = new string[cols,rows];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    result[j, i] = input[i, j];
            return result;
        }
    }
}