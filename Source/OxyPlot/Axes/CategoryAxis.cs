//-----------------------------------------------------------------------
// <copyright file="CategoryAxis.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

namespace OxyPlot
{
    using System.Collections;
    using System.Collections.Generic;

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

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the number of attached series.
        ///   This is used to find the bar width of BarSeries
        /// </summary>
        internal int AttachedSeriesCount { get; set; }

        /// <summary>
        ///   Gets or sets the current offset of the bars (not used for stacked bar series).
        /// </summary>
        internal double BarOffset { get; set; }

        /// <summary>
        ///   Gets or sets the base value. This is used by stacked BarSeries. 
        ///   Each category will contain a BaseValue that helps the rendering to calculate the positions of the stacked bars.
        /// </summary>
        internal double[] BaseValue { get; set; }

        /// <summary>
        ///   Gets or sets the base value in screen coordinate.
        /// </summary>
        internal double[] BaseValueScreen { get; set; }

        /// <summary>
        ///   Gets or sets the max value (aggregated when using stacked bar series).
        /// </summary>
        internal double[] MaxValue { get; set; }

        /// <summary>
        ///   Gets or sets the min value.
        /// </summary>
        /// <value>The min value.</value>
        internal double[] MinValue { get; set; }

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
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
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

            this.MajorStep = 1;
            this.MinorStep = 1;
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
            // count series that are using this axis
            this.AttachedSeriesCount = 0;
            foreach (var s in series)
            {
                if (s.IsUsing(this))
                {
                    this.AttachedSeriesCount++;
                }
            }

            this.BarOffset = 0;

            if (this.ItemsSource == null)
            {
                this.BaseValue = new double[this.Labels.Count];
                this.BaseValueScreen = new double[this.Labels.Count];
                Fill(this.BaseValueScreen, double.NaN);
                this.MaxValue = new double[this.Labels.Count];
                this.MinValue = new double[this.Labels.Count];
                return;
            }

            this.Labels.Clear();
            ReflectionHelper.FillValues(this.ItemsSource, this.LabelField, this.Labels);

            if (this.Labels.Count > 0)
            {
                this.BaseValue = new double[this.Labels.Count];
                this.BaseValueScreen = new double[this.Labels.Count];
                Fill(this.BaseValueScreen, double.NaN);
            }
        }

        #endregion
    }
}
