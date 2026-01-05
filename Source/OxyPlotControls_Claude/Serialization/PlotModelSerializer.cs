using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Legends;

// Use type aliases to avoid namespace conflicts with OxyPlot.Wpf
using LineSeries = OxyPlot.Series.LineSeries;
using ScatterSeries = OxyPlot.Series.ScatterSeries;
using BarSeries = OxyPlot.Series.BarSeries;
using AreaSeries = OxyPlot.Series.AreaSeries;

// Use OxyPlot types instead of System.Windows types where there's ambiguity
using OxyHorizontalAlignment = OxyPlot.HorizontalAlignment;
using OxyVerticalAlignment = OxyPlot.VerticalAlignment;

namespace OxyPlotControls.Serialization;

/// <summary>
/// Provides versioned serialization and deserialization for <see cref="PlotModel"/>.
/// </summary>
/// <remarks>
/// Version 2.0 uses the new PlotModel-based architecture with proper culture-invariant serialization.
/// Version 1.0 compatibility provides best-effort loading of old VB-based Plot control settings.
/// </remarks>
public static class PlotModelSerializer
{
    private const string CurrentVersion = "2.0";
    private const string RootTag = "OxyPlotSettings";
    private const string GeneralPropertiesTag = "General";
    private const string LegendPropertiesTag = "Legend";
    private const string AxesPropertiesTag = "Axes";
    private const string AxisPropertiesTag = "Axis";
    private const string AnnotationsPropertiesTag = "Annotations";
    private const string AnnotationPropertiesTag = "Annotation";
    private const string SeriesPropertiesTag = "Series";
    private const string SeriesItemTag = "SeriesItem";

    #region Serialization

    /// <summary>
    /// Serializes a <see cref="PlotModel"/> to XML.
    /// </summary>
    /// <param name="model">The plot model to serialize.</param>
    /// <returns>XML element containing the serialized plot model.</returns>
    public static XElement Serialize(PlotModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var root = new XElement(RootTag,
            new XAttribute("Version", CurrentVersion));

        // Serialize general properties
        root.Add(SerializeGeneralProperties(model));

        // Serialize legend properties
        root.Add(SerializeLegendProperties(model));

        // Serialize axes
        root.Add(SerializeAxes(model));

        // Serialize annotations
        root.Add(SerializeAnnotations(model));

        // Serialize series
        root.Add(SerializeSeries(model));

        return root;
    }

    /// <summary>
    /// Serializes general plot properties to XML.
    /// </summary>
    private static XElement SerializeGeneralProperties(PlotModel model)
    {
        var general = new XElement(GeneralPropertiesTag);

        // Title properties
        var title = new XElement("Title");
        if (!string.IsNullOrEmpty(model.Title))
            title.SetAttributeValue(nameof(model.Title), model.Title);

        title.SetAttributeValue(nameof(model.TitleColor), SerializationHelpers.SerializeColor(model.TitleColor));

        if (!string.IsNullOrEmpty(model.TitleFont))
            title.SetAttributeValue(nameof(model.TitleFont), model.TitleFont);

        title.SetAttributeValue(nameof(model.TitleFontSize), SerializationHelpers.SerializeDouble(model.TitleFontSize));
        title.SetAttributeValue(nameof(model.TitleFontWeight), SerializationHelpers.SerializeDouble(model.TitleFontWeight));
        title.SetAttributeValue(nameof(model.TitlePadding), SerializationHelpers.SerializeDouble(model.TitlePadding));

        general.Add(title);

        // Subtitle properties
        var subtitle = new XElement("Subtitle");
        if (!string.IsNullOrEmpty(model.Subtitle))
            subtitle.SetAttributeValue(nameof(model.Subtitle), model.Subtitle);

        subtitle.SetAttributeValue(nameof(model.SubtitleColor), SerializationHelpers.SerializeColor(model.SubtitleColor));

        if (!string.IsNullOrEmpty(model.SubtitleFont))
            subtitle.SetAttributeValue(nameof(model.SubtitleFont), model.SubtitleFont);

        subtitle.SetAttributeValue(nameof(model.SubtitleFontSize), SerializationHelpers.SerializeDouble(model.SubtitleFontSize));
        subtitle.SetAttributeValue(nameof(model.SubtitleFontWeight), SerializationHelpers.SerializeDouble(model.SubtitleFontWeight));

        general.Add(subtitle);

        // Plot area properties
        var plotArea = new XElement("PlotArea");
        plotArea.SetAttributeValue(nameof(model.PlotAreaBackground), SerializationHelpers.SerializeColor(model.PlotAreaBackground));
        plotArea.SetAttributeValue(nameof(model.PlotAreaBorderColor), SerializationHelpers.SerializeColor(model.PlotAreaBorderColor));
        plotArea.SetAttributeValue(nameof(model.PlotAreaBorderThickness), SerializationHelpers.SerializeThickness(model.PlotAreaBorderThickness));

        general.Add(plotArea);

        // Background properties
        var background = new XElement("Background");
        background.SetAttributeValue(nameof(model.Background), SerializationHelpers.SerializeColor(model.Background));
        background.SetAttributeValue(nameof(model.Padding), SerializationHelpers.SerializeThickness(model.Padding));

        general.Add(background);

        return general;
    }

    /// <summary>
    /// Serializes legend properties to XML.
    /// In OxyPlot 2.x, legend properties are on a separate Legend object in model.Legends collection.
    /// </summary>
    private static XElement SerializeLegendProperties(PlotModel model)
    {
        var legendElement = new XElement(LegendPropertiesTag);

        // Get the first legend from the collection (OxyPlot 2.x architecture)
        var legend = model.Legends.FirstOrDefault() as Legend;

        // Visibility (IsLegendVisible is still on PlotModel)
        legendElement.SetAttributeValue("IsLegendVisible", SerializationHelpers.SerializeBoolean(model.IsLegendVisible));

        // If no legend exists, return just the visibility
        if (legend == null)
            return legendElement;

        // Position and orientation
        legendElement.SetAttributeValue("LegendPlacement", SerializationHelpers.SerializeEnum(legend.LegendPlacement));
        legendElement.SetAttributeValue("LegendPosition", SerializationHelpers.SerializeEnum(legend.LegendPosition));
        legendElement.SetAttributeValue("LegendOrientation", SerializationHelpers.SerializeEnum(legend.LegendOrientation));

        // Styling
        legendElement.SetAttributeValue("LegendBackground", SerializationHelpers.SerializeColor(legend.LegendBackground));
        legendElement.SetAttributeValue("LegendBorder", SerializationHelpers.SerializeColor(legend.LegendBorder));
        legendElement.SetAttributeValue("LegendBorderThickness", SerializationHelpers.SerializeDouble(legend.LegendBorderThickness));
        legendElement.SetAttributeValue("LegendPadding", SerializationHelpers.SerializeDouble(legend.LegendPadding));
        legendElement.SetAttributeValue("LegendMargin", SerializationHelpers.SerializeDouble(legend.LegendMargin));
        legendElement.SetAttributeValue("LegendItemSpacing", SerializationHelpers.SerializeDouble(legend.LegendItemSpacing));
        legendElement.SetAttributeValue("LegendLineSpacing", SerializationHelpers.SerializeDouble(legend.LegendLineSpacing));
        legendElement.SetAttributeValue("LegendColumnSpacing", SerializationHelpers.SerializeDouble(legend.LegendColumnSpacing));

        // Title
        if (!string.IsNullOrEmpty(legend.LegendTitle))
            legendElement.SetAttributeValue("LegendTitle", legend.LegendTitle);
        legendElement.SetAttributeValue("LegendTitleColor", SerializationHelpers.SerializeColor(legend.LegendTitleColor));
        if (!string.IsNullOrEmpty(legend.LegendTitleFont))
            legendElement.SetAttributeValue("LegendTitleFont", legend.LegendTitleFont);
        legendElement.SetAttributeValue("LegendTitleFontSize", SerializationHelpers.SerializeDouble(legend.LegendTitleFontSize));
        legendElement.SetAttributeValue("LegendTitleFontWeight", SerializationHelpers.SerializeDouble(legend.LegendTitleFontWeight));

        // Items (font weight is double in OxyPlot 2.x, not FontWeight)
        legendElement.SetAttributeValue("LegendTextColor", SerializationHelpers.SerializeColor(legend.LegendTextColor));
        if (!string.IsNullOrEmpty(legend.LegendFont))
            legendElement.SetAttributeValue("LegendFont", legend.LegendFont);
        legendElement.SetAttributeValue("LegendFontSize", SerializationHelpers.SerializeDouble(legend.LegendFontSize));
        legendElement.SetAttributeValue("LegendFontWeight", SerializationHelpers.SerializeDouble(legend.LegendFontWeight));
        legendElement.SetAttributeValue("LegendSymbolLength", SerializationHelpers.SerializeDouble(legend.LegendSymbolLength));
        legendElement.SetAttributeValue("LegendSymbolMargin", SerializationHelpers.SerializeDouble(legend.LegendSymbolMargin));
        legendElement.SetAttributeValue("LegendSymbolPlacement", SerializationHelpers.SerializeEnum(legend.LegendSymbolPlacement));
        legendElement.SetAttributeValue("LegendItemAlignment", SerializationHelpers.SerializeEnum(legend.LegendItemAlignment));
        legendElement.SetAttributeValue("LegendItemOrder", SerializationHelpers.SerializeEnum(legend.LegendItemOrder));
        legendElement.SetAttributeValue("LegendMaxWidth", SerializationHelpers.SerializeDouble(legend.LegendMaxWidth));
        legendElement.SetAttributeValue("LegendMaxHeight", SerializationHelpers.SerializeDouble(legend.LegendMaxHeight));

        return legendElement;
    }

    /// <summary>
    /// Serializes all axes to XML.
    /// </summary>
    private static XElement SerializeAxes(PlotModel model)
    {
        var axes = new XElement(AxesPropertiesTag);

        foreach (var axis in model.Axes)
        {
            axes.Add(SerializeAxis(axis));
        }

        return axes;
    }

