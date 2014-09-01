// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Table.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a table.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a table.
    /// </summary>
    public class Table : ReportItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "Table" /> class.
        /// </summary>
        public Table()
        {
            this.Rows = new List<TableRow>();
            this.Columns = new List<TableColumn>();
            this.Width = double.NaN;
        }

        /// <summary>
        /// Gets the actual width of the table (mm).
        /// </summary>
        /// <value>The actual width.</value>
        public double ActualWidth { get; private set; }

        /// <summary>
        /// Gets or sets Caption.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Gets Columns.
        /// </summary>
        public IList<TableColumn> Columns { get; private set; }

        /// <summary>
        /// Gets Rows.
        /// </summary>
        public IList<TableRow> Rows { get; private set; }

        /// <summary>
        /// Gets or sets TableNumber.
        /// </summary>
        public int TableNumber { get; set; }

        /// <summary>
        /// Gets or sets the width of the table (mm).
        /// NaN: auto width.
        /// 0..-1: fraction of page width.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Gets the full caption.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <returns>The caption string.</returns>
        public string GetFullCaption(ReportStyle style)
        {
            return string.Format(style.TableCaptionFormatString, this.TableNumber, this.Caption);
        }

        /// <summary>
        /// Updates the table.
        /// </summary>
        public override void Update()
        {
            base.Update();
            this.UpdateWidths();
        }

        /// <summary>
        /// Writes the content of the table.
        /// </summary>
        /// <param name="w">The target <see cref="IReportWriter" />.</param>
        public override void WriteContent(IReportWriter w)
        {
            // todo
        }

        /// <summary>
        /// Updates the column widths of the table.
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
                if (c.Width < 0 && !totalWeight.Equals(0))
                {
                    double weight = -c.Width;
                    c.ActualWidth = w * (weight / totalWeight);
                }
            }
        }
    }
}