// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemsTable.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a table of items.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a table of items.
    /// </summary>
    public class ItemsTable : Table
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsTable" /> class.
        /// </summary>
        /// <param name="itemsInRows">The items in rows.</param>
        public ItemsTable(bool itemsInRows = true)
        {
            this.Fields = new List<ItemsTableField>();
            this.ItemsInRows = itemsInRows;
            this.Alignment = Alignment.Center;
        }

        /// <summary>
        /// Gets or sets Alignment.
        /// </summary>
        public Alignment Alignment { get; set; }

        /// <summary>
        /// Gets or sets Fields.
        /// </summary>
        public IList<ItemsTableField> Fields { get; set; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>The items.</value>
        /// <remarks>The table will be filled when this property is set.</remarks>
        public IEnumerable Items { get; set; }

        /// <summary>
        /// Gets a value indicating whether the items should be exported in rows.
        /// </summary>
        public bool ItemsInRows { get; private set; }

        /// <summary>
        /// Determines if the table has a header.
        /// </summary>
        /// <returns><c>true</c> if the table has a header.</returns>
        public bool HasHeader()
        {
            return this.Fields.Any(c => c.Header != null);
        }

        /// <summary>
        /// Converts the table to an array of strings.
        /// </summary>
        /// <returns>A string array.</returns>
        public string[,] ToArray()
        {
            List<object> items = this.Items.Cast<object>().ToList();
            int nrows = items.Count;

            bool hasHeader = this.HasHeader();
            if (hasHeader)
            {
                nrows++;
            }

            var result = new string[nrows, this.Fields.Count];

            int row = 0;
            if (hasHeader)
            {
                for (int i = 0; i < this.Fields.Count; i++)
                {
                    ItemsTableField c = this.Fields[i];
                    result[row, i] = c.Header;
                }

                row++;
            }

            foreach (var item in items)
            {
                for (int i = 0; i < this.Fields.Count; i++)
                {
                    ItemsTableField c = this.Fields[i];
                    string text = c.GetText(item, this.Report.ActualCulture);
                    result[row, i] = text;
                }

                row++;
            }

            if (!this.ItemsInRows)
            {
                result = Transpose(result);
            }

            return result;
        }

        /// <summary>
        /// Updates the table.
        /// </summary>
        public override void Update()
        {
            base.Update();
            this.UpdateItems();
        }

        /// <summary>
        /// Updates the table items.
        /// </summary>
        public void UpdateItems()
        {
            this.Rows.Clear();
            this.Columns.Clear();
            if (this.Fields == null || this.Fields.Count == 0)
            {
                return;
            }

            string[,] cells = this.ToArray();

            int rows = cells.GetUpperBound(0) + 1;
            int columns = cells.GetUpperBound(1) + 1;
            for (int i = 0; i < rows; i++)
            {
                var tr = new TableRow();
                if (this.ItemsInRows)
                {
                    tr.IsHeader = i == 0;
                }

                this.Rows.Add(tr);
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
                if (this.ItemsInRows)
                {
                    ItemsTableField f = this.Fields[j];
                    tc.Alignment = f.Alignment;
                    tc.Width = f.Width;
                }
                else
                {
                    tc.IsHeader = j == 0;
                    tc.Alignment = this.Alignment;
                }

                this.Columns.Add(tc);
            }
        }

        /// <summary>
        /// Writes the content of the item.
        /// </summary>
        /// <param name="w">The writer.</param>
        public override void WriteContent(IReportWriter w)
        {
            w.WriteTable(this);
        }

        /// <summary>
        /// Transposes the specified string array.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>A transposed string array.</returns>
        private static string[,] Transpose(string[,] input)
        {
            int rows = input.GetUpperBound(0) + 1;
            int cols = input.GetUpperBound(1) + 1;
            var result = new string[cols, rows];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[j, i] = input[i, j];
                }
            }

            return result;
        }
    }
}