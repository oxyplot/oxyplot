// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EllipseAnnotation.cs" company="OxyPlot">
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
//   This is a WPF wrapper of OxyPlot.EllipseAnnotation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    ///     This is a WPF wrapper of OxyPlot.EllipseAnnotation
    /// </summary>
    public class EllipseAnnotation : TextualAnnotation
    {
        /// <summary>
        ///     The Fill property.
        /// </summary>
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
            "Fill",
            typeof(Color),
            typeof(EllipseAnnotation),
            new PropertyMetadata(Colors.LightBlue, AppearanceChanged));

        /// <summary>
        ///     The Stroke property.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
            "Stroke",
            typeof(Color),
            typeof(EllipseAnnotation),
            new PropertyMetadata(Colors.Black, AppearanceChanged));

        /// <summary>
        ///     The Stroke property.
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            "StrokeThickness",
            typeof(double),
            typeof(EllipseAnnotation),
            new PropertyMetadata(0.0, AppearanceChanged));

        /// <summary>
        ///     The X property.
        /// </summary>
        public static readonly DependencyProperty XProperty = DependencyProperty.Register(
            "X", typeof(double), typeof(EllipseAnnotation), new PropertyMetadata(0.0, DataChanged));

        /// <summary>
        ///     The Y property.
        /// </summary>
        public static readonly DependencyProperty YProperty = DependencyProperty.Register(
            "Y", typeof(double), typeof(EllipseAnnotation), new PropertyMetadata(0.0, DataChanged));

        /// <summary>
        ///     The text rotation property
        /// </summary>
        public static readonly DependencyProperty TextRotationProperty = DependencyProperty.Register(
            "TextRotation", typeof(double), typeof(EllipseAnnotation), new UIPropertyMetadata(0.0));

        /// <summary>
        ///     Initializes a new instance of the <see cref="EllipseAnnotation" /> class.
        /// </summary>
        public EllipseAnnotation()
        {
            this.InternalAnnotation = new Annotations.EllipseAnnotation();
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
        ///     Gets or sets the  X.
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
        ///     Gets or sets the  Y.
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
            var a = (Annotations.EllipseAnnotation)this.InternalAnnotation;

            a.Fill = this.Fill.ToOxyColor();
            a.Stroke = this.Stroke.ToOxyColor();
            a.StrokeThickness = this.StrokeThickness;

            a.X = this.X;
            a.Width = this.Width;
            a.Y = this.Y;
            a.Height = this.Height;
            a.TextRotation = this.TextRotation;
        }
    }
}