// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PointAnnotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a Avalonia wrapper of <see cref="OxyPlot.Annotations.PointAnnotation" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    using global::Avalonia.Layout;

    /// <summary>
    /// This is a Avalonia wrapper of <see cref="OxyPlot.Annotations.PointAnnotation" />.
    /// </summary>
    public class PointAnnotation : ShapeAnnotation
    {
        /// <summary>
        /// Identifies the <see cref="X"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> XProperty = AvaloniaProperty.Register<PointAnnotation, double>(nameof(X), 0.0);

        /// <summary>
        /// Identifies the <see cref="Y"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> YProperty = AvaloniaProperty.Register<PointAnnotation, double>(nameof(Y), 0.0);

        /// <summary>
        /// Identifies the <see cref="Size"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> SizeProperty = AvaloniaProperty.Register<PointAnnotation, double>(nameof(Size), 4d);

        /// <summary>
        /// Identifies the <see cref="TextMargin"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> TextMarginProperty = AvaloniaProperty.Register<PointAnnotation, double>(nameof(TextMargin), 2d);

        /// <summary>
        /// Identifies the <see cref="Shape"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<MarkerType> ShapeProperty = AvaloniaProperty.Register<PointAnnotation, MarkerType>(nameof(Shape), MarkerType.Circle);

        /// <summary>
        /// Initializes static members of the <see cref="PointAnnotation"/> class.
        /// </summary>
        static PointAnnotation()
        {
            TextColorProperty.OverrideDefaultValue<PointAnnotation>(MoreColors.Automatic);
            TextColorProperty.Changed.AddClassHandler<PointAnnotation>(AppearanceChanged);
            TextVerticalAlignmentProperty.OverrideDefaultValue<PointAnnotation>(VerticalAlignment.Top);
            TextVerticalAlignmentProperty.Changed.AddClassHandler<PointAnnotation>(AppearanceChanged);


            XProperty.Changed.AddClassHandler<PointAnnotation>(DataChanged);
            YProperty.Changed.AddClassHandler<PointAnnotation>(DataChanged);
            SizeProperty.Changed.AddClassHandler<PointAnnotation>(AppearanceChanged);
            TextMarginProperty.Changed.AddClassHandler<PointAnnotation>(AppearanceChanged);
            ShapeProperty.Changed.AddClassHandler<PointAnnotation>(AppearanceChanged);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointAnnotation" /> class.
        /// </summary>
        public PointAnnotation()
        {
            InternalAnnotation = new Annotations.PointAnnotation();
        }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        public double Size
        {
            get
            {
                return GetValue(SizeProperty);
            }

            set
            {
                SetValue(SizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the text margin.
        /// </summary>
        public double TextMargin
        {
            get
            {
                return GetValue(TextMarginProperty);
            }

            set
            {
                SetValue(TextMarginProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the shape.
        /// </summary>
        public MarkerType Shape
        {
            get
            {
                return GetValue(ShapeProperty);
            }

            set
            {
                SetValue(ShapeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the X coordinate.
        /// </summary>
        public double X
        {
            get
            {
                return GetValue(XProperty);
            }

            set
            {
                SetValue(XProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the Y coordinate.
        /// </summary>
        public double Y
        {
            get
            {
                return GetValue(YProperty);
            }

            set
            {
                SetValue(YProperty, value);
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
            var a = (Annotations.PointAnnotation)InternalAnnotation;

            a.X = X;
            a.Y = Y;
            a.Size = Size;
            a.TextMargin = TextMargin;
            a.Shape = Shape;
        }
    }
}