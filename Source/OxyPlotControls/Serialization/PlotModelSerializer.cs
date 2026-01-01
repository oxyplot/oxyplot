using System;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
using OxyPlot;

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

        // TODO: Serialize axes, series, annotations in future phases

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
        title.SetAttributeValue(nameof(model.TitleFontWeight), SerializationHelpers.SerializeFontWeight(model.TitleFontWeight));
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
        subtitle.SetAttributeValue(nameof(model.SubtitleFontWeight), SerializationHelpers.SerializeFontWeight(model.SubtitleFontWeight));

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

        // TODO: Deserialize axes, series, annotations in future phases

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
            model.TitleFontWeight = SerializationHelpers.DeserializeFontWeight(title.Attribute(nameof(model.TitleFontWeight))?.Value ?? "Bold");
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
            model.SubtitleFontWeight = SerializationHelpers.DeserializeFontWeight(subtitle.Attribute(nameof(model.SubtitleFontWeight))?.Value ?? "Normal");
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
            model.TitleFontWeight = SerializationHelpers.DeserializeFontWeight(title.Attribute("TitleFontWeight")?.Value ?? title.Attribute("Weight")?.Value ?? "Bold");
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
            model.SubtitleFontWeight = SerializationHelpers.DeserializeFontWeight(subtitle.Attribute("SubtitleFontWeight")?.Value ?? subtitle.Attribute("Weight")?.Value ?? "Normal");
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
