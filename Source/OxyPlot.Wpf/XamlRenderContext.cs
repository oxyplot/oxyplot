// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XamlRenderContext.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;

    using Path = System.Windows.Shapes.Path;

    /// <summary>
    /// Implements <see cref="IRenderContext" /> for <see cref="Canvas" />. This does not use <see cref="StreamGeometry"/> and therefore the output can be serialized to XAML.
    /// </summary>
    public class XamlRenderContext : CanvasRenderContext
    {
        /// <summary>
        /// The maximum number of figures per geometry.
        /// </summary>
        private const int MaxFiguresPerGeometry = 16;

        /// <summary>
        /// The maximum number of polylines per line.
        /// </summary>
        private const int MaxPolylinesPerLine = 64;

        /// <summary>
        /// The minimum number of points per polyline.
        /// </summary>
        private const int MinPointsPerPolyline = 16;

        /// <summary>
        /// Initializes a new instance of the <see cref="CanvasRenderContext" /> class.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        public XamlRenderContext(Canvas canvas) : base(canvas)
        {
            this.RendersToScreen = false;
            this.BalancedLineDrawingThicknessLimit = 3.5;
        }

        /// <summary>
        /// Gets or sets the thickness limit for "balanced" line drawing.
        /// </summary>
        public double BalancedLineDrawingThicknessLimit { get; set; }

        ///<inheritdoc/>
        public override void DrawEllipses(IList<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode)
        {
            if (rectangles.Count == 0)
            {
                return;
            }

            var path = this.CreateAndAdd<Path>();
            this.SetStroke(path, stroke, thickness, edgeRenderingMode);
            if (!fill.IsUndefined())
            {
                path.Fill = this.GetCachedBrush(fill);
            }

            var gg = new GeometryGroup { FillRule = FillRule.Nonzero };
            foreach (var rect in rectangles)
            {
                gg.Children.Add(new EllipseGeometry(ToRect(rect)));
            }

            path.Data = gg;
        }

        ///<inheritdoc/>
        public override void DrawLine(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray,
            LineJoin lineJoin)
        {
            if (points.Count < 2)
            {
                return;
            }

            if (thickness < this.BalancedLineDrawingThicknessLimit)
            {
                this.DrawLineBalanced(points, stroke, thickness, edgeRenderingMode, dashArray, lineJoin);
                return;
            }

            var e = this.CreateAndAdd<Polyline>();
            this.SetStroke(e, stroke, thickness, edgeRenderingMode, lineJoin, dashArray, 0);
            e.Points = this.ToPointCollection(points, e.StrokeThickness, edgeRenderingMode);
        }

        ///<inheritdoc/>
        public override void DrawLineSegments(
            IList<ScreenPoint> points,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray,
            LineJoin lineJoin)
        {
            if (points.Count < 2)
            {
                return;
            }

            Path path = null;
            PathGeometry pathGeometry = null;

            var count = 0;

            for (var i = 0; i + 1 < points.Count; i += 2)
            {
                if (path == null)
                {
                    path = this.CreateAndAdd<Path>();
                    this.SetStroke(path, stroke, thickness, edgeRenderingMode, lineJoin, dashArray, 0);
                    pathGeometry = new PathGeometry();
                }

                var actualPoints = this.GetActualPoints( new[] { points[i], points[i + 1] }, path.StrokeThickness, edgeRenderingMode).ToList();

                var figure = new PathFigure { StartPoint = actualPoints[0], IsClosed = false };
                figure.Segments.Add(new LineSegment(actualPoints[1], true) { IsSmoothJoin = false });
                pathGeometry.Figures.Add(figure);

                count++;

                // Must limit the number of figures, otherwise drawing errors...
                if (count > MaxFiguresPerGeometry || dashArray != null)
                {
                    path.Data = pathGeometry;
                    path = null;
                    count = 0;
                }
            }

            if (path != null)
            {
                path.Data = pathGeometry;
            }
        }

        ///<inheritdoc/>
        public override void DrawPolygons(
            IList<IList<ScreenPoint>> polygons,
            OxyColor fill,
            OxyColor stroke,
            double thickness,
            EdgeRenderingMode edgeRenderingMode,
            double[] dashArray,
            LineJoin lineJoin)
        {
            if (polygons.Count == 0)
            {
                return;
            }

            Path path = null;
            PathGeometry pathGeometry = null;
            var count = 0;

            foreach (var polygon in polygons)
            {
                if (path == null)
                {
                    path = this.CreateAndAdd<Path>();
                    this.SetStroke(path, stroke, thickness, edgeRenderingMode, lineJoin, dashArray, 0);
                    if (!fill.IsUndefined())
                    {
                        path.Fill = this.GetCachedBrush(fill);
                    }

                    pathGeometry = new PathGeometry { FillRule = FillRule.Nonzero };
                }

                PathFigure figure = null;
                var first = true;
                foreach (var point in this.GetActualPoints(polygon, path.StrokeThickness, edgeRenderingMode))
                {
                    if (first)
                    {
                        figure = new PathFigure
                        {
                            StartPoint = point,
                            IsFilled = !fill.IsUndefined(),
                            IsClosed = true
                        };

                        pathGeometry.Figures.Add(figure);
                        first = false;
                    }
                    else
                    {
                        figure.Segments.Add(new LineSegment(point, !stroke.IsUndefined()));
                    }
                }

                count++;

                // Must limit the number of figures, otherwise drawing errors...
                if (count > MaxFiguresPerGeometry)
                {
                    path.Data = pathGeometry;
                    path = null;
                    count = 0;
                }
            }

            if (path != null)
            {
                path.Data = pathGeometry;
            }
        }

        ///<inheritdoc/>
        public override void DrawRectangles(IList<OxyRect> rectangles, OxyColor fill, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode)
        {
            if (rectangles.Count == 0)
            {
                return;
            }

            var path = this.CreateAndAdd<Path>();
            this.SetStroke(path, stroke, thickness, edgeRenderingMode);
            if (!fill.IsUndefined())
            {
                path.Fill = this.GetCachedBrush(fill);
            }

            var gg = new GeometryGroup { FillRule = FillRule.Nonzero };
            foreach (var rect in rectangles)
            {
                gg.Children.Add(new RectangleGeometry { Rect = this.GetActualRect(rect, path.StrokeThickness, edgeRenderingMode) });
            }

            path.Data = gg;
        }

        /// <summary>
        /// Draws the line using the MaxPolylinesPerLine and MinPointsPerPolyline properties.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="stroke">The stroke color.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <param name="dashArray">The dash array. Use <c>null</c> to get a solid line.</param>
        /// <param name="lineJoin">The line join.</param>
        private void DrawLineBalanced(IList<ScreenPoint> points, OxyColor stroke, double thickness, EdgeRenderingMode edgeRenderingMode, double[] dashArray, LineJoin lineJoin)
        {
            // balance the number of points per polyline and the number of polylines
            var numPointsPerPolyline = Math.Max(points.Count / MaxPolylinesPerLine, MinPointsPerPolyline);

            var polyline = this.CreateAndAdd<Polyline>();
            this.SetStroke(polyline, stroke, thickness, edgeRenderingMode, lineJoin, dashArray, 0);
            var pc = new PointCollection(numPointsPerPolyline);

            var n = points.Count;
            double lineLength = 0;
            var dashPatternLength = (dashArray != null) ? dashArray.Sum() : 0;
            var last = new Point();
            var i = 0;
            foreach (var p in this.GetActualPoints(points, thickness, edgeRenderingMode))
            {
                pc.Add(p);

                if (dashArray != null)
                {
                    if (i > 0)
                    {
                        var delta = p - last;
                        var dist = Math.Sqrt((delta.X * delta.X) + (delta.Y * delta.Y));
                        lineLength += dist;
                    }

                    last = p;
                }

                // use multiple polylines with limited number of points to improve WPF performance
                if (pc.Count >= numPointsPerPolyline)
                {
                    polyline.Points = pc;

                    if (i < n - 1)
                    {
                        // start a new polyline at last point so there is no gap (it is not necessary to use the % operator)
                        var dashOffset = dashPatternLength > 0 ? lineLength / thickness : 0;
                        polyline = this.CreateAndAdd<Polyline>();
                        this.SetStroke(polyline, stroke, thickness, edgeRenderingMode, lineJoin, dashArray, dashOffset);
                        pc = new PointCollection(numPointsPerPolyline) { pc.Last() };
                    }
                }

                i++;
            }

            if (pc.Count > 1 || n == 1)
            {
                polyline.Points = pc;
            }
        }

        /// <summary>
        /// Creates a point collection from the specified points. The points are snapped to pixels if required by the edge rendering mode,
        /// </summary>
        /// <param name="points">The points to convert.</param>
        /// <param name="strokeThickness">The stroke thickness.</param>
        /// <param name="edgeRenderingMode">The edge rendering mode.</param>
        /// <returns>The point collection.</returns>
        private PointCollection ToPointCollection(IList<ScreenPoint> points, double strokeThickness, EdgeRenderingMode edgeRenderingMode)
        {
            return new PointCollection(this.GetActualPoints(points, strokeThickness, edgeRenderingMode));
        }
    }
}
