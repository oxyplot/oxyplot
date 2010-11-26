using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace OxyPlot
{
    /// <summary>
    /// Plot type
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
        /// Polar coordinate system - distance&angle axes
        /// http://en.wikipedia.org/wiki/Polar_coordinate_system
        /// </summary>
        Polar
    } ;

    /// <summary>
    /// PlotModel
    /// </summary>
    public class PlotModel
    {
        /// <summary>
        /// This is the default font for all text in the plots.
        /// </summary>
        public static string DefaultFont = "Segoe UI";

        internal Axis DefaultAngleAxis;
        internal Axis DefaultMagnitudeAxis;
        internal Axis DefaultXAxis;
        internal Axis DefaultYAxis;

        /// <summary>
        /// The Bounds rectangle is defining the rectangular 
        /// area inside the margins.
        /// </summary>
        internal OxyRect Bounds;

        private int currentColorIndex;

        /// <summary>
        /// The midpoint of the plot area, used for the polar coordinate system.
        /// </summary>
        internal ScreenPoint MidPoint;

        public PlotModel(string title = null, string subtitle = null)
        {
            Axes = new Collection<Axis>();
            Series = new Collection<DataSeries>();

            Title = title;
            Subtitle = subtitle;

            // Default values
            PlotType = PlotType.XY;

            LegendPosition = LegendPosition.TopRight;
            IsLegendOutsidePlotArea = false;

            AxisMargins = new OxyThickness(60, 60, 50, 50);

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
        public Collection<Axis> Axes { get; set; }

        /// <summary>
        /// Gets or sets the series.
        /// </summary>
        /// <value>The series.</value>
        public Collection<DataSeries> Series { get; set; }

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
        public OxyThickness AxisMargins { get; set; }

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
            foreach (DataSeries s in Series)
            {
                s.UpdatePointsFromItemsSource();
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

            foreach (Axis a in Axes)
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
            foreach (DataSeries s in Series)
            {
                if (s.XAxisKey != null)
                {
                    s.XAxis = FindAxis(s.XAxisKey);
                }
                if (s.YAxisKey != null)
                {
                    s.YAxis = FindAxis(s.YAxisKey);
                }
                // If axes are not found, use the default axes
                if (s.XAxis == null)
                {
                    s.XAxis = DefaultXAxis;
                }
                if (s.YAxis == null)
                {
                    s.YAxis = DefaultYAxis;
                }
            }
        }

        private Axis FindAxis(string key)
        {
            return Axes.FirstOrDefault(a => a.Key == key);
        }

        /// <summary>
        /// Update max and min values of the axes from values of all data series.
        /// Only axes with automatic set to true are changed.
        /// </summary>
        private void UpdateMaxMin()
        {
            foreach (Axis a in Axes)
            {
                a.ActualMaximum = double.NaN;
                a.ActualMinimum = double.NaN;
            }
            foreach (DataSeries s in Series)
            {
                s.UpdateMaxMin();
                s.XAxis.Include(s.MinX);
                s.XAxis.Include(s.MaxX);
                s.YAxis.Include(s.MinY);
                s.YAxis.Include(s.MaxY);
            }
            foreach (Axis a in Axes)
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

            Bounds = new OxyRect
                         {
                             Left = AxisMargins.Left,
                             Width = rc.Width - AxisMargins.Left - AxisMargins.Right,
                             Top = AxisMargins.Top,
                             Height = rc.Height - AxisMargins.Top - AxisMargins.Bottom
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
            MidPoint = new ScreenPoint((Bounds.Left + Bounds.Right) * 0.5, (Bounds.Top + Bounds.Bottom) * 0.5);
        }

        /// <summary>
        /// Renders the axes.
        /// </summary>
        /// <param name="rc">The rc.</param>
        public void RenderAxes(IRenderContext rc)
        {
            // Update the transforms
            foreach (Axis a in Axes)
            {
                a.UpdateTransform(Bounds);
            }

            // Set the same scaling to all axes if CartesianAxes is selected
            if (this.PlotType == PlotType.Cartesian)
            {
                double minimumScale = double.MaxValue;
                foreach (Axis a in Axes)
                    minimumScale = Math.Min(minimumScale, Math.Abs(a.Scale));
                foreach (Axis a in Axes)
                    a.SetScale(minimumScale);
            }

            // Update the intervals for all axes
            foreach (Axis a in Axes)
            {
                a.UpdateIntervals(Bounds.Width, Bounds.Height);
            }

            foreach (Axis a in Axes)
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
            rc.DrawRectangle(Bounds, Background, BoxColor, BoxThickness);
            pp.RenderTitle(Title, Subtitle);
            foreach (DataSeries s in Series)
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
            foreach (DataSeries s in Series)
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
    }
}