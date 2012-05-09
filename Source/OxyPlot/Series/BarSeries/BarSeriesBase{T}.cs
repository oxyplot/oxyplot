// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarSeriesBase{T}.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   Base class that provides common properties and methods for the BarSeries and ColumnSeries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Generic base class that provides common properties and methods for the BarSeries and ColumnSeries.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the items. 
    /// </typeparam>
    public abstract class BarSeriesBase<T> : BarSeriesBase
        where T : BarItemBase, new()
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BarSeriesBase{T}"/> class. Initializes a new instance of the <see cref="BarSeriesBase&lt;T&gt;"/> class.
        /// </summary>
        protected BarSeriesBase()
        {
            this.Items = new List<T>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>
        /// The items. 
        /// </value>
        public IList<T> Items { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the items of this series.
        /// </summary>
        /// <returns>
        /// The items. 
        /// </returns>
        protected internal override IList<CategorizedItem> GetItems()
        {
            return this.Items.Cast<CategorizedItem>().ToList();
        }

        /// <summary>
        /// Updates the data.
        /// </summary>
        protected internal override void UpdateData()
        {
            if (this.ItemsSource == null)
            {
                return;
            }

            var dest = new List<T>();

            // Using reflection to add points
            var filler = new ListFiller<T>();
            filler.Add(this.ValueField, (item, value) => item.Value = Convert.ToDouble(value));
            filler.Add(this.ColorField, (item, value) => item.Color = (OxyColor)value);
            filler.Fill(dest, this.ItemsSource);
            this.Items = dest;
        }

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="i">
        /// The index of the item. 
        /// </param>
        /// <returns>
        /// The item of the index. 
        /// </returns>
        protected override object GetItem(int i)
        {
            if (this.ItemsSource != null || this.Items == null || this.Items.Count == 0)
            {
                return base.GetItem(i);
            }

            return this.Items[i];
        }

        #endregion
    }
}