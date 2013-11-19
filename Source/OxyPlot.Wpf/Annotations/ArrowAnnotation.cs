// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrowAnnotation.cs" company="OxyPlot">
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
//   This is a WPF wrapper of OxyPlot.ArrowAnnotation
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.ArrowAnnotation
    /// </summary>
    public class ArrowAnnotation : TextualAnnotation
    {
        /// <summary>
        /// The ArrowDirection property.
        /// </summary>
        public static readonly DependencyProperty ArrowDirectionProperty = DependencyProperty.Register(
            "ArrowDirection", typeof(ScreenVector), typeof(ArrowAnnotation), new PropertyMetadata(DataChanged));

        /// <summary>
        /// The Color property.
        /// </summary>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof(Color), typeof(ArrowAnnotation), new PropertyMetadata(Colors.Blue, AppearanceChanged));

        /// <summary>
        /// The EndPoint property.
        /// </summary>
        public static readonly DependencyProperty EndPointProperty = DependencyProperty.Register(
            "EndPoint", typeof(DataPoint), typeof(ArrowAnnotation), new PropertyMetadata(DataChanged));

        /// <summary>
        /// The HeadLength property.
        /// </summary>
        public static readonly DependencyProperty HeadLengthProperty = DependencyProperty.Register(
            "HeadLength", typeof(double), typeof(ArrowAnnotation), new PropertyMetadata(10.0, AppearanceChanged));

        /// <summary>
        /// The HeadWidth property.
        /// </summary>
        public static readonly DependencyProperty HeadWidthProperty = DependencyProperty.Register(
            "HeadWidth", typeof(double), typeof(ArrowAnnotation), new PropertyMetadata(3.0, AppearanceChanged));

        /// <summary>
        /// The LineJoin property.
        /// </summary>
        public static readonly DependencyProperty LineJoinProperty = DependencyProperty.Register(
            "LineJoin", typeof(OxyPenLineJoin), typeof(ArrowAnnotation), new UIPropertyMetadata(OxyPenLineJoin.Miter, AppearanceChanged));

        /// <summary>
        /// The LineStyle property.
        /// </summary>
        public static readonly DependencyProperty LineStyleProperty = DependencyProperty.Register(
            "LineStyle", typeof(LineStyle), typeof(ArrowAnnotation), new PropertyMetadata(LineStyle.Solid, AppearanceChanged));

        /// <summary>
        /// The StartPoint property.
        /// </summary>
        public static readonly DependencyProperty StartPointProperty = DependencyProperty.Register(
            "StartPoint", typeof(DataPoint), typeof(ArrowAnnotation), new PropertyMetadata(DataChanged));

        /// <summary>
        /// The StrokeThickness property.
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(
                "StrokeThickness", typeof(double), typeof(ArrowAnnotation), new PropertyMetadata(2.0, AppearanceChanged));

        /// <summary>
        /// The Veeness property.
        /// </summary>
        public static readonly DependencyProperty VeenessProperty = DependencyProperty.Register(
            "Veeness", typeof(double), typeof(ArrowAnnotation), new PropertyMetadata(0.0, AppearanceChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref = "ArrowAnnotation" /> class.
        /// </summary>
        public ArrowAnnotation()
        {
            this.InternalAnnotation = new OxyPlot.Annotations.ArrowAnnotation();
        }

        /// <summary>
        /// Gets or sets the arrow direction.
        /// </summary>
        public ScreenVector ArrowDirection
        {
            get
            {
                return (ScreenVector)this.GetValue(ArrowDirectionProperty);
            }

            set
            {
                this.SetValue(ArrowDirectionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        public Color Color
        {
            get
            {
                return (Color)this.GetValue(ColorProperty);
            }

            set
            {
                this.SetValue(ColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the end point.
        /// </summary>
        public DataPoint EndPoint
        {
            get
            {
                return (DataPoint)this.GetValue(EndPointProperty);
            }

            set
            {
                this.SetValue(EndPointProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the length of the head (relative to the stroke thickness).
        /// </summary>
        /// <value> The length of the head. </value>
        public double HeadLength
        {
            get
            {
                return (double)this.GetValue(HeadLengthProperty);
            }

            set
            {
                this.SetValue(HeadLengthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the width of the head (relative to the stroke thickness).
        /// </summary>
        /// <value> The width of the head. </value>
        public double HeadWidth
        {
            get
            {
                return (double)this.GetValue(HeadWidthProperty);
            }

            set
            {
                this.SetValue(HeadWidthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the line join.
        /// </summary>
        /// <value>The line join.</value>
        public OxyPenLineJoin LineJoin
        {
            get
            {
                return (OxyPenLineJoin)this.GetValue(LineJoinProperty);
            }

            set
            {
                this.SetValue(LineJoinProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets LineStyle.
        /// </summary>
        public LineStyle LineStyle
        {
            get
            {
                return (LineStyle)this.GetValue(LineStyleProperty);
            }

            set
            {
                this.SetValue(LineStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the start point.
        /// </summary>
        /// <remarks>
        /// This property is overridden by the ArrowDirection property, if set.
        /// </remarks>
        public DataPoint StartPoint
        {
            get
            {
                return (DataPoint)this.GetValue(StartPointProperty);
            }

            set
            {
                this.SetValue(StartPointProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the stroke thickness.
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
        /// Gets or sets the 'veeness' of the arrow head (relative to thickness).
        /// </summary>
        public double Veeness
        {
            get
            {
                return (double)this.GetValue(VeenessProperty);
            }

            set
            {
                this.SetValue(VeenessProperty, value);
            }
        }

        /// <summary>
        /// Creates the internal annotation object.
        /// </summary>
        /// <returns>
        /// The annotation.
        /// </returns>
        public override OxyPlot.Annotations.Annotation CreateModel()
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
            var a = (OxyPlot.Annotations.ArrowAnnotation)this.InternalAnnotation;

            a.StartPoint = this.StartPoint;
            a.EndPoint = this.EndPoint;
            a.ArrowDirection = this.ArrowDirection;

            a.HeadLength = this.HeadLength;
            a.HeadWidth = this.HeadWidth;
            a.Veeness = this.Veeness;

            a.Color = this.Color.ToOxyColor();
            a.StrokeThickness = this.StrokeThickness;
            a.LineStyle = this.LineStyle;
            a.LineJoin = this.LineJoin;
        }
    }
}