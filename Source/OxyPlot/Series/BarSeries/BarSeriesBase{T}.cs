// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarSeriesBase{T}.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Generic base class that provides common properties and methods for the BarSeries and ColumnSeries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Generic base class that provides common properties and methods for the BarSeries and ColumnSeries.
    /// </summary>
    /// <typeparam name="T">The type of the items.</typeparam>
    public abstract class BarSeriesBase<T> : BarSeriesBase
        where T : BarItemBase, new()
    {
        /// <summary>
        /// Specifies if the ownsItemsSourceItems list can be modified.
        /// </summary>
        private bool ownsItemsSourceItems;

        /// <summary>
        /// Initializes a new instance of the <see cref="BarSeriesBase{T}" /> class. Initializes a new instance of the <see cref="BarSeriesBase&lt;T&gt;" /> class.
        /// </summary>
        protected BarSeriesBase()
        {
            this.Items = new List<T>();
        }

        /// <summary>
        /// Gets the items list.
        /// </summary>
        /// <value>A list of <see cref="BarItem" /> or <see cref="ColumnItem" />.</value>
        public List<T> Items { get; private set; }

        /// <summary>
        /// Gets the list of items that should be rendered.
        /// </summary>
        protected List<T> ActualItems
        {
            get
            {
                return this.ItemsSource != null ? this.ItemsSourceItems : this.Items;
            }
        }

        /// <summary>
        /// Gets or sets the items from the items source.
        /// </summary>
        protected List<T> ItemsSourceItems { get; set; }

        /// <summary>
        /// Gets the items of this series.
        /// </summary>
        /// <returns>The items.</returns>
        protected internal override IList<CategorizedItem> GetItems()
        {
            return this.ActualItems.Cast<CategorizedItem>().ToList();
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

            var sourceAsListOfT = this.ItemsSource as List<T>;
            if (sourceAsListOfT != null)
            {
                this.ItemsSourceItems = sourceAsListOfT;
                this.ownsItemsSourceItems = false;
                return;
            }

            this.ClearItemsSourceItems();

            if (this.ValueField == null)
            {
                this.ItemsSourceItems.AddRange(this.ItemsSource.OfType<T>());
            }
            else
            {
                this.UpdateFromDataFields();
            }
        }

        /// <summary>
        /// Updates the <see cref="F:itemsSourceItems" /> from the <see cref="P:ItemsSource" /> and data fields.
        /// </summary>
        protected abstract void UpdateFromDataFields();

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="i">The index of the item.</param>
        /// <returns>The item of the index.</returns>
        protected override object GetItem(int i)
        {
            if (this.ItemsSource != null || this.ActualItems == null || this.ActualItems.Count == 0)
            {
                return base.GetItem(i);
            }

            return this.ActualItems[i];
        }

        /// <summary>
        /// Clears or creates the <see cref="ItemsSourceItems"/> list.
        /// </summary>
        private void ClearItemsSourceItems()
        {
            if (!this.ownsItemsSourceItems || this.ItemsSourceItems == null)
            {
                this.ItemsSourceItems = new List<T>();
            }
            else
            {
                this.ItemsSourceItems.Clear();
            }

            this.ownsItemsSourceItems = true;
        }
    }
}