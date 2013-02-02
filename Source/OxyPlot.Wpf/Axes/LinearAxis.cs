// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinearAxis.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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
        /// The format as fractions property.
        /// </summary>
        public static readonly DependencyProperty FormatAsFractionsProperty =
            DependencyProperty.Register(
                "FormatAsFractions", typeof(bool), typeof(LinearAxis), new PropertyMetadata(false));

        /// <summary>
        /// The fraction unit property.
        /// </summary>
        public static readonly DependencyProperty FractionUnitProperty = DependencyProperty.Register(
            "FractionUnit", typeof(double), typeof(LinearAxis), new PropertyMetadata(1.0));

        /// <summary>
        /// The fraction unit symbol property.
        /// </summary>
        public static readonly DependencyProperty FractionUnitSymbolProperty =
            DependencyProperty.Register(
                "FractionUnitSymbol", typeof(string), typeof(LinearAxis), new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref = "LinearAxis" /> class.
        /// </summary>
        public LinearAxis()
        {
            this.InternalAxis = new OxyPlot.Axes.LinearAxis();
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
        /// The create model.
        /// </summary>
        /// <returns>
        /// </returns>
        public override OxyPlot.Axes.Axis CreateModel()
        {
            this.SynchronizeProperties();
            return this.InternalAxis;
        }

        /// <summary>
        /// The synchronize properties.
        /// </summary>
        protected override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            var a = this.InternalAxis as OxyPlot.Axes.LinearAxis;
            a.FormatAsFractions = this.FormatAsFractions;
            a.FractionUnit = this.FractionUnit;
            a.FractionUnitSymbol = this.FractionUnitSymbol;
        }

    }
}