// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EdgeRenderingMode.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// <summary>
//   Defines the edge rendering mode.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Enumerates the available edge rendering modes. This gives an indication to the renderer which tradoffs to make between sharpness, speed and geomitric accuracy
    /// when rendering shapes like lines, polygons, ellipses etc.
    /// </summary>
    public enum EdgeRenderingMode
    {
        /// <summary>
        /// Indicates that a rendering mode should be chosen automatically by the <see cref="PlotElement"/>. The renderer will treat this equivalently to <see cref="Adaptive"/>.
        /// </summary>
        Automatic,

        /// <summary>
        /// The renderer will try to find the best rendering mode depending on the rendered shape.
        /// </summary>
        Adaptive,

        /// <summary>
        /// The renderer will try to maximise the sharpness of edges. To that end, it may disable Anti-Aliasing for some lines or snap the position and stroke thickness 
        /// of rendered elements to device pixels.
        /// </summary>
        PreferSharpness,

        /// <summary>
        /// The renderer will try to maximise the rendering speed. To that end, it may disable Anti-Aliasing.
        /// </summary>
        PreferSpeed,

        /// <summary>
        /// The renderer will try to maximise geometric accuracy. To that end, it may enable Anti-Aliasing even for straight lines.
        /// </summary>
        PreferGeometricAccuracy,
    }
}
