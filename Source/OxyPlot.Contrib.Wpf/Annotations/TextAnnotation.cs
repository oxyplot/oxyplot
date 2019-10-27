// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextAnnotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a WPF wrapper of OxyPlot.TextAnnotation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.TextAnnotation
    /// </summary>
    public class TextAnnotation : TextualAnnotation
    {
        /// <summary>
        /// Identifies the <see cref="Background"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
            "Background", typeof(Color), typeof(TextAnnotation), new PropertyMetadata(MoreColors.Undefined, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="Offset"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register(
            "Offset",
            typeof(Vector),
            typeof(TextAnnotation),
            new PropertyMetadata(default(Vector), AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="Padding"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register(
            "Padding",
            typeof(Thickness),
            typeof(TextAnnotation),
            new PropertyMetadata(new Thickness(4), AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="Stroke"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
            "Stroke", typeof(Color), typeof(TextAnnotation), new PropertyMetadata(Colors.Black, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="StrokeThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(
                "StrokeThickness", typeof(double), typeof(TextAnnotation), new PropertyMetadata(1.0, AppearanceChanged));

        /// <summary>
        /// Initializes static members of the <see cref = "TextAnnotation" /> class.
        /// </summary>
        static TextAnnotation()
        {
            TextHorizontalAlignmentProperty.OverrideMetadata(typeof(TextAnnotation), new FrameworkPropertyMetadata(HorizontalAlignment.Center, AppearanceChanged));
            TextVerticalAlignmentProperty.OverrideMetadata(typeof(TextAnnotation), new FrameworkPropertyMetadata(VerticalAlignment.Bottom, AppearanceChanged));
            TextColorProperty.OverrideMetadata(typeof(TextAnnotation), new FrameworkPropertyMetadata(MoreColors.Automatic, AppearanceChanged));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "TextAnnotation" /> class.
        /// </summary>
        public TextAnnotation()
        {
            this.InternalAnnotation = new Annotations.TextAnnotation();
        }

        /// <summary>
        /// Gets or sets the fill color of the background rectangle.
        /// </summary>
        public Color Background
        {
            get
            {
                return (Color)this.GetValue(BackgroundProperty);
            }

            set
            {
                this.SetValue(BackgroundProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the position offset (screen coordinates).
        /// </summary>
        public Vector Offset
        {
            get
            {
                return (Vector)this.GetValue(OffsetProperty);
            }

            set
            {
                this.SetValue(OffsetProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the padding of the background rectangle.
        /// </summary>
        public Thickness Padding
        {
            get
            {
                return (Thickness)this.GetValue(PaddingProperty);
            }

            set
            {
                this.SetValue(PaddingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the stroke color of the background rectangle.
        /// </summary>
        public Color Stroke
        {
            get
            {
                return (Color)this.GetValue(StrokeProperty);
            }

            set
            {
                this.SetValue(StrokeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the stroke thickness of the background rectangle.
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
        /// Creates the internal annotation object.
        /// </summary>
        /// <returns>The annotation.</returns>
        public override Annotations.Annotation CreateModel()
        {
            this.SynchronizeProperties();
            return this.InternalAnnotation;
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        public override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            var a = (Annotations.TextAnnotation)this.InternalAnnotation;
            a.TextHorizontalAlignment = this.HorizontalAlignment.ToHorizontalAlignment();
            a.Background = this.Background.ToOxyColor();

            a.Offset = this.Offset.ToScreenVector();
            a.TextVerticalAlignment = this.VerticalAlignment.ToVerticalAlignment();
            a.Padding = this.Padding.ToOxyThickness();

            a.Stroke = this.Stroke.ToOxyColor();
            a.StrokeThickness = this.StrokeThickness;
        }
    }
}