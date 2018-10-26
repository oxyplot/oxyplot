using System;
using System.Collections.Generic;

namespace OxyPlot.Wpf
{
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Shapes;

    /// <summary>
    /// A re-implementation of <see cref="T:System.Windows.Shapes.Polyline"/> that prevents infinity size being returned.
    /// </summary>
    public class Polyline2 : Shape
    {
        /// <summary>Identifies the <see cref="Polyline2.Points" /> dependency property. </summary>
        /// <returns>The identifier for the <see cref="Polyline2.Points" /> dependency property.</returns>
        public static readonly DependencyProperty PointsProperty = DependencyProperty.Register(nameof(Points), typeof(PointCollection), typeof(Polyline2), (PropertyMetadata)new FrameworkPropertyMetadata(new PointCollection(), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>Identifies the <see cref="Polyline2.FillRule" /> dependency property. </summary>
        /// <returns>The identifier for the <see cref="Polyline2.FillRule" /> dependency property.</returns>
        public static readonly DependencyProperty FillRuleProperty = DependencyProperty.Register(nameof(FillRule), typeof(FillRule), typeof(Polyline2), (PropertyMetadata)new FrameworkPropertyMetadata((object)FillRule.EvenOdd, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>Gets or sets a collection that contains the vertex points of the <see cref="Polyline2" />.  </summary>
        /// <returns>A collection of <see cref="T:System.Windows.Point" /> structures that describe the vertex points of the <see cref="Polyline2" />. The default is a null  reference (Nothing in Visual Basic).</returns>
        public PointCollection Points
        {
            get
            {
                return (PointCollection)this.GetValue(Polyline2.PointsProperty);
            }
            set
            {
                var prev = (PointCollection)this.GetValue(Polyline2.PointsProperty);
                if (null != prev && !prev.IsFrozen)
                {
                    prev.Changed -= this.OnPointsChanged;
                }

                this.SetValue(Polyline2.PointsProperty, (object)value);
                if (null != value && !value.IsFrozen)
                {
                    value.Changed += this.OnPointsChanged;
                }
            }
        }

        /// <summary>Gets or sets a <see cref="T:System.Windows.Media.FillRule" /> enumeration that specifies how the interior fill of the shape is determined.  </summary>
        /// <returns>One of the <see cref="T:System.Windows.Media.FillRule" /> enumeration values. The default is <see cref="F:System.Windows.Media.FillRule.EvenOdd" />.</returns>
        public FillRule FillRule
        {
            get
            {
                return (FillRule)this.GetValue(Polyline2.FillRuleProperty);
            }
            set
            {
                this.SetValue(Polyline2.FillRuleProperty, (object)value);
            }
        }

        private void OnPointsChanged(object sender, EventArgs args)
        {
            this._cachedGeometry = null;
        }

        /// <summary>Measures a <see cref="Polyline2" /> during the first layout pass prior to arranging it.</summary>
        /// <param name="constraint">A maximum <see cref="T:System.Windows.Size" /> to not exceed.</param>
        /// <returns>The maximum <see cref="T:System.Windows.Size" /> for the <see cref="Polyline2" />.</returns>
        protected override Size MeasureOverride(Size constraint)
        {
            var size = base.MeasureOverride(constraint);
            if (double.IsInfinity(size.Height))
            {
                if (double.IsInfinity(constraint.Height))
                {
                    size.Height = 0;
                }
                else
                {
                    size.Height = constraint.Height;
                }
            }

            if (double.IsInfinity(size.Width))
            {
                if (double.IsInfinity(constraint.Width))
                {
                    size.Width = 0;
                }
                else
                {
                    size.Width = constraint.Width;
                }
            }

            return size;
        }

        private Geometry _cachedGeometry = null;

        /// <summary>Gets a value that represents the <see cref="T:System.Windows.Media.Geometry" /> of the <see cref="Polyline2" />.</summary>
        /// <returns>The <see cref="T:System.Windows.Media.Geometry" /> of the <see cref="Polyline2" />.</returns>
        protected override Geometry DefiningGeometry
        {
            get
            {
                if (null != this._cachedGeometry)
                    return this._cachedGeometry;

                PointCollection points = this.Points;
                PathFigure pathFigure = new PathFigure();
                if (points == null)
                {
                    this._cachedGeometry = Geometry.Empty;
                }
                else
                {
                    if (points.Count > 0)
                    {
                        pathFigure.StartPoint = points[0];
                        if (points.Count > 1)
                        {
                            Point[] pointArray = new Point[points.Count - 1];
                            for (int index = 1; index < points.Count; ++index)
                                pointArray[index - 1] = points[index];
                            pathFigure.Segments.Add((PathSegment)new PolyLineSegment((IEnumerable<Point>)pointArray, true));
                        }
                    }
                    PathGeometry pathGeometry = new PathGeometry();
                    pathGeometry.Figures.Add(pathFigure);
                    pathGeometry.FillRule = this.FillRule;
                    if (pathGeometry.Bounds == Rect.Empty)
                        this._cachedGeometry = Geometry.Empty;
                    else
                        this._cachedGeometry = (Geometry)pathGeometry;
                }

                return this._cachedGeometry;
            }
        }

        internal void CacheDefiningGeometry()
        {

        }
    }
}
