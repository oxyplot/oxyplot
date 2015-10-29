// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Forms.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Initializes OxyPlot renderers for use with Xamarin.Forms.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Xamarin.Forms.Platform.iOS.Classic
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
        /// <remarks>This method must be called before a <see cref="T:PlotView" /> is used.</remarks>
        [Obsolete("Use PlotViewRenderer.Init() instead.")]
        public static void Init()
        {
            PlotViewRenderer.Init();
        }
    }
}
