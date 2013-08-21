// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarSeriesBase.cs" company="OxyPlot">
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
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    using OxyPlot.Series;

    /// <summary>
    ///     This is a WPF wrapper of OxyPlot.BarSeriesBase
    /// </summary>
    public class BarSeriesBase : CategorizedSeries
    {
        /// <summary>
        ///     The bar width property.
        /// </summary>
        public static readonly DependencyProperty BaseValueProperty = DependencyProperty.Register(
            "BaseValue", typeof(double), typeof(BarSeriesBase), new PropertyMetadata(0.0, AppearanceChanged));

        /// <summary>
        /// The color field property
        /// </summary>
        public static readonly DependencyProperty ColorFieldProperty = DependencyProperty.Register(
            "ColorField", typeof(string), typeof(BarSeriesBase), new PropertyMetadata(null, DataChanged));

        /// <summary>
        ///     The fill color property.
        /// </summary>
        public static readonly DependencyProperty FillColorProperty = DependencyProperty.Register(
            "FillColor", typeof(Color?), typeof(BarSeriesBase), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        ///     The is stacked property.
        /// </summary>
        public static readonly DependencyProperty IsStackedProperty = DependencyProperty.Register(
            "IsStacked", typeof(bool), typeof(BarSeriesBase), new PropertyMetadata(false, AppearanceChanged));

        /// <summary>
        ///     The label format string property.
        /// </summary>
        public static readonly DependencyProperty LabelFormatStringProperty =
            DependencyProperty.Register(
                "LabelFormatString", typeof(string), typeof(BarSeriesBase), new UIPropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// The label margin property
        /// </summary>
        public static readonly DependencyProperty LabelMarginProperty = DependencyProperty.Register(
            "LabelMargin", typeof(double), typeof(BarSeriesBase), new PropertyMetadata(2.0, AppearanceChanged));

        /// <summary>
        /// The label placement property
        /// </summary>
        public static readonly DependencyProperty LabelPlacementProperty = DependencyProperty.Register(
            "LabelPlacement",
            typeof(LabelPlacement),
            typeof(BarSeriesBase),
            new PropertyMetadata(LabelPlacement.Outside, AppearanceChanged));

        /// <summary>
        ///     The negative fill color property.
        /// </summary>
        public static readonly DependencyProperty NegativeFillColorProperty =
            DependencyProperty.Register(
                "NegativeFillColor",
                typeof(Color?),
                typeof(BarSeriesBase),
                new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// The stack group property
        /// </summary>
        public static readonly DependencyProperty StackGroupProperty = DependencyProperty.Register(
            "StackGroup", typeof(string), typeof(BarSeriesBase), new PropertyMetadata(string.Empty, AppearanceChanged));

        /// <summary>
        ///     The stroke color property.
        /// </summary>
        public static readonly DependencyProperty StrokeColorProperty = DependencyProperty.Register(
            "StrokeColor", typeof(Color), typeof(BarSeriesBase), new PropertyMetadata(Colors.Black, AppearanceChanged));

        /// <summary>
        ///     The stroke thickness property.
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(
                "StrokeThickness", typeof(double), typeof(BarSeriesBase), new PropertyMetadata(0.0, AppearanceChanged));

        /// <summary>
        ///     The value field property.
        /// </summary>
        public static readonly DependencyProperty ValueFieldProperty = DependencyProperty.Register(
            "ValueField", typeof(string), typeof(BarSeriesBase), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        ///     Initializes static members of the <see cref="BarSeriesBase" /> class.
        /// </summary>
        static BarSeriesBase()
        {
            TrackerFormatStringProperty.OverrideMetadata(typeof(CategoryAxis), new PropertyMetadata("{0}, {1}: {2}", AppearanceChanged));
        }

        /// <summary>
        ///     Gets or sets BaseValue.
        /// </summary>
        public double BaseValue
        {
            get
            {
                return (double)this.GetValue(BaseValueProperty);
            }

            set
            {
                this.SetValue(BaseValueProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the color field.
        /// </summary>
        /// <value>
        /// The color field.
        /// </value>
        public string ColorField
        {
            get
            {
                return (string)this.GetValue(ColorFieldProperty);
            }

            set
            {
                this.SetValue(ColorFieldProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the color of the fill.
        /// </summary>
        /// <value>
        /// The color of the fill.
        /// </value>
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
        ///     Gets or sets a value indicating whether the series is stacked.
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
        ///     Gets or sets the label format string.
        /// </summary>
        /// <value> The label format string. </value>
        public string LabelFormatString
        {
            get
            {
                return (string)this.GetValue(LabelFormatStringProperty);
            }

            set
            {
                this.SetValue(LabelFormatStringProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label margin.
        /// </summary>
        /// <value>
        /// The label margin.
        /// </value>
        public double LabelMargin
        {
            get
            {
                return (double)this.GetValue(LabelMarginProperty);
            }

            set
            {
                this.SetValue(LabelMarginProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label placement.
        /// </summary>
        /// <value>
        /// The label placement.
        /// </value>
        public LabelPlacement LabelPlacement
        {
            get
            {
                return (LabelPlacement)this.GetValue(LabelPlacementProperty);
            }

            set
            {
                this.SetValue(LabelPlacementProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets NegativeFillColor.
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
        /// Gets or sets the stack group.
        /// </summary>
        /// <value>
        /// The stack group.
        /// </value>
        public string StackGroup
        {
            get
            {
                return (string)this.GetValue(StackGroupProperty);
            }

            set
            {
                this.SetValue(StackGroupProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the stroke color.
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
        ///     Gets or sets the stroke thickness.
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
        ///     Gets or sets the value field.
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

        /// <summary>
        ///     Creates the model.
        /// </summary>
        /// <returns>
        ///     The series.
        /// </returns>
        public override OxyPlot.Series.Series CreateModel()
        {
            this.SynchronizeProperties(this.InternalSeries);
            return this.InternalSeries;
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        /// <param name="series">The series.</param>
        protected override void SynchronizeProperties(OxyPlot.Series.Series series)
        {
            base.SynchronizeProperties(series);
            var s = (OxyPlot.Series.BarSeriesBase)series;
            s.BaseValue = this.BaseValue;
            s.ColorField = this.ColorField;
            s.FillColor = this.FillColor.ToOxyColor();
            s.IsStacked = this.IsStacked;
            s.NegativeFillColor = this.NegativeFillColor.ToOxyColor();
            s.StrokeColor = this.StrokeColor.ToOxyColor();
            s.StrokeThickness = this.StrokeThickness;
            s.StackGroup = this.StackGroup;
            s.ValueField = this.ValueField;
            s.LabelFormatString = this.LabelFormatString;
            s.LabelMargin = this.LabelMargin;
            s.LabelPlacement = this.LabelPlacement;
        }
    }
}