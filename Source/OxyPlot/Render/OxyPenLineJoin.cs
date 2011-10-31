// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyPenLineJoin.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Pen line join.
    /// </summary>
    public enum OxyPenLineJoin
    {
        /// <summary>
        ///   Line joins use regular angular vertices.
        /// </summary>
        Miter, 

        /// <summary>
        ///   Line joins use rounded vertices.
        /// </summary>
        Round, 

        /// <summary>
        ///   Line joins use beveled vertices.
        /// </summary>
        Bevel
    }
}