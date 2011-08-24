// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarSeries.cs" company="OxyPlot">
//   see http://oxyplot.codeplex.com
// </copyright>
// <summary>
//   This is a WPF wrapper of OxyPlot.BarSeries
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.BarSeries
    /// </summary>
    public class BarSeries : ItemsSeries
    {
        #region Constants and Fields

        /// <summary>
        /// The bar width property.
        /// </summary>
        public static readonly DependencyProperty BarWidthProperty = DependencyProperty.Register(
            "BarWidth", typeof(double), typeof(BarSeries), new PropertyMetadata(0.5, AppearanceChanged));

        /// <summary>
        /// The fill color property.
        /// </summary>
        public static readonly DependencyProperty FillColorProperty = DependencyProperty.Register(
            "FillColor", typeof(Color?), typeof(BarSeries), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// The is stacked property.
        /// </summary>
        public static readonly DependencyProperty IsStackedProperty = DependencyProperty.Register(
            "IsStacked", typeof(bool), typeof(BarSeries), new PropertyMetadata(false, AppearanceChanged));

        /// <summary>
        /// The negative fill color property.
        /// </summary>
        public static readonly DependencyProperty NegativeFillColorProperty =
            DependencyProperty.Register(
                "NegativeFillColor", 
                typeof(Color?), 
                typeof(BarSeries), 
                new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// The stroke color property.
        /// </summary>
        public static readonly DependencyProperty StrokeColorProperty = DependencyProperty.Register(
            "StrokeColor", 
            typeof(Color), 
            typeof(BarSeries), 
            new PropertyMetadata(Colors.Black, AppearanceChanged));

        /// <summary>
        /// The stroke thickness property.
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(
                "StrokeThickness", 
                typeof(double), 
                typeof(BarSeries), 
                new PropertyMetadata(0.0, AppearanceChanged));

        /// <summary>
        /// The value field property.
        /// </summary>
        public static readonly DependencyProperty ValueFieldProperty = DependencyProperty.Register(
            "ValueField", typeof(string), typeof(BarSeries), new PropertyMetadata(null, AppearanceChanged));

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="BarSeries"/> class.
        /// </summary>
        static BarSeries()
        {
            TrackerFormatStringProperty.OverrideMetadata(
                typeof(CategoryAxis), new PropertyMetadata("{0} {1}: {2}", DataChanged));
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets BarWidth.
        /// </summary>
        public double BarWidth
        {
            get
            {
                return (double)this.GetValue(BarWidthProperty);
            }

            set
            {
                this.SetValue(BarWidthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets FillColor.
        /// </summary>
        public Color? FillColor
        {
            get
            {
                return (Color?)this.GetValue(FillColorProperty);
            }

            set
            {
                this.SetValue(FillColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether IsStacked.
        /// </summary>
        public bool IsStacked
        {
            get
            {
                return (bool)this.GetValue(IsStackedProperty);
            }

            set
            {
                this.SetValue(IsStackedProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets NegativeFillColor.
        /// </summary>
        public Color? NegativeFillColor
        {
            get
            {
                return (Color?)this.GetValue(NegativeFillColorProperty);
            }

            set
            {
                this.SetValue(NegativeFillColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets StrokeColor.
        /// </summary>
        public Color StrokeColor
        {
            get
            {
                return (Color)this.GetValue(StrokeColorProperty);
            }

            set
            {
                this.SetValue(StrokeColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets StrokeThickness.
        /// </summary>
        public double StrokeThickness
        {
            get
            {
                return (double)this.GetValue(StrokeThicknessProperty);
            }

            set
            {
                this.SetValue(StrokeThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets ValueField.
        /// </summary>
        public string ValueField
        {
            get
            {
                return (string)this.GetValue(ValueFieldProperty);
            }

            set
            {
                this.SetValue(ValueFieldProperty, value);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The create model.
        /// </summary>
        /// <returns>
        /// </returns>
        public override OxyPlot.Series CreateModel()
        {
            var s = new OxyPlot.BarSeries();
            this.SynchronizeProperties(s);
            return s;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The synchronize properties.
        /// </summary>
        /// <param name="series">
        /// The series.
        /// </param>
        protected override void SynchronizeProperties(OxyPlot.ISeries series)
        {
            base.SynchronizeProperties(series);
            var s = series as OxyPlot.BarSeries;
            if (s != null)
            {
                s.BarWidth = this.BarWidth;
                s.FillColor = this.FillColor.ToOxyColor();
                s.IsStacked = this.IsStacked;
                s.NegativeFillColor = this.NegativeFillColor.ToOxyColor();
                s.StrokeColor = this.StrokeColor.ToOxyColor();
                s.StrokeThickness = this.StrokeThickness;
                s.ValueField = this.ValueField;
            }
        }

        #endregion
    }
}