//-----------------------------------------------------------------------
// <copyright file="ItemsTable.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// The items table.
    /// </summary>
    public class ItemsTable : Table
    {
        #region Constants and Fields

        /// <summary>
        /// The items.
        /// </summary>
        private IEnumerable items;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsTable"/> class.
        /// </summary>
        /// <param name="itemsInRows">
        /// The items in rows.
        /// </param>
        public ItemsTable(bool itemsInRows = true)
        {
            this.Fields = new List<ItemsTableField>();
            this.ItemsInRows = itemsInRows;
            this.Alignment = Alignment.Center;
        }

        #endregion

        #region Public Properties

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
        /// The table will be filled when this property is set. 
        /// </summary>
        /// <value>The items.</value>
        public IEnumerable Items
        {
            get
            {
                return this.items;
            }

            set
            {
                this.items = value;
                this.UpdateItems();
            }
        }

        /// <summary>
        /// Gets a value indicating whether ItemsInRows.
        /// </summary>
        public bool ItemsInRows { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The has header.
        /// </summary>
        /// <returns>
        /// The has header.
        /// </returns>
        public bool HasHeader()
        {
            foreach (ItemsTableField c in this.Fields)
            {
                if (c.Header != null)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// The to array.
        /// </summary>
        /// <returns>
        /// </returns>
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

            foreach (object item in items)
            {
                for (int i = 0; i < this.Fields.Count; i++)
                {
                    ItemsTableField c = this.Fields[i];
                    string text = c.GetText(item);
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
        /// The update items.
        /// </summary>
        public void UpdateItems()
        {
            this.Rows.Clear();
            this.Columns.Clear();
            if (this.Fields == null || this.Fields.Count == 0)
            {
                Debug.WriteLine("ItemsTable: Fields are not defined.");
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
        /// The write content.
        /// </summary>
        /// <param name="w">
        /// The w.
        /// </param>
        public override void WriteContent(IReportWriter w)
        {
            w.WriteTable(this);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The transpose.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// </returns>
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

        #endregion
    }
}
