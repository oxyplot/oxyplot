// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Color.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    public class Color
    {
        public byte A { get; set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public static Color FromARGB(byte a, byte r, byte g, byte b)
        {
            return new Color { A = a, R = r, G = g, B = b };
        }

        public static Color FromRGB(byte r, byte g, byte b)
        {
            return new Color { A = 255, R = r, G = g, B = b };
        }

        public static Color FromAColor(byte a, Color color)
        {
            return new Color { A = a, R = color.R, G = color.G, B = color.B };
        }
        public override string ToString()
        {
            return string.Format("#{0:x2}{1:x2}{2:x2}{3:x2}", A, R, G, B);
        }
    }
}