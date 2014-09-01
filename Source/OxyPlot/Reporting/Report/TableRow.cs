// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TableRow.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a table row definition.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a table row definition.
    /// </summary>
    public class TableRow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "TableRow" /> class.
        /// </summary>
        public TableRow()
        {
            this.Cells = new List<TableCell>();
        }

        /// <summary>
        /// Gets Cells.
        /// </summary>
        public IList<TableCell> Cells { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsHeader.
        /// </summary>
        public bool IsHeader { get; set; }
    }
}