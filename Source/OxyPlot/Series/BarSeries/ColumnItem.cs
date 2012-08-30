// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColumnItem.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   Represents an item used in the ColumnSeries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Represents an item used in the ColumnSeries.
    /// </summary>
    public class ColumnItem : BarItemBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnItem"/> class.
        /// </summary>
        public ColumnItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnItem"/> class.
        /// </summary>
        /// <param name="value">
        /// The value. 
        /// </param>
        /// <param name="categoryIndex">
        /// Index of the category. 
        /// </param>
        /// <param name="color">
        /// The color. 
        /// </param>
        public ColumnItem(double value, int categoryIndex = -1, OxyColor color = null)
        {
            this.Value = value;
            this.CategoryIndex = categoryIndex;
            this.Color = color;
        }

        #endregion
    }
}