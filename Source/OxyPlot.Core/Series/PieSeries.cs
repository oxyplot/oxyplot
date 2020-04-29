// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PieSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a series for pie/circle/doughnut charts.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Series
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OxyPlot.Axes;

    /// <summary>
    /// Represents a series for pie/circle/doughnut charts.
    /// </summary>
    /// <remarks>The arc length/central angle/area of each slice is proportional to the quantity it represents.
    /// See <a href="http://en.wikipedia.org/wiki/Pie_chart">Pie charts</a>.</remarks>
    public class PieSeries : ItemsSeries
    {
        /// <summary>
        /// The default tracker format string
        /// </summary>
        public const string DefaultTrackerFormatString = "{1}: {2:0.###} ({3:P1})";

        /// <summary>
        /// The slices.
        /// </summary>
        private IList<PieSlice> slices;

        /// <summary>
        /// The actual points of the slices.
        /// </summary>
        private List<IList<ScreenPoint>> slicePoints = new List<IList<ScreenPoint>>();

        /// <summary>
        /// The total value of all the pie slices.
        /// </summary>
        private double total;

        /// <summary>
        /// Initializes a new instance of the <see cref="PieSeries" /> class.
        /// </summary>
        public PieSeries()
        {
            this.slices = new List<PieSlice>();

            this.Stroke = OxyColors.White;
            this.StrokeThickness = 1.0;
            this.Diameter = 1.0;
            this.InnerDiameter = 0.0;
            this.StartAngle = 0.0;
            this.AngleSpan = 360.0;
            this.AngleIncrement = 1.0;

            this.LegendFormat = null;
            this.OutsideLabelFormat = "{2:0} %";
            this.InsideLabelColor = OxyColors.Automatic;
            this.InsideLabelFormat = "{1}";
            this.TickDistance = 0;
            this.TickRadialLength = 6;
            this.TickHorizontalLength = 8;
            this.TickLabelDistance = 4;
            this.InsideLabelPosition = 0.5;
            this.FontSize = 12;
            this.TrackerFormatString = DefaultTrackerFormatString;
        }

        /// <summary>
        /// Gets or sets the angle increment.
        /// </summary>
        public double AngleIncrement { get; set; }

        /// <summary>
        /// Gets or sets the angle span.
        /// </summary>
        public double AngleSpan { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether inside labels are angled.
        /// </summary>
        public bool AreInsideLabelsAngled { get; set; }

        /// <summary>
        /// Gets or sets the name of the property containing the color.
        /// </summary>
        /// <value>The color field.</value>
        public string ColorField { get; set; }

        /// <summary>
        /// Gets or sets the diameter.
        /// </summary>
        /// <value>The diameter.</value>
        public double Diameter { get; set; }

        /// <summary>
        /// Gets or sets the exploded distance.
        /// </summary>
        /// <value>The exploded distance.</value>
        public double ExplodedDistance { get; set; }

        /// <summary>
        /// Gets or sets the inner diameter.
        /// </summary>
        /// <value>The inner diameter.</value>
        public double InnerDiameter { get; set; }

        /// <summary>
        /// Gets or sets the color of the inside labels.
        /// </summary>
        /// <remarks>If the value is <c>OxyColors.Automatic</c>, the <see cref="PlotElement.TextColor" /> will be used.</remarks>
        public OxyColor InsideLabelColor { get; set; }

        /// <summary>
        /// Gets or sets the inside label format.
        /// </summary>
        /// <value>The inside label format.</value>
        /// <remarks>The formatting arguments are: value {0}, label {1} and percentage {2}.</remarks>
        public string InsideLabelFormat { get; set; }

        /// <summary>
        /// Gets or sets the inside label position.
        /// </summary>
        /// <value>The inside label position.</value>
        public double InsideLabelPosition { get; set; }

        /// <summary>
        /// Gets or sets the is exploded field.
        /// </summary>
        /// <value>The is exploded field.</value>
        public string IsExplodedField { get; set; }

        /// <summary>
        /// Gets or sets the label field.
        /// </summary>
        /// <value>The label field.</value>
        public string LabelField { get; set; }

        /// <summary>
        /// Gets or sets the legend format.
        /// </summary>
        /// <value>The legend format.</value>
        public string LegendFormat { get; set; }

        /// <summary>
        /// Gets or sets the outside label format.
        /// </summary>
        /// <value>The outside label format.</value>
        public string OutsideLabelFormat { get; set; }

        /// <summary>
        /// Gets or sets the slices.
        /// </summary>
        /// <value>The slices.</value>
        public IList<PieSlice> Slices
        {
            get
            {
                return this.slices;
            }

            set
            {
                this.slices = value;
            }
        }

        /// <summary>
        /// Gets or sets the start angle.
        /// </summary>
        /// <value>The start angle.</value>
        public double StartAngle { get; set; }

        /// <summary>
        /// Gets or sets the stroke color.
        /// </summary>
        /// <value>The stroke color.</value>
        public OxyColor Stroke { get; set; }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>The stroke thickness.</value>
        public double StrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the distance from the edge of the pie slice to the tick line.
        /// </summary>
        /// <value>The distance.</value>
        public double TickDistance { get; set; }

        /// <summary>
        /// Gets or sets the length of the horizontal part of the tick.
        /// </summary>
        /// <value>The length.</value>
        public double TickHorizontalLength { get; set; }

        /// <summary>
        /// Gets or sets the distance from the tick line to the outside label.
        /// </summary>
        /// <value>The distance.</value>
        public double TickLabelDistance { get; set; }

        /// <summary>
        /// Gets or sets the length of the radial part of the tick line.
        /// </summary>
        /// <value>The length.</value>
        public double TickRadialLength { get; set; }

        /// <summary>
        /// Gets or sets the name of the property containing the value.
        /// </summary>
        /// <value>The value field.</value>
        public string ValueField { get; set; }

        /// <summary>
        /// Gets the point on the series that is nearest the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="interpolate">Interpolate the series if this flag is set to <c>true</c> .</param>
        /// <returns>A TrackerHitResult for the current hit.</returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            for (int i = 0; i < this.slicePoints.Count; i++)
            {
                if (ScreenPointHelper.IsPointInPolygon(point, this.slicePoints[i]))
                {
                    var slice = this.slices[i];
                    var item = this.GetItem(i);
                    return new TrackerHitResult
                    {
                        Series = this,
                        Position = point,
                        Item = item,
                        Index = i,
                        Text = StringHelper.Format(this.ActualCulture, this.TrackerFormatString, slice, this.Title, slice.Label, slice.Value, slice.Value / this.total)
                    };
                }
            }

            return null;
        }

        /// <summary>
        /// Renders the series on the specified render context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        public override void Render(IRenderContext rc)
        {
            this.slicePoints.Clear();

            if (this.Slices.Count == 0)
            {
                return;
            }

            this.total = this.slices.Sum(slice => slice.Value);
            if (Math.Abs(this.total) < double.Epsilon)
            {
                return;
            }

            // todo: reduce available size due to the labels
            double radius = Math.Min(this.PlotModel.PlotArea.Width, this.PlotModel.PlotArea.Height) / 2;

            double outerRadius = radius * (this.Diameter - this.ExplodedDistance);
            double innerRadius = radius * this.InnerDiameter;

            double angle = this.StartAngle;
            var midPoint = new ScreenPoint(
                (this.PlotModel.PlotArea.Left + this.PlotModel.PlotArea.Right) * 0.5, (this.PlotModel.PlotArea.Top + this.PlotModel.PlotArea.Bottom) * 0.5);

            foreach (var slice in this.slices)
            {
                var outerPoints = new List<ScreenPoint>();
                var innerPoints = new List<ScreenPoint>();

                double sliceAngle = slice.Value / this.total * this.AngleSpan;
                double endAngle = angle + sliceAngle;
                double explodedRadius = slice.IsExploded ? this.ExplodedDistance * radius : 0.0;

                double midAngle = angle + (sliceAngle / 2);
                double midAngleRadians = midAngle * Math.PI / 180;
                var mp = new ScreenPoint(
                    midPoint.X + (explodedRadius * Math.Cos(midAngleRadians)),
                    midPoint.Y + (explodedRadius * Math.Sin(midAngleRadians)));

                // Create the pie sector points for both outside and inside arcs
                while (true)
                {
                    bool stop = false;
                    if (angle >= endAngle)
                    {
                        angle = endAngle;
                        stop = true;
                    }

                    double a = angle * Math.PI / 180;
                    var op = new ScreenPoint(mp.X + (outerRadius * Math.Cos(a)), mp.Y + (outerRadius * Math.Sin(a)));
                    outerPoints.Add(op);
                    var ip = new ScreenPoint(mp.X + (innerRadius * Math.Cos(a)), mp.Y + (innerRadius * Math.Sin(a)));
                    if (innerRadius + explodedRadius > 0)
                    {
                        innerPoints.Add(ip);
                    }

                    if (stop)
                    {
                        break;
                    }

                    angle += this.AngleIncrement;
                }

                innerPoints.Reverse();
                if (innerPoints.Count == 0)
                {
                    innerPoints.Add(mp);
                }

                innerPoints.Add(outerPoints[0]);

                var points = outerPoints;
                points.AddRange(innerPoints);

                rc.DrawPolygon(points, slice.ActualFillColor, this.Stroke, this.StrokeThickness, this.EdgeRenderingMode, null, LineJoin.Bevel);

                // keep the point for hit testing
                this.slicePoints.Add(points);

                // Render label outside the slice
                if (this.OutsideLabelFormat != null)
                {
                    string label = string.Format(
                        this.OutsideLabelFormat, slice.Value, slice.Label, slice.Value / this.total * 100);
                    int sign = Math.Sign(Math.Cos(midAngleRadians));

                    // tick points
                    var tp0 = new ScreenPoint(
                        mp.X + ((outerRadius + this.TickDistance) * Math.Cos(midAngleRadians)),
                        mp.Y + ((outerRadius + this.TickDistance) * Math.Sin(midAngleRadians)));
                    var tp1 = new ScreenPoint(
                        tp0.X + (this.TickRadialLength * Math.Cos(midAngleRadians)),
                        tp0.Y + (this.TickRadialLength * Math.Sin(midAngleRadians)));
                    var tp2 = new ScreenPoint(tp1.X + (this.TickHorizontalLength * sign), tp1.Y);

                    // draw the tick line with the same color as the text
                    rc.DrawLine(new[] { tp0, tp1, tp2 }, this.ActualTextColor, 1, this.EdgeRenderingMode, null, LineJoin.Bevel);

                    // label
                    var labelPosition = new ScreenPoint(tp2.X + (this.TickLabelDistance * sign), tp2.Y);
                    rc.DrawText(
                        labelPosition,
                        label,
                        this.ActualTextColor,
                        this.ActualFont,
                        this.ActualFontSize,
                        this.ActualFontWeight,
                        0,
                        sign > 0 ? HorizontalAlignment.Left : HorizontalAlignment.Right,
                        VerticalAlignment.Middle);
                }

                // Render a label inside the slice
                if (this.InsideLabelFormat != null && !this.InsideLabelColor.IsUndefined())
                {
                    string label = string.Format(
                        this.InsideLabelFormat, slice.Value, slice.Label, slice.Value / this.total * 100);
                    double r = (innerRadius * (1 - this.InsideLabelPosition)) + (outerRadius * this.InsideLabelPosition);
                    var labelPosition = new ScreenPoint(
                        mp.X + (r * Math.Cos(midAngleRadians)), mp.Y + (r * Math.Sin(midAngleRadians)));
                    double textAngle = 0;
                    if (this.AreInsideLabelsAngled)
                    {
                        textAngle = midAngle;
                        if (Math.Cos(midAngleRadians) < 0)
                        {
                            textAngle += 180;
                        }
                    }

                    var actualInsideLabelColor = this.InsideLabelColor.IsAutomatic() ? this.ActualTextColor : this.InsideLabelColor;

                    rc.DrawText(
                        labelPosition,
                        label,
                        actualInsideLabelColor,
                        this.ActualFont,
                        this.ActualFontSize,
                        this.ActualFontWeight,
                        textAngle,
                        HorizontalAlignment.Center,
                        VerticalAlignment.Middle);
                }
            }
        }

        /// <summary>
        /// Renders the legend symbol on the specified render context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="legendBox">The legend rectangle.</param>
        public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
        }

        /// <summary>
        /// Checks if this data series requires X/Y axes. (e.g. PieSeries does not require axes)
        /// </summary>
        /// <returns>True if no axes are required.</returns>
        protected internal override bool AreAxesRequired()
        {
            return false;
        }

        /// <summary>
        /// Ensures that the axes of the series is defined.
        /// </summary>
        protected internal override void EnsureAxes()
        {
        }

        /// <summary>
        /// Check if the data series is using the specified axis.
        /// </summary>
        /// <param name="axis">An axis.</param>
        /// <returns>True if the axis is in use.</returns>
        protected internal override bool IsUsing(Axis axis)
        {
            return false;
        }

        /// <summary>
        /// Sets the default values.
        /// </summary>
        protected internal override void SetDefaultValues()
        {
            foreach (var slice in this.Slices)
            {
                if (slice.Fill.IsAutomatic())
                {
                    slice.DefaultFillColor = this.PlotModel.GetDefaultColor();
                }
            }
        }

        /// <summary>
        /// Updates the maximum and minimum values of the axes used by this series.
        /// </summary>
        protected internal override void UpdateAxisMaxMin()
        {
        }

        /// <summary>
        /// Updates the data.
        /// </summary>
        protected internal override void UpdateData()
        {
            if (this.ItemsSource == null)
            {
                return;
            }

            this.slices.Clear();

            var filler = new ListBuilder<PieSlice>();
            filler.Add(this.LabelField, (string)null);
            filler.Add(this.ValueField, double.NaN);
            filler.Add(this.ColorField, OxyColors.Automatic);
            filler.Add(this.IsExplodedField, false);
            filler.FillT(
                this.slices, 
                this.ItemsSource,
                args =>
                new PieSlice((string)args[0], Convert.ToDouble(args[1]))
                {
                    Fill = (OxyColor)args[2],
                    IsExploded = (bool)args[3]
                });
        }

        /// <summary>
        /// Updates the maximum and minimum values of the series.
        /// </summary>
        protected internal override void UpdateMaxMin()
        {
        }
    }
}