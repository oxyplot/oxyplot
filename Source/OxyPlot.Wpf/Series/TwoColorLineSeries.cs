// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TwoColorLineSeries.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// The WPF wrapper for OxyPlot.TwoColorLineSeries.
    /// </summary>
    public class TwoColorLineSeries : LineSeries
    {
        #region Constants and Fields

        /// <summary>
        ///   The color 2 property.
        /// </summary>
        public static readonly DependencyProperty Color2Property = DependencyProperty.Register(
            "Color2", typeof(Color), typeof(TwoColorLineSeries), new UIPropertyMetadata(Colors.Blue, AppearanceChanged));

        /// <summary>
        ///   The limit property.
        /// </summary>
        public static readonly DependencyProperty LimitProperty = DependencyProperty.Register(
            "Limit", typeof(double), typeof(TwoColorLineSeries), new UIPropertyMetadata(0.0, AppearanceChanged));

        /// <summary>
        ///   The line style 2 property.
        /// </summary>
        public static readonly DependencyProperty LineStyle2Property = DependencyProperty.Register(
            "LineStyle2", 
            typeof(LineStyle), 
            typeof(TwoColorLineSeries), 
            new UIPropertyMetadata(LineStyle.Solid, AppearanceChanged));

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "TwoColorLineSeries" /> class.
        /// </summary>
        public TwoColorLineSeries()
        {
            this.InternalSeries = new OxyPlot.TwoColorLineSeries();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets Color2.
        /// </summary>
        public Color Color2
        {
            get
            {
                return (Color)this.GetValue(Color2Property);
            }

            set
            {
                this.SetValue(Color2Property, value);
            }
        }

        /// <summary>
        ///   Gets or sets Limit.
        /// </summary>
        public double Limit
        {
            get
            {
                return (double)this.GetValue(LimitProperty);
            }

            set
            {
                this.SetValue(LimitProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets LineStyle2.
        /// </summary>
        public LineStyle LineStyle2
        {
            get
            {
                return (LineStyle)this.GetValue(LineStyle2Property);
            }

            set
            {
                this.SetValue(LineStyle2Property, value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The synchronize properties.
        /// </summary>
        /// <param name="series">
        /// The series.
        /// </param>
        protected override void SynchronizeProperties(OxyPlot.Series series)
        {
            base.SynchronizeProperties(series);
            var s = series as OxyPlot.TwoColorLineSeries;
            s.Limit = this.Limit;
            s.Color2 = this.Color2.ToOxyColor();
        }

        #endregion
    }
}