// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThicknessConverter.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Converts Thickness to double.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Windows
{
    using System;

    using global::Windows.UI.Xaml;
    using global::Windows.UI.Xaml.Data;

    /// <summary>
    /// Converts Thickness to double.
    /// </summary>
    /// <remarks>This is used to convert BorderThickness properties to Path.StrokeThickness (double).
    /// The maximum thickness value is used.</remarks>
    public class ThicknessConverter : IValueConverter
    {
        /// <summary>
        /// Converts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="language">The language.</param>
        /// <returns>The maximum value of the thickness.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is Thickness)
            {
                var t = (Thickness)value;
                if (targetType == typeof(double))
                {
                    return Math.Max(Math.Max(t.Left, t.Right), Math.Max(t.Top, t.Bottom));
                }
            }

            return null;
        }

        /// <summary>
        /// Converts back.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="language">The language.</param>
        /// <returns>Not implemented.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}