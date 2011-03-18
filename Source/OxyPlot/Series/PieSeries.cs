using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace OxyPlot
{
    /// <summary>
    ///   The PieSeries class renders a Pie/Circle/Doughnut chart.
    ///   The arc length/central angle/area of each slice is proportional to the quantity it represents.
    ///   http://en.wikipedia.org/wiki/Pie_chart
    /// </summary>
    public class PieSeries : ISeries
    {
        private Collection<PieSlice> slices;

        public PieSeries()
        {
            slices = new Collection<PieSlice>();

            Stroke = OxyColors.White;
            StrokeThickness = 1.0;
            Diameter = 1.0;
            InnerDiameter = 0.0;
            StartAngle = 0.0;
            AngleSpan = 360.0;
            AngleIncrement = 1.0;

            LegendFormat = null;
            OutsideLabelFormat = "{2:0} %";
            InsideLabelFormat = "{1}";
            TickDistance = 0;
            TickRadialLength = 6;
            TickHorizontalLength = 8;
            TickLabelDistance = 4;
            InsideLabelPosition = 0.5;
        }

        public string Title { get; set; }

        /// <summary>
        ///   Gets or sets the items source.
        /// </summary>
        /// <value>The items source.</value>
        public IEnumerable ItemsSource { get; set; }

        public string LabelField { get; set; }
        public string ValueField { get; set; }
        public string ColorField { get; set; }
        public string IsExplodedField { get; set; }

        public OxyColor Stroke { get; set; }
        public double StrokeThickness { get; set; }

        public double Diameter { get; set; }
        public double InnerDiameter { get; set; }
        public double ExplodedDistance { get; set; }

        public double StartAngle { get; set; }
        public double AngleSpan { get; set; }
        public double AngleIncrement { get; set; }

        public string LegendFormat { get; set; }
        public string OutsideLabelFormat { get; set; }
        public string InsideLabelFormat { get; set; }

        public double TickDistance { get; set; }
        public double TickRadialLength { get; set; }
        public double TickHorizontalLength { get; set; }
        public double TickLabelDistance { get; set; }
        public double InsideLabelPosition { get; set; }
        public bool AreInsideLabelsAngled { get; set; }

        public Collection<PieSlice> Slices
        {
            get { return slices; }
            set { slices = value; }
        }

        public OxyColor Background { get; set; }

        #region ISeries Members

        public void RenderLegend(IRenderContext rc, OxyRect legendBox)
        {
        }

        public void UpdateData()
        {
            if (ItemsSource == null)
            {
                return;
            }

            slices.Clear();

            // Use reflection to find the data items
            PropertyInfo pil = null;
            PropertyInfo piv = null;
            PropertyInfo pic = null;
            PropertyInfo pie = null;
            Type t = null;

            foreach (object o in ItemsSource)
            {
                if (pil == null || o.GetType() != t)
                {
                    t = o.GetType();
                    pil = t.GetProperty(LabelField);
                    piv = t.GetProperty(ValueField);
                    pic = t.GetProperty(ColorField);
                    pie = t.GetProperty(IsExplodedField);
                    if (piv == null)
                    {
                        throw new InvalidOperationException(
                            string.Format("Could not find value data field {0} on type {1}", ValueField, t));
                    }
                }

                var slice = new PieSlice();
                slice.Value = Convert.ToDouble(piv.GetValue(o, null));
                if (pil != null)
                {
                    slice.Label = Convert.ToString(pil.GetValue(o, null));
                }

                if (pic != null)
                {
                    slice.Fill = (OxyColor) pic.GetValue(o, null);
                }

                if (pie != null)
                {
                    slice.IsExploded = (bool) pie.GetValue(o, null);
                }

                slices.Add(slice);
            }
        }

        public bool AreAxesRequired()
        {
            return false;
        }

        public void EnsureAxes(Collection<IAxis> axes, IAxis defaultXAxis, IAxis defaultYAxis)
        {
        }

        public void UpdateMaxMin()
        {
        }

        public bool GetNearestInterpolatedPoint(ScreenPoint pt, out DataPoint dp, out ScreenPoint sp)
        {
            dp = DataPoint.Undefined;
            sp = ScreenPoint.Undefined;
            return false;
        }

        public bool GetNearestPoint(ScreenPoint pt, out DataPoint dp, out ScreenPoint sp)
        {
            dp = DataPoint.Undefined;
            sp = ScreenPoint.Undefined;
            return false;
        }

        public void SetDefaultValues(PlotModel model)
        {
            foreach (var slice in Slices)
                if (slice.Fill == null)
                    slice.Fill = model.GetDefaultColor();
        }

        public void Render(IRenderContext rc, PlotModel model)
        {
            if (Slices.Count == 0)
            {
                return;
            }

            double total = slices.Sum(slice => slice.Value);
            if (total == 0.0)
            {
                return;
            }

            // todo: reduce available size due to the labels
            double radius = Math.Min(model.PlotArea.Width, model.PlotArea.Height)/2;

            double outerRadius = radius*(Diameter - ExplodedDistance);
            double innerRadius = radius*InnerDiameter;

            double angle = StartAngle;
            foreach (PieSlice slice in slices)
            {
                var outerPoints = new List<ScreenPoint>();
                var innerPoints = new List<ScreenPoint>();

                double sliceAngle = slice.Value/total*AngleSpan;
                double endAngle = angle + sliceAngle;
                double explodedRadius = slice.IsExploded ? ExplodedDistance*radius : 0.0;

                double midAngle = angle + sliceAngle/2;
                double midAngleRadians = midAngle*Math.PI/180;
                var mp = new ScreenPoint(model.MidPoint.X + explodedRadius*Math.Cos(midAngleRadians),
                                         model.MidPoint.Y + explodedRadius*Math.Sin(midAngleRadians));

                // Create the pie sector points for both outside and inside arcs
                while (true)
                {
                    bool stop = false;
                    if (angle >= endAngle)
                    {
                        angle = endAngle;
                        stop = true;
                    }

                    double a = angle*Math.PI/180;
                    var op = new ScreenPoint(mp.X + outerRadius*Math.Cos(a),
                                             mp.Y + outerRadius*Math.Sin(a));
                    outerPoints.Add(op);
                    var ip = new ScreenPoint(mp.X + innerRadius*Math.Cos(a),
                                             mp.Y + innerRadius*Math.Sin(a));
                    if (innerRadius + explodedRadius > 0)
                    {
                        innerPoints.Add(ip);
                    }

                    if (stop)
                    {
                        break;
                    }

                    angle += AngleIncrement;
                }


                innerPoints.Reverse();
                if (innerPoints.Count == 0)
                {
                    innerPoints.Add(mp);
                }

                innerPoints.Add(outerPoints[0]);

                List<ScreenPoint> points = outerPoints;
                points.AddRange(innerPoints);

                rc.DrawPolygon(points, slice.Fill, Stroke, StrokeThickness, null, OxyPenLineJoin.Bevel);

                // Render label outside the slice
                if (OutsideLabelFormat != null)
                {
                    string label = String.Format(OutsideLabelFormat, slice.Value, slice.Label, slice.Value/total*100);
                    int sign = Math.Sign(Math.Cos(midAngleRadians));

                    // tick points
                    var tp0 = new ScreenPoint(mp.X + (outerRadius + TickDistance)*Math.Cos(midAngleRadians),
                                              mp.Y + (outerRadius + TickDistance)*Math.Sin(midAngleRadians));
                    var tp1 = new ScreenPoint(tp0.X + TickRadialLength*Math.Cos(midAngleRadians),
                                              tp0.Y + TickRadialLength*Math.Sin(midAngleRadians));
                    var tp2 = new ScreenPoint(tp1.X + TickHorizontalLength*sign, tp1.Y);
                    rc.DrawLine(new[] {tp0, tp1, tp2}, Stroke, StrokeThickness, null, OxyPenLineJoin.Bevel);

                    // label
                    var labelPosition = new ScreenPoint(tp2.X + TickLabelDistance*sign, tp2.Y);
                    rc.DrawText(labelPosition, label, model.TextColor, model.LegendFont, model.LegendFontSize, 500, 0,
                                sign > 0 ? HorizontalTextAlign.Left : HorizontalTextAlign.Right,
                                VerticalTextAlign.Middle);
                }

                // Render label inside the slice
                if (InsideLabelFormat != null)
                {
                    string label = String.Format(InsideLabelFormat, slice.Value, slice.Label, slice.Value/total*100);
                    double r = innerRadius*(1 - InsideLabelPosition) + outerRadius*InsideLabelPosition;
                    var labelPosition = new ScreenPoint(mp.X + r*Math.Cos(midAngleRadians),
                                                        mp.Y + r*Math.Sin(midAngleRadians));
                    double textAngle = 0;
                    if (AreInsideLabelsAngled)
                    {
                        textAngle = midAngle;
                        if (Math.Cos(midAngleRadians) < 0)
                        {
                            textAngle += 180;
                        }
                    }

                    rc.DrawText(labelPosition, label, model.TextColor, model.LegendFont, model.LegendFontSize, 500,
                                textAngle, HorizontalTextAlign.Center, VerticalTextAlign.Middle);
                }
            }
        }

        #endregion
    }
}