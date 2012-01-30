// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataPointSeries.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Collections;
    using System.Windows;

    /// <summary>
    /// Base class for data series
    /// </summary>
    public abstract class DataPointSeries : ItemsSeries
    {
        #region Constants and Fields

        /// <summary>
        /// The can tracker interpolate points property.
        /// </summary>
        public static readonly DependencyProperty CanTrackerInterpolatePointsProperty =
            DependencyProperty.Register(
                "CanTrackerInterpolatePoints", 
                typeof(bool), 
                typeof(DataPointSeries), 
                new PropertyMetadata(false, AppearanceChanged));

        /// <summary>
        ///   The data field x property.
        /// </summary>
        public static readonly DependencyProperty DataFieldXProperty = DependencyProperty.Register(
            "DataFieldX", typeof(string), typeof(DataPointSeries), new PropertyMetadata("X", DataChanged));

        /// <summary>
        ///   The data field y property.
        /// </summary>
        public static readonly DependencyProperty DataFieldYProperty = DependencyProperty.Register(
            "DataFieldY", typeof(string), typeof(DataPointSeries), new PropertyMetadata("Y", DataChanged));

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets a value indicating whether the tracker can interpolate points.
        /// </summary>
        public bool CanTrackerInterpolatePoints
        {
            get
            {
                return (bool)this.GetValue(CanTrackerInterpolatePointsProperty);
            }

            set
            {
                this.SetValue(CanTrackerInterpolatePointsProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets DataFieldX.
        /// </summary>
        public string DataFieldX
        {
            get
            {
                return (string)this.GetValue(DataFieldXProperty);
            }

            set
            {
                this.SetValue(DataFieldXProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets DataFieldY.
        /// </summary>
        public string DataFieldY
        {
            get
            {
                return (string)this.GetValue(DataFieldYProperty);
            }

            set
            {
                this.SetValue(DataFieldYProperty, value);
            }
        }

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
        protected override void SynchronizeProperties(OxyPlot.ISeries series)
        {
            base.SynchronizeProperties(series);
            var s = series as OxyPlot.DataPointSeries;
            s.ItemsSource = this.ItemsSource;
            s.DataFieldX = this.DataFieldX;
            s.DataFieldY = this.DataFieldY;
            s.CanTrackerInterpolatePoints = this.CanTrackerInterpolatePoints;
        }

        #endregion
    }
}