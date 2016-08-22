// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Specifies the coordinate system type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using OxyPlot.Annotations;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    /// <summary>
    /// Specifies the coordinate system type.
    /// </summary>
    public enum PlotType
    {
        /// <summary>
        /// XY coordinate system - two perpendicular axes
        /// </summary>
        XY,

        /// <summary>
        /// Cartesian coordinate system - perpendicular axes with the same scaling.
        /// </summary>
        /// <remarks>See http://en.wikipedia.org/wiki/Cartesian_coordinate_system</remarks>
        Cartesian,

        /// <summary>
        /// Polar coordinate system - with radial and angular axes
        /// </summary>
        /// <remarks>See http://en.wikipedia.org/wiki/Polar_coordinate_system</remarks>
        Polar
    }

    /// <summary>
    /// Specifies the placement of the legend box.
    /// </summary>
    public enum LegendPlacement
    {
        /// <summary>
        /// Place the legends inside the plot area.
        /// </summary>
        Inside,

        /// <summary>
        /// Place the legends outside the plot area.
        /// </summary>
        Outside
    }

    /// <summary>
    /// Specifies the position of the legend box.
    /// </summary>
    public enum LegendPosition
    {
        /// <summary>
        /// Place the legend box in the top-left corner.
        /// </summary>
        TopLeft,

        /// <summary>
        /// Place the legend box centered at the top.
        /// </summary>
        TopCenter,

        /// <summary>
        /// Place the legend box in the top-right corner.
        /// </summary>
        TopRight,

        /// <summary>
        /// Place the legend box in the bottom-left corner.
        /// </summary>
        BottomLeft,

        /// <summary>
        /// Place the legend box centered at the bottom.
        /// </summary>
        BottomCenter,

        /// <summary>
        /// Place the legend box in the bottom-right corner.
        /// </summary>
        BottomRight,

        /// <summary>
        /// Place the legend box in the left-top corner.
        /// </summary>
        LeftTop,

        /// <summary>
        /// Place the legend box centered at the left.
        /// </summary>
        LeftMiddle,

        /// <summary>
        /// Place the legend box in the left-bottom corner.
        /// </summary>
        LeftBottom,

        /// <summary>
        /// Place the legend box in the right-top corner.
        /// </summary>
        RightTop,

        /// <summary>
        /// Place the legend box centered at the right.
        /// </summary>
        RightMiddle,

        /// <summary>
        /// Place the legend box in the right-bottom corner.
        /// </summary>
        RightBottom
    }

    /// <summary>
    /// Specifies the orientation of the items in the legend box.
    /// </summary>
    public enum LegendOrientation
    {
        /// <summary>
        /// Orient the items horizontally.
        /// </summary>
        Horizontal,

        /// <summary>
        /// Orient the items vertically.
        /// </summary>
        Vertical
    }

    /// <summary>
    /// Specifies the item order of the legends.
    /// </summary>
    public enum LegendItemOrder
    {
        /// <summary>
        /// Render the items in the normal order.
        /// </summary>
        Normal,

        /// <summary>
        /// Render the items in the reverse order.
        /// </summary>
        Reverse
    }

    /// <summary>
    /// Specifies the placement of the legend symbols.
    /// </summary>
    public enum LegendSymbolPlacement
    {
        /// <summary>
        /// Render symbols to the left of the labels.
        /// </summary>
        Left,

        /// <summary>
        /// Render symbols to the right of the labels.
        /// </summary>
        Right
    }

    /// <summary>
    /// Specifies the horizontal alignment of the titles.
    /// </summary>
    public enum TitleHorizontalAlignment
    {
        /// <summary>
        /// Centered within the plot area.
        /// </summary>
        CenteredWithinPlotArea,

        /// <summary>
        /// Centered within the client view (excluding padding defined in <see cref="PlotModel.Padding" />).
        /// </summary>
        CenteredWithinView
    }

    /// <summary>
    /// Represents a plot.
    /// </summary>
    public partial class PlotModel : Model, IPlotModel
    {
        /// <summary>
        /// The plot view that renders this plot.
        /// </summary>
        private WeakReference plotViewReference;

        /// <summary>
        /// The current color index.
        /// </summary>
        private int currentColorIndex;

        /// <summary>
        /// Flags if the data has been updated.
        /// </summary>
        private bool isDataUpdated;

        /// <summary>
        /// The last update exception.
        /// </summary>
        /// <value>The exception or <c>null</c> if there was no exceptions during the last update.</value>
        private Exception lastPlotException;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlotModel" /> class.
        /// </summary>
        public PlotModel()
        {
            this.Axes = new ElementCollection<Axis>(this);
            this.Series = new ElementCollection<Series.Series>(this);
            this.Annotations = new ElementCollection<Annotation>(this);

            this.PlotType = PlotType.XY;

            this.PlotMargins = new OxyThickness(double.NaN);
            this.Padding = new OxyThickness(8);

            this.Background = OxyColors.Undefined;
            this.PlotAreaBackground = OxyColors.Undefined;

            this.TextColor = OxyColors.Black;
            this.TitleColor = OxyColors.Automatic;
            this.SubtitleColor = OxyColors.Automatic;

            this.DefaultFont = "Segoe UI";
            this.DefaultFontSize = 12;

            this.TitleToolTip = null;
            this.TitleFont = null;
            this.TitleFontSize = 18;
            this.TitleFontWeight = FontWeights.Bold;
            this.SubtitleFont = null;
            this.SubtitleFontSize = 14;
            this.SubtitleFontWeight = FontWeights.Normal;
            this.TitlePadding = 6;

            this.PlotAreaBorderColor = OxyColors.Black;
            this.PlotAreaBorderThickness = new OxyThickness(1);

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
            this.LegendColumnSpacing = 8;
            this.LegendItemSpacing = 24;
            this.LegendLineSpacing = 0;
            this.LegendMargin = 8;

            this.LegendBackground = OxyColors.Undefined;
            this.LegendBorder = OxyColors.Undefined;
            this.LegendBorderThickness = 1;

            this.LegendTextColor = OxyColors.Automatic;
            this.LegendTitleColor = OxyColors.Automatic;

            this.LegendMaxWidth = double.NaN;
            this.LegendMaxHeight = double.NaN;
            this.LegendPlacement = LegendPlacement.Inside;
            this.LegendPosition = LegendPosition.RightTop;
            this.LegendOrientation = LegendOrientation.Vertical;
            this.LegendItemOrder = LegendItemOrder.Normal;
            this.LegendItemAlignment = HorizontalAlignment.Left;
            this.LegendSymbolPlacement = LegendSymbolPlacement.Left;

            this.DefaultColors = new List<OxyColor>
            {
                    OxyColor.FromRgb(0x4E, 0x9A, 0x06),
                    OxyColor.FromRgb(0xC8, 0x8D, 0x00),
                    OxyColor.FromRgb(0xCC, 0x00, 0x00),
                    OxyColor.FromRgb(0x20, 0x4A, 0x87),
                    OxyColors.Red,
                    OxyColors.Orange,
                    OxyColors.Yellow,
                    OxyColors.Green,
                    OxyColors.Blue,
                    OxyColors.Indigo,
                    OxyColors.Violet
                };

            this.AxisTierDistance = 4.0;
        }

        /// <summary>
        /// Occurs when the tracker has been changed.
        /// </summary>
        public event EventHandler<TrackerEventArgs> TrackerChanged;

        /// <summary>
        /// Occurs when the plot has been updated.
        /// </summary>
        public event EventHandler Updated;

        /// <summary>
        /// Occurs when the plot is about to be updated.
        /// </summary>
        public event EventHandler Updating;

        /// <summary>
        /// Gets or sets the default font.
        /// </summary>
        /// <value>The default font.</value>
        /// <remarks>This font is used for text on axes, series, legends and plot titles unless other fonts are specified.</remarks>
        public string DefaultFont { get; set; }

        /// <summary>
        /// Gets or sets the default size of the fonts.
        /// </summary>
        /// <value>The default size of the font.</value>
        public double DefaultFontSize { get; set; }

        /// <summary>
        /// Gets the actual culture.
        /// </summary>
        public CultureInfo ActualCulture
        {
            get
            {
                return this.Culture ?? CultureInfo.CurrentCulture;
            }
        }

        /// <summary>
        /// Gets the actual plot margins.
        /// </summary>
        /// <value>The actual plot margins.</value>
        public OxyThickness ActualPlotMargins { get; private set; }

        /// <summary>
        /// Gets the plot view that renders this plot.
        /// </summary>
        /// <value>The plot view.</value>
        /// <remarks>Only one view can render the plot at the same time.</remarks>
        public IPlotView PlotView
        {
            get
            {
                return (this.plotViewReference != null) ? (IPlotView)this.plotViewReference.Target : null;
            }
        }

        /// <summary>
        /// Gets the annotations.
        /// </summary>
        /// <value>The annotations.</value>
        public ElementCollection<Annotation> Annotations { get; private set; }

        /// <summary>
        /// Gets the axes.
        /// </summary>
        /// <value>The axes.</value>
        public ElementCollection<Axis> Axes { get; private set; }

        /// <summary>
        /// Gets or sets the color of the background of the plot.
        /// </summary>
        /// <value>The color. The default is <see cref="OxyColors.Undefined" />.</value>
        /// <remarks>If the background color is set to <see cref="OxyColors.Undefined" />, the default color of the plot view will be used.</remarks>
        public OxyColor Background { get; set; }

        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        /// <value>The culture.</value>
        public CultureInfo Culture { get; set; }

        /// <summary>
        /// Gets or sets the default colors.
        /// </summary>
        /// <value>The default colors.</value>
        public IList<OxyColor> DefaultColors { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the legend is visible. The titles of the series must be set to use the legend.
        /// </summary>
        public bool IsLegendVisible { get; set; }

        /// <summary>
        /// Gets the legend area.
        /// </summary>
        /// <value>The legend area.</value>
        public OxyRect LegendArea { get; private set; }

        /// <summary>
        /// Gets or sets the background color of the legend. Use <c>null</c> for no background.
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
        /// Gets or sets the spacing between columns of legend items (only for vertical orientation).
        /// </summary>
        /// <value>The spacing in device independent units.</value>
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
        /// Gets or sets the color of the legend text.
        /// </summary>
        /// <value>The color of the legend text.</value>
        /// <remarks>If this value is <c>null</c>, the TextColor will be used.</remarks>
        public OxyColor LegendTextColor { get; set; }

        /// <summary>
        /// Gets or sets the legend font weight.
        /// </summary>
        /// <value>The legend font weight.</value>
        public double LegendFontWeight { get; set; }

        /// <summary>
        /// Gets or sets the legend item alignment.
        /// </summary>
        /// <value>The legend item alignment.</value>
        public HorizontalAlignment LegendItemAlignment { get; set; }

        /// <summary>
        /// Gets or sets the legend item order.
        /// </summary>
        /// <value>The legend item order.</value>
        public LegendItemOrder LegendItemOrder { get; set; }

        /// <summary>
        /// Gets or sets the horizontal spacing between legend items when the orientation is horizontal.
        /// </summary>
        /// <value>The horizontal distance between items in device independent units.</value>
        public double LegendItemSpacing { get; set; }

        /// <summary>
        /// Gets or sets the vertical spacing between legend items.
        /// </summary>
        /// <value>The spacing in device independent units.</value>
        public double LegendLineSpacing { get; set; }

        /// <summary>
        /// Gets or sets the legend margin.
        /// </summary>
        /// <value>The legend margin.</value>
        public double LegendMargin { get; set; }

        /// <summary>
        /// Gets or sets the max width of the legend.
        /// </summary>
        /// <value>The max width of the legend.</value>
        public double LegendMaxWidth { get; set; }

        /// <summary>
        /// Gets or sets the max height of the legend.
        /// </summary>
        /// <value>The max height of the legend.</value>
        public double LegendMaxHeight { get; set; }

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
        /// Gets or sets the color of the legend title.
        /// </summary>
        /// <value>The color of the legend title.</value>
        /// <remarks>If this value is <c>null</c>, the TextColor will be used.</remarks>
        public OxyColor LegendTitleColor { get; set; }

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
        /// Gets the total width of the plot (in device units).
        /// </summary>
        public double Width { get; private set; }

        /// <summary>
        /// Gets the total height of the plot (in device units).
        /// </summary>
        public double Height { get; private set; }

        /// <summary>
        /// Gets the area including both the plot and the axes. Outside legends are rendered outside this rectangle.
        /// </summary>
        /// <value>The plot and axis area.</value>
        public OxyRect PlotAndAxisArea { get; private set; }

        /// <summary>
        /// Gets the plot area. This area is used to draw the series (not including axes or legends).
        /// </summary>
        /// <value>The plot area.</value>
        public OxyRect PlotArea { get; private set; }

        /// <summary>
        /// Gets or sets the distance between two neighborhood tiers of the same AxisPosition.
        /// </summary>
        public double AxisTierDistance { get; set; }

        /// <summary>
        /// Gets or sets the color of the background of the plot area.
        /// </summary>
        public OxyColor PlotAreaBackground { get; set; }

        /// <summary>
        /// Gets or sets the color of the border around the plot area.
        /// </summary>
        /// <value>The color of the box.</value>
        public OxyColor PlotAreaBorderColor { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the border around the plot area.
        /// </summary>
        /// <value>The box thickness.</value>
        public OxyThickness PlotAreaBorderThickness { get; set; }

        /// <summary>
        /// Gets or sets the margins around the plot (this should be large enough to fit the axes).
        /// If any of the values is set to <c>double.NaN</c>, the margin is adjusted to the value required by the axes.
        /// </summary>
        public OxyThickness PlotMargins { get; set; }

        /// <summary>
        /// Gets or sets the type of the coordinate system.
        /// </summary>
        /// <value>The type of the plot.</value>
        public PlotType PlotType { get; set; }

        /// <summary>
        /// Gets the series.
        /// </summary>
        /// <value>The series.</value>
        public ElementCollection<Series.Series> Series { get; private set; }

        /// <summary>
        /// Gets or sets the rendering decorator.
        /// </summary>
        /// <value>
        /// The rendering decorator.
        /// </value>
        public Func<IRenderContext, IRenderContext> RenderingDecorator { get; set; }

        /// <summary>
        /// Gets or sets the subtitle.
        /// </summary>
        /// <value>The subtitle.</value>
        public string Subtitle { get; set; }

        /// <summary>
        /// Gets or sets the subtitle font. If this property is <c>null</c>, the Title font will be used.
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
        /// Gets or sets the default color of the text in the plot (titles, legends, annotations, axes).
        /// </summary>
        /// <value>The color of the text.</value>
        public OxyColor TextColor { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the title tool tip.
        /// </summary>
        /// <value>The title tool tip.</value>
        public string TitleToolTip { get; set; }

        /// <summary>
        /// Gets or sets the color of the title.
        /// </summary>
        /// <value>The color of the title.</value>
        /// <remarks>If the value is <c>null</c>, the TextColor will be used.</remarks>
        public OxyColor TitleColor { get; set; }

        /// <summary>
        /// Gets or sets the color of the subtitle.
        /// </summary>
        /// <value>The color of the subtitle.</value>
        public OxyColor SubtitleColor { get; set; }

        /// <summary>
        /// Gets or sets the horizontal alignment of the title and subtitle.
        /// </summary>
        /// <value>
        /// The alignment.
        /// </value>
        public TitleHorizontalAlignment TitleHorizontalAlignment { get; set; }

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

        /// <summary>
        /// Gets the default angle axis.
        /// </summary>
        /// <value>The default angle axis.</value>
        public AngleAxis DefaultAngleAxis { get; private set; }

        /// <summary>
        /// Gets the default magnitude axis.
        /// </summary>
        /// <value>The default magnitude axis.</value>
        public MagnitudeAxis DefaultMagnitudeAxis { get; private set; }

        /// <summary>
        /// Gets the default X axis.
        /// </summary>
        /// <value>The default X axis.</value>
        public Axis DefaultXAxis { get; private set; }

        /// <summary>
        /// Gets the default Y axis.
        /// </summary>
        /// <value>The default Y axis.</value>
        public Axis DefaultYAxis { get; private set; }

        /// <summary>
        /// Gets the default color axis.
        /// </summary>
        /// <value>The default color axis.</value>
        public IColorAxis DefaultColorAxis { get; private set; }

        /// <summary>
        /// Gets the actual title font.
        /// </summary>
        protected string ActualTitleFont
        {
            get
            {
                return this.TitleFont ?? this.DefaultFont;
            }
        }

        /// <summary>
        /// Gets the actual subtitle font.
        /// </summary>
        protected string ActualSubtitleFont
        {
            get
            {
                return this.SubtitleFont ?? this.DefaultFont;
            }
        }

        /// <summary>
        /// Attaches this model to the specified plot view.
        /// </summary>
        /// <param name="plotView">The plot view.</param>
        /// <remarks>Only one plot view can be attached to the plot model.
        /// The plot model contains data (e.g. axis scaling) that is only relevant to the current plot view.</remarks>
        void IPlotModel.AttachPlotView(IPlotView plotView)
        {
            var currentPlotView = this.PlotView;
            if (!object.ReferenceEquals(currentPlotView, null) &&
                !object.ReferenceEquals(plotView, null) &&
                !object.ReferenceEquals(currentPlotView, plotView))
            {
                throw new InvalidOperationException(
                    "This PlotModel is already in use by some other PlotView control.");
            }

            this.plotViewReference = (plotView == null) ? null : new WeakReference(plotView);
        }

        /// <summary>
        /// Invalidates the plot.
        /// </summary>
        /// <param name="updateData">Updates all data sources if set to <c>true</c>.</param>
        public void InvalidatePlot(bool updateData)
        {
            var plotView = this.PlotView;
            if (plotView == null)
            {
                return;
            }

            plotView.InvalidatePlot(updateData);
        }

        /// <summary>
        /// Gets the first axes that covers the area of the specified point.
        /// </summary>
        /// <param name="pt">The point.</param>
        /// <param name="xaxis">The x-axis.</param>
        /// <param name="yaxis">The y-axis.</param>
        public void GetAxesFromPoint(ScreenPoint pt, out Axis xaxis, out Axis yaxis)
        {
            xaxis = yaxis = null;

            // Get the axis position of the given point. Using null if the point is inside the plot area.
            AxisPosition? position = null;
            double plotAreaValue = 0;
            if (pt.X < this.PlotArea.Left)
            {
                position = AxisPosition.Left;
                plotAreaValue = this.PlotArea.Left;
            }

            if (pt.X > this.PlotArea.Right)
            {
                position = AxisPosition.Right;
                plotAreaValue = this.PlotArea.Right;
            }

            if (pt.Y < this.PlotArea.Top)
            {
                position = AxisPosition.Top;
                plotAreaValue = this.PlotArea.Top;
            }

            if (pt.Y > this.PlotArea.Bottom)
            {
                position = AxisPosition.Bottom;
                plotAreaValue = this.PlotArea.Bottom;
            }

            foreach (var axis in this.Axes)
            {
                if (axis is IColorAxis)
                {
                    continue;
                }

                if (axis is MagnitudeAxis)
                {
                    xaxis = axis;
                    continue;
                }

                if (axis is AngleAxis)
                {
                    yaxis = axis;
                    continue;
                }

                double x = double.NaN;
                if (axis.IsHorizontal())
                {
                    x = axis.InverseTransform(pt.X);
                }

                if (axis.IsVertical())
                {
                    x = axis.InverseTransform(pt.Y);
                }

                if (x >= axis.ActualMinimum && x <= axis.ActualMaximum)
                {
                    if (position == null)
                    {
                        if (axis.IsHorizontal())
                        {
                            if (xaxis == null)
                            {
                                xaxis = axis;
                            }
                        }
                        else if (axis.IsVertical())
                        {
                            if (yaxis == null)
                            {
                                yaxis = axis;
                            }
                        }
                    }
                    else if (position == axis.Position)
                    {
                        // Choose right tier
                        double positionTierMinShift = axis.PositionTierMinShift;
                        double positionTierMaxShift = axis.PositionTierMaxShift;

                        double posValue = axis.IsHorizontal() ? pt.Y : pt.X;
                        bool isLeftOrTop = position == AxisPosition.Top || position == AxisPosition.Left;
                        if ((posValue >= plotAreaValue + positionTierMinShift
                             && posValue < plotAreaValue + positionTierMaxShift && !isLeftOrTop)
                            ||
                            (posValue <= plotAreaValue - positionTierMinShift
                             && posValue > plotAreaValue - positionTierMaxShift && isLeftOrTop))
                        {
                            if (axis.IsHorizontal())
                            {
                                if (xaxis == null)
                                {
                                    xaxis = axis;
                                }
                            }
                            else if (axis.IsVertical())
                            {
                                if (yaxis == null)
                                {
                                    yaxis = axis;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the default color from the DefaultColors palette.
        /// </summary>
        /// <returns>The next default color.</returns>
        public OxyColor GetDefaultColor()
        {
            return this.DefaultColors[this.currentColorIndex++ % this.DefaultColors.Count];
        }

        /// <summary>
        /// Gets the default line style.
        /// </summary>
        /// <returns>The next default line style.</returns>
        public LineStyle GetDefaultLineStyle()
        {
            return (LineStyle)((this.currentColorIndex / this.DefaultColors.Count) % (int)LineStyle.None);
        }

        /// <summary>
        /// Gets a series from the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="limit">The limit.</param>
        /// <returns>The nearest series.</returns>
        public Series.Series GetSeriesFromPoint(ScreenPoint point, double limit = 100)
        {
            double mindist = double.MaxValue;
            Series.Series nearestSeries = null;
            foreach (var series in this.Series.Reverse().Where(s => s.IsVisible))
            {
                var thr = series.GetNearestPoint(point, true) ?? series.GetNearestPoint(point, false);

                if (thr == null)
                {
                    continue;
                }

                // find distance to this point on the screen
                double dist = point.DistanceTo(thr.Position);
                if (dist < mindist)
                {
                    nearestSeries = series;
                    mindist = dist;
                }
            }

            if (mindist < limit)
            {
                return nearestSeries;
            }

            return null;
        }

        /// <summary>
        /// Generates C# code of the model.
        /// </summary>
        /// <returns>C# code.</returns>
        public string ToCode()
        {
            var cg = new CodeGenerator(this);
            return cg.ToCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return this.Title;
        }

        /// <summary>
        /// Gets any exception thrown during the last <see cref="IPlotModel.Update" /> call.
        /// </summary>
        /// <returns>The exception or <c>null</c> if there was no exception.</returns>
        public Exception GetLastPlotException()
        {
            return this.lastPlotException;
        }

        /// <summary>
        /// Updates all axes and series.
        /// 0. Updates the owner PlotModel of all plot items (axes, series and annotations)
        /// 1. Updates the data of each Series (only if updateData==<c>true</c>).
        /// 2. Ensure that all series have axes assigned.
        /// 3. Updates the max and min of the axes.
        /// </summary>
        /// <param name="updateData">if set to <c>true</c> , all data collections will be updated.</param>
        void IPlotModel.Update(bool updateData)
        {
            lock (this.SyncRoot)
            {
                try
                {
                    this.lastPlotException = null;
                    this.OnUpdating();

                    // Updates the default axes
                    this.EnsureDefaultAxes();

                    var visibleSeries = this.Series.Where(s => s.IsVisible).ToArray();

                    // Update data of the series
                    if (updateData || !this.isDataUpdated)
                    {
                        foreach (var s in visibleSeries)
                        {
                            s.UpdateData();
                        }

                        this.isDataUpdated = true;
                    }

                    // Updates axes with information from the series
                    // This is used by the category axis that need to know the number of series using the axis.
                    foreach (var a in this.Axes)
                    {
                        a.UpdateFromSeries(visibleSeries);
                        a.ResetCurrentValues();
                    }

                    // Update valid data of the series
                    // This must be done after the axes are updated from series!
                    if (updateData)
                    {
                        foreach (var s in visibleSeries)
                        {
                            s.UpdateValidData();
                        }
                    }

                    // Update the max and min of the axes
                    this.UpdateMaxMin(updateData);

                    // Update undefined colors
                    this.ResetDefaultColor();
                    foreach (var s in visibleSeries)
                    {
                        s.SetDefaultValues();
                    }

                    this.OnUpdated();
                }
                catch (Exception e)
                {
                    this.lastPlotException = e;
                }
            }
        }

        /// <summary>
        /// Gets the axis for the specified key.
        /// </summary>
        /// <param name="key">The axis key.</param>
        /// <param name="defaultAxis">The default axis.</param>
        /// <returns>The axis, or the defaultAxis if the key is not specified.</returns>
        /// <exception cref="System.InvalidOperationException">Cannot find axis with the specified key.</exception>
        public Axis GetAxisOrDefault(string key, Axis defaultAxis)
        {
            if (key != null)
            {
                var axis = this.Axes.FirstOrDefault(a => a.Key == key);
                if (axis == null)
                {
                    throw new InvalidOperationException(string.Format("Cannot find axis with Key = \"{0}\"", key));
                }

                return axis;
            }

            return defaultAxis;
        }

        /// <summary>
        /// Resets all axes in the model.
        /// </summary>
        public void ResetAllAxes()
        {
            foreach (var a in this.Axes)
            {
                a.Reset();
            }
        }

        /// <summary>
        /// Pans all axes.
        /// </summary>
        /// <param name="dx">The horizontal distance to pan (screen coordinates).</param>
        /// <param name="dy">The vertical distance to pan (screen coordinates).</param>
        public void PanAllAxes(double dx, double dy)
        {
            foreach (var a in this.Axes)
            {
                a.Pan(a.IsHorizontal() ? dx : dy);
            }
        }

        /// <summary>
        /// Zooms all axes.
        /// </summary>
        /// <param name="factor">The zoom factor.</param>
        public void ZoomAllAxes(double factor)
        {
            foreach (var a in this.Axes)
            {
                a.ZoomAtCenter(factor);
            }
        }

        /// <summary>
        /// Raises the TrackerChanged event.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <remarks>
        /// This method is public so custom implementations of tracker manipulators can invoke this method.
        /// </remarks>
        public void RaiseTrackerChanged(TrackerHitResult result)
        {
            var handler = this.TrackerChanged;
            if (handler != null)
            {
                var args = new TrackerEventArgs { HitResult = result };
                handler(this, args);
            }
        }

        /// <summary>
        /// Raises the TrackerChanged event.
        /// </summary>
        /// <param name="result">The result.</param>
        protected internal virtual void OnTrackerChanged(TrackerHitResult result)
        {
            this.RaiseTrackerChanged(result);
        }

        /// <summary>
        /// Gets all elements of the model, top-level elements first.
        /// </summary>
        /// <returns>
        /// An enumerator of the elements.
        /// </returns>
        protected override IEnumerable<PlotElement> GetHitTestElements()
        {
            foreach (var axis in this.Axes.Reverse().Where(a => a.IsAxisVisible && a.Layer == AxisLayer.AboveSeries))
            {
                yield return axis;
            }

            foreach (var annotation in this.Annotations.Reverse().Where(a => a.Layer == AnnotationLayer.AboveSeries))
            {
                yield return annotation;
            }

            foreach (var s in this.Series.Reverse().Where(s => s.IsVisible))
            {
                yield return s;
            }

            foreach (var annotation in this.Annotations.Reverse().Where(a => a.Layer == AnnotationLayer.BelowSeries))
            {
                yield return annotation;
            }

            foreach (var axis in this.Axes.Reverse().Where(a => a.IsAxisVisible && a.Layer == AxisLayer.BelowSeries))
            {
                yield return axis;
            }

            foreach (var annotation in this.Annotations.Reverse().Where(a => a.Layer == AnnotationLayer.BelowAxes))
            {
                yield return annotation;
            }
        }

        /// <summary>
        /// Raises the Updated event.
        /// </summary>
        protected virtual void OnUpdated()
        {
            var handler = this.Updated;
            if (handler != null)
            {
                var args = new EventArgs();
                handler(this, args);
            }
        }

        /// <summary>
        /// Raises the Updating event.
        /// </summary>
        protected virtual void OnUpdating()
        {
            var handler = this.Updating;
            if (handler != null)
            {
                var args = new EventArgs();
                handler(this, args);
            }
        }

        /// <summary>
        /// Updates the axis transforms.
        /// </summary>
        private void UpdateAxisTransforms()
        {
            // Update the axis transforms
            foreach (var a in this.Axes)
            {
                a.UpdateTransform(this.PlotArea);
            }
        }

        /// <summary>
        /// Enforces the same scale on all axes.
        /// </summary>
        private void EnforceCartesianTransforms()
        {
            var notColorAxes = this.Axes.Where(a => !(a is IColorAxis)).ToArray();

            // Set the same scaling on all axes
            double sharedScale = notColorAxes.Min(a => Math.Abs(a.Scale));
            foreach (var a in notColorAxes)
            {
                a.Zoom(sharedScale);
            }

            sharedScale = notColorAxes.Max(a => Math.Abs(a.Scale));
            foreach (var a in notColorAxes)
            {
                a.Zoom(sharedScale);
            }

            foreach (var a in notColorAxes)
            {
                a.UpdateTransform(this.PlotArea);
            }
        }

        /// <summary>
        /// Updates the intervals (major and minor step values).
        /// </summary>
        private void UpdateIntervals()
        {
            // Update the intervals for all axes
            foreach (var a in this.Axes)
            {
                a.UpdateIntervals(this.PlotArea);
            }
        }

        /// <summary>
        /// Finds and sets the default horizontal and vertical axes (the first horizontal/vertical axes in the Axes collection).
        /// </summary>
        private void EnsureDefaultAxes()
        {
            this.DefaultXAxis = this.Axes.FirstOrDefault(a => a.IsHorizontal() && a.IsXyAxis());
            this.DefaultYAxis = this.Axes.FirstOrDefault(a => a.IsVertical() && a.IsXyAxis());
            this.DefaultMagnitudeAxis = this.Axes.FirstOrDefault(a => a is MagnitudeAxis) as MagnitudeAxis;
            this.DefaultAngleAxis = this.Axes.FirstOrDefault(a => a is AngleAxis) as AngleAxis;
            this.DefaultColorAxis = this.Axes.FirstOrDefault(a => a is IColorAxis) as IColorAxis;

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
                    this.DefaultXAxis = this.DefaultMagnitudeAxis = new MagnitudeAxis();
                }

                if (this.DefaultYAxis == null)
                {
                    this.DefaultYAxis = this.DefaultAngleAxis = new AngleAxis();
                }
            }
            else
            {
                bool createdlinearxaxis = false;
                bool createdlinearyaxis = false;
                if (this.DefaultXAxis == null)
                {
                    if (this.Series.Any(s => s.IsVisible && s is ColumnSeries))
                    {
                        this.DefaultXAxis = new CategoryAxis { Position = AxisPosition.Bottom };
                    }
                    else
                    {
                        this.DefaultXAxis = new LinearAxis { Position = AxisPosition.Bottom };
                        createdlinearxaxis = true;
                    }
                }

                if (this.DefaultYAxis == null)
                {
                    if (this.Series.Any(s => s.IsVisible && s is BarSeries))
                    {
                        this.DefaultYAxis = new CategoryAxis { Position = AxisPosition.Left };
                    }
                    else
                    {
                        this.DefaultYAxis = new LinearAxis { Position = AxisPosition.Left };
                        createdlinearyaxis = true;
                    }
                }

                if (createdlinearxaxis && this.DefaultYAxis is CategoryAxis)
                {
                    this.DefaultXAxis.MinimumPadding = 0;
                }

                if (createdlinearyaxis && this.DefaultXAxis is CategoryAxis)
                {
                    this.DefaultYAxis.MinimumPadding = 0;
                }
            }

            var areAxesRequired = this.Series.Any(s => s.IsVisible && s.AreAxesRequired());

            if (areAxesRequired)
            {
                if (!this.Axes.Contains(this.DefaultXAxis))
                {
                    System.Diagnostics.Debug.Assert(this.DefaultXAxis != null, "Default x-axis not created.");
                    if (this.DefaultXAxis != null)
                    {
                        this.Axes.Add(this.DefaultXAxis);
                    }
                }

                if (!this.Axes.Contains(this.DefaultYAxis))
                {
                    System.Diagnostics.Debug.Assert(this.DefaultYAxis != null, "Default y-axis not created.");
                    if (this.DefaultYAxis != null)
                    {
                        this.Axes.Add(this.DefaultYAxis);
                    }
                }
            }

            // Update the axes of series without axes defined
            foreach (var s in this.Series)
            {
                if (s.IsVisible && s.AreAxesRequired())
                {
                    s.EnsureAxes();
                }
            }

            // Update the axes of annotations without axes defined
            foreach (var a in this.Annotations)
            {
                a.EnsureAxes();
            }
        }

        /// <summary>
        /// Resets the default color index.
        /// </summary>
        private void ResetDefaultColor()
        {
            this.currentColorIndex = 0;
        }

        /// <summary>
        /// Updates maximum and minimum values of the axes from values of all data series.
        /// </summary>
        /// <param name="isDataUpdated">if set to <c>true</c> , the data has been updated.</param>
        private void UpdateMaxMin(bool isDataUpdated)
        {
            if (isDataUpdated)
            {
                foreach (var a in this.Axes)
                {
                    a.ResetDataMaxMin();
                }

                // data has been updated, so we need to calculate the max/min of the series again
                foreach (var s in this.Series.Where(s => s.IsVisible))
                {
                    s.UpdateMaxMin();
                }
            }

            foreach (var s in this.Series.Where(s => s.IsVisible))
            {
                s.UpdateAxisMaxMin();
            }

            foreach (var a in this.Axes)
            {
                a.UpdateActualMaxMin();
            }
        }
    }
}