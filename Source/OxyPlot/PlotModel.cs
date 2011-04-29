using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
		  public OxyRect Bounds { get; private set; }

        private int currentColorIndex;

        public ScreenPoint MidPoint { get; private set; }
        // The midpoint of the plot area, used for the polar coordinate system.

        public PlotModel(string title = null, string subtitle = null)
        {
            Axes = new Collection<IAxis>();
            Series = new Collection<ISeries>();
            Annotations = new Collection<IAnnotation>();

            Title = title;
            Subtitle = subtitle;

            // Default values
            PlotType = PlotType.XY;

            LegendPosition = LegendPosition.TopRight;
        		LegendLayout = LegendLayout.Vertical;
            IsLegendOutsidePlotArea = false;

            PlotMargins = new OxyThickness(60, 60, 50, 50);

            TitleFont = DefaultFont;
            TitleFontSize = 18;
            SubtitleFontSize = 14;
            TitleFontWeight = FontWeights.Bold;
            SubtitleFontWeight = FontWeights.Normal;
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
        /// Gets or sets the legend position.
        /// </summary>
        /// <value>The legend position.</value>
        public LegendPosition LegendPosition { get; set; }

		  /// <summary>
		  /// Gets or sets the legend layout. The default value is vertical.
		  /// </summary>
		  /// <value>The legend layout.</value>
		  public LegendLayout LegendLayout { get; set; }

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
        public void UpdateMaxMin()
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

        public void Render(IRenderContext rc)
        {
            if (rc.Width <= 0 || rc.Height <= 0)
                return;
            RenderInit(rc);

            UpdateAxisTransforms();
            RenderBackgrounds(rc);
            RenderAxes(rc);
            RenderAnnotations(rc, AnnotationLayer.BelowSeries);
            RenderSeries(rc);
            RenderAnnotations(rc, AnnotationLayer.OverSeries);
            RenderBox(rc);
        }

        private void RenderAnnotations(IRenderContext rc, AnnotationLayer layer)
        {
            foreach (var a in Annotations.Where(a => a.Layer == layer))
                a.Render(rc, this);
        }

        public void RenderInit(IRenderContext rc)
        {
            double w = rc.Width - PlotMargins.Left - PlotMargins.Right;
            double h = rc.Height - PlotMargins.Top - PlotMargins.Bottom;
        		double plotAreaTop = PlotMargins.Top;

				//Calculate size of legend box, and modify width and height of PlotArea accordingly:
        		OxyRect legendBox = GetLegendBoxRect(rc);

				if (legendBox.Height > 0.0)
				{
					if (IsLegendOutsidePlotArea)
					{
						if (LegendPosition == LegendPosition.Top)
						{
							h -= legendBox.Height;
							plotAreaTop += legendBox.Height;
						}
						else if (LegendPosition == LegendPosition.Bottom)
						{
							h -= legendBox.Height;
						}
					}
				}
        		if (legendBox.Width > 0.0 && IsLegendOutsidePlotArea && 
					(LegendPosition == LegendPosition.BottomLeft || LegendPosition == LegendPosition.BottomRight ||
					 LegendPosition == LegendPosition.TopLeft || LegendPosition == LegendPosition.TopRight))
				{
					w -= legendBox.Width;
				}

	        	if (w < 0) w = 0;
            if (h < 0) h = 0;

            PlotArea = new OxyRect
                           {
                               Left = PlotMargins.Left,
                               Width = w,
                               Top = plotAreaTop,
                               Height = h
                           };

			   //Calculate height and top of the area bounding the plot area and top/bottom axes:
        		double boundsTop = PlotArea.Top;
        		double boundsHeight = PlotArea.Height;
        		foreach (AxisBase axis in Axes)
        		{
					if(!axis.IsHorizontal())
						continue;

        			double axisTextHeight = 0.0;
					if (axis.TickStyle != TickStyle.None)
					{
						axisTextHeight = rc.MeasureText(axis.Minimum.ToString(), axis.FontFamily, axis.FontSize, axis.FontWeight).Height + 
													rc.MeasureText(axis.Title, axis.FontFamily, axis.FontSize, axis.FontWeight).Height;
					}

					if(axis.Position == AxisPosition.Top)
					{
						boundsTop -= axisTextHeight;
					}
					boundsHeight += axisTextHeight;
        		}

			  Bounds = new OxyRect(PlotArea.Left, boundsTop, PlotArea.Width, boundsHeight);

			  //TODO: Do the above for left/right axes as well?

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
        /// Renders the axes.
        /// </summary>
        /// <param name="rc">The rc.</param>
        public void RenderAxes(IRenderContext rc)
        {

            foreach (var a in Axes)
            {
                if (a.IsVisible)
                {
                    a.Render(rc, this);
                }
            }
        }

        /// <summary>
        /// Renders the series backgrounds.
        /// </summary>
        /// <param name="rc">The rc.</param>
        public void RenderBackgrounds(IRenderContext rc)
        {
            // Render the main background of the plot (only if there are axes)
            // The border is rendered in RenderBox.
            if (Axes.Count > 0)
                rc.DrawRectangle(PlotArea, Background, null, 0);

            foreach (var s in Series)
            {
                var s2 = s as PlotSeriesBase;
                if (s2 == null || s2.Background == null)
                    continue;
                rc.DrawRectangle(s2.GetScreenRectangle(), s2.Background, null, 0);
            }
        }

        /// <summary>
        /// Renders the box around the plot area.
        /// </summary>
        /// <param name="rc">The rc.</param>
        public void RenderBox(IRenderContext rc)
        {
            var pp = new PlotRenderingHelper(rc, this);

            // Render the title
            pp.RenderTitle(Title, Subtitle);

            // Render the box around the plot (only if there are axes)
            if (Axes.Count > 0)
            {
                rc.DrawBox(PlotArea, null, BoxColor, BoxThickness);
            }
            // Render the legends
            pp.RenderLegends();
        }

        /// <summary>
        /// Renders the series.
        /// </summary>
        /// <param name="rc">The rc.</param>
        public void RenderSeries(IRenderContext rc)
        {
            // Update undefined colors
            ResetDefaultColor();
            foreach (var s in Series)
            {
                s.SetDefaultValues(this);
            }

            foreach (var s in Series)
            {
                s.Render(rc, this);
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
                        // todo: only accept axis if it is within the plot area or the axis area
                        xaxis = axis;
                    }
                    else
                        yaxis = axis;
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

		  private OxyRect GetLegendBoxRect(IRenderContext rc)
		  {
		  		double height = 0.0;
				double width = 0.0;
			 
			 foreach (var s in Series)
			 {
				 if (String.IsNullOrEmpty(s.Title))
				 {
					 continue;
				 }

				 var oxySize = rc.MeasureMathText(s.Title, LegendFont, LegendFontSize, 500);

				 if (LegendLayout == LegendLayout.Vertical)
				 {
				 	height += oxySize.Height;
					if(oxySize.Width > width)
					{
						width = oxySize.Width;
					}
				 }
				 else
				 {
					 width += oxySize.Width;
				 	 if(oxySize.Height > height)
				 	 {
				 	 	height = oxySize.Height;
				 	 }
				 }
			 }

		  	return new OxyRect(0, 0, width, height);

		  }
    }
}