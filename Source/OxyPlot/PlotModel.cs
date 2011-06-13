using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using OxyPlot.Reporting;

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
        /// Polar coordinate system - with radial and angular axes
        /// http://en.wikipedia.org/wiki/Polar_coordinate_system
        /// </summary>
        Polar
    }

    public enum LegendPlacement { Inside, Outside }
    public enum LegendPosition { TopLeft, TopCenter, TopRight, BottomLeft, BottomCenter, BottomRight, LeftTop, LeftMiddle, LeftBottom, RightTop, RightMiddle, RightBottom }
    public enum LegendOrientation { Horizontal, Vertical }
    public enum LegendItemOrder { Normal, Reverse }
    public enum LegendSymbolPlacement { Left, Right }

    /// <summary>
    /// The PlotModel represents all the content of the plot (titles, axes, series).
    /// </summary>
    public partial class PlotModel
    {
        private static string defaultFont = "Segoe UI";
        internal IAxis DefaultAngleAxis;
        internal IAxis DefaultMagnitudeAxis;
        internal IAxis DefaultXAxis;
        internal IAxis DefaultYAxis;
        private int currentColorIndex;

        public PlotModel(string title = null, string subtitle = null)
        {
            Axes = new Collection<IAxis>();
            Series = new Collection<ISeries>();
            Annotations = new Collection<IAnnotation>();

            Title = title;
            Subtitle = subtitle;

            PlotType = PlotType.XY;

            AxisTitleDistance = 4;
            AxisTickToLabelDistance = 4;

            PlotMargins = new OxyThickness(60, 10, 20, 40);
            PlotMargins = new OxyThickness(60, 4, 4, 40);
            Padding = new OxyThickness(8, 8, 16, 8);

            TitleFont = DefaultFont;
            TitleFontSize = 18;
            SubtitleFontSize = 14;
            TitleFontWeight = FontWeights.Bold;
            SubtitleFontWeight = FontWeights.Normal;
            TitlePadding = 6;

            TextColor = OxyColors.Black;
            BoxColor = OxyColors.Black;
            BoxThickness = 1;

            LegendTitleFont = null;
            LegendTitleFontSize = 12;
            LegendTitleFontWeight = FontWeights.Bold;
            LegendFont = null;
            LegendFontSize = 12;
            LegendFontWeight = FontWeights.Normal;
            LegendSymbolLength = 16;
            LegendSymbolMargin = 4;
            LegendPadding = 8;
            LegendItemSpacing = 24;
            LegendMargin = 8;

            LegendBackground = OxyColor.FromAColor(220, OxyColors.White);
            LegendBorder = OxyColors.Black;
            LegendBorderThickness = 1;

            LegendPlacement = LegendPlacement.Inside;
            LegendPosition = LegendPosition.RightTop;
            LegendOrientation = LegendOrientation.Vertical;
            LegendItemOrder = LegendItemOrder.Normal;
            LegendItemAlignment = HorizontalTextAlign.Left;
            LegendSymbolPlacement = LegendSymbolPlacement.Left;

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
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Title;
        }

        /// <summary>
        ///   This is the default font for text on axes, series, legends and plot title.
        /// </summary>
        public static string DefaultFont
        {
            get { return defaultFont; }
        }

        /// <summary>
        /// Gets or sets the legend title.
        /// </summary>
        /// <value>The legend title.</value>
        public string LegendTitle { get; set; }

        /// <summary>
        /// Gets or sets the legend placement.
        /// </summary>
        /// <value>The legend placement.</value>
        public LegendPlacement LegendPlacement { get; set; }

        /// <summary>
        /// Gets or sets the legend position.
        /// </summary>
        /// <value>The legend position.</value>
        public LegendPosition LegendPosition { get; set; }

        /// <summary>
        /// Gets or sets the legend orientation.
        /// </summary>
        /// <value>The legend orientation.</value>
        public LegendOrientation LegendOrientation { get; set; }

        /// <summary>
        /// Gets or sets the legend item order.
        /// </summary>
        /// <value>The legend item order.</value>
        public LegendItemOrder LegendItemOrder { get; set; }

        /// <summary>
        /// Gets or sets the legend item alignment.
        /// </summary>
        /// <value>The legend item alignment.</value>
        public HorizontalTextAlign LegendItemAlignment { get; set; }

        /// <summary>
        /// Gets or sets the legend symbol placement.
        /// </summary>
        /// <value>The legend symbol placement.</value>
        public LegendSymbolPlacement LegendSymbolPlacement { get; set; }

        /// <summary>
        /// Gets or sets the legend padding.
        /// </summary>
        /// <value>The legend padding.</value>
        public double LegendPadding { get; set; }

        /// <summary>
        /// Gets or sets the legend spacing.
        /// </summary>
        /// <value>The legend spacing.</value>
        public double LegendItemSpacing { get; set; }

        /// <summary>
        /// Gets or sets the legend column spacing.
        /// </summary>
        /// <value>The legend column spacing.</value>
        public double LegendColumnSpacing { get; set; }

        /// <summary>
        /// The area used to draw the plot, excluding axis labels and legends.
        /// </summary>
        public OxyRect PlotArea { get; private set; }

        /// <summary>
        /// Gets or sets the area including both the plot and the axes.
        /// Outside legends are rendered outside this rectangle.       
        /// </summary>
        /// <value>The plot and axis area.</value>
        public OxyRect PlotAndAxisArea { get; private set; }

        /// <summary>
        /// Gets or sets the title area.
        /// </summary>
        /// <value>The title area.</value>
        public OxyRect TitleArea { get; private set; }

        /// <summary>
        /// Gets or sets the legend area.
        /// </summary>
        /// <value>The legend area.</value>
        public OxyRect LegendArea { get; private set; }

        /// <summary>
        /// Gets or sets the distance from axis number to axis title.
        /// </summary>
        /// <value>The axis title distance.</value>
        public double AxisTitleDistance { get; set; }

        /// <summary>
        /// Gets or sets the distance from axis tick to number label.
        /// </summary>
        /// <value>The axis tick to label distance.</value>
        public double AxisTickToLabelDistance { get; set; }

        /// <summary>
        /// Gets or sets the type of the coordinate system.
        /// </summary>
        /// <value>The type of the plot.</value>
        public PlotType PlotType { get; set; }

        /// <summary>
        /// Gets or sets the default colors.
        /// </summary>
        /// <value>The default colors.</value>
        [Browsable(false)]
        public List<OxyColor> DefaultColors { get; set; }

        /// <summary>
        /// Gets or sets the axes.
        /// </summary>
        /// <value>The axes.</value>
        [Browsable(false)]
        public Collection<IAxis> Axes { get; set; }

        /// <summary>
        /// Gets or sets the series.
        /// </summary>
        /// <value>The series.</value>
        [Browsable(false)]
        public Collection<ISeries> Series { get; set; }

        /// <summary>
        /// Gets or sets the annotations.
        /// </summary>
        /// <value>The annotations.</value>
        [Browsable(false)]
        public Collection<IAnnotation> Annotations { get; set; }

        /// <summary>
        /// Gets or sets the title font.
        /// </summary>
        /// <value>The title font.</value>
        public string TitleFont { get; set; }

        /// <summary>
        /// Gets or sets the subtitle font.
        /// If this property is null, the Title font will be used.
        /// </summary>
        /// <value>The subtitle font.</value>
        public string SubtitleFont { get; set; }

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
        /// Gets or sets the legend font weight.
        /// </summary>
        /// <value>The legend font weight.</value>
        public double LegendFontWeight { get; set; }

        /// <summary>
        /// Gets or sets the legend title font.
        /// </summary>
        /// <value>The legend title font.</value>
        public string LegendTitleFont { get; set; }

        /// <summary>
        /// Gets or sets the size of the legend title font.
        /// </summary>
        /// <value>The size of the legend title font.</value>
        public double LegendTitleFontSize { get; set; }

        /// <summary>
        /// Gets or sets the legend title font weight.
        /// </summary>
        /// <value>The legend title font weight.</value>
        public double LegendTitleFontWeight { get; set; }

        /// <summary>
        /// Gets or sets the length of the legend symbols.
        /// </summary>
        public double LegendSymbolLength { get; set; }

        /// <summary>
        /// Gets or sets the legend margin.
        /// </summary>
        /// <value>The legend margin.</value>
        public double LegendMargin { get; set; }

        /// <summary>
        /// Gets or sets the legend symbol margins (distance between the symbol and the text).
        /// </summary>
        /// <value>The legend symbol margin.</value>
        public double LegendSymbolMargin { get; set; }

        /// <summary>
        /// Gets or sets the background color of the legend. Use null for no background.
        /// </summary>
        /// <value>The legend background.</value>
        public OxyColor LegendBackground { get; set; }

        /// <summary>
        /// Gets or sets the border color of the legend.
        /// </summary>
        /// <value>The legend border.</value>
        public OxyColor LegendBorder { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the legend border. Use 0 for no border.
        /// </summary>
        /// <value>The legend border thickness.</value>
        public double LegendBorderThickness { get; set; }

        /// <summary>
        /// Gets or sets the margins around the plot (this should be large enough to fit the axes).
        /// </summary>
        public OxyThickness PlotMargins { get; set; }

        /// <summary>
        /// Gets or sets the padding around the plot.
        /// </summary>
        /// <value>The padding.</value>
        public OxyThickness Padding { get; set; }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        /// <value>The color of the text.</value>
        public OxyColor TextColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the background of the plot area.
        /// </summary>
        public OxyColor Background { get; set; }

        /// <summary>
        /// Gets or sets the color of the border around the plot area.
        /// </summary>
        /// <value>The color of the box.</value>
        public OxyColor BoxColor { get; set; }

        /// <summary>
        /// Gets or sets the box thickness.
        /// </summary>
        /// <value>The box thickness.</value>
        public double BoxThickness { get; set; }

        /// <summary>
        /// Gets or sets the padding around the title.
        /// </summary>
        /// <value>The title padding.</value>
        public double TitlePadding { get; set; }

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

        public LineStyle GetDefaultLineStyle()
        {
            return (LineStyle)((currentColorIndex / DefaultColors.Count) % (int)LineStyle.None);
        }

        public OxyColor GetDefaultColor()
        {
            return DefaultColors[currentColorIndex++ % DefaultColors.Count];
        }

        /// <summary>
        /// Force an update of the data.
        /// 1. Updates the data of each Series.
        /// 2. Ensure that all series have axes assigned.
        /// 3. Updates the max and min of the axes.
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

            bool areAxesRequired = false;
            foreach (var s in Series)
            {
                if (s.AreAxesRequired())
                    areAxesRequired = true;
            }

            if (areAxesRequired)
            {
                if (!Axes.Contains(DefaultXAxis))
                    Axes.Add(DefaultXAxis);
                if (!Axes.Contains(DefaultYAxis))
                    Axes.Add(DefaultYAxis);
            }

            // Update the x/y axes of series without axes defined
            foreach (var s in Series)
            {
                if (s.AreAxesRequired())
                    s.EnsureAxes(Axes, DefaultXAxis, DefaultYAxis);
            }

            // Update the x/y axes of series without axes defined
            foreach (var s in Annotations)
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
            foreach (var a in Axes)
            {
                a.ActualMaximum = double.NaN;
                a.ActualMinimum = double.NaN;
            }
            foreach (var s in Series)
            {
                s.UpdateMaxMin();
            }
            foreach (var a in Axes)
            {
                a.UpdateActualMaxMin();
            }
        }

        /// <summary>
        /// Updates the axis transforms and intervals.
        /// This is used after pan/zoom.
        /// </summary>
        public void UpdateAxisTransforms()
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
                a.UpdateIntervals(PlotArea);
            }

        }

        /// <summary>
        /// Saves the plot to a SVG file.
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

        /// <summary>
        /// Gets the first axes that covers the area of the specified point.
        /// </summary>
        /// <param name="pt">The pointt.</param>
        /// <param name="xaxis">The xaxis.</param>
        /// <param name="yaxis">The yaxis.</param>
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
                        xaxis = axis;
                    }
                    else
                    {
                        yaxis = axis;
                    }
                }
            }
        }

        /// <summary>
        /// Gets a series from the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="limit">The limit.</param>
        /// <returns></returns>
        public ISeries GetSeriesFromPoint(ScreenPoint point, double limit = 100)
        {
            double mindist = double.MaxValue;
            ISeries closest = null;
            foreach (var s in Series)
            {
                var ts = s as ITrackableSeries;
                if (ts == null)
                    continue;
                ScreenPoint sp;
                DataPoint dp;
                if (!ts.GetNearestInterpolatedPoint(point, out dp, out sp))
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

        /// <summary>
        /// Creates a report for the plot.
        /// </summary>
        /// <returns></returns>
        public Report CreateReport()
        {
            var r = new Report();

            r.AddHeader(1, "P L O T   R E P O R T");
            r.AddHeader(2, "=== PlotModel ===");
            r.AddPropertyTable("PlotModel", this);

            r.AddHeader(2, "=== Axes ===");
            foreach (var a in Axes)
                r.AddPropertyTable(a.GetType().Name, a);

            r.AddHeader(2, "=== Annotations ===");
            foreach (var a in Annotations)
                r.AddPropertyTable(a.GetType().Name, a);

            r.AddHeader(2, "=== Series ===");
            foreach (var s in Series)
            {
                r.AddPropertyTable(s.GetType().Name, s);
                var ds = s as DataSeries;
                if (ds != null)
                {
                    var fields = new List<ItemsTableField>
                                     {
                                         new ItemsTableField("X", "X"),
                                         new ItemsTableField("Y", "Y")
                                     };
                    r.AddItemsTable("Data", ds.Points, fields);
                }
            }
            r.AddParagraph(string.Format("Report generated by OxyPlot {0}, {1:u}", Assembly.GetExecutingAssembly().GetName().Version, DateTime.Now));
            return r;
        }

        /// <summary>
        /// Creates a text report for the plot.
        /// </summary>
        /// <returns></returns>
        public string CreateTextReport()
        {
            using (var ms = new MemoryStream())
            {
                using (var trw = new TextReportWriter(ms))
                {
                    var report = CreateReport();
                    report.Write(trw);
                    trw.Flush();
                    ms.Position = 0;
                    var r = new StreamReader(ms);
                    return r.ReadToEnd();
                }
            }
        }
    }
}