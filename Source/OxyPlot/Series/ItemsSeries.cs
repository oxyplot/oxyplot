//-----------------------------------------------------------------------
// <copyright file="ItemsSeries.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

namespace OxyPlot
{
    using System.Collections;

    /// <summary>
    /// Abstract base class for Series that can contains items.
    /// </summary>
    public abstract class ItemsSeries : XYAxisSeries
    {
        #region Public Properties

        /// <summary>
        ///   Gets or sets the items source.
        /// </summary>
        /// <value>The items source.</value>
        public IEnumerable ItemsSource { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the item of the specified index.
        ///   Returns null if ItemsSource is not set, or the index is outside the boundaries.
        /// </summary>
        /// <param name="itemsSource">
        /// The items source.
        /// </param>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>
        /// The get item.
        /// </returns>
        protected object GetItem(IEnumerable itemsSource, int index)
        {
            if (itemsSource == null || index < 0)
            {
                return null;
            }

            var list = itemsSource as IList;
            if (list != null)
            {
                if (index < list.Count && index >= 0)
                {
                    return list[index];
                }

                return null;
            }

            // todo: can this be improved?
            int i = 0;
            foreach (object item in itemsSource)
            {
                if (i++ == index)
                {
                    return item;
                }
            }

            return null;
        }

        #endregion
    }
}
