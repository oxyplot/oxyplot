namespace OxyPlot.MonoForAndroid
{
    using Android.Graphics;

    public static class ExtensionMethods
    {
        public static Color ToColor(this OxyColor color)
        {
            if (color == null)
                return Color.Transparent;
            return new Color(color.R, color.G, color.B, color.A);
        }

        public static RectF ToRectF(this OxyRect rect)
        {
            return new RectF((float)rect.Left, (float)rect.Top, (float)rect.Right, (float)rect.Bottom);
        }
    }
}