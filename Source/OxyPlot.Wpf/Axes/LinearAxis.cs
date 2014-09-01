// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinearAxis.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a WPF wrapper of OxyPlot.LinearAxis.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.LinearAxis.
    /// </summary>
    public class LinearAxis : Axis
    {
        /// <summary>
        /// Identifies the <see cref="FormatAsFractions"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FormatAsFractionsProperty =
            DependencyProperty.Register(
                "FormatAsFractions", typeof(bool), typeof(LinearAxis), new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="FractionUnit"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FractionUnitProperty = DependencyProperty.Register(
            "FractionUnit", typeof(double), typeof(LinearAxis), new PropertyMetadata(1.0));

        /// <summary>
        /// Identifies the <see cref="FractionUnitSymbol"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FractionUnitSymbolProperty =
            DependencyProperty.Register(
                "FractionUnitSymbol", typeof(string), typeof(LinearAxis), new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref = "LinearAxis" /> class.
        /// </summary>
        public LinearAxis()
        {
            this.InternalAxis = new Axes.LinearAxis();
        }

        /// <summary>
        /// Gets or sets a value indicating whether FormatAsFractions.
        /// </summary>
        public bool FormatAsFractions
        {
            get
            {
                return (bool)this.GetValue(FormatAsFractionsProperty);
            }

            set
            {
                this.SetValue(FormatAsFractionsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets FractionUnit.
        /// </summary>
        public double FractionUnit
        {
            get
            {
                return (double)this.GetValue(FractionUnitProperty);
            }

            set
            {
                this.SetValue(FractionUnitProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets FractionUnitSymbol.
        /// </summary>
        public string FractionUnitSymbol
        {
            get
            {
                return (string)this.GetValue(FractionUnitSymbolProperty);
            }

            set
            {
                this.SetValue(FractionUnitSymbolProperty, value);
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
            var a = (Axes.LinearAxis)this.InternalAxis;
            a.FormatAsFractions = this.FormatAsFractions;
            a.FractionUnit = this.FractionUnit;
            a.FractionUnitSymbol = this.FractionUnitSymbol;
        }
    }
}