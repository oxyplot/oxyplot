// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarSeriesManager.cs" company="OxyPlot">
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
    /// Represents a manager for bar series.
    /// </summary>
    /// <remarks>
    /// This handles all operations that need information about all bar series in the plot that share the same axes. This includes:
    /// - determine and keep track of bar width and offset
    /// - determine and keep track of stacked bar offsets
    /// </remarks>
    public class BarSeriesManager
    {
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
        /// The base value per StackIndex and Label for negative values of stacked bar series.
        /// </summary>
        /// <remarks>These values are modified during rendering.</remarks>
        private double[,] currentNegativeBaseValues;

        /// <summary>
        /// The base value per StackIndex and Label for positive values of stacked bar series.
        /// </summary>
        /// <remarks>These values are modified during rendering.</remarks>
        private double[,] currentPositiveBaseValues;

        /// <summary>
        /// The maximal width of all labels.
        /// </summary>
        private double maxWidth;

        /// <summary>
        /// Initializes a new instance of the <see cref="BarSeriesManager"/> class.
        /// </summary>
        /// <param name="categoryAxis">The category axis the <paramref name="series"/> belong to.</param>
        /// <param name="valueAxis">The value axis the <paramref name="series"/> belong to.</param>
        /// <param name="series">The bar series this instance should manage.</param>
        public BarSeriesManager(CategoryAxis categoryAxis, Axis valueAxis, IEnumerable<IBarSeries> series)
        {
            this.PlotModel = categoryAxis.PlotModel ?? throw new InvalidOperationException("The category axis must be part of a plot model.");
            this.CategoryAxis = categoryAxis;
            this.ValueAxis = valueAxis;
            this.ManagedSeries = series.ToList();

            foreach (var s in this.ManagedSeries)
            {
                s.Manager = this;
            }
        }

        /// <summary>
        /// Gets the <see cref="CategoryAxis"/> whose bar series this instance manages.
        /// </summary>
        public CategoryAxis CategoryAxis { get; }

        /// <summary>
        /// Gets all bar series that are managed by this instance.
        /// </summary>
        public IList<IBarSeries> ManagedSeries { get; }

        /// <summary>
        /// Gets the <see cref="PlotModel"/> whose bar series this instance manages.
        /// </summary>
        public PlotModel PlotModel { get; }

        /// <summary>
        /// Gets the value <see cref="Axis"/> whose bar series this instance manages.
        /// </summary>
        public Axis ValueAxis { get; }

        /// <summary>
        /// Gets the string representation of the categories.
        /// </summary>
        internal IList<string> Categories => this.CategoryAxis.ActualLabels;

        /// <summary>
        /// Gets or set the offset of the bars.
        /// </summary>
        private double[] BarOffset { get; set; }

        /// <summary>
        /// Gets or sets the offset of the bars per StackIndex and Label (only used for stacked bar series).
        /// </summary>
        private double[,] StackedBarOffset { get; set; }

        /// <summary>
        /// Gets or sets the stack index mapping. The mapping indicates to which rank a specific stack index belongs.
        /// </summary>
        private Dictionary<string, int> StackIndexMapping { get; } = new Dictionary<string, int>();

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
        /// Gets the current bar offset for the specified category index.
        /// </summary>
        /// <param name="categoryIndex">The category index.</param>
        /// <returns>The offset.</returns>
        public double GetCurrentBarOffset(int categoryIndex)
        {
            return this.currentBarOffset[categoryIndex];
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
        /// Gets the maximum width of all category labels.
        /// </summary>
        /// <returns>The maximum width.</returns>
        public double GetMaxWidth()
        {
            return this.maxWidth;
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
        /// Increases the current bar offset for the specified category index.
        /// </summary>
        /// <param name="categoryIndex">The category index.</param>
        /// <param name="delta">The offset increase.</param>
        public void IncreaseCurrentBarOffset(int categoryIndex, double delta)
        {
            this.currentBarOffset[categoryIndex] += delta;
        }

        /// <summary>
        /// Initializes the manager for rendering. This should be called before any of the managed series are rendered.
        /// </summary>
        public void InitializeRender()
        {
            this.ResetCurrentValues();
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
        /// Bar series should call this after they updated their data.
        /// </summary>
        public void Update()
        {
            this.CategoryAxis.UpdateLabels(this.ManagedSeries.Max(s => s.ActualItems.Count));
            this.UpdateBarOffsets();
            this.UpdateValidData();
            this.ResetCurrentValues();
        }

        /// <summary>
        /// Gets a value indicating whether the bar series has an item at the specified category index.
        /// </summary>
        /// <param name="series">The bar series.</param>
        /// <param name="categoryIndex">The category index.</param>
        /// <returns><c>true</c> of the bar series has an item at the specified category index; <c>false</c> otherwise.</returns>
        private static bool HasCategory(IBarSeries series, int categoryIndex)
        {
            if (series.ActualItems.Count > categoryIndex && series.ActualItems[categoryIndex].CategoryIndex < 0)
            {
                return true;
            }

            return series.ActualItems.Any(item => item.CategoryIndex == categoryIndex);
        }

        /// <summary>
        /// Resets the current values.
        /// </summary>
        /// <remarks>The current values may be modified during update of max/min and rendering.</remarks>
        private void ResetCurrentValues()
        {
            this.currentBarOffset = this.BarOffset?.ToArray();
            var actualLabels = this.CategoryAxis.ActualLabels;
            if (this.StackIndexMapping.Count > 0)
            {
                this.currentPositiveBaseValues = new double[this.StackIndexMapping.Count, actualLabels.Count];
                this.currentPositiveBaseValues.Fill2D(double.NaN);
                this.currentNegativeBaseValues = new double[this.StackIndexMapping.Count, actualLabels.Count];
                this.currentNegativeBaseValues.Fill2D(double.NaN);

                this.currentMaxValue = new double[this.StackIndexMapping.Count, actualLabels.Count];
                this.currentMaxValue.Fill2D(double.NaN);
                this.currentMinValue = new double[this.StackIndexMapping.Count, actualLabels.Count];
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
        /// Updates the bar offsets.
        /// </summary>
        private void UpdateBarOffsets()
        {
            if (this.Categories.Count == 0)
            {
                this.maxWidth = double.NaN;
                this.StackedBarOffset = null;
                this.StackIndexMapping.Clear();

                return;
            }

            var totalWidthPerCategory = new double[this.Categories.Count];
            var stackGroupWidthDict = new Dictionary<string, double>();
            this.BarOffset = new double[this.Categories.Count];

            var stackGroups = this.ManagedSeries
                .OfType<IStackableSeries>()
                .Where(s => s.IsStacked)
                .GroupBy(s => s.StackGroup)
                .ToList();

            foreach (var stackGroup in stackGroups)
            {
                var groupList = stackGroup.ToList();
                var maxBarWidth = groupList
                    .Select(s => s.BarWidth)
                    .MaxOrDefault(0);

                stackGroupWidthDict.Add(stackGroup.Key, maxBarWidth);

                for (var i = 0; i < this.Categories.Count; i++)
                {
                    if (groupList.Any(s => HasCategory(s, i)))
                    {
                        totalWidthPerCategory[i] += maxBarWidth;
                    }
                }
            }

            // Add width of unstacked series
            foreach (var s in this.ManagedSeries.Where(s => !(s is IStackableSeries stackable) || !stackable.IsStacked))
            {
                for (var i = 0; i < this.Categories.Count; i++)
                {
                    var numberOfItems = s.ActualItems.Count(item => item.CategoryIndex == i);
                    if (s.ActualItems.Count > i && s.ActualItems[i].CategoryIndex < 0)
                    {
                        numberOfItems++;
                    }

                    totalWidthPerCategory[i] += s.BarWidth * numberOfItems;
                }
            }

            this.maxWidth = totalWidthPerCategory.Max();

            // Calculate BarOffset and StackedBarOffset
            this.StackedBarOffset = new double[stackGroups.Count + 1, this.Categories.Count];

            var widthScale = 1 / (1 + this.CategoryAxis.GapWidth) / this.maxWidth;
            for (var i = 0; i < this.Categories.Count; i++)
            {
                this.BarOffset[i] = 0.5 - (totalWidthPerCategory[i] * widthScale * 0.5);
            }

            for (var j = 0; j < stackGroups.Count; j++)
            {
                var stackGroup = stackGroups[j];
                var groupList = stackGroup.ToList();
                for (var i = 0; i < this.Categories.Count; i++)
                {
                    this.StackedBarOffset[j, i] = this.BarOffset[i];
                    if (groupList.Any(s => HasCategory(s, i)))
                    {
                        this.BarOffset[i] += stackGroupWidthDict[stackGroup.Key] * widthScale;
                    }
                }
            }

            for (var i = 0; i < this.Categories.Count; i++)
            {
                this.StackedBarOffset[stackGroups.Count, i] = this.BarOffset[i];
            }

            this.StackIndexMapping.Clear();
            var groupCounter = 0;
            foreach (var group in stackGroups.Select(group => group.Key).OrderBy(key => key))
            {
                this.StackIndexMapping.Add(group, groupCounter++);
            }
        }

        /// <summary>
        /// Updates the valid data of all managed series.
        /// </summary>
        private void UpdateValidData()
        {
            foreach (var item in this.ManagedSeries)
            {
                item.UpdateValidData();
            }
        }
    }
}
