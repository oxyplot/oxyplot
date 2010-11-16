using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace OxyPlot
{
    public enum PlotType { Cartesian, Polar };

    public class PlotModel
    {
        public PlotType PlotType { get; set; }
        private int currentColorIndex;
        internal Axis DefaultXAxis;
        internal Axis DefaultYAxis;
        internal Axis DefaultAngleAxis;
        internal Axis DefaultMagnitudeAxis;

        public PlotModel(string title = null, string subtitle = null)
        {
            Title = title;
            Subtitle = subtitle;

            PlotType = PlotType.Cartesian;
            Axes = new Collection<Axis>();
            Series = new Collection<DataSeries>();
            MarginLeft = 60;
            MarginTop = 60;
            MarginBottom = 50;
            MarginRight = 30;
            TitleFont = "Segoe UI";
            TitleFontSize = 18;
            SubtitleFontSize = 14;
            TitleFontWeight = 800;
            SubtitleFontWeight = 500;
            TextColor = Colors.Black;
            BorderColor = Colors.Black;
            BorderThickness = 2;

            LegendFont = "Segoe UI";
            LegendFontSize = 12;
            LegendLineLength = 16;

            DefaultColors = new List<Color>
                                {
                                    Color.FromRGB(0x4E, 0x9A, 0x06),
                                    Color.FromRGB(0xC8, 0x8D, 0x00),
                                    Color.FromRGB(0xCC, 0x00, 0x00),
                                    Color.FromRGB(0x20, 0x4A, 0x87),
                                    Colors.Red,
                                    Colors.Orange,
                                    Colors.Yellow,
                                    Colors.Green,
                                    Colors.Blue,
                                    Colors.Indigo,
                                    Colors.Violet
                                };
        }

        public List<Color> DefaultColors { get; set; }

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

        public double MarginLeft { get; set; }
        public double MarginRight { get; set; }
        public double MarginTop { get; set; }
        public double MarginBottom { get; set; }
        public double Margins { set { MarginLeft = MarginRight = MarginTop = MarginBottom = value; } }

        public Color TextColor { get; set; }

        public Color BorderColor { get; set; }
        public double BorderThickness { get; set; }

        public string Title { get; set; }
        public string Subtitle { get; set; }

        public bool CartesianAxes { get; set; }

        public Color GetDefaultColor()
        {
            if (currentColorIndex >= DefaultColors.Count)
                currentColorIndex = 0;
            return DefaultColors[currentColorIndex++];
        }

        public void UpdateData()
        {
            foreach (var s in Series)
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
            foreach (var a in Axes)
            {
                if (a.IsHorizontal)
                {
                    if (DefaultXAxis == null)
                    {
                        DefaultXAxis = a;
                    }
                }
                if (a.IsVertical)
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
            foreach (var a in Axes)
            {
                a.ActualMaximum = double.NaN;
                a.ActualMinimum = double.NaN;
            }
            foreach (var s in Series)
            {
                s.UpdateMaxMin();
                s.XAxis.Include(s.MinX);
                s.XAxis.Include(s.MaxX);
                s.YAxis.Include(s.MinY);
                s.YAxis.Include(s.MaxY);
            }
            foreach (Axis a in Axes)
            {
                if (!double.IsNaN(a.Maximum))
                {
                    a.ActualMaximum = a.Maximum;
                }
                else
                {
                    a.ActualMaximum += a.MaximumPadding * (a.ActualMaximum - a.ActualMinimum);
                }
                if (!double.IsNaN(a.Minimum))
                {
                    a.ActualMinimum = a.Minimum;
                }
                else
                {
                    a.ActualMinimum -= a.MinimumPadding * (a.ActualMaximum - a.ActualMinimum);
                }

                if (double.IsNaN(a.ActualMaximum))
                {
                    a.ActualMaximum = 100;
                }
                if (double.IsNaN(a.ActualMinimum))
                {
                    a.ActualMinimum = 0;
                }
            }
        }

        public void Render(IRenderContext rc)
        {
            double x0 = MarginLeft;
            double x1 = rc.Width - MarginRight;
            double y0 = rc.Height - MarginBottom;
            double y1 = MarginTop;

            // Update the transforms
            double minScale = double.MaxValue;
            foreach (var a in Axes)
            {
                double s = a.UpdateTransform(x0, x1, y0, y1);
                minScale = Math.Min(minScale, Math.Abs(s));
            }

            if (CartesianAxes)
            {
                foreach (var a in Axes)
                    a.SetScale(minScale);
            }

            foreach (var a in Axes)
            {
                a.UpdateIntervals(x1 - x0, y0 - y1);
            }
            var pp = new PlotRenderer(rc, this);
            var ap = new AxisRenderer(rc, this);
            var sp = new SeriesRenderer(rc, this);

            foreach (var a in Axes)
            {
                if (a.IsVisible)
                {
                    ap.Render(a);
                }
            }

            currentColorIndex = 0;
            foreach (var s in Series)
            {
                var ls = s as LineSeries;
                if (ls != null && ls.Color == null)
                {
                    ls.Color = ls.MarkerFill = GetDefaultColor();
                }

                sp.Render(s);
            }

            var border = new[]
                             {
                                 new Point(x0, y0), new Point(x1, y0), 
                                 new Point(x1, y1), new Point(x0, y1),
                                 new Point(x0, y0)
                             };

            if (!Equals(BorderColor, Colors.Transparent) && BorderThickness > 0)
                rc.DrawPolygon(border, null, BorderColor, BorderThickness);

            pp.RenderTitle(Title, Subtitle);

            sp.RenderLegends();

        }

        public void SaveSvg(string fileName, double width, double height)
        {
            using (var svgrc = new SvgRenderContext(fileName, width, height))
            {
                Render(svgrc);
            }
        }

        public string ToSvg(double width, double height)
        {
            var ms = new MemoryStream();
            var svgrc = new SvgRenderContext(ms, width, height, false);
            Render(svgrc);
            svgrc.Flush();
            ms.Flush();
            ms.Position = 0;
            var sr = new StreamReader(ms);
            var svg = sr.ReadToEnd();
            svgrc.Close();
            ms.Close();
            return svg;
        }


    }
}