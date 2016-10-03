// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryAxis.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a Avalonia wrapper of OxyPlot.CategoryAxis.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// This is a Avalonia wrapper of OxyPlot.CategoryAxis.
    /// </summary>
    public class CategoryAxis : LinearAxis
    {
        /// <summary>
        /// Identifies the <see cref="GapWidth"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> GapWidthProperty = AvaloniaProperty.Register<CategoryAxis, double>(nameof(GapWidth), 1.0);

        /// <summary>
        /// Identifies the <see cref="IsTickCentered"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<bool> IsTickCenteredProperty = AvaloniaProperty.Register<CategoryAxis, bool>(nameof(IsTickCentered), false);

        /// <summary>
        /// Identifies the <see cref="Items"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<IEnumerable> ItemsProperty = AvaloniaProperty.Register<CategoryAxis, IEnumerable>(nameof(Items), null);

        /// <summary>
        /// Identifies the <see cref="LabelField"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> LabelFieldProperty = AvaloniaProperty.Register<CategoryAxis, string>(nameof(LabelField), null);

        /// <summary>
        /// Identifies the <see cref="Labels"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<IList<string>> LabelsProperty = AvaloniaProperty.Register<CategoryAxis, IList<string>>(nameof(Labels), new List<string>());

        /// <summary>
        /// Initializes static members of the <see cref="CategoryAxis" /> class.
        /// </summary>
        static CategoryAxis()
        {
            PositionProperty.OverrideMetadata(typeof(CategoryAxis), new StyledPropertyMetadata<Axes.AxisPosition>(Axes.AxisPosition.Bottom));
            MinimumPaddingProperty.OverrideMetadata(typeof(CategoryAxis), new StyledPropertyMetadata<double>(0.0));
            MaximumPaddingProperty.OverrideMetadata(typeof(CategoryAxis), new StyledPropertyMetadata<double>(0.0));
            MajorStepProperty.OverrideMetadata(typeof(CategoryAxis), new StyledPropertyMetadata<double>(1.0));
            IsTickCenteredProperty.Changed.AddClassHandler<CategoryAxis>(DataChanged);
            ItemsProperty.Changed.AddClassHandler<CategoryAxis>(DataChanged);
            LabelFieldProperty.Changed.AddClassHandler<CategoryAxis>(DataChanged);
            LabelsProperty.Changed.AddClassHandler<CategoryAxis>(DataChanged);
            PositionProperty.Changed.AddClassHandler<CategoryAxis>(DataChanged);
            MinimumPaddingProperty.Changed.AddClassHandler<CategoryAxis>(DataChanged);
            MaximumPaddingProperty.Changed.AddClassHandler<CategoryAxis>(DataChanged);
            MajorStepProperty.Changed.AddClassHandler<CategoryAxis>(DataChanged);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryAxis" /> class.
        /// </summary>
        public CategoryAxis()
        {
            InternalAxis = new Axes.CategoryAxis();
        }

        /// <summary>
        /// Gets or sets the gap width.
        /// </summary>
        /// <value>The width of the gap.</value>
        public double GapWidth
        {
            get
            {
                return GetValue(GapWidthProperty);
            }

            set
            {
                SetValue(GapWidthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether IsTickCentered.
        /// </summary>
        public bool IsTickCentered
        {
            get
            {
                return GetValue(IsTickCenteredProperty);
            }

            set
            {
                SetValue(IsTickCenteredProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets ItemsSource.
        /// </summary>
        public IEnumerable Items
        {
            get
            {
                return GetValue(ItemsProperty);
            }

            set
            {
                SetValue(ItemsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LabelField.
        /// </summary>
        public string LabelField
        {
            get
            {
                return GetValue(LabelFieldProperty);
            }

            set
            {
                SetValue(LabelFieldProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets Labels.
        /// </summary>
        public IList<string> Labels
        {
            get
            {
                return GetValue(LabelsProperty);
            }

            set
            {
                SetValue(LabelsProperty, value);
            }
        }

        /// <summary>
        /// Creates the internal axis.
        /// </summary>
        /// <returns>The internal axis.</returns>
        public override OxyPlot.Axes.Axis CreateModel()
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
            var a = (OxyPlot.Axes.CategoryAxis)InternalAxis;
            a.IsTickCentered = IsTickCentered;
            a.ItemsSource = Items;
            a.LabelField = LabelField;
            a.GapWidth = GapWidth;
            if (Labels != null && Items == null)
            {
                a.Labels.Clear();
                a.Labels.AddRange(Labels);
            }
        }
    }
}