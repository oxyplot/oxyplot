using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyPlot
{
    /// <summary>
    /// Item used in BarSeriesBase
    /// </summary>
    public class BarItem
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="BarItem" /> class.
        /// </summary>
        public BarItem()
        {
            Label = null;
            Value = double.NaN;
            Color = null;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the Label of BarItem corresponding to the Labels in CategoryAxis.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the Value of the BarItem.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets the Color of the BarItem.
        /// </summary>
        public OxyColor Color { get; set; }

        #endregion
    }
}
