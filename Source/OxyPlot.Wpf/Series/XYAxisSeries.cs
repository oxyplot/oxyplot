// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XYAxisSeries.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Collections;
    using System.Windows;

    /// <summary>
    /// Abstract base class for series that use X and Y axes.
    /// </summary>
    public abstract class XYAxisSeries : Series
    {
        #region Constants and Fields

        /// <summary>
        ///   The x axis key property.
        /// </summary>
        public static readonly DependencyProperty XAxisKeyProperty = DependencyProperty.Register(
            "XAxisKey", typeof(string), typeof(XYAxisSeries), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        ///   The y axis key property.
        /// </summary>
        public static readonly DependencyProperty YAxisKeyProperty = DependencyProperty.Register(
            "YAxisKey", typeof(string), typeof(XYAxisSeries), new PropertyMetadata(null, AppearanceChanged));

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the x-axis key.
        /// </summary>
        public string XAxisKey
        {
            get
            {
                return (string)this.GetValue(XAxisKeyProperty);
            }

            set
            {
                this.SetValue(XAxisKeyProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the y axis key.
        /// </summary>
        public string YAxisKey
        {
            get
            {
                return (string)this.GetValue(YAxisKeyProperty);
            }

            set
            {
                this.SetValue(YAxisKeyProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets the internal series.
        /// </summary>
        protected OxyPlot.Series InternalSeries { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The on items source changed.
        /// </summary>
        /// <param name="oldValue">
        /// The old value.
        /// </param>
        /// <param name="newValue">
        /// The new value.
        /// </param>
        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            this.OnDataChanged();
        }

        /// <summary>
        /// The synchronize properties.
        /// </summary>
        /// <param name="series">
        /// The series.
        /// </param>
        protected override void SynchronizeProperties(OxyPlot.Series series)
        {
            base.SynchronizeProperties(series);
            var s = (OxyPlot.XYAxisSeries)series;
            s.XAxisKey = this.XAxisKey;
            s.YAxisKey = this.YAxisKey;
        }

        #endregion
    }
}