    /// <summary>
    /// Serializes a single axis to XML.
    /// </summary>
    private static XElement SerializeAxis(Axis axis)
    {
        var axisElement = new XElement(AxisPropertiesTag);
        axisElement.SetAttributeValue("AxisType", axis.GetType().FullName);

        // General properties
        var general = new XElement("General");
        if (!string.IsNullOrEmpty(axis.Key))
            general.SetAttributeValue("Key", axis.Key);
        general.SetAttributeValue("IsAxisVisible", SerializationHelpers.SerializeBoolean(axis.IsAxisVisible));
        general.SetAttributeValue("Position", SerializationHelpers.SerializeEnum(axis.Position));
        general.SetAttributeValue("PositionTier", SerializationHelpers.SerializeInt(axis.PositionTier));
        general.SetAttributeValue("PositionAtZeroCrossing", SerializationHelpers.SerializeBoolean(axis.PositionAtZeroCrossing));
        general.SetAttributeValue("IsPanEnabled", SerializationHelpers.SerializeBoolean(axis.IsPanEnabled));
        general.SetAttributeValue("IsZoomEnabled", SerializationHelpers.SerializeBoolean(axis.IsZoomEnabled));
        general.SetAttributeValue("StartPosition", SerializationHelpers.SerializeDouble(axis.StartPosition));
        general.SetAttributeValue("EndPosition", SerializationHelpers.SerializeDouble(axis.EndPosition));
        general.SetAttributeValue("Layer", SerializationHelpers.SerializeEnum(axis.Layer));
        axisElement.Add(general);

        // Numeric properties
        var numbers = new XElement("Numbers");
        numbers.SetAttributeValue("Minimum", SerializationHelpers.SerializeDouble(axis.Minimum));
        numbers.SetAttributeValue("Maximum", SerializationHelpers.SerializeDouble(axis.Maximum));
        numbers.SetAttributeValue("AbsoluteMinimum", SerializationHelpers.SerializeDouble(axis.AbsoluteMinimum));
        numbers.SetAttributeValue("AbsoluteMaximum", SerializationHelpers.SerializeDouble(axis.AbsoluteMaximum));
        numbers.SetAttributeValue("FilterMinValue", SerializationHelpers.SerializeDouble(axis.FilterMinValue));
        numbers.SetAttributeValue("FilterMaxValue", SerializationHelpers.SerializeDouble(axis.FilterMaxValue));
        axisElement.Add(numbers);

        // Title properties
        var title = new XElement("Title");
        if (!string.IsNullOrEmpty(axis.Title))
            title.SetAttributeValue("Title", axis.Title);
        title.SetAttributeValue("TitleColor", SerializationHelpers.SerializeColor(axis.TitleColor));
        if (!string.IsNullOrEmpty(axis.TitleFont))
            title.SetAttributeValue("TitleFont", axis.TitleFont);
        title.SetAttributeValue("TitleFontSize", SerializationHelpers.SerializeDouble(axis.TitleFontSize));
        title.SetAttributeValue("TitleFontWeight", SerializationHelpers.SerializeDouble(axis.TitleFontWeight));
        title.SetAttributeValue("TitlePosition", SerializationHelpers.SerializeDouble(axis.TitlePosition));
        title.SetAttributeValue("AxisTitleDistance", SerializationHelpers.SerializeDouble(axis.AxisTitleDistance));
        if (!string.IsNullOrEmpty(axis.Unit))
            title.SetAttributeValue("Unit", axis.Unit);
        axisElement.Add(title);

        // Label properties
        var labels = new XElement("Labels");
        labels.SetAttributeValue("TextColor", SerializationHelpers.SerializeColor(axis.TextColor));
        if (!string.IsNullOrEmpty(axis.Font))
            labels.SetAttributeValue("Font", axis.Font);
        labels.SetAttributeValue("FontSize", SerializationHelpers.SerializeDouble(axis.FontSize));
        labels.SetAttributeValue("FontWeight", SerializationHelpers.SerializeDouble(axis.FontWeight));
        labels.SetAttributeValue("Angle", SerializationHelpers.SerializeDouble(axis.Angle));
        labels.SetAttributeValue("AxisTickToLabelDistance", SerializationHelpers.SerializeDouble(axis.AxisTickToLabelDistance));
        if (!string.IsNullOrEmpty(axis.StringFormat))
            labels.SetAttributeValue("StringFormat", axis.StringFormat);
        labels.SetAttributeValue("UseSuperExponentialFormat", SerializationHelpers.SerializeBoolean(axis.UseSuperExponentialFormat));
        axisElement.Add(labels);

        // Major gridline properties
        var majorGridlines = new XElement("MajorGridlines");
        majorGridlines.SetAttributeValue("MajorGridlineColor", SerializationHelpers.SerializeColor(axis.MajorGridlineColor));
        majorGridlines.SetAttributeValue("MajorGridlineStyle", SerializationHelpers.SerializeLineStyle(axis.MajorGridlineStyle));
        majorGridlines.SetAttributeValue("MajorGridlineThickness", SerializationHelpers.SerializeDouble(axis.MajorGridlineThickness));
        majorGridlines.SetAttributeValue("MajorStep", SerializationHelpers.SerializeDouble(axis.MajorStep));
        majorGridlines.SetAttributeValue("MajorTickSize", SerializationHelpers.SerializeDouble(axis.MajorTickSize));
        axisElement.Add(majorGridlines);

        // Minor gridline properties
        var minorGridlines = new XElement("MinorGridlines");
        minorGridlines.SetAttributeValue("MinorGridlineColor", SerializationHelpers.SerializeColor(axis.MinorGridlineColor));
        minorGridlines.SetAttributeValue("MinorGridlineStyle", SerializationHelpers.SerializeLineStyle(axis.MinorGridlineStyle));
        minorGridlines.SetAttributeValue("MinorGridlineThickness", SerializationHelpers.SerializeDouble(axis.MinorGridlineThickness));
        minorGridlines.SetAttributeValue("MinorStep", SerializationHelpers.SerializeDouble(axis.MinorStep));
        minorGridlines.SetAttributeValue("MinorTickSize", SerializationHelpers.SerializeDouble(axis.MinorTickSize));
        axisElement.Add(minorGridlines);

        // Tick properties
        var tick = new XElement("Tick");
        tick.SetAttributeValue("TickStyle", SerializationHelpers.SerializeEnum(axis.TickStyle));
        tick.SetAttributeValue("TicklineColor", SerializationHelpers.SerializeColor(axis.TicklineColor));
        axisElement.Add(tick);

        // Style properties
        var style = new XElement("Style");
        style.SetAttributeValue("AxislineColor", SerializationHelpers.SerializeColor(axis.AxislineColor));
        style.SetAttributeValue("AxislineStyle", SerializationHelpers.SerializeLineStyle(axis.AxislineStyle));
        style.SetAttributeValue("AxislineThickness", SerializationHelpers.SerializeDouble(axis.AxislineThickness));
        style.SetAttributeValue("AxisDistance", SerializationHelpers.SerializeDouble(axis.AxisDistance));
        axisElement.Add(style);

        // Type-specific properties
        // Note: Derived types must come before base types in pattern matching
        switch (axis)
        {
            // CategoryAxis, DateTimeAxis, TimeSpanAxis extend LinearAxis, so check them first
            case CategoryAxis categoryAxis:
                var categoryProps = new XElement("CategoryAxis");
                categoryProps.SetAttributeValue("IsTickCentered", SerializationHelpers.SerializeBoolean(categoryAxis.IsTickCentered));
                categoryProps.SetAttributeValue("GapWidth", SerializationHelpers.SerializeDouble(categoryAxis.GapWidth));
                // Serialize labels as child elements to properly handle special characters
                if (categoryAxis.Labels != null && categoryAxis.Labels.Count > 0)
                {
                    var labelsElement = new XElement("Labels");
                    foreach (var label in categoryAxis.Labels)
                    {
                        labelsElement.Add(new XElement("Label", label));
                    }
                    categoryProps.Add(labelsElement);
                }
                axisElement.Add(categoryProps);
                break;

            case DateTimeAxis dateTimeAxis:
                var dateTimeProps = new XElement("DateTimeAxis");
                dateTimeProps.SetAttributeValue("CalendarWeekRule", SerializationHelpers.SerializeEnum(dateTimeAxis.CalendarWeekRule));
                dateTimeProps.SetAttributeValue("FirstDayOfWeek", SerializationHelpers.SerializeEnum(dateTimeAxis.FirstDayOfWeek));
                dateTimeProps.SetAttributeValue("IntervalType", SerializationHelpers.SerializeEnum(dateTimeAxis.IntervalType));
                dateTimeProps.SetAttributeValue("MinorIntervalType", SerializationHelpers.SerializeEnum(dateTimeAxis.MinorIntervalType));
                axisElement.Add(dateTimeProps);
                break;

            case TimeSpanAxis timeSpanAxis:
                var timeSpanProps = new XElement("TimeSpanAxis");
                axisElement.Add(timeSpanProps);
                break;

            case LinearAxis linearAxis:
                var linearProps = new XElement("LinearAxis");
                linearProps.SetAttributeValue("FormatAsFractions", SerializationHelpers.SerializeBoolean(linearAxis.FormatAsFractions));
                linearProps.SetAttributeValue("FractionUnit", SerializationHelpers.SerializeDouble(linearAxis.FractionUnit));
                if (!string.IsNullOrEmpty(linearAxis.FractionUnitSymbol))
                    linearProps.SetAttributeValue("FractionUnitSymbol", linearAxis.FractionUnitSymbol);
                axisElement.Add(linearProps);
                break;

            case LogarithmicAxis logAxis:
                var logProps = new XElement("LogarithmicAxis");
                logProps.SetAttributeValue("Base", SerializationHelpers.SerializeDouble(logAxis.Base));
                logProps.SetAttributeValue("PowerPadding", SerializationHelpers.SerializeBoolean(logAxis.PowerPadding));
                axisElement.Add(logProps);
                break;
        }

        return axisElement;
    }

    /// <summary>
    /// Serializes all annotations to XML.
    /// </summary>
    private static XElement SerializeAnnotations(PlotModel model)
    {
        var annotations = new XElement(AnnotationsPropertiesTag);

        foreach (var annotation in model.Annotations)
        {
            annotations.Add(SerializeAnnotation(annotation));
        }

        return annotations;
    }

