using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace OxyPlot
{
    /// <summary>
    ///   Plot coordinate system type
    /// </summary>
    public enum PlotType
    {
        /// <summary>
        /// XY coordinate system - two perpendicular axes
        /// </summary>
        XY,

        /// <summary>
        /// Cartesian coordinate system - perpendicular axes with the same unit length
        /// http://en.wikipedia.org/wiki/Cartesian_coordinate_system
        /// </summary>
        Cartesian,

        /// <summary>
        /// Polar coordinate system - distance and angle axes
        /// http://en.wikipedia.org/wiki/Polar_coordinate_system
        /// </summary>
        Polar
    }


    /// <summary>
    /// The PlotModel represents all the content of the plot (titles, axes, series).
    /// </summary>
    public class PlotModel
    {
        private static string defaultFont = "Segoe UI";
        internal IAxis DefaultAngleAxis;
        internal IAxis DefaultMagnitudeAxis;
        internal IAxis DefaultXAxis;
        internal IAxis DefaultYAxis;

        public OxyRect PlotArea { get; private set; }

        private int currentColorIndex;

        public ScreenPoint MidPoint { get; private set; }
        // The midpoint of the plot area, used for the polar coordinate system.

        public PlotModel(string title = null, string subtitle = null)
        {
            Axes = new Collection<IAxis>();
            Series = new Collection<ISeries>();

            Title = title;
            Subtitle = subtitle;

            // Default values
            PlotType = PlotType.XY;

            LegendPosition = LegendPosition.TopRight;
            IsLegendOutsidePlotArea = false;

            PlotMargins = new OxyThickness(60, 60, 50, 50);

            TitleFont = DefaultFont;
            TitleFontSize = 18;
            SubtitleFontSize = 14;
            TitleFontWeight = 800;
            SubtitleFontWeight = 500;
            TextColor = OxyColors.Black;
            BoxColor = OxyColors.Black;
            BoxThickness = 1;

            LegendFont = DefaultFont;
            LegendFontSize = 12;
            LegendSymbolLength = 16;

            DefaultColors = new List<OxyColor>
                                {
                                    OxyColor.FromRGB(0x4E, 0x9A, 0x06),
                                    OxyColor.FromRGB(0xC8, 0x8D, 0x00),
                                    OxyColor.FromRGB(0xCC, 0x00, 0x00),
                                    OxyColor.FromRGB(0x20, 0x4A, 0x87),
                                    OxyColors.Red,
                                    OxyColors.Orange,
                                    OxyColors.Yellow,
                                    OxyColors.Green,
                                    OxyColors.Blue,
                                    OxyColors.Indigo,
                                    OxyColors.Violet
                                };
        }

        /// <summary>
        ///   This is the default font for text on axes, series, legends and plot title.
        /// </summary>
        public static string DefaultFont
        {
            get { return defaultFont; }
        }

        /// <summary>
        /// Gets or sets the legend position.
        /// </summary>
        /// <value>The legend position.</value>
        public LegendPosition LegendPosition { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the legend 
        /// should be shown outside the plot area. The default
        /// value is false - legend should be shown inside the plot area.
        /// </summary>
        public bool IsLegendOutsidePlotArea { get; set; }

        /// <summary>
        /// Gets or sets the type of the plot.
        /// </summary>
        /// <value>The type of the plot.</value>
        public PlotType PlotType { get; set; }

        /// <summary>
        /// Gets or sets the default colors.
        /// </summary>
        /// <value>The default colors.</value>
        public List<OxyColor> DefaultColors { get; set; }

        /// <summary>
        /// Gets or sets the axes.
        /// </summary>
        /// <value>The axes.</value>
        public Collection<IAxis> Axes { get; set; }

        /// <summary>
        /// Gets or sets the series.
        /// </summary>
        /// <value>The series.</value>
        public Collection<ISeries> Series { get; set; }

        /// <summary>
        /// Gets or sets the title font.
        /// </summary>
        /// <value>The title font.</value>
        public string TitleFont { get; set; }

        /// <summary>
        /// Gets or sets the size of the title font.
        /// </summary>
        /// <value>The size of the title font.</value>
        public double TitleFontSize { get; set; }

        /// <summary>
        /// Gets or sets the size of the subtitle font.
        /// </summary>
        /// <value>The size of the subtitle font.</value>
        public double SubtitleFontSize { get; set; }

        /// <summary>
        /// Gets or sets the title font weight.
        /// </summary>
        /// <value>The title font weight.</value>
        public double TitleFontWeight { get; set; }

        /// <summary>
        /// Gets or sets the subtitle font weight.
        /// </summary>
        /// <value>The subtitle font weight.</value>
        public double SubtitleFontWeight { get; set; }

        /// <summary>
        /// Gets or sets the legend font.
        /// </summary>
        /// <value>The legend font.</value>
        public string LegendFont { get; set; }

        /// <summary>
        /// Gets or sets the size of the legend font.
        /// </summary>
        /// <value>The size of the legend font.</value>
        public double LegendFontSize { get; set; }

        /// <summary>
        /// Gets or sets the length of the legend symbol.
        /// </summary>
        public double LegendSymbolLength { get; set; }

        /// <summary>
        /// Gets or sets the axis margins.
        /// </summary>
        /// <value>The axis margins.</value>
        public OxyThickness PlotMargins { get; set; }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        /// <value>The color of the text.</value>
        public OxyColor TextColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the box.
        /// </summary>
        /// <value>The color of the box.</value>
        public OxyColor BoxColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the background of the plot area.
        /// </summary>
        public OxyColor Background { get; set; }

        /// <summary>
        /// Gets or sets the box thickness.
        /// </summary>
        /// <value>The box thickness.</value>
        public double BoxThickness { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the subtitle.
        /// </summary>
        /// <value>The subtitle.</value>
        public string Subtitle { get; set; }

        private void ResetDefaultColor()
        {
            currentColorIndex = 0;
        }

        private OxyColor GetDefaultColor()
        {
            if (currentColorIndex >= DefaultColors.Count)
                currentColorIndex = 0;
            return DefaultColors[currentColorIndex++];
        }

        /// <summary>
        /// Force an update of the data.
        /// </summary>
        public void UpdateData()
        {
            foreach (var s in Series)
            {
                s.UpdateData();
            }

            // Ensure that there are default axes available
            EnsureDefaultAxes();
            UpdateMaxMin();
        }

        /// <summary>
        /// Find the default x/y axes (first in collection)
        /// </summary>
        private void EnsureDefaultAxes()
        {
            DefaultXAxis = null;
            DefaultYAxis = null;

            foreach (var a in Axes)
            {
                if (a.IsHorizontal())
                {
                    if (DefaultXAxis == null)
                    {
                        DefaultXAxis = a;
                    }
                }
                if (a.IsVertical())
                {
                    if (DefaultYAxis == null)
                    {
                        DefaultYAxis = a;
                    }
                }
                if (a.Position == AxisPosition.Magnitude)
                {
                    if (DefaultMagnitudeAxis == null)
                    {
                        DefaultMagnitudeAxis = a;
                    }
                }
                if (a.Position == AxisPosition.Angle)
                {
                    if (DefaultAngleAxis == null)
                        DefaultAngleAxis = a;
                }
            }
            if (DefaultXAxis == null)
                DefaultXAxis = DefaultMagnitudeAxis;
            if (DefaultYAxis == null)
                DefaultYAxis = DefaultAngleAxis;

            if (PlotType == PlotType.Polar)
            {
                if (DefaultXAxis == null)
                {
                    DefaultXAxis = DefaultMagnitudeAxis = new LinearAxis { Position = AxisPosition.Magnitude };
                }

                if (DefaultYAxis == null)
                {
                    DefaultYAxis = DefaultAngleAxis = new LinearAxis { Position = AxisPosition.Angle };
                }
            }
            else
            {
                if (DefaultXAxis == null)
                {
                    DefaultXAxis = new LinearAxis { Position = AxisPosition.Bottom };
                }

                if (DefaultYAxis == null)
                {
                    DefaultYAxis = new LinearAxis { Position = AxisPosition.Left };
                }
            }

            if (!Axes.Contains(DefaultXAxis))
                Axes.Add(DefaultXAxis);
            if (!Axes.Contains(DefaultYAxis))
                Axes.Add(DefaultYAxis);

            // Update the x/y axes of series without axes defined
            foreach (var s in Series)
            {
                s.EnsureAxes(Axes, DefaultXAxis, DefaultYAxis);
            }
        }

        /// <summary>
        /// Update max and min values of the axes from values of all data series.
        /// Only axes with automatic set to true are changed.
        /// </summary>
        private void UpdateMaxMin()
        {
            //foreach (var a in Axes)
            //{
            //    a.ActualMaximum = double.NaN;
            //    a.ActualMinimum = double.NaN;
            //}
            foreach (var s in Series)
            {
                s.UpdateMaxMin();
            }
            foreach (var a in Axes)
            {
                a.UpdateActualMaxMin();
            }
        }

        public void Render(IRenderContext rc)
        {
            RenderInit(rc);

            RenderAxes(rc);
            RenderSeries(rc);
            RenderBox(rc);
        }

        public void RenderInit(IRenderContext rc)
        {
            double w = rc.Width - PlotMargins.Left - PlotMargins.Right;
            double h = rc.Height - PlotMargins.Top - PlotMargins.Bottom;
            if (w < 0) w = 0;
            if (h < 0) h = 0;

            PlotArea = new OxyRect
                           {
                               Left = PlotMargins.Left,
                               Width = w,
                               Top = PlotMargins.Top,
                               Height = h
                           };


            // todo...
            /*
            // check the length of the maximum/minimum labels and 
            // extended margins if neccessary
            foreach (var axis in Axes)
            {
                if (!axis.IsVertical())
                    continue;

                double x = axis.ActualMaximum;
                x = (int)(x / axis.ActualMajorStep)*axis.ActualMajorStep;
                x = Axis.RemoveNoiseFromDoubleMath(x);
                 
                var smax = axis.FormatValue(x);
                var smin = axis.FormatValue(x);
                var sizeMax = rc.MeasureText(smax, axis.FontFamily, axis.FontSize, axis.FontWeight);
                var sizeMin = rc.MeasureText(smin, axis.FontFamily, axis.FontSize, axis.FontWeight);
                var maxWidth = Math.Max(sizeMax.Width, sizeMin.Width)+axis.MajorTickSize*2;

                if (axis.Position == AxisPosition.Left)
                {
                    if (maxWidth > bounds.Left)
                    {
                        double r = bounds.Right;
                        bounds.Left = maxWidth;
                        bounds.Right = r;
                    }
                }
                if (axis.Position == AxisPosition.Right)
                {
                    if (maxWidth > rc.Width - bounds.Right)
                        bounds.Right = rc.Width - maxWidth;
                }
            }
              */
            MidPoint = new ScreenPoint((PlotArea.Left + PlotArea.Right) * 0.5, (PlotArea.Top + PlotArea.Bottom) * 0.5);
        }

        /// <summary>
        /// Renders the axes.
        /// </summary>
        /// <param name="rc">The rc.</param>
        public void RenderAxes(IRenderContext rc)
        {
            // Update the transforms
            foreach (var a in Axes)
            {
                a.UpdateTransform(PlotArea);
            }

            // Set the same scaling to all axes if CartesianAxes is selected
            if (PlotType == PlotType.Cartesian)
            {
                double minimumScale = double.MaxValue;
                foreach (var a in Axes)
                    minimumScale = Math.Min(minimumScale, Math.Abs(a.Scale));
                foreach (var a in Axes)
                    a.SetScale(minimumScale);
            }

            // Update the intervals for all axes
            foreach (var a in Axes)
            {
                a.UpdateIntervals(PlotArea.Width, PlotArea.Height);
            }

            foreach (var a in Axes)
            {
                if (a.IsVisible)
                {
                    a.Render(rc, this);
                }
            }
        }

        /// <summary>
        /// Renders the box around the plot area.
        /// </summary>
        /// <param name="rc">The rc.</param>
        public void RenderBox(IRenderContext rc)
        {
            var pp = new PlotRenderingHelper(rc, this);
            rc.DrawRectangle(PlotArea, Background, BoxColor, BoxThickness);
            pp.RenderTitle(Title, Subtitle);
            foreach (var s in Series)
            {
                var ls = s as LineSeries;
                if (ls == null)
                    continue;
                if (ls.Background == null)
                    continue;
                var axisBounds = new OxyRect(
                    ls.XAxis.ScreenMin.X,
                    ls.YAxis.ScreenMin.Y,
                    ls.XAxis.ScreenMax.X - ls.XAxis.ScreenMin.X,
                    ls.YAxis.ScreenMax.Y - ls.YAxis.ScreenMin.Y);
                rc.DrawRectangle(axisBounds, ls.Background, null, 0);
            }
            pp.RenderLegends();
        }

        /// <summary>
        /// Renders the series.
        /// </summary>
        /// <param name="rc">The rc.</param>
        public void RenderSeries(IRenderContext rc)
        {
            ResetDefaultColor();
            foreach (var s in Series)
            {
                var ls = s as LineSeries;
                if (ls != null && ls.Color == null)
                {
                    ls.Color = ls.MarkerFill = GetDefaultColor();
                }

                s.Render(rc, this);
            }
        }

        /// <summary>
        /// Saves the SVG.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public void SaveSvg(string fileName, double width, double height)
        {
            using (var svgrc = new SvgRenderContext(fileName, width, height))
            {
                Render(svgrc);
            }
        }

        /// <summary>
        /// Create an SVG model and return it as a string.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="isDocument">if set to <c>true</c> [is document].</param>
        /// <returns></returns>
        public string ToSvg(double width, double height, bool isDocument = false)
        {
            string svg;
            using (var ms = new MemoryStream())
            {
                var svgrc = new SvgRenderContext(ms, width, height, isDocument);
                Render(svgrc);
                svgrc.Complete();
                svgrc.Flush();
                ms.Flush();
                ms.Position = 0;
                var sr = new StreamReader(ms);
                svg = sr.ReadToEnd();
                svgrc.Close();
            }
            return svg;
        }

        public void GetAxesFromPoint(ScreenPoint pt, out IAxis xaxis, out IAxis yaxis)
        {
            xaxis = yaxis = null;
            foreach (var axis in Axes)
            {
                double x = axis.InverseTransform(axis.IsHorizontal() ? pt.X : pt.Y);
                if (x >= axis.ActualMinimum && x <= axis.ActualMaximum)
                {
                    if (axis.IsHorizontal())
                    {
                        // todo: only accept axis if it is within the plot area or the axis area
                        xaxis = axis;
                    }
                    else
                        yaxis = axis;
                }
            }
        }

        public ISeries GetSeriesFromPoint(ScreenPoint point, double limit = 100)
        {
            double mindist = double.MaxValue;
            ISeries closest = null;
            foreach (var s in Series)
            {
                ScreenPoint sp;
                DataPoint dp;
                if (!s.GetNearestInterpolatedPoint(point, out dp, out sp))
                    continue;

                // find distance to this point on the screen
                double dist = point.DistanceTo(sp);
                if (dist < mindist)
                {
                    closest = s;
                    mindist = dist;
                }
            }
            if (mindist < limit)
                return closest;
            return null;
        }
    }
}