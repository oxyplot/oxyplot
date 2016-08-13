// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PathAnnotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a Avalonia wrapper of OxyPlot.PathAnnotation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    using global::Avalonia.Media;

    using OxyPlot.Annotations;

    /// <summary>
    /// This is a Avalonia wrapper of OxyPlot.PathAnnotation
    /// </summary>
    public abstract class PathAnnotation : TextualAnnotation
    {
        /// <summary>
        /// Identifies the <see cref="Color"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> ColorProperty = AvaloniaProperty.Register<PathAnnotation, Color>(nameof(Color), Colors.Blue);

        /// <summary>
        /// Identifies the <see cref="ClipByXAxis"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<bool> ClipByXAxisProperty = AvaloniaProperty.Register<PathAnnotation, bool>(nameof(ClipByXAxis), true);

        /// <summary>
        /// Identifies the <see cref="ClipByYAxis"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<bool> ClipByYAxisProperty = AvaloniaProperty.Register<PathAnnotation, bool>(nameof(ClipByYAxis), true);

        /// <summary>
        /// Identifies the <see cref="ClipText"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<bool> ClipTextProperty = AvaloniaProperty.Register<PathAnnotation, bool>(nameof(ClipText), true);

        /// <summary>
        /// Identifies the <see cref="LineJoin"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<LineJoin> LineJoinProperty = AvaloniaProperty.Register<PathAnnotation, LineJoin>(nameof(LineJoin), LineJoin.Miter);

        /// <summary>
        /// Identifies the <see cref="LineStyle"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<LineStyle> LineStyleProperty = AvaloniaProperty.Register<PathAnnotation, LineStyle>(nameof(LineStyle), LineStyle.Dash);

        /// <summary>
        /// Identifies the <see cref="StrokeThickness"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> StrokeThicknessProperty = AvaloniaProperty.Register<PathAnnotation, double>(nameof(StrokeThickness), 1.0);

        /// <summary>
        /// Identifies the <see cref="TextMargin"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> TextMarginProperty = AvaloniaProperty.Register<PathAnnotation, double>(nameof(TextMargin), 12.0);

        /// <summary>
        /// Identifies the <see cref="TextOrientation"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<AnnotationTextOrientation> TextOrientationProperty = AvaloniaProperty.Register<PathAnnotation, AnnotationTextOrientation>(nameof(TextOrientation), AnnotationTextOrientation.AlongLine);

        /// <summary>
        /// Identifies the <see cref="TextLinePosition"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> TextLinePositionProperty = AvaloniaProperty.Register<PathAnnotation, double>(nameof(TextLinePosition), 1.0);

        /// <summary>
        /// Gets or sets a value indicating whether to clip the annotation line by the X axis range.
        /// </summary>
        /// <value><c>true</c> if clipping by the X axis is enabled; otherwise, <c>false</c>.</value>
        public bool ClipByXAxis
        {
            get
            {
                return GetValue(ClipByXAxisProperty);
            }

            set
            {
                SetValue(ClipByXAxisProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to clip the annotation line by the Y axis range.
        /// </summary>
        /// <value><c>true</c> if clipping by the Y axis is enabled; otherwise, <c>false</c>.</value>
        public bool ClipByYAxis
        {
            get
            {
                return GetValue(ClipByYAxisProperty);
            }

            set
            {
                SetValue(ClipByYAxisProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the text should be clipped within the plot area.
        /// </summary>
        /// <value><c>true</c> if text should be clipped; otherwise, <c>false</c>.</value>
        public bool ClipText
        {
            get
            {
                return GetValue(ClipTextProperty);
            }

            set
            {
                SetValue(ClipTextProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the annotation color.
        /// </summary>
        /// <value>The color.</value>
        public Color Color
        {
            get
            {
                return GetValue(ColorProperty);
            }

            set
            {
                SetValue(ColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the line join.
        /// </summary>
        /// <value>The line join.</value>
        public LineJoin LineJoin
        {
            get
            {
                return GetValue(LineJoinProperty);
            }

            set
            {
                SetValue(LineJoinProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LineStyle.
        /// </summary>
        public LineStyle LineStyle
        {
            get
            {
                return GetValue(LineStyleProperty);
            }

            set
            {
                SetValue(LineStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets StrokeThickness.
        /// </summary>
        public double StrokeThickness
        {
            get
            {
                return GetValue(StrokeThicknessProperty);
            }

            set
            {
                SetValue(StrokeThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the text margin (along the line).
        /// </summary>
        /// <value>The text margin.</value>
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
        /// Gets or sets the text orientation.
        /// </summary>
        /// <value>The text orientation.</value>
        public AnnotationTextOrientation TextOrientation
        {
            get
            {
                return GetValue(TextOrientationProperty);
            }

            set
            {
                SetValue(TextOrientationProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the text position relative to the line.
        /// </summary>
        /// <value>The text position in the interval [0,1].</value>
        /// <remarks>Positions smaller than 0.25 are left aligned at the start of the line
        /// Positions larger than 0.75 are right aligned at the end of the line
        /// Other positions are center aligned at the specified position</remarks>
        public double TextLinePosition
        {
            get
            {
                return GetValue(TextLinePositionProperty);
            }

            set
            {
                SetValue(TextLinePositionProperty, value);
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

            var a = (Annotations.PathAnnotation)InternalAnnotation;
            a.Color = Color.ToOxyColor();

            a.ClipByXAxis = ClipByXAxis;
            a.ClipByYAxis = ClipByYAxis;

            a.StrokeThickness = StrokeThickness;
            a.LineStyle = LineStyle;
            a.LineJoin = LineJoin;

            a.TextLinePosition = TextLinePosition;
            a.TextOrientation = TextOrientation;
            a.TextMargin = TextMargin;
            a.ClipText = ClipText;
        }

        static PathAnnotation()
        {
            ColorProperty.Changed.AddClassHandler<PathAnnotation>(AppearanceChanged);
            ClipByXAxisProperty.Changed.AddClassHandler<PathAnnotation>(AppearanceChanged);
            ClipByYAxisProperty.Changed.AddClassHandler<PathAnnotation>(AppearanceChanged);
            LineJoinProperty.Changed.AddClassHandler<PathAnnotation>(AppearanceChanged);
            LineStyleProperty.Changed.AddClassHandler<PathAnnotation>(AppearanceChanged);
            StrokeThicknessProperty.Changed.AddClassHandler<PathAnnotation>(AppearanceChanged);
            TextMarginProperty.Changed.AddClassHandler<PathAnnotation>(AppearanceChanged);
            TextOrientationProperty.Changed.AddClassHandler<PathAnnotation>(AppearanceChanged);
            TextLinePositionProperty.Changed.AddClassHandler<PathAnnotation>(AppearanceChanged);
        }
    }
}