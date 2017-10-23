// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlagSeries.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Renders a 'flag' above the x-axis at the specified positions (in the Values list).
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System.Collections.Generic;
    using System.Linq;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    /// <summary>
    /// Renders a 'flag' above the x-axis at the specified positions (in the Values list).
    /// </summary>
    public class FlagSeries : ItemsSeries
    {
        /// <summary>
        /// The symbol position (y coordinate).
        /// </summary>
        private double symbolPosition;

        /// <summary>
        /// The symbol text size.
        /// </summary>
        private OxySize symbolSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlagSeries" /> class.
        /// </summary>
        public FlagSeries()
        {
            this.Values = new List<double>();
            this.Color = OxyColors.Black;
            this.FontSize = 10;
            this.Symbol = ((char)0xEA).ToString();
            this.Font = "Wingdings 2";
            this.TrackerFormatString = "{0}: {1}";
        }

        /// <summary>
        /// Gets or sets the color of the symbols.
        /// </summary>
        /// <value>The color.</value>
        public OxyColor Color { get; set; }

        /// <summary>
        /// Gets the maximum value.
        /// </summary>
        /// <value>The maximum value.</value>
        public double MaximumX { get; private set; }

        /// <summary>
        /// Gets the minimum value.
        /// </summary>
        /// <value>The minimum value.</value>
        public double MinimumX { get; private set; }

        /// <summary>
        /// Gets or sets the symbol to draw at each value.
        /// </summary>
        /// <value>The symbol.</value>
        public string Symbol { get; set; }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>The values.</value>
        public List<double> Values { get; private set; }

        /// <summary>
        /// Gets the x-axis.
        /// </summary>
        /// <value>The x-axis.</value>
        public Axis XAxis { get; private set; }

        /// <summary>
        /// Gets or sets the x-axis key.
        /// </summary>
        /// <value>The x-axis key.</value>
        public string XAxisKey { get; set; }

        /// <summary>
        /// Gets the point on the series that is nearest the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="interpolate">Interpolate the series if this flag is set to <c>true</c>.</param>
        /// <returns>A TrackerHitResult for the current hit.</returns>
        public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
        {
            foreach (var v in this.Values)
            {
                if (double.IsNaN(v) || v < this.XAxis.ActualMinimum || v > this.XAxis.ActualMaximum)
                {
                    continue;
                }

                double x = this.XAxis.Transform(v);
                var r = new OxyRect(x - (this.symbolSize.Width / 2), this.symbolPosition - this.symbolSize.Height, this.symbolSize.Width, this.symbolSize.Height);
                if (r.Contains(point))
                {
                    return new TrackerHitResult
                    {
                        Series = this,
                        DataPoint = new DataPoint(v, double.NaN),
                        Position = new ScreenPoint(x, this.symbolPosition - this.symbolSize.Height),
                        Text = StringHelper.Format(this.ActualCulture, this.TrackerFormatString, null, this.Title, v)
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
            if (this.XAxis == null)
            {
                return;
            }

            this.symbolPosition = this.PlotModel.PlotArea.Bottom;
            this.symbolSize = rc.MeasureText(this.Symbol, this.ActualFont, this.ActualFontSize);
            foreach (var v in this.Values)
            {
                if (double.IsNaN(v) || v < this.XAxis.ActualMinimum || v > this.XAxis.ActualMaximum)
                {
                    continue;
                }

                double x = this.XAxis.Transform(v);
                rc.DrawText(
                    new ScreenPoint(x, this.symbolPosition),
                    this.Symbol,
                    this.Color,
                    this.ActualFont,
                    this.ActualFontSize,
                    this.ActualFontWeight,
                    0,
                    HorizontalAlignment.Center,
                    VerticalAlignment.Bottom);
            }
        }

        /// <summary>
        /// Renders the legend symbol on the specified render context.
        /// </summary>
        /// <param name="rc">The rendering context.</param>
        /// <param name="legendBox">The legend rectangle.</param>
        public override void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
            rc.DrawText(
                legendBox.Center,
                this.Symbol,
                this.Color,
                this.ActualFont,
                this.ActualFontSize,
                this.ActualFontWeight,
                0,
                HorizontalAlignment.Center,
                VerticalAlignment.Middle);
        }

        /// <summary>
        /// Check if this data series requires X/Y axes. (e.g. Pie series do not require axes)
        /// </summary>
        /// <returns>True if no axes are required.</returns>
        protected override bool AreAxesRequired()
        {
            return true;
        }

        /// <summary>
        /// Ensures that the axes of the series is defined.
        /// </summary>
        protected override void EnsureAxes()
        {
            this.XAxis = this.XAxisKey != null ?
                         this.PlotModel.GetAxis(this.XAxisKey) :
                         this.PlotModel.DefaultXAxis;
        }

        /// <summary>
        /// Check if the data series is using the specified axis.
        /// </summary>
        /// <param name="axis">An axis which should be checked if used</param>
        /// <returns>True if the axis is in use.</returns>
        protected override bool IsUsing(Axis axis)
        {
            return axis == this.XAxis;
        }

        /// <summary>
        /// Sets default values (colors, line style etc) from the plot model.
        /// </summary>
        protected override void SetDefaultValues()
        {
        }

        /// <summary>
        /// Updates the axis maximum and minimum values.
        /// </summary>
        protected override void UpdateAxisMaxMin()
        {
            this.XAxis.Include(this.MinimumX);
            this.XAxis.Include(this.MaximumX);
        }

        /// <summary>
        /// Updates the data from the ItemsSource.
        /// </summary>
        protected override void UpdateData()
        {
            // todo
        }

        /// <summary>
        /// Updates the maximum and minimum values of the series.
        /// </summary>
        protected override void UpdateMaxMin()
        {
            this.MinimumX = this.Values.Min();
            this.MaximumX = this.Values.Max();
        }
    }
}
