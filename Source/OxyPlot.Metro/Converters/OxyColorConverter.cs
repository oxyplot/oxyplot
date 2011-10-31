// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyColorConverter.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Metro
{
    using System;
    using System.Globalization;
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Media;

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
        public object Convert(object value, string targetType, object parameter, string language)
        {
            if (value is OxyColor)
            {
                var color = (OxyColor)value;
                if (targetType == typeof(Color).FullName)
                {
                    return color.ToColor();
                }

                if (targetType == typeof(Brush).FullName)
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
        public object ConvertBack(object value, string targetType, object parameter, string language)
        {
            if (targetType == typeof(OxyColor).FullName)
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