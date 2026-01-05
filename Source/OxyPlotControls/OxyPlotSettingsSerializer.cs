using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;
using OxyPlot;
using OxyPlot.Wpf;

namespace OxyPlotControls
{
    /// <summary>
    /// Provides serialization and deserialization functionality for OxyPlot plot settings.
    /// This module handles converting plot properties to and from XML elements for persistence.
    /// Supports both V1 (legacy) and V2 (modern) XML formats for backward compatibility.
    /// </summary>
    public static class OxyPlotSettingsSerializer
    {
        /// <summary>
        /// The XML tag name used for the root element containing OxyPlot properties.
        /// </summary>
        public static readonly string OxyplotPropertiesTag = "OxyplotProperties";

        /// <summary>
        /// Version attribute name for the serialization format.
        /// </summary>
        public const string VersionAttribute = "Version";

        /// <summary>
        /// Current serialization format version.
        /// </summary>
        public const int CurrentVersion = 2;

        /// <summary>
        /// Serializes all plot properties to an XElement for persistence using the modern V2 format.
        /// </summary>
        /// <param name="plotModel">The PlotModel to serialize.</param>
        /// <returns>An XElement containing all serialized plot properties including general settings, legend, axes, annotations, and series.</returns>
        public static XElement ToXelement(PlotModel plotModel)
        {
            var plotPropertiesElement = new XElement(OxyplotPropertiesTag);
            plotPropertiesElement.SetAttributeValue(VersionAttribute, CurrentVersion);

            // General Settings
            plotPropertiesElement.Add(GeneralPlotControl.GeneralPropertiesToXElement(plotModel));

            plotPropertiesElement.Add(LegendControl.LegendPropertiesToXElement(plotModel));

            plotPropertiesElement.Add(AxesControl.AxesPropertiesToXElement(plotModel));

            plotPropertiesElement.Add(AnnotationSelectorControl.AnnotationsPropertiesToXElement(plotModel));

            plotPropertiesElement.Add(GenericSeriesControl.SeriesPropertiesToXElement(plotModel));

            return plotPropertiesElement;
        }

        /// <summary>
        /// Deserializes plot properties from an XElement and applies them to the plot model.
        /// Automatically detects V1 (legacy) or V2 (modern) format.
        /// </summary>
        /// <param name="plotModel">The PlotModel to apply settings to.</param>
        /// <param name="element">The XElement containing serialized plot properties.</param>
        public static void FromXelement(PlotModel plotModel, XElement element)
        {
            // Detect version - if no version attribute, assume V1 (legacy)
            int version = 1;
            if (GetIntegerAttribute(element, VersionAttribute, out int ver))
            {
                version = ver;
            }

            // General
            var generalElement = element.Element(GeneralPlotControl.GeneralPropertiesTag);
            if (generalElement != null) GeneralPlotControl.XElementToGeneralProperties(plotModel, generalElement, version);

            // Legend
            var legendElement = element.Element(LegendControl.LegendPropertiesTag);
            if (legendElement != null) LegendControl.XElementToLegendProperties(plotModel, legendElement, version);

            // Axes
            var axesElement = element.Element(AxesControl.AxesPropertiesTag);
            if (axesElement != null) AxesControl.XElementToAxesProperties(plotModel, axesElement, version);

            // Annotations
            var annotationsElement = element.Element(AnnotationSelectorControl.AnnotationsPropertiesTag);
            if (annotationsElement != null) AnnotationSelectorControl.XElementToAnnotationsProperties(plotModel, annotationsElement, version);

            // Series
            var seriesElement = element.Element(GenericSeriesControl.SeriesPropertiesTag);
            if (seriesElement != null) GenericSeriesControl.XElementToSeriesProperties(plotModel, seriesElement, version);

            // Update the plot.
            plotModel.InvalidatePlot(true);
        }

        /// <summary>
        /// Serializes plot properties for a PlotView control (convenience method).
        /// </summary>
        /// <param name="plotView">The PlotView containing the PlotModel to serialize.</param>
        /// <returns>An XElement containing all serialized plot properties.</returns>
        public static XElement ToXelement(PlotView plotView)
        {
            if (plotView?.Model == null)
                throw new ArgumentNullException(nameof(plotView), "PlotView or its Model cannot be null");
            return ToXelement(plotView.Model);
        }

        /// <summary>
        /// Deserializes plot properties and applies them to a PlotView's model (convenience method).
        /// </summary>
        /// <param name="plotView">The PlotView containing the PlotModel to apply settings to.</param>
        /// <param name="element">The XElement containing serialized plot properties.</param>
        public static void FromXelement(PlotView plotView, XElement element)
        {
            if (plotView?.Model == null)
                throw new ArgumentNullException(nameof(plotView), "PlotView or its Model cannot be null");
            FromXelement(plotView.Model, element);
        }

        /// <summary>
        /// Attempts to get a Color value from an XML element attribute.
        /// </summary>
        /// <param name="el">The XElement to read from.</param>
        /// <param name="attributeName">The name of the attribute to read.</param>
        /// <param name="c">When this method returns, contains the parsed Color if successful.</param>
        /// <returns>True if the attribute exists and was successfully parsed; otherwise, false.</returns>
        public static bool GetColorAttribute(XElement el, string attributeName, out Color c)
        {
            c = default(Color);
            if (el.Attribute(attributeName) == null) return false;
            string value = el.Attribute(attributeName).Value;
            if (string.IsNullOrEmpty(value)) return false;

            c = (Color)ColorConverter.ConvertFromString(value);
            return true;
        }

        /// <summary>
        /// Attempts to get a Brush value from an XML element attribute.
        /// </summary>
        /// <param name="el">The XElement to read from.</param>
        /// <param name="attributeName">The name of the attribute to read.</param>
        /// <param name="converter">The BrushConverter to use for parsing.</param>
        /// <param name="b">When this method returns, contains the parsed Brush if successful.</param>
        /// <returns>True if the attribute exists and was successfully parsed; otherwise, false.</returns>
        public static bool GetBrushAttribute(XElement el, string attributeName, BrushConverter converter, out Brush b)
        {
            b = null;
            if (el.Attribute(attributeName) == null) return false;
            string value = el.Attribute(attributeName).Value;
            if (string.IsNullOrEmpty(value)) return false;

            b = (Brush)converter.ConvertFromInvariantString(value);
            return true;
        }

        /// <summary>
        /// Attempts to get a String value from an XML element attribute.
        /// </summary>
        /// <param name="el">The XElement to read from.</param>
        /// <param name="attributeName">The name of the attribute to read.</param>
        /// <param name="s">When this method returns, contains the attribute value if successful.</param>
        /// <returns>True if the attribute exists; otherwise, false.</returns>
        public static bool GetStringAttribute(XElement el, string attributeName, out string s)
        {
            s = null;
            if (el.Attribute(attributeName) == null) return false;

            s = el.Attribute(attributeName).Value;
            return true;
        }

        /// <summary>
        /// Attempts to get a Double value from an XML element attribute.
        /// </summary>
        /// <param name="el">The XElement to read from.</param>
        /// <param name="attributeName">The name of the attribute to read.</param>
        /// <param name="d">When this method returns, contains the parsed Double if successful.</param>
        /// <returns>True if the attribute exists and was successfully parsed; otherwise, false.</returns>
        public static bool GetDoubleAttribute(XElement el, string attributeName, out double d)
        {
            d = 0;
            if (el.Attribute(attributeName) == null) return false;
            string value = el.Attribute(attributeName).Value;
            if (string.IsNullOrEmpty(value)) return false;

            return double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out d);
        }

        /// <summary>
        /// Attempts to get an Integer value from an XML element attribute.
        /// </summary>
        /// <param name="el">The XElement to read from.</param>
        /// <param name="attributeName">The name of the attribute to read.</param>
        /// <param name="i">When this method returns, contains the parsed Integer if successful.</param>
        /// <returns>True if the attribute exists and was successfully parsed; otherwise, false.</returns>
        public static bool GetIntegerAttribute(XElement el, string attributeName, out int i)
        {
            i = 0;
            if (el.Attribute(attributeName) == null) return false;
            string value = el.Attribute(attributeName).Value;
            if (string.IsNullOrEmpty(value)) return false;

            return int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out i);
        }

        /// <summary>
        /// Attempts to get a Boolean value from an XML element attribute.
        /// </summary>
        /// <param name="el">The XElement to read from.</param>
        /// <param name="attributeName">The name of the attribute to read.</param>
        /// <param name="b">When this method returns, contains the parsed Boolean if successful.</param>
        /// <returns>True if the attribute exists and was successfully parsed; otherwise, false.</returns>
        public static bool GetBooleanAttribute(XElement el, string attributeName, out bool b)
        {
            b = false;
            if (el.Attribute(attributeName) == null) return false;
            string value = el.Attribute(attributeName).Value;
            if (string.IsNullOrEmpty(value)) return false;

            return bool.TryParse(value, out b);
        }

        /// <summary>
        /// Attempts to get a FontFamily value from an XML element attribute.
        /// </summary>
        /// <param name="el">The XElement to read from.</param>
        /// <param name="attributeName">The name of the attribute to read.</param>
        /// <param name="converter">The FontFamilyConverter to use for parsing.</param>
        /// <param name="ff">When this method returns, contains the parsed FontFamily if successful.</param>
        /// <returns>True if the attribute exists and was successfully parsed; otherwise, false.</returns>
        public static bool GetFontFamilyAttribute(XElement el, string attributeName, FontFamilyConverter converter, out FontFamily ff)
        {
            ff = null;
            if (el.Attribute(attributeName) == null) return false;
            string value = el.Attribute(attributeName).Value;
            if (string.IsNullOrEmpty(value)) return false;

            ff = (FontFamily)converter.ConvertFromInvariantString(value);
            return true;
        }

        /// <summary>
        /// Attempts to get a FontWeight value from an XML element attribute.
        /// </summary>
        /// <param name="el">The XElement to read from.</param>
        /// <param name="attributeName">The name of the attribute to read.</param>
        /// <param name="converter">The FontWeightConverter to use for parsing.</param>
        /// <param name="fw">When this method returns, contains the parsed FontWeight if successful.</param>
        /// <returns>True if the attribute exists and was successfully parsed; otherwise, false.</returns>
        public static bool GetFontWeightAttribute(XElement el, string attributeName, FontWeightConverter converter, out FontWeight fw)
        {
            fw = default(FontWeight);
            if (el.Attribute(attributeName) == null) return false;
            string value = el.Attribute(attributeName).Value;
            if (string.IsNullOrEmpty(value)) return false;

            fw = (FontWeight)converter.ConvertFromInvariantString(value);
            return true;
        }

        /// <summary>
        /// Attempts to get a Thickness value from an XML element attribute.
        /// </summary>
        /// <param name="el">The XElement to read from.</param>
        /// <param name="attributeName">The name of the attribute to read.</param>
        /// <param name="converter">The ThicknessConverter to use for parsing.</param>
        /// <param name="t">When this method returns, contains the parsed Thickness if successful.</param>
        /// <returns>True if the attribute exists and was successfully parsed; otherwise, false.</returns>
        public static bool GetThicknessAttribute(XElement el, string attributeName, System.Windows.ThicknessConverter converter, out Thickness t)
        {
            t = default(Thickness);
            if (el.Attribute(attributeName) == null) return false;
            string value = el.Attribute(attributeName).Value;
            if (string.IsNullOrEmpty(value)) return false;

            t = (Thickness)converter.ConvertFromInvariantString(value);
            return true;
        }

        /// <summary>
        /// Attempts to get an Enum value from an XML element attribute.
        /// </summary>
        /// <typeparam name="TEnum">The enum type to parse.</typeparam>
        /// <param name="el">The XElement to read from.</param>
        /// <param name="attributeName">The name of the attribute to read.</param>
        /// <param name="e">When this method returns, contains the parsed enum value if successful.</param>
        /// <returns>True if the attribute exists and was successfully parsed; otherwise, false.</returns>
        public static bool GetEnumAttribute<TEnum>(XElement el, string attributeName, out TEnum e) where TEnum : struct
        {
            e = default(TEnum);
            if (el.Attribute(attributeName) == null) return false;
            string value = el.Attribute(attributeName).Value;
            if (string.IsNullOrEmpty(value)) return false;

            return Enum.TryParse(value, out e);
        }

        /// <summary>
        /// Attempts to get an OxyPlot DataPoint value from an XML element attribute.
        /// </summary>
        /// <param name="el">The XElement to read from.</param>
        /// <param name="attributeName">The name of the attribute to read.</param>
        /// <param name="dp">When this method returns, contains the parsed DataPoint if successful.</param>
        /// <returns>True if the attribute exists and was successfully parsed; otherwise, false.</returns>
        public static bool GetDataPointAttribute(XElement el, string attributeName, out OxyPlot.DataPoint dp)
        {
            dp = OxyPlot.DataPoint.Undefined;
            if (el.Attribute(attributeName) == null) return false;
            string value = el.Attribute(attributeName).Value;
            if (string.IsNullOrEmpty(value)) return false;

            dp = value.FromPrettyDataText();
            return true;
        }

        /// <summary>
        /// Attempts to get an OxyPlot ScreenVector value from an XML element attribute.
        /// </summary>
        /// <param name="el">The XElement to read from.</param>
        /// <param name="attributeName">The name of the attribute to read.</param>
        /// <param name="vp">When this method returns, contains the parsed ScreenVector if successful.</param>
        /// <returns>True if the attribute exists and was successfully parsed; otherwise, false.</returns>
        public static bool GetScreenVectorAttribute(XElement el, string attributeName, out OxyPlot.ScreenVector vp)
        {
            vp = default(OxyPlot.ScreenVector);
            if (el.Attribute(attributeName) == null) return false;
            string value = el.Attribute(attributeName).Value;
            if (string.IsNullOrEmpty(value)) return false;

            vp = value.FromPrettyVectorText();
            return true;
        }

        /// <summary>
        /// Attempts to get an OxyPlot ScreenPoint value from an XML element attribute.
        /// </summary>
        /// <param name="el">The XElement to read from.</param>
        /// <param name="attributeName">The name of the attribute to read.</param>
        /// <param name="vp">When this method returns, contains the parsed ScreenPoint if successful.</param>
        /// <returns>True if the attribute exists and was successfully parsed; otherwise, false.</returns>
        public static bool GetScreenPointAttribute(XElement el, string attributeName, out OxyPlot.ScreenPoint vp)
        {
            vp = OxyPlot.ScreenPoint.Undefined;
            if (el.Attribute(attributeName) == null) return false;
            string value = el.Attribute(attributeName).Value;
            if (string.IsNullOrEmpty(value)) return false;

            vp = value.FromPrettyScreenText();
            return true;
        }

        /// <summary>
        /// Attempts to get a WPF Vector value from an XML element attribute.
        /// </summary>
        /// <param name="el">The XElement to read from.</param>
        /// <param name="attributeName">The name of the attribute to read.</param>
        /// <param name="v">When this method returns, contains the parsed Vector if successful.</param>
        /// <returns>True if the attribute exists and was successfully parsed; otherwise, false.</returns>
        public static bool GetVectorAttribute(XElement el, string attributeName, out Vector v)
        {
            v = default(Vector);
            if (el.Attribute(attributeName) == null) return false;
            string value = el.Attribute(attributeName).Value;
            if (string.IsNullOrEmpty(value)) return false;

            v = value.FromPrettyVectorString();
            return true;
        }

        /// <summary>
        /// Attempts to get an OxyColor value from an XML element attribute.
        /// Supports both hex format (#AARRGGBB) and named color format.
        /// </summary>
        /// <param name="el">The XElement to read from.</param>
        /// <param name="attributeName">The name of the attribute to read.</param>
        /// <param name="oxyColor">When this method returns, contains the parsed OxyColor if successful.</param>
        /// <returns>True if the attribute exists and was successfully parsed; otherwise, false.</returns>
        public static bool GetOxyColorAttribute(XElement el, string attributeName, out OxyColor oxyColor)
        {
            oxyColor = OxyColors.Undefined;
            if (el.Attribute(attributeName) == null) return false;
            string value = el.Attribute(attributeName).Value;
            if (string.IsNullOrEmpty(value)) return false;

            // Try to parse as hex color first (#AARRGGBB or #RRGGBB)
            if (value.StartsWith("#"))
            {
                try
                {
                    oxyColor = OxyColor.Parse(value);
                    return true;
                }
                catch
                {
                    // Fall through to try other parsing methods
                }
            }

            // Try to parse using WPF ColorConverter (handles named colors like "Red", "Blue", etc.)
            try
            {
                var wpfColor = (Color)ColorConverter.ConvertFromString(value);
                oxyColor = OxyColor.FromArgb(wpfColor.A, wpfColor.R, wpfColor.G, wpfColor.B);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Converts an OxyColor to a string suitable for XML serialization.
        /// </summary>
        /// <param name="color">The OxyColor to convert.</param>
        /// <returns>A hex string representation of the color in #AARRGGBB format.</returns>
        public static string OxyColorToString(OxyColor color)
        {
            if (color.IsUndefined())
                return string.Empty;
            return color.ToString();
        }
    }
}
