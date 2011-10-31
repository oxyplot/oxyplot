// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyCursor.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// The oxy cursor.
    /// </summary>
    public enum OxyCursor
    {
        /// <summary>
        ///   The arrow cursor.
        /// </summary>
        Arrow, 

        /// <summary>
        ///   The four-headed sizing Cursor, which consists of four joined arrows that point north, south, east, and west.
        /// </summary>
        SizeAll, 

        /// <summary>
        ///   The two-headed northwest/southeast sizing cursor.
        /// </summary>
        SizeNWSE, 

        /// <summary>
        ///   The crosshair cursor.
        /// </summary>
        Cross, 

        /// <summary>
        ///   The invisible cursor.
        /// </summary>
        None
    }
}