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
        /// <param name="value">
        /// The value. 
        /// </param>
        /// <param name="error">
        /// The error. 
        /// </param>
        /// <param name="categoryIndex">
        /// Index of the category. 
        /// </param>
        /// <param name="color">
        /// The color. 
        /// </param>
        public ErrorColumnItem(double value, double error, int categoryIndex = -1, OxyColor color = null)
        {
            this.Value = value;
            this.Error = error;
            this.CategoryIndex = categoryIndex;
            this.Color = color;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the error of the item.
        /// </summary>
        public double Error { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns c# code that generates this instance.
        /// </summary>
        /// <returns>
        /// C# code. 
        /// </returns>
        public override string ToCode()
        {
            if (this.Color != null)
            {
                return CodeGenerator.FormatConstructor(
                    this.GetType(), "{0},{1},{2},{3}", this.Value, this.Error, this.CategoryIndex, this.Color.ToCode());
            }

            if (this.CategoryIndex != -1)
            {
                return CodeGenerator.FormatConstructor(
                    this.GetType(), "{0},{1},{2}", this.Value, this.Error, this.CategoryIndex);
            }

            return CodeGenerator.FormatConstructor(this.GetType(), "{0},{1}", this.Value, this.Error);
        }

        #endregion
    }
}