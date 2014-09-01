// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextualAnnotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides an abstract base class for annotations that contains text.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Provides an abstract base class for annotations that contains text.
    /// </summary>
    public abstract class TextualAnnotation : Annotation
    {
        /// <summary>
        /// Identifies the <see cref="Text"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(TextualAnnotation), new PropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TextPosition"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextPositionProperty =
            DependencyProperty.Register("TextPosition", typeof(DataPoint), typeof(TextualAnnotation), new PropertyMetadata(DataPoint.Undefined, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TextRotation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextRotationProperty = DependencyProperty.Register(
            "TextRotation", typeof(double), typeof(TextualAnnotation), new PropertyMetadata(0.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TextColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextColorProperty = DependencyProperty.Register(
            "TextColor", typeof(Color), typeof(TextualAnnotation), new PropertyMetadata(Colors.Blue, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TextHorizontalAlignment"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextHorizontalAlignmentProperty =
            DependencyProperty.Register(
                "TextHorizontalAlignment",
                typeof(HorizontalAlignment),
                typeof(TextualAnnotation),
                new UIPropertyMetadata(HorizontalAlignment.Center, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TextVerticalAlignment"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextVerticalAlignmentProperty =
            DependencyProperty.Register(
                "TextVerticalAlignment",
                typeof(VerticalAlignment),
                typeof(TextualAnnotation),
                new UIPropertyMetadata(VerticalAlignment.Center, AppearanceChanged));

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text
        {
            get
            {
                return (string)this.GetValue(TextProperty);
            }

            set
            {
                this.SetValue(TextProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        public Color TextColor
        {
            get
            {
                return (Color)this.GetValue(TextColorProperty);
            }

            set
            {
                this.SetValue(TextColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the text position.
        /// </summary>
        /// <remarks>If the value is <c>DataPoint.Undefined</c>, the centroid of the polygon will be used.</remarks>
        public DataPoint TextPosition
        {
            get { return (DataPoint)this.GetValue(TextPositionProperty); }
            set { this.SetValue(TextPositionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text horizontal alignment.
        /// </summary>
        /// <value>The text horizontal alignment.</value>
        public HorizontalAlignment TextHorizontalAlignment
        {
            get
            {
                return (HorizontalAlignment)this.GetValue(TextHorizontalAlignmentProperty);
            }

            set
            {
                this.SetValue(TextHorizontalAlignmentProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the vertical alignment of text (above or below the line).
        /// </summary>
        public VerticalAlignment TextVerticalAlignment
        {
            get
            {
                return (VerticalAlignment)this.GetValue(TextVerticalAlignmentProperty);
            }

            set
            {
                this.SetValue(TextVerticalAlignmentProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the rotation angle (degrees).
        /// </summary>
        public double TextRotation
        {
            get
            {
                return (double)this.GetValue(TextRotationProperty);
            }

            set
            {
                this.SetValue(TextRotationProperty, value);
            }
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        public override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            var a = (Annotations.TextualAnnotation)this.InternalAnnotation;
            a.TextColor = this.TextColor.ToOxyColor();
            a.Text = this.Text;
            a.TextPosition = this.TextPosition;
            a.TextRotation = this.TextRotation;
            a.TextHorizontalAlignment = this.TextHorizontalAlignment.ToHorizontalAlignment();
            a.TextVerticalAlignment = this.TextVerticalAlignment.ToVerticalAlignment();
        }
    }
}