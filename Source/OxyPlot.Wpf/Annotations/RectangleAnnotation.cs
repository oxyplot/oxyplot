// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RectangleAnnotation.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
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
//   This is a WPF wrapper of OxyPlot.RectangleAnnotation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    ///     This is a WPF wrapper of OxyPlot.RectangleAnnotation
    /// </summary>
    public class RectangleAnnotation : TextualAnnotation
    {
        /// <summary>
        ///     The Fill property.
        /// </summary>
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
            "Fill",
            typeof(Color),
            typeof(RectangleAnnotation),
            new PropertyMetadata(Colors.LightBlue, AppearanceChanged));

        /// <summary>
        ///     The Stroke property.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
            "Stroke",
            typeof(Color),
            typeof(RectangleAnnotation),
            new PropertyMetadata(Colors.Black, AppearanceChanged));

        /// <summary>
        ///     The Stroke property.
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            "StrokeThickness",
            typeof(double),
            typeof(RectangleAnnotation),
            new PropertyMetadata(0.0, AppearanceChanged));

        /// <summary>
        ///     The MaximumX property.
        /// </summary>
        public static readonly DependencyProperty MaximumXProperty = DependencyProperty.Register(
            "MaximumX", typeof(double), typeof(RectangleAnnotation), new PropertyMetadata(double.MaxValue, DataChanged));

        /// <summary>
        ///     The MaximumY property.
        /// </summary>
        public static readonly DependencyProperty MaximumYProperty = DependencyProperty.Register(
            "MaximumY", typeof(double), typeof(RectangleAnnotation), new PropertyMetadata(double.MaxValue, DataChanged));

        /// <summary>
        ///     The MinimumX property.
        /// </summary>
        public static readonly DependencyProperty MinimumXProperty = DependencyProperty.Register(
            "MinimumX", typeof(double), typeof(RectangleAnnotation), new PropertyMetadata(double.MinValue, DataChanged));

        /// <summary>
        ///     The MinimumY property.
        /// </summary>
        public static readonly DependencyProperty MinimumYProperty = DependencyProperty.Register(
            "MinimumY", typeof(double), typeof(RectangleAnnotation), new PropertyMetadata(double.MinValue, DataChanged));

        /// <summary>
        ///     The text rotation property
        /// </summary>
        public static readonly DependencyProperty TextRotationProperty = DependencyProperty.Register(
            "TextRotation", typeof(double), typeof(RectangleAnnotation), new UIPropertyMetadata(0.0));

        /// <summary>
        ///     Initializes a new instance of the <see cref="RectangleAnnotation" /> class.
        /// </summary>
        public RectangleAnnotation()
        {
            this.InternalAnnotation = new Annotations.RectangleAnnotation();
        }

        /// <summary>
        ///     Gets or sets the fill color.
        /// </summary>
        public Color Fill
        {
            get
            {
                return (Color)this.GetValue(FillProperty);
            }

            set
            {
                this.SetValue(FillProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the stroke color.
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
        ///     Gets or sets the stroke thickness.
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
        ///     Gets or sets the Maximum X.
        /// </summary>
        public double MaximumX
        {
            get
            {
                return (double)this.GetValue(MaximumXProperty);
            }

            set
            {
                this.SetValue(MaximumXProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the Maximum Y.
        /// </summary>
        public double MaximumY
        {
            get
            {
                return (double)this.GetValue(MaximumYProperty);
            }

            set
            {
                this.SetValue(MaximumYProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the Minimum X.
        /// </summary>
        public double MinimumX
        {
            get
            {
                return (double)this.GetValue(MinimumXProperty);
            }

            set
            {
                this.SetValue(MinimumXProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the Minimum Y.
        /// </summary>
        public double MinimumY
        {
            get
            {
                return (double)this.GetValue(MinimumYProperty);
            }

            set
            {
                this.SetValue(MinimumYProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the text rotation (degrees).
        /// </summary>
        /// <value>The text rotation in degrees.</value>
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
        ///     Creates the internal annotation object.
        /// </summary>
        /// <returns>
        ///     The annotation.
        /// </returns>
        public override Annotations.Annotation CreateModel()
        {
            this.SynchronizeProperties();
            return this.InternalAnnotation;
        }

        /// <summary>
        ///     Synchronizes the properties.
        /// </summary>
        public override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            var a = (Annotations.RectangleAnnotation)this.InternalAnnotation;

            a.Fill = this.Fill.ToOxyColor();
            a.Stroke = this.Stroke.ToOxyColor();
            a.StrokeThickness = this.StrokeThickness;

            a.MinimumX = this.MinimumX;
            a.MaximumX = this.MaximumX;
            a.MinimumY = this.MinimumY;
            a.MaximumY = this.MaximumY;
            a.TextRotation = this.TextRotation;
        }
    }
}