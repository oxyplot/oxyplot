// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyColorConverter.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Silverlight
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    // [ValueConversion(typeof(OxyColor), typeof(Rect))]
    /// <summary>
    /// The oxy color converter.
    /// </summary>
    public class OxyColorConverter : IValueConverter
    {
        #region Public Methods

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
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// The convert.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
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
        /// The convert back.
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
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// The convert back.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
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

        #endregion
    }
}