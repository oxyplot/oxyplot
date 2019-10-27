// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryAxis.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
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
        /// Identifies the <see cref="GapWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GapWidthProperty = DependencyProperty.Register(
            "GapWidth", typeof(double), typeof(CategoryAxis), new UIPropertyMetadata(1.0));

        /// <summary>
        /// Identifies the <see cref="IsTickCentered"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsTickCenteredProperty = DependencyProperty.Register(
            "IsTickCentered", typeof(bool), typeof(CategoryAxis), new PropertyMetadata(false, DataChanged));

        /// <summary>
        /// Identifies the <see cref="ItemsSource"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource", typeof(IEnumerable), typeof(CategoryAxis), new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// Identifies the <see cref="LabelField"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelFieldProperty = DependencyProperty.Register(
            "LabelField", typeof(string), typeof(CategoryAxis), new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// Identifies the <see cref="Labels"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelsProperty = DependencyProperty.Register(
            "Labels", typeof(IList<string>), typeof(CategoryAxis), new PropertyMetadata(new List<string>(), DataChanged));

        /// <summary>
        /// Initializes static members of the <see cref="CategoryAxis" /> class.
        /// </summary>
        static CategoryAxis()
        {
            PositionProperty.OverrideMetadata(typeof(CategoryAxis), new PropertyMetadata(Axes.AxisPosition.Bottom, DataChanged));
            MinimumPaddingProperty.OverrideMetadata(typeof(CategoryAxis), new PropertyMetadata(0.0, DataChanged));
            MaximumPaddingProperty.OverrideMetadata(typeof(CategoryAxis), new PropertyMetadata(0.0, DataChanged));
            MajorStepProperty.OverrideMetadata(typeof(CategoryAxis), new PropertyMetadata(1.0, DataChanged));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryAxis" /> class.
        /// </summary>
        public CategoryAxis()
        {
            this.InternalAxis = new Axes.CategoryAxis();
        }

        /// <summary>
        /// Gets or sets the gap width.
        /// </summary>
        /// <value>The width of the gap.</value>
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
        /// Creates the internal axis.
        /// </summary>
        /// <returns>The internal axis.</returns>
        public override OxyPlot.Axes.Axis CreateModel()
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
            var a = (OxyPlot.Axes.CategoryAxis)this.InternalAxis;
            a.IsTickCentered = this.IsTickCentered;
            a.ItemsSource = this.ItemsSource;
            a.LabelField = this.LabelField;
            a.GapWidth = this.GapWidth;
            if (this.Labels != null && this.ItemsSource == null)
            {
                a.Labels.Clear();
                a.Labels.AddRange(this.Labels);
            }
        }
    }
}