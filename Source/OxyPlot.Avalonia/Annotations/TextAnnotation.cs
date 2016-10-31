// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextAnnotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a Avalonia wrapper of OxyPlot.TextAnnotation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    using global::Avalonia.Layout;
    using global::Avalonia.Media;

    /// <summary>
    /// This is a Avalonia wrapper of OxyPlot.TextAnnotation
    /// </summary>
    public class TextAnnotation : TextualAnnotation
    {
        /// <summary>
        /// Identifies the <see cref="Background"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> BackgroundProperty = AvaloniaProperty.Register<TextAnnotation, Color>(nameof(Background), MoreColors.Undefined);

        /// <summary>
        /// Identifies the <see cref="Offset"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Vector> OffsetProperty = AvaloniaProperty.Register<TextAnnotation, Vector>(nameof(Offset), default(Vector));

        /// <summary>
        /// Identifies the <see cref="Padding"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Thickness> PaddingProperty = AvaloniaProperty.Register<TextAnnotation, Thickness>(nameof(Padding), new Thickness(4));

        /// <summary>
        /// Identifies the <see cref="Stroke"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> StrokeProperty = AvaloniaProperty.Register<TextAnnotation, Color>(nameof(Stroke), Colors.Black);

        /// <summary>
        /// Identifies the <see cref="StrokeThickness"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> StrokeThicknessProperty = AvaloniaProperty.Register<TextAnnotation, double>(nameof(StrokeThickness), 1.0);

        /// <summary>
        /// Initializes static members of the <see cref = "TextAnnotation" /> class.
        /// </summary>
        static TextAnnotation()
        {
            TextColorProperty.OverrideDefaultValue<TextAnnotation>(MoreColors.Automatic);
            TextColorProperty.Changed.AddClassHandler<TextAnnotation>(AppearanceChanged);
            TextHorizontalAlignmentProperty.OverrideDefaultValue<TextAnnotation>(HorizontalAlignment.Right);
            TextHorizontalAlignmentProperty.Changed.AddClassHandler<TextAnnotation>(AppearanceChanged);
            TextVerticalAlignmentProperty.OverrideDefaultValue<TextAnnotation>(VerticalAlignment.Top);
            TextVerticalAlignmentProperty.Changed.AddClassHandler<TextAnnotation>(AppearanceChanged);

            BackgroundProperty.Changed.AddClassHandler<TextAnnotation>(AppearanceChanged);
            OffsetProperty.Changed.AddClassHandler<TextAnnotation>(AppearanceChanged);
            PaddingProperty.Changed.AddClassHandler<TextAnnotation>(AppearanceChanged);
            StrokeProperty.Changed.AddClassHandler<TextAnnotation>(AppearanceChanged);
            StrokeThicknessProperty.Changed.AddClassHandler<TextAnnotation>(AppearanceChanged);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "TextAnnotation" /> class.
        /// </summary>
        public TextAnnotation()
        {
            InternalAnnotation = new Annotations.TextAnnotation();
        }

        /// <summary>
        /// Gets or sets the fill color of the background rectangle.
        /// </summary>
        public Color Background
        {
            get
            {
                return GetValue(BackgroundProperty);
            }

            set
            {
                SetValue(BackgroundProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the position offset (screen coordinates).
        /// </summary>
        public Vector Offset
        {
            get
            {
                return GetValue(OffsetProperty);
            }

            set
            {
                SetValue(OffsetProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the padding of the background rectangle.
        /// </summary>
        public Thickness Padding
        {
            get
            {
                return GetValue(PaddingProperty);
            }

            set
            {
                SetValue(PaddingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the stroke color of the background rectangle.
        /// </summary>
        public Color Stroke
        {
            get
            {
                return GetValue(StrokeProperty);
            }

            set
            {
                SetValue(StrokeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the stroke thickness of the background rectangle.
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
            var a = (Annotations.TextAnnotation)InternalAnnotation;
            a.TextHorizontalAlignment = HorizontalAlignment.ToHorizontalAlignment();
            a.Background = Background.ToOxyColor();

            a.Offset = Offset.ToScreenVector();
            a.TextVerticalAlignment = VerticalAlignment.ToVerticalAlignment();
            a.Padding = Padding.ToOxyThickness();

            a.Stroke = Stroke.ToOxyColor();
            a.StrokeThickness = StrokeThickness;
        }
    }
}