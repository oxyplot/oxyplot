namespace OxyPlot.Reporting
{
    public class ParagraphStyle
    {
        public ParagraphStyle BasedOn { get; set; }

        private const string DefaultFont = "Arial";
        private const double DefaultFontSize = 12;

        private string fontFamily;
        public string FontFamily
        {
            get
            {
                if (fontFamily != null) return fontFamily;
                if (BasedOn != null) return BasedOn.FontFamily;
                return DefaultFont;
            }
            set { fontFamily = value; }
        }

        private double? fontSize;
        public double FontSize
        {
            get
            {
                if (fontSize != null) return fontSize.Value;
                if (BasedOn != null) return BasedOn.FontSize;
                return DefaultFontSize;
            }
            set { fontSize = value; }
        }

        private OxyColor textColor;
        public OxyColor TextColor
        {
            get
            {
                if (textColor != null) return textColor;
                if (BasedOn != null) return BasedOn.TextColor;
                return OxyColors.Black;
            }
            set { textColor = value; }
        }

        private bool? bold;
        public bool Bold
        {
            get
            {
                if (bold != null) return bold.Value;
                if (BasedOn != null) return BasedOn.Bold;
                return false;
            }
            set { bold = value; }
        }

        private bool? italic;
        public bool Italic
        {
            get
            {
                if (italic != null) return italic.Value;
                if (BasedOn != null) return BasedOn.Italic;
                return false;
            }
            set { italic = value; }
        }

        // margin
        // padding
        // borders
    }
}
