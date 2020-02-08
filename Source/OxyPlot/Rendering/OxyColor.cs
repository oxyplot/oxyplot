// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyColor.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Describes a color in terms of alpha, red, green, and blue channels.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Describes a color in terms of alpha, red, green, and blue channels.
    /// </summary>
    public struct OxyColor : ICodeGenerating, IEquatable<OxyColor>
    {
        /// <summary>
        /// The red component.
        /// </summary>
        private readonly byte r;

        /// <summary>
        /// The green component.
        /// </summary>
        private readonly byte g;

        /// <summary>
        /// The blue component.
        /// </summary>
        private readonly byte b;

        /// <summary>
        /// The alpha component.
        /// </summary>
        private readonly byte a;

        /// <summary>
        /// Initializes a new instance of the <see cref="OxyColor"/> struct.
        /// </summary>
        /// <param name="a">The alpha value.</param>
        /// <param name="r">The red value.</param>
        /// <param name="g">The green value.</param>
        /// <param name="b">The blue value.</param>
        private OxyColor(byte a, byte r, byte g, byte b)
        {
            this.a = a;
            this.r = r;
            this.g = g;
            this.b = b;
        }

        /// <summary>
        /// Gets the alpha value.
        /// </summary>
        /// <value>The alpha value.</value>
        public byte A
        {
            get
            {
                return this.a;
            }
        }

        /// <summary>
        /// Gets the blue value.
        /// </summary>
        /// <value>The blue value.</value>
        public byte B
        {
            get
            {
                return this.b;
            }
        }

        /// <summary>
        /// Gets the green value.
        /// </summary>
        /// <value>The green value.</value>
        public byte G
        {
            get
            {
                return this.g;
            }
        }

        /// <summary>
        /// Gets the red value.
        /// </summary>
        /// <value>The red value.</value>
        public byte R
        {
            get
            {
                return this.r;
            }
        }

        /// <summary>
        /// Parse a string.
        /// </summary>
        /// <param name="value">The string in the format <c>"#FFFFFF00"</c> or <c>"255,200,180,50"</c>.</param>
        /// <returns>The parsed color.</returns>
        /// <exception cref="System.FormatException">Invalid format.</exception>
        public static OxyColor Parse(string value)
        {
            if (value == null || string.Equals(value, "none", StringComparison.OrdinalIgnoreCase))
            {
                return OxyColors.Undefined;
            }

            if (string.Equals(value, "auto", StringComparison.OrdinalIgnoreCase))
            {
                return OxyColors.Automatic;
            }

            value = value.Trim();
            if (value.StartsWith("#"))
            {
                value = value.Trim('#');
                if (value.Length == 3)
                {
                    // replicate digits
                    value = string.Format("{0}{0}{1}{1}{2}{2}", value[0], value[1], value[2]);
                }

                var u = uint.Parse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                if (value.Length < 8)
                {
                    // alpha value was not specified
                    u += 0xFF000000;
                }

                return FromUInt32(u);
            }

            var values = value.Split(',');
            if (values.Length < 3 || values.Length > 4)
            {
                throw new FormatException("Invalid format.");
            }

            var i = 0;

            byte alpha = 255;
            if (values.Length > 3)
            {
                alpha = byte.Parse(values[i++], CultureInfo.InvariantCulture);
            }

            var red = byte.Parse(values[i++], CultureInfo.InvariantCulture);
            var green = byte.Parse(values[i++], CultureInfo.InvariantCulture);
            var blue = byte.Parse(values[i], CultureInfo.InvariantCulture);
            return FromArgb(alpha, red, green, blue);
        }

        /// <summary>
        /// Calculates the difference between two <see cref="OxyColor" />s
        /// </summary>
        /// <param name="c1">The first color.</param>
        /// <param name="c2">The second color.</param>
        /// <returns>L2-norm in ARGB space</returns>
        public static double ColorDifference(OxyColor c1, OxyColor c2)
        {
            // http://en.wikipedia.org/wiki/OxyColor_difference
            // http://mathworld.wolfram.com/L2-Norm.html
            double dr = (c1.R - c2.R) / 255.0;
            double dg = (c1.G - c2.G) / 255.0;
            double db = (c1.B - c2.B) / 255.0;
            double da = (c1.A - c2.A) / 255.0;
            double e = (dr * dr) + (dg * dg) + (db * db) + (da * da);
            return Math.Sqrt(e);
        }

        /// <summary>
        /// Convert an <see cref="uint" /> to a <see cref="OxyColor" />.
        /// </summary>
        /// <param name="color">The unsigned integer color value.</param>
        /// <returns>The <see cref="OxyColor" />.</returns>
        public static OxyColor FromUInt32(uint color)
        {
            var a = (byte)(color >> 24);
            var r = (byte)(color >> 16);
            var g = (byte)(color >> 8);
            var b = (byte)(color >> 0);
            return FromArgb(a, r, g, b);
        }

        /// <summary>
        /// Creates a OxyColor from the specified HSV array.
        /// </summary>
        /// <param name="hsv">The HSV value array.</param>
        /// <returns>A OxyColor.</returns>
        public static OxyColor FromHsv(double[] hsv)
        {
            if (hsv.Length != 3)
            {
                throw new InvalidOperationException("Wrong length of hsv array.");
            }

            return FromHsv(hsv[0], hsv[1], hsv[2]);
        }

        /// <summary>
        /// Converts from HSV to <see cref="OxyColor" />
        /// </summary>
        /// <param name="hue">The hue value [0,1]</param>
        /// <param name="sat">The saturation value [0,1]</param>
        /// <param name="val">The intensity value [0,1]</param>
        /// <returns>The <see cref="OxyColor" />.</returns>
        /// <remarks>See <a href="http://en.wikipedia.org/wiki/HSL_Color_space">Wikipedia</a>.</remarks>
        public static OxyColor FromHsv(double hue, double sat, double val)
        {
            double g, b;
            double r = g = b = 0;

            if (sat.Equals(0))
            {
                // Gray scale
                r = g = b = val;
            }
            else
            {
                if (hue.Equals(1))
                {
                    hue = 0;
                }

                hue *= 6.0;
                var i = (int)Math.Floor(hue);
                double f = hue - i;
                double aa = val * (1 - sat);
                double bb = val * (1 - (sat * f));
                double cc = val * (1 - (sat * (1 - f)));
                switch (i)
                {
                    case 0:
                        r = val;
                        g = cc;
                        b = aa;
                        break;
                    case 1:
                        r = bb;
                        g = val;
                        b = aa;
                        break;
                    case 2:
                        r = aa;
                        g = val;
                        b = cc;
                        break;
                    case 3:
                        r = aa;
                        g = bb;
                        b = val;
                        break;
                    case 4:
                        r = cc;
                        g = aa;
                        b = val;
                        break;
                    case 5:
                        r = val;
                        g = aa;
                        b = bb;
                        break;
                }
            }

            return FromRgb((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
        }

        /// <summary>
        /// Calculate the difference in hue between two <see cref="OxyColor" />s.
        /// </summary>
        /// <param name="c1">The first color.</param>
        /// <param name="c2">The second color.</param>
        /// <returns>The hue difference.</returns>
        public static double HueDifference(OxyColor c1, OxyColor c2)
        {
            var hsv1 = c1.ToHsv();
            var hsv2 = c2.ToHsv();
            double dh = hsv1[0] - hsv2[0];

            // clamp to [-0.5,0.5]
            if (dh > 0.5)
            {
                dh -= 1.0;
            }

            if (dh < -0.5)
            {
                dh += 1.0;
            }

            double e = dh * dh;
            return Math.Sqrt(e);
        }

        /// <summary>
        /// Creates a color defined by an alpha value and another color.
        /// </summary>
        /// <param name="a">Alpha value.</param>
        /// <param name="color">The original color.</param>
        /// <returns>A color.</returns>
        public static OxyColor FromAColor(byte a, OxyColor color)
        {
            return FromArgb(a, color.R, color.G, color.B);
        }

        /// <summary>
        /// Creates a color from the specified ARGB values.
        /// </summary>
        /// <param name="a">The alpha value.</param>
        /// <param name="r">The red value.</param>
        /// <param name="g">The green value.</param>
        /// <param name="b">The blue value.</param>
        /// <returns>A color.</returns>
        public static OxyColor FromArgb(byte a, byte r, byte g, byte b)
        {
            return new OxyColor(a, r, g, b);
        }

        /// <summary>
        /// Creates a new <see cref="OxyColor" /> structure from the specified RGB values.
        /// </summary>
        /// <param name="r">The red value.</param>
        /// <param name="g">The green value.</param>
        /// <param name="b">The blue value.</param>
        /// <returns>A <see cref="OxyColor" /> structure with the specified values and an alpha channel value of 1.</returns>
        public static OxyColor FromRgb(byte r, byte g, byte b)
        {
            // ReSharper restore InconsistentNaming
            return new OxyColor(255, r, g, b);
        }

        /// <summary>
        /// Interpolates the specified colors.
        /// </summary>
        /// <param name="color1">The color1.</param>
        /// <param name="color2">The color2.</param>
        /// <param name="t">The t.</param>
        /// <returns>The interpolated color</returns>
        public static OxyColor Interpolate(OxyColor color1, OxyColor color2, double t)
        {
            double a = (color1.A * (1 - t)) + (color2.A * t);
            double r = (color1.R * (1 - t)) + (color2.R * t);
            double g = (color1.G * (1 - t)) + (color2.G * t);
            double b = (color1.B * (1 - t)) + (color2.B * t);
            return FromArgb((byte)a, (byte)r, (byte)g, (byte)b);
        }

        /// <summary>
        /// Determines whether the specified colors are equal to each other.
        /// </summary>
        /// <param name="first">The first color.</param>
        /// <param name="second">The second color.</param>
        /// <returns><c>true</c> if the two colors are equal; otherwise, <c>false</c> .</returns>
        public static bool operator ==(OxyColor first, OxyColor second)
        {
            return first.Equals(second);
        }

        /// <summary>
        /// Determines whether the specified colors are not equal to each other.
        /// </summary>
        /// <param name="first">The first color.</param>
        /// <param name="second">The second color.</param>
        /// <returns><c>true</c> if the two colors are not equal; otherwise, <c>false</c> .</returns>
        public static bool operator !=(OxyColor first, OxyColor second)
        {
            return !first.Equals(second);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c> .</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != typeof(OxyColor))
            {
                return false;
            }

            return this.Equals((OxyColor)obj);
        }

        /// <summary>
        /// Determines whether the specified <see cref="OxyColor" /> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="OxyColor" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="OxyColor" /> is equal to this instance; otherwise, <c>false</c> .</returns>
        public bool Equals(OxyColor other)
        {
            return other.A == this.A && other.R == this.R && other.G == this.G && other.B == this.B;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = this.A.GetHashCode();
                result = (result * 397) ^ this.R.GetHashCode();
                result = (result * 397) ^ this.G.GetHashCode();
                result = (result * 397) ^ this.B.GetHashCode();
                return result;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture, "#{0:x2}{1:x2}{2:x2}{3:x2}", this.A, this.R, this.G, this.B);
        }

        /// <summary>
        /// Determines whether this color is invisible.
        /// </summary>
        /// <returns><c>True</c> if the alpha value is 0.</returns>
        public bool IsInvisible()
        {
            return this.A == 0;
        }

        /// <summary>
        /// Determines whether this color is visible.
        /// </summary>
        /// <returns><c>True</c> if the alpha value is greater than 0.</returns>
        public bool IsVisible()
        {
            return this.A > 0;
        }

        /// <summary>
        /// Determines whether this color is undefined.
        /// </summary>
        /// <returns><c>True</c> if the color equals <see cref="OxyColors.Undefined" />.</returns>
        public bool IsUndefined()
        {
            return this.Equals(OxyColors.Undefined);
        }

        /// <summary>
        /// Determines whether this color is automatic.
        /// </summary>
        /// <returns><c>True</c> if the color equals <see cref="OxyColors.Automatic" />.</returns>
        public bool IsAutomatic()
        {
            return this.Equals(OxyColors.Automatic);
        }

        /// <summary>
        /// Gets the actual color.
        /// </summary>
        /// <param name="defaultColor">The default color.</param>
        /// <returns>The default color if the current color equals OxyColors.Automatic, otherwise the color itself.</returns>
        public OxyColor GetActualColor(OxyColor defaultColor)
        {
            return this.IsAutomatic() ? defaultColor : this;
        }

        /// <summary>
        /// Returns C# code that generates this instance.
        /// </summary>
        /// <returns>The C# code.</returns>
        string ICodeGenerating.ToCode()
        {
            return this.ToCode();
        }
    }
}
