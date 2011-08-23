// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Series.cs" company="OxyPlot">
//   see http://oxyplot.codeplex.com
// </copyright>
// <summary>
//   Abstract base class for series that use X and Y axes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Collections;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Abstract base class for series.
    /// </summary>
    public abstract class Series : ItemsControl, ISeries
    {
        #region Constants and Fields

        /// <summary>
        /// The color property.
        /// </summary>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof(Color?), typeof(Series), new FrameworkPropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// The title property.
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(Series), new FrameworkPropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// The tracker format string property.
        /// </summary>
        public static readonly DependencyProperty TrackerFormatStringProperty =
            DependencyProperty.Register(
                "TrackerFormatString", 
                typeof(string), 
                typeof(XYAxisSeries), 
                new FrameworkPropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// The tracker key property.
        /// </summary>
        public static readonly DependencyProperty TrackerKeyProperty = DependencyProperty.Register(
            "TrackerKey", typeof(string), typeof(Series), new FrameworkPropertyMetadata(null, AppearanceChanged));

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Color.
        /// </summary>
        public Color? Color
        {
            get
            {
                return (Color?)this.GetValue(ColorProperty);
            }

            set
            {
                this.SetValue(ColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets Title.
        /// </summary>
        public string Title
        {
            get
            {
                return (string)this.GetValue(TitleProperty);
            }

            set
            {
                this.SetValue(TitleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets TrackerFormatString.
        /// </summary>
        public string TrackerFormatString
        {
            get
            {
                return (string)this.GetValue(TrackerFormatStringProperty);
            }

            set
            {
                this.SetValue(TrackerFormatStringProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets TrackerKey.
        /// </summary>
        public string TrackerKey
        {
            get
            {
                return (string)this.GetValue(TrackerKeyProperty);
            }

            set
            {
                this.SetValue(TrackerKeyProperty, value);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The create model.
        /// </summary>
        /// <returns>
        /// </returns>
        public abstract OxyPlot.Series CreateModel();

        #endregion

        #region Methods

        /// <summary>
        /// The appearance changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected static void AppearanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((XYAxisSeries)d).OnVisualChanged();
        }

        /// <summary>
        /// The data changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected static void DataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((XYAxisSeries)d).OnDataChanged();
        }

        /// <summary>
        /// The on data changed.
        /// </summary>
        protected void OnDataChanged()
        {
            var pc = this.Parent as IPlotControl;
            if (pc != null)
            {
                // pc.UpdateModel();
            }

            this.OnVisualChanged();
        }

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
        /// The on visual changed.
        /// </summary>
        protected void OnVisualChanged()
        {
            var pc = this.Parent as IPlotControl;
            if (pc != null)
            {
                pc.InvalidatePlot();
            }
        }

        /// <summary>
        /// The synchronize properties.
        /// </summary>
        /// <param name="series">
        /// The series.
        /// </param>
        protected virtual void SynchronizeProperties(OxyPlot.ISeries series)
        {
            var s = series as OxyPlot.Series;
            s.Background = this.Background.ToOxyColor();
            s.Title = this.Title;
            s.TrackerFormatString = this.TrackerFormatString;
            s.TrackerKey = this.TrackerKey;
            s.TrackerFormatString = this.TrackerFormatString;
        }

        #endregion
    }
}