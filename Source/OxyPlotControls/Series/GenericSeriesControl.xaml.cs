using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Xml.Linq;
using OxyPlot;
using static OxyPlotControls.OxyPlotSettingsSerializer;

namespace OxyPlotControls
{
    /// <summary>
    /// A generic control for editing series properties that applies to all series types.
    /// Provides UI for common series properties and handles serialization/deserialization.
    /// </summary>
    public partial class GenericSeriesControl : UserControl
    {
        #region Constants

        /// <summary>
        /// The XML tag used for series properties in serialization.
        /// </summary>
        public static readonly string SeriesPropertiesTag = "Series";

        #endregion

        #region Static Properties

        /// <summary>
        /// Gets the list of available marker types (excluding Custom).
        /// </summary>
        public static List<MarkerType> MarkerTypeOptions
        {
            get
            {
                var types = new List<MarkerType>((MarkerType[])Enum.GetValues(typeof(MarkerType)));
                types.Remove(MarkerType.Custom);
                return types;
            }
        }

        /// <summary>
        /// Gets the list of available line legend positions.
        /// </summary>
        public static List<OxyPlot.Series.LineLegendPosition> LineLegendPositionOptions { get; } =
            new List<OxyPlot.Series.LineLegendPosition>((OxyPlot.Series.LineLegendPosition[])Enum.GetValues(typeof(OxyPlot.Series.LineLegendPosition)));

        /// <summary>
        /// Gets the list of available line join options.
        /// </summary>
        public static List<LineJoin> LineJoinOptions { get; } =
            new List<LineJoin>((LineJoin[])Enum.GetValues(typeof(LineJoin)));

        /// <summary>
        /// Gets the list of available line style options.
        /// </summary>
        public static List<DoubleCollection> LineStyleOptions => GenericControls.LineStyleSelectorControl.LineStyleOptions;

        #endregion

        #region Dependency Properties

        /// <summary>
        /// Identifies the <see cref="Series"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SeriesProperty = DependencyProperty.Register(
            nameof(Series),
            typeof(OxyPlot.Wpf.Series),
            typeof(GenericSeriesControl),
            new PropertyMetadata(null, InitializeControl));

