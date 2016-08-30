// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PathAnnotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a WPF wrapper of OxyPlot.PathAnnotation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Media;

    using OxyPlot.Annotations;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.PathAnnotation
    /// </summary>
    public abstract class PathAnnotation : TextualAnnotation
    {
        /// <summary>
        /// Identifies the <see cref="Color"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof(Color), typeof(PathAnnotation), new PropertyMetadata(Colors.Blue, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="ClipByXAxis"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ClipByXAxisProperty = DependencyProperty.Register(
            "ClipByXAxis", typeof(bool), typeof(PathAnnotation), new UIPropertyMetadata(true, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="ClipByYAxis"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ClipByYAxisProperty = DependencyProperty.Register(
            "ClipByYAxis", typeof(bool), typeof(PathAnnotation), new UIPropertyMetadata(true, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="ClipText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ClipTextProperty =
            DependencyProperty.Register("ClipText", typeof(bool), typeof(PathAnnotation), new PropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="LineJoin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LineJoinProperty = DependencyProperty.Register(
            "LineJoin",
            typeof(LineJoin),
            typeof(PathAnnotation),
            new UIPropertyMetadata(LineJoin.Miter, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LineStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LineStyleProperty = DependencyProperty.Register(
            "LineStyle",
            typeof(LineStyle),
            typeof(PathAnnotation),
            new PropertyMetadata(LineStyle.Dash, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="StrokeThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(
                "StrokeThickness", typeof(double), typeof(PathAnnotation), new PropertyMetadata(1.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TextMargin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextMarginProperty = DependencyProperty.Register(
            "TextMargin", typeof(double), typeof(PathAnnotation), new UIPropertyMetadata(12.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TextOrientation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextOrientationProperty =
            DependencyProperty.Register(
                "TextOrientation",
                typeof(AnnotationTextOrientation),
                typeof(PathAnnotation),
                new UIPropertyMetadata(AnnotationTextOrientation.AlongLine, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="TextLinePosition"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextLinePositionProperty = DependencyProperty.Register(
            "TextLinePosition", typeof(double), typeof(PathAnnotation), new UIPropertyMetadata(1.0, AppearanceChanged));

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
        /// Gets or sets a value indicating whether the text should be clipped within the plot area.
        /// </summary>
        /// <value><c>true</c> if text should be clipped; otherwise, <c>false</c>.</value>
        public bool ClipText
        {
            get
            {
                return (bool)this.GetValue(ClipTextProperty);
            }

            set
            {
                this.SetValue(ClipTextProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the annotation color.
        /// </summary>
        /// <value>The color.</value>
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
        /// Gets or sets the line join.
        /// </summary>
        /// <value>The line join.</value>
        public LineJoin LineJoin
        {
            get
            {
                return (LineJoin)this.GetValue(LineJoinProperty);
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
        /// Gets or sets the text position relative to the line.
        /// </summary>
        /// <value>The text position in the interval [0,1].</value>
        /// <remarks>Positions smaller than 0.25 are left aligned at the start of the line
        /// Positions larger than 0.75 are right aligned at the end of the line
        /// Other positions are center aligned at the specified position</remarks>
        public double TextLinePosition
        {
            get
            {
                return (double)this.GetValue(TextLinePositionProperty);
            }

            set
            {
                this.SetValue(TextLinePositionProperty, value);
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

            var a = (Annotations.PathAnnotation)this.InternalAnnotation;
            a.Color = this.Color.ToOxyColor();

            a.ClipByXAxis = this.ClipByXAxis;
            a.ClipByYAxis = this.ClipByYAxis;

            a.StrokeThickness = this.StrokeThickness;
            a.LineStyle = this.LineStyle;
            a.LineJoin = this.LineJoin;

            a.TextLinePosition = this.TextLinePosition;
            a.TextOrientation = this.TextOrientation;
            a.TextMargin = this.TextMargin;
            a.ClipText = this.ClipText;
        }
    }
}