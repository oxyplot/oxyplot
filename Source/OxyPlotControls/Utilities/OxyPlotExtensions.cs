using System;
using System.Globalization;
using System.Xml.Linq;
using OxyPlot;

namespace OxyPlotControls.Utilities;

/// <summary>
/// Extension methods for OxyPlot types.
/// </summary>
/// <remarks>
/// Provides utility methods for formatting and parsing OxyPlot data types.
/// Migrated from VB ExtensionsModule with modernizations for PlotModel architecture.
/// </remarks>
public static class OxyPlotExtensions
{
    #region DataPoint Extensions

    /// <summary>
    /// Converts a <see cref="DataPoint"/> to a formatted string representation.
    /// </summary>
    /// <param name="dataPoint">The data point to format.</param>
    /// <returns>String in format "X, Y" using invariant culture and G17 precision.</returns>
    public static string ToPrettyText(this DataPoint dataPoint)
    {
        return $"{dataPoint.X.ToString("G17", CultureInfo.InvariantCulture)}, {dataPoint.Y.ToString("G17", CultureInfo.InvariantCulture)}";
    }

    /// <summary>
    /// Parses a formatted string into a <see cref="DataPoint"/>.
    /// </summary>
    /// <param name="text">String in format "X, Y".</param>
    /// <returns>Parsed DataPoint, or DataPoint.Undefined if parsing fails.</returns>
    public static DataPoint FromPrettyDataText(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return DataPoint.Undefined;

        var parts = text.Split(',', StringSplitOptions.TrimEntries);
        if (parts.Length != 2)
            return DataPoint.Undefined;

        if (!double.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out var x))
            return DataPoint.Undefined;

        if (!double.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var y))
            return DataPoint.Undefined;

        return new DataPoint(x, y);
    }

    /// <summary>
    /// Converts a <see cref="DataPoint"/> to an XML element.
    /// </summary>
    /// <param name="dataPoint">The data point to serialize.</param>
    /// <returns>XElement containing X and Y attributes.</returns>
    public static XElement ToXElement(this DataPoint dataPoint)
    {
        var element = new XElement("DataPoint");
        element.SetAttributeValue("X", dataPoint.X.ToString("G17", CultureInfo.InvariantCulture));
        element.SetAttributeValue("Y", dataPoint.Y.ToString("G17", CultureInfo.InvariantCulture));
        return element;
    }

    /// <summary>
    /// Parses an XML element into a <see cref="DataPoint"/>.
    /// </summary>
    /// <param name="element">The XML element to parse.</param>
    /// <returns>Parsed DataPoint, or DataPoint.Undefined if parsing fails.</returns>
    public static DataPoint PointFromXElement(this XElement element)
    {
        if (element.Name != "DataPoint")
            return DataPoint.Undefined;

        var xAttr = element.Attribute("X");
        var yAttr = element.Attribute("Y");

        if (xAttr is null || yAttr is null)
            return DataPoint.Undefined;

        if (!double.TryParse(xAttr.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var x))
            return DataPoint.Undefined;

        if (!double.TryParse(yAttr.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var y))
            return DataPoint.Undefined;

        return new DataPoint(x, y);
    }

    #endregion

    #region ScreenPoint Extensions

    /// <summary>
    /// Converts a <see cref="ScreenPoint"/> to a formatted string representation.
    /// </summary>
    /// <param name="screenPoint">The screen point to format.</param>
    /// <returns>String in format "X, Y" using invariant culture and G17 precision.</returns>
    public static string ToPrettyText(this ScreenPoint screenPoint)
    {
        return $"{screenPoint.X.ToString("G17", CultureInfo.InvariantCulture)}, {screenPoint.Y.ToString("G17", CultureInfo.InvariantCulture)}";
    }

    /// <summary>
    /// Parses a formatted string into a <see cref="ScreenPoint"/>.
    /// </summary>
    /// <param name="text">String in format "X, Y".</param>
    /// <returns>Parsed ScreenPoint, or ScreenPoint.Undefined if parsing fails.</returns>
    public static ScreenPoint FromPrettyScreenText(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return ScreenPoint.Undefined;

        var parts = text.Split(',', StringSplitOptions.TrimEntries);
        if (parts.Length != 2)
            return ScreenPoint.Undefined;

        if (!double.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out var x))
            return ScreenPoint.Undefined;

        if (!double.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var y))
            return ScreenPoint.Undefined;

        return new ScreenPoint(x, y);
    }

    #endregion

    #region ScreenVector Extensions

    /// <summary>
    /// Converts a <see cref="ScreenVector"/> to a formatted string representation.
    /// </summary>
    /// <param name="screenVector">The screen vector to format.</param>
    /// <returns>String in format "X, Y" using invariant culture and G17 precision.</returns>
    public static string ToPrettyText(this ScreenVector screenVector)
    {
        return $"{screenVector.X.ToString("G17", CultureInfo.InvariantCulture)}, {screenVector.Y.ToString("G17", CultureInfo.InvariantCulture)}";
    }

    /// <summary>
    /// Parses a formatted string into a <see cref="ScreenVector"/>.
    /// </summary>
    /// <param name="text">String in format "X, Y".</param>
    /// <returns>Parsed ScreenVector, or zero vector if parsing fails.</returns>
    public static ScreenVector FromPrettyVectorText(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return new ScreenVector(0, 0);

        var parts = text.Split(',', StringSplitOptions.TrimEntries);
        if (parts.Length != 2)
            return new ScreenVector(0, 0);

        if (!double.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out var x))
            return new ScreenVector(0, 0);

        if (!double.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var y))
            return new ScreenVector(0, 0);

        return new ScreenVector(x, y);
    }

    #endregion
}
