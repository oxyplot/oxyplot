using System;
using System.Globalization;
using System.Windows.Data;

namespace OxyPlotControls.Converters;

/// <summary>
/// Converts font size values, handling NaN as "use default font size".
/// This replaces the buggy VB converter that incorrectly treated 12 as a sentinel value.
/// </summary>
/// <remarks>
/// The old VB converter had a critical bug where it assumed any font size of exactly 12
/// should be converted back to NaN. This meant users could never set a font size to 12.
/// This new converter uses a configurable default value and doesn't use sentinel values.
/// </remarks>
public class OxyFontSizeConverter : IValueConverter
{
    /// <summary>
    /// Gets or sets the default font size to use when the value is NaN, infinity, or null.
    /// Default is 12.0.
    /// </summary>
    public double DefaultFontSize { get; set; } = 12.0;

    /// <summary>
    /// Gets or sets whether to display NaN as the default value (true) or as an empty string (false).
    /// Default is true.
    /// </summary>
    public bool DisplayDefaultForNaN { get; set; } = true;

    /// <summary>
    /// Converts a font size value to a displayable value, replacing NaN with the default.
    /// </summary>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        try
        {
            // Try to get the default from parameter if provided
            var defaultSize = DefaultFontSize;
            if (parameter is string paramStr && double.TryParse(paramStr, NumberStyles.Float, CultureInfo.InvariantCulture, out var paramDefault))
            {
                defaultSize = paramDefault;
            }
            else if (parameter is double paramDouble)
            {
                defaultSize = paramDouble;
            }

            if (value is double d)
            {
                // Return default for NaN or infinity
                if (double.IsNaN(d) || double.IsInfinity(d))
                {
                    return DisplayDefaultForNaN ? defaultSize : (object?)null;
                }

                return d;
            }

            // Return default for null or invalid types
            return DisplayDefaultForNaN ? defaultSize : (object?)null;
        }
        catch
        {
            return DisplayDefaultForNaN ? DefaultFontSize : null;
        }
    }

    /// <summary>
    /// Converts a font size value back to the source.
    /// Unlike the buggy VB version, this does NOT treat any specific value as NaN.
    /// Returns NaN only when explicitly requested or when conversion fails.
    /// </summary>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        try
        {
            if (value is null)
            {
                return double.NaN;
            }

            if (value is double d)
            {
                // Allow any valid positive number (including 12!)
                return d > 0 ? d : double.NaN;
            }

            if (value is string str)
            {
                // Try to parse the string
                if (string.IsNullOrWhiteSpace(str))
                {
                    return double.NaN;
                }

                if (double.TryParse(str, NumberStyles.Float, culture, out var parsed))
                {
                    return parsed > 0 ? parsed : double.NaN;
                }

                return double.NaN;
            }

            // Try to convert other numeric types
            if (value is int i)
            {
                return i > 0 ? (double)i : double.NaN;
            }

            return double.NaN;
        }
        catch
        {
            return double.NaN;
        }
    }
}

/// <summary>
/// Helper converter for font sizes that should never be NaN (always shows default).
/// </summary>
public class RequiredFontSizeConverter : OxyFontSizeConverter
{
    public RequiredFontSizeConverter()
    {
        DisplayDefaultForNaN = true;
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var result = base.ConvertBack(value, targetType, parameter, culture);

        // Never return NaN for required fields
        if (result is double d && double.IsNaN(d))
        {
            return DefaultFontSize;
        }

        return result;
    }
}
