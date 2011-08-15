using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace OxyPlot.Reporting
{
    public class ItemsTable : Table
    {
        public IList<ItemsTableField> Fields { get; set; }

        private IEnumerable items;

        /// <summary>
        /// Gets or sets the items.
        /// The table will be filled when this property is set. 
        /// </summary>
        /// <value>The items.</value>
        public IEnumerable Items
        {
            get { return items; }
            set { items = value; UpdateItems(); }
        }

        public bool ItemsInRows { get; private set; }

        public Alignment Alignment { get; set; }

        public ItemsTable(bool itemsInRows = true)
        {
            Fields = new List<ItemsTableField>();
            this.ItemsInRows = itemsInRows;
            this.Alignment = Alignment.Center;
        }

        public bool HasHeader()
        {
            foreach (var c in Fields)
                if (c.Header != null) return true;
            return false;
        }

        public void UpdateItems()
        {
            Rows.Clear();
            Columns.Clear();
            if (Fields == null || Fields.Count == 0)
            {
                Debug.WriteLine("ItemsTable: Fields are not defined.");
                return;
            }

            var cells = ToArray();

            int rows = cells.GetUpperBound(0) + 1;
            int columns = cells.GetUpperBound(1) + 1;
            for (int i = 0; i < rows; i++)
            {
                var tr = new TableRow();
                if (ItemsInRows)
                    tr.IsHeader = i == 0;
                Rows.Add(tr);
                for (int j = 0; j < columns; j++)
                {
                    var tc = new TableCell();
                    tc.Content = cells[i, j];
                    tr.Cells.Add(tc);
                }
            }

            for (int j = 0; j < columns; j++)
            {
                var tc = new TableColumn();
                if (ItemsInRows)
                {
                    var f = Fields[j];
                    tc.Alignment = f.Alignment;
                    tc.Width = f.Width;
                }
                else
                {
                    tc.IsHeader = j == 0;
                    tc.Alignment = Alignment;
                }
                Columns.Add(tc);
            }
        }

        public override void WriteContent(IReportWriter w)
        {
            w.WriteTable(this);
        }

        public string[,] ToArray()
        {

            var items = Items.Cast<object>().ToList();
            int nrows = items.Count;

            var hasHeader = HasHeader();
            if (hasHeader) nrows++;

            var result = new string[nrows, Fields.Count];

            int row = 0;
            if (hasHeader)
            {
                for (int i = 0; i < Fields.Count; i++)
                {
                    var c = Fields[i];
                    result[row, i] = c.Header;
                }
                row++;
            }

            foreach (var item in items)
            {
                for (int i = 0; i < Fields.Count; i++)
                {
                    var c = Fields[i];
                    var text = c.GetText(item);
                    result[row, i] = text;
                }
                row++;
            }
            if (!ItemsInRows)
                result = Transpose(result);
            return result;
        }

        private static string[,] Transpose(string[,] input)
        {
            int rows = input.GetUpperBound(0) + 1;
            int cols = input.GetUpperBound(1) + 1;
            var result = new string[cols, rows];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    result[j, i] = input[i, j];
            return result;
        }
    }
}