    /// <summary>
    /// Serializes a single annotation to XML.
    /// </summary>
    private static XElement SerializeAnnotation(Annotation annotation)
    {
        var annotationElement = new XElement(AnnotationPropertiesTag);
        annotationElement.SetAttributeValue("AnnotationType", annotation.GetType().FullName);

        // General properties
        var general = new XElement("General");
        general.SetAttributeValue("Layer", SerializationHelpers.SerializeEnum(annotation.Layer));
        if (!string.IsNullOrEmpty(annotation.XAxisKey))
            general.SetAttributeValue("XAxisKey", annotation.XAxisKey);
        if (!string.IsNullOrEmpty(annotation.YAxisKey))
            general.SetAttributeValue("YAxisKey", annotation.YAxisKey);
        annotationElement.Add(general);

        // Textual properties for TextualAnnotation
        if (annotation is TextualAnnotation textualAnnotation)
        {
            var textual = new XElement("Textual");
            if (!string.IsNullOrEmpty(textualAnnotation.Text))
                textual.SetAttributeValue("Text", textualAnnotation.Text);
            textual.SetAttributeValue("TextColor", SerializationHelpers.SerializeColor(textualAnnotation.TextColor));
            if (!string.IsNullOrEmpty(textualAnnotation.Font))
                textual.SetAttributeValue("Font", textualAnnotation.Font);
            textual.SetAttributeValue("FontSize", SerializationHelpers.SerializeDouble(textualAnnotation.FontSize));
            textual.SetAttributeValue("FontWeight", SerializationHelpers.SerializeDouble(textualAnnotation.FontWeight));
            textual.SetAttributeValue("TextPosition", SerializationHelpers.SerializeDataPoint(textualAnnotation.TextPosition));
            textual.SetAttributeValue("TextRotation", SerializationHelpers.SerializeDouble(textualAnnotation.TextRotation));
            textual.SetAttributeValue("TextHorizontalAlignment", SerializationHelpers.SerializeEnum(textualAnnotation.TextHorizontalAlignment));
            textual.SetAttributeValue("TextVerticalAlignment", SerializationHelpers.SerializeEnum(textualAnnotation.TextVerticalAlignment));
            annotationElement.Add(textual);
        }

        // Type-specific properties
        switch (annotation)
        {
            case ArrowAnnotation arrowAnnotation:
                var arrow = new XElement("Arrow");
                arrow.SetAttributeValue("Color", SerializationHelpers.SerializeColor(arrowAnnotation.Color));
                arrow.SetAttributeValue("StartPoint", SerializationHelpers.SerializeDataPoint(arrowAnnotation.StartPoint));
                arrow.SetAttributeValue("EndPoint", SerializationHelpers.SerializeDataPoint(arrowAnnotation.EndPoint));
                arrow.SetAttributeValue("ArrowDirection", SerializationHelpers.SerializeScreenVector(arrowAnnotation.ArrowDirection));
                arrow.SetAttributeValue("HeadLength", SerializationHelpers.SerializeDouble(arrowAnnotation.HeadLength));
                arrow.SetAttributeValue("HeadWidth", SerializationHelpers.SerializeDouble(arrowAnnotation.HeadWidth));
                arrow.SetAttributeValue("Veeness", SerializationHelpers.SerializeDouble(arrowAnnotation.Veeness));
                arrow.SetAttributeValue("LineStyle", SerializationHelpers.SerializeLineStyle(arrowAnnotation.LineStyle));
                arrow.SetAttributeValue("StrokeThickness", SerializationHelpers.SerializeDouble(arrowAnnotation.StrokeThickness));
                annotationElement.Add(arrow);
                break;

            case TextAnnotation textAnnotation:
                var text = new XElement("Text");
                text.SetAttributeValue("Background", SerializationHelpers.SerializeColor(textAnnotation.Background));
                text.SetAttributeValue("Stroke", SerializationHelpers.SerializeColor(textAnnotation.Stroke));
                text.SetAttributeValue("StrokeThickness", SerializationHelpers.SerializeDouble(textAnnotation.StrokeThickness));
                text.SetAttributeValue("Padding", SerializationHelpers.SerializeThickness(textAnnotation.Padding));
                text.SetAttributeValue("Offset", SerializationHelpers.SerializeScreenVector(textAnnotation.Offset));
                annotationElement.Add(text);
                break;

            case LineAnnotation lineAnnotation:
                var line = new XElement("Line");
                line.SetAttributeValue("Color", SerializationHelpers.SerializeColor(lineAnnotation.Color));
                line.SetAttributeValue("Type", SerializationHelpers.SerializeEnum(lineAnnotation.Type));
                line.SetAttributeValue("X", SerializationHelpers.SerializeDouble(lineAnnotation.X));
                line.SetAttributeValue("Y", SerializationHelpers.SerializeDouble(lineAnnotation.Y));
                line.SetAttributeValue("Slope", SerializationHelpers.SerializeDouble(lineAnnotation.Slope));
                line.SetAttributeValue("Intercept", SerializationHelpers.SerializeDouble(lineAnnotation.Intercept));
                line.SetAttributeValue("MinimumX", SerializationHelpers.SerializeDouble(lineAnnotation.MinimumX));
                line.SetAttributeValue("MaximumX", SerializationHelpers.SerializeDouble(lineAnnotation.MaximumX));
                line.SetAttributeValue("MinimumY", SerializationHelpers.SerializeDouble(lineAnnotation.MinimumY));
                line.SetAttributeValue("MaximumY", SerializationHelpers.SerializeDouble(lineAnnotation.MaximumY));
                line.SetAttributeValue("LineStyle", SerializationHelpers.SerializeLineStyle(lineAnnotation.LineStyle));
                line.SetAttributeValue("StrokeThickness", SerializationHelpers.SerializeDouble(lineAnnotation.StrokeThickness));
                annotationElement.Add(line);
                break;

            case RectangleAnnotation rectangleAnnotation:
                var rectangle = new XElement("Rectangle");
                rectangle.SetAttributeValue("Fill", SerializationHelpers.SerializeColor(rectangleAnnotation.Fill));
                rectangle.SetAttributeValue("Stroke", SerializationHelpers.SerializeColor(rectangleAnnotation.Stroke));
                rectangle.SetAttributeValue("StrokeThickness", SerializationHelpers.SerializeDouble(rectangleAnnotation.StrokeThickness));
                rectangle.SetAttributeValue("MinimumX", SerializationHelpers.SerializeDouble(rectangleAnnotation.MinimumX));
                rectangle.SetAttributeValue("MaximumX", SerializationHelpers.SerializeDouble(rectangleAnnotation.MaximumX));
                rectangle.SetAttributeValue("MinimumY", SerializationHelpers.SerializeDouble(rectangleAnnotation.MinimumY));
                rectangle.SetAttributeValue("MaximumY", SerializationHelpers.SerializeDouble(rectangleAnnotation.MaximumY));
                annotationElement.Add(rectangle);
                break;

            case EllipseAnnotation ellipseAnnotation:
                var ellipse = new XElement("Ellipse");
                ellipse.SetAttributeValue("Fill", SerializationHelpers.SerializeColor(ellipseAnnotation.Fill));
                ellipse.SetAttributeValue("Stroke", SerializationHelpers.SerializeColor(ellipseAnnotation.Stroke));
                ellipse.SetAttributeValue("StrokeThickness", SerializationHelpers.SerializeDouble(ellipseAnnotation.StrokeThickness));
                ellipse.SetAttributeValue("X", SerializationHelpers.SerializeDouble(ellipseAnnotation.X));
                ellipse.SetAttributeValue("Y", SerializationHelpers.SerializeDouble(ellipseAnnotation.Y));
                ellipse.SetAttributeValue("Width", SerializationHelpers.SerializeDouble(ellipseAnnotation.Width));
                ellipse.SetAttributeValue("Height", SerializationHelpers.SerializeDouble(ellipseAnnotation.Height));
                annotationElement.Add(ellipse);
                break;

            case PointAnnotation pointAnnotation:
                var point = new XElement("Point");
                point.SetAttributeValue("Fill", SerializationHelpers.SerializeColor(pointAnnotation.Fill));
                point.SetAttributeValue("Stroke", SerializationHelpers.SerializeColor(pointAnnotation.Stroke));
                point.SetAttributeValue("StrokeThickness", SerializationHelpers.SerializeDouble(pointAnnotation.StrokeThickness));
                point.SetAttributeValue("X", SerializationHelpers.SerializeDouble(pointAnnotation.X));
                point.SetAttributeValue("Y", SerializationHelpers.SerializeDouble(pointAnnotation.Y));
                point.SetAttributeValue("Size", SerializationHelpers.SerializeDouble(pointAnnotation.Size));
                point.SetAttributeValue("Shape", SerializationHelpers.SerializeEnum(pointAnnotation.Shape));
                point.SetAttributeValue("TextMargin", SerializationHelpers.SerializeDouble(pointAnnotation.TextMargin));
                annotationElement.Add(point);
                break;

            case PolygonAnnotation polygonAnnotation:
                var polygon = new XElement("Polygon");
                polygon.SetAttributeValue("Fill", SerializationHelpers.SerializeColor(polygonAnnotation.Fill));
                polygon.SetAttributeValue("Stroke", SerializationHelpers.SerializeColor(polygonAnnotation.Stroke));
                polygon.SetAttributeValue("StrokeThickness", SerializationHelpers.SerializeDouble(polygonAnnotation.StrokeThickness));
                polygon.SetAttributeValue("LineStyle", SerializationHelpers.SerializeLineStyle(polygonAnnotation.LineStyle));
                polygon.SetAttributeValue("LineJoin", SerializationHelpers.SerializeEnum(polygonAnnotation.LineJoin));
                var polygonPoints = new XElement("Points");
                foreach (var p in polygonAnnotation.Points)
                {
                    var pointEl = new XElement("Point");
                    pointEl.SetAttributeValue("X", SerializationHelpers.SerializeDouble(p.X));
                    pointEl.SetAttributeValue("Y", SerializationHelpers.SerializeDouble(p.Y));
                    polygonPoints.Add(pointEl);
                }
                polygon.Add(polygonPoints);
                annotationElement.Add(polygon);
                break;

            case PolylineAnnotation polylineAnnotation:
                var polyline = new XElement("Polyline");
                polyline.SetAttributeValue("Color", SerializationHelpers.SerializeColor(polylineAnnotation.Color));
                polyline.SetAttributeValue("StrokeThickness", SerializationHelpers.SerializeDouble(polylineAnnotation.StrokeThickness));
                polyline.SetAttributeValue("LineStyle", SerializationHelpers.SerializeLineStyle(polylineAnnotation.LineStyle));
                polyline.SetAttributeValue("LineJoin", SerializationHelpers.SerializeEnum(polylineAnnotation.LineJoin));
                polyline.SetAttributeValue("MinimumSegmentLength", SerializationHelpers.SerializeDouble(polylineAnnotation.MinimumSegmentLength));
                var polylinePoints = new XElement("Points");
                foreach (var p in polylineAnnotation.Points)
                {
                    var pointEl = new XElement("Point");
                    pointEl.SetAttributeValue("X", SerializationHelpers.SerializeDouble(p.X));
                    pointEl.SetAttributeValue("Y", SerializationHelpers.SerializeDouble(p.Y));
                    polylinePoints.Add(pointEl);
                }
                polyline.Add(polylinePoints);
                annotationElement.Add(polyline);
                break;
        }

        return annotationElement;
    }

    /// <summary>
    /// Serializes all series to XML.
    /// </summary>
    private static XElement SerializeSeries(PlotModel model)
    {
        var series = new XElement(SeriesPropertiesTag);

        foreach (var s in model.Series)
        {
            series.Add(SerializeSeriesItem(s));
        }

        return series;
    }

