// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarSeriesBase.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using OxyPlot.Axes;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Base class for bar series.
    /// </summary>
    public abstract class BarSeriesBase<T> : XYAxisSeries, IBarSeries where T : BarItemBase
    {
        /// <summary>
        /// The default category axis title
        /// </summary>
        protected const string DefaultCategoryAxisTitle = "Category";

        /// <summary>
        /// The default value axis title
        /// </summary>
        protected const string DefaultValueAxisTitle = "Value";

        /// <summary>
        /// Specifies if the ItemsSourceItems list can be modified.
        /// </summary>
        private bool ownsItemsSourceItems;

        /// <summary>
        /// Initializes a new instance of the <see cref="BarSeriesBase{T}"/> class.
        /// </summary>
        protected BarSeriesBase()
        {
            this.StrokeColor = OxyColors.Black;
            this.BarWidth = 1;
        }

        /// <summary>
        /// Gets the list of items that should be rendered.
        /// </summary>
        public List<T> ActualItems => this.ItemsSource != null ? this.ItemsSourceItems : this.Items;

        /// <inheritdoc/>
        IReadOnlyList<BarItemBase> IBarSeries.ActualItems => this.ActualItems.AsReadOnlyList();

        /// <summary>
        /// Gets or sets the width of the bars. The default value is 1.
        /// </summary>
        public double BarWidth { get; set; }

        /// <inheritdoc/>
        CategoryAxis IBarSeries.CategoryAxis => this.GetCategoryAxis();

        /// <summary>
        /// Gets the items list.
        /// </summary>
        /// <value>A list of <see cref="BarItem" />.</value>
        public List<T> Items { get; } = new List<T>();

        /// <inheritdoc/>
        Axis IBarSeries.ValueAxis => this.XAxis;

        /// <summary>
        /// Gets or sets the items from the items source.
        /// </summary>
        protected List<T> ItemsSourceItems { get; set; }

        /// <inheritdoc/>
        BarSeriesManager IBarSeries.Manager { get => this.Manager; set => this.Manager = value; }

        /// <summary>
        /// Gets or sets the color of the border around the bars.
        /// </summary>
        /// <value>The color of the stroke.</value>
        public OxyColor StrokeColor { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the bar border strokes.
        /// </summary>
        /// <value>The stroke thickness.</value>
        public double StrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the manager of this <see cref="BarSeriesBase{T}"/>.
        /// </summary>
        protected BarSeriesManager Manager { get; set; }

        /// <summary>
        /// Gets the valid items.
        /// </summary>
        protected IList<T> ValidItems { get; } = new List<T>();

        /// <summary>
        /// Gets or sets the dictionary which stores the index-inversion for the valid items
        /// </summary>
        protected Dictionary<int, int> ValidItemsIndexInversion { get; } = new Dictionary<int, int>();

        /// <summary>
        /// Gets the actual width of the items of this series.
        /// </summary>
        /// <returns>The width.</returns>
        /// <remarks>The actual width is also influenced by the GapWidth of the CategoryAxis used by this series.</remarks>
        protected double GetActualBarWidth()
        {
            var categoryAxis = this.GetCategoryAxis();
            return this.BarWidth / (1 + categoryAxis.GapWidth) / this.Manager.GetMaxWidth();
        }

        /// <summary>
        /// Gets the category axis.
        /// </summary>
        /// <returns>The category axis.</returns>
        protected CategoryAxis GetCategoryAxis()
        {
            if (!(this.YAxis is CategoryAxis ca))
            {
                throw new Exception("BarSeries requires a CategoryAxis on the Y Axis.");
            }

            return ca;
        }

        /// <inheritdoc/>
        protected override object GetItem(int i)
        {
            if (this.ItemsSource != null || this.ActualItems == null || this.ActualItems.Count == 0)
            {
                return base.GetItem(i);
            }

            return this.ActualItems[i];
        }

        /// <summary>
        /// Gets a value indicating whether the specified item is valid.
        /// </summary>
        /// <param name="item">The items.</param>
        /// <returns><c>true</c> if the item is valid; <c>false</c> otherwise.</returns>
        protected abstract bool IsValid(T item);

        /// <summary>
        /// Updates the <see cref="ItemsSourceItems"/> from the <see cref="ItemsSeries.ItemsSource"/> and data fields.
        /// </summary>
        protected abstract bool UpdateFromDataFields();

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

        /// <inheritdoc/>
        void IBarSeries.UpdateValidData()
        {
            this.UpdateValidData();
        }

        /// <inheritdoc/>
        protected internal override bool IsUsing(Axis axis)
        {
            return this.XAxis == axis || this.YAxis == axis;
        }

        /// <inheritdoc/>
        protected internal override void UpdateAxisMaxMin()
        {
            this.XAxis.Include(this.MinX);
            this.XAxis.Include(this.MaxX);
        }

        /// <inheritdoc/>
        protected internal override void UpdateData()
        {
            if (this.ItemsSource is List<T> lst)
            {
                this.ItemsSourceItems = lst;
                this.ownsItemsSourceItems = false;
            }
            else
            {
                this.ClearItemsSourceItems();
                if (this.ItemsSource != null && !this.UpdateFromDataFields())
                {
                    this.ItemsSourceItems.AddRange(this.ItemsSource.OfType<T>());
                }
            }
        }

        /// <summary>
        /// Updates the <see cref="ValidItems"/> list with the valid items.
        /// </summary>
        protected void UpdateValidData()
        {
            this.ValidItems.Clear();
            this.ValidItemsIndexInversion.Clear();
            var numberOfCategories = this.Manager.Categories.Count;
            var valueAxis = this.XAxis;

            var i = 0;
            var items = this.ActualItems
                .Where(item => item.GetCategoryIndex(i) < numberOfCategories)
                .Where(this.IsValid);

            foreach (var item in items)
            {
                this.ValidItemsIndexInversion.Add(this.ValidItems.Count, i);
                this.ValidItems.Add(item);
                i++;
            }
        }
    }
}
