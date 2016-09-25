namespace OxyPlot
{
    /// <summary>
    /// Defines interpolation algorithm for smoothing a line.
    /// </summary>
    public enum InterpolationAlgorithm
    {
        /// <summary>
        /// Canonical spline, also known as Cardinal spline.
        /// </summary>
        Canonical,

        /// <summary>
        /// Centripetal Catmull–Rom spline.
        /// </summary>
        CatmullRom,

        /// <summary>
        /// Uniform Catmull–Rom spline.
        /// </summary>
        UniformCatmullRom,

        /// <summary>
        /// Chordal Catmull–Rom spline.
        /// </summary>
        ChordalCatmullRom
    }
}