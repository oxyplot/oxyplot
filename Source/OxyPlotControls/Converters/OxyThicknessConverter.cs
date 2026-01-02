using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using OxyPlot;

namespace OxyPlotControls.Converters;

/// <summary>
/// Converts between <see cref="OxyThickness"/> and WPF <see cref="Thickness"/>.
/// </summary>
public class OxyThicknessConverter : IValueConverter
{
    /// <summary>
    /// Converts an <see cref="OxyThickness"/> to a WPF <see cref="Thickness"/>.
    /// </summary>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        try
        {
            return value switch
            {
                OxyThickness oxy => new Thickness(oxy.Left, oxy.Top, oxy.Right, oxy.Bottom),
                Thickness wpf => wpf, // Pass through if already WPF thickness
                double uniform => new Thickness(uniform), // Uniform thickness
                _ => new Thickness(0)
            };
        }
        catch
        {
            return new Thickness(0);
        }
    }

    /// <summary>
    /// Converts a WPF <see cref="Thickness"/> back to an <see cref="OxyThickness"/>.
    /// </summary>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        try
        {
            return value switch
            {
                Thickness wpf => new OxyThickness(wpf.Left, wpf.Top, wpf.Right, wpf.Bottom),
                OxyThickness oxy => oxy, // Pass through if already OxyThickness
                double uniform => new OxyThickness(uniform), // Uniform thickness
                _ => new OxyThickness(0)
            };
        }
        catch
        {
            return new OxyThickness(0);
        }
    }
}

/// <summary>
/// Converts between <see cref="OxyThickness"/> and a single <see cref="double"/> value.
/// Used for uniform thickness (same on all sides).
/// </summary>
public class OxyThicknessToDoubleConverter : IValueConverter
{
    /// <summary>
    /// Converts an <see cref="OxyThickness"/> to a <see cref="double"/> by taking the left value.
    /// </summary>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        try
        {
            return value switch
            {
                OxyThickness oxy => oxy.Left, // Use left value for uniform thickness
                Thickness wpf => wpf.Left,
                double d => d,
                _ => 0.0
            };
        }
        catch
        {
            return 0.0;
        }
    }

    /// <summary>
    /// Converts a <see cref="double"/> back to an <see cref="OxyThickness"/> with uniform values.
    /// </summary>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        try
        {
            if (value is double d)
            {
                return targetType == typeof(OxyThickness)
                    ? new OxyThickness(d)
                    : (object)new Thickness(d);
            }

            return targetType == typeof(OxyThickness)
                ? new OxyThickness(0)
                : new Thickness(0);
        }
        catch
        {
            return targetType == typeof(OxyThickness)
                ? new OxyThickness(0)
                : new Thickness(0);
        }
    }
}
