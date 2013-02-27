// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineAnnotation.cs" company="OxyPlot">
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
//   This is a WPF wrapper of OxyPlot.LineAnnotation
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    using OxyPlot.Annotations;

    using HorizontalAlignment = OxyPlot.HorizontalAlignment;
    using VerticalAlignment = OxyPlot.VerticalAlignment;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.LineAnnotation
    /// </summary>
    public class LineAnnotation : TextualAnnotation
    {
        /// <summary>
        /// The ClipByXAxis property.
        /// </summary>
        public static readonly DependencyProperty ClipByXAxisProperty = DependencyProperty.Register(
            "ClipByXAxis", typeof(bool), typeof(LineAnnotation), new UIPropertyMetadata(true, AppearanceChanged));

        /// <summary>
        /// The ClipByYAxis property.
        /// </summary>
        public static readonly DependencyProperty ClipByYAxisProperty = DependencyProperty.Register(
            "ClipByYAxis", typeof(bool), typeof(LineAnnotation), new UIPropertyMetadata(true, AppearanceChanged));

        /// <summary>
        /// The color property.
        /// </summary>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof(Color), typeof(LineAnnotation), new PropertyMetadata(Colors.Blue, AppearanceChanged));

        /// <summary>
        /// The equation type property.
        /// </summary>
        public static readonly DependencyProperty EquationTypeProperty = DependencyProperty.Register(
            "Type",
            typeof(LineAnnotationType),
            typeof(LineAnnotation),
            new PropertyMetadata(LineAnnotationType.LinearEquation, DataChanged));

        /// <summary>
        /// The intercept property.
        /// </summary>
        public static readonly DependencyProperty InterceptProperty = DependencyProperty.Register(
            "Intercept", typeof(double), typeof(LineAnnotation), new PropertyMetadata(0.0, DataChanged));

        /// <summary>
        /// The line join property.
        /// </summary>
        public static readonly DependencyProperty LineJoinProperty = DependencyProperty.Register(
            "LineJoin",
            typeof(OxyPenLineJoin),
            typeof(LineAnnotation),
            new UIPropertyMetadata(OxyPenLineJoin.Miter, AppearanceChanged));

        /// <summary>
        /// The line style property.
        /// </summary>
        public static readonly DependencyProperty LineStyleProperty = DependencyProperty.Register(
            "LineStyle",
            typeof(LineStyle),
            typeof(LineAnnotation),
            new PropertyMetadata(LineStyle.Dash, AppearanceChanged));

        /// <summary>
        /// The maximum x property.
        /// </summary>
        public static readonly DependencyProperty MaximumXProperty = DependencyProperty.Register(
            "MaximumX", typeof(double), typeof(LineAnnotation), new PropertyMetadata(double.MaxValue, DataChanged));

        /// <summary>
        /// The maximum y property.
        /// </summary>
        public static readonly DependencyProperty MaximumYProperty = DependencyProperty.Register(
            "MaximumY", typeof(double), typeof(LineAnnotation), new PropertyMetadata(double.MaxValue, DataChanged));

        /// <summary>
        /// The minimum x property.
        /// </summary>
        public static readonly DependencyProperty MinimumXProperty = DependencyProperty.Register(
            "MinimumX", typeof(double), typeof(LineAnnotation), new PropertyMetadata(double.MinValue, DataChanged));

        /// <summary>
        /// The minimum y property.
        /// </summary>
        public static readonly DependencyProperty MinimumYProperty = DependencyProperty.Register(
            "MinimumY", typeof(double), typeof(LineAnnotation), new PropertyMetadata(double.MinValue, DataChanged));

        /// <summary>
        /// The slope property.
        /// </summary>
        public static readonly DependencyProperty SlopeProperty = DependencyProperty.Register(
            "Slope", typeof(double), typeof(LineAnnotation), new PropertyMetadata(0.0, DataChanged));

        /// <summary>
        /// The stroke thickness property.
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(
                "StrokeThickness", typeof(double), typeof(LineAnnotation), new PropertyMetadata(1.0, AppearanceChanged));

        /// <summary>
        /// The TextHorizontalAlignment property.
        /// </summary>
        public static readonly DependencyProperty TextHorizontalAlignmentProperty =
            DependencyProperty.Register(
                "TextHorizontalAlignment",
                typeof(HorizontalAlignment),
                typeof(LineAnnotation),
                new UIPropertyMetadata(OxyPlot.HorizontalAlignment.Right, AppearanceChanged));

        /// <summary>
        /// The TextMargin property.
        /// </summary>
        public static readonly DependencyProperty TextMarginProperty = DependencyProperty.Register(
            "TextMargin", typeof(double), typeof(LineAnnotation), new UIPropertyMetadata(12.0, AppearanceChanged));

        /// <summary>
        /// The TextOrientation property.
        /// </summary>
        public static readonly DependencyProperty TextOrientationProperty =
            DependencyProperty.Register(
                "TextOrientation",
                typeof(AnnotationTextOrientation),
                typeof(LineAnnotation),
                new UIPropertyMetadata(AnnotationTextOrientation.AlongLine, AppearanceChanged));

        /// <summary>
        /// The TextPosition property.
        /// </summary>
        public static readonly DependencyProperty TextPositionProperty = DependencyProperty.Register(
            "TextPosition", typeof(double), typeof(LineAnnotation), new UIPropertyMetadata(1.0, AppearanceChanged));

        /// <summary>
        /// The TextVerticalAlignment property.
        /// </summary>
        public static readonly DependencyProperty TextVerticalAlignmentProperty =
            DependencyProperty.Register(
                "TextVerticalAlignment",
                typeof(VerticalAlignment),
                typeof(LineAnnotation),
                new UIPropertyMetadata(OxyPlot.VerticalAlignment.Top, AppearanceChanged));

        /// <summary>
        /// The x property.
        /// </summary>
        public static readonly DependencyProperty XProperty = DependencyProperty.Register(
            "X", typeof(double), typeof(LineAnnotation), new PropertyMetadata(0.0, DataChanged));

        /// <summary>
        /// The y property.
        /// </summary>
        public static readonly DependencyProperty YProperty = DependencyProperty.Register(
            "Y", typeof(double), typeof(LineAnnotation), new PropertyMetadata(0.0, DataChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref = "LineAnnotation" /> class.
        /// </summary>
        public LineAnnotation()
        {
            this.InternalAnnotation = new OxyPlot.Annotations.LineAnnotation();
        }

        /// <summary>
        /// Gets or sets a value indicating whether to clip the annotation line by the X axis range.
        /// </summary>
        /// <value><c>true</c> if clipping by the X axis is enabled; otherwise, <c>false</c>.</value>
        public bool ClipByXAxis
        {
            get
            {
                return (bool)this.GetValue(ClipByXAxisProperty);
            }

            set
            {
                this.SetValue(ClipByXAxisProperty, value);
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
                return (bool)this.GetValue(ClipByYAxisProperty);
            }

            set
            {
                this.SetValue(ClipByYAxisProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets Color.
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
        /// Gets or sets Intercept.
        /// </summary>
        public double Intercept
        {
            get
            {
                return (double)this.GetValue(InterceptProperty);
            }

            set
            {
                this.SetValue(InterceptProperty, value);
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
        /// Gets or sets MaximumX.
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
        /// Gets or sets MaximumY.
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
        /// Gets or sets MinimumX.
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
        /// Gets or sets MinimumY.
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
        /// Gets or sets Slope.
        /// </summary>
        public double Slope
        {
            get
            {
                return (double)this.GetValue(SlopeProperty);
            }

            set
            {
                this.SetValue(SlopeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets StrokeThickness.
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
        /// Gets or sets the text margin (along the line).
        /// </summary>
        /// <value>The text margin.</value>
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
        /// Gets or sets the text orientation.
        /// </summary>
        /// <value>The text orientation.</value>
        public AnnotationTextOrientation TextOrientation
        {
            get
            {
                return (AnnotationTextOrientation)this.GetValue(TextOrientationProperty);
            }

            set
            {
                this.SetValue(TextOrientationProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the text position fraction.
        /// </summary>
        /// <value>The text position in the interval [0,1].</value>
        /// <remarks>
        /// Positions smaller than 0.25 are left aligned at the start of the line
        /// Positions larger than 0.75 are right aligned at the end of the line
        /// Other positions are center aligned at the specified position
        /// </remarks>
        public double TextPosition
        {
            get
            {
                return (double)this.GetValue(TextPositionProperty);
            }

            set
            {
                this.SetValue(TextPositionProperty, value);
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
        /// Gets or sets Type.
        /// </summary>
        public LineAnnotationType Type
        {
            get
            {
                return (LineAnnotationType)this.GetValue(EquationTypeProperty);
            }

            set
            {
                this.SetValue(EquationTypeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets X.
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
        /// Gets or sets Y.
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
            var a = (OxyPlot.Annotations.LineAnnotation)this.InternalAnnotation;
            a.Type = this.Type;
            a.Slope = this.Slope;
            a.Intercept = this.Intercept;
            a.X = this.X;
            a.Y = this.Y;

            a.MinimumX = this.MinimumX;
            a.MaximumX = this.MaximumX;
            a.MinimumY = this.MinimumY;
            a.MaximumY = this.MaximumY;

            a.Color = this.Color.ToOxyColor();
            a.StrokeThickness = this.StrokeThickness;
            a.LineStyle = this.LineStyle;
            a.LineJoin = this.LineJoin;

            a.ClipByXAxis = this.ClipByXAxis;
            a.ClipByYAxis = this.ClipByYAxis;

            a.TextPosition = this.TextPosition;
            a.TextOrientation = this.TextOrientation;
            a.TextMargin = this.TextMargin;
            a.TextHorizontalAlignment = this.TextHorizontalAlignment;
            a.TextVerticalAlignment = this.TextVerticalAlignment;
        }
    }
}