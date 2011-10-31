// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogarithmicAxis.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.LogarithmicAxis.
    /// </summary>
    public class LogarithmicAxis : Axis
    {
        #region Constants and Fields

        /// <summary>
        ///   Gets or sets the logarithmic base (normally 10).
        ///   http://en.wikipedia.org/wiki/Logarithm
        /// </summary>
        /// <value>The logarithmic base.</value>
        public static readonly DependencyProperty BaseProperty = DependencyProperty.Register(
            "Base", typeof(double), typeof(LogarithmicAxis), new PropertyMetadata(10.0, DataChanged));

        /// <summary>
        ///   The power padding property.
        /// </summary>
        public static readonly DependencyProperty PowerPaddingProperty = DependencyProperty.Register(
            "PowerPadding", typeof(bool), typeof(LogarithmicAxis), new PropertyMetadata(true, DataChanged));

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "LogarithmicAxis" /> class.
        /// </summary>
        public LogarithmicAxis()
        {
            this.internalAxis = new OxyPlot.LogarithmicAxis();
            this.FilterMinValue = 0;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets Base.
        /// </summary>
        public double Base
        {
            get
            {
                return (double)this.GetValue(BaseProperty);
            }

            set
            {
                this.SetValue(BaseProperty, value);
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether the ActualMaximum and ActualMinimum values should be padded to the nearest power of the Base.
        /// </summary>
        public bool PowerPadding
        {
            get
            {
                return (bool)this.GetValue(PowerPaddingProperty);
            }

            set
            {
                this.SetValue(PowerPaddingProperty, value);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The create model.
        /// </summary>
        /// <returns>
        /// </returns>
        public override OxyPlot.Axis CreateModel()
        {
            this.SynchronizeProperties();
            return this.internalAxis;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The synchronize properties.
        /// </summary>
        protected override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            var a = this.internalAxis as OxyPlot.LogarithmicAxis;
            a.Base = this.Base;
            a.PowerPadding = this.PowerPadding;
        }

        #endregion
    }
}