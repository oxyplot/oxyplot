// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Forms.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Initializes OxyPlot renderers for use with Xamarin.Forms.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.XamarinForms.iOS
{
    /// <summary>
    /// Initializes OxyPlot renderers for use with Xamarin.Forms.
    /// </summary>
    public static class Forms
    {
        /// <summary>
        /// Initializes OxyPlot for Xamarin.Forms.
        /// </summary>
        /// <remarks>This method must be called before Forms.Init().</remarks>
        public static void Init()
        {
            // Just bring this assembly into the current appdomain.
            // Forms.Init() should now find it!
        }
    }
}
