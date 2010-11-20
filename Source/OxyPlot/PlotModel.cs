using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace OxyPlot
{
    public enum PlotType
    {
        Cartesian,
        Polar
    } ;

    public class PlotModel
    {
        public const string DEFAULT_FONT = "Segoe UI";
        internal Axis DefaultAngleAxis;
        internal Axis DefaultMagnitudeAxis;
        internal Axis DefaultXAxis;
        internal Axis DefaultYAxis;
        internal OxyRect bounds;

        private int currentColorIndex;
        internal ScreenPoint midPoint;

        public PlotModel(string title = null, string subtitle = null)
        {
            Title = title;
            Subtitle = subtitle;

            PlotType = PlotType.Cartesian;
            Axes = new Collection<Axis>();
            Series = new Collection<DataSeries>();

            LegendPosition = LegendPosition.TopRight;
            IsLegendOutsidePlotArea = false;

            AxisMargins = new OxyThickness(60, 60, 50, 50);

            TitleFont = DEFAULT_FONT;
            TitleFontSize = 18;
            SubtitleFontSize = 14;
            TitleFontWeight = 800;
            SubtitleFontWeight = 500;
            TextColor = OxyColors.Black;
            BoxColor = OxyColors.Black;
            BoxThickness = 1;

            LegendFont = DEFAULT_FONT;
            LegendFontSize = 12;
            LegendLineLength = 16;

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

        public LegendPosition LegendPosition { get; set; }
        public bool IsLegendOutsidePlotArea { get; set; }

        public PlotType PlotType { get; set; }

        public List<OxyColor> DefaultColors { get; set; }

        public Collection<Axis> Axes { get; set; }
        public Collection<DataSeries> Series { get; set; }

        public string TitleFont { get; set; }
        public double TitleFontSize { get; set; }
        public double SubtitleFontSize { get; set; }
        public double TitleFontWeight { get; set; }
        public double SubtitleFontWeight { get; set; }

        public string LegendFont { get; set; }
        public double LegendFontSize { get; set; }
        public double LegendLineLength { get; set; }

        //public double MarginLeft { get; set; }
        //public double MarginRight { get; set; }
        //public double MarginTop { get; set; }
        //public double MarginBottom { get; set; }
        public OxyThickness AxisMargins { get; set; }

        public OxyColor TextColor { get; set; }

        public OxyColor BoxColor { get; set; }
        public OxyColor Background { get; set; }
        public double BoxThickness { get; set; }

        public string Title { get; set; }
        public string Subtitle { get; set; }

        public bool CartesianAxes { get; set; }

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

            bounds = new OxyRect
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
            midPoint = new ScreenPoint((bounds.Left + bounds.Right) * 0.5, (bounds.Top + bounds.Bottom) * 0.5);
        }

        public void RenderAxes(IRenderContext rc)
        {
            // Update the transforms
            foreach (Axis a in Axes)
            {
                a.UpdateTransform(bounds);
            }

            // Set the same scaling to all axes if CartesianAxes is selected
            if (CartesianAxes)
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
                a.UpdateIntervals(bounds.Width, bounds.Height);
            }

            foreach (Axis a in Axes)
            {
                if (a.IsVisible)
                {
                    a.Render(rc, this);
                }
            }
        }

        public void RenderBox(IRenderContext rc)
        {
            var pp = new PlotRenderer(rc, this);
            pp.RenderRect(bounds, Background, BoxColor, BoxThickness);
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
                pp.RenderRect(axisBounds, ls.Background, null, 0);
            }
            pp.RenderLegends();
        }

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

        public void SaveSvg(string fileName, double width, double height)
        {
            using (var svgrc = new SvgRenderContext(fileName, width, height))
            {
                Render(svgrc);
            }
        }

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