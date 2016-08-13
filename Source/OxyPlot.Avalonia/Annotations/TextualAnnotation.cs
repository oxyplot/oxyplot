// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextualAnnotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an abstract base class for annotations that contains text.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    using global::Avalonia.Layout;
    using global::Avalonia.Media;

    /// <summary>
    /// Provides an abstract base class for annotations that contains text.
    /// </summary>
    public abstract class TextualAnnotation : Annotation
    {
        /// <summary>
        /// Identifies the <see cref="Text"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<string> TextProperty = AvaloniaProperty.Register<TextualAnnotation, string>(nameof(Text), null);

        /// <summary>
        /// Identifies the <see cref="TextPosition"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<DataPoint> TextPositionProperty = AvaloniaProperty.Register<TextualAnnotation, DataPoint>(nameof(TextPosition), DataPoint.Undefined);

        /// <summary>
        /// Identifies the <see cref="TextRotation"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> TextRotationProperty = AvaloniaProperty.Register<TextualAnnotation, double>(nameof(TextRotation), 0.0);

        /// <summary>
        /// Identifies the <see cref="TextColor"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<Color> TextColorProperty = AvaloniaProperty.Register<TextualAnnotation, Color>(nameof(TextColor), Colors.Blue);

        /// <summary>
        /// Identifies the <see cref="TextHorizontalAlignment"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<HorizontalAlignment> TextHorizontalAlignmentProperty = AvaloniaProperty.Register<TextualAnnotation, HorizontalAlignment>(nameof(TextHorizontalAlignment), HorizontalAlignment.Center);

        /// <summary>
        /// Identifies the <see cref="TextVerticalAlignment"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<VerticalAlignment> TextVerticalAlignmentProperty = AvaloniaProperty.Register<TextualAnnotation, VerticalAlignment>(nameof(TextVerticalAlignment), VerticalAlignment.Center);

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text
        {
            get
            {
                return GetValue(TextProperty);
            }

            set
            {
                SetValue(TextProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        public Color TextColor
        {
            get
            {
                return GetValue(TextColorProperty);
            }

            set
            {
                SetValue(TextColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the text position.
        /// </summary>
        /// <remarks>If the value is <c>DataPoint.Undefined</c>, the centroid of the polygon will be used.</remarks>
        public DataPoint TextPosition
        {
            get { return GetValue(TextPositionProperty); }
            set { SetValue(TextPositionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text horizontal alignment.
        /// </summary>
        /// <value>The text horizontal alignment.</value>
        public HorizontalAlignment TextHorizontalAlignment
        {
            get
            {
                return GetValue(TextHorizontalAlignmentProperty);
            }

            set
            {
                SetValue(TextHorizontalAlignmentProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the vertical alignment of text (above or below the line).
        /// </summary>
        public VerticalAlignment TextVerticalAlignment
        {
            get
            {
                return GetValue(TextVerticalAlignmentProperty);
            }

            set
            {
                SetValue(TextVerticalAlignmentProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the rotation angle (degrees).
        /// </summary>
        public double TextRotation
        {
            get
            {
                return GetValue(TextRotationProperty);
            }

            set
            {
                SetValue(TextRotationProperty, value);
            }
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        public override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            var a = (Annotations.TextualAnnotation)InternalAnnotation;
            a.TextColor = TextColor.ToOxyColor();
            a.Text = Text;
            a.TextPosition = TextPosition;
            a.TextRotation = TextRotation;
            a.TextHorizontalAlignment = TextHorizontalAlignment.ToHorizontalAlignment();
            a.TextVerticalAlignment = TextVerticalAlignment.ToVerticalAlignment();
        }

        static TextualAnnotation()
        {
            TextProperty.Changed.AddClassHandler<TextualAnnotation>(AppearanceChanged);
            TextPositionProperty.Changed.AddClassHandler<TextualAnnotation>(AppearanceChanged);
            TextRotationProperty.Changed.AddClassHandler<TextualAnnotation>(AppearanceChanged);
            TextColorProperty.Changed.AddClassHandler<TextualAnnotation>(AppearanceChanged);
            TextHorizontalAlignmentProperty.Changed.AddClassHandler<TextualAnnotation>(AppearanceChanged);
            TextVerticalAlignmentProperty.Changed.AddClassHandler<TextualAnnotation>(AppearanceChanged);
        }
    }
}