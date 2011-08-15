namespace OxyPlot.Wpf
{
    using System.Windows;

    public class LogarithmicAxis : AxisBase
    {
        #region Constants and Fields

        /// <summary>
        /// Gets or sets the logarithmic base (normally 10).
        /// http://en.wikipedia.org/wiki/Logarithm
        /// </summary>
        /// <value>The logarithmic base.</value>
        public static readonly DependencyProperty BaseProperty = DependencyProperty.Register(
            "Base", typeof(double), typeof(LogarithmicAxis), new FrameworkPropertyMetadata(10.0, DataChanged));

        public static readonly DependencyProperty PowerPaddingProperty = DependencyProperty.Register(
            "PowerPadding", typeof(bool), typeof(LogarithmicAxis), new UIPropertyMetadata(false, DataChanged));

        #endregion

        #region Constructors and Destructors

        public LogarithmicAxis()
        {
            this.Axis = new OxyPlot.LogarithmicAxis();
        }

        #endregion

        #region Public Properties

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
        /// Gets or sets a value indicating whether the ActualMaximum and ActualMinimum values should be padded to the nearest power of the Base.
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

        public override OxyPlot.IAxis CreateModel()
        {
            this.SynchronizeProperties();
            return this.Axis;
        }

        public override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            var a = this.Axis as OxyPlot.LogarithmicAxis;
            a.Base = this.Base;
            a.PowerPadding = this.PowerPadding;
        }

        #endregion
    }
}