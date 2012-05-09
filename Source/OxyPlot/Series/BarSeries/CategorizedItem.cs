// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategorizedItem.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   Represents an item in a CategorizedSeries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Represents an item in a CategorizedSeries.
    /// </summary>
    public abstract class CategorizedItem
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CategorizedItem"/> class. Initializes a new instance of the <see cref="CategorizedItem"/> class.
        /// </summary>
        protected CategorizedItem()
        {
            this.CategoryIndex = -1;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the index of the category.
        /// </summary>
        /// <value>
        /// The index of the category. 
        /// </value>
        public int CategoryIndex { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the index of the category.
        /// </summary>
        /// <param name="defaultIndex">
        /// The default index. 
        /// </param>
        /// <returns>
        /// The index. 
        /// </returns>
        internal int GetCategoryIndex(int defaultIndex)
        {
            if (this.CategoryIndex < 0)
            {
                return defaultIndex;
            }

            return this.CategoryIndex;
        }

        #endregion
    }
}