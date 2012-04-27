// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryAxis.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Represents a category axes.
    /// </summary>
    /// <remarks>
    /// The category axis is using the label collection indices as the coordinate.
    ///   If you have 5 categories in the Labels collection, the categories will be placed at coordinates 0 to 4. The range of the axis will be from -0.5 to 4.5 (excl. padding).
    /// </remarks>
    public class CategoryAxis : LinearAxis
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "CategoryAxis" /> class.
        /// </summary>
        public CategoryAxis()
        {
            this.Labels = new List<string>();
            this.TickStyle = TickStyle.Outside;
            this.Position = AxisPosition.Bottom;
            this.MinimumPadding = 0;
            this.MaximumPadding = 0;
            this.MajorStep = 1;
            this.CategoryWidth = 0.5;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryAxis"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="categories">The categories.</param>
        public CategoryAxis(string title, params string[] categories)
            : this()
        {
            this.Title = title;
            if (categories != null)
            {
                foreach (var c in categories)
                {
                    this.Labels.Add(c);
                }
            }
        }
        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets a value indicating whether the ticks are centered.
        ///   If this is false, ticks will be drawn between each category.
        ///   If this is true, ticks will be drawn in the middle of each category.
        /// </summary>
        public bool IsTickCentered { get; set; }

        /// <summary>
        ///   Gets or sets the items source (used to update the Labels collection).
        /// </summary>
        /// <value>The items source.</value>
        public IEnumerable ItemsSource { get; set; }

        /// <summary>
        ///   Gets or sets the data field for the labels.
        /// </summary>
        public string LabelField { get; set; }

        /// <summary>
        ///   Gets or sets the labels collection.
        /// </summary>
        public IList<string> Labels { get; set; }

        /// <summary>
        /// Gets or sets the Category width as a fraction of the width of a category.
        /// The default value is 0.5 (50%)
        /// </summary>
        public double CategoryWidth { get; set; }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets sum of the widths of the single bars per label.
        ///   This is used to find the bar width of BarSeries
        /// </summary>
        internal double[] TotalWidth { get; set; }

        /// <summary>
        /// Gets or sets the maximal width of all labels
        /// </summary>
        internal double MaxWidth { get; set; }

        /// <summary>
        ///   Gets or sets the current offset of the bars (not used for stacked bar series).
        /// </summary>
        internal double[] BarOffset { get; set; }

        /// <summary>
        ///   Gets or sets the offset of the bars  per StackIndex and Label (only used for stacked bar series).
        /// </summary>
        internal double[,] StackedBarOffset { get; set; }

        /// <summary>
        /// Gets or sets per StackIndex and Label the base value for positive values of stacked bar series. 
        /// </summary>
        internal double[,] PositiveBaseValues { get; set; }

        /// <summary>
        /// Gets or sets per StackIndex and Label the base value for negative values of stacked bar series. 
        /// </summary>
        internal double[,] NegativeBaseValues { get; set; }

        /// <summary>
        /// Gets or sets the StackIndexMapping. The mapping indicates to which rank a specific stack index belongs.
        /// </summary>
        internal Dictionary<int, int> StackIndexMapping { get; set; }

        /// <summary>
        ///   Gets or sets the max value per StackIndex and Label  (only used for stacked bar series).
        /// </summary>
        internal double[,] MaxValue { get; set; }

        /// <summary>
        ///   Gets or sets the min value per StackIndex and Label  (only used for stacked bar series).
        /// </summary>
        internal double[,] MinValue { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Fills the specified array.
        /// </summary>
        /// <param name="array">
        /// The array.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public static void Fill(double[] array, double value)
        {
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }

        /// <summary>
        /// Fills the specified array.
        /// </summary>
        /// <param name="array">
        /// The array.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public static void Fill(double[,] array, double value)
        {
            for (var i = 0; i < array.GetLength(0); i++)
            {
                for (var j = 0; j < array.GetLength(1); j++)
                {
                    array[i, j] = value;
                }
            }
        }

        /// <summary>
        /// Formats the value to be used on the axis.
        /// </summary>
        /// <param name="x">
        /// The value.
        /// </param>
        /// <returns>
        /// The formatted value.
        /// </returns>
        public override string FormatValue(double x)
        {
            var index = (int)x;
            if (this.Labels != null && index >= 0 && index < this.Labels.Count)
            {
                return this.Labels[index];
            }

            return null;
        }

        /// <summary>
        /// Formats the value to be used by the tracker.
        /// </summary>
        /// <param name="x">
        /// The value.
        /// </param>
        /// <returns>
        /// The formatted value.
        /// </returns>
        public override string FormatValueForTracker(double x)
        {
            return this.FormatValue(x);
        }

        /// <summary>
        /// Gets the coordinates used to draw ticks and tick labels (numbers or category names).
        /// </summary>
        /// <param name="majorLabelValues">
        /// The major label values.
        /// </param>
        /// <param name="majorTickValues">
        /// The major tick values.
        /// </param>
        /// <param name="minorTickValues">
        /// The minor tick values.
        /// </param>
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
                foreach (double v in majorLabelValues)
                {
                    mv.Add(v - 0.5);
                }

                mv.Add(mv[mv.Count - 1] + 1);
                majorTickValues = mv;
            }
        }

        /// <summary>
        /// Gets the value from an axis coordinate, converts from double to the correct data type if neccessary.
        ///   e.g. DateTimeAxis returns the DateTime and CategoryAxis returns category strings.
        /// </summary>
        /// <param name="x">
        /// The coordinate.
        /// </param>
        /// <returns>
        /// The value.
        /// </returns>
        public override object GetValue(double x)
        {
            return this.FormatValue(x);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the actual maximum and minimum values.
        ///   If the user has zoomed/panned the axis, the internal ViewMaximum/ViewMinimum values will be used.
        ///   If Maximum or Minimum have been set, these values will be used.
        ///   Otherwise the maximum and minimum values of the series will be used, including the 'padding'.
        /// </summary>
        internal override void UpdateActualMaxMin()
        {
            base.UpdateActualMaxMin();
            this.ActualMinimum = -0.5;

            if (this.Labels != null && this.Labels.Count > 0)
            {
                this.ActualMaximum = (this.Labels.Count - 1) + 0.5;
            }
            else
            {
                this.ActualMaximum = 0.5;
            }

            if (this.Labels != null)
            {
                this.ActualMinimum -= this.MinimumPadding * this.Labels.Count;
                this.ActualMaximum += this.MaximumPadding * this.Labels.Count;
            }

            this.MinorStep = 1;
        }

        /// <summary>
        /// Creates Labels list if no labels were set
        /// </summary>
        /// <param name="series">The list of series which are rendered</param>
        internal void UpdateLabels(IEnumerable<Series> series)
        {
            if (this.ItemsSource != null)
            {
                this.Labels.Clear();
                ReflectionHelper.FillList(this.ItemsSource, this.LabelField, this.Labels);
            }

            if (this.Labels.Count == 0)
            {
                foreach (var s in series)
                {
                    var labels = new List<string>();
                    if (!s.IsUsing(this))
                    {
                        continue;
                    }

                    var bsb = s as BarSeriesBase;
                    if (bsb != null)
                    {
                        labels.AddRange(bsb.Items.Select(item => item.Label));
                    }

                    var tbs = s as TornadoBarSeries;
                    if (tbs != null)
                    {
                        labels.AddRange(tbs.Items.Select(item => item.Label));
                    }

                    var ibs = s as IntervalBarSeries;
                    if (ibs != null)
                    {
                        labels.AddRange(ibs.Items.Select(item => item.Label));
                    }

                    labels = labels.Distinct().ToList();
                    labels.Sort();
                    this.Labels = labels;
                }
            }
        }

        /// <summary>
        /// Updates the axis with information from the plot series.
        ///   This is used by the category axis that need to know the number of series using the axis.
        /// </summary>
        /// <param name="series">
        /// The series collection.
        /// </param>
        internal override void UpdateFromSeries(IEnumerable<Series> series)
        {
            this.TotalWidth = new double[this.Labels.Count];
            Fill(this.TotalWidth, 0);

            var usedSeries = series.Where(s => s.IsUsing(this)).ToList();

            // Add Width of Stacked bars to TotalWidth
            var stackedBarSeries = usedSeries.OfType<BarSeriesBase>().Where(s => s.IsStacked).ToList();
            var stackIndices = stackedBarSeries.Select(s => s.StackIndex).Distinct().ToList();
            var stackRankBarWidth = new Dictionary<int, double>();
            for (var j = 0; j < stackIndices.Count; j++)
            {
                var maxBarWidth = stackedBarSeries.Where(s => s.StackIndex == stackIndices[j]).Select(s => s.BarWidth).Concat(new[] { 0.0 }).Max();
                for (var i = 0; i < this.Labels.Count; i++)
                {
                    if (stackedBarSeries.SelectMany(s => s.ValidItems).Any(item => item.Label == this.Labels[i]))
                    {
                        this.TotalWidth[i] += maxBarWidth;
                    }
                }

                stackRankBarWidth[j] = maxBarWidth;
            }

            // Add Width of unstacked bars to TotalWidth
            var unstackedBarSeries = usedSeries.OfType<BarSeriesBase>().Where(s => !s.IsStacked).ToList();
            foreach (var s in unstackedBarSeries)
            {
                for (var i = 0; i < this.Labels.Count; i++)
                {
                    var numberOfItems = s.ValidItems.Count(item => item.Label == this.Labels[i]);
                    this.TotalWidth[i] += s.BarWidth * numberOfItems;
                }
            }

            // Add Width of tornado bars to TotalWidth
            var tbs = usedSeries.OfType<TornadoBarSeries>().ToList();
            for (var i = 0; i < this.Labels.Count; i++)
            {
                var tbsOfLabel = tbs.Where(s => s.ValidItems.Any(item => item.Label == this.Labels[i])).ToList();
                if (tbsOfLabel.Count == 0)
                {
                    continue;
                }

                var maxBarWidth = tbsOfLabel.Select(s => s.BarWidth).Max();
                this.TotalWidth[i] += maxBarWidth;
            }

            // Add Width of interval bars to TotalWidth
            var ibs = usedSeries.OfType<IntervalBarSeries>().ToList();
            for (var i = 0; i < this.Labels.Count; i++)
            {
                var ibsOfLabel = ibs.Where(s => s.ValidItems.Any(item => item.Label == this.Labels[i])).ToList();
                if (ibsOfLabel.Count == 0)
                {
                    continue;
                }

                var maxBarWidth = ibsOfLabel.Select(s => s.BarWidth).Max();
                this.TotalWidth[i] += maxBarWidth;
            }

            this.MaxWidth = this.TotalWidth.Max();

            // Calculate BarOffset and StackedBarOffset
            this.BarOffset = new double[this.Labels.Count];
            Fill(this.BarOffset, 0);
            this.StackedBarOffset = new double[stackIndices.Count + 1, this.Labels.Count];
            Fill(this.StackedBarOffset, 0);

            var factor = 0.5 * this.CategoryWidth / this.MaxWidth;
            for (var i = 0; i < this.Labels.Count; i++)
            {
                this.BarOffset[i] = 0.5 - (this.TotalWidth[i] * factor);
            }

            for (var j = 0; j <= stackIndices.Count; j++)
            {
                for (var i = 0; i < this.Labels.Count; i++)
                {
                    if (stackedBarSeries.SelectMany(s => s.ValidItems).All(item => item.Label != this.Labels[i]))
                    {
                        continue;
                    }

                    this.StackedBarOffset[j, i] = this.BarOffset[i];
                    if (j < stackIndices.Count)
                    {
                        this.BarOffset[i] += stackRankBarWidth[j] * this.CategoryWidth / this.MaxWidth;
                    }
                }
            }

            stackIndices.Sort();
            this.StackIndexMapping = new Dictionary<int, int>();
            for (var i = 0; i < stackIndices.Count; i++)
            {
                this.StackIndexMapping.Add(stackIndices[i], i);
            }

            this.PositiveBaseValues = new double[stackIndices.Count, this.Labels.Count];
            Fill(this.PositiveBaseValues, double.NaN);
            this.NegativeBaseValues = new double[stackIndices.Count, this.Labels.Count];
            Fill(this.NegativeBaseValues, double.NaN);

            this.MaxValue = new double[stackIndices.Count, this.Labels.Count];
            Fill(this.MaxValue, double.NaN);
            this.MinValue = new double[stackIndices.Count, this.Labels.Count];
            Fill(this.MinValue, double.NaN);
        }

        #endregion
    }
}