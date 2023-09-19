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
        IReadOnlyList<BarItemBase> IBarSeries.ActualItems => this.ActualItems;

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
        /// Gets or sets the label color.
        /// </summary>
        public OxyColor LabelColor { get; set; }

        /// <summary>
        /// Gets or sets the label margins.
        /// </summary>
        public double LabelMargin { get; set; }

        /// <summary>
        /// Gets or sets the label angle in degrees.
        /// </summary>
        public double LabelAngle { get; set; }

        /// <summary>
        /// Gets or sets label placements.
        /// </summary>
        public LabelPlacement LabelPlacement { get; set; }

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

        /// <summary>
        /// Renders the item label.
        /// </summary>
        /// <param name="rc">The render context</param>
        /// <param name="item">The item.</param>
        /// <param name="baseValue">The bar item base value.</param>
        /// <param name="topValue">The bar item top value.</param>
        /// <param name="categoryValue">The bar item category value.</param>
        /// <param name="categoryEndValue">The bar item category end value.</param>
        /// <param name="labelFormatString">The format string to use for the label.</param>
        /// <param name="labelValues">An optional set of data values to use when generating label strings.</param>
        protected void RenderLabel(
            IRenderContext rc,
            T item,
            double baseValue,
            double topValue,
            double categoryValue,
            double categoryEndValue,
            string labelFormatString,
            params double[] labelValues)
        {
            var v = new List<object>();
            if (!labelValues.Any())
            {
                switch (item)
                {
                    case BarItem barItem:
                        v.Add(barItem.Value);
                        break;

                    case IntervalBarItem intervalBarItem:
                        v.Add(intervalBarItem.Start);
                        v.Add(intervalBarItem.End);
                        break;

                    case TornadoBarItem tornadoBarItem:
                        throw new NotImplementedException(
                            $"RenderLabel does not support automatic determination of label values for TornadoBarItem objects. Please populate the {nameof(labelValues)} parameter.");

                    default:
                        throw new NotImplementedException(
                            $"RenderLabel automatic value determination not implemented for {this.GetType().Name}. Please populate the {nameof(labelValues)} parameter.");
                }
            }
            else
            {
                v.AddRange(labelValues.Cast<object>());
            }

            var s = StringHelper.Format(this.ActualCulture, labelFormatString, item, v.ToArray());
            ScreenPoint pt;
            var y = (categoryEndValue + categoryValue) / 2;
            var sign = Math.Sign(topValue - baseValue);
            var marginVector = new ScreenVector(this.LabelMargin, 0) * sign;
            var centreVector = new ScreenVector(0, 0);

            var size = rc.MeasureText(
                s,
                this.ActualFont,
                this.ActualFontSize,
                this.ActualFontWeight,
                this.LabelAngle);
            var halfSize = (this.IsTransposed() ? size.Height : size.Width) / 2;

            switch (this.LabelPlacement)
            {
                case LabelPlacement.Inside:
                    pt = this.Transform(topValue, y);
                    marginVector = -marginVector;
                    centreVector = new ScreenVector(-sign * halfSize, 0);
                    break;
                case LabelPlacement.Outside:
                    pt = this.Transform(topValue, y);
                    centreVector = new ScreenVector(sign * halfSize, 0);
                    break;
                case LabelPlacement.Middle:
                    pt = this.Transform((topValue + baseValue) / 2, y);
                    marginVector = new ScreenVector(0, 0);
                    break;
                case LabelPlacement.Base:
                    pt = this.Transform(baseValue, y);
                    centreVector = new ScreenVector(sign * halfSize, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            pt += this.Orientate(marginVector) + this.Orientate(centreVector);

            rc.DrawText(
                pt,
                s,
                this.ActualTextColor,
                this.ActualFont,
                this.ActualFontSize,
                this.ActualFontWeight,
                this.LabelAngle,
                HorizontalAlignment.Center,
                VerticalAlignment.Middle);
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
            for (int index = 0; index < this.ActualItems.Count; index++)
            {
                var item = this.ActualItems[index];
                if (item != null && item.GetCategoryIndex(index) < numberOfCategories && this.IsValid(item))
                {
                    this.ValidItemsIndexInversion.Add(this.ValidItems.Count, index);
                    this.ValidItems.Add(item);
                }
            }
        }
    }
}
