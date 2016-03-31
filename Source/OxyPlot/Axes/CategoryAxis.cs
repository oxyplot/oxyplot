// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryAxis.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a category axis.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Axes
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using OxyPlot.Series;

    /// <summary>
    /// Represents a category axis.
    /// </summary>
    /// <remarks>The category axis is using the index of the label collection items as coordinates.
    /// If you have 5 categories in the Labels collection, the categories will be placed at coordinates 0 to 4.
    /// The range of the axis will be from -0.5 to 4.5 (excluding padding).</remarks>
    public class CategoryAxis : LinearAxis
    {
        /// <summary>
        /// The labels.
        /// </summary>
        private readonly List<string> labels = new List<string>();

        /// <summary>
        /// The labels from the <see cref="ItemsSource" />.
        /// </summary>
        private readonly List<string> itemsSourceLabels = new List<string>();

        /// <summary>
        /// The current offset of the bars (not used for stacked bar series).
        /// </summary>
        /// <remarks>These offsets are modified during rendering.</remarks>
        private double[] currentBarOffset;

        /// <summary>
        /// The current max value per StackIndex and Label.
        /// </summary>
        /// <remarks>These values are modified during rendering.</remarks>
        private double[,] currentMaxValue;

        /// <summary>
        /// The current min value per StackIndex and Label.
        /// </summary>
        /// <remarks>These values are modified during rendering.</remarks>
        private double[,] currentMinValue;

        /// <summary>
        /// The base value per StackIndex and Label for positive values of stacked bar series.
        /// </summary>
        /// <remarks>These values are modified during rendering.</remarks>
        private double[,] currentPositiveBaseValues;

        /// <summary>
        /// The base value per StackIndex and Label for negative values of stacked bar series.
        /// </summary>
        /// <remarks>These values are modified during rendering.</remarks>
        private double[,] currentNegativeBaseValues;

        /// <summary>
        /// The maximum stack index.
        /// </summary>
        private int maxStackIndex;

        /// <summary>
        /// The maximal width of all labels.
        /// </summary>
        private double maxWidth;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryAxis" /> class.
        /// </summary>
        public CategoryAxis()
        {
            this.TickStyle = TickStyle.Outside;
            this.Position = AxisPosition.Bottom;
            this.MinimumPadding = 0;
            this.MaximumPadding = 0;
            this.MajorStep = 1;
            this.GapWidth = 1;
        }

        /// <summary>
        /// Gets or sets the gap width.
        /// </summary>
        /// <remarks>The default value is 1.0 (100%). The gap width is given as a fraction of the total width/height of the items in a category.</remarks>
        public double GapWidth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the ticks are centered. If this is <c>false</c>, ticks will be drawn between each category. If this is <c>true</c>, ticks will be drawn in the middle of each category.
        /// </summary>
        public bool IsTickCentered { get; set; }

        /// <summary>
        /// Gets or sets the items source (used to update the Labels collection).
        /// </summary>
        /// <value>The items source.</value>
        public IEnumerable ItemsSource { get; set; }

        /// <summary>
        /// Gets or sets the data field for the labels.
        /// </summary>
        public string LabelField { get; set; }

        /// <summary>
        /// Gets the list of category labels.
        /// </summary>
        public List<string> Labels
        {
            get
            {
                return this.labels;
            }
        }

        /// <summary>
        /// Gets the actual category labels.
        /// </summary>
        /// <value>
        /// The actual labels.
        /// </value>
        public List<string> ActualLabels
        {
            get
            {
                return this.ItemsSource != null ? this.itemsSourceLabels : this.labels;
            }
        }

        /// <summary>
        /// Gets or sets the original offset of the bars (not used for stacked bar series).
        /// </summary>
        private double[] BarOffset { get; set; }

        /// <summary>
        /// Gets or sets the stack index mapping. The mapping indicates to which rank a specific stack index belongs.
        /// </summary>
        private Dictionary<string, int> StackIndexMapping { get; set; }

        /// <summary>
        /// Gets or sets the offset of the bars per StackIndex and Label (only used for stacked bar series).
        /// </summary>
        private double[,] StackedBarOffset { get; set; }

        /// <summary>
        /// Gets or sets sum of the widths of the single bars per label. This is used to find the bar width of BarSeries
        /// </summary>
        private double[] TotalWidthPerCategory { get; set; }

        /// <summary>
        /// Gets the maximum width of all category labels.
        /// </summary>
        /// <returns>The maximum width.</returns>
        public double GetMaxWidth()
        {
            return this.maxWidth;
        }

        /// <summary>
        /// Gets the category value.
        /// </summary>
        /// <param name="categoryIndex">Index of the category.</param>
        /// <param name="stackIndex">Index of the stack.</param>
        /// <param name="actualBarWidth">Actual width of the bar.</param>
        /// <returns>The get category value.</returns>
        public double GetCategoryValue(int categoryIndex, int stackIndex, double actualBarWidth)
        {
            var offsetBegin = this.StackedBarOffset[stackIndex, categoryIndex];
            var offsetEnd = this.StackedBarOffset[stackIndex + 1, categoryIndex];
            return categoryIndex - 0.5 + ((offsetEnd + offsetBegin - actualBarWidth) * 0.5);
        }

        /// <summary>
        /// Gets the category value.
        /// </summary>
        /// <param name="categoryIndex">Index of the category.</param>
        /// <returns>The get category value.</returns>
        public double GetCategoryValue(int categoryIndex)
        {
            return categoryIndex - 0.5 + this.BarOffset[categoryIndex];
        }

        /// <summary>
        /// Gets the coordinates used to draw ticks and tick labels (numbers or category names).
        /// </summary>
        /// <param name="majorLabelValues">The major label values.</param>
        /// <param name="majorTickValues">The major tick values.</param>
        /// <param name="minorTickValues">The minor tick values.</param>
        public override void GetTickValues(
            out IList<double> majorLabelValues, out IList<double> majorTickValues, out IList<double> minorTickValues)
        {
            base.GetTickValues(out majorLabelValues, out majorTickValues, out minorTickValues);
            minorTickValues.Clear();

            if (!this.IsTickCentered)
            {
                // Subtract 0.5 from the label values to get the tick values.
                // Add one extra tick at the end.
                var mv = new List<double>(majorLabelValues.Count);
                mv.AddRange(majorLabelValues.Select(v => v - 0.5));
                if (mv.Count > 0)
                {
                    mv.Add(mv[mv.Count - 1] + 1);
                }

                majorTickValues = mv;
            }
        }

        /// <summary>
        /// Gets the value from an axis coordinate, converts from double to the correct data type if necessary. e.g. DateTimeAxis returns the DateTime and CategoryAxis returns category strings.
        /// </summary>
        /// <param name="x">The coordinate.</param>
        /// <returns>The value.</returns>
        public override object GetValue(double x)
        {
            return this.FormatValue(x);
        }

        /// <summary>
        /// Gets the current bar offset for the specified category index.
        /// </summary>
        /// <param name="categoryIndex">The category index.</param>
        /// <returns>The offset.</returns>
        public double GetCurrentBarOffset(int categoryIndex)
        {
            return this.currentBarOffset[categoryIndex];
        }

        /// <summary>
        /// Increases the current bar offset for the specified category index.
        /// </summary>
        /// <param name="categoryIndex">The category index.</param>
        /// <param name="delta">The offset increase.</param>
        public void IncreaseCurrentBarOffset(int categoryIndex, double delta)
        {
            this.currentBarOffset[categoryIndex] += delta;
        }

        /// <summary>
        /// Gets the current base value for the specified stack and category index.
        /// </summary>
        /// <param name="stackIndex">The stack index.</param>
        /// <param name="categoryIndex">The category index.</param>
        /// <param name="negativeValue">if set to <c>true</c> get the base value for negative values.</param>
        /// <returns>The current base value.</returns>
        public double GetCurrentBaseValue(int stackIndex, int categoryIndex, bool negativeValue)
        {
            return negativeValue ? this.currentNegativeBaseValues[stackIndex, categoryIndex] : this.currentPositiveBaseValues[stackIndex, categoryIndex];
        }

        /// <summary>
        /// Sets the current base value for the specified stack and category index.
        /// </summary>
        /// <param name="stackIndex">Index of the stack.</param>
        /// <param name="categoryIndex">Index of the category.</param>
        /// <param name="negativeValue">if set to <c>true</c> set the base value for negative values.</param>
        /// <param name="newValue">The new value.</param>
        public void SetCurrentBaseValue(int stackIndex, int categoryIndex, bool negativeValue, double newValue)
        {
            if (negativeValue)
            {
                this.currentNegativeBaseValues[stackIndex, categoryIndex] = newValue;
            }
            else
            {
                this.currentPositiveBaseValues[stackIndex, categoryIndex] = newValue;
            }
        }

        /// <summary>
        /// Gets the current maximum value for the specified stack and category index.
        /// </summary>
        /// <param name="stackIndex">The stack index.</param>
        /// <param name="categoryIndex">The category index.</param>
        /// <returns>The current value.</returns>
        public double GetCurrentMaxValue(int stackIndex, int categoryIndex)
        {
            return this.currentMaxValue[stackIndex, categoryIndex];
        }

        /// <summary>
        /// Sets the current maximum value for the specified stack and category index.
        /// </summary>
        /// <param name="stackIndex">The stack index.</param>
        /// <param name="categoryIndex">The category index.</param>
        /// <param name="newValue">The new value.</param>
        public void SetCurrentMaxValue(int stackIndex, int categoryIndex, double newValue)
        {
            this.currentMaxValue[stackIndex, categoryIndex] = newValue;
        }

        /// <summary>
        /// Gets the current minimum value for the specified stack and category index.
        /// </summary>
        /// <param name="stackIndex">The stack index.</param>
        /// <param name="categoryIndex">The category index.</param>
        /// <returns>The current value.</returns>
        public double GetCurrentMinValue(int stackIndex, int categoryIndex)
        {
            return this.currentMinValue[stackIndex, categoryIndex];
        }

        /// <summary>
        /// Sets the current minimum value for the specified stack and category index.
        /// </summary>
        /// <param name="stackIndex">The stack index.</param>
        /// <param name="categoryIndex">The category index.</param>
        /// <param name="newValue">The new value.</param>
        public void SetCurrentMinValue(int stackIndex, int categoryIndex, double newValue)
        {
            this.currentMinValue[stackIndex, categoryIndex] = newValue;
        }

        /// <summary>
        /// Gets the stack index for the specified stack group.
        /// </summary>
        /// <param name="stackGroup">The stack group.</param>
        /// <returns>The stack index.</returns>
        public int GetStackIndex(string stackGroup)
        {
            return this.StackIndexMapping[stackGroup];
        }

        /// <summary>
        /// Updates the actual maximum and minimum values. If the user has zoomed/panned the axis, the internal ViewMaximum/ViewMinimum values will be used. If Maximum or Minimum have been set, these values will be used. Otherwise the maximum and minimum values of the series will be used, including the 'padding'.
        /// </summary>
        internal override void UpdateActualMaxMin()
        {
            // Update the DataMinimum/DataMaximum from the number of categories
            this.Include(-0.5);

            var actualLabels = this.ActualLabels;

            if (actualLabels.Count > 0)
            {
                this.Include((actualLabels.Count - 1) + 0.5);
            }
            else
            {
                this.Include(0.5);
            }

            base.UpdateActualMaxMin();

            this.MinorStep = 1;
        }

        /// <summary>
        /// Updates the axis with information from the plot series.
        /// </summary>
        /// <param name="series">The series collection.</param>
        /// <remarks>This is used by the category axis that need to know the number of series using the axis.</remarks>
        internal override void UpdateFromSeries(Series[] series)
        {
            base.UpdateFromSeries(series);

            this.UpdateLabels(series);

            var actualLabels = this.ActualLabels;
            if (actualLabels.Count == 0)
            {
                this.TotalWidthPerCategory = null;
                this.maxWidth = double.NaN;
                this.BarOffset = null;
                this.StackedBarOffset = null;
                this.StackIndexMapping = null;

                return;
            }

            this.TotalWidthPerCategory = new double[actualLabels.Count];

            var usedSeries = series.Where(s => s.IsUsing(this)).ToList();

            // Add width of stacked series
            var categorizedSeries = usedSeries.OfType<CategorizedSeries>().ToList();
            var stackedSeries = categorizedSeries.OfType<IStackableSeries>().Where(s => s.IsStacked).ToList();
            var stackIndices = stackedSeries.Select(s => s.StackGroup).Distinct().ToList();
            var stackRankBarWidth = new Dictionary<int, double>();
            for (var j = 0; j < stackIndices.Count; j++)
            {
                var maxBarWidth =
                    stackedSeries.Where(s => s.StackGroup == stackIndices[j]).Select(
                        s => ((CategorizedSeries)s).GetBarWidth()).Concat(new[] { 0.0 }).Max();
                for (var i = 0; i < actualLabels.Count; i++)
                {
                    int k = 0;
                    if (
                        stackedSeries.SelectMany(s => ((CategorizedSeries)s).GetItems()).Any(
                            item => item.GetCategoryIndex(k++) == i))
                    {
                        this.TotalWidthPerCategory[i] += maxBarWidth;
                    }
                }

                stackRankBarWidth[j] = maxBarWidth;
            }

            // Add width of unstacked series
            var unstackedBarSeries = categorizedSeries.Where(s => !(s is IStackableSeries) || !((IStackableSeries)s).IsStacked).ToList();
            foreach (var s in unstackedBarSeries)
            {
                for (var i = 0; i < actualLabels.Count; i++)
                {
                    int j = 0;
                    var numberOfItems = s.GetItems().Count(item => item.GetCategoryIndex(j++) == i);
                    this.TotalWidthPerCategory[i] += s.GetBarWidth() * numberOfItems;
                }
            }

            this.maxWidth = this.TotalWidthPerCategory.Max();

            // Calculate BarOffset and StackedBarOffset
            this.BarOffset = new double[actualLabels.Count];
            this.StackedBarOffset = new double[stackIndices.Count + 1, actualLabels.Count];

            var factor = 0.5 / (1 + this.GapWidth) / this.maxWidth;
            for (var i = 0; i < actualLabels.Count; i++)
            {
                this.BarOffset[i] = 0.5 - (this.TotalWidthPerCategory[i] * factor);
            }

            for (var j = 0; j <= stackIndices.Count; j++)
            {
                for (var i = 0; i < actualLabels.Count; i++)
                {
                    int k = 0;
                    if (
                        stackedSeries.SelectMany(s => ((CategorizedSeries)s).GetItems()).All(
                            item => item.GetCategoryIndex(k++) != i))
                    {
                        continue;
                    }

                    this.StackedBarOffset[j, i] = this.BarOffset[i];
                    if (j < stackIndices.Count)
                    {
                        this.BarOffset[i] += stackRankBarWidth[j] / (1 + this.GapWidth) / this.maxWidth;
                    }
                }
            }

            stackIndices.Sort();
            this.StackIndexMapping = new Dictionary<string, int>();
            for (var i = 0; i < stackIndices.Count; i++)
            {
                this.StackIndexMapping.Add(stackIndices[i], i);
            }

            this.maxStackIndex = stackIndices.Count;
        }

        /// <summary>
        /// Resets the current values.
        /// </summary>
        /// <remarks>The current values may be modified during update of max/min and rendering.</remarks>
        protected internal override void ResetCurrentValues()
        {
            base.ResetCurrentValues();
            this.currentBarOffset = this.BarOffset != null ? this.BarOffset.ToArray() : null;
            var actualLabels = this.ActualLabels;
            if (this.maxStackIndex > 0)
            {
                this.currentPositiveBaseValues = new double[this.maxStackIndex, actualLabels.Count];
                this.currentPositiveBaseValues.Fill2D(double.NaN);
                this.currentNegativeBaseValues = new double[this.maxStackIndex, actualLabels.Count];
                this.currentNegativeBaseValues.Fill2D(double.NaN);

                this.currentMaxValue = new double[this.maxStackIndex, actualLabels.Count];
                this.currentMaxValue.Fill2D(double.NaN);
                this.currentMinValue = new double[this.maxStackIndex, actualLabels.Count];
                this.currentMinValue.Fill2D(double.NaN);
            }
            else
            {
                this.currentPositiveBaseValues = null;
                this.currentNegativeBaseValues = null;
                this.currentMaxValue = null;
                this.currentMinValue = null;
            }
        }

        /// <summary>
        /// Formats the value to be used on the axis.
        /// </summary>
        /// <param name="x">The value to format.</param>
        /// <returns>The formatted value.</returns>
        protected override string FormatValueOverride(double x)
        {
            var index = (int)x;
            var actualLabels = this.ActualLabels;
            if (index >= 0 && index < actualLabels.Count)
            {
                return actualLabels[index];
            }

            return null;
        }

        /// <summary>
        /// Creates Labels list if no labels were set
        /// </summary>
        /// <param name="series">The list of series which are rendered</param>
        private void UpdateLabels(IEnumerable<Series> series)
        {
            if (this.ItemsSource != null)
            {
                this.itemsSourceLabels.Clear();
                this.itemsSourceLabels.AddRange(this.ItemsSource.Format(this.LabelField, this.StringFormat, this.ActualCulture));
                return;
            }

            if (this.Labels.Count == 0)
            {
                // auto-create labels
                // TODO: should not modify Labels collection
                foreach (var s in series)
                {
                    if (!s.IsUsing(this))
                    {
                        continue;
                    }

                    var bsb = s as CategorizedSeries;
                    if (bsb != null)
                    {
                        int max = bsb.GetItems().Count;
                        while (this.Labels.Count < max)
                        {
                            this.Labels.Add((this.Labels.Count + 1).ToString(CultureInfo.InvariantCulture));
                        }
                    }
                }
            }
        }
    }
}