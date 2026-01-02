using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using OxyPlot;

namespace OxyPlotControls.Converters;

/// <summary>
/// Converts between <see cref="OxyColor"/> and WPF <see cref="Color"/> or <see cref="SolidColorBrush"/>.
/// Handles automatic colors and provides proper fallback values.
/// </summary>
/// <remarks>
/// This consolidated converter replaces multiple duplicate converters from the VB codebase.
/// It handles null values, automatic colors, and type mismatches gracefully.
/// </remarks>
public class OxyColorConverter : IValueConverter
{
    /// <summary>
    /// Gets or sets the default color to use when the OxyColor is automatic.
    /// Can be specified as converter parameter (e.g., "#FF000000" for black).
    /// </summary>
    public string DefaultColorHex { get; set; } = "#FF000000";

    /// <summary>
    /// Gets or sets whether to return a <see cref="SolidColorBrush"/> instead of a <see cref="Color"/>.
    /// </summary>
    public bool ReturnBrush { get; set; } = false;

    /// <summary>
    /// Converts an <see cref="OxyColor"/> to a WPF <see cref="Color"/> or <see cref="SolidColorBrush"/>.
    /// </summary>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        try
        {
            // Determine the default color (from parameter or property)
            var defaultHex = parameter as string ?? DefaultColorHex;
            var defaultColor = (Color)ColorConverter.ConvertFromString(defaultHex);

            Color resultColor;

            if (value is OxyColor oxyColor)
            {
                // Handle automatic color
                if (oxyColor.IsAutomatic())
                {
                    resultColor = defaultColor;
                }
                else if (oxyColor.IsUndefined() || oxyColor.IsInvisible())
                {
                    resultColor = Colors.Transparent;
                }
                else
                {
                    resultColor = Color.FromArgb(oxyColor.A, oxyColor.R, oxyColor.G, oxyColor.B);
                }
            }
            else if (value is Color wpfColor)
            {
                // Already a WPF color, pass through
                resultColor = wpfColor;
            }
            else if (value is null)
            {
                resultColor = defaultColor;
            }
            else
            {
                // Unsupported type
                resultColor = defaultColor;
            }

            // Return as brush or color based on ReturnBrush property or target type
            var shouldReturnBrush = ReturnBrush ||
                                   targetType == typeof(Brush) ||
                                   targetType == typeof(SolidColorBrush);

            return shouldReturnBrush ? new SolidColorBrush(resultColor) : resultColor;
        }
        catch
        {
            // Fallback on any error
            return ReturnBrush ? Brushes.Black : Colors.Black;
        }
    }

    /// <summary>
    /// Converts a WPF <see cref="Color"/> or <see cref="SolidColorBrush"/> back to an <see cref="OxyColor"/>.
    /// </summary>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        try
        {
            Color? color = value switch
            {
                Color c => c,
                SolidColorBrush brush => brush.Color,
                Brush brush when brush is SolidColorBrush solidBrush => solidBrush.Color,
                _ => null
            };

            if (color.HasValue)
            {
                var c = color.Value;
                return OxyColor.FromArgb(c.A, c.R, c.G, c.B);
            }

            // Return automatic for null/unsupported types
            return OxyColors.Automatic;
        }
        catch
        {
            return OxyColors.Automatic;
        }
    }
}

/// <summary>
/// Specialized converter for marker fill colors.
/// Uses transparent as default instead of black.
/// </summary>
public class OxyMarkerFillConverter : OxyColorConverter
{
    public OxyMarkerFillConverter()
    {
        DefaultColorHex = "#00FFFFFF"; // Transparent
    }
}

/// <summary>
/// Specialized converter for marker stroke colors.
/// Uses automatic color behavior.
/// </summary>
public class OxyMarkerStrokeConverter : OxyColorConverter
{
    public OxyMarkerStrokeConverter()
    {
        DefaultColorHex = "#FF000000"; // Black
    }
}
