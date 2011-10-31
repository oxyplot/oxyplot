// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Table.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a table column definition.
    /// </summary>
    public class TableColumn
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "TableColumn" /> class.
        /// </summary>
        public TableColumn()
        {
            this.Width = double.NaN;
            this.Alignment = Alignment.Center;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the actual width (mm).
        /// </summary>
        /// <value>The actual width.</value>
        public double ActualWidth { get; internal set; }

        /// <summary>
        ///   Gets or sets Alignment.
        /// </summary>
        public Alignment Alignment { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether IsHeader.
        /// </summary>
        public bool IsHeader { get; set; }

        /// <summary>
        ///   Gets or sets the width.
        ///   NaN: auto width.
        ///   Negative numbers: weights
        /// </summary>
        /// <value>The width.</value>
        public double Width { get; set; }

        #endregion
    }

    /// <summary>
    /// Represents a table row definition.
    /// </summary>
    public class TableRow
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "TableRow" /> class.
        /// </summary>
        public TableRow()
        {
            this.Cells = new List<TableCell>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets Cells.
        /// </summary>
        public IList<TableCell> Cells { get; private set; }

        /// <summary>
        ///   Gets or sets a value indicating whether IsHeader.
        /// </summary>
        public bool IsHeader { get; set; }

        #endregion
    }

    /// <summary>
    /// Represents a table cell.
    /// </summary>
    public class TableCell
    {
        // public Alignment Alignment { get; set; }
        // public int RowSpan { get; set; }
        // public int ColumnSpan { get; set; }
        #region Public Properties

        /// <summary>
        ///   Gets or sets Content.
        /// </summary>
        public string Content { get; set; }

        #endregion
    }

    /// <summary>
    /// Represents a table.
    /// </summary>
    public class Table : ReportItem
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "Table" /> class.
        /// </summary>
        public Table()
        {
            this.Rows = new List<TableRow>();
            this.Columns = new List<TableColumn>();
            this.Width = double.NaN;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the actual width of the table (mm).
        /// </summary>
        /// <value>The actual width.</value>
        public double ActualWidth { get; private set; }

        /// <summary>
        ///   Gets or sets Caption.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        ///   Gets Columns.
        /// </summary>
        public IList<TableColumn> Columns { get; private set; }

        /// <summary>
        ///   Gets Rows.
        /// </summary>
        public IList<TableRow> Rows { get; private set; }

        /// <summary>
        ///   Gets or sets TableNumber.
        /// </summary>
        public int TableNumber { get; set; }

        /// <summary>
        ///   Gets or sets the width of the table (mm).
        ///   NaN: auto width.
        ///   0..-1: fraction of page width.
        /// </summary>
        public double Width { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The get full caption.
        /// </summary>
        /// <param name="style">
        /// The style.
        /// </param>
        /// <returns>
        /// The get full caption.
        /// </returns>
        public string GetFullCaption(ReportStyle style)
        {
            return string.Format(style.TableCaptionFormatString, this.TableNumber, this.Caption);
        }

        /// <summary>
        /// The update.
        /// </summary>
        public override void Update()
        {
            base.Update();
            this.UpdateWidths();
        }

        /// <summary>
        /// The write content.
        /// </summary>
        /// <param name="w">
        /// The w.
        /// </param>
        public override void WriteContent(IReportWriter w)
        {
            // todo
        }

        #endregion

        #region Methods

        /// <summary>
        /// The update widths.
        /// </summary>
        private void UpdateWidths()
        {
            if (this.Width < 0)
            {
                this.ActualWidth = 150 * (-this.Width);
            }
            else
            {
                this.ActualWidth = this.Width;
            }

            // update actual widths of all columns
            double totalWeight = 0;
            double totalWidth = 0;
            foreach (var c in this.Columns)
            {
                if (double.IsNaN(c.Width))
                {
                    // todo: find auto width
                    c.ActualWidth = 40;
                    totalWidth += c.ActualWidth;
                }

                if (c.Width < 0)
                {
                    totalWeight += -c.Width;
                }

                if (c.Width >= 0)
                {
                    totalWidth += c.Width;
                    c.ActualWidth = c.Width;
                }
            }

            if (double.IsNaN(this.ActualWidth))
            {
                this.ActualWidth = Math.Max(150, totalWidth + 100);
            }

            double w = this.ActualWidth - totalWidth;
            foreach (var c in this.Columns)
            {
                if (c.Width < 0 && totalWeight != 0)
                {
                    double weight = -c.Width;
                    c.ActualWidth = w * (weight / totalWeight);
                }
            }
        }

        #endregion
    }
}