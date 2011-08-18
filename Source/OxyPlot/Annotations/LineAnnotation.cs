using System;
using System.Collections.Generic;

namespace OxyPlot
{
    public enum LineAnnotationType
    {
        /// <summary>
        /// Horizontal line given by the Y property
        /// </summary>
        Horizontal,
        /// <summary>
        /// Vertical line given by the X property
        /// </summary>
        Vertical,
        /// <summary>
        /// Linear equation y=mx+b given by the Slope and Intercept properties
        /// </summary>
        LinearEquation,
        /// <summary>
        /// Curve equation x=f(y) given by the Equation property
        /// </summary>
        EquationX,
        /// <summary>
        /// Curve equation y=f(x) given by the Equation property
        /// </summary>
        EquationY
    }

    /// <summary>
    /// The line annotation class.
    /// </summary>
    public class LineAnnotation : Annotation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LineAnnotation"/> class.
        /// </summary>
        public LineAnnotation()
        {
            Type = LineAnnotationType.LinearEquation;
            MinimumX = double.MinValue;
            MaximumX = double.MaxValue;
            MinimumY = double.MinValue;
            MaximumY = double.MaxValue;
            Color = OxyColors.Blue;
            StrokeThickness = 1;
            LineStyle = LineStyle.Dash;
            LineJoin = OxyPenLineJoin.Miter;

            TextPosition = 1;
            TextMargin = 12;
            TextHorizontalAlignment = HorizontalTextAlign.Right;
            TextVerticalAlignment = VerticalTextAlign.Top;
        }

        /// <summary>
        /// Vertical alignment of text (above or below the line).
        /// </summary>
        public VerticalTextAlign TextVerticalAlignment { get; set; }

        /// <summary>
        /// Horizontal alignment of text.
        /// </summary>
        public HorizontalTextAlign TextHorizontalAlignment { get; set; }

        /// <summary>
        /// Position of the text along the line [0,1]
        /// Positions smaller than 0.25 is left aligned at the start of the line
        /// Positions larger than 0.75 is right aligned at the end of the line
        /// Other positions are center aligned at the specified position
        /// </summary>
        public double TextPosition { get; set; }

        /// <summary>
        /// Text margin.
        /// </summary>
        public double TextMargin { get; set; }

        /// <summary>
        /// Type of line equation.
        /// </summary>
        public LineAnnotationType Type { get; set; }

        /// <summary>
        /// X position for vertical lines (only for Type==Vertical)
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y position for horizontal lines (only for Type==Horizontal)
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Linear equation slope (the m in y=mx+b) (only for Type==LinearEquation)
        /// http://en.wikipedia.org/wiki/Linear_equation
        /// </summary>
        public double Slope { get; set; }

        /// <summary>
        /// Linear equation y-intercept (the b in y=mx+b)  (only for Type==LinearEquation)
        /// http://en.wikipedia.org/wiki/Linear_equation
        /// </summary>
        public double Intercept { get; set; }

        /// <summary>
        /// The y=f(x) equation  (only for Type==Equation)
        /// </summary>
        public Func<double, double> Equation { get; set; }

        /// <summary>
        /// Gets or sets the minimum X coordinate for the line.
        /// </summary>
        public double MinimumX { get; set; }

        /// <summary>
        /// Gets or sets the maximum X coordinate for the line.
        /// </summary>
        public double MaximumX { get; set; }

        /// <summary>
        /// Gets or sets the minimum Y coordinate for the line.
        /// </summary>
        public double MinimumY { get; set; }

        /// <summary>
        /// Gets or sets the maximum Y coordinate for the line.
        /// </summary>
        public double MaximumY { get; set; }

        /// <summary>
        /// Gets or sets the color of the line.
        /// </summary>
        public OxyColor Color { get; set; }

        /// <summary>
        /// Thickness of the line.
        /// </summary>
        public double StrokeThickness { get; set; }

        /// <summary>
        ///   Gets or sets the line style.
        /// </summary>
        /// <value>The line style.</value>
        public LineStyle LineStyle { get; set; }

        /// <summary>
        /// Gets or sets the line join.
        /// </summary>
        /// <value>The line join.</value>
        public OxyPenLineJoin LineJoin { get; set; }

        /// <summary>
        /// Renders the line annotation.
        /// </summary>
        /// <param name="rc">The render context.</param>
        /// <param name="model">The plot model.</param>
        public override void Render(IRenderContext rc, PlotModel model)
        {
            base.Render(rc, model);

            bool isStraightLine = true;
            if (!(XAxis is LinearAxis) || !(YAxis is LinearAxis) || Type == LineAnnotationType.EquationY)
                isStraightLine = false;

            double actualMinimumX = Math.Max(MinimumX, XAxis.ActualMinimum);
            double actualMaximumX = Math.Min(MaximumX, XAxis.ActualMaximum);
            double actualMinimumY = Math.Max(MinimumY, YAxis.ActualMinimum);
            double actualMaximumY = Math.Min(MaximumY, YAxis.ActualMaximum);

            // y=f(x) or x=f(y)
            Func<double, double> f;
            bool isFunctionX = true;

            switch (Type)
            {
                case LineAnnotationType.Horizontal:
                    f = x => Y;
                    break;
                case LineAnnotationType.Vertical:
                    f = y => X;
                    isFunctionX = false;
                    break;
                case LineAnnotationType.EquationY:
                    f = Equation;
                    break;
                case LineAnnotationType.EquationX:
                    f = Equation;
                    isFunctionX = false;
                    break;
                default:
                    f = x => Slope * x + Intercept;
                    break;
            }

            var points = new List<DataPoint>();

            if (isStraightLine)
            {
                // we only need to calculate two points if it is a straight line
                if (isFunctionX)
                {
                    points.Add(new DataPoint(actualMinimumX, f(actualMinimumX)));
                    points.Add(new DataPoint(actualMaximumX, f(actualMaximumX)));
                }
                else
                {
                    points.Add(new DataPoint(f(actualMinimumY), actualMinimumY));
                    points.Add(new DataPoint(f(actualMaximumY), actualMaximumY));
                }
            }
            else
            {
                if (isFunctionX)
                {
                    double x = actualMinimumX;
                    // todo: the step size should be adaptive
                    double dx = (actualMaximumX - actualMinimumX) / 100;
                    while (true)
                    {
                        points.Add(new DataPoint(x, f(x)));
                        if (x > actualMaximumX)
                            break;
                        x += dx;
                    }
                }
                else
                {
                    double y = actualMinimumY;
                    // todo: the step size should be adaptive
                    double dy = (actualMaximumY - actualMinimumY) / 100;
                    while (true)
                    {
                        points.Add(new DataPoint(f(y), y));
                        if (y > actualMaximumY)
                            break;
                        y += dy;
                    }
                }
            }

            // transform to screen coordinates
            var screenPoints = new List<ScreenPoint>();
            foreach (var p in points)
            {
                screenPoints.Add(XAxis.Transform(p.X, p.Y, YAxis));
            }

            // clip to the plot area
            double minimumSegmentLength = 4;
            double minDistSquared = minimumSegmentLength * minimumSegmentLength;

            var clipping = new CohenSutherlandClipping(
                Math.Min(XAxis.ScreenMin.X, XAxis.ScreenMax.X),
                Math.Max(XAxis.ScreenMin.X, XAxis.ScreenMax.X),
                Math.Min(YAxis.ScreenMin.Y, YAxis.ScreenMax.Y),
                Math.Max(YAxis.ScreenMin.Y, YAxis.ScreenMax.Y));

            IList<ScreenPoint> clippedPoints = RenderClippedLine(rc, screenPoints, clipping, minDistSquared);

            ScreenPoint position;
            double angle;
            double margin = TextMargin;
            if (TextHorizontalAlignment == HorizontalTextAlign.Center)
                margin = 0;
            if (TextHorizontalAlignment == HorizontalTextAlign.Right)
                margin *= -1;

            if (GetPosition(clippedPoints, TextPosition, margin, out position, out angle))
            {
                rc.DrawText(position, Text, model.TextColor, model.LegendFont, model.LegendFontSize, FontWeights.Normal, angle,
                            TextHorizontalAlignment, TextVerticalAlignment);
            }
        }

        private bool GetPosition(IList<ScreenPoint> pts, double p, double margin, out ScreenPoint position,
                                 out double angle)
        {
            if (pts == null || pts.Count == 0)
            {
                position = new ScreenPoint();
                angle = 0;
                return false;
            }

            double length = 0;
            for (int i = 1; i < pts.Count; i++)
            {
                double dx = pts[i].X - pts[i - 1].X;
                double dy = pts[i].Y - pts[i - 1].Y;
                length += Math.Sqrt(dx * dx + dy * dy);
            }
            double l = length * p + margin;
            length = 0;
            for (int i = 1; i < pts.Count; i++)
            {
                double dx = pts[i].X - pts[i - 1].X;
                double dy = pts[i].Y - pts[i - 1].Y;
                double dl = Math.Sqrt(dx * dx + dy * dy);
                if (l >= length && l <= length + dl)
                {
                    double f = (l - length) / dl;
                    double x = pts[i].X * f + pts[i - 1].X * (1 - f);
                    double y = pts[i].Y * f + pts[i - 1].Y * (1 - f);
                    position = new ScreenPoint(x, y);
                    angle = Math.Atan2(dy, dx) / Math.PI * 180;
                    return true;
                }
                length += dl;
            }

            position = pts[0];
            angle = 0;
            return false;
        }

        // todo: refactor - this is the same code as in the LineSeries
        private IList<ScreenPoint> RenderClippedLine(IRenderContext rc, IList<ScreenPoint> points,
                                                     CohenSutherlandClipping clipping, double minDistSquared)
        {
            List<ScreenPoint> result = null;

            var pts = new List<ScreenPoint>();
            int n = points.Count;
            if (n > 0)
            {
                ScreenPoint s0 = points[0];
                ScreenPoint last = points[0];

                for (int i = 1; i < n; i++)
                {
                    ScreenPoint s1 = points[i];

                    // Clipped version of this and next point.

                    ScreenPoint s0c = s0;
                    ScreenPoint s1c = s1;
                    bool isInside = clipping.ClipLine(ref s0c, ref s1c);
                    s0 = s1;

                    if (!isInside)
                    {
                        // keep the previous coordinate
                        continue;
                    }

                    // render from s0c-s1c
                    double dx = s1c.x - last.x;
                    double dy = s1c.y - last.y;

                    if (dx * dx + dy * dy > minDistSquared || i == 1)
                    {
                        if (!s0c.Equals(last) || i == 1)
                        {
                            pts.Add(s0c);
                        }

                        pts.Add(s1c);
                        last = s1c;
                    }

                    // render the line if we are leaving the clipping region););
                    if (!clipping.IsInside(s1))
                    {
                        RenderLine(rc, pts);
                        result = pts;
                        pts = new List<ScreenPoint>();
                    }
                }
                RenderLine(rc, pts);
                if (pts.Count > 0)
                    result = pts;
            }

            return result;
        }

        // todo: refactor
        protected void RenderLine(IRenderContext rc, List<ScreenPoint> pts)
        {
            if (pts.Count == 0)
            {
                return;
            }

            rc.DrawLine(pts.ToArray(), Color, StrokeThickness, LineStyleHelper.GetDashArray(LineStyle), LineJoin);
        }
    }
}