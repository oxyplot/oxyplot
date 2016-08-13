// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThicknessConverter.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Converts from <see cref="Thickness" /> to the maximum thicknesses.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Avalonia.Converters
{
    using global::Avalonia;
    using global::Avalonia.Markup;
    using System;
    using System.Globalization;

    /// <summary>
    /// Converts from <see cref="Thickness" /> to the maximum thicknesses.
    /// </summary>
    /// <remarks>This is used in the <see cref="TrackerControl" /> to convert BorderThickness properties to Path.StrokeThickness (double).
    /// The maximum thickness value is used.</remarks>
    public class ThicknessConverter : IValueConverter
    {
        public static readonly ThicknessConverter Instance = new ThicknessConverter();

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns <c>null</c>, the valid <c>null</c> value is used.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Thickness)
            {
                var t = (Thickness)value;
                if (targetType == typeof(double))
                {
                    return Math.Max(Math.Max(t.Left, t.Right), Math.Max(t.Top, t.Bottom));
                }
            }

            return value;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns <c>null</c>, the valid <c>null</c> value is used.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}