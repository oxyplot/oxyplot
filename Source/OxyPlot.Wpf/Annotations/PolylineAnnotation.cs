// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PolylineAnnotation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   This is a WPF wrapper of OxyPlot.PolylineAnnotation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.PolylineAnnotation
    /// </summary>
    public class PolylineAnnotation : PathAnnotation
    {
        /// <summary>
        /// Identifies the <see cref="InterpolationAlgorithm"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty InterpolationAlgorithmProperty = DependencyProperty.Register(
            "InterpolationAlgorithm", typeof(IInterpolationAlgorithm), typeof(PolylineAnnotation), new PropertyMetadata(null, DataChanged));

        /// <summary>
        /// Identifies the <see cref="MinimumSegmentLength"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinimumSegmentLengthProperty =
            DependencyProperty.Register("MinimumSegmentLength", typeof(double), typeof(PolylineAnnotation), new PropertyMetadata(0d));

        /// <summary>
        /// Identifies the <see cref="Points"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof(IList<DataPoint>), typeof(PolylineAnnotation), new PropertyMetadata(new List<DataPoint>()));

        /// <summary>
        /// Initializes static members of the <see cref="PolylineAnnotation"/> class.
        /// </summary>
        static PolylineAnnotation()
        {
            TextColorProperty.OverrideMetadata(typeof(PolylineAnnotation), new FrameworkPropertyMetadata(MoreColors.Automatic, AppearanceChanged));
            TextHorizontalAlignmentProperty.OverrideMetadata(typeof(PolylineAnnotation), new FrameworkPropertyMetadata(HorizontalAlignment.Right, AppearanceChanged));
            TextVerticalAlignmentProperty.OverrideMetadata(typeof(PolylineAnnotation), new FrameworkPropertyMetadata(VerticalAlignment.Top, AppearanceChanged));
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
                return (IList<DataPoint>)this.GetValue(PointsProperty);
            }

            set
            {
                this.SetValue(PointsProperty, value);
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
            get { return (double)this.GetValue(MinimumSegmentLengthProperty); }
            set { this.SetValue(MinimumSegmentLengthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the interpolation algorithm.
        /// </summary>
        /// <value>Interpolation algorithm.</value>
        public IInterpolationAlgorithm InterpolationAlgorithm
        {
            get { return (IInterpolationAlgorithm)this.GetValue(InterpolationAlgorithmProperty); }
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