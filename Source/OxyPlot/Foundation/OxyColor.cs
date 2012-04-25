// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyColor.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Describes a color in terms of alpha, red, green, and blue channels.
    /// </summary>
    [Serializable]
    public class OxyColor : ICodeGenerating
    {
        #region Public Properties

        /// <summary>
        ///   Gets or sets the alpha value.
        /// </summary>
        /// <value> The alpha value. </value>
        public byte A { get; set; }

        /// <summary>
        ///   Gets or sets the blue value.
        /// </summary>
        /// <value> The blue value. </value>
        public byte B { get; set; }

        /// <summary>
        ///   Gets or sets the green value.
        /// </summary>
        /// <value> The green value. </value>
        public byte G { get; set; }

        /// <summary>
        ///   Gets or sets the red value.
        /// </summary>
        /// <value> The red value. </value>
        public byte R { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a color defined by an alpha value and another color.
        /// </summary>
        /// <param name="a">
        /// Alpha value. 
        /// </param>
        /// <param name="color">
        /// The original color. 
        /// </param>
        /// <returns>
        /// A color.
        /// </returns>
        public static OxyColor FromAColor(byte a, OxyColor color)
        {
            return new OxyColor { A = a, R = color.R, G = color.G, B = color.B };
        }

        /// <summary>
        /// Creates a color from the specified ARGB values.
        /// </summary>
        /// <param name="a">
        /// The alpha value. 
        /// </param>
        /// <param name="r">
        /// The red value. 
        /// </param>
        /// <param name="g">
        /// The green value. 
        /// </param>
        /// <param name="b">
        /// The blue value. 
        /// </param>
        /// <returns>
        /// A color.
        /// </returns>
        public static OxyColor FromArgb(byte a, byte r, byte g, byte b)
        {
            return new OxyColor { A = a, R = r, G = g, B = b };
        }

        /// <summary>
        /// Creates a color from the specified RGB values.
        /// </summary>
        /// <param name="r">
        /// The red value. 
        /// </param>
        /// <param name="g">
        /// The green value. 
        /// </param>
        /// <param name="b">
        /// The blue value. 
        /// </param>
        /// <returns>
        /// A color.
        /// </returns>
// ReSharper disable InconsistentNaming
        public static OxyColor FromRGB(byte r, byte g, byte b)
// ReSharper restore InconsistentNaming
        {
            return new OxyColor { A = 255, R = r, G = g, B = b };
        }

        /// <summary>
        /// Creates a color from an unsigned integer.
        /// </summary>
        /// <param name="argb">
        /// The ARGB value. 
        /// </param>
        /// <returns>
        /// A color.
        /// </returns>
        public static OxyColor FromUInt32(uint argb)
        {
            var a = (byte)((argb & -16777216) >> 0x18);
            var r = (byte)((argb & 0xff0000) >> 0x10);
            var g = (byte)((argb & 0xff00) >> 8);
            var b = (byte)(argb & 0xff);
            return FromArgb(a, r, g, b);
        }

        /// <summary>
        /// Interpolates the specified colors.
        /// </summary>
        /// <param name="color1">
        /// The color1. 
        /// </param>
        /// <param name="color2">
        /// The color2. 
        /// </param>
        /// <param name="t">
        /// The t. 
        /// </param>
        /// <returns>
        /// The interpolated color 
        /// </returns>
        public static OxyColor Interpolate(OxyColor color1, OxyColor color2, double t)
        {
            double a = (color1.A * (1 - t)) + (color2.A * t);
            double r = (color1.R * (1 - t)) + (color2.R * t);
            double g = (color1.G * (1 - t)) + (color2.G * t);
            double b = (color1.B * (1 - t)) + (color2.B * t);
            return FromArgb((byte)a, (byte)r, (byte)g, (byte)b);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="System.Object"/> to compare with this instance. 
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c> . 
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != typeof(OxyColor))
            {
                return false;
            }

            return this.Equals((OxyColor)obj);
        }

        /// <summary>
        /// Determines whether the specified <see cref="OxyColor"/> is equal to this instance.
        /// </summary>
        /// <param name="other">
        /// The <see cref="OxyColor"/> to compare with this instance. 
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="OxyColor"/> is equal to this instance; otherwise, <c>false</c> . 
        /// </returns>
        public bool Equals(OxyColor other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other.A == this.A && other.R == this.R && other.G == this.G && other.B == this.B;
        }

        /// <summary>
        /// Gets the color name.
        /// </summary>
        /// <returns>
        /// The color name. 
        /// </returns>
        public string GetColorName()
        {
            Type t = typeof(OxyColors);
            FieldInfo[] colors = t.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var color in colors)
            {
                object c = color.GetValue(null);
                if (this.Equals(c))
                {
                    return color.Name;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
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
        /// Returns C# code that generates this instance.
        /// </summary>
        /// <returns>
        /// The to code. 
        /// </returns>
        public string ToCode()
        {
            string name = this.GetColorName();
            if (name != null)
            {
                return string.Format("OxyColors.{0}", name);
            }

            return string.Format("OxyColor.FromArgb({0}, {1}, {2}, {3})", this.A, this.R, this.G, this.B);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance. 
        /// </returns>
        public override string ToString()
        {
            return string.Format("#{0:x2}{1:x2}{2:x2}{3:x2}", this.A, this.R, this.G, this.B);
        }

        #endregion
    }
}