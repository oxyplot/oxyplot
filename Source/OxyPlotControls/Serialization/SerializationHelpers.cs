using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using OxyPlot;

namespace OxyPlotControls.Serialization;

/// <summary>
/// Provides culture-invariant serialization and deserialization helpers for OxyPlot types.
/// </summary>
/// <remarks>
/// This class ensures consistent serialization regardless of the user's culture settings.
/// All serialization uses InvariantCulture to prevent issues with decimal separators,
/// date formats, and other culture-specific formatting.
/// </remarks>
public static class SerializationHelpers
{
    #region OxyColor Serialization

    /// <summary>
    /// Serializes an <see cref="OxyColor"/> to a hex string (e.g., "#FF000000").
    /// </summary>
    /// <param name="color">The color to serialize.</param>
    /// <returns>Hex string representation of the color.</returns>
    public static string SerializeColor(OxyColor color)
    {
        if (color.IsAutomatic())
        {
            return "Automatic";
        }

        if (color.IsUndefined())
        {
            return "Undefined";
        }

        return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
    }

    /// <summary>
    /// Deserializes an <see cref="OxyColor"/> from a hex string or color name.
    /// </summary>
    /// <param name="value">The hex string or color name.</param>
    /// <returns>The deserialized color, or Automatic if parsing fails.</returns>
    public static OxyColor DeserializeColor(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return OxyColors.Automatic;
        }

        if (value.Equals("Automatic", StringComparison.OrdinalIgnoreCase))
        {
            return OxyColors.Automatic;
        }

        if (value.Equals("Undefined", StringComparison.OrdinalIgnoreCase))
        {
            return OxyColors.Undefined;
        }

        try
        {
            return OxyColor.Parse(value);
        }
        catch
        {
            // Fallback to Automatic on parse failure
            return OxyColors.Automatic;
        }
    }

    #endregion

    #region Double Serialization

    /// <summary>
    /// Serializes a double value using invariant culture and full precision.
    /// </summary>
    /// <param name="value">The value to serialize.</param>
    /// <returns>String representation with invariant culture.</returns>
    public static string SerializeDouble(double value)
    {
        if (double.IsNaN(value))
        {
            return "NaN";
        }

        if (double.IsPositiveInfinity(value))
        {
            return "Infinity";
        }

        if (double.IsNegativeInfinity(value))
        {
            return "-Infinity";
        }

        return value.ToString("G17", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Deserializes a double value from a string.
    /// </summary>
    /// <param name="value">The string to deserialize.</param>
    /// <returns>The deserialized double, or NaN if parsing fails.</returns>
    public static double DeserializeDouble(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return double.NaN;
        }

        if (value.Equals("NaN", StringComparison.OrdinalIgnoreCase))
        {
            return double.NaN;
        }

        if (value.Equals("Infinity", StringComparison.OrdinalIgnoreCase))
        {
            return double.PositiveInfinity;
        }

        if (value.Equals("-Infinity", StringComparison.OrdinalIgnoreCase))
        {
            return double.NegativeInfinity;
        }

        if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
        {
            return result;
        }

        return double.NaN;
    }

    #endregion

    #region OxyThickness Serialization

    /// <summary>
    /// Serializes an <see cref="OxyThickness"/> to a string.
    /// </summary>
    /// <param name="thickness">The thickness to serialize.</param>
    /// <returns>String representation (e.g., "1,2,3,4" or "5" for uniform).</returns>
    public static string SerializeThickness(OxyThickness thickness)
    {
        // If uniform, return single value
        if (thickness.Left == thickness.Top &&
            thickness.Left == thickness.Right &&
            thickness.Left == thickness.Bottom)
        {
            return SerializeDouble(thickness.Left);
        }

        return $"{SerializeDouble(thickness.Left)},{SerializeDouble(thickness.Top)}," +
               $"{SerializeDouble(thickness.Right)},{SerializeDouble(thickness.Bottom)}";
    }

    /// <summary>
    /// Deserializes an <see cref="OxyThickness"/> from a string.
    /// </summary>
    /// <param name="value">The string to deserialize.</param>
    /// <returns>The deserialized thickness, or zero thickness if parsing fails.</returns>
    public static OxyThickness DeserializeThickness(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return new OxyThickness(0);
        }

        var parts = value.Split(',');

        try
        {
            if (parts.Length == 1)
            {
                // Uniform thickness
                var uniform = DeserializeDouble(parts[0]);
                return new OxyThickness(uniform);
            }

            if (parts.Length == 4)
            {
                // Individual sides
                var left = DeserializeDouble(parts[0]);
                var top = DeserializeDouble(parts[1]);
                var right = DeserializeDouble(parts[2]);
                var bottom = DeserializeDouble(parts[3]);
                return new OxyThickness(left, top, right, bottom);
            }
        }
        catch
        {
            // Fall through to default
        }

        return new OxyThickness(0);
    }

    #endregion

    #region WPF Brush Serialization

    /// <summary>
    /// Serializes a WPF <see cref="Brush"/> to a string.
    /// </summary>
    /// <param name="brush">The brush to serialize.</param>
    /// <returns>String representation of the brush color, or empty string for null.</returns>
    public static string SerializeBrush(Brush? brush)
    {
        if (brush is null)
        {
            return string.Empty;
        }

        if (brush is SolidColorBrush solidBrush)
        {
            var color = solidBrush.Color;
            return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        // For non-solid brushes, convert to string (limited support)
        return brush.ToString() ?? string.Empty;
    }

    /// <summary>
    /// Deserializes a WPF <see cref="Brush"/> from a string.
    /// </summary>
    /// <param name="value">The string to deserialize.</param>
    /// <returns>The deserialized brush, or null if parsing fails.</returns>
    public static Brush? DeserializeBrush(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        try
        {
            var converter = new BrushConverter();
            return converter.ConvertFromInvariantString(value) as Brush;
        }
        catch
        {
            return null;
        }
    }

    #endregion

    #region FontWeight Serialization

    /// <summary>
    /// Serializes a <see cref="FontWeight"/> to a string.
    /// </summary>
    /// <param name="weight">The font weight to serialize.</param>
    /// <returns>String representation of the font weight.</returns>
    public static string SerializeFontWeight(FontWeight weight)
    {
        var converter = new FontWeightConverter();
        return converter.ConvertToInvariantString(weight) ?? "Normal";
    }

    /// <summary>
    /// Deserializes a <see cref="FontWeight"/> from a string.
    /// </summary>
    /// <param name="value">The string to deserialize.</param>
    /// <returns>The deserialized font weight, or Normal if parsing fails.</returns>
    public static FontWeight DeserializeFontWeight(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return FontWeights.Normal;
        }

        try
        {
            var converter = new FontWeightConverter();
            return (FontWeight)(converter.ConvertFromInvariantString(value) ?? FontWeights.Normal);
        }
        catch
        {
            return FontWeights.Normal;
        }
    }

    #endregion

    #region Boolean Serialization

    /// <summary>
    /// Serializes a boolean value to a string.
    /// </summary>
    /// <param name="value">The boolean value.</param>
    /// <returns>"true" or "false".</returns>
    public static string SerializeBoolean(bool value) => value.ToString(CultureInfo.InvariantCulture);

    /// <summary>
    /// Deserializes a boolean value from a string.
    /// </summary>
    /// <param name="value">The string to deserialize.</param>
    /// <returns>The deserialized boolean, or false if parsing fails.</returns>
    public static bool DeserializeBoolean(string value)
    {
        if (bool.TryParse(value, out var result))
        {
            return result;
        }

        return false;
    }

    #endregion
}