    /// <summary>
    /// Serializes a single series to XML.
    /// </summary>
    private static XElement SerializeSeriesItem(OxyPlot.Series.Series seriesItem)
    {
        var seriesElement = new XElement(SeriesItemTag);
        seriesElement.SetAttributeValue("SeriesType", seriesItem.GetType().FullName);

        // General properties
        var general = new XElement("General");
        if (!string.IsNullOrEmpty(seriesItem.Title))
            general.SetAttributeValue("Title", seriesItem.Title);
        general.SetAttributeValue("IsVisible", SerializationHelpers.SerializeBoolean(seriesItem.IsVisible));
        general.SetAttributeValue("Background", SerializationHelpers.SerializeColor(seriesItem.Background));
        if (!string.IsNullOrEmpty(seriesItem.TrackerKey))
            general.SetAttributeValue("TrackerKey", seriesItem.TrackerKey);
        seriesElement.Add(general);

        // Type-specific properties
        // Note: Derived types must come before base types in pattern matching
        switch (seriesItem)
        {
            // AreaSeries extends LineSeries, so check it first
            case AreaSeries areaSeries:
                var areaProps = new XElement("AreaSeries");
                areaProps.SetAttributeValue("Color", SerializationHelpers.SerializeColor(areaSeries.Color));
                areaProps.SetAttributeValue("Color2", SerializationHelpers.SerializeColor(areaSeries.Color2));
                areaProps.SetAttributeValue("Fill", SerializationHelpers.SerializeColor(areaSeries.Fill));
                areaProps.SetAttributeValue("StrokeThickness", SerializationHelpers.SerializeDouble(areaSeries.StrokeThickness));
                if (!string.IsNullOrEmpty(areaSeries.XAxisKey))
                    areaProps.SetAttributeValue("XAxisKey", areaSeries.XAxisKey);
                if (!string.IsNullOrEmpty(areaSeries.YAxisKey))
                    areaProps.SetAttributeValue("YAxisKey", areaSeries.YAxisKey);
                seriesElement.Add(areaProps);
                break;

            case LineSeries lineSeries:
                var lineProps = new XElement("LineSeries");
                lineProps.SetAttributeValue("Color", SerializationHelpers.SerializeColor(lineSeries.Color));
                lineProps.SetAttributeValue("StrokeThickness", SerializationHelpers.SerializeDouble(lineSeries.StrokeThickness));
                lineProps.SetAttributeValue("LineStyle", SerializationHelpers.SerializeLineStyle(lineSeries.LineStyle));
                lineProps.SetAttributeValue("LineJoin", SerializationHelpers.SerializeEnum(lineSeries.LineJoin));
                lineProps.SetAttributeValue("MarkerType", SerializationHelpers.SerializeEnum(lineSeries.MarkerType));
                lineProps.SetAttributeValue("MarkerSize", SerializationHelpers.SerializeDouble(lineSeries.MarkerSize));
                lineProps.SetAttributeValue("MarkerStroke", SerializationHelpers.SerializeColor(lineSeries.MarkerStroke));
                lineProps.SetAttributeValue("MarkerStrokeThickness", SerializationHelpers.SerializeDouble(lineSeries.MarkerStrokeThickness));
                lineProps.SetAttributeValue("MarkerFill", SerializationHelpers.SerializeColor(lineSeries.MarkerFill));
                // In OxyPlot 2.x, Smooth was replaced with InterpolationAlgorithm
                lineProps.SetAttributeValue("Smooth", SerializationHelpers.SerializeBoolean(lineSeries.InterpolationAlgorithm != null));
                if (!string.IsNullOrEmpty(lineSeries.XAxisKey))
                    lineProps.SetAttributeValue("XAxisKey", lineSeries.XAxisKey);
                if (!string.IsNullOrEmpty(lineSeries.YAxisKey))
                    lineProps.SetAttributeValue("YAxisKey", lineSeries.YAxisKey);
                seriesElement.Add(lineProps);
                break;

            case ScatterSeries scatterSeries:
                var scatterProps = new XElement("ScatterSeries");
                scatterProps.SetAttributeValue("MarkerType", SerializationHelpers.SerializeEnum(scatterSeries.MarkerType));
                scatterProps.SetAttributeValue("MarkerSize", SerializationHelpers.SerializeDouble(scatterSeries.MarkerSize));
                scatterProps.SetAttributeValue("MarkerStroke", SerializationHelpers.SerializeColor(scatterSeries.MarkerStroke));
                scatterProps.SetAttributeValue("MarkerStrokeThickness", SerializationHelpers.SerializeDouble(scatterSeries.MarkerStrokeThickness));
                scatterProps.SetAttributeValue("MarkerFill", SerializationHelpers.SerializeColor(scatterSeries.MarkerFill));
                if (!string.IsNullOrEmpty(scatterSeries.XAxisKey))
                    scatterProps.SetAttributeValue("XAxisKey", scatterSeries.XAxisKey);
                if (!string.IsNullOrEmpty(scatterSeries.YAxisKey))
                    scatterProps.SetAttributeValue("YAxisKey", scatterSeries.YAxisKey);
                seriesElement.Add(scatterProps);
                break;

            case BarSeries barSeries:
                var barProps = new XElement("BarSeries");
                barProps.SetAttributeValue("FillColor", SerializationHelpers.SerializeColor(barSeries.FillColor));
                barProps.SetAttributeValue("StrokeColor", SerializationHelpers.SerializeColor(barSeries.StrokeColor));
                barProps.SetAttributeValue("StrokeThickness", SerializationHelpers.SerializeDouble(barSeries.StrokeThickness));
                barProps.SetAttributeValue("BarWidth", SerializationHelpers.SerializeDouble(barSeries.BarWidth));
                if (!string.IsNullOrEmpty(barSeries.XAxisKey))
                    barProps.SetAttributeValue("XAxisKey", barSeries.XAxisKey);
                if (!string.IsNullOrEmpty(barSeries.YAxisKey))
                    barProps.SetAttributeValue("YAxisKey", barSeries.YAxisKey);
                seriesElement.Add(barProps);
                break;
        }

        return seriesElement;
    }

    #endregion

    #region Deserialization

    /// <summary>
    /// Deserializes a <see cref="PlotModel"/> from XML with version detection.
    /// </summary>
    /// <param name="element">The XML element containing the plot model.</param>
    /// <returns>The deserialized plot model, or null if deserialization fails.</returns>
    public static PlotModel? Deserialize(XElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        try
        {
            var version = element.Attribute("Version")?.Value ?? "1.0";

            return version switch
            {
                "2.0" => DeserializeV2(element),
                "1.0" => DeserializeV1(element),
                _ => throw new NotSupportedException($"Version {version} is not supported")
            };
        }
        catch (Exception ex)
        {
            // Log error and return null
            System.Diagnostics.Debug.WriteLine($"Error deserializing PlotModel: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Deserializes version 2.0 (current format).
    /// </summary>
    private static PlotModel DeserializeV2(XElement root)
    {
        var model = new PlotModel();

        var general = root.Element(GeneralPropertiesTag);
        if (general is not null)
        {
            DeserializeGeneralPropertiesV2(model, general);
        }

        var legend = root.Element(LegendPropertiesTag);
        if (legend is not null)
        {
            DeserializeLegendPropertiesV2(model, legend);
        }

        var axes = root.Element(AxesPropertiesTag);
        if (axes is not null)
        {
            DeserializeAxesV2(model, axes);
        }

        var annotations = root.Element(AnnotationsPropertiesTag);
        if (annotations is not null)
        {
            DeserializeAnnotationsV2(model, annotations);
        }

        var series = root.Element(SeriesPropertiesTag);
        if (series is not null)
        {
            DeserializeSeriesV2(model, series);
        }

        return model;
    }

    /// <summary>
    /// Deserializes general properties for version 2.0.
    /// </summary>
    private static void DeserializeGeneralPropertiesV2(PlotModel model, XElement general)
    {
        // Title properties
        var title = general.Element("Title");
        if (title is not null)
        {
            model.Title = title.Attribute(nameof(model.Title))?.Value ?? string.Empty;
            model.TitleColor = SerializationHelpers.DeserializeColor(title.Attribute(nameof(model.TitleColor))?.Value ?? "Automatic");
            model.TitleFont = title.Attribute(nameof(model.TitleFont))?.Value;
            model.TitleFontSize = SerializationHelpers.DeserializeDouble(title.Attribute(nameof(model.TitleFontSize))?.Value ?? "18");
            model.TitleFontWeight = SerializationHelpers.DeserializeOxyFontWeight(title.Attribute(nameof(model.TitleFontWeight))?.Value ?? "Bold", OxyPlot.FontWeights.Bold);
            model.TitlePadding = SerializationHelpers.DeserializeDouble(title.Attribute(nameof(model.TitlePadding))?.Value ?? "6");
        }

        // Subtitle properties
        var subtitle = general.Element("Subtitle");
        if (subtitle is not null)
        {
            model.Subtitle = subtitle.Attribute(nameof(model.Subtitle))?.Value ?? string.Empty;
            model.SubtitleColor = SerializationHelpers.DeserializeColor(subtitle.Attribute(nameof(model.SubtitleColor))?.Value ?? "Automatic");
            model.SubtitleFont = subtitle.Attribute(nameof(model.SubtitleFont))?.Value;
            model.SubtitleFontSize = SerializationHelpers.DeserializeDouble(subtitle.Attribute(nameof(model.SubtitleFontSize))?.Value ?? "14");
            model.SubtitleFontWeight = SerializationHelpers.DeserializeOxyFontWeight(subtitle.Attribute(nameof(model.SubtitleFontWeight))?.Value ?? "Normal");
        }

        // Plot area properties
        var plotArea = general.Element("PlotArea");
        if (plotArea is not null)
        {
            model.PlotAreaBackground = SerializationHelpers.DeserializeColor(plotArea.Attribute(nameof(model.PlotAreaBackground))?.Value ?? "Undefined");
            model.PlotAreaBorderColor = SerializationHelpers.DeserializeColor(plotArea.Attribute(nameof(model.PlotAreaBorderColor))?.Value ?? "Black");
            model.PlotAreaBorderThickness = SerializationHelpers.DeserializeThickness(plotArea.Attribute(nameof(model.PlotAreaBorderThickness))?.Value ?? "1");
        }

        // Background properties
        var background = general.Element("Background");
        if (background is not null)
        {
            model.Background = SerializationHelpers.DeserializeColor(background.Attribute(nameof(model.Background))?.Value ?? "Undefined");
            model.Padding = SerializationHelpers.DeserializeThickness(background.Attribute(nameof(model.Padding))?.Value ?? "8");
        }
    }

    /// <summary>
    /// Deserializes legend properties for version 2.0.
    /// In OxyPlot 2.x, legend properties are on a separate Legend object in model.Legends collection.
    /// </summary>
    private static void DeserializeLegendPropertiesV2(PlotModel model, XElement legendElement)
    {
        // IsLegendVisible is still on PlotModel
        model.IsLegendVisible = SerializationHelpers.DeserializeBoolean(legendElement.Attribute("IsLegendVisible")?.Value ?? "True");

        // Create a new Legend object for OxyPlot 2.x
        var legend = new Legend();

        // Position and orientation
        legend.LegendPlacement = SerializationHelpers.DeserializeEnum(legendElement.Attribute("LegendPlacement")?.Value ?? "Inside", LegendPlacement.Inside);
        legend.LegendPosition = SerializationHelpers.DeserializeEnum(legendElement.Attribute("LegendPosition")?.Value ?? "RightTop", LegendPosition.RightTop);
        legend.LegendOrientation = SerializationHelpers.DeserializeEnum(legendElement.Attribute("LegendOrientation")?.Value ?? "Vertical", LegendOrientation.Vertical);

        // Styling
        legend.LegendBackground = SerializationHelpers.DeserializeColor(legendElement.Attribute("LegendBackground")?.Value ?? "Undefined");
        legend.LegendBorder = SerializationHelpers.DeserializeColor(legendElement.Attribute("LegendBorder")?.Value ?? "Undefined");
        legend.LegendBorderThickness = SerializationHelpers.DeserializeDouble(legendElement.Attribute("LegendBorderThickness")?.Value ?? "1");
        legend.LegendPadding = SerializationHelpers.DeserializeDouble(legendElement.Attribute("LegendPadding")?.Value ?? "8");
        legend.LegendMargin = SerializationHelpers.DeserializeDouble(legendElement.Attribute("LegendMargin")?.Value ?? "8");
        legend.LegendItemSpacing = SerializationHelpers.DeserializeDouble(legendElement.Attribute("LegendItemSpacing")?.Value ?? "24");
        legend.LegendLineSpacing = SerializationHelpers.DeserializeDouble(legendElement.Attribute("LegendLineSpacing")?.Value ?? "0");
        legend.LegendColumnSpacing = SerializationHelpers.DeserializeDouble(legendElement.Attribute("LegendColumnSpacing")?.Value ?? "8");

        // Title
        legend.LegendTitle = legendElement.Attribute("LegendTitle")?.Value;
        legend.LegendTitleColor = SerializationHelpers.DeserializeColor(legendElement.Attribute("LegendTitleColor")?.Value ?? "Automatic");
        legend.LegendTitleFont = legendElement.Attribute("LegendTitleFont")?.Value;
        legend.LegendTitleFontSize = SerializationHelpers.DeserializeDouble(legendElement.Attribute("LegendTitleFontSize")?.Value ?? "12");
        legend.LegendTitleFontWeight = SerializationHelpers.DeserializeDouble(legendElement.Attribute("LegendTitleFontWeight")?.Value ?? "700"); // Bold = 700

        // Items (font weight is double in OxyPlot 2.x)
        legend.LegendTextColor = SerializationHelpers.DeserializeColor(legendElement.Attribute("LegendTextColor")?.Value ?? "Automatic");
        legend.LegendFont = legendElement.Attribute("LegendFont")?.Value;
        legend.LegendFontSize = SerializationHelpers.DeserializeDouble(legendElement.Attribute("LegendFontSize")?.Value ?? "12");
        legend.LegendFontWeight = SerializationHelpers.DeserializeDouble(legendElement.Attribute("LegendFontWeight")?.Value ?? "400"); // Normal = 400
        legend.LegendSymbolLength = SerializationHelpers.DeserializeDouble(legendElement.Attribute("LegendSymbolLength")?.Value ?? "16");
        legend.LegendSymbolMargin = SerializationHelpers.DeserializeDouble(legendElement.Attribute("LegendSymbolMargin")?.Value ?? "4");
        legend.LegendSymbolPlacement = SerializationHelpers.DeserializeEnum(legendElement.Attribute("LegendSymbolPlacement")?.Value ?? "Left", LegendSymbolPlacement.Left);
        legend.LegendItemAlignment = SerializationHelpers.DeserializeEnum(legendElement.Attribute("LegendItemAlignment")?.Value ?? "Left", OxyHorizontalAlignment.Left);
        legend.LegendItemOrder = SerializationHelpers.DeserializeEnum(legendElement.Attribute("LegendItemOrder")?.Value ?? "Normal", LegendItemOrder.Normal);
        legend.LegendMaxWidth = SerializationHelpers.DeserializeDouble(legendElement.Attribute("LegendMaxWidth")?.Value ?? "NaN");
        legend.LegendMaxHeight = SerializationHelpers.DeserializeDouble(legendElement.Attribute("LegendMaxHeight")?.Value ?? "NaN");

        // Add the legend to the model's Legends collection
        model.Legends.Add(legend);
    }

    /// <summary>
    /// Deserializes all axes for version 2.0.
    /// </summary>
    private static void DeserializeAxesV2(PlotModel model, XElement axesElement)
    {
        foreach (var axisElement in axesElement.Elements(AxisPropertiesTag))
        {
            var axis = DeserializeAxisV2(axisElement);
            if (axis is not null)
            {
                model.Axes.Add(axis);
            }
        }
    }

    /// <summary>
    /// Deserializes a single axis for version 2.0.
    /// </summary>
    private static Axis? DeserializeAxisV2(XElement axisElement)
    {
        var axisType = axisElement.Attribute("AxisType")?.Value;
        if (string.IsNullOrEmpty(axisType))
            return null;

        // Create the appropriate axis type
        Axis axis = axisType switch
        {
            var t when t.Contains("LinearAxis") => new LinearAxis(),
            var t when t.Contains("LogarithmicAxis") => new LogarithmicAxis(),
            var t when t.Contains("CategoryAxis") => new CategoryAxis(),
            var t when t.Contains("DateTimeAxis") => new DateTimeAxis(),
            var t when t.Contains("TimeSpanAxis") => new TimeSpanAxis(),
            _ => new LinearAxis()
        };

        // General properties
        var general = axisElement.Element("General");
        if (general is not null)
        {
            axis.Key = general.Attribute("Key")?.Value;
            axis.IsAxisVisible = SerializationHelpers.DeserializeBoolean(general.Attribute("IsAxisVisible")?.Value ?? "True");
            axis.Position = SerializationHelpers.DeserializeEnum(general.Attribute("Position")?.Value ?? "Bottom", AxisPosition.Bottom);
            axis.PositionTier = SerializationHelpers.DeserializeInt(general.Attribute("PositionTier")?.Value ?? "0");
            axis.PositionAtZeroCrossing = SerializationHelpers.DeserializeBoolean(general.Attribute("PositionAtZeroCrossing")?.Value ?? "False");
            axis.IsPanEnabled = SerializationHelpers.DeserializeBoolean(general.Attribute("IsPanEnabled")?.Value ?? "True");
            axis.IsZoomEnabled = SerializationHelpers.DeserializeBoolean(general.Attribute("IsZoomEnabled")?.Value ?? "True");
            axis.StartPosition = SerializationHelpers.DeserializeDouble(general.Attribute("StartPosition")?.Value ?? "0");
            axis.EndPosition = SerializationHelpers.DeserializeDouble(general.Attribute("EndPosition")?.Value ?? "1");
            axis.Layer = SerializationHelpers.DeserializeEnum(general.Attribute("Layer")?.Value ?? "BelowSeries", AxisLayer.BelowSeries);
        }

        // Numeric properties
        var numbers = axisElement.Element("Numbers");
        if (numbers is not null)
        {
            axis.Minimum = SerializationHelpers.DeserializeDouble(numbers.Attribute("Minimum")?.Value ?? "NaN");
            axis.Maximum = SerializationHelpers.DeserializeDouble(numbers.Attribute("Maximum")?.Value ?? "NaN");
            axis.AbsoluteMinimum = SerializationHelpers.DeserializeDouble(numbers.Attribute("AbsoluteMinimum")?.Value ?? "-Infinity");
            axis.AbsoluteMaximum = SerializationHelpers.DeserializeDouble(numbers.Attribute("AbsoluteMaximum")?.Value ?? "Infinity");
            axis.FilterMinValue = SerializationHelpers.DeserializeDouble(numbers.Attribute("FilterMinValue")?.Value ?? "-Infinity");
            axis.FilterMaxValue = SerializationHelpers.DeserializeDouble(numbers.Attribute("FilterMaxValue")?.Value ?? "Infinity");
        }

        // Title properties
        var title = axisElement.Element("Title");
        if (title is not null)
        {
            axis.Title = title.Attribute("Title")?.Value;
            axis.TitleColor = SerializationHelpers.DeserializeColor(title.Attribute("TitleColor")?.Value ?? "Automatic");
            axis.TitleFont = title.Attribute("TitleFont")?.Value;
            axis.TitleFontSize = SerializationHelpers.DeserializeDouble(title.Attribute("TitleFontSize")?.Value ?? "12");
            axis.TitleFontWeight = SerializationHelpers.DeserializeOxyFontWeight(title.Attribute("TitleFontWeight")?.Value ?? "Normal");
            axis.TitlePosition = SerializationHelpers.DeserializeDouble(title.Attribute("TitlePosition")?.Value ?? "0.5");
            axis.AxisTitleDistance = SerializationHelpers.DeserializeDouble(title.Attribute("AxisTitleDistance")?.Value ?? "4");
            axis.Unit = title.Attribute("Unit")?.Value;
        }

        // Label properties
        var labels = axisElement.Element("Labels");
        if (labels is not null)
        {
            axis.TextColor = SerializationHelpers.DeserializeColor(labels.Attribute("TextColor")?.Value ?? "Automatic");
            axis.Font = labels.Attribute("Font")?.Value;
            axis.FontSize = SerializationHelpers.DeserializeDouble(labels.Attribute("FontSize")?.Value ?? "12");
            axis.FontWeight = SerializationHelpers.DeserializeOxyFontWeight(labels.Attribute("FontWeight")?.Value ?? "Normal");
            axis.Angle = SerializationHelpers.DeserializeDouble(labels.Attribute("Angle")?.Value ?? "0");
            axis.AxisTickToLabelDistance = SerializationHelpers.DeserializeDouble(labels.Attribute("AxisTickToLabelDistance")?.Value ?? "4");
            axis.StringFormat = labels.Attribute("StringFormat")?.Value;
            axis.UseSuperExponentialFormat = SerializationHelpers.DeserializeBoolean(labels.Attribute("UseSuperExponentialFormat")?.Value ?? "False");
        }

        // Major gridline properties
        var majorGridlines = axisElement.Element("MajorGridlines");
        if (majorGridlines is not null)
        {
            axis.MajorGridlineColor = SerializationHelpers.DeserializeColor(majorGridlines.Attribute("MajorGridlineColor")?.Value ?? "LightGray");
            axis.MajorGridlineStyle = SerializationHelpers.DeserializeLineStyle(majorGridlines.Attribute("MajorGridlineStyle")?.Value ?? "None");
            axis.MajorGridlineThickness = SerializationHelpers.DeserializeDouble(majorGridlines.Attribute("MajorGridlineThickness")?.Value ?? "1");
            axis.MajorStep = SerializationHelpers.DeserializeDouble(majorGridlines.Attribute("MajorStep")?.Value ?? "NaN");
            axis.MajorTickSize = SerializationHelpers.DeserializeDouble(majorGridlines.Attribute("MajorTickSize")?.Value ?? "7");
        }

        // Minor gridline properties
        var minorGridlines = axisElement.Element("MinorGridlines");
        if (minorGridlines is not null)
        {
            axis.MinorGridlineColor = SerializationHelpers.DeserializeColor(minorGridlines.Attribute("MinorGridlineColor")?.Value ?? "LightGray");
            axis.MinorGridlineStyle = SerializationHelpers.DeserializeLineStyle(minorGridlines.Attribute("MinorGridlineStyle")?.Value ?? "None");
            axis.MinorGridlineThickness = SerializationHelpers.DeserializeDouble(minorGridlines.Attribute("MinorGridlineThickness")?.Value ?? "1");
            axis.MinorStep = SerializationHelpers.DeserializeDouble(minorGridlines.Attribute("MinorStep")?.Value ?? "NaN");
            axis.MinorTickSize = SerializationHelpers.DeserializeDouble(minorGridlines.Attribute("MinorTickSize")?.Value ?? "4");
        }

        // Tick properties
        var tick = axisElement.Element("Tick");
        if (tick is not null)
        {
            axis.TickStyle = SerializationHelpers.DeserializeEnum(tick.Attribute("TickStyle")?.Value ?? "Outside", TickStyle.Outside);
            axis.TicklineColor = SerializationHelpers.DeserializeColor(tick.Attribute("TicklineColor")?.Value ?? "Automatic");
        }

        // Style properties
        var style = axisElement.Element("Style");
        if (style is not null)
        {
            axis.AxislineColor = SerializationHelpers.DeserializeColor(style.Attribute("AxislineColor")?.Value ?? "Black");
            axis.AxislineStyle = SerializationHelpers.DeserializeLineStyle(style.Attribute("AxislineStyle")?.Value ?? "None");
            axis.AxislineThickness = SerializationHelpers.DeserializeDouble(style.Attribute("AxislineThickness")?.Value ?? "1");
            axis.AxisDistance = SerializationHelpers.DeserializeDouble(style.Attribute("AxisDistance")?.Value ?? "0");
        }

        // Type-specific properties
        // Note: Derived types must come before base types in pattern matching
        switch (axis)
        {
            // CategoryAxis, DateTimeAxis, TimeSpanAxis extend LinearAxis, so check them first
            case CategoryAxis categoryAxis:
                var categoryProps = axisElement.Element("CategoryAxis");
                if (categoryProps is not null)
                {
                    categoryAxis.IsTickCentered = SerializationHelpers.DeserializeBoolean(categoryProps.Attribute("IsTickCentered")?.Value ?? "True");
                    categoryAxis.GapWidth = SerializationHelpers.DeserializeDouble(categoryProps.Attribute("GapWidth")?.Value ?? "1");

                    // Try new format (child elements) first, then fall back to old format (pipe-separated attribute)
                    var labelsElement = categoryProps.Element("Labels");
                    if (labelsElement is not null)
                    {
                        categoryAxis.Labels.Clear();
                        foreach (var labelElement in labelsElement.Elements("Label"))
                        {
                            categoryAxis.Labels.Add(labelElement.Value);
                        }
                    }
                    else
                    {
                        // Backward compatibility: try pipe-separated attribute
                        var labelsStr = categoryProps.Attribute("Labels")?.Value;
                        if (!string.IsNullOrEmpty(labelsStr))
                        {
                            categoryAxis.Labels.Clear();
                            foreach (var label in labelsStr.Split('|'))
                            {
                                categoryAxis.Labels.Add(label);
                            }
                        }
                    }
                }
                break;

            case DateTimeAxis dateTimeAxis:
                var dateTimeProps = axisElement.Element("DateTimeAxis");
                if (dateTimeProps is not null)
                {
                    dateTimeAxis.CalendarWeekRule = SerializationHelpers.DeserializeEnum(dateTimeProps.Attribute("CalendarWeekRule")?.Value ?? "FirstFourDayWeek", System.Globalization.CalendarWeekRule.FirstFourDayWeek);
                    dateTimeAxis.FirstDayOfWeek = SerializationHelpers.DeserializeEnum(dateTimeProps.Attribute("FirstDayOfWeek")?.Value ?? "Monday", DayOfWeek.Monday);
                    dateTimeAxis.IntervalType = SerializationHelpers.DeserializeEnum(dateTimeProps.Attribute("IntervalType")?.Value ?? "Auto", DateTimeIntervalType.Auto);
                    dateTimeAxis.MinorIntervalType = SerializationHelpers.DeserializeEnum(dateTimeProps.Attribute("MinorIntervalType")?.Value ?? "Auto", DateTimeIntervalType.Auto);
                }
                break;

            case TimeSpanAxis timeSpanAxis:
                // TimeSpanAxis doesn't have additional properties in our serialization
                break;

            case LinearAxis linearAxis:
                var linearProps = axisElement.Element("LinearAxis");
                if (linearProps is not null)
                {
                    linearAxis.FormatAsFractions = SerializationHelpers.DeserializeBoolean(linearProps.Attribute("FormatAsFractions")?.Value ?? "False");
                    linearAxis.FractionUnit = SerializationHelpers.DeserializeDouble(linearProps.Attribute("FractionUnit")?.Value ?? "1");
                    linearAxis.FractionUnitSymbol = linearProps.Attribute("FractionUnitSymbol")?.Value;
                }
                break;

            case LogarithmicAxis logAxis:
                var logProps = axisElement.Element("LogarithmicAxis");
                if (logProps is not null)
                {
                    logAxis.Base = SerializationHelpers.DeserializeDouble(logProps.Attribute("Base")?.Value ?? "10");
                    logAxis.PowerPadding = SerializationHelpers.DeserializeBoolean(logProps.Attribute("PowerPadding")?.Value ?? "True");
                }
                break;
        }

        return axis;
    }

    /// <summary>
    /// Deserializes all annotations for version 2.0.
    /// </summary>
    private static void DeserializeAnnotationsV2(PlotModel model, XElement annotationsElement)
    {
        foreach (var annotationElement in annotationsElement.Elements(AnnotationPropertiesTag))
        {
            var annotation = DeserializeAnnotationV2(annotationElement);
            if (annotation is not null)
            {
                model.Annotations.Add(annotation);
            }
        }
    }

    /// <summary>
    /// Deserializes a single annotation for version 2.0.
    /// </summary>
    private static Annotation? DeserializeAnnotationV2(XElement annotationElement)
    {
        var annotationType = annotationElement.Attribute("AnnotationType")?.Value;
        if (string.IsNullOrEmpty(annotationType))
            return null;

        // Create the appropriate annotation type based on type-specific element presence
        Annotation? annotation = annotationType switch
        {
            var t when t.Contains("ArrowAnnotation") => new ArrowAnnotation(),
            var t when t.Contains("TextAnnotation") => new TextAnnotation(),
            var t when t.Contains("LineAnnotation") => new LineAnnotation(),
            var t when t.Contains("RectangleAnnotation") => new RectangleAnnotation(),
            var t when t.Contains("EllipseAnnotation") => new EllipseAnnotation(),
            var t when t.Contains("PointAnnotation") => new PointAnnotation(),
            var t when t.Contains("PolygonAnnotation") => new PolygonAnnotation(),
            var t when t.Contains("PolylineAnnotation") => new PolylineAnnotation(),
            _ => null
        };

        if (annotation is null)
            return null;

        // General properties
        var general = annotationElement.Element("General");
        if (general is not null)
        {
            annotation.Layer = SerializationHelpers.DeserializeEnum(general.Attribute("Layer")?.Value ?? "AboveSeries", AnnotationLayer.AboveSeries);
            annotation.XAxisKey = general.Attribute("XAxisKey")?.Value;
            annotation.YAxisKey = general.Attribute("YAxisKey")?.Value;
        }

        // Textual properties for TextualAnnotation
        if (annotation is TextualAnnotation textualAnnotation)
        {
            var textual = annotationElement.Element("Textual");
            if (textual is not null)
            {
                textualAnnotation.Text = textual.Attribute("Text")?.Value;
                textualAnnotation.TextColor = SerializationHelpers.DeserializeColor(textual.Attribute("TextColor")?.Value ?? "Automatic");
                textualAnnotation.Font = textual.Attribute("Font")?.Value;
                textualAnnotation.FontSize = SerializationHelpers.DeserializeDouble(textual.Attribute("FontSize")?.Value ?? "12");
                textualAnnotation.FontWeight = SerializationHelpers.DeserializeOxyFontWeight(textual.Attribute("FontWeight")?.Value ?? "Normal");
                textualAnnotation.TextPosition = SerializationHelpers.DeserializeDataPoint(textual.Attribute("TextPosition")?.Value ?? "NaN, NaN");
                textualAnnotation.TextRotation = SerializationHelpers.DeserializeDouble(textual.Attribute("TextRotation")?.Value ?? "0");
                textualAnnotation.TextHorizontalAlignment = SerializationHelpers.DeserializeEnum(textual.Attribute("TextHorizontalAlignment")?.Value ?? "Center", OxyHorizontalAlignment.Center);
                textualAnnotation.TextVerticalAlignment = SerializationHelpers.DeserializeEnum(textual.Attribute("TextVerticalAlignment")?.Value ?? "Middle", OxyVerticalAlignment.Middle);
            }
        }

        // Type-specific properties
        switch (annotation)
        {
            case ArrowAnnotation arrowAnnotation:
                var arrow = annotationElement.Element("Arrow");
                if (arrow is not null)
                {
                    arrowAnnotation.Color = SerializationHelpers.DeserializeColor(arrow.Attribute("Color")?.Value ?? "Automatic");
                    arrowAnnotation.StartPoint = SerializationHelpers.DeserializeDataPoint(arrow.Attribute("StartPoint")?.Value ?? "0, 0");
                    arrowAnnotation.EndPoint = SerializationHelpers.DeserializeDataPoint(arrow.Attribute("EndPoint")?.Value ?? "0, 0");
                    arrowAnnotation.ArrowDirection = SerializationHelpers.DeserializeScreenVector(arrow.Attribute("ArrowDirection")?.Value ?? "0, 0");
                    arrowAnnotation.HeadLength = SerializationHelpers.DeserializeDouble(arrow.Attribute("HeadLength")?.Value ?? "10");
                    arrowAnnotation.HeadWidth = SerializationHelpers.DeserializeDouble(arrow.Attribute("HeadWidth")?.Value ?? "3");
                    arrowAnnotation.Veeness = SerializationHelpers.DeserializeDouble(arrow.Attribute("Veeness")?.Value ?? "0");
                    arrowAnnotation.LineStyle = SerializationHelpers.DeserializeLineStyle(arrow.Attribute("LineStyle")?.Value ?? "Solid");
                    arrowAnnotation.StrokeThickness = SerializationHelpers.DeserializeDouble(arrow.Attribute("StrokeThickness")?.Value ?? "2");
                }
                break;

            case TextAnnotation textAnnotation:
                var text = annotationElement.Element("Text");
                if (text is not null)
                {
                    textAnnotation.Background = SerializationHelpers.DeserializeColor(text.Attribute("Background")?.Value ?? "Undefined");
                    textAnnotation.Stroke = SerializationHelpers.DeserializeColor(text.Attribute("Stroke")?.Value ?? "Black");
                    textAnnotation.StrokeThickness = SerializationHelpers.DeserializeDouble(text.Attribute("StrokeThickness")?.Value ?? "1");
                    textAnnotation.Padding = SerializationHelpers.DeserializeThickness(text.Attribute("Padding")?.Value ?? "4");
                    textAnnotation.Offset = SerializationHelpers.DeserializeScreenVector(text.Attribute("Offset")?.Value ?? "0, 0");
                }
                break;

            case LineAnnotation lineAnnotation:
                var line = annotationElement.Element("Line");
                if (line is not null)
                {
                    lineAnnotation.Color = SerializationHelpers.DeserializeColor(line.Attribute("Color")?.Value ?? "Automatic");
                    lineAnnotation.Type = SerializationHelpers.DeserializeEnum(line.Attribute("Type")?.Value ?? "Vertical", LineAnnotationType.Vertical);
                    lineAnnotation.X = SerializationHelpers.DeserializeDouble(line.Attribute("X")?.Value ?? "0");
                    lineAnnotation.Y = SerializationHelpers.DeserializeDouble(line.Attribute("Y")?.Value ?? "0");
                    lineAnnotation.Slope = SerializationHelpers.DeserializeDouble(line.Attribute("Slope")?.Value ?? "0");
                    lineAnnotation.Intercept = SerializationHelpers.DeserializeDouble(line.Attribute("Intercept")?.Value ?? "0");
                    lineAnnotation.MinimumX = SerializationHelpers.DeserializeDouble(line.Attribute("MinimumX")?.Value ?? "-Infinity");
                    lineAnnotation.MaximumX = SerializationHelpers.DeserializeDouble(line.Attribute("MaximumX")?.Value ?? "Infinity");
                    lineAnnotation.MinimumY = SerializationHelpers.DeserializeDouble(line.Attribute("MinimumY")?.Value ?? "-Infinity");
                    lineAnnotation.MaximumY = SerializationHelpers.DeserializeDouble(line.Attribute("MaximumY")?.Value ?? "Infinity");
                    lineAnnotation.LineStyle = SerializationHelpers.DeserializeLineStyle(line.Attribute("LineStyle")?.Value ?? "Solid");
                    lineAnnotation.StrokeThickness = SerializationHelpers.DeserializeDouble(line.Attribute("StrokeThickness")?.Value ?? "1");
                }
                break;

            case RectangleAnnotation rectangleAnnotation:
                var rectangle = annotationElement.Element("Rectangle");
                if (rectangle is not null)
                {
                    rectangleAnnotation.Fill = SerializationHelpers.DeserializeColor(rectangle.Attribute("Fill")?.Value ?? "Automatic");
                    rectangleAnnotation.Stroke = SerializationHelpers.DeserializeColor(rectangle.Attribute("Stroke")?.Value ?? "Black");
                    rectangleAnnotation.StrokeThickness = SerializationHelpers.DeserializeDouble(rectangle.Attribute("StrokeThickness")?.Value ?? "1");
                    rectangleAnnotation.MinimumX = SerializationHelpers.DeserializeDouble(rectangle.Attribute("MinimumX")?.Value ?? "0");
                    rectangleAnnotation.MaximumX = SerializationHelpers.DeserializeDouble(rectangle.Attribute("MaximumX")?.Value ?? "0");
                    rectangleAnnotation.MinimumY = SerializationHelpers.DeserializeDouble(rectangle.Attribute("MinimumY")?.Value ?? "0");
                    rectangleAnnotation.MaximumY = SerializationHelpers.DeserializeDouble(rectangle.Attribute("MaximumY")?.Value ?? "0");
                }
                break;

            case EllipseAnnotation ellipseAnnotation:
                var ellipse = annotationElement.Element("Ellipse");
                if (ellipse is not null)
                {
                    ellipseAnnotation.Fill = SerializationHelpers.DeserializeColor(ellipse.Attribute("Fill")?.Value ?? "Automatic");
                    ellipseAnnotation.Stroke = SerializationHelpers.DeserializeColor(ellipse.Attribute("Stroke")?.Value ?? "Black");
                    ellipseAnnotation.StrokeThickness = SerializationHelpers.DeserializeDouble(ellipse.Attribute("StrokeThickness")?.Value ?? "1");
                    ellipseAnnotation.X = SerializationHelpers.DeserializeDouble(ellipse.Attribute("X")?.Value ?? "0");
                    ellipseAnnotation.Y = SerializationHelpers.DeserializeDouble(ellipse.Attribute("Y")?.Value ?? "0");
                    ellipseAnnotation.Width = SerializationHelpers.DeserializeDouble(ellipse.Attribute("Width")?.Value ?? "0");
                    ellipseAnnotation.Height = SerializationHelpers.DeserializeDouble(ellipse.Attribute("Height")?.Value ?? "0");
                }
                break;

            case PointAnnotation pointAnnotation:
                var point = annotationElement.Element("Point");
                if (point is not null)
                {
                    pointAnnotation.Fill = SerializationHelpers.DeserializeColor(point.Attribute("Fill")?.Value ?? "Automatic");
                    pointAnnotation.Stroke = SerializationHelpers.DeserializeColor(point.Attribute("Stroke")?.Value ?? "Black");
                    pointAnnotation.StrokeThickness = SerializationHelpers.DeserializeDouble(point.Attribute("StrokeThickness")?.Value ?? "1");
                    pointAnnotation.X = SerializationHelpers.DeserializeDouble(point.Attribute("X")?.Value ?? "0");
                    pointAnnotation.Y = SerializationHelpers.DeserializeDouble(point.Attribute("Y")?.Value ?? "0");
                    pointAnnotation.Size = SerializationHelpers.DeserializeDouble(point.Attribute("Size")?.Value ?? "4");
                    pointAnnotation.Shape = SerializationHelpers.DeserializeEnum(point.Attribute("Shape")?.Value ?? "Square", MarkerType.Square);
                    pointAnnotation.TextMargin = SerializationHelpers.DeserializeDouble(point.Attribute("TextMargin")?.Value ?? "12");
                }
                break;

            case PolygonAnnotation polygonAnnotation:
                var polygon = annotationElement.Element("Polygon");
                if (polygon is not null)
                {
                    polygonAnnotation.Fill = SerializationHelpers.DeserializeColor(polygon.Attribute("Fill")?.Value ?? "Automatic");
                    polygonAnnotation.Stroke = SerializationHelpers.DeserializeColor(polygon.Attribute("Stroke")?.Value ?? "Black");
                    polygonAnnotation.StrokeThickness = SerializationHelpers.DeserializeDouble(polygon.Attribute("StrokeThickness")?.Value ?? "1");
                    polygonAnnotation.LineStyle = SerializationHelpers.DeserializeLineStyle(polygon.Attribute("LineStyle")?.Value ?? "Solid");
                    polygonAnnotation.LineJoin = SerializationHelpers.DeserializeEnum(polygon.Attribute("LineJoin")?.Value ?? "Miter", LineJoin.Miter);
                    var points = polygon.Element("Points");
                    if (points is not null)
                    {
                        foreach (var p in points.Elements("Point"))
                        {
                            var x = SerializationHelpers.DeserializeDouble(p.Attribute("X")?.Value ?? "0");
                            var y = SerializationHelpers.DeserializeDouble(p.Attribute("Y")?.Value ?? "0");
                            polygonAnnotation.Points.Add(new DataPoint(x, y));
                        }
                    }
                }
                break;

            case PolylineAnnotation polylineAnnotation:
                var polyline = annotationElement.Element("Polyline");
                if (polyline is not null)
                {
                    polylineAnnotation.Color = SerializationHelpers.DeserializeColor(polyline.Attribute("Color")?.Value ?? "Automatic");
                    polylineAnnotation.StrokeThickness = SerializationHelpers.DeserializeDouble(polyline.Attribute("StrokeThickness")?.Value ?? "1");
                    polylineAnnotation.LineStyle = SerializationHelpers.DeserializeLineStyle(polyline.Attribute("LineStyle")?.Value ?? "Solid");
                    polylineAnnotation.LineJoin = SerializationHelpers.DeserializeEnum(polyline.Attribute("LineJoin")?.Value ?? "Miter", LineJoin.Miter);
                    polylineAnnotation.MinimumSegmentLength = SerializationHelpers.DeserializeDouble(polyline.Attribute("MinimumSegmentLength")?.Value ?? "2");
                    var points = polyline.Element("Points");
                    if (points is not null)
                    {
                        foreach (var p in points.Elements("Point"))
                        {
                            var x = SerializationHelpers.DeserializeDouble(p.Attribute("X")?.Value ?? "0");
                            var y = SerializationHelpers.DeserializeDouble(p.Attribute("Y")?.Value ?? "0");
                            polylineAnnotation.Points.Add(new DataPoint(x, y));
                        }
                    }
                }
                break;
        }

        return annotation;
    }

    /// <summary>
    /// Deserializes all series for version 2.0.
    /// </summary>
    private static void DeserializeSeriesV2(PlotModel model, XElement seriesElement)
    {
        foreach (var item in seriesElement.Elements(SeriesItemTag))
        {
            var series = DeserializeSeriesItemV2(item);
            if (series is not null)
            {
                model.Series.Add(series);
            }
        }
    }

    /// <summary>
    /// Deserializes a single series for version 2.0.
    /// </summary>
    private static OxyPlot.Series.Series? DeserializeSeriesItemV2(XElement seriesElement)
    {
        var seriesType = seriesElement.Attribute("SeriesType")?.Value;
        if (string.IsNullOrEmpty(seriesType))
            return null;

        // Create the appropriate series type
        OxyPlot.Series.Series? series = seriesType switch
        {
            var t when t.Contains("LineSeries") => new LineSeries(),
            var t when t.Contains("ScatterSeries") => new ScatterSeries(),
            var t when t.Contains("BarSeries") => new BarSeries(),
            var t when t.Contains("AreaSeries") => new AreaSeries(),
            _ => null
        };

        if (series is null)
            return null;

        // General properties
        var general = seriesElement.Element("General");
        if (general is not null)
        {
            series.Title = general.Attribute("Title")?.Value;
            series.IsVisible = SerializationHelpers.DeserializeBoolean(general.Attribute("IsVisible")?.Value ?? "True");
            series.Background = SerializationHelpers.DeserializeColor(general.Attribute("Background")?.Value ?? "Undefined");
            series.TrackerKey = general.Attribute("TrackerKey")?.Value;
        }

        // Type-specific properties
        // Note: Derived types must come before base types in pattern matching
        switch (series)
        {
            // AreaSeries extends LineSeries, so check it first
            case AreaSeries areaSeries:
                var areaProps = seriesElement.Element("AreaSeries");
                if (areaProps is not null)
                {
                    areaSeries.Color = SerializationHelpers.DeserializeColor(areaProps.Attribute("Color")?.Value ?? "Automatic");
                    areaSeries.Color2 = SerializationHelpers.DeserializeColor(areaProps.Attribute("Color2")?.Value ?? "Automatic");
                    areaSeries.Fill = SerializationHelpers.DeserializeColor(areaProps.Attribute("Fill")?.Value ?? "Automatic");
                    areaSeries.StrokeThickness = SerializationHelpers.DeserializeDouble(areaProps.Attribute("StrokeThickness")?.Value ?? "2");
                    areaSeries.XAxisKey = areaProps.Attribute("XAxisKey")?.Value;
                    areaSeries.YAxisKey = areaProps.Attribute("YAxisKey")?.Value;
                }
                break;

            case LineSeries lineSeries:
                var lineProps = seriesElement.Element("LineSeries");
                if (lineProps is not null)
                {
                    lineSeries.Color = SerializationHelpers.DeserializeColor(lineProps.Attribute("Color")?.Value ?? "Automatic");
                    lineSeries.StrokeThickness = SerializationHelpers.DeserializeDouble(lineProps.Attribute("StrokeThickness")?.Value ?? "2");
                    lineSeries.LineStyle = SerializationHelpers.DeserializeLineStyle(lineProps.Attribute("LineStyle")?.Value ?? "Solid");
                    lineSeries.LineJoin = SerializationHelpers.DeserializeEnum(lineProps.Attribute("LineJoin")?.Value ?? "Bevel", LineJoin.Bevel);
                    lineSeries.MarkerType = SerializationHelpers.DeserializeEnum(lineProps.Attribute("MarkerType")?.Value ?? "None", MarkerType.None);
                    lineSeries.MarkerSize = SerializationHelpers.DeserializeDouble(lineProps.Attribute("MarkerSize")?.Value ?? "3");
                    lineSeries.MarkerStroke = SerializationHelpers.DeserializeColor(lineProps.Attribute("MarkerStroke")?.Value ?? "Automatic");
                    lineSeries.MarkerStrokeThickness = SerializationHelpers.DeserializeDouble(lineProps.Attribute("MarkerStrokeThickness")?.Value ?? "1");
                    lineSeries.MarkerFill = SerializationHelpers.DeserializeColor(lineProps.Attribute("MarkerFill")?.Value ?? "Automatic");
                    // In OxyPlot 2.x, Smooth was replaced with InterpolationAlgorithm
                    if (SerializationHelpers.DeserializeBoolean(lineProps.Attribute("Smooth")?.Value ?? "False"))
                        lineSeries.InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline;
                    lineSeries.XAxisKey = lineProps.Attribute("XAxisKey")?.Value;
                    lineSeries.YAxisKey = lineProps.Attribute("YAxisKey")?.Value;
                }
                break;

            case ScatterSeries scatterSeries:
                var scatterProps = seriesElement.Element("ScatterSeries");
                if (scatterProps is not null)
                {
                    scatterSeries.MarkerType = SerializationHelpers.DeserializeEnum(scatterProps.Attribute("MarkerType")?.Value ?? "Circle", MarkerType.Circle);
                    scatterSeries.MarkerSize = SerializationHelpers.DeserializeDouble(scatterProps.Attribute("MarkerSize")?.Value ?? "5");
                    scatterSeries.MarkerStroke = SerializationHelpers.DeserializeColor(scatterProps.Attribute("MarkerStroke")?.Value ?? "Automatic");
                    scatterSeries.MarkerStrokeThickness = SerializationHelpers.DeserializeDouble(scatterProps.Attribute("MarkerStrokeThickness")?.Value ?? "1");
                    scatterSeries.MarkerFill = SerializationHelpers.DeserializeColor(scatterProps.Attribute("MarkerFill")?.Value ?? "Automatic");
                    scatterSeries.XAxisKey = scatterProps.Attribute("XAxisKey")?.Value;
                    scatterSeries.YAxisKey = scatterProps.Attribute("YAxisKey")?.Value;
                }
                break;

            case BarSeries barSeries:
                var barProps = seriesElement.Element("BarSeries");
                if (barProps is not null)
                {
                    barSeries.FillColor = SerializationHelpers.DeserializeColor(barProps.Attribute("FillColor")?.Value ?? "Automatic");
                    barSeries.StrokeColor = SerializationHelpers.DeserializeColor(barProps.Attribute("StrokeColor")?.Value ?? "Black");
                    barSeries.StrokeThickness = SerializationHelpers.DeserializeDouble(barProps.Attribute("StrokeThickness")?.Value ?? "0");
                    barSeries.BarWidth = SerializationHelpers.DeserializeDouble(barProps.Attribute("BarWidth")?.Value ?? "1");
                    barSeries.XAxisKey = barProps.Attribute("XAxisKey")?.Value;
                    barSeries.YAxisKey = barProps.Attribute("YAxisKey")?.Value;
                }
                break;
        }

        return series;
    }

    /// <summary>
    /// Deserializes version 1.0 (old VB format) with best-effort compatibility.
    /// </summary>
    /// <remarks>
    /// This provides backward compatibility with the old Plot control XML format.
    /// Not all features may be supported, but basic properties will be loaded.
    /// </remarks>
    private static PlotModel DeserializeV1(XElement root)
    {
        var model = new PlotModel();

        try
        {
            var general = root.Element(GeneralPropertiesTag);
            if (general is not null)
            {
                DeserializeGeneralPropertiesV1(model, general);
            }

            // TODO: Deserialize axes, series, annotations in future phases

            return model;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error deserializing V1 format: {ex.Message}");
            return model; // Return partially loaded model
        }
    }

    /// <summary>
    /// Deserializes general properties for version 1.0 (old format).
    /// </summary>
    private static void DeserializeGeneralPropertiesV1(PlotModel model, XElement general)
    {
        // Title properties (try new names first, then old names for backward compatibility)
        var title = general.Element("Title");
        if (title is not null)
        {
            model.Title = title.Attribute("Title")?.Value ?? title.Attribute("Text")?.Value ?? string.Empty;
            model.TitleColor = SerializationHelpers.DeserializeColor(title.Attribute("TitleColor")?.Value ?? title.Attribute("Color")?.Value ?? "Automatic");
            model.TitleFont = title.Attribute("TitleFont")?.Value ?? title.Attribute("Font")?.Value;
            model.TitleFontSize = SerializationHelpers.DeserializeDouble(title.Attribute("TitleFontSize")?.Value ?? title.Attribute("Size")?.Value ?? "18");
            model.TitleFontWeight = SerializationHelpers.DeserializeOxyFontWeight(title.Attribute("TitleFontWeight")?.Value ?? title.Attribute("Weight")?.Value ?? "Bold", OxyPlot.FontWeights.Bold);
            model.TitlePadding = SerializationHelpers.DeserializeDouble(title.Attribute("TitlePadding")?.Value ?? title.Attribute("Padding")?.Value ?? "6");
        }

        // Subtitle properties (similar backward compatibility)
        var subtitle = general.Element("Subtitle");
        if (subtitle is not null)
        {
            model.Subtitle = subtitle.Attribute("Subtitle")?.Value ?? subtitle.Attribute("Title")?.Value ?? string.Empty;
            model.SubtitleColor = SerializationHelpers.DeserializeColor(subtitle.Attribute("SubtitleColor")?.Value ?? subtitle.Attribute("Color")?.Value ?? "Automatic");
            model.SubtitleFont = subtitle.Attribute("SubtitleFont")?.Value ?? subtitle.Attribute("Font")?.Value;
            model.SubtitleFontSize = SerializationHelpers.DeserializeDouble(subtitle.Attribute("SubtitleFontSize")?.Value ?? subtitle.Attribute("Size")?.Value ?? "14");
            model.SubtitleFontWeight = SerializationHelpers.DeserializeOxyFontWeight(subtitle.Attribute("SubtitleFontWeight")?.Value ?? subtitle.Attribute("Weight")?.Value ?? "Normal");
        }

        // Plot area properties (old format might use "Plot" or "PlotArea")
        var plotArea = general.Element("Plot") ?? general.Element("PlotArea");
        if (plotArea is not null)
        {
            // Old format might have used Brush serialization - try to parse
            var bgAttr = plotArea.Attribute("PlotAreaBackground")?.Value;
            if (!string.IsNullOrEmpty(bgAttr))
            {
                // Try as color first, fallback to brush parsing
                model.PlotAreaBackground = SerializationHelpers.DeserializeColor(bgAttr);
            }

            model.PlotAreaBorderColor = SerializationHelpers.DeserializeColor(plotArea.Attribute("PlotAreaBorderColor")?.Value ?? plotArea.Attribute("BorderColor")?.Value ?? "Black");
            model.PlotAreaBorderThickness = SerializationHelpers.DeserializeThickness(plotArea.Attribute("PlotAreaBorderThickness")?.Value ?? plotArea.Attribute("BorderThickness")?.Value ?? "1");
        }

        // Background properties (old format might use "Chart")
        var background = general.Element("Background") ?? general.Element("Chart");
        if (background is not null)
        {
            var bgAttr = background.Attribute("Background")?.Value;
            if (!string.IsNullOrEmpty(bgAttr))
            {
                model.Background = SerializationHelpers.DeserializeColor(bgAttr);
            }

            model.Padding = SerializationHelpers.DeserializeThickness(background.Attribute("Padding")?.Value ?? "8");
        }
    }

    #endregion
}
