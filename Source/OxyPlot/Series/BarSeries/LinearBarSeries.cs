// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinearBarSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a series to display bars in a linear axis
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a series to display bars in a linear axis
    /// </summary>
    public class LinearBarSeries : DataPointSeries
    {
        /// <summary>
        /// The rendered rectangles.
        /// </summary>
        private readonly List<OxyRect> rectangles = new List<OxyRect>();

        /// <summary>
        /// The indexes matching rendered rectangles.
        /// </summary>
        private readonly List<int> rectanglesPointIndexes = new List<int>();

        /// <summary>
        /// The default color.
        /// </summary>
        private OxyColor defaultColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearBarSeries" /> class.
        /// </summary>
        public LinearBarSeries()
        {
            this.FillColor = OxyColors.Automatic;
            this.BarWidth = 5;
            this.StrokeColor = OxyColors.Black;
            this.StrokeThickness = 0;
            this.TrackerFormatString = XYAxisSeries.DefaultTrackerFormatString;
            this.NegativeFillColor = OxyColors.Undefined;
            this.NegativeStrokeColor = OxyColors.Undefined;
        }

        /// <summary>
        /// Gets or sets the color of the interior of the bars.
        /// </summary>
        /// <value>The color.</value>
        public OxyColor FillColor { get; set; }

        /// <summary>
        /// Gets or sets the width of the bars.
        /// </summary>
        /// <value>The width of the bars.</value>
        public double BarWidth { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the curve.
        /// </summary>
        /// <value> The stroke thickness.</value>
        public double StrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the color of the border around the bars.
        /// </summary>
        /// <value>The color of the stroke.</value>
        public OxyColor StrokeColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the interior of the bars when the value is negative.
        /// </summary>
        /// <value>The color.</value>
        public OxyColor NegativeFillColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the border around the bars when the value is negative.
        /// </summary>
        /// <value>The color of the stroke.</value>
        public OxyColor NegativeStrokeColor { get; set; }

        /// <summary>
        /// Gets the actual color.
        /// </summary>
        /// <value>The actual color.</value>
        public OxyColor ActualColor
        {
            get
            {
                return this.FillColor.GetActualColor(this.defaultColor);
            }
        }

        /// <summary>
        /// Gets the nearest point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="interpolate">interpolate if set to <c>true</c> .</param>
        /// <returns>A TrackerHitResult for the current hit.</returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            var rectangleIndex = this.FindRectangleIndex(point);
            if (rectangleIndex < 0)
            {
                return null;
            }

            var rectangle = this.rectangles[rectangleIndex];
            if (!rectangle.Contains(point))
            {
                return null;
            }

            var pointIndex = this.rectanglesPointIndexes[rectangleIndex];
            var dataPoint = this.ActualPoints[pointIndex];
            var item = this.GetItem(pointIndex);

            // Format: {0}\n{1}: {2}\n{3}: {4}
            var trackerParameters = new[]
            {
                this.Title,
                this.XAxis.Title ?? "X",
                this.XAxis.GetValue(dataPoint.X), 
                this.YAxis.Title ?? "Y", 
                this.YAxis.GetValue(dataPoint.Y),
            };

            var text = StringHelper.Format(this.ActualCulture, this.TrackerFormatString, item, trackerParameters);

            return new TrackerHitResult
            {
                Series = this,
                DataPoint = dataPoint,
                Position = point,
                Item = item,
                Index = pointIndex,
                Text = text,
            };
        }

        /// <summary>
        /// Renders the series on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        public override void Render(IRenderContext rc)
        {
            this.rectangles.Clear();
            this.rectanglesPointIndexes.Clear();

            var actualPoints = this.ActualPoints;
            if (actualPoints == null || actualPoints.Count == 0)
            {
                return;
            }

            this.VerifyAxes();

            var clippingRect = this.GetClippingRect();
            rc.SetClip(clippingRect);

            this.RenderBars(rc, clippingRect, actualPoints);

            rc.ResetClip();
        }

        /// <summary>
        /// Renders the legend symbol for the line series on the
        /// specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="legendBox">The bounding rectangle of the legend box.</param>
        public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
            var xmid = (legendBox.Left + legendBox.Right) / 2;
            var ymid = (legendBox.Top + legendBox.Bottom) / 2;
            var height = (legendBox.Bottom - legendBox.Top) * 0.8;
            var width = height;
            rc.DrawRectangle(
                new OxyRect(xmid - (0.5 * width), ymid - (0.5 * height), width, height),
                this.GetSelectableColor(this.ActualColor),
                this.StrokeColor,
                this.StrokeThickness,
                this.EdgeRenderingMode);
        }

        /// <summary>
        /// Sets default values from the plot model.
        /// </summary>
        protected internal override void SetDefaultValues()
        {
            if (this.FillColor.IsAutomatic())
            {
                this.defaultColor = this.PlotModel.GetDefaultColor();
            }
        }

        /// <summary>
        /// Updates the axes to include the max and min of this series.
        /// </summary>
        protected internal override void UpdateAxisMaxMin()
        {
            base.UpdateAxisMaxMin();

            this.YAxis.Include(0.0);
        }

        /// <summary>
        /// Find the index of a rectangle that contains the specified point.
        /// </summary>
        /// <param name="point">the target point</param>
        /// <returns>the rectangle index</returns>
        private int FindRectangleIndex(ScreenPoint point)
        {
            IComparer<OxyRect> comparer;
            if (this.IsTransposed())
            {
                comparer = ComparerHelper.CreateComparer<OxyRect>(
                    (x, y) =>
                        {
                            if (x.Bottom < point.Y)
                            {
                                return 1;
                            }

                            if (x.Top > point.Y)
                            {
                                return -1;
                            }

                            return 0;
                        });
            }
            else
            {
                comparer = ComparerHelper.CreateComparer<OxyRect>(
                    (x, y) =>
                        {
                            if (x.Right < point.X)
                            {
                                return -1;
                            }

                            if (x.Left > point.X)
                            {
                                return 1;
                            }

                            return 0;
                        });
            }

            return this.rectangles.BinarySearch(0, this.rectangles.Count, new OxyRect(), comparer);
        }

        /// <summary>
        /// Renders the series bars.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="clippingRect">The clipping rectangle.</param>
        /// <param name="actualPoints">The list of points that should be rendered.</param>
        private void RenderBars(IRenderContext rc, OxyRect clippingRect, List<DataPoint> actualPoints)
        {
            var widthOffset = this.GetBarWidth(actualPoints) / 2;
            var widthVector = this.Orientate(new ScreenVector(widthOffset, 0));

            for (var pointIndex = 0; pointIndex < actualPoints.Count; pointIndex++)
            {
                var actualPoint = actualPoints[pointIndex];
                if (!this.IsValidPoint(actualPoint))
                {
                    continue;
                }

                var screenPoint = this.Transform(actualPoint) - widthVector;
                var basePoint = this.Transform(new DataPoint(actualPoint.X, 0)) + widthVector;
                var rectangle = new OxyRect(basePoint, screenPoint);
                this.rectangles.Add(rectangle);
                this.rectanglesPointIndexes.Add(pointIndex);

                var barColors = this.GetBarColors(actualPoint.Y);

                rc.DrawClippedRectangle(
                    clippingRect, 
                    rectangle, 
                    barColors.FillColor, 
                    barColors.StrokeColor, 
                    this.StrokeThickness, 
                    this.EdgeRenderingMode.GetActual(EdgeRenderingMode.PreferSharpness));
            }
        }

        /// <summary>
        /// Computes the bars width.
        /// </summary>
        /// <param name="actualPoints">The list of points.</param>
        /// <returns>The bars width.</returns>
        private double GetBarWidth(List<DataPoint> actualPoints)
        {
            var minDistance = this.BarWidth / this.XAxis.Scale;
            for (var pointIndex = 1; pointIndex < actualPoints.Count; pointIndex++)
            {
                var distance = actualPoints[pointIndex].X - actualPoints[pointIndex - 1].X;
                if (distance < minDistance)
                {
                    minDistance = distance;
                }
            }

            return minDistance * this.XAxis.Scale;
        }

        /// <summary>
        /// Gets the colors used to draw a bar.
        /// </summary>
        /// <param name="y">The point y value</param>
        /// <returns>The bar colors</returns>
        private BarColors GetBarColors(double y)
        {
            var positive = y >= 0.0;
            var fillColor = (positive || this.NegativeFillColor.IsUndefined()) ? this.GetSelectableFillColor(this.ActualColor) : this.NegativeFillColor;
            var strokeColor = (positive || this.NegativeStrokeColor.IsUndefined()) ? this.StrokeColor : this.NegativeStrokeColor;

            return new BarColors(fillColor, strokeColor);
        }

        /// <summary>
        /// Stores the colors used to draw a bar.
        /// </summary>
        private struct BarColors
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="BarColors" /> struct.
            /// </summary>
            /// <param name="fillColor">The fill color</param>
            /// <param name="strokeColor">The stroke color</param>
            public BarColors(OxyColor fillColor, OxyColor strokeColor) : this()
            {
                this.FillColor = fillColor;
                this.StrokeColor = strokeColor;
            }

            /// <summary>
            /// Gets the fill color.
            /// </summary>
            public OxyColor FillColor { get; private set; }

            /// <summary>
            /// Gets the stroke color.
            /// </summary>
            public OxyColor StrokeColor { get; private set; }
        }
    }
}
