// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PolygonAnnotation.cs" company="OxyPlot">
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
//   This is a WPF wrapper of OxyPlot.PolygonAnnotation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.PolygonAnnotation
    /// </summary>
    public class PolygonAnnotation : TextualAnnotation
    {
        /// <summary>
        /// Identifies the <see cref="Color"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof(Color), typeof(PolygonAnnotation), new PropertyMetadata(Colors.Blue, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="Fill"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
            "Fill", typeof(Color), typeof(PolygonAnnotation), new PropertyMetadata(Colors.LightBlue, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LineJoin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LineJoinProperty = DependencyProperty.Register(
            "LineJoin", typeof(OxyPenLineJoin), typeof(PolygonAnnotation), new UIPropertyMetadata(OxyPenLineJoin.Miter, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LineStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LineStyleProperty = DependencyProperty.Register(
            "LineStyle", typeof(LineStyle), typeof(PolygonAnnotation), new PropertyMetadata(LineStyle.Solid, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="Points"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PointsProperty = DependencyProperty.Register(
            "Points", typeof(IList<DataPoint>), typeof(PolygonAnnotation), new PropertyMetadata(new List<DataPoint>(), DataChanged));

        /// <summary>
        /// Identifies the <see cref="StrokeThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(
                "StrokeThickness", typeof(double), typeof(PolygonAnnotation), new PropertyMetadata(1.0, AppearanceChanged));


        /// <summary>
        /// Initializes static members of the <see cref="PolygonAnnotation"/> class.
        /// </summary>
        static PolygonAnnotation()
        {
            TextColorProperty.OverrideMetadata(typeof(PolygonAnnotation), new FrameworkPropertyMetadata(MoreColors.Automatic, AppearanceChanged));
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref = "PolygonAnnotation" /> class.
        /// </summary>
        public PolygonAnnotation()
        {
            this.InternalAnnotation = new Annotations.PolygonAnnotation();
        }

        /// <summary>
        /// Gets or sets the stroke color.
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
        /// Gets or sets the fill color.
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
        /// Gets or sets the line style.
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
        /// Gets or sets the points.
        /// </summary>
        public IList<DataPoint> Points
        {
            get
            {
                return (IList<DataPoint>)this.GetValue(PointsProperty);
            }

            set
            {
                this.SetValue(PointsProperty, value);
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
            var a = (Annotations.PolygonAnnotation)this.InternalAnnotation;
            a.Points.Clear();
            a.Points.AddRange(this.Points);

            a.Fill = this.Fill.ToOxyColor();
            a.Color = this.Color.ToOxyColor();
            a.StrokeThickness = this.StrokeThickness;
            a.LineStyle = this.LineStyle;
            a.LineJoin = this.LineJoin;
        }
    }
}