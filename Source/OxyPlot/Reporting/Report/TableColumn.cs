// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TableColumn.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a table column definition.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    /// <summary>
    /// Represents a table column definition.
    /// </summary>
    public class TableColumn
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "TableColumn" /> class.
        /// </summary>
        public TableColumn()
        {
            this.Width = double.NaN;
            this.Alignment = Alignment.Center;
        }

        /// <summary>
        /// Gets the actual width (mm).
        /// </summary>
        /// <value>The actual width.</value>
        public double ActualWidth { get; internal set; }

        /// <summary>
        /// Gets or sets the horizontal alignment of the column.
        /// </summary>
        public Alignment Alignment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the column is a header.
        /// </summary>
        public bool IsHeader { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        /// <remarks>NaN: auto width.
        /// Negative numbers: weights</remarks>
        public double Width { get; set; }
    }
}