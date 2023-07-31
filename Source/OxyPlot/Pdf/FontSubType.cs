// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FontSubType.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Defines the page size.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Defines the page size.
    /// </summary>
    public enum PageSize
    {
        /// <summary>
        /// ISO A4 size (595pt x 842pt).
        /// </summary>
        A4,

        /// <summary>
        /// ISO A3 size (842pt x 1190pt).
        /// </summary>
        A3,

        /// <summary>
        /// American letter size (612pt x 792pt).
        /// </summary>
        Letter
    }

    /// <summary>
    /// Defines the page orientation.
    /// </summary>
    public enum PageOrientation
    {
        /// <summary>
        /// Portrait orientation (where the height is greater than the width).
        /// </summary>
        Portrait,

        /// <summary>
        /// Landscape orientation (where the width is greater than the height).
        /// </summary>
        Landscape
    }

    /// <summary>
    /// Defines the line cap type.
    /// </summary>
    public enum LineCap
    {
        /// <summary>
        /// Butt cap. The stroke is squared off at the endpoint of the path. There is no projection beyond the end of the path.
        /// </summary>
        Butt = 0,

        /// <summary>
        /// Round cap. A semicircular arc with a diameter equal to the line width is drawn around the endpoint and filled in.
        /// </summary>
        Round = 1,

        /// <summary>
        /// Projecting square cap. The stroke continues beyond the endpoint of the path for a distance equal to half the line width and is squared off.
        /// </summary>
        ProjectingSquare = 2
    }

    /// <summary>
    /// Defines the color space.
    /// </summary>
    public enum ColorSpace
    {
        /// <summary>
        /// The colors are defined by intensities of red, green and blue light, the three additive primary colors used in displays.
        /// </summary>
        DeviceRGB
    }

    /// <summary>
    /// Defines the font encoding.
    /// </summary>
    public enum FontEncoding
    {
        /// <summary>
        /// Windows Code Page 1252, often called the “Windows ANSI” encoding. This is the standard Windows encoding for Latin text in
        /// Western writing systems. PDF has a predefined encoding named WinAnsiEncoding that can be used with both Type 1 and TrueType fonts.
        /// </summary>
        WinAnsiEncoding
    }

    /// <summary>
    /// Defines the font subtype
    /// </summary>
    public enum FontSubType
    {
        /// <summary>
        /// Adobe type 1 font.
        /// </summary>
        Type1,

        /// <summary>
        /// TrueType font.
        /// </summary>
        TrueType
    }
}