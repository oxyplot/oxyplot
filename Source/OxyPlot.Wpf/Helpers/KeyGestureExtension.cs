// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyGestureExtension.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System;
    using System.Windows.Input;
    using System.Windows.Markup;

    /// <summary>
    /// Markup extension for key and mouse gestures.
    /// </summary>
    public class KeyGestureExtension : MarkupExtension
    {
        #region Constants and Fields

        /// <summary>
        ///   The gesture.
        /// </summary>
        private readonly KeyGesture gesture;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyGestureExtension"/> class.
        /// </summary>
        /// <param name="gesture">
        /// The gesture.
        /// </param>
        public KeyGestureExtension(string gesture)
        {
            var kgc = new KeyGestureConverter();
            this.gesture = kgc.ConvertFromString(gesture) as KeyGesture;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Provides the value.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        /// <returns>
        /// The provide value.
        /// </returns>
        public override object ProvideValue(IServiceProvider service)
        {
            return this.gesture;
        }

        #endregion
    }
}