        /// <summary>
        /// Gets or sets the series whose properties are being edited.
        /// </summary>
        public OxyPlot.Wpf.Series Series
        {
            get => (OxyPlot.Wpf.Series)GetValue(SeriesProperty);
            set => SetValue(SeriesProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ExpanderStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ExpanderStyleProperty = DependencyProperty.Register(
            nameof(ExpanderStyle),
            typeof(Style),
            typeof(GenericSeriesControl));

        /// <summary>
        /// Gets or sets the style to apply to expanders in this control.
        /// </summary>
        public Style ExpanderStyle
        {
            get => (Style)GetValue(ExpanderStyleProperty);
            set => SetValue(ExpanderStyleProperty, value);
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericSeriesControl"/> class.
        /// </summary>
        public GenericSeriesControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles changes to the Series property.
        /// </summary>
        private static void InitializeControl(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d == null) return;
            if (d.GetType() != typeof(GenericSeriesControl)) return;
            var thisControl = (GenericSeriesControl)d;

            if (e.NewValue == null) return;
            var wpfSeries = e.NewValue as OxyPlot.Wpf.Series;
            if (wpfSeries == null) return;

            // Note: The commented code in VB relates to setting up a data grid for series data
            // which is not currently implemented

            // Force layout update to sync bindings after series change
            thisControl.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Loaded, new Action(() =>
            {
                thisControl.UpdateLayout();
            }));
        }

        /// <summary>
        /// Closes all expanders in this control.
        /// </summary>
        public void CloseExpanders()
        {
            DisplayEXP.IsExpanded = false;
            MarkersEXP.IsExpanded = false;
            ErrorBarSettingsEXP.IsExpanded = false;
            BoxAndWhiskerEXP.IsExpanded = false;
        }

        /// <summary>
        /// Expands the specified property section.
        /// </summary>
        /// <param name="expansionZone">The property section to expand.</param>
        public void Expand(OxyPlotPropertiesControl.PropertyEXP expansionZone)
        {
            switch (expansionZone)
            {
                case OxyPlotPropertiesControl.PropertyEXP.Series_General:
                    DisplayEXP.IsExpanded = true;
                    break;
                case OxyPlotPropertiesControl.PropertyEXP.Series_Display:
                    DisplayEXP.IsExpanded = true;
                    break;
                case OxyPlotPropertiesControl.PropertyEXP.Series_Markers:
                    MarkersEXP.IsExpanded = true;
                    break;
                case OxyPlotPropertiesControl.PropertyEXP.Series_BoxAndWhiskers:
                    BoxAndWhiskerEXP.IsExpanded = true;
                    break;
                case OxyPlotPropertiesControl.PropertyEXP.Series_ErrorBarSettings:
                    ErrorBarSettingsEXP.IsExpanded = true;
                    break;
            }
        }

        #region Serialization - Plot Level

        /// <summary>
        /// Serializes all series properties from a plot to an XElement.
        /// </summary>
        /// <param name="plot">The plot containing the series to serialize.</param>
        /// <returns>An XElement containing all series properties.</returns>
        public static XElement SeriesPropertiesToXElement(OxyPlot.Wpf.Plot plot)
        {
            var seriesProperties = new XElement(SeriesPropertiesTag);
            foreach (OxyPlot.Wpf.Series series in plot.Series)
            {
                seriesProperties.Add(SeriesPropertiesToXElement(series));
            }
            return seriesProperties;
        }

        /// <summary>
        /// Deserializes series properties from an XElement and applies them to a plot.
        /// </summary>
        /// <param name="plot">The plot to populate with series.</param>
        /// <param name="element">The XElement containing series properties.</param>
        public static void XElementToSeriesProperties(OxyPlot.Wpf.Plot plot, XElement element)
        {
            // Early Exit
            if (element.Name != SeriesPropertiesTag) return;

            // Set up the series
            plot.Series.Clear();

            foreach (var el in element.Elements(SeriesPropertiesTag))
            {
                var tempSeries = XElementToSeriesProperties(el);
                if (tempSeries == null) continue;
                plot.Series.Add(tempSeries); // This doesn't bring any data, just loads the properties
            }
        }

        #endregion

        #region Serialization - Series Level

        /// <summary>
        /// Serializes a single series' properties to an XElement.
        /// </summary>
        /// <param name="series">The series to serialize.</param>
        /// <returns>An XElement containing the series properties.</returns>
        public static XElement SeriesPropertiesToXElement(OxyPlot.Wpf.Series series)
        {
            var seriesElement = new XElement(SeriesPropertiesTag);
            var seriesType = series.GetType();
            seriesElement.SetAttributeValue("SeriesType", seriesType.ToString());

            // Serialize general series properties
            var generalProperties = new XElement("General");
            generalProperties.SetAttributeValue(nameof(series.Name), series.Name.ToString());
            generalProperties.SetAttributeValue(nameof(series.Title), series.Title);
            generalProperties.SetAttributeValue(nameof(series.IsEnabled), series.IsEnabled.ToString());
            generalProperties.SetAttributeValue(nameof(series.Visibility), series.Visibility.ToString());
            generalProperties.SetAttributeValue(nameof(series.RenderInLegend), series.RenderInLegend.ToString());

            var bc = new BrushConverter();
            generalProperties.SetAttributeValue(nameof(series.Background), bc.ConvertToInvariantString(series.Background));
            generalProperties.SetAttributeValue(nameof(series.Foreground), bc.ConvertToInvariantString(series.Foreground));

            var fwc = new FontWeightConverter();
            var tc = new ThicknessConverter();
            var ffc = new FontFamilyConverter();
            generalProperties.SetAttributeValue(nameof(series.Color), series.Color.ToString());
            if (series.FontFamily != null)
                generalProperties.SetAttributeValue(nameof(series.FontFamily), ffc.ConvertToInvariantString(series.FontFamily));
            generalProperties.SetAttributeValue(nameof(series.FontSize), series.FontSize.ToString("G17", CultureInfo.InvariantCulture));
            generalProperties.SetAttributeValue(nameof(series.FontWeight), fwc.ConvertToInvariantString(series.FontWeight));
            generalProperties.SetAttributeValue(nameof(series.Padding), tc.ConvertToInvariantString(series.Padding));
            generalProperties.SetAttributeValue(nameof(series.TrackerFormatString), series.TrackerFormatString);
            generalProperties.SetAttributeValue(nameof(series.TrackerKey), series.TrackerKey);

            seriesElement.Add(generalProperties);

            // Serialize XY axis series properties
            var xyAxisSeries = series as OxyPlot.Wpf.XYAxisSeries;
            if (xyAxisSeries != null)
            {
                var xyAxisSeriesElement = new XElement(nameof(OxyPlot.Wpf.XYAxisSeries));
                xyAxisSeriesElement.SetAttributeValue(nameof(xyAxisSeries.XAxisKey), xyAxisSeries.XAxisKey);
                xyAxisSeriesElement.SetAttributeValue(nameof(xyAxisSeries.YAxisKey), xyAxisSeries.YAxisKey);
                seriesElement.Add(xyAxisSeriesElement);
            }

            // Serialize data point series properties
            var dataPointSeries = series as OxyPlot.Wpf.DataPointSeries;
            if (dataPointSeries != null)
            {
                var dataPointSeriesElement = new XElement(nameof(OxyPlot.Wpf.DataPointSeries));
                dataPointSeriesElement.SetAttributeValue(nameof(dataPointSeries.CanTrackerInterpolatePoints), dataPointSeries.CanTrackerInterpolatePoints.ToString());
                dataPointSeriesElement.SetAttributeValue(nameof(dataPointSeries.DataFieldX), dataPointSeries.DataFieldX);
                dataPointSeriesElement.SetAttributeValue(nameof(dataPointSeries.DataFieldY), dataPointSeries.DataFieldY);
                seriesElement.Add(dataPointSeriesElement);
            }

            // Serialize bar base series properties
            var barBaseSeries = series as OxyPlot.Wpf.BarSeriesBase;
            if (barBaseSeries != null)
            {
                var barBaseElement = new XElement(nameof(OxyPlot.Wpf.BarSeriesBase));
                barBaseElement.SetAttributeValue(nameof(barBaseSeries.BaseValue), barBaseSeries.BaseValue.ToString("G17", CultureInfo.InvariantCulture));
                barBaseElement.SetAttributeValue(nameof(barBaseSeries.FillColor), barBaseSeries.FillColor);
                barBaseElement.SetAttributeValue(nameof(barBaseSeries.ColorField), barBaseSeries.ColorField);
                barBaseElement.SetAttributeValue(nameof(barBaseSeries.ValueField), barBaseSeries.ValueField);
                barBaseElement.SetAttributeValue(nameof(barBaseSeries.LabelMargin), barBaseSeries.LabelMargin.ToString("G17", CultureInfo.InvariantCulture));
                barBaseElement.SetAttributeValue(nameof(barBaseSeries.StrokeColor), barBaseSeries.StrokeColor);
                barBaseElement.SetAttributeValue(nameof(barBaseSeries.StackGroup), barBaseSeries.StackGroup);
                barBaseElement.SetAttributeValue(nameof(barBaseSeries.NegativeFillColor), barBaseSeries.NegativeFillColor);
                barBaseElement.SetAttributeValue(nameof(barBaseSeries.LabelPlacement), barBaseSeries.LabelPlacement);
                barBaseElement.SetAttributeValue(nameof(barBaseSeries.LabelFormatString), barBaseSeries.LabelFormatString);
                barBaseElement.SetAttributeValue(nameof(barBaseSeries.StrokeThickness), barBaseSeries.StrokeThickness.ToString("G17", CultureInfo.InvariantCulture));
                barBaseElement.SetAttributeValue(nameof(barBaseSeries.IsStacked), barBaseSeries.IsStacked);
                seriesElement.Add(barBaseElement);
            }

            // Serialize bar series properties
            var barSeries = series as OxyPlot.Wpf.BarSeries;
            if (barSeries != null)
            {
                var barElement = new XElement(nameof(OxyPlot.Wpf.BarSeries));
                barElement.SetAttributeValue(nameof(barSeries.BarWidth), barSeries.BarWidth.ToString("G17", CultureInfo.InvariantCulture));
                seriesElement.Add(barElement);
            }

            // Serialize column series properties
            var columnSeries = series as OxyPlot.Wpf.ColumnSeries;
            if (columnSeries != null)
            {
                var columnElement = new XElement(nameof(OxyPlot.Wpf.ColumnSeries));
                columnElement.SetAttributeValue(nameof(columnSeries.ColumnWidth), columnSeries.ColumnWidth.ToString("G17", CultureInfo.InvariantCulture));
                seriesElement.Add(columnElement);
            }

            // Serialize histogram series properties
            var histogramSeries = series as OxyPlot.Wpf.HistogramSeries;
            if (histogramSeries != null)
            {
                var histogramElement = new XElement(nameof(OxyPlot.Wpf.HistogramSeries));
                histogramElement.SetAttributeValue(nameof(histogramSeries.FillColor), histogramSeries.FillColor);
                histogramElement.SetAttributeValue(nameof(histogramSeries.NegativeFillColor), histogramSeries.NegativeFillColor);
                histogramElement.SetAttributeValue(nameof(histogramSeries.LabelFormatString), histogramSeries.LabelFormatString);
                histogramElement.SetAttributeValue(nameof(histogramSeries.LabelPlacement), histogramSeries.LabelPlacement);
                histogramElement.SetAttributeValue(nameof(histogramSeries.StrokeColor), histogramSeries.StrokeColor);
                histogramElement.SetAttributeValue(nameof(histogramSeries.StrokeThickness), histogramSeries.StrokeThickness.ToString("G17", CultureInfo.InvariantCulture));
                seriesElement.Add(histogramElement);
            }

            // Serialize line series properties
            var lineSeries = series as OxyPlot.Wpf.LineSeries;
            if (lineSeries != null)
            {
                var lineSeriesElement = new XElement(nameof(OxyPlot.Wpf.LineSeries));
                lineSeriesElement.SetAttributeValue(nameof(lineSeries.LineJoin), lineSeries.LineJoin);
                lineSeriesElement.SetAttributeValue(nameof(lineSeries.LineLegendPosition), lineSeries.LineLegendPosition);
                lineSeriesElement.SetAttributeValue(nameof(lineSeries.LineStyle), lineSeries.LineStyle);
                lineSeriesElement.SetAttributeValue(nameof(lineSeries.MarkerFill), lineSeries.MarkerFill);
                lineSeriesElement.SetAttributeValue(nameof(lineSeries.MarkerOutline), lineSeries.MarkerOutline);
                lineSeriesElement.SetAttributeValue(nameof(lineSeries.MarkerResolution), lineSeries.MarkerResolution.ToString("G17", CultureInfo.InvariantCulture));
                lineSeriesElement.SetAttributeValue(nameof(lineSeries.MarkerSize), lineSeries.MarkerSize.ToString("G17", CultureInfo.InvariantCulture));
                lineSeriesElement.SetAttributeValue(nameof(lineSeries.MarkerStroke), lineSeries.MarkerStroke);
                lineSeriesElement.SetAttributeValue(nameof(lineSeries.MarkerStrokeThickness), lineSeries.MarkerStrokeThickness.ToString("G17", CultureInfo.InvariantCulture));
                lineSeriesElement.SetAttributeValue(nameof(lineSeries.MarkerType), lineSeries.MarkerType);
                lineSeriesElement.SetAttributeValue(nameof(lineSeries.MinimumSegmentLength), lineSeries.MinimumSegmentLength.ToString("G17", CultureInfo.InvariantCulture));
                lineSeriesElement.SetAttributeValue(nameof(lineSeries.StrokeThickness), lineSeries.StrokeThickness.ToString("G17", CultureInfo.InvariantCulture));
                lineSeriesElement.SetAttributeValue(nameof(lineSeries.LabelFormatString), lineSeries.LabelFormatString);
                lineSeriesElement.SetAttributeValue(nameof(lineSeries.LabelMargin), lineSeries.LabelMargin.ToString("G17", CultureInfo.InvariantCulture));
                lineSeriesElement.SetAttributeValue(nameof(lineSeries.BrokenLineColor), lineSeries.BrokenLineColor);
                lineSeriesElement.SetAttributeValue(nameof(lineSeries.BrokenLineStyle), lineSeries.BrokenLineStyle);
                lineSeriesElement.SetAttributeValue(nameof(lineSeries.BrokenLineThickness), lineSeries.BrokenLineThickness.ToString("G17", CultureInfo.InvariantCulture));
                seriesElement.Add(lineSeriesElement);
            }

            // Serialize area series properties
            var areaSeries = series as OxyPlot.Wpf.AreaSeries;
            if (areaSeries != null)
            {
                var areaSeriesElement = new XElement(nameof(OxyPlot.Wpf.AreaSeries));
                areaSeriesElement.SetAttributeValue(nameof(areaSeries.Color2), areaSeries.Color2);
                areaSeriesElement.SetAttributeValue(nameof(areaSeries.Fill), areaSeries.Fill);
                areaSeriesElement.SetAttributeValue(nameof(areaSeries.DataFieldX2), areaSeries.DataFieldX2);
                areaSeriesElement.SetAttributeValue(nameof(areaSeries.DataFieldY2), areaSeries.DataFieldY2);
                areaSeriesElement.SetAttributeValue(nameof(areaSeries.Reverse2), areaSeries.Reverse2);
                seriesElement.Add(areaSeriesElement);
            }

            // Serialize boxplot series properties
            var boxPlotSeries = series as OxyPlot.Wpf.BoxPlotSeries;
            if (boxPlotSeries != null)
            {
                var boxPlotElement = new XElement(nameof(OxyPlot.Wpf.BoxPlotSeries));
                boxPlotElement.SetAttributeValue(nameof(boxPlotSeries.StrokeThickness), boxPlotSeries.StrokeThickness.ToString("G17", CultureInfo.InvariantCulture));
                boxPlotElement.SetAttributeValue(nameof(boxPlotSeries.Stroke), boxPlotSeries.Stroke);
                boxPlotElement.SetAttributeValue(nameof(boxPlotSeries.LineStyle), boxPlotSeries.LineStyle);
                boxPlotElement.SetAttributeValue(nameof(boxPlotSeries.IsVertical), boxPlotSeries.IsVertical);
                boxPlotElement.SetAttributeValue(nameof(boxPlotSeries.Fill), boxPlotSeries.Fill);
                boxPlotElement.SetAttributeValue(nameof(boxPlotSeries.OutlierType), boxPlotSeries.OutlierType);
                boxPlotElement.SetAttributeValue(nameof(boxPlotSeries.WhiskerWidth), boxPlotSeries.WhiskerWidth.ToString("G17", CultureInfo.InvariantCulture));
                boxPlotElement.SetAttributeValue(nameof(boxPlotSeries.ShowMedianAsDot), boxPlotSeries.ShowMedianAsDot);
                boxPlotElement.SetAttributeValue(nameof(boxPlotSeries.MedianPointSize), boxPlotSeries.MedianPointSize.ToString("G17", CultureInfo.InvariantCulture));
                boxPlotElement.SetAttributeValue(nameof(boxPlotSeries.OutlierSize), boxPlotSeries.OutlierSize.ToString("G17", CultureInfo.InvariantCulture));
                boxPlotElement.SetAttributeValue(nameof(boxPlotSeries.BoxWidth), boxPlotSeries.BoxWidth.ToString("G17", CultureInfo.InvariantCulture));
                boxPlotElement.SetAttributeValue(nameof(boxPlotSeries.ShowBox), boxPlotSeries.ShowBox);
                seriesElement.Add(boxPlotElement);
            }

            // Serialize scatter point series properties
            var scatterPointSeries = series as OxyPlot.Wpf.ScatterPointSeries;
            if (scatterPointSeries != null)
            {
                var scatterPointElement = new XElement(nameof(OxyPlot.Wpf.ScatterPointSeries));
                scatterPointElement.SetAttributeValue(nameof(scatterPointSeries.DataFieldTag), scatterPointSeries.DataFieldTag);
                scatterPointElement.SetAttributeValue(nameof(scatterPointSeries.DataFieldValue), scatterPointSeries.DataFieldValue);
                scatterPointElement.SetAttributeValue(nameof(scatterPointSeries.ColorAxisKey), scatterPointSeries.ColorAxisKey);
                scatterPointElement.SetAttributeValue(nameof(scatterPointSeries.BinSize), scatterPointSeries.BinSize.ToString("G17", CultureInfo.InvariantCulture));
                scatterPointElement.SetAttributeValue(nameof(scatterPointSeries.MarkerFill), scatterPointSeries.MarkerFill);
                scatterPointElement.SetAttributeValue(nameof(scatterPointSeries.MarkerOutline), scatterPointSeries.MarkerOutline);
                scatterPointElement.SetAttributeValue(nameof(scatterPointSeries.MarkerSize), scatterPointSeries.MarkerSize.ToString("G17", CultureInfo.InvariantCulture));
                scatterPointElement.SetAttributeValue(nameof(scatterPointSeries.MarkerStroke), scatterPointSeries.MarkerStroke);
                scatterPointElement.SetAttributeValue(nameof(scatterPointSeries.MarkerStrokeThickness), scatterPointSeries.MarkerStrokeThickness.ToString("G17", CultureInfo.InvariantCulture));
                scatterPointElement.SetAttributeValue(nameof(scatterPointSeries.MarkerType), scatterPointSeries.MarkerType);
                seriesElement.Add(scatterPointElement);
            }

            // Serialize error bar series properties
            var errorBarsSeries = series as OxyPlot.Wpf.ScatterErrorSeries;
            if (errorBarsSeries != null)
            {
                var element = new XElement(nameof(OxyPlot.Wpf.ScatterErrorSeries));
                element.SetAttributeValue(nameof(errorBarsSeries.DataFieldTag), errorBarsSeries.DataFieldTag);
                element.SetAttributeValue(nameof(errorBarsSeries.DataFieldValue), errorBarsSeries.DataFieldValue);
                element.SetAttributeValue(nameof(errorBarsSeries.ColorAxisKey), errorBarsSeries.ColorAxisKey);
                element.SetAttributeValue(nameof(errorBarsSeries.BinSize), errorBarsSeries.BinSize.ToString("G17", CultureInfo.InvariantCulture));
                element.SetAttributeValue(nameof(errorBarsSeries.MarkerFill), errorBarsSeries.MarkerFill);
                element.SetAttributeValue(nameof(errorBarsSeries.MarkerOutline), errorBarsSeries.MarkerOutline);
                element.SetAttributeValue(nameof(errorBarsSeries.MarkerSize), errorBarsSeries.MarkerSize.ToString("G17", CultureInfo.InvariantCulture));
                element.SetAttributeValue(nameof(errorBarsSeries.MarkerStroke), errorBarsSeries.MarkerStroke);
                element.SetAttributeValue(nameof(errorBarsSeries.MarkerStrokeThickness), errorBarsSeries.MarkerStrokeThickness.ToString("G17", CultureInfo.InvariantCulture));
                element.SetAttributeValue(nameof(errorBarsSeries.MarkerType), errorBarsSeries.MarkerType);
                element.SetAttributeValue(nameof(errorBarsSeries.ErrorBarStopWidth), errorBarsSeries.ErrorBarStopWidth.ToString("G17", CultureInfo.InvariantCulture));
                element.SetAttributeValue(nameof(errorBarsSeries.MinimumErrorSize), errorBarsSeries.MinimumErrorSize.ToString("G17", CultureInfo.InvariantCulture));
                element.SetAttributeValue(nameof(errorBarsSeries.ErrorBarStrokeThickness), errorBarsSeries.ErrorBarStrokeThickness.ToString("G17", CultureInfo.InvariantCulture));
                element.SetAttributeValue(nameof(errorBarsSeries.ErrorBarColor), errorBarsSeries.ErrorBarColor);
                element.SetAttributeValue(nameof(errorBarsSeries.DataFieldLowerErrorX), errorBarsSeries.DataFieldLowerErrorX);
                element.SetAttributeValue(nameof(errorBarsSeries.DataFieldUpperErrorX), errorBarsSeries.DataFieldUpperErrorX);
                element.SetAttributeValue(nameof(errorBarsSeries.DataFieldLowerErrorY), errorBarsSeries.DataFieldLowerErrorY);
                element.SetAttributeValue(nameof(errorBarsSeries.DataFieldUpperErrorY), errorBarsSeries.DataFieldUpperErrorY);
                seriesElement.Add(element);
            }

            // Serialize heat map series properties
            var heatMapSeries = series as OxyPlot.Wpf.HeatMapSeries;
            if (heatMapSeries != null)
            {
                var heatMapElement = new XElement(nameof(OxyPlot.Wpf.HeatMapSeries));
                heatMapElement.SetAttributeValue(nameof(heatMapSeries.ColorAxisKey), heatMapSeries.ColorAxisKey);
                heatMapElement.SetAttributeValue(nameof(heatMapSeries.Y0), heatMapSeries.Y0.ToString("G17", CultureInfo.InvariantCulture));
                heatMapElement.SetAttributeValue(nameof(heatMapSeries.Y1), heatMapSeries.Y1.ToString("G17", CultureInfo.InvariantCulture));
                heatMapElement.SetAttributeValue(nameof(heatMapSeries.X0), heatMapSeries.X0.ToString("G17", CultureInfo.InvariantCulture));
                heatMapElement.SetAttributeValue(nameof(heatMapSeries.X1), heatMapSeries.X1.ToString("G17", CultureInfo.InvariantCulture));
                heatMapElement.SetAttributeValue(nameof(heatMapSeries.HighColor), heatMapSeries.HighColor);
                heatMapElement.SetAttributeValue(nameof(heatMapSeries.CoordinateDefinition), heatMapSeries.CoordinateDefinition);
                heatMapElement.SetAttributeValue(nameof(heatMapSeries.Interpolate), heatMapSeries.Interpolate);
                heatMapElement.SetAttributeValue(nameof(heatMapSeries.LabelFontSize), heatMapSeries.LabelFontSize.ToString("G17", CultureInfo.InvariantCulture));
                heatMapElement.SetAttributeValue(nameof(heatMapSeries.LowColor), heatMapSeries.LowColor);
                seriesElement.Add(heatMapElement);
            }

            return seriesElement;
        }

        /// <summary>
        /// Deserializes series properties from an XElement and creates the appropriate series type.
        /// </summary>
        /// <param name="element">The XElement containing series properties.</param>
        /// <returns>A new series instance with the deserialized properties, or null if the type is not recognized.</returns>
        public static OxyPlot.Wpf.Series XElementToSeriesProperties(XElement element)
        {
            // Early Exit
            if (element == null) return null;
            if (element.Name != SeriesPropertiesTag) return null;

            // Set up converters
            var fontWeightConverter = new FontWeightConverter();
            var thicknessConverter = new ThicknessConverter();
            var oxycolorConverter = new OxyPlot.Wpf.OxyColorConverter();
            var brushConverter = new BrushConverter();
            var fontFamilyConverter = new FontFamilyConverter();
            var booleanToVisibilityConverter = new BooleanToVisibilityConverter();

            // Set up series to return
            OxyPlot.Wpf.Series series = null;

            GetStringAttribute(element, "SeriesType", out string seriesType);

            // Create the Series Type and Deserialize properties specific to the series
            if (seriesType == typeof(OxyPlot.Wpf.HeatMapSeries).ToString())
                series = new OxyPlot.Wpf.HeatMapSeries();
            else if (seriesType == typeof(OxyPlot.Wpf.LineSeries).ToString())
                series = new OxyPlot.Wpf.LineSeries();
            else if (seriesType == typeof(OxyPlot.Wpf.ColumnSeries).ToString())
                series = new OxyPlot.Wpf.ColumnSeries();
            else if (seriesType == typeof(OxyPlot.Wpf.BarSeries).ToString())
                series = new OxyPlot.Wpf.BarSeries();
            else if (seriesType == typeof(OxyPlot.Wpf.HistogramSeries).ToString())
                series = new OxyPlot.Wpf.HistogramSeries();
            else if (seriesType == typeof(OxyPlot.Wpf.ScatterPointSeries).ToString())
                series = new OxyPlot.Wpf.ScatterPointSeries();
            else if (seriesType == typeof(OxyPlot.Wpf.ScatterErrorSeries).ToString())
                series = new OxyPlot.Wpf.ScatterErrorSeries();
            else if (seriesType == typeof(OxyPlot.Wpf.AreaSeries).ToString())
                series = new OxyPlot.Wpf.AreaSeries();
            else if (seriesType == typeof(OxyPlot.Wpf.BoxPlotSeries).ToString())
                series = new OxyPlot.Wpf.BoxPlotSeries();
            else
                return null; // not a recognized type

            var bConverter = new BrushConverter();

            // Deserialize General Series Properties
            var generalElement = element.Element("General");
            if (generalElement != null)
            {
                if (GetStringAttribute(generalElement, nameof(series.Name), out var name)) series.Name = name;
                if (GetStringAttribute(generalElement, nameof(series.Title), out var title)) series.Title = title;
                if (GetBooleanAttribute(generalElement, nameof(series.IsEnabled), out var isEnabled)) series.IsEnabled = isEnabled;
                if (generalElement.Attribute(nameof(series.Visibility)) != null)
                {
                    var visibilityString = generalElement.Attribute(nameof(series.Visibility)).Value;
                    if (visibilityString == "Visible") series.Visibility = Visibility.Visible;
                    if (visibilityString == "Hidden") series.Visibility = Visibility.Hidden;
                    if (visibilityString == "Collapsed") series.Visibility = Visibility.Collapsed;
                }

                if (GetBooleanAttribute(generalElement, nameof(series.RenderInLegend), out var renderInLegend)) series.RenderInLegend = renderInLegend;
                if (GetBrushAttribute(generalElement, nameof(series.Background), bConverter, out var background)) series.Background = background;
                if (GetBrushAttribute(generalElement, nameof(series.Foreground), bConverter, out var foreground)) series.Foreground = foreground;
                if (GetColorAttribute(generalElement, nameof(series.Color), out var color)) series.Color = color;
                if (GetFontFamilyAttribute(generalElement, nameof(series.FontFamily), fontFamilyConverter, out var fontFamily)) series.FontFamily = fontFamily;
                if (GetDoubleAttribute(generalElement, nameof(series.FontSize), out var fontSize)) series.FontSize = fontSize;
                if (GetFontWeightAttribute(generalElement, nameof(series.FontWeight), fontWeightConverter, out var fontWeight)) series.FontWeight = fontWeight;
                if (GetThicknessAttribute(generalElement, nameof(series.Padding), thicknessConverter, out var padding)) series.Padding = padding;
                if (GetStringAttribute(generalElement, nameof(series.TrackerFormatString), out var trackerFormatString)) series.TrackerFormatString = trackerFormatString;
                if (GetStringAttribute(generalElement, nameof(series.TrackerKey), out var trackerKey)) series.TrackerKey = trackerKey;
            }

            // Deserialize XY Axis Series Properties
            var xyAxisSeries = series as OxyPlot.Wpf.XYAxisSeries;
            if (xyAxisSeries != null)
            {
                var xyAxesSeriesElement = element.Element(nameof(OxyPlot.Wpf.XYAxisSeries));
                if (GetStringAttribute(xyAxesSeriesElement, nameof(xyAxisSeries.XAxisKey), out var xAxisKey)) xyAxisSeries.XAxisKey = xAxisKey;
                if (GetStringAttribute(xyAxesSeriesElement, nameof(xyAxisSeries.YAxisKey), out var yAxisKey)) xyAxisSeries.YAxisKey = yAxisKey;
            }

            // Deserialize Data Series Properties
            var dataPointSeries = series as OxyPlot.Wpf.DataPointSeries;
            if (dataPointSeries != null)
            {
                var dataPointSeriesElement = element.Element(nameof(OxyPlot.Wpf.DataPointSeries));
                if (GetBooleanAttribute(dataPointSeriesElement, nameof(dataPointSeries.CanTrackerInterpolatePoints), out var canTrackerInterpolatePoints)) dataPointSeries.CanTrackerInterpolatePoints = canTrackerInterpolatePoints;
                if (GetStringAttribute(dataPointSeriesElement, nameof(dataPointSeries.DataFieldX), out var dataFieldX)) dataPointSeries.DataFieldX = dataFieldX;
                if (GetStringAttribute(dataPointSeriesElement, nameof(dataPointSeries.DataFieldY), out var dataFieldY)) dataPointSeries.DataFieldY = dataFieldY;
            }

            // Deserialize Bar series Base properties
            var barBaseSeries = series as OxyPlot.Wpf.BarSeriesBase;
            if (barBaseSeries != null)
            {
                var barBaseSeriesElement = element.Element(nameof(OxyPlot.Wpf.BarSeriesBase));
                if (GetDoubleAttribute(barBaseSeriesElement, nameof(barBaseSeries.BaseValue), out var baseValue)) barBaseSeries.BaseValue = baseValue;
                if (GetColorAttribute(barBaseSeriesElement, nameof(barBaseSeries.FillColor), out var fillColor)) barBaseSeries.FillColor = fillColor;
                if (GetStringAttribute(barBaseSeriesElement, nameof(barBaseSeries.ColorField), out var colorField)) barBaseSeries.ColorField = colorField;
                if (GetStringAttribute(barBaseSeriesElement, nameof(barBaseSeries.ValueField), out var valueField)) barBaseSeries.ValueField = valueField;
                if (GetDoubleAttribute(barBaseSeriesElement, nameof(barBaseSeries.LabelMargin), out var labelMargin)) barBaseSeries.LabelMargin = labelMargin;
                if (GetColorAttribute(barBaseSeriesElement, nameof(barBaseSeries.StrokeColor), out var strokeColor)) barBaseSeries.StrokeColor = strokeColor;
                if (GetStringAttribute(barBaseSeriesElement, nameof(barBaseSeries.StackGroup), out var stackGroup)) barBaseSeries.StackGroup = stackGroup;
                if (GetColorAttribute(barBaseSeriesElement, nameof(barBaseSeries.NegativeFillColor), out var negativeFillColor)) barBaseSeries.NegativeFillColor = negativeFillColor;
                if (!GetEnumAttribute(barBaseSeriesElement, nameof(barBaseSeries.LabelPlacement), out OxyPlot.Series.LabelPlacement labelPlacement))
                    labelPlacement = OxyPlot.Series.LabelPlacement.Inside;
                barBaseSeries.LabelPlacement = labelPlacement;
                if (GetStringAttribute(barBaseSeriesElement, nameof(barBaseSeries.LabelFormatString), out var barLabelFormatString)) barBaseSeries.LabelFormatString = barLabelFormatString;
                if (GetDoubleAttribute(barBaseSeriesElement, nameof(barBaseSeries.StrokeThickness), out var strokeThickness)) barBaseSeries.StrokeThickness = strokeThickness;
                if (GetBooleanAttribute(barBaseSeriesElement, nameof(barBaseSeries.IsStacked), out var isStacked)) barBaseSeries.IsStacked = isStacked;
            }

            // Deserialize bar series properties
            var barSeries = series as OxyPlot.Wpf.BarSeries;
            if (barSeries != null)
            {
                var barSeriesElement = element.Element(nameof(OxyPlot.Wpf.BarSeries));
                if (GetDoubleAttribute(barSeriesElement, nameof(barSeries.BarWidth), out var barWidth)) barSeries.BarWidth = barWidth;
            }

            // Deserialize column properties
            var columnSeries = series as OxyPlot.Wpf.ColumnSeries;
            if (columnSeries != null)
            {
                var columnSeriesElement = element.Element(nameof(OxyPlot.Wpf.ColumnSeries));
                if (GetDoubleAttribute(columnSeriesElement, nameof(columnSeries.ColumnWidth), out var columnWidth)) columnSeries.ColumnWidth = columnWidth;
            }

            // Deserialize histogram series properties
            var histogramSeries = series as OxyPlot.Wpf.HistogramSeries;
            if (histogramSeries != null)
            {
                var histogramSeriesElement = element.Element(nameof(OxyPlot.Wpf.HistogramSeries));
                if (GetColorAttribute(histogramSeriesElement, nameof(histogramSeries.FillColor), out var histFillColor)) histogramSeries.FillColor = histFillColor;
                if (GetColorAttribute(histogramSeriesElement, nameof(histogramSeries.NegativeFillColor), out var histNegativeFillColor)) histogramSeries.NegativeFillColor = histNegativeFillColor;
                if (GetStringAttribute(histogramSeriesElement, nameof(histogramSeries.LabelFormatString), out var histLabelFormatString)) histogramSeries.LabelFormatString = histLabelFormatString;
                if (!GetEnumAttribute(histogramSeriesElement, nameof(histogramSeries.LabelPlacement), out OxyPlot.Series.LabelPlacement histLabelPlacement))
                    histLabelPlacement = OxyPlot.Series.LabelPlacement.Inside;
                histogramSeries.LabelPlacement = histLabelPlacement;
                if (GetColorAttribute(histogramSeriesElement, nameof(histogramSeries.StrokeColor), out var histStrokeColor)) histogramSeries.StrokeColor = histStrokeColor;
                if (GetDoubleAttribute(histogramSeriesElement, nameof(histogramSeries.StrokeThickness), out var histStrokeThickness)) histogramSeries.StrokeThickness = histStrokeThickness;
            }

            // Deserialize line series properties
            var lineSeries = series as OxyPlot.Wpf.LineSeries;
            if (lineSeries != null)
            {
                var lineSeriesElement = element.Element(nameof(OxyPlot.Wpf.LineSeries));
                if (!GetEnumAttribute(lineSeriesElement, nameof(lineSeries.LineJoin), out OxyPlot.LineJoin lineJoin))
                    lineJoin = OxyPlot.LineJoin.Bevel;
                lineSeries.LineJoin = lineJoin;
                if (!GetEnumAttribute(lineSeriesElement, nameof(lineSeries.LineLegendPosition), out OxyPlot.Series.LineLegendPosition lineLegendPosition))
                    lineLegendPosition = OxyPlot.Series.LineLegendPosition.End;
                lineSeries.LineLegendPosition = lineLegendPosition;
                if (!GetEnumAttribute(lineSeriesElement, nameof(lineSeries.LineStyle), out OxyPlot.LineStyle lineStyle))
                    lineStyle = OxyPlot.LineStyle.Automatic;
                lineSeries.LineStyle = lineStyle;
                if (GetColorAttribute(lineSeriesElement, nameof(lineSeries.MarkerFill), out var lineMarkerFill)) lineSeries.MarkerFill = lineMarkerFill;
                // TODO - Deserialize MarkerOutline value into point array
                if (GetIntegerAttribute(lineSeriesElement, nameof(lineSeries.MarkerResolution), out var markerResolution)) lineSeries.MarkerResolution = markerResolution;
                if (GetDoubleAttribute(lineSeriesElement, nameof(lineSeries.MarkerSize), out var lineMarkerSize)) lineSeries.MarkerSize = lineMarkerSize;
                if (GetColorAttribute(lineSeriesElement, nameof(lineSeries.MarkerStroke), out var lineMarkerStroke)) lineSeries.MarkerStroke = lineMarkerStroke;
                if (GetDoubleAttribute(lineSeriesElement, nameof(lineSeries.MarkerStrokeThickness), out var lineMarkerStrokeThickness)) lineSeries.MarkerStrokeThickness = lineMarkerStrokeThickness;
                if (!GetEnumAttribute(lineSeriesElement, nameof(lineSeries.MarkerType), out OxyPlot.MarkerType lineMarkerType))
                    lineMarkerType = OxyPlot.MarkerType.Circle;
                lineSeries.MarkerType = lineMarkerType;
                if (GetDoubleAttribute(lineSeriesElement, nameof(lineSeries.MinimumSegmentLength), out var minimumSegmentLength)) lineSeries.MinimumSegmentLength = minimumSegmentLength;
                if (GetDoubleAttribute(lineSeriesElement, nameof(lineSeries.StrokeThickness), out var lineStrokeThickness)) lineSeries.StrokeThickness = lineStrokeThickness;
                if (GetStringAttribute(lineSeriesElement, nameof(lineSeries.LabelFormatString), out var lineLabelFormatString)) lineSeries.LabelFormatString = lineLabelFormatString;
                if (GetDoubleAttribute(lineSeriesElement, nameof(lineSeries.LabelMargin), out var lineLabelMargin)) lineSeries.LabelMargin = lineLabelMargin;
                if (GetColorAttribute(lineSeriesElement, nameof(lineSeries.BrokenLineColor), out var brokenLineColor)) lineSeries.BrokenLineColor = brokenLineColor;
                if (!GetEnumAttribute(lineSeriesElement, nameof(lineSeries.BrokenLineStyle), out OxyPlot.LineStyle brokenLineStyle))
                    brokenLineStyle = OxyPlot.LineStyle.Automatic;
                lineSeries.BrokenLineStyle = brokenLineStyle;
                if (GetDoubleAttribute(lineSeriesElement, nameof(lineSeries.BrokenLineThickness), out var brokenLineThickness)) lineSeries.BrokenLineThickness = brokenLineThickness;
            }

            // Deserialize area series properties
            var areaSeries = series as OxyPlot.Wpf.AreaSeries;
            if (areaSeries != null)
            {
                var areaSeriesElement = element.Element(nameof(OxyPlot.Wpf.AreaSeries));
                if (GetColorAttribute(areaSeriesElement, nameof(areaSeries.Color2), out var color2)) areaSeries.Color2 = color2;
                if (GetColorAttribute(areaSeriesElement, nameof(areaSeries.Fill), out var areaFill)) areaSeries.Fill = areaFill;
                if (GetStringAttribute(areaSeriesElement, nameof(areaSeries.DataFieldX2), out var dataFieldX2)) areaSeries.DataFieldX2 = dataFieldX2;
                if (GetStringAttribute(areaSeriesElement, nameof(areaSeries.DataFieldY2), out var dataFieldY2)) areaSeries.DataFieldY2 = dataFieldY2;
                if (GetBooleanAttribute(areaSeriesElement, nameof(areaSeries.Reverse2), out var reverse2)) areaSeries.Reverse2 = reverse2;
            }

            // Deserialize box plot properties
            var boxPlotSeries = series as OxyPlot.Wpf.BoxPlotSeries;
            if (boxPlotSeries != null)
            {
                var boxPlotseriesElement = element.Element(nameof(OxyPlot.Wpf.BoxPlotSeries));
                if (GetDoubleAttribute(boxPlotseriesElement, nameof(boxPlotSeries.StrokeThickness), out var boxStrokeThickness)) boxPlotSeries.StrokeThickness = boxStrokeThickness;
                if (GetColorAttribute(boxPlotseriesElement, nameof(boxPlotSeries.Stroke), out var boxStroke)) boxPlotSeries.Stroke = boxStroke;
                if (!GetEnumAttribute(boxPlotseriesElement, nameof(boxPlotSeries.LineStyle), out OxyPlot.LineStyle boxLineStyle))
                    boxLineStyle = OxyPlot.LineStyle.Automatic;
                boxPlotSeries.LineStyle = boxLineStyle;
                if (GetBooleanAttribute(boxPlotseriesElement, nameof(boxPlotSeries.IsVertical), out var isVertical)) boxPlotSeries.IsVertical = isVertical;
                if (GetColorAttribute(boxPlotseriesElement, nameof(boxPlotSeries.Fill), out var boxFill)) boxPlotSeries.Fill = boxFill;
                if (!GetEnumAttribute(boxPlotseriesElement, nameof(boxPlotSeries.OutlierType), out OxyPlot.MarkerType outlierType))
                    outlierType = OxyPlot.MarkerType.Circle;
                boxPlotSeries.OutlierType = outlierType;
                if (GetDoubleAttribute(boxPlotseriesElement, nameof(boxPlotSeries.WhiskerWidth), out var whiskerWidth)) boxPlotSeries.WhiskerWidth = whiskerWidth;
                if (GetBooleanAttribute(boxPlotseriesElement, nameof(boxPlotSeries.ShowMedianAsDot), out var showMedianAsDot)) boxPlotSeries.ShowMedianAsDot = showMedianAsDot;
                if (GetDoubleAttribute(boxPlotseriesElement, nameof(boxPlotSeries.MedianPointSize), out var medianPointSize)) boxPlotSeries.MedianPointSize = medianPointSize;
                if (GetDoubleAttribute(boxPlotseriesElement, nameof(boxPlotSeries.OutlierSize), out var outlierSize)) boxPlotSeries.OutlierSize = outlierSize;
                if (GetDoubleAttribute(boxPlotseriesElement, nameof(boxPlotSeries.BoxWidth), out var boxWidth)) boxPlotSeries.BoxWidth = boxWidth;
                if (GetBooleanAttribute(boxPlotseriesElement, nameof(boxPlotSeries.ShowBox), out var showBox)) boxPlotSeries.ShowBox = showBox;
            }

            // Deserialize scatter point series properties
            var scatterPointSeries = series as OxyPlot.Wpf.ScatterPointSeries;
            if (scatterPointSeries != null)
            {
                var scatterPointSeriesElement = element.Element(nameof(OxyPlot.Wpf.ScatterPointSeries));
                if (GetStringAttribute(scatterPointSeriesElement, nameof(scatterPointSeries.DataFieldTag), out var scatterDataFieldTag)) scatterPointSeries.DataFieldTag = scatterDataFieldTag;
                if (GetStringAttribute(scatterPointSeriesElement, nameof(scatterPointSeries.DataFieldValue), out var scatterDataFieldValue)) scatterPointSeries.DataFieldValue = scatterDataFieldValue;
                if (GetStringAttribute(scatterPointSeriesElement, nameof(scatterPointSeries.ColorAxisKey), out var scatterColorAxisKey)) scatterPointSeries.ColorAxisKey = scatterColorAxisKey;
                if (GetIntegerAttribute(scatterPointSeriesElement, nameof(scatterPointSeries.BinSize), out var scatterBinSize)) scatterPointSeries.BinSize = scatterBinSize;
                if (GetColorAttribute(scatterPointSeriesElement, nameof(scatterPointSeries.MarkerFill), out var scatterMarkerFill)) scatterPointSeries.MarkerFill = scatterMarkerFill;
                if (GetDoubleAttribute(scatterPointSeriesElement, nameof(scatterPointSeries.MarkerSize), out var scatterMarkerSize)) scatterPointSeries.MarkerSize = scatterMarkerSize;
                if (GetColorAttribute(scatterPointSeriesElement, nameof(scatterPointSeries.MarkerStroke), out var scatterMarkerStroke)) scatterPointSeries.MarkerStroke = scatterMarkerStroke;
                if (GetDoubleAttribute(scatterPointSeriesElement, nameof(scatterPointSeries.MarkerStrokeThickness), out var scatterMarkerStrokeThickness)) scatterPointSeries.MarkerStrokeThickness = scatterMarkerStrokeThickness;
                if (!GetEnumAttribute(scatterPointSeriesElement, nameof(scatterPointSeries.MarkerType), out OxyPlot.MarkerType scatterMarkerType))
                    scatterMarkerType = OxyPlot.MarkerType.Circle;
                scatterPointSeries.MarkerType = scatterMarkerType;
            }

            // Deserialize scatter error series properties
            var scatterErrorSeries = series as OxyPlot.Wpf.ScatterErrorSeries;
            if (scatterErrorSeries != null)
            {
                var errorSeriesElement = element.Element(nameof(OxyPlot.Wpf.ScatterErrorSeries));
                if (GetStringAttribute(errorSeriesElement, nameof(scatterErrorSeries.DataFieldTag), out var errDataFieldTag)) scatterErrorSeries.DataFieldTag = errDataFieldTag;
                if (GetStringAttribute(errorSeriesElement, nameof(scatterErrorSeries.DataFieldValue), out var errDataFieldValue)) scatterErrorSeries.DataFieldValue = errDataFieldValue;
                if (GetStringAttribute(errorSeriesElement, nameof(scatterErrorSeries.ColorAxisKey), out var errColorAxisKey)) scatterErrorSeries.ColorAxisKey = errColorAxisKey;
                if (GetIntegerAttribute(errorSeriesElement, nameof(scatterErrorSeries.BinSize), out var errBinSize)) scatterErrorSeries.BinSize = errBinSize;
                if (GetColorAttribute(errorSeriesElement, nameof(scatterErrorSeries.MarkerFill), out var errMarkerFill)) scatterErrorSeries.MarkerFill = errMarkerFill;
                if (GetDoubleAttribute(errorSeriesElement, nameof(scatterErrorSeries.MarkerSize), out var errMarkerSize)) scatterErrorSeries.MarkerSize = errMarkerSize;
                if (GetColorAttribute(errorSeriesElement, nameof(scatterErrorSeries.MarkerStroke), out var errMarkerStroke)) scatterErrorSeries.MarkerStroke = errMarkerStroke;
                if (GetDoubleAttribute(errorSeriesElement, nameof(scatterErrorSeries.MarkerStrokeThickness), out var errMarkerStrokeThickness)) scatterErrorSeries.MarkerStrokeThickness = errMarkerStrokeThickness;
                if (!GetEnumAttribute(errorSeriesElement, nameof(scatterErrorSeries.MarkerType), out OxyPlot.MarkerType errMarkerType))
                    errMarkerType = OxyPlot.MarkerType.Circle;
                scatterErrorSeries.MarkerType = errMarkerType;
                if (GetDoubleAttribute(errorSeriesElement, nameof(scatterErrorSeries.ErrorBarStopWidth), out var errorBarStopWidth)) scatterErrorSeries.ErrorBarStopWidth = errorBarStopWidth;
                if (GetDoubleAttribute(errorSeriesElement, nameof(scatterErrorSeries.MinimumErrorSize), out var minimumErrorSize)) scatterErrorSeries.MinimumErrorSize = minimumErrorSize;
                if (GetDoubleAttribute(errorSeriesElement, nameof(scatterErrorSeries.ErrorBarStrokeThickness), out var errorBarStrokeThickness)) scatterErrorSeries.ErrorBarStrokeThickness = errorBarStrokeThickness;
                if (GetColorAttribute(errorSeriesElement, nameof(scatterErrorSeries.ErrorBarColor), out var errorBarColor)) scatterErrorSeries.ErrorBarColor = errorBarColor;
                if (GetStringAttribute(errorSeriesElement, nameof(scatterErrorSeries.DataFieldLowerErrorX), out var dataFieldLowerErrorX)) scatterErrorSeries.DataFieldLowerErrorX = dataFieldLowerErrorX;
                if (GetStringAttribute(errorSeriesElement, nameof(scatterErrorSeries.DataFieldUpperErrorX), out var dataFieldUpperErrorX)) scatterErrorSeries.DataFieldUpperErrorX = dataFieldUpperErrorX;
                if (GetStringAttribute(errorSeriesElement, nameof(scatterErrorSeries.DataFieldLowerErrorY), out var dataFieldLowerErrorY)) scatterErrorSeries.DataFieldLowerErrorY = dataFieldLowerErrorY;
                if (GetStringAttribute(errorSeriesElement, nameof(scatterErrorSeries.DataFieldUpperErrorY), out var dataFieldUpperErrorY)) scatterErrorSeries.DataFieldUpperErrorY = dataFieldUpperErrorY;
            }

            // Deserialize heat map series properties
            var heatMapSeries = series as OxyPlot.Wpf.HeatMapSeries;
            if (heatMapSeries != null)
            {
                var heatMapSeriesElement = element.Element(nameof(OxyPlot.Wpf.HeatMapSeries));
                if (GetStringAttribute(heatMapSeriesElement, nameof(heatMapSeries.ColorAxisKey), out var heatColorAxisKey)) heatMapSeries.ColorAxisKey = heatColorAxisKey;
                if (GetDoubleAttribute(heatMapSeriesElement, nameof(heatMapSeries.Y0), out var y0)) heatMapSeries.Y0 = y0;
                if (GetDoubleAttribute(heatMapSeriesElement, nameof(heatMapSeries.Y1), out var y1)) heatMapSeries.Y1 = y1;
                if (GetDoubleAttribute(heatMapSeriesElement, nameof(heatMapSeries.X0), out var x0)) heatMapSeries.X0 = x0;
                if (GetDoubleAttribute(heatMapSeriesElement, nameof(heatMapSeries.X1), out var x1)) heatMapSeries.X1 = x1;
                if (GetColorAttribute(heatMapSeriesElement, nameof(heatMapSeries.HighColor), out var highColor)) heatMapSeries.HighColor = highColor;
                if (GetColorAttribute(heatMapSeriesElement, nameof(heatMapSeries.LowColor), out var lowColor)) heatMapSeries.LowColor = lowColor;
                // TODO - Deserialize coordinateDefinition value
                if (GetBooleanAttribute(heatMapSeriesElement, nameof(heatMapSeries.Interpolate), out var interpolate)) heatMapSeries.Interpolate = interpolate;
                if (GetDoubleAttribute(heatMapSeriesElement, nameof(heatMapSeries.LabelFontSize), out var labelFontSize)) heatMapSeries.LabelFontSize = labelFontSize;
            }

            return series;
        }

        #endregion
    }

    #region Converters

    /// <summary>
    /// Converts series color to/from a SolidColorBrush, handling automatic colors.
    /// </summary>
    public class OxySeriesColorConverter : IMultiValueConverter
    {
        private OxyPlot.Series.Series _series;

        /// <summary>
        /// Converts a color and series to a SolidColorBrush.
        /// </summary>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Get Color
            if (values[0] == null) return null;
            if (values[0].GetType() != typeof(Color)) return null;
            var c = (Color)values[0];
            var oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B);

            // Get Series (this should only be set on convert with one-way binding)
            if (values[1] == null) { _series = null; return false; }
            _series = ((OxyPlot.Wpf.Series)values[1]).InternalSeries;

            // Convert
            if (oxyCol.IsAutomatic())
            {
                if (_series.GetType() == typeof(OxyPlot.Series.LineSeries))
                {
                    var actualColor = ((OxyPlot.Series.LineSeries)_series).ActualColor;
                    return new SolidColorBrush(Color.FromArgb(actualColor.A, actualColor.R, actualColor.G, actualColor.B));
                }
                else if (_series.GetType() == typeof(OxyPlot.Series.ContourSeries))
                {
                    var actualColor = ((OxyPlot.Series.ContourSeries)_series).ActualColor;
                    return new SolidColorBrush(Color.FromArgb(actualColor.A, actualColor.R, actualColor.G, actualColor.B));
                }
                else if (_series.GetType() == typeof(OxyPlot.Series.HighLowSeries))
                {
                    var actualColor = ((OxyPlot.Series.HighLowSeries)_series).ActualColor;
                    return new SolidColorBrush(Color.FromArgb(actualColor.A, actualColor.R, actualColor.G, actualColor.B));
                }
            }

            return new SolidColorBrush(c);
        }

        /// <summary>
        /// Converts a SolidColorBrush back to color and series.
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (_series == null) return new object[] { Color.FromArgb(255, 0, 0, 0), null };
            if (value.GetType() != typeof(SolidColorBrush)) return new object[] { Color.FromArgb(255, 0, 0, 0), null };
            var c = ((SolidColorBrush)value).Color;

            var oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B);
            if (_series.GetType() == typeof(OxyPlot.Series.LineSeries))
            {
                if (OxyColor.ColorDifference(oxyCol, ((OxyPlot.Series.LineSeries)_series).ActualColor) == 0)
                {
                    var actualColor = ((OxyPlot.Series.LineSeries)_series).ActualColor;
                    return new object[] { Color.FromArgb(actualColor.A, actualColor.R, actualColor.G, actualColor.B), _series };
                }
            }
            else if (_series.GetType() == typeof(OxyPlot.Series.ContourSeries))
            {
                if (OxyColor.ColorDifference(oxyCol, ((OxyPlot.Series.ContourSeries)_series).ActualColor) == 0)
                {
                    var actualColor = ((OxyPlot.Series.ContourSeries)_series).ActualColor;
                    return new object[] { Color.FromArgb(actualColor.A, actualColor.R, actualColor.G, actualColor.B), _series };
                }
            }
            else if (_series.GetType() == typeof(OxyPlot.Series.HighLowSeries))
            {
                if (OxyColor.ColorDifference(oxyCol, ((OxyPlot.Series.HighLowSeries)_series).ActualColor) == 0)
                {
                    var actualColor = ((OxyPlot.Series.HighLowSeries)_series).ActualColor;
                    return new object[] { Color.FromArgb(actualColor.A, actualColor.R, actualColor.G, actualColor.B), _series };
                }
            }

            return new object[] { c, _series };
        }
    }

    /// <summary>
    /// Converts series marker fill color to/from a SolidColorBrush, handling automatic colors.
    /// </summary>
    public class OxySeriesMarkerFillConverter : IMultiValueConverter
    {
        private OxyPlot.Series.Series _series;

        /// <summary>
        /// Converts a marker fill color and series to a SolidColorBrush.
        /// </summary>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Get Color
            if (values[0] == null) return null;
            if (values[0].GetType() != typeof(Color)) return null;
            var c = (Color)values[0];
            var oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B);

            // Get Series
            if (values[1] == null) { _series = null; return false; }
            _series = ((OxyPlot.Wpf.Series)values[1]).InternalSeries;

            // Convert
            if (oxyCol.IsAutomatic())
            {
                if (_series.GetType() == typeof(OxyPlot.Series.LineSeries))
                {
                    var actualColor = ((OxyPlot.Series.LineSeries)_series).ActualMarkerFill;
                    return new SolidColorBrush(Color.FromArgb(actualColor.A, actualColor.R, actualColor.G, actualColor.B));
                }
                else if (_series.GetType() == typeof(OxyPlot.Series.ScatterSeries))
                {
                    var actualColor = ((OxyPlot.Series.ScatterSeries)_series).ActualMarkerFillColor;
                    return new SolidColorBrush(Color.FromArgb(actualColor.A, actualColor.R, actualColor.G, actualColor.B));
                }
            }

            return new SolidColorBrush(c);
        }

        /// <summary>
        /// Converts a SolidColorBrush back to marker fill color and series.
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (_series == null) return new object[] { Color.FromArgb(255, 0, 0, 0), null };
            if (value.GetType() != typeof(SolidColorBrush)) return new object[] { Color.FromArgb(255, 0, 0, 0), null };
            var c = ((SolidColorBrush)value).Color;

            var oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B);
            if (_series.GetType() == typeof(OxyPlot.Series.LineSeries))
            {
                if (OxyColor.ColorDifference(oxyCol, ((OxyPlot.Series.LineSeries)_series).ActualMarkerFill) == 0)
                {
                    var actualColor = ((OxyPlot.Series.LineSeries)_series).ActualMarkerFill;
                    return new object[] { Color.FromArgb(actualColor.A, actualColor.R, actualColor.G, actualColor.B), _series };
                }
            }
            else if (_series.GetType() == typeof(OxyPlot.Series.ScatterSeries))
            {
                if (OxyColor.ColorDifference(oxyCol, ((OxyPlot.Series.ScatterSeries)_series).ActualMarkerFillColor) == 0)
                {
                    var actualColor = ((OxyPlot.Series.ScatterSeries)_series).ActualMarkerFillColor;
                    return new object[] { Color.FromArgb(actualColor.A, actualColor.R, actualColor.G, actualColor.B), _series };
                }
            }

            return new object[] { c, _series };
        }
    }

    /// <summary>
    /// Converts series marker stroke color to/from a SolidColorBrush, handling automatic colors.
    /// </summary>
    public class OxySeriesMarkerStrokeConverter : IMultiValueConverter
    {
        private OxyPlot.Series.Series _series;

        /// <summary>
        /// Converts a marker stroke color and series to a SolidColorBrush.
        /// </summary>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Get Color
            if (values[0] == null) return null;
            if (values[0].GetType() != typeof(Color)) return null;
            var c = (Color)values[0];
            var oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B);

            // Get Series
            if (values[1] == null) { _series = null; return false; }
            _series = ((OxyPlot.Wpf.Series)values[1]).InternalSeries;

            // Convert
            if (oxyCol.IsAutomatic())
            {
                if (_series.GetType() == typeof(OxyPlot.Series.LineSeries))
                {
                    var actualColor = ((OxyPlot.Series.LineSeries)_series).ActualMarkerFill;
                    return new SolidColorBrush(Color.FromArgb(actualColor.A, actualColor.R, actualColor.G, actualColor.B));
                }
                else if (_series.GetType() == typeof(OxyPlot.Series.ScatterSeries))
                {
                    var actualColor = ((OxyPlot.Series.ScatterSeries)_series).ActualMarkerFillColor;
                    return new SolidColorBrush(Color.FromArgb(actualColor.A, actualColor.R, actualColor.G, actualColor.B));
                }
            }

            return new SolidColorBrush(c);
        }

        /// <summary>
        /// Converts a SolidColorBrush back to marker stroke color and series.
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (_series == null) return new object[] { Color.FromArgb(255, 0, 0, 0), null };
            if (value.GetType() != typeof(SolidColorBrush)) return new object[] { Color.FromArgb(255, 0, 0, 0), null };
            var c = ((SolidColorBrush)value).Color;

            var oxyCol = OxyColor.FromArgb(c.A, c.R, c.G, c.B);
            if (_series.GetType() == typeof(OxyPlot.Series.LineSeries))
            {
                if (OxyColor.ColorDifference(oxyCol, ((OxyPlot.Series.LineSeries)_series).ActualMarkerFill) == 0)
                {
                    var actualColor = ((OxyPlot.Series.LineSeries)_series).ActualMarkerFill;
                    return new object[] { Color.FromArgb(actualColor.A, actualColor.R, actualColor.G, actualColor.B), _series };
                }
            }
            else if (_series.GetType() == typeof(OxyPlot.Series.ScatterSeries))
            {
                if (OxyColor.ColorDifference(oxyCol, ((OxyPlot.Series.ScatterSeries)_series).ActualMarkerFillColor) == 0)
                {
                    var actualColor = ((OxyPlot.Series.ScatterSeries)_series).ActualMarkerFillColor;
                    return new object[] { Color.FromArgb(actualColor.A, actualColor.R, actualColor.G, actualColor.B), _series };
                }
            }

            return new object[] { c, _series };
        }
    }

    #endregion
}
