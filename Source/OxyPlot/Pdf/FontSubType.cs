// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FontSubType.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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
    /// Defines the shape that joins two lines or segments.
    /// </summary>
    public enum LineJoin
    {
        /// <summary>
        /// Miter join. The outer edges of the strokes for the two segments are extended until they meet at an angle, as in a picture frame. If the segments meet at too sharp an angle (as defined by the miter limit parameter—see “Miter Limit,” above), a bevel join is used instead.
        /// </summary>
        Miter = 0,

        /// <summary>
        /// Round join. An arc of a circle with a diameter equal to the line width is drawn around the point where the two segments meet, connecting the outer edges of the strokes for the two segments. This pieslice-shaped figure is filled in, producing a rounded corner.
        /// </summary>
        Round = 1,

        /// <summary>
        /// Bevel join. The two segments are finished with butt caps (see “Line Cap Style” on page 216) and the resulting notch beyond the ends of the segments is filled with a triangle.
        /// </summary>
        Bevel = 2
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