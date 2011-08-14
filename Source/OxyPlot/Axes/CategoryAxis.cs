using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OxyPlot
{
    /// <summary>
    /// Category axes are using label collection indices as the coordinate.
    /// If you have 5 categories in the Labels collection, the categories will be placed at coordinates 0 to 4. The range of the axis will be from -0.5 to 4.5 (excl. padding).
    /// </summary>
    public class CategoryAxis : LinearAxis
    {
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
        /// Gets or sets the labels collection.
        /// </summary>
        public Collection<string> Labels { get; set; }

        /// <summary>
        /// Gets or sets the number of attached series.
        /// This is used to find the bar width of BarSeries
        /// </summary>
        internal int AttachedSeriesCount { get; set; }

        /// <summary>
        /// Gets or sets the base value. This is used by stacked BarSeries. 
        /// Each category will contain a BaseValue that helps the rendering to calculate the positions of the stacked bars.
        /// </summary>
        internal double[] BaseValue { get; set; }

        /// <summary>
        /// Gets or sets the base value in screen coordinate.
        /// </summary>
        internal double[] BaseValueScreen { get; set; }

        /// <summary>
        /// Gets or sets the max value (aggregated when using stacked bar series).
        /// </summary>
        internal double[] MaxValue { get; set; }

        /// <summary>
        /// Gets or sets the min value.
        /// </summary>
        /// <value>The min value.</value>
        internal double[] MinValue { get; set; }

        /// <summary>
        /// Gets or sets the current offset of the bars (not used for stacked bar series).
        /// </summary>
        internal double BarOffset { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the ticks are centered.
        /// If this is false, ticks will be drawn between each category.
        /// If this is true, ticks will be drawn in the middle of each category.
        /// </summary>
        public bool IsTickCentered { get; set; }

        public CategoryAxis()
        {
            Labels = new Collection<string>();
            TickStyle = TickStyle.Outside;
            Position = AxisPosition.Bottom;
            MinimumPadding = 0;
            MaximumPadding = 0;
            Labels = new Collection<string>();
        }

        public override void UpdateData(IEnumerable<ISeries> series)
        {
            // count series that are using this axis
            AttachedSeriesCount = 0;
            foreach (var s in series)
                if (s.IsUsing(this))
                    AttachedSeriesCount++;
            BarOffset = 0;

            if (ItemsSource == null)
            {
                BaseValue = new double[Labels.Count];
                BaseValueScreen = new double[Labels.Count];
                Fill(BaseValueScreen, double.NaN);
                MaxValue = new double[Labels.Count];
                MinValue = new double[Labels.Count];
                return;
            }

            Labels.Clear();
            ReflectionHelper.FillValues(ItemsSource, LabelField, Labels);

            if (Labels.Count > 0)
            {
                BaseValue = new double[Labels.Count];
                BaseValueScreen = new double[Labels.Count];
                Fill(BaseValueScreen, double.NaN);
            }
        }

        public static void Fill(double[] array, double value)
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = value;
        }
        
        public override string FormatValue(double x)
        {
            int index = (int)x;
            if (Labels != null && index >= 0 && index < Labels.Count)
                return Labels[index];
            return null;
        }
        
        public override object GetValue(double x)
        {
            return FormatValue(x);
        }

        public override void UpdateActualMaxMin()
        {
            base.UpdateActualMaxMin();
            ActualMinimum = -0.5;

            if (Labels != null && Labels.Count > 0)
                ActualMaximum = (Labels.Count - 1) + 0.5;
            else
                ActualMaximum = 0.5;

            if (Labels != null)
            {
                ActualMinimum -= MinimumPadding * Labels.Count;
                ActualMaximum += MaximumPadding * Labels.Count;
            }

            MajorStep = 1;
            MinorStep = 1;
        }

        public override void GetTickValues(out ICollection<double> majorLabelValues, out ICollection<double> majorTickValues, out ICollection<double> minorTickValues)
        {
            base.GetTickValues(out majorLabelValues, out majorTickValues, out minorTickValues);
            minorTickValues.Clear();

            if (!IsTickCentered)
            {
                // Subtract 0.5 from the label values to get the tick values.
                // Add one extra tick at the end.

                var mv = new List<double>();
                foreach (double v in majorLabelValues)
                    mv.Add(v - 0.5);
                mv.Add(mv[mv.Count - 1] + 1);
                majorTickValues = mv;
            }
        }
    }
}
