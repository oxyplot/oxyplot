// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogarithmicAxis.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a WPF wrapper of OxyPlot.LogarithmicAxis.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.LogarithmicAxis.
    /// </summary>
    public class LogarithmicAxis : Axis
    {
        /// <summary>
        /// Identifies the <see cref="Base"/> dependency property.
        /// </summary>
        /// <value>The logarithmic base.</value>
        public static readonly DependencyProperty BaseProperty = DependencyProperty.Register(
            "Base", typeof(double), typeof(LogarithmicAxis), new PropertyMetadata(10.0, DataChanged));

        /// <summary>
        /// Identifies the <see cref="PowerPadding"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PowerPaddingProperty = DependencyProperty.Register(
            "PowerPadding", typeof(bool), typeof(LogarithmicAxis), new PropertyMetadata(true, DataChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref = "LogarithmicAxis" /> class.
        /// </summary>
        public LogarithmicAxis()
        {
            this.InternalAxis = new Axes.LogarithmicAxis();
            this.FilterMinValue = 0;
        }

        /// <summary>
        /// Gets or sets Base.
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

        /// <summary>
        /// Creates the internal axis.
        /// </summary>
        /// <returns>The internal axis.</returns>
        public override Axes.Axis CreateModel()
        {
            this.SynchronizeProperties();
            return this.InternalAxis;
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        protected override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            var a = (Axes.LogarithmicAxis)this.InternalAxis;
            a.Base = this.Base;
            a.PowerPadding = this.PowerPadding;
        }
    }
}