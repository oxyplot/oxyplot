// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogarithmicAxis.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a Avalonia wrapper of OxyPlot.LogarithmicAxis.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    /// <summary>
    /// This is a Avalonia wrapper of OxyPlot.LogarithmicAxis.
    /// </summary>
    public class LogarithmicAxis : Axis
    {
        /// <summary>
        /// Identifies the <see cref="Base"/> dependency property.
        /// </summary>
        /// <value>The logarithmic base.</value>
        public static readonly StyledProperty<double> BaseProperty = AvaloniaProperty.Register<LogarithmicAxis, double>(nameof(Base), 10.0);

        /// <summary>
        /// Identifies the <see cref="PowerPadding"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<bool> PowerPaddingProperty = AvaloniaProperty.Register<LogarithmicAxis, bool>(nameof(PowerPadding), true);

        /// <summary>
        /// Initializes a new instance of the <see cref = "LogarithmicAxis" /> class.
        /// </summary>
        public LogarithmicAxis()
        {
            InternalAxis = new Axes.LogarithmicAxis();
            FilterMinValue = 0;
        }

        /// <summary>
        /// Gets or sets Base.
        /// </summary>
        public double Base
        {
            get
            {
                return GetValue(BaseProperty);
            }

            set
            {
                SetValue(BaseProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the ActualMaximum and ActualMinimum values should be padded to the nearest power of the Base.
        /// </summary>
        public bool PowerPadding
        {
            get
            {
                return GetValue(PowerPaddingProperty);
            }

            set
            {
                SetValue(PowerPaddingProperty, value);
            }
        }

        /// <summary>
        /// Creates the internal axis.
        /// </summary>
        /// <returns>The internal axis.</returns>
        public override Axes.Axis CreateModel()
        {
            SynchronizeProperties();
            return InternalAxis;
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        protected override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            var a = (Axes.LogarithmicAxis)InternalAxis;
            a.Base = Base;
            a.PowerPadding = PowerPadding;
        }

        static LogarithmicAxis()
        {
            BaseProperty.Changed.AddClassHandler<LogarithmicAxis>(DataChanged);
            PowerPaddingProperty.Changed.AddClassHandler<LogarithmicAxis>(DataChanged);
        }
    }
}