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

    #region Integer Serialization

    /// <summary>
    /// Serializes an integer value to a string.
    /// </summary>
    /// <param name="value">The integer value.</param>
    /// <returns>String representation.</returns>
    public static string SerializeInt(int value) => value.ToString(CultureInfo.InvariantCulture);

    /// <summary>
    /// Deserializes an integer value from a string.
    /// </summary>
    /// <param name="value">The string to deserialize.</param>
    /// <returns>The deserialized integer, or 0 if parsing fails.</returns>
    public static int DeserializeInt(string value)
    {
        if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
        {
            return result;
        }

        return 0;
    }

    #endregion

    #region Enum Serialization

    /// <summary>
    /// Serializes an enum value to its string name.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="value">The enum value.</param>
    /// <returns>String name of the enum value.</returns>
    public static string SerializeEnum<TEnum>(TEnum value) where TEnum : struct, Enum
    {
        return value.ToString();
    }

    /// <summary>
    /// Deserializes an enum value from a string.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="value">The string to deserialize.</param>
    /// <param name="defaultValue">Default value if parsing fails.</param>
    /// <returns>The deserialized enum value, or default if parsing fails.</returns>
    public static TEnum DeserializeEnum<TEnum>(string value, TEnum defaultValue) where TEnum : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return defaultValue;
        }

        if (Enum.TryParse<TEnum>(value, ignoreCase: true, out var result))
        {
            return result;
        }

        return defaultValue;
    }

    #endregion

    #region DataPoint Serialization

    /// <summary>
    /// Serializes a <see cref="DataPoint"/> to a string.
    /// </summary>
    /// <param name="point">The data point.</param>
    /// <returns>String in format "X, Y".</returns>
    public static string SerializeDataPoint(DataPoint point)
    {
        return $"{SerializeDouble(point.X)}, {SerializeDouble(point.Y)}";
    }

    /// <summary>
    /// Deserializes a <see cref="DataPoint"/> from a string.
    /// </summary>
    /// <param name="value">The string to deserialize.</param>
    /// <returns>The deserialized DataPoint, or Undefined if parsing fails.</returns>
    public static DataPoint DeserializeDataPoint(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return DataPoint.Undefined;
        }

        var parts = value.Split(',', StringSplitOptions.TrimEntries);
        if (parts.Length != 2)
        {
            return DataPoint.Undefined;
        }

        var x = DeserializeDouble(parts[0]);
        var y = DeserializeDouble(parts[1]);

        return new DataPoint(x, y);
    }

    #endregion

    #region ScreenPoint Serialization

    /// <summary>
    /// Serializes a <see cref="ScreenPoint"/> to a string.
    /// </summary>
    /// <param name="point">The screen point.</param>
    /// <returns>String in format "X, Y".</returns>
    public static string SerializeScreenPoint(ScreenPoint point)
    {
        return $"{SerializeDouble(point.X)}, {SerializeDouble(point.Y)}";
    }

    /// <summary>
    /// Deserializes a <see cref="ScreenPoint"/> from a string.
    /// </summary>
    /// <param name="value">The string to deserialize.</param>
    /// <returns>The deserialized ScreenPoint, or Undefined if parsing fails.</returns>
    public static ScreenPoint DeserializeScreenPoint(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return ScreenPoint.Undefined;
        }

        var parts = value.Split(',', StringSplitOptions.TrimEntries);
        if (parts.Length != 2)
        {
            return ScreenPoint.Undefined;
        }

        var x = DeserializeDouble(parts[0]);
        var y = DeserializeDouble(parts[1]);

        return new ScreenPoint(x, y);
    }

    #endregion

    #region ScreenVector Serialization

    /// <summary>
    /// Serializes a <see cref="ScreenVector"/> to a string.
    /// </summary>
    /// <param name="vector">The screen vector.</param>
    /// <returns>String in format "X, Y".</returns>
    public static string SerializeScreenVector(ScreenVector vector)
    {
        return $"{SerializeDouble(vector.X)}, {SerializeDouble(vector.Y)}";
    }

    /// <summary>
    /// Deserializes a <see cref="ScreenVector"/> from a string.
    /// </summary>
    /// <param name="value">The string to deserialize.</param>
    /// <returns>The deserialized ScreenVector, or zero vector if parsing fails.</returns>
    public static ScreenVector DeserializeScreenVector(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return new ScreenVector(0, 0);
        }

        var parts = value.Split(',', StringSplitOptions.TrimEntries);
        if (parts.Length != 2)
        {
            return new ScreenVector(0, 0);
        }

        var x = DeserializeDouble(parts[0]);
        var y = DeserializeDouble(parts[1]);

        return new ScreenVector(x, y);
    }

    #endregion

    #region LineStyle Serialization

    /// <summary>
    /// Serializes a <see cref="LineStyle"/> to a string.
    /// </summary>
    public static string SerializeLineStyle(LineStyle style) => SerializeEnum(style);

    /// <summary>
    /// Deserializes a <see cref="LineStyle"/> from a string.
    /// </summary>
    public static LineStyle DeserializeLineStyle(string value) => DeserializeEnum(value, LineStyle.Solid);

    #endregion
}
