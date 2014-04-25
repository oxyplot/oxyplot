// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextualAnnotation.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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