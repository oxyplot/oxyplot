// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PolylineAnnotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a Avalonia wrapper of OxyPlot.PolylineAnnotation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Avalonia;

namespace OxyPlot.Avalonia
{
    using global::Avalonia.Layout;
    using System.Collections.Generic;

    /// <summary>
    /// This is a Avalonia wrapper of OxyPlot.PolylineAnnotation
    /// </summary>
    public class PolylineAnnotation : PathAnnotation
    {
        /// <summary>
        /// Identifies the <see cref="Smooth"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<bool> SmoothProperty = AvaloniaProperty.Register<PolylineAnnotation, bool>(nameof(Smooth), false);

        /// <summary>
        /// Identifies the <see cref="MinimumSegmentLength"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<double> MinimumSegmentLengthProperty = AvaloniaProperty.Register<PolylineAnnotation, double>(nameof(MinimumSegmentLength), 0d);

        /// <summary>
        /// Identifies the <see cref="Points"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<IList<DataPoint>> PointsProperty = AvaloniaProperty.Register<PolylineAnnotation, IList<DataPoint>>(nameof(Points), new List<DataPoint>());

        /// <summary>
        /// Initializes static members of the <see cref="PolylineAnnotation"/> class.
        /// </summary>
        static PolylineAnnotation()
        {
            TextColorProperty.OverrideDefaultValue<PolylineAnnotation>(MoreColors.Automatic);
            TextColorProperty.Changed.AddClassHandler<PolylineAnnotation>(AppearanceChanged);
            TextHorizontalAlignmentProperty.OverrideDefaultValue<PolylineAnnotation>(HorizontalAlignment.Right);
            TextHorizontalAlignmentProperty.Changed.AddClassHandler<PolylineAnnotation>(AppearanceChanged);
            TextVerticalAlignmentProperty.OverrideDefaultValue<PolylineAnnotation>(VerticalAlignment.Top);
            TextVerticalAlignmentProperty.Changed.AddClassHandler<PolylineAnnotation>(AppearanceChanged);

            SmoothProperty.Changed.AddClassHandler<PolylineAnnotation>(DataChanged);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolylineAnnotation" /> class.
        /// </summary>
        public PolylineAnnotation()
        {
            InternalAnnotation = new Annotations.PolylineAnnotation();
        }

        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        /// <value>The points.</value>
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
        /// Gets or sets the minimum length of the segment.
        /// Increasing this number will increase performance,
        /// but make the curve less accurate.
        /// </summary>
        /// <value>The minimum length of the segment.</value>
        public double MinimumSegmentLength
        {
            get { return GetValue(MinimumSegmentLengthProperty); }
            set { SetValue(MinimumSegmentLengthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the polyline should be smoothed.
        /// </summary>
        /// <value><c>true</c> if smooth; otherwise, <c>false</c>.</value>
        public bool Smooth
        {
            get { return GetValue(SmoothProperty); }
            set { SetValue(SmoothProperty, value); }
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

            var a = (Annotations.PolylineAnnotation)InternalAnnotation;
            a.Points.Clear();
            a.Points.AddRange(Points);
            a.Smooth = Smooth;
            a.MinimumSegmentLength = MinimumSegmentLength;
        }
    }
}