// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PolygonAnnotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a Avalonia wrapper of OxyPlot.PolygonAnnotation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    using System.Collections.Generic;

    /// <summary>
    /// This is a Avalonia wrapper of OxyPlot.PolygonAnnotation
    /// </summary>
    public class PolygonAnnotation : ShapeAnnotation
    {
        /// <summary>
        /// Identifies the <see cref="LineJoin"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<LineJoin> LineJoinProperty = AvaloniaProperty.Register<PolygonAnnotation, LineJoin>(nameof(LineJoin), LineJoin.Miter);

        /// <summary>
        /// Identifies the <see cref="LineStyle"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<LineStyle> LineStyleProperty = AvaloniaProperty.Register<PolygonAnnotation, LineStyle>(nameof(LineStyle), LineStyle.Solid);

        /// <summary>
        /// Identifies the <see cref="Points"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<IList<DataPoint>> PointsProperty = AvaloniaProperty.Register<PolygonAnnotation, IList<DataPoint>>(nameof(Points), new List<DataPoint>());

        /// <summary>
        /// Initializes static members of the <see cref="PolygonAnnotation"/> class.
        /// </summary>
        static PolygonAnnotation()
        {
            TextColorProperty.OverrideDefaultValue<PolygonAnnotation>(MoreColors.Automatic);
            TextColorProperty.Changed.AddClassHandler<PolygonAnnotation>(AppearanceChanged);

            LineJoinProperty.Changed.AddClassHandler<PolygonAnnotation>(AppearanceChanged);
            LineStyleProperty.Changed.AddClassHandler<PolygonAnnotation>(AppearanceChanged);
            PointsProperty.Changed.AddClassHandler<PolygonAnnotation>(DataChanged);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "PolygonAnnotation" /> class.
        /// </summary>
        public PolygonAnnotation()
        {
            InternalAnnotation = new Annotations.PolygonAnnotation();
        }

        /// <summary>
        /// Gets or sets the line join.
        /// </summary>
        /// <value>The line join.</value>
        public LineJoin LineJoin
        {
            get
            {
                return GetValue(LineJoinProperty);
            }

            set
            {
                SetValue(LineJoinProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the line style.
        /// </summary>
        public LineStyle LineStyle
        {
            get
            {
                return GetValue(LineStyleProperty);
            }

            set
            {
                SetValue(LineStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        public IList<DataPoint> Points
        {
            get
            {
                return GetValue(PointsProperty);
            }

            set
            {
                SetValue(PointsProperty, value);
            }
        }

        /// <summary>
        /// Creates the internal annotation object.
        /// </summary>
        /// <returns>The annotation.</returns>
        public override Annotations.Annotation CreateModel()
        {
            SynchronizeProperties();
            return InternalAnnotation;
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        public override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            var a = (Annotations.PolygonAnnotation)InternalAnnotation;
            a.Points.Clear();
            a.Points.AddRange(Points);

            a.LineStyle = LineStyle;
            a.LineJoin = LineJoin;
        }
    }
}