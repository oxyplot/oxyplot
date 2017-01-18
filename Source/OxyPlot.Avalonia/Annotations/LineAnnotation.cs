// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineAnnotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a Avalonia wrapper of OxyPlot.LineAnnotation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    using global::Avalonia.Layout;
    using OxyPlot.Annotations;

    /// <summary>
    /// This is a Avalonia wrapper of OxyPlot.LineAnnotation
    /// </summary>
    public class LineAnnotation : PathAnnotation
    {
        /// <summary>
        /// Identifies the <see cref="Type"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<LineAnnotationType> TypeProperty = AvaloniaProperty.Register<LineAnnotation, LineAnnotationType>(nameof(Type), LineAnnotationType.LinearEquation);

        /// <summary>
        /// Identifies the <see cref="Intercept"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> InterceptProperty = AvaloniaProperty.Register<LineAnnotation, double>(nameof(Intercept), 0.0);

        /// <summary>
        /// Identifies the <see cref="MaximumX"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> MaximumXProperty = AvaloniaProperty.Register<LineAnnotation, double>(nameof(MaximumX), double.MaxValue);

        /// <summary>
        /// Identifies the <see cref="MaximumY"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> MaximumYProperty = AvaloniaProperty.Register<LineAnnotation, double>(nameof(MaximumY), double.MaxValue);

        /// <summary>
        /// Identifies the <see cref="MinimumX"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> MinimumXProperty = AvaloniaProperty.Register<LineAnnotation, double>(nameof(MinimumX), double.MinValue);

        /// <summary>
        /// Identifies the <see cref="MinimumY"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> MinimumYProperty = AvaloniaProperty.Register<LineAnnotation, double>(nameof(MinimumY), double.MinValue);

        /// <summary>
        /// Identifies the <see cref="Slope"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> SlopeProperty = AvaloniaProperty.Register<LineAnnotation, double>(nameof(Slope), 0.0);

        /// <summary>
        /// Identifies the <see cref="X"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> XProperty = AvaloniaProperty.Register<LineAnnotation, double>(nameof(X), 0.0);

        /// <summary>
        /// Identifies the <see cref="Y"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> YProperty = AvaloniaProperty.Register<LineAnnotation, double>(nameof(Y), 0.0);

        /// <summary>
        /// Initializes static members of the <see cref="LineAnnotation"/> class.
        /// </summary>
        static LineAnnotation()
        {
            TextColorProperty.OverrideDefaultValue<LineAnnotation>(MoreColors.Automatic);
            TextColorProperty.Changed.AddClassHandler<LineAnnotation>(AppearanceChanged);
            TextHorizontalAlignmentProperty.OverrideDefaultValue<LineAnnotation>(HorizontalAlignment.Right);
            TextHorizontalAlignmentProperty.Changed.AddClassHandler<LineAnnotation>(AppearanceChanged);
            TextVerticalAlignmentProperty.OverrideDefaultValue<LineAnnotation>(VerticalAlignment.Top);
            TextVerticalAlignmentProperty.Changed.AddClassHandler<LineAnnotation>(AppearanceChanged);

            TypeProperty.Changed.AddClassHandler<LineAnnotation>(DataChanged);
            InterceptProperty.Changed.AddClassHandler<LineAnnotation>(DataChanged);
            MaximumXProperty.Changed.AddClassHandler<LineAnnotation>(DataChanged);
            MaximumYProperty.Changed.AddClassHandler<LineAnnotation>(DataChanged);
            MinimumXProperty.Changed.AddClassHandler<LineAnnotation>(DataChanged);
            MinimumYProperty.Changed.AddClassHandler<LineAnnotation>(DataChanged);
            SlopeProperty.Changed.AddClassHandler<LineAnnotation>(DataChanged);
            XProperty.Changed.AddClassHandler<LineAnnotation>(DataChanged);
            YProperty.Changed.AddClassHandler<LineAnnotation>(DataChanged);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "LineAnnotation" /> class.
        /// </summary>
        public LineAnnotation()
        {
            InternalAnnotation = new Annotations.LineAnnotation();
        }

        /// <summary>
        /// Gets or sets Intercept.
        /// </summary>
        public double Intercept
        {
            get
            {
                return GetValue(InterceptProperty);
            }

            set
            {
                SetValue(InterceptProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets MaximumX.
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
        /// Gets or sets MaximumY.
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
        /// Gets or sets MinimumX.
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
        /// Gets or sets MinimumY.
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
        /// Gets or sets Slope.
        /// </summary>
        public double Slope
        {
            get
            {
                return GetValue(SlopeProperty);
            }

            set
            {
                SetValue(SlopeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets Type.
        /// </summary>
        public LineAnnotationType Type
        {
            get
            {
                return GetValue(TypeProperty);
            }

            set
            {
                SetValue(TypeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets X.
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
        /// Gets or sets Y.
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
            var a = (Annotations.LineAnnotation)InternalAnnotation;
            a.Type = Type;
            a.Slope = Slope;
            a.Intercept = Intercept;
            a.X = X;
            a.Y = Y;

            a.MinimumX = MinimumX;
            a.MaximumX = MaximumX;
            a.MinimumY = MinimumY;
            a.MaximumY = MaximumY;
        }
    }
}