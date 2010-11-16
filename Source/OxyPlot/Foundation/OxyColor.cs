namespace OxyPlot
{
    public class OxyColor
    {
        public byte A { get; set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public static OxyColor FromArgb(byte a, byte r, byte g, byte b)
        {
            return new OxyColor { A = a, R = r, G = g, B = b };
        }

        public static OxyColor FromRGB(byte r, byte g, byte b)
        {
            return new OxyColor { A = 255, R = r, G = g, B = b };
        }

        public static OxyColor FromAColor(byte a, OxyColor color)
        {
            return new OxyColor { A = a, R = color.R, G = color.G, B = color.B };
        }

        public override string ToString()
        {
            return string.Format("#{0:x2}{1:x2}{2:x2}{3:x2}", A, R, G, B);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(OxyColor)) return false;
            return Equals((OxyColor)obj);
        }

        public bool Equals(OxyColor other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.A == A && other.R == R && other.G == G && other.B == B;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = A.GetHashCode();
                result = (result * 397) ^ R.GetHashCode();
                result = (result * 397) ^ G.GetHashCode();
                result = (result * 397) ^ B.GetHashCode();
                return result;
            }
        }

        public static OxyColor FromUInt32(uint argb)
        {
            var a = (byte)((argb & -16777216) >> 0x18);
            var r = (byte)((argb & 0xff0000) >> 0x10);
            var g = (byte)((argb & 0xff00) >> 8);
            var b = (byte)(argb & 0xff);
            return FromArgb(a, r, g, b);
        }
    }
}