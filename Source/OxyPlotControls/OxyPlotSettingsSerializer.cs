using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;
using OxyPlot.Wpf;

namespace OxyPlotControls
{
    /// <summary>
    /// Provides serialization and deserialization functionality for OxyPlot plot settings.
    /// This module handles converting plot properties to and from XML elements for persistence.
    /// </summary>
    public static class OxyPlotSettingsSerializer
    {
        /// <summary>
        /// The XML tag name used for the root element containing OxyPlot properties.
        /// </summary>
        public static readonly string OxyplotPropertiesTag = "OxyplotProperties";

        /// <summary>
        /// Serializes all plot properties to an XElement for persistence.
        /// </summary>
        /// <param name="plot">The OxyPlot Plot control to serialize.</param>
        /// <returns>An XElement containing all serialized plot properties including general settings, legend, axes, annotations, and series.</returns>
        public static XElement ToXelement(Plot plot)
        {
            var plotPropertiesElement = new XElement(OxyplotPropertiesTag);

            // General Settings
            plotPropertiesElement.Add(GeneralPlotControl.GeneralPropertiesToXElement(plot));

            plotPropertiesElement.Add(LegendControl.LegendPropertiesToXElement(plot));

            plotPropertiesElement.Add(AxesControl.AxesPropertiesToXElement(plot));

            plotPropertiesElement.Add(AnnotationSelectorControl.AnnotationsPropertiesToXElement(plot));

            plotPropertiesElement.Add(GenericSeriesControl.SeriesPropertiesToXElement(plot));

            return plotPropertiesElement;
        }

        /// <summary>
        /// Deserializes plot properties from an XElement and applies them to the plot.
        /// </summary>
        /// <param name="plot">The OxyPlot Plot control to apply settings to.</param>
        /// <param name="element">The XElement containing serialized plot properties.</param>
        public static void FromXelement(Plot plot, XElement element)
        {
            // General
            var generalElement = element.Element(GeneralPlotControl.GeneralPropertiesTag);
            if (generalElement != null) GeneralPlotControl.XElementToGeneralProperties(plot, generalElement);

            // Legend
            var legendElement = element.Element(LegendControl.LegendPropertiesTag);
            if (legendElement != null) LegendControl.XElementToLegendProperties(plot, legendElement);

            // Axes
            var axesElement = element.Element(AxesControl.AxesPropertiesTag);
            if (axesElement != null) AxesControl.XElementToAxesProperties(plot, axesElement);

            // Annotations
            var annotationsElement = element.Element(AnnotationSelectorControl.AnnotationsPropertiesTag);
            if (annotationsElement != null) AnnotationSelectorControl.XElementToAnnotationsProperties(plot, annotationsElement);

            // Series
            var seriesElement = element.Element(GenericSeriesControl.SeriesPropertiesTag);
            if (seriesElement != null) GenericSeriesControl.XElementToSeriesProperties(plot, seriesElement);

            // Update the plot.
            plot.InvalidatePlot(true);
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
    }
}
