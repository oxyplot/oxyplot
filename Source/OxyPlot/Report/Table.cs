using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OxyReporter
{
    public class Table : ReportItem
    {
        public IList<TableColumn> Fields { get; set; }
        public string Caption { get; set; }
        public IEnumerable Items { get; set; }
        public bool Flipped { get; set; }

        public Table()
        {
            Fields = new List<TableColumn>();
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
            var hasHead = false;
            foreach (var c in Fields)
                if (c.Header != null) hasHead = true;

            var rows = Items.Cast<object>().ToList();
            int nrows = rows.Count;
            if (hasHead) nrows++;
            var result = new string[nrows, Fields.Count];


            int row = 0;
            if (hasHead)
            {
                for (int i = 0; i < Fields.Count; i++)
                {
                    var c = Fields[i];
                    result[row, i] = c.Header;
                }
                row++;
            }

            foreach (var item in rows)
            {
                for (int i = 0; i < Fields.Count; i++)
                {
                    var c = Fields[i];
                    var text = c.GetText(item);
                    result[row, i] = text;
                }
                row++;
            }
            if (this.Flipped)
                result = Flip(result);
            return result;
        }

        private string[,] Flip(string[,] input)
        {
            int rows = input.GetUpperBound(0)+1;
            int cols = input.GetUpperBound(1)+1;
            var result = new string[cols, rows];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    result[j, i] = input[i, j];
            return result;
        }
    }
}