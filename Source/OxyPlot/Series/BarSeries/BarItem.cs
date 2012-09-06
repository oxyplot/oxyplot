// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarItem.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   Represents an item used in the BarSeries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Represents an item used in the BarSeries.
    /// </summary>
    public class BarItem : BarItemBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BarItem"/> class.
        /// </summary>
        public BarItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BarItem"/> class.
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
        public BarItem(double value, int categoryIndex = -1, OxyColor color = null)
        {
            this.Value = value;
            this.CategoryIndex = categoryIndex;
            this.Color = color;
        }

        #endregion
    }
}