// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RectangleAnnotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a Avalonia wrapper of OxyPlot.RectangleAnnotation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    /// <summary>
    /// This is a Avalonia wrapper of OxyPlot.RectangleAnnotation
    /// </summary>
    public class RectangleAnnotation : ShapeAnnotation
    {
        /// <summary>
        /// Identifies the <see cref="MaximumX"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> MaximumXProperty = AvaloniaProperty.Register<RectangleAnnotation, double>(nameof(MaximumX), double.MaxValue);

        /// <summary>
        /// Identifies the <see cref="MaximumY"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> MaximumYProperty = AvaloniaProperty.Register<RectangleAnnotation, double>(nameof(MaximumY), double.MaxValue);

        /// <summary>
        /// Identifies the <see cref="MinimumX"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> MinimumXProperty = AvaloniaProperty.Register<RectangleAnnotation, double>(nameof(MinimumX), double.MinValue);

        /// <summary>
        /// Identifies the <see cref="MinimumY"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> MinimumYProperty = AvaloniaProperty.Register<RectangleAnnotation, double>(nameof(MinimumY), double.MinValue);

        /// <summary>
        /// Initializes static members of the <see cref="RectangleAnnotation"/> class.
        /// </summary>
        static RectangleAnnotation()
        {
            TextColorProperty.OverrideDefaultValue<RectangleAnnotation>(MoreColors.Automatic);
            TextColorProperty.Changed.AddClassHandler<RectangleAnnotation>(AppearanceChanged);

            MaximumXProperty.Changed.AddClassHandler<RectangleAnnotation>(DataChanged);
            MaximumYProperty.Changed.AddClassHandler<RectangleAnnotation>(DataChanged);
            MinimumXProperty.Changed.AddClassHandler<RectangleAnnotation>(DataChanged);
            MinimumYProperty.Changed.AddClassHandler<RectangleAnnotation>(DataChanged);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RectangleAnnotation" /> class.
        /// </summary>
        public RectangleAnnotation()
        {
            InternalAnnotation = new Annotations.RectangleAnnotation();
        }

        /// <summary>
        /// Gets or sets the Maximum X.
        /// </summary>
        public double MaximumX
        {
            get
            {
                return GetValue(MaximumXProperty);
            }

            set
            {
                SetValue(MaximumXProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the Maximum Y.
        /// </summary>
        public double MaximumY
        {
            get
            {
                return GetValue(MaximumYProperty);
            }

            set
            {
                SetValue(MaximumYProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the Minimum X.
        /// </summary>
        public double MinimumX
        {
            get
            {
                return GetValue(MinimumXProperty);
            }

            set
            {
                SetValue(MinimumXProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the Minimum Y.
        /// </summary>
        public double MinimumY
        {
            get
            {
                return GetValue(MinimumYProperty);
            }

            set
            {
                SetValue(MinimumYProperty, value);
            }
        }

        /// <summary>
        /// Creates the internal annotation object.
        /// </summary>
        /// <returns>The annotation.</returns>
        public override Annotations.Annotation CreateModel()
        {
            SynchronizeProperties();
            return InternalAnnotation;
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        public override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            var a = (Annotations.RectangleAnnotation)InternalAnnotation;

            a.MinimumX = MinimumX;
            a.MaximumX = MaximumX;
            a.MinimumY = MinimumY;
            a.MaximumY = MaximumY;
        }
    }
}