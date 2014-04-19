// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AngleAxis.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
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
//   This is a WPF wrapper of OxyPlot.AngleAxis.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;

    using OxyPlot.Axes;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.AngleAxis.
    /// </summary>
    public class AngleAxis : LinearAxis
    {
        /// <summary>
        /// Identifies the <see cref="StartAngle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(AngleAxis), new PropertyMetadata(0d));

        /// <summary>
        /// Identifies the <see cref="EndAngle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EndAngleProperty =
            DependencyProperty.Register("EndAngle", typeof(double), typeof(AngleAxis), new PropertyMetadata(360d));

        /// <summary>
        /// Initializes static members of the <see cref = "AngleAxis" /> class.
        /// </summary>
        static AngleAxis()
        {
            MajorGridlineStyleProperty.OverrideMetadata(typeof(AngleAxis), new PropertyMetadata(LineStyle.Solid));
            MinorGridlineStyleProperty.OverrideMetadata(typeof(AngleAxis), new PropertyMetadata(LineStyle.Solid));
            PositionProperty.OverrideMetadata(typeof(AngleAxis), new PropertyMetadata(AxisPosition.None, AppearanceChanged));
            TickStyleProperty.OverrideMetadata(typeof(AngleAxis), new PropertyMetadata(TickStyle.None, AppearanceChanged));
            IsPanEnabledProperty.OverrideMetadata(typeof(AngleAxis), new PropertyMetadata(false));
            IsZoomEnabledProperty.OverrideMetadata(typeof(AngleAxis), new PropertyMetadata(false));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "AngleAxis" /> class.
        /// </summary>
        public AngleAxis()
        {
            this.InternalAxis = new Axes.AngleAxis();
            this.MajorGridlineStyle = LineStyle.Solid;
            this.MinorGridlineStyle = LineStyle.Solid;
        }

        /// <summary>
        /// Gets or sets the start angle (degrees).
        /// </summary>
        public double StartAngle
        {
            get { return (double)this.GetValue(StartAngleProperty); }
            set { this.SetValue(StartAngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the end angle (degrees).
        /// </summary>
        public double EndAngle
        {
            get { return (double)this.GetValue(EndAngleProperty); }
            set { this.SetValue(EndAngleProperty, value); }
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        protected override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            var a = (Axes.AngleAxis)this.InternalAxis;
            a.StartAngle = this.StartAngle;
            a.EndAngle = this.EndAngle;
            a.FractionUnitSymbol = this.FractionUnitSymbol;
        }
    }
}