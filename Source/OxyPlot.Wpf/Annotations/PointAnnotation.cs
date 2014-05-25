// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PointAnnotation.cs" company="OxyPlot">
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
//   This is a WPF wrapper of OxyPlot.PointAnnotation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;

    /// <summary>
    /// This is a WPF wrapper of <see cref="OxyPlot.Annotations.PointAnnotation" />.
    /// </summary>
    public class PointAnnotation : ShapeAnnotation
    {
        /// <summary>
        /// Identifies the <see cref="X"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty XProperty = DependencyProperty.Register(
            "X", typeof(double), typeof(PointAnnotation), new PropertyMetadata(0.0, DataChanged));

        /// <summary>
        /// Identifies the <see cref="Y"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty YProperty = DependencyProperty.Register(
            "Y", typeof(double), typeof(PointAnnotation), new PropertyMetadata(0.0, DataChanged));

        /// <summary>
        /// Identifies the <see cref="Size"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            "Size", typeof(double), typeof(PointAnnotation), new PropertyMetadata(4d, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TextMargin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextMarginProperty = DependencyProperty.Register(
            "TextMargin", typeof(double), typeof(PointAnnotation), new PropertyMetadata(2d, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="Shape"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShapeProperty = DependencyProperty.Register(
            "Shape", typeof(MarkerType), typeof(PointAnnotation), new PropertyMetadata(MarkerType.Circle, AppearanceChanged));
        
        /// <summary>
        /// Initializes static members of the <see cref="PointAnnotation"/> class.
        /// </summary>
        static PointAnnotation()
        {
            TextColorProperty.OverrideMetadata(typeof(PointAnnotation), new FrameworkPropertyMetadata(MoreColors.Automatic, AppearanceChanged));
            TextVerticalAlignmentProperty.OverrideMetadata(typeof(PointAnnotation), new FrameworkPropertyMetadata(VerticalAlignment.Top, AppearanceChanged));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointAnnotation" /> class.
        /// </summary>
        public PointAnnotation()
        {
            this.InternalAnnotation = new Annotations.PointAnnotation();
        }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        public double Size
        {
            get
            {
                return (double)this.GetValue(SizeProperty);
            }

            set
            {
                this.SetValue(SizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the text margin.
        /// </summary>
        public double TextMargin
        {
            get
            {
                return (double)this.GetValue(TextMarginProperty);
            }

            set
            {
                this.SetValue(TextMarginProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the shape.
        /// </summary>
        public MarkerType Shape
        {
            get
            {
                return (MarkerType)this.GetValue(ShapeProperty);
            }

            set
            {
                this.SetValue(ShapeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the X coordinate.
        /// </summary>
        public double X
        {
            get
            {
                return (double)this.GetValue(XProperty);
            }

            set
            {
                this.SetValue(XProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the Y coordinate.
        /// </summary>
        public double Y
        {
            get
            {
                return (double)this.GetValue(YProperty);
            }

            set
            {
                this.SetValue(YProperty, value);
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
            var a = (Annotations.PointAnnotation)this.InternalAnnotation;

            a.X = this.X;
            a.Y = this.Y;
            a.Size = this.Size;
            a.TextMargin = this.TextMargin;
            a.Shape = this.Shape;
        }
    }
}