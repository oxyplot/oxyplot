// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PolygonAnnotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a WPF wrapper of OxyPlot.PolygonAnnotation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System.Collections.Generic;
    using System.Windows;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.PolygonAnnotation
    /// </summary>
    public class PolygonAnnotation : ShapeAnnotation
    {
        /// <summary>
        /// Identifies the <see cref="LineJoin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LineJoinProperty = DependencyProperty.Register(
            "LineJoin", typeof(LineJoin), typeof(PolygonAnnotation), new UIPropertyMetadata(LineJoin.Miter, AppearanceChanged));

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

            a.LineStyle = this.LineStyle;
            a.LineJoin = this.LineJoin;
        }
    }
}