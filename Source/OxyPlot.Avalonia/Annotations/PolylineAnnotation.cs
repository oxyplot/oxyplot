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
        /// Identifies the <see cref="InterpolationAlgorithm"/> dependency property.
        /// </summary>
        public static readonly StyledProperty<IInterpolationAlgorithm> InterpolationAlgorithmProperty = AvaloniaProperty.Register<PolylineAnnotation, IInterpolationAlgorithm>(nameof(InterpolationAlgorithm), null);

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

            InterpolationAlgorithmProperty.Changed.AddClassHandler<PolylineAnnotation>(DataChanged);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolylineAnnotation" /> class.
        /// </summary>
        public PolylineAnnotation()
        {
            this.InternalAnnotation = new Annotations.PolylineAnnotation();
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
        /// Gets or sets the interpolation algorithm.
        /// </summary>
        /// <value>Interpolation algorithm.</value>
        public IInterpolationAlgorithm InterpolationAlgorithm
        {
            get { return this.GetValue(InterpolationAlgorithmProperty); }
            set { this.SetValue(InterpolationAlgorithmProperty, value); }
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

            var a = (Annotations.PolylineAnnotation)this.InternalAnnotation;
            a.Points.Clear();
            a.Points.AddRange(this.Points);
            a.InterpolationAlgorithm = this.InterpolationAlgorithm;
            a.MinimumSegmentLength = this.MinimumSegmentLength;
        }
    }
}