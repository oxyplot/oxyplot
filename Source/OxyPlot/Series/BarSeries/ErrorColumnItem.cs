// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorColumnItem.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   Represents an item used in the ErrorColumnSeries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot
{
    /// <summary>
    /// Represents an item used in the ErrorColumnSeries.
    /// </summary>
    public class ErrorColumnItem : ColumnItem
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorColumnItem"/> class.
        /// </summary>
        public ErrorColumnItem()
        {
        }
 
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorColumnItem"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="errorValue">The errorValue. </param>
        /// <param name="categoryIndex">Index of the category. </param>
        /// <param name="color">The color. </param>
        public ErrorColumnItem(double value, double errorValue, int categoryIndex = -1, OxyColor color = null)
        {
            this.Value = value;
            this.ErrorValue = errorValue;
            this.CategoryIndex = categoryIndex;
            this.Color = color;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the error value of the item.
        /// </summary>
        public double ErrorValue { get; set; }

        #endregion

    }
}
