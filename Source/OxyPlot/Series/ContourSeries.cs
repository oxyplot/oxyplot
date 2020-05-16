// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContourSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a series that renders contours.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Represents a series that renders contours.
    /// </summary>
    /// <remarks>See <a href="http://en.wikipedia.org/wiki/Contour_line">wikipedia</a> and <a href="http://www.mathworks.se/help/techdoc/ref/contour.html">link</a>.</remarks>
    public class ContourSeries : XYAxisSeries
    {
        /// <summary>
        /// The default tracker format string
        /// </summary>
        public new const string DefaultTrackerFormatString = "{0}\n{1}: {2}\n{3}: {4}\n{5}: {6}";

        /// <summary>
        /// The contour collection.
        /// </summary>
        private List<Contour> contours;

        /// <summary>
        /// The temporary segment collection.
        /// </summary>
        private List<ContourSegment> segments;

        /// <summary>
        /// The default color.
        /// </summary>
        private OxyColor defaultColor;

        /// <summary>
        /// Initializes a new instance of the <see cref = "ContourSeries" /> class.
        /// </summary>
        public ContourSeries()
        {
            this.ContourLevelStep = double.NaN;
            this.LabelStep = 1;
            this.MultiLabel = false;
            this.LabelSpacing = 150;
            this.LabelBackground = OxyColor.FromAColor(220, OxyColors.White);

            this.Color = OxyColors.Automatic;
            this.StrokeThickness = 1.0;
            this.LineStyle = LineStyle.Solid;

            this.TrackerFormatString = DefaultTrackerFormatString;
        }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>The color.</value>
        public OxyColor Color { get; set; }

        /// <summary>
        /// Gets the actual color.
        /// </summary>
        /// <value>The actual color.</value>
        public OxyColor ActualColor
        {
            get { return this.Color.GetActualColor(this.defaultColor); }
        }

        /// <summary>
        /// Gets or sets the column coordinates.
        /// </summary>
        /// <value>The column coordinates.</value>
        public double[] ColumnCoordinates { get; set; }

        /// <summary>
        /// Gets or sets the contour level step size.
        /// This property is not used if the <see cref="ContourLevels"/> vector is set.
        /// </summary>
        /// <value>The contour level step size.</value>
        public double ContourLevelStep { get; set; }

        /// <summary>
        /// Gets or sets the contour levels.
        /// </summary>
        /// <value>The contour levels.</value>
        public double[] ContourLevels { get; set; }

        /// <summary>
        /// Gets or sets the contour colors.
        /// </summary>
        /// <value>The contour colors.</value>
        /// <remarks>These colors will override the Color of the series.
        /// If there are less colors than the number of contour levels, the colors will cycle.</remarks>
        public OxyColor[] ContourColors { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public double[,] Data { get; set; }

        /// <summary>
        /// Gets or sets the text background color.
        /// </summary>
        /// <value>The text background color.</value>
        public OxyColor LabelBackground { get; set; }

        /// <summary>
        /// Gets or sets the format string for contour values.
        /// </summary>
        /// <value>The format string.</value>
        public string LabelFormatString { get; set; }

        /// <summary>
        /// Gets or sets the label spacing, which is the space between labels on the same contour. Not used if <see cref="MultiLabel"/>==<see langword="false"/>
        /// </summary>
        /// <value>The label spacing.</value>
        public double LabelSpacing { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether multiple labels should be displayed per Contour. The default value is <c>false</c>
        /// </summary>
        public bool MultiLabel { get; set; }

        /// <summary>
        /// Gets or sets the interval between labeled contours. LabelStep = 1 is default and it means that all contours have a label
        /// </summary>
        /// <value>The label step.</value>
        public int LabelStep { get; set; }

        /// <summary>
        /// Gets or sets the line style.
        /// </summary>
        /// <value>The line style.</value>
        public LineStyle LineStyle { get; set; }

        /// <summary>
        /// Gets or sets the row coordinates.
        /// </summary>
        /// <value>The row coordinates.</value>
        public double[] RowCoordinates { get; set; }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>The stroke thickness.</value>
        public double StrokeThickness { get; set; }

        /// <summary>
        /// Calculates the contours.
        /// </summary>
        public void CalculateContours()
        {
            if (this.Data == null)
            {
                return;
            }

            double[] actualContourLevels = this.ContourLevels;

            this.segments = new List<ContourSegment>();
            Conrec.RendererDelegate renderer = (startX, startY, endX, endY, contourLevel) =>
                this.segments.Add(new ContourSegment(new DataPoint(startX, startY), new DataPoint(endX, endY), contourLevel));

            if (actualContourLevels == null)
            {
                double max = this.Data[0, 0];
                double min = this.Data[0, 0];
                for (int i = 0; i < this.Data.GetUpperBound(0); i++)
                {
                    for (int j = 0; j < this.Data.GetUpperBound(1); j++)
                    {
                        max = Math.Max(max, this.Data[i, j]);
                        min = Math.Min(min, this.Data[i, j]);
                    }
                }

                double actualStep = this.ContourLevelStep;
                if (double.IsNaN(actualStep))
                {
                    double range = max - min;
                    double step = range / 20;
                    double stepExp = Math.Round(Math.Log(Math.Abs(step), 10));
                    actualStep = Math.Pow(10, Math.Floor(stepExp));
                    this.ContourLevelStep = actualStep;
                }

                max = Math.Round(actualStep * (int)Math.Ceiling(max / actualStep), 14);
                min = Math.Round(actualStep * (int)Math.Floor(min / actualStep), 14);

                actualContourLevels = ArrayBuilder.CreateVector(min, max, actualStep);
            }

            Conrec.Contour(this.Data, this.ColumnCoordinates, this.RowCoordinates, actualContourLevels, renderer);

            this.JoinContourSegments();

            if (this.ContourColors != null && this.ContourColors.Length > 0)
            {
                foreach (var c in this.contours)
                {
                    // get the index of the contour's level
                    var index = IndexOf(actualContourLevels, c.ContourLevel);
                    if (index >= 0)
                    {
                        // clamp the index to the range of the ContourColors array
                        index = index % this.ContourColors.Length;
                        c.Color = this.ContourColors[index];
                    }
                }
            }
        }

        /// <summary>
        /// Gets the point in the dataset that is nearest the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="interpolate">The interpolate.</param>
        /// <returns>A hit result object.</returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            TrackerHitResult result = null;

            var xaxisTitle = this.XAxis.Title ?? "X";
            var yaxisTitle = this.YAxis.Title ?? "Y";
            var zaxisTitle = "Z";

            foreach (var c in this.contours)
            {
                var r = interpolate ? this.GetNearestInterpolatedPointInternal(c.Points, point) : this.GetNearestPointInternal(c.Points, point);
                if (r != null)
                {
                    if (result == null || result.Position.DistanceToSquared(point) > r.Position.DistanceToSquared(point))
                    {
                        result = r;
                        result.Text = StringHelper.Format(
                            this.ActualCulture,
                            this.TrackerFormatString,
                            null,
                            this.Title,
                            xaxisTitle,
                            this.XAxis.GetValue(r.DataPoint.X),
                            yaxisTitle,
                            this.YAxis.GetValue(r.DataPoint.Y),
                            zaxisTitle,
                            c.ContourLevel);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Renders the series on the specified rendering context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        public override void Render(IRenderContext rc)
        {
            if (this.contours == null)
            {
                this.CalculateContours();
            }

            if (this.contours.Count == 0)
            {
                return;
            }

            this.VerifyAxes();

            var clippingRect = this.GetClippingRect();
            rc.SetClip(clippingRect);

            var contourLabels = new List<ContourLabel>();
            var dashArray = this.LineStyle.GetDashArray();

            foreach (var contour in this.contours)
            {
                if (this.StrokeThickness <= 0 || this.LineStyle == LineStyle.None)
                {
                    continue;
                }

                var transformedPoints = contour.Points.Select(this.Transform).ToArray();

                var strokeColor = contour.Color.GetActualColor(this.ActualColor);

                rc.DrawClippedLine(
                    clippingRect,
                    transformedPoints,
                    4,
                    this.GetSelectableColor(strokeColor),
                    this.StrokeThickness,
                    this.EdgeRenderingMode,
                    dashArray,
                    LineJoin.Miter);

                // measure total contour length
                var contourLength = 0.0;
                for (int i = 1; i < transformedPoints.Length; i++)
                {
                    contourLength += (transformedPoints[i] - transformedPoints[i - 1]).Length;
                }

                // don't add label to contours, if ContourLevel is not close to LabelStep
                if (transformedPoints.Length <= 10 || (Math.Round(contour.ContourLevel / this.ContourLevelStep) % this.LabelStep != 0))
                {
                    continue;
                }

                if (!this.MultiLabel)
                {
                    this.AddContourLabels(contour, transformedPoints, clippingRect, contourLabels, (transformedPoints.Length - 1) * 0.5);
                    continue;
                }

                // calculate how many labels fit per contour
                var labelsCount = (int)(contourLength / this.LabelSpacing);
                if (labelsCount == 0)
                {
                    this.AddContourLabels(contour, transformedPoints, clippingRect, contourLabels, (transformedPoints.Length - 1) * 0.5);
                    continue;
                }

                var contourPartLength = 0.0;
                var contourPartLengthOld = 0.0;
                var intervalIndex = 1;
                var contourPartLengthTarget = 0.0;
                var contourFirstPartLengthTarget = (contourLength - ((labelsCount - 1) * this.LabelSpacing)) / 2;
                for (var j = 0; j < labelsCount; j++)
                {
                    var labelIndex = 0.0;

                    if (intervalIndex == 1)
                    {
                        contourPartLengthTarget = contourFirstPartLengthTarget;
                    }
                    else
                    {
                        contourPartLengthTarget = contourFirstPartLengthTarget + (j * this.LabelSpacing);
                    }

                    // find index of contour points where next label should be positioned
                    for (var k = intervalIndex; k < transformedPoints.Length; k++)
                    {
                        contourPartLength += (transformedPoints[k] - transformedPoints[k - 1]).Length;

                        if (contourPartLength > contourPartLengthTarget)
                        {
                            labelIndex = (k - 1) + ((contourPartLengthTarget - contourPartLengthOld) / (contourPartLength - contourPartLengthOld));
                            intervalIndex = k + 1;
                            break;
                        }

                        contourPartLengthOld = contourPartLength;
                    }

                    this.AddContourLabels(contour, transformedPoints, clippingRect, contourLabels, labelIndex);
                }
            }

            foreach (var cl in contourLabels)
            {
                this.RenderLabelBackground(rc, cl);
            }

            foreach (var cl in contourLabels)
            {
                this.RenderLabel(rc, cl);
            }

            rc.ResetClip();
        }

        /// <summary>
        /// Sets default values from the plot model.
        /// </summary>
        protected internal override void SetDefaultValues()
        {
            if (this.Color.IsAutomatic())
            {
                this.LineStyle = this.PlotModel.GetDefaultLineStyle();
                this.defaultColor = this.PlotModel.GetDefaultColor();
            }
        }

        /// <summary>
        /// Updates the maximum and minimum values of the series.
        /// </summary>
        protected internal override void UpdateMaxMin()
        {
            this.MinX = this.ColumnCoordinates.Min();
            this.MaxX = this.ColumnCoordinates.Max();
            this.MinY = this.RowCoordinates.Min();
            this.MaxY = this.RowCoordinates.Max();
        }

        /// <summary>
        /// Determines if two values are close.
        /// </summary>
        /// <param name="x1">The first value.</param>
        /// <param name="x2">The second value.</param>
        /// <param name="eps">The squared tolerance.</param>
        /// <returns>True if the values are close.</returns>
        private static bool AreClose(double x1, double x2, double eps = 1e-6)
        {
            double dx = x1 - x2;
            return dx * dx < eps;
        }

        /// <summary>
        /// Determines if two points are close.
        /// </summary>
        /// <param name="p0">The first point.</param>
        /// <param name="p1">The second point.</param>
        /// <param name="eps">The squared tolerance.</param>
        /// <returns>True if the points are close.</returns>
        private static bool AreClose(DataPoint p0, DataPoint p1, double eps = 1e-6)
        {
            double dx = p0.X - p1.X;
            double dy = p0.Y - p1.Y;
            return (dx * dx) + (dy * dy) < eps;
        }

        /// <summary>
        /// Gets the index of item that is closest to the specified value.
        /// </summary>
        /// <param name="values">A list of values.</param>
        /// <param name="value">A value.</param>
        /// <returns>An index.</returns>
        private static int IndexOf(IList<double> values, double value)
        {
            double min = double.MaxValue;
            int index = -1;
            for (int i = 0; i < values.Count; i++)
            {
                var d = Math.Abs(values[i] - value);
                if (d < min)
                {
                    min = d;
                    index = i;
                }
            }

            return index;
        }

        /// <summary>
        /// The add contour labels.
        /// </summary>
        /// <param name="contour">The contour.</param>
        /// <param name="pts">The points of the contour.</param>
        /// <param name="clippingRect">The clipping rectangle.</param>
        /// <param name="contourLabels">The contour labels.</param>
        /// <param name="labelIndex">The index of the point in the list of points, where the label should get added.</param>
        private void AddContourLabels(Contour contour, ScreenPoint[] pts, OxyRect clippingRect, ICollection<ContourLabel> contourLabels, double labelIndex)
        {
            if (pts.Length < 2)
            {
                return;
            }

            // Calculate position and angle of the label
            var i0 = (int)labelIndex;
            var i1 = i0 + 1;
            var dx = pts[i1].X - pts[i0].X;
            var dy = pts[i1].Y - pts[i0].Y;
            var x = pts[i0].X + (dx * (labelIndex - i0));
            var y = pts[i0].Y + (dy * (labelIndex - i0));
            if (!clippingRect.Contains(x, y))
            {
                return;
            }

            var pos = new ScreenPoint(x, y);
            var angle = Math.Atan2(dy, dx) * 180 / Math.PI;
            if (angle > 90)
            {
                angle -= 180;
            }

            if (angle < -90)
            {
                angle += 180;
            }

            var formatString = string.Concat("{0:", this.LabelFormatString, "}");
            var text = string.Format(this.ActualCulture, formatString, contour.ContourLevel);
            contourLabels.Add(new ContourLabel { Position = pos, Angle = angle, Text = text });
        }

        /// <summary>
        /// Finds the connected segment.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="contourLevel">The contour level.</param>
        /// <param name="eps">The distance tolerance.</param>
        /// <param name="reverse">reverse the segment if set to <c>true</c>.</param>
        /// <returns>The connected segment, or <c>null</c> if no segment was found.</returns>
        private ContourSegment FindConnectedSegment(DataPoint point, double contourLevel, double eps, out bool reverse)
        {
            reverse = false;
            foreach (var s in this.segments)
            {
                if (!AreClose(s.ContourLevel, contourLevel, eps))
                {
                    continue;
                }

                if (AreClose(point, s.StartPoint, eps))
                {
                    return s;
                }

                if (AreClose(point, s.EndPoint, eps))
                {
                    reverse = true;
                    return s;
                }
            }

            return null;
        }

        /// <summary>
        /// Joins the contour segments.
        /// </summary>
        /// <param name="eps">The tolerance for segment ends to connect (squared distance).</param>
        private void JoinContourSegments(double eps = 1e-10)
        {
            // This is a simple, slow, naïve method - should be improved:
            // http://stackoverflow.com/questions/1436091/joining-unordered-line-segments
            this.contours = new List<Contour>();
            var contourPoints = new List<DataPoint>();
            int contourPointsCount = 0;

            ContourSegment firstSegment = null;
            int segmentCount = this.segments.Count;
            while (segmentCount > 0)
            {
                ContourSegment segment1 = null, segment2 = null;

                if (firstSegment != null)
                {
                    bool reverse;

                    // Find a segment that is connected to the head of the contour
                    segment1 = this.FindConnectedSegment(contourPoints[0], firstSegment.ContourLevel, eps, out reverse);
                    if (segment1 != null)
                    {
                        contourPoints.Insert(0, reverse ? segment1.StartPoint : segment1.EndPoint);
                        contourPointsCount++;
                        this.segments.Remove(segment1);
                        segmentCount--;
                    }

                    // Find a segment that is connected to the tail of the contour
                    segment2 = this.FindConnectedSegment(contourPoints[contourPointsCount - 1], firstSegment.ContourLevel, eps, out reverse);
                    if (segment2 != null)
                    {
                        contourPoints.Add(reverse ? segment2.StartPoint : segment2.EndPoint);
                        contourPointsCount++;
                        this.segments.Remove(segment2);
                        segmentCount--;
                    }
                }

                if ((segment1 == null && segment2 == null) || segmentCount == 0)
                {
                    if (contourPointsCount > 0 && firstSegment != null)
                    {
                        this.contours.Add(new Contour(contourPoints, firstSegment.ContourLevel));
                        contourPoints = new List<DataPoint>();
                        contourPointsCount = 0;
                    }

                    if (segmentCount > 0)
                    {
                        firstSegment = this.segments.First();
                        contourPoints.Add(firstSegment.StartPoint);
                        contourPoints.Add(firstSegment.EndPoint);
                        contourPointsCount += 2;
                        this.segments.Remove(firstSegment);
                        segmentCount--;
                    }
                }
            }
        }

        /// <summary>
        /// Renders the contour label.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="cl">The contour label.</param>
        private void RenderLabel(IRenderContext rc, ContourLabel cl)
        {
            if (this.ActualFontSize > 0)
            {
                rc.DrawText(
                    cl.Position,
                    cl.Text,
                    this.ActualTextColor,
                    this.ActualFont,
                    this.ActualFontSize,
                    this.ActualFontWeight,
                    cl.Angle,
                    HorizontalAlignment.Center,
                    VerticalAlignment.Middle);
            }
        }

        /// <summary>
        /// Renders the contour label background.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="cl">The contour label.</param>
        private void RenderLabelBackground(IRenderContext rc, ContourLabel cl)
        {
            if (this.LabelBackground.IsInvisible())
            {
                return;
            }

            // Calculate background polygon
            var size = rc.MeasureText(cl.Text, this.ActualFont, this.ActualFontSize, this.ActualFontWeight);
            double a = cl.Angle / 180 * Math.PI;
            double dx = Math.Cos(a);
            double dy = Math.Sin(a);

            double ux = dx * 0.6;
            double uy = dy * 0.6;
            double vx = -dy * 0.5;
            double vy = dx * 0.5;
            double x = cl.Position.X;
            double y = cl.Position.Y;

            var bpts = new[]
                           {
                               new ScreenPoint(x - (size.Width * ux) - (size.Height * vx), y - (size.Width * uy) - (size.Height * vy)),
                               new ScreenPoint(x + (size.Width * ux) - (size.Height * vx), y + (size.Width * uy) - (size.Height * vy)),
                               new ScreenPoint(x + (size.Width * ux) + (size.Height * vx), y + (size.Width * uy) + (size.Height * vy)),
                               new ScreenPoint(x - (size.Width * ux) + (size.Height * vx), y - (size.Width * uy) + (size.Height * vy))
                           };
            rc.DrawPolygon(bpts, this.LabelBackground, OxyColors.Undefined, 0, this.EdgeRenderingMode);
        }

        /// <summary>
        /// Represents a contour.
        /// </summary>
        private class Contour
        {
            /// <summary>
            /// Gets or sets the contour level.
            /// </summary>
            /// <value>The contour level.</value>
            internal readonly double ContourLevel;

            /// <summary>
            /// Gets or sets the points.
            /// </summary>
            /// <value>The points.</value>
            internal readonly List<DataPoint> Points;

            /// <summary>
            /// Initializes a new instance of the <see cref="Contour" /> class.
            /// </summary>
            /// <param name="points">The points.</param>
            /// <param name="contourLevel">The contour level.</param>
            public Contour(List<DataPoint> points, double contourLevel)
            {
                this.Points = points;
                this.ContourLevel = contourLevel;
                this.Color = OxyColors.Automatic;
            }

            /// <summary>
            /// Gets or sets the color of the contour.
            /// </summary>
            public OxyColor Color { get; set; }
        }

        /// <summary>
        /// Represents a contour label.
        /// </summary>
        private class ContourLabel
        {
            /// <summary>
            /// Gets or sets the angle.
            /// </summary>
            /// <value>The angle.</value>
            public double Angle { get; set; }

            /// <summary>
            /// Gets or sets the position.
            /// </summary>
            /// <value>The position.</value>
            public ScreenPoint Position { get; set; }

            /// <summary>
            /// Gets or sets the text.
            /// </summary>
            /// <value>The text.</value>
            public string Text { get; set; }
        }

        /// <summary>
        /// Represents a contour segment.
        /// </summary>
        private class ContourSegment
        {
            /// <summary>
            /// The contour level.
            /// </summary>
            internal readonly double ContourLevel;

            /// <summary>
            /// The end point.
            /// </summary>
            internal readonly DataPoint EndPoint;

            /// <summary>
            /// The start point.
            /// </summary>
            internal readonly DataPoint StartPoint;

            /// <summary>
            /// Initializes a new instance of the <see cref="ContourSegment" /> class.
            /// </summary>
            /// <param name="startPoint">The start point.</param>
            /// <param name="endPoint">The end point.</param>
            /// <param name="contourLevel">The contour level.</param>
            public ContourSegment(DataPoint startPoint, DataPoint endPoint, double contourLevel)
            {
                this.ContourLevel = contourLevel;
                this.StartPoint = startPoint;
                this.EndPoint = endPoint;
            }
        }
    }
}
