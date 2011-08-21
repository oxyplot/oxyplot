// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotModel.cs" company="OxyPlot">
//   See http://oxyplot.codeplex.com
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Reflection;
    using System.Xml.Serialization;

    using OxyPlot.Reporting;

    /// <summary>
    /// Plot coordinate system type
    /// </summary>
    public enum PlotType
    {
        /// <summary>
        /// XY coordinate system - two perpendicular axes
        /// </summary>
        XY, 

        /// <summary>
        /// Cartesian coordinate system - perpendicular axes with the same scaling
        /// http://en.wikipedia.org/wiki/Cartesian_coordinate_system
        /// </summary>
        Cartesian, 

        /// <summary>
        /// Polar coordinate system - with radial and angular axes
        /// http://en.wikipedia.org/wiki/Polar_coordinate_system
        /// </summary>
        Polar
    }

    /// <summary>
    /// Legend placement enumeration.
    /// </summary>
    public enum LegendPlacement
    {
        /// <summary>
        /// The inside.
        /// </summary>
        Inside, 

        /// <summary>
        /// The outside.
        /// </summary>
        Outside
    }

    /// <summary>
    /// Legend position enumeration.
    /// </summary>
    public enum LegendPosition
    {
        /// <summary>
        /// The top left.
        /// </summary>
        TopLeft, 

        /// <summary>
        /// The top center.
        /// </summary>
        TopCenter, 

        /// <summary>
        /// The top right.
        /// </summary>
        TopRight, 

        /// <summary>
        /// The bottom left.
        /// </summary>
        BottomLeft, 

        /// <summary>
        /// The bottom center.
        /// </summary>
        BottomCenter, 

        /// <summary>
        /// The bottom right.
        /// </summary>
        BottomRight, 

        /// <summary>
        /// The left top.
        /// </summary>
        LeftTop, 

        /// <summary>
        /// The left middle.
        /// </summary>
        LeftMiddle, 

        /// <summary>
        /// The left bottom.
        /// </summary>
        LeftBottom, 

        /// <summary>
        /// The right top.
        /// </summary>
        RightTop, 

        /// <summary>
        /// The right middle.
        /// </summary>
        RightMiddle, 

        /// <summary>
        /// The right bottom.
        /// </summary>
        RightBottom
    }

    /// <summary>
    /// Legend orientation enumeration.
    /// </summary>
    public enum LegendOrientation
    {
        /// <summary>
        /// The horizontal.
        /// </summary>
        Horizontal, 

        /// <summary>
        /// The vertical.
        /// </summary>
        Vertical
    }

    /// <summary>
    /// Legend item order enumeration.
    /// </summary>
    public enum LegendItemOrder
    {
        /// <summary>
        /// The normal.
        /// </summary>
        Normal, 

        /// <summary>
        /// The reverse.
        /// </summary>
        Reverse
    }

    /// <summary>
    /// Legend symbol placement enumeration.
    /// </summary>
    public enum LegendSymbolPlacement
    {
        /// <summary>
        /// The left.
        /// </summary>
        Left, 

        /// <summary>
        /// The right.
        /// </summary>
        Right
    }

    /// <summary>
    /// The PlotModel represents all the content of the plot (titles, axes, series).
    /// </summary>
    public partial class PlotModel
    {
        #region Constants and Fields

        /// <summary>
        /// The default font.
        /// </summary>
        private static string defaultFont = "Segoe UI";

        /// <summary>
        /// The current color index.
        /// </summary>
        private int currentColorIndex;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PlotModel"/> class.
        /// </summary>
        public PlotModel()
        {
            this.Axes = new Collection<IAxis>();
            this.Series = new Collection<ISeries>();
            this.Annotations = new Collection<IAnnotation>();

            this.PlotType = PlotType.XY;

            this.PlotMargins = new OxyThickness(60, 4, 4, 40);
            this.Padding = new OxyThickness(8, 8, 16, 8);
            this.AutoAdjustPlotMargins = true;

            this.TitleFont = null;
            this.TitleFontSize = 18;
            this.SubtitleFontSize = 14;
            this.TitleFontWeight = FontWeights.Bold;
            this.SubtitleFontWeight = FontWeights.Normal;
            this.TitlePadding = 6;

            this.TextColor = OxyColors.Black;
            this.BoxColor = OxyColors.Black;
            this.BoxThickness = 1;

            this.IsLegendVisible = true;
            this.LegendTitleFont = null;
            this.LegendTitleFontSize = 12;
            this.LegendTitleFontWeight = FontWeights.Bold;
            this.LegendFont = null;
            this.LegendFontSize = 12;
            this.LegendFontWeight = FontWeights.Normal;
            this.LegendSymbolLength = 16;
            this.LegendSymbolMargin = 4;
            this.LegendPadding = 8;
            this.LegendItemSpacing = 24;
            this.LegendMargin = 8;

            this.LegendBackground = null;
            this.LegendBorder = null;
            this.LegendBorderThickness = 1;

            this.LegendPlacement = LegendPlacement.Inside;
            this.LegendPosition = LegendPosition.RightTop;
            this.LegendOrientation = LegendOrientation.Vertical;
            this.LegendItemOrder = LegendItemOrder.Normal;
            this.LegendItemAlignment = HorizontalTextAlign.Left;
            this.LegendSymbolPlacement = LegendSymbolPlacement.Left;

            this.AnnotationFontSize = 12;

            this.DefaultColors = new List<OxyColor> {
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
        /// Initializes a new instance of the <see cref="PlotModel"/> class.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="subtitle">
        /// The subtitle.
        /// </param>
        public PlotModel(string title, string subtitle = null)
            : this()
        {
            this.Title = title;
            this.Subtitle = subtitle;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the default font.
        /// This font is used for text on axes, series, legends and plot title unless other fonts are specified.
        /// </summary>
        /// <value>The default font.</value>
        public static string DefaultFont
        {
            get
            {
                return defaultFont;
            }
        }

        /// <summary>
        /// Gets the actual annotation font.
        /// </summary>
        /// <value>The actual annotation font.</value>
        public string ActualAnnotationFont
        {
            get
            {
                return this.AnnotationFont ?? DefaultFont;
            }
        }

        /// <summary>
        /// Gets the actual legend font.
        /// </summary>
        /// <value>The actual legend font.</value>
        public string ActualLegendFont
        {
            get
            {
                return this.LegendFont ?? DefaultFont;
            }
        }

        /// <summary>
        /// Gets the actual legend title font.
        /// </summary>
        /// <value>The actual legend title font.</value>
        public string ActualLegendTitleFont
        {
            get
            {
                return this.LegendTitleFont ?? DefaultFont;
            }
        }

        /// <summary>
        /// Gets the actual plot margins.
        /// </summary>
        /// <value>The actual plot margins.</value>
        public OxyThickness ActualPlotMargins { get; private set; }

        /// <summary>
        /// Gets the actual title font.
        /// </summary>
        public string ActualTitleFont
        {
            get
            {
                return this.TitleFont ?? DefaultFont;
            }
        }

        /// <summary>
        /// Gets or sets the annotation font.
        /// </summary>
        /// <value>The annotation font.</value>
        public string AnnotationFont { get; set; }

        /// <summary>
        /// Gets or sets the size of the annotation font.
        /// </summary>
        /// <value>The size of the annotation font.</value>
        public double AnnotationFontSize { get; set; }

        /// <summary>
        /// Gets or sets the annotations.
        /// </summary>
        /// <value>The annotations.</value>
        public Collection<IAnnotation> Annotations { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to auto adjust plot margins.
        /// </summary>
        public bool AutoAdjustPlotMargins { get; set; }

        /// <summary>
        /// Gets or sets the axes.
        /// </summary>
        /// <value>The axes.</value>
        public Collection<IAxis> Axes { get; set; }

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
        /// Gets or sets the default colors.
        /// </summary>
        /// <value>The default colors.</value>
        public List<OxyColor> DefaultColors { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the legend is visible.
        /// The titles of the series must be set to use the legend.
        /// </summary>
        public bool IsLegendVisible { get; set; }

        /// <summary>
        /// Gets the legend area.
        /// </summary>
        /// <value>The legend area.</value>
        public OxyRect LegendArea { get; private set; }

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
        /// Gets or sets the legend column spacing.
        /// </summary>
        /// <value>The legend column spacing.</value>
        public double LegendColumnSpacing { get; set; }

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
        /// Gets or sets the legend item alignment.
        /// </summary>
        /// <value>The legend item alignment.</value>
        public HorizontalTextAlign LegendItemAlignment { get; set; }

        /// <summary>
        /// Gets or sets the legend item order.
        /// </summary>
        /// <value>The legend item order.</value>
        public LegendItemOrder LegendItemOrder { get; set; }

        /// <summary>
        /// Gets or sets the legend spacing.
        /// </summary>
        /// <value>The legend spacing.</value>
        public double LegendItemSpacing { get; set; }

        /// <summary>
        /// Gets or sets the legend margin.
        /// </summary>
        /// <value>The legend margin.</value>
        public double LegendMargin { get; set; }

        /// <summary>
        /// Gets or sets the legend orientation.
        /// </summary>
        /// <value>The legend orientation.</value>
        public LegendOrientation LegendOrientation { get; set; }

        /// <summary>
        /// Gets or sets the legend padding.
        /// </summary>
        /// <value>The legend padding.</value>
        public double LegendPadding { get; set; }

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
        /// Gets or sets the length of the legend symbols (the default value is 16).
        /// </summary>
        public double LegendSymbolLength { get; set; }

        /// <summary>
        /// Gets or sets the legend symbol margins (distance between the symbol and the text).
        /// </summary>
        /// <value>The legend symbol margin.</value>
        public double LegendSymbolMargin { get; set; }

        /// <summary>
        /// Gets or sets the legend symbol placement.
        /// </summary>
        /// <value>The legend symbol placement.</value>
        public LegendSymbolPlacement LegendSymbolPlacement { get; set; }

        /// <summary>
        /// Gets or sets the legend title.
        /// </summary>
        /// <value>The legend title.</value>
        public string LegendTitle { get; set; }

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
        /// Gets or sets the padding around the plot.
        /// </summary>
        /// <value>The padding.</value>
        public OxyThickness Padding { get; set; }

        /// <summary>
        /// Gets the area including both the plot and the axes.
        /// Outside legends are rendered outside this rectangle.       
        /// </summary>
        /// <value>The plot and axis area.</value>
        public OxyRect PlotAndAxisArea { get; private set; }

        /// <summary>
        /// Gets the plot area.
        /// This area is used to draw the series (not including axes or legends).
        /// </summary>
        /// <value>The plot area.</value>
        public OxyRect PlotArea { get; private set; }

        /// <summary>
        /// Gets or sets the minimum margins around the plot (this should be large enough to fit the axes).
        /// The default value is (60, 4, 4, 40).
        /// Set AutoAdjustPlotMargins if you want the margins to be adjusted when the axes require more space.
        /// </summary>
        public OxyThickness PlotMargins { get; set; }

        /// <summary>
        /// Gets or sets the type of the coordinate system.
        /// </summary>
        /// <value>The type of the plot.</value>
        public PlotType PlotType { get; set; }

        /// <summary>
        /// Gets or sets the series.
        /// </summary>
        /// <value>The series.</value>
        public Collection<ISeries> Series { get; set; }

        /// <summary>
        /// Gets or sets the subtitle.
        /// </summary>
        /// <value>The subtitle.</value>
        public string Subtitle { get; set; }

        /// <summary>
        /// Gets or sets the subtitle font.
        /// If this property is null, the Title font will be used.
        /// </summary>
        /// <value>The subtitle font.</value>
        public string SubtitleFont { get; set; }

        /// <summary>
        /// Gets or sets the size of the subtitle font.
        /// </summary>
        /// <value>The size of the subtitle font.</value>
        public double SubtitleFontSize { get; set; }

        /// <summary>
        /// Gets or sets the subtitle font weight.
        /// </summary>
        /// <value>The subtitle font weight.</value>
        public double SubtitleFontWeight { get; set; }

        /// <summary>
        /// Gets or sets the color of the text.
        /// </summary>
        /// <value>The color of the text.</value>
        public OxyColor TextColor { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets the title area.
        /// </summary>
        /// <value>The title area.</value>
        public OxyRect TitleArea { get; private set; }

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
        /// Gets or sets the title font weight.
        /// </summary>
        /// <value>The title font weight.</value>
        public double TitleFontWeight { get; set; }

        /// <summary>
        /// Gets or sets the padding around the title.
        /// </summary>
        /// <value>The title padding.</value>
        public double TitlePadding { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the default angle axis.
        /// </summary>
        /// <value>The default angle axis.</value>
        internal IAxis DefaultAngleAxis { get; set; }

        /// <summary>
        /// Gets or sets the default magnitude axis.
        /// </summary>
        /// <value>The default magnitude axis.</value>
        internal IAxis DefaultMagnitudeAxis { get; set; }

        /// <summary>
        /// Gets or sets the default X axis.
        /// </summary>
        /// <value>The default X axis.</value>
        internal IAxis DefaultXAxis { get; set; }

        /// <summary>
        /// Gets or sets the default Y axis.
        /// </summary>
        /// <value>The default Y axis.</value>
        internal IAxis DefaultYAxis { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a report for the plot.
        /// </summary>
        /// <returns>
        /// A report.
        /// </returns>
        public Report CreateReport()
        {
            var r = new Report();

            r.AddHeader(1, "P L O T   R E P O R T");
            r.AddHeader(2, "=== PlotModel ===");
            r.AddPropertyTable("PlotModel", this);

            r.AddHeader(2, "=== Axes ===");
            foreach (IAxis a in this.Axes)
            {
                r.AddPropertyTable(a.GetType().Name, a);
            }

            r.AddHeader(2, "=== Annotations ===");
            foreach (IAnnotation a in this.Annotations)
            {
                r.AddPropertyTable(a.GetType().Name, a);
            }

            r.AddHeader(2, "=== Series ===");
            foreach (ISeries s in this.Series)
            {
                r.AddPropertyTable(s.GetType().Name, s);
                var ds = s as DataPointSeries;
                if (ds != null)
                {
                    var fields = new List<ItemsTableField> { new ItemsTableField("X", "X"), new ItemsTableField("Y", "Y") };
                    r.AddItemsTable("Data", ds.Points, fields);
                }
            }

            r.AddParagraph(
                string.Format(
                    "Report generated by OxyPlot {0}, {1:u}", 
                    Assembly.GetExecutingAssembly().GetName().Version.ToString(3), 
                    DateTime.Now));
            return r;
        }

        /// <summary>
        /// Creates a text report for the plot.
        /// </summary>
        /// <returns>
        /// The create text report.
        /// </returns>
        public string CreateTextReport()
        {
            using (var ms = new MemoryStream())
            {
                using (var trw = new TextReportWriter(ms))
                {
                    Report report = this.CreateReport();
                    report.Write(trw);
                    trw.Flush();
                    ms.Position = 0;
                    var r = new StreamReader(ms);
                    return r.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Gets the first axes that covers the area of the specified point.
        /// </summary>
        /// <param name="pt">
        /// The point.
        /// </param>
        /// <param name="xaxis">
        /// The xaxis.
        /// </param>
        /// <param name="yaxis">
        /// The yaxis.
        /// </param>
        public void GetAxesFromPoint(ScreenPoint pt, out IAxis xaxis, out IAxis yaxis)
        {
            xaxis = yaxis = null;

            // Get the axis position of the given point. Using null if the point is inside the plot area.
            AxisPosition? position = null;
            if (pt.X < this.PlotArea.Left)
            {
                position = AxisPosition.Left;
            }

            if (pt.X > this.PlotArea.Right)
            {
                position = AxisPosition.Right;
            }

            if (pt.Y < this.PlotArea.Top)
            {
                position = AxisPosition.Top;
            }

            if (pt.Y > this.PlotArea.Bottom)
            {
                position = AxisPosition.Bottom;
            }

            foreach (IAxis axis in this.Axes)
            {
                double x = axis.InverseTransform(axis.IsHorizontal() ? pt.X : pt.Y);
                if (x >= axis.ActualMinimum && x <= axis.ActualMaximum
                    && (position == null || position == axis.Position))
                {
                    if (axis.IsHorizontal())
                    {
                        if (xaxis == null)
                        {
                            xaxis = axis;
                        }
                    }
                    else
                    {
                        if (yaxis == null)
                        {
                            yaxis = axis;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the default color from the DefaultColors palette.
        /// </summary>
        /// <returns>
        /// The next default color.
        /// </returns>
        public OxyColor GetDefaultColor()
        {
            return this.DefaultColors[this.currentColorIndex++ % this.DefaultColors.Count];
        }

        /// <summary>
        /// Gets the default line style.
        /// </summary>
        /// <returns>
        /// The next default line style.
        /// </returns>
        public LineStyle GetDefaultLineStyle()
        {
            return (LineStyle)((this.currentColorIndex / this.DefaultColors.Count) % (int)LineStyle.None);
        }

        /// <summary>
        /// Gets a series from the specified point.
        /// </summary>
        /// <param name="point">
        /// The point.
        /// </param>
        /// <param name="limit">
        /// The limit.
        /// </param>
        /// <returns>
        /// The nearest series.
        /// </returns>
        public ISeries GetSeriesFromPoint(ScreenPoint point, double limit)
        {
            double mindist = double.MaxValue;
            ISeries closest = null;
            foreach (ISeries s in this.Series)
            {
                var ts = s as ITrackableSeries;
                if (ts == null)
                {
                    continue;
                }

                TrackerHitResult thr = ts.GetNearestPoint(point, true) ?? ts.GetNearestPoint(point, false);

                if (thr == null)
                {
                    continue;
                }

                // find distance to this point on the screen
                double dist = point.DistanceTo(thr.Position);
                if (dist < mindist)
                {
                    closest = s;
                    mindist = dist;
                }
            }

            if (mindist < limit)
            {
                return closest;
            }

            return null;
        }

        /// <summary>
        /// Saves the plot to a SVG file.
        /// </summary>
        /// <param name="fileName">
        /// Name of the file.
        /// </param>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        public void SaveSvg(string fileName, double width, double height)
        {
            using (var svgrc = new SvgRenderContext(fileName, width, height))
            {
                this.Render(svgrc);
            }
        }

        /// <summary>
        /// Generates C# code of the model.
        /// </summary>
        /// <returns>
        /// C# code.
        /// </returns>
        public string ToCode()
        {
            var cg = new CodeGenerator(this);
            return cg.ToCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Title;
        }

        /// <summary>
        /// Create an SVG model and return it as a string.
        /// </summary>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        /// <param name="isDocument">
        /// if set to <c>true</c> [is document].
        /// </param>
        /// <returns>
        /// The to svg.
        /// </returns>
        public string ToSvg(double width, double height, bool isDocument = false)
        {
            string svg;
            using (var ms = new MemoryStream())
            {
                var svgrc = new SvgRenderContext(ms, width, height, isDocument);
                this.Render(svgrc);
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
        /// Updates the axis transforms and intervals.
        /// This is used after pan/zoom.
        /// </summary>
        public void UpdateAxisTransforms()
        {
            // Update the transforms
            foreach (IAxis a in this.Axes)
            {
                a.UpdateTransform(this.PlotArea);
            }

            // Set the same scaling to all axes if CartesianAxes is selected
            if (this.PlotType == PlotType.Cartesian)
            {
                double minimumScale = double.MaxValue;
                foreach (IAxis a in this.Axes)
                {
                    minimumScale = Math.Min(minimumScale, Math.Abs(a.Scale));
                }

                foreach (IAxis a in this.Axes)
                {
                    a.SetScale(minimumScale);
                }
            }

            // Update the intervals for all axes
            foreach (IAxis a in this.Axes)
            {
                a.UpdateIntervals(this.PlotArea);
            }
        }

        /// <summary>
        /// Force an update of the data.
        /// 1. Updates the data of each Series.
        /// 2. Ensure that all series have axes assigned.
        /// 3. Updates the max and min of the axes.
        /// </summary>
        public void UpdateData()
        {
            foreach (ISeries s in this.Series)
            {
                s.UpdateData();
            }

            // Ensure that there are default axes available
            this.EnsureDefaultAxes();

            foreach (IAxis a in this.Axes)
            {
                a.UpdateData(this.Series);
            }

            // Update the max and min of the axes
            this.UpdateMaxMin();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Find the default x/y axes (first in collection)
        /// </summary>
        private void EnsureDefaultAxes()
        {
            this.DefaultXAxis = null;
            this.DefaultYAxis = null;

            foreach (IAxis a in this.Axes)
            {
                if (a.IsHorizontal())
                {
                    if (this.DefaultXAxis == null)
                    {
                        this.DefaultXAxis = a;
                    }
                }

                if (a.IsVertical())
                {
                    if (this.DefaultYAxis == null)
                    {
                        this.DefaultYAxis = a;
                    }
                }

                if (a.Position == AxisPosition.Magnitude)
                {
                    if (this.DefaultMagnitudeAxis == null)
                    {
                        this.DefaultMagnitudeAxis = a;
                    }
                }

                if (a.Position == AxisPosition.Angle)
                {
                    if (this.DefaultAngleAxis == null)
                    {
                        this.DefaultAngleAxis = a;
                    }
                }
            }

            if (this.DefaultXAxis == null)
            {
                this.DefaultXAxis = this.DefaultMagnitudeAxis;
            }

            if (this.DefaultYAxis == null)
            {
                this.DefaultYAxis = this.DefaultAngleAxis;
            }

            if (this.PlotType == PlotType.Polar)
            {
                if (this.DefaultXAxis == null)
                {
                    this.DefaultXAxis = this.DefaultMagnitudeAxis = new LinearAxis { Position = AxisPosition.Magnitude };
                }

                if (this.DefaultYAxis == null)
                {
                    this.DefaultYAxis = this.DefaultAngleAxis = new LinearAxis { Position = AxisPosition.Angle };
                }
            }
            else
            {
                if (this.DefaultXAxis == null)
                {
                    this.DefaultXAxis = new LinearAxis { Position = AxisPosition.Bottom };
                }

                if (this.DefaultYAxis == null)
                {
                    this.DefaultYAxis = new LinearAxis { Position = AxisPosition.Left };
                }
            }

            bool areAxesRequired = false;
            foreach (ISeries s in this.Series)
            {
                if (s.AreAxesRequired())
                {
                    areAxesRequired = true;
                }
            }

            if (areAxesRequired)
            {
                if (!this.Axes.Contains(this.DefaultXAxis))
                {
                    this.Axes.Add(this.DefaultXAxis);
                }

                if (!this.Axes.Contains(this.DefaultYAxis))
                {
                    this.Axes.Add(this.DefaultYAxis);
                }
            }

            // Update the x/y axes of series without axes defined
            foreach (ISeries s in this.Series)
            {
                if (s.AreAxesRequired())
                {
                    s.EnsureAxes(this.Axes, this.DefaultXAxis, this.DefaultYAxis);
                }
            }

            // Update the x/y axes of series without axes defined
            foreach (IAnnotation s in this.Annotations)
            {
                s.EnsureAxes(this.Axes, this.DefaultXAxis, this.DefaultYAxis);
            }
        }

        /// <summary>
        /// Resets the default color.
        /// </summary>
        private void ResetDefaultColor()
        {
            this.currentColorIndex = 0;
        }

        /// <summary>
        /// Update max and min values of the axes from values of all data series.
        /// Only axes with automatic set to true are changed.
        /// </summary>
        private void UpdateMaxMin()
        {
            foreach (IAxis a in this.Axes)
            {
                a.ResetActualMaxMin();
            }

            foreach (ISeries s in this.Series)
            {
                s.UpdateMaxMin();
            }

            foreach (IAxis a in this.Axes)
            {
                a.UpdateActualMaxMin();
            }
        }

        #endregion

        ///// <summary>
        ///// Xml serialize the plotmodel to a stream.
        ///// </summary>
        ///// <param name="s">The stream.</param>
        //public void XmlSerialize(Stream s)
        //{
        //    var serializer = new XmlSerializer(typeof(PlotModel));
        //    serializer.Serialize(s, this);
        //}

        ///// <summary>
        ///// Xml serialize the plotmodel to a file.
        ///// </summary>
        ///// <param name="filename">The filename.</param>
        //public void XmlSerialize(string filename)
        //{
        //    using (var s = File.OpenWrite(filename))
        //    {
        //        this.XmlSerialize(s);
        //    }
        //}
    }
}