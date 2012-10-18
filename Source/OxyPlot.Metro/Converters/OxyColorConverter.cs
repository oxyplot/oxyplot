// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyColorConverter.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Metro
{
    using System;
    using Windows.UI;
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Media;

    /// <summary>
    /// Converts from OxyPlot colors to Windows.UI.Color and vice versa.
    /// </summary>
    public class OxyColorConverter : IValueConverter
    {
        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="targetType">
        /// The target type.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="language">
        /// The language.
        /// </param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is OxyColor)
            {
                var color = (OxyColor)value;
                if (targetType == typeof(Color))
                {
                    return color.ToColor();
                }

                if (targetType == typeof(Brush))
                {
                    return color.ToBrush();
                }
            }

            return null;
        }

        /// <summary>
        /// Converts back.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="language">The language.</param>
        /// <returns>
        /// The converted value.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (targetType == typeof(OxyColor))
            {
                if (value is Color)
                {
                    var color = (Color)value;
                    return OxyColor.FromArgb(color.A, color.R, color.G, color.B);
                }

                if (value is SolidColorBrush)
                {
                    var brush = (SolidColorBrush)value;
                    Color color = brush.Color;
                    return OxyColor.FromArgb(color.A, color.R, color.G, color.B);
                }
            }

            return null;
        }
    }
}