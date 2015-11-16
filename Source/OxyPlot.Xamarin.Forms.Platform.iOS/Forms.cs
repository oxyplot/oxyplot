// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Forms.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Initializes OxyPlot renderers for use with Xamarin.Forms.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Xamarin.Forms.Platform.iOS
{
    using System;

    /// <summary>
    /// Initializes OxyPlot renderers for use with Xamarin.Forms.
    /// </summary>
    public static class Forms
    {
        /// <summary>
        /// Initializes OxyPlot for Xamarin.Forms.
        /// </summary>
        [Obsolete("Use PlotViewRenderer.Init() instead.")]
        public static void Init()
        {
            PlotViewRenderer.Init();
        }
    }
}
