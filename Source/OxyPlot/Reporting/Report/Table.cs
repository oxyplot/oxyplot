using System;
using System.Collections.Generic;

namespace OxyPlot.Reporting
{
    public class TableColumn
    {
        /// <summary>
        /// Gets or sets the width.
        /// NaN: auto width.
        /// Negative numbers: weights
        /// </summary>
        /// <value>The width.</value>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the actual width (mm).
        /// </summary>
        /// <value>The actual width.</value>
        public double ActualWidth { get; internal set; }

        public Alignment Alignment { get; set; }

        public bool IsHeader { get; set; }

        public TableColumn()
        {
            Width = double.NaN;
            Alignment = Reporting.Alignment.Center;
        }
    }

    public class TableRow
    {
        public IList<TableCell> Cells { get; private set; }
        public bool IsHeader { get; set; }

        public TableRow()
        {
            Cells = new List<TableCell>();
        }
    }

    public class TableCell
    {
        // public Alignment Alignment { get; set; }
        // public int RowSpan { get; set; }
        // public int ColumnSpan { get; set; }
        public string Content { get; set; }
    }

    public class Table : ReportItem
    {
        public IList<TableRow> Rows { get; private set; }
     
        public IList<TableColumn> Columns { get; private set; }

        /// <summary>
        /// Gets or sets the width of the table (mm).
        /// NaN: auto width.
        /// 0..-1: fraction of page width.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets the actual width of the table (mm).
        /// </summary>
        /// <value>The actual width.</value>
        public double ActualWidth { get; private set; }

        public string Caption { get; set; }
        public int TableNumber { get; set; }

        public string GetFullCaption(ReportStyle style)
        {
            return String.Format(style.TableCaptionFormatString, TableNumber, Caption);
        }

        public Table()
        {
            Rows = new List<TableRow>();
            Columns = new List<TableColumn>();
            Width = double.NaN;
        }

        public override void Update()
        {
            base.Update();
            UpdateWidths();
        }

        private void UpdateWidths()
        {

            if (Width < 0)
                ActualWidth = 150 * (-Width);
            else
                ActualWidth = Width;

            // update actual widths of all columns
            double totalWeight = 0;
            double totalWidth = 0;
            foreach (var c in Columns)
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
            if (double.IsNaN(ActualWidth))
                ActualWidth = Math.Max(150, totalWidth + 100);
            double w = ActualWidth - totalWidth;
            foreach (var c in Columns)
            {
                if (c.Width < 0 && totalWeight != 0)
                {
                    double weight = -c.Width;
                    c.ActualWidth = w * (weight / totalWeight);
                }
            }
        }
    }
}