// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinearAxis.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a Avalonia wrapper of OxyPlot.LinearAxis.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    /// <summary>
    /// This is a Avalonia wrapper of OxyPlot.LinearAxis.
    /// </summary>
    public class LinearAxis : Axis
    {
        /// <summary>
        /// Identifies the <see cref="FormatAsFractions"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<bool> FormatAsFractionsProperty = AvaloniaProperty.Register<LinearAxis, bool>(nameof(FormatAsFractions), false);

        /// <summary>
        /// Identifies the <see cref="FractionUnit"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> FractionUnitProperty = AvaloniaProperty.Register<LinearAxis, double>(nameof(FractionUnit), 1.0);

        /// <summary>
        /// Identifies the <see cref="FractionUnitSymbol"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> FractionUnitSymbolProperty = AvaloniaProperty.Register<LinearAxis, string>(nameof(FractionUnitSymbol));

        /// <summary>
        /// Initializes a new instance of the <see cref = "LinearAxis" /> class.
        /// </summary>
        public LinearAxis()
        {
            InternalAxis = new Axes.LinearAxis();
        }

        /// <summary>
        /// Gets or sets a value indicating whether FormatAsFractions.
        /// </summary>
        public bool FormatAsFractions
        {
            get
            {
                return GetValue(FormatAsFractionsProperty);
            }

            set
            {
                SetValue(FormatAsFractionsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets FractionUnit.
        /// </summary>
        public double FractionUnit
        {
            get
            {
                return GetValue(FractionUnitProperty);
            }

            set
            {
                SetValue(FractionUnitProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets FractionUnitSymbol.
        /// </summary>
        public string FractionUnitSymbol
        {
            get
            {
                return GetValue(FractionUnitSymbolProperty);
            }

            set
            {
                SetValue(FractionUnitSymbolProperty, value);
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
            var a = (Axes.LinearAxis)InternalAxis;
            a.FormatAsFractions = FormatAsFractions;
            a.FractionUnit = FractionUnit;
            a.FractionUnitSymbol = FractionUnitSymbol;
        }
    }
}