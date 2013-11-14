// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryAxis.cs" company="OxyPlot">
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
//   This is a WPF wrapper of OxyPlot.CategoryAxis.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Wpf
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.CategoryAxis.
    /// </summary>
    public class CategoryAxis : LinearAxis
    {
        /// <summary>
        /// The gap width property.
        /// </summary>
        public static readonly DependencyProperty GapWidthProperty = DependencyProperty.Register(
            "GapWidth", typeof(double), typeof(CategoryAxis), new UIPropertyMetadata(1.0));

        /// <summary>
        /// The is tick centered property.
        /// </summary>
        public static readonly DependencyProperty IsTickCenteredProperty = DependencyProperty.Register(
            "IsTickCentered", typeof(bool), typeof(CategoryAxis), new PropertyMetadata(false, DataChanged));

        /// <summary>
        /// The items source property.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource", typeof(IEnumerable), typeof(CategoryAxis), new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// The label field property.
        /// </summary>
        public static readonly DependencyProperty LabelFieldProperty = DependencyProperty.Register(
            "LabelField", typeof(string), typeof(CategoryAxis), new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// The labels property.
        /// </summary>
        public static readonly DependencyProperty LabelsProperty = DependencyProperty.Register(
            "Labels", typeof(IList<string>), typeof(CategoryAxis), new PropertyMetadata(new List<string>(), DataChanged));

        /// <summary>
        /// Initializes static members of the <see cref="CategoryAxis"/> class.
        /// </summary>
        static CategoryAxis()
        {
            PositionProperty.OverrideMetadata(typeof(CategoryAxis), new PropertyMetadata(Axes.AxisPosition.Bottom, DataChanged));
            MinimumPaddingProperty.OverrideMetadata(typeof(CategoryAxis), new PropertyMetadata(0.0, DataChanged));
            MaximumPaddingProperty.OverrideMetadata(typeof(CategoryAxis), new PropertyMetadata(0.0, DataChanged));
            MajorStepProperty.OverrideMetadata(typeof(CategoryAxis), new PropertyMetadata(1.0, DataChanged));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryAxis"/> class.
        /// </summary>
        public CategoryAxis()
        {
            this.InternalAxis = new OxyPlot.Axes.CategoryAxis();
        }

        /// <summary>
        /// Gets or sets the gap width.
        /// </summary>
        /// <value>
        /// The width of the gap.
        /// </value>
        public double GapWidth
        {
            get
            {
                return (double)this.GetValue(GapWidthProperty);
            }

            set
            {
                this.SetValue(GapWidthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether IsTickCentered.
        /// </summary>
        public bool IsTickCentered
        {
            get
            {
                return (bool)this.GetValue(IsTickCenteredProperty);
            }

            set
            {
                this.SetValue(IsTickCenteredProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets ItemsSource.
        /// </summary>
        public IEnumerable ItemsSource
        {
            get
            {
                return (IEnumerable)this.GetValue(ItemsSourceProperty);
            }

            set
            {
                this.SetValue(ItemsSourceProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LabelField.
        /// </summary>
        public string LabelField
        {
            get
            {
                return (string)this.GetValue(LabelFieldProperty);
            }

            set
            {
                this.SetValue(LabelFieldProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets Labels.
        /// </summary>
        public IList<string> Labels
        {
            get
            {
                return (IList<string>)this.GetValue(LabelsProperty);
            }

            set
            {
                this.SetValue(LabelsProperty, value);
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
            var a = (OxyPlot.Axes.CategoryAxis)this.InternalAxis;
            a.IsTickCentered = this.IsTickCentered;
            a.ItemsSource = this.ItemsSource;
            a.LabelField = this.LabelField;
            a.GapWidth = this.GapWidth;
            if (this.Labels != null && this.ItemsSource == null)
            {
                a.Labels.Clear();
                foreach (string label in this.Labels)
                {
                    a.Labels.Add(label);
                }
            }
        }

    }
}