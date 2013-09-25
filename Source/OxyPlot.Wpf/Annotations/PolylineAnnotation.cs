// <copyright file="PolylineAnnotation.cs" company="OxyPlot">
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
namespace OxyPlot.Wpf
{
    using System.Windows;

    /// <summary>
    /// This is a WPF wrapper of OxyPlot.PolylineAnnotation
    /// </summary>
    public class PolylineAnnotation : PathAnnotation
    {
        /// <summary>
        /// The smooth property.
        /// </summary>
        public static readonly DependencyProperty SmoothProperty = DependencyProperty.Register(
            "Smooth", typeof(bool), typeof(PolylineAnnotation), new PropertyMetadata(false, DataChanged));

        /// <summary>
        /// The minimum segment length property
        /// </summary>
        public static readonly DependencyProperty MinimumSegmentLengthProperty =
            DependencyProperty.Register("MinimumSegmentLength", typeof(double), typeof(PolylineAnnotation), new PropertyMetadata(0));

        /// <summary>
        /// Initializes a new instance of the <see cref="PolylineAnnotation"/> class.
        /// </summary>
        public PolylineAnnotation()
        {
            this.InternalAnnotation = new OxyPlot.Annotations.PolylineAnnotation();
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
        /// Gets or sets a value indicating whether the polyline should be smoothed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if smooth; otherwise, <c>false</c>.
        /// </value>
        public bool Smooth
        {
            get { return (bool)this.GetValue(SmoothProperty); }
            set { this.SetValue(SmoothProperty, value); }
        }

        /// <summary>
        /// Creates the internal annotation object.
        /// </summary>
        /// <returns>
        /// The annotation.
        /// </returns>
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

            var a = (OxyPlot.Annotations.PolylineAnnotation)this.InternalAnnotation;

            a.Smooth = this.Smooth;
            a.MinimumSegmentLength = this.MinimumSegmentLength;
        }
    }
}
