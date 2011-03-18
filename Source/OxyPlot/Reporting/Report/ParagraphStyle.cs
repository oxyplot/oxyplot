using System;

namespace OxyPlot.Reporting
{
    public class ParagraphStyle
    {
        public ParagraphStyle BasedOn { get; set; }

        private const string DefaultFont = "Arial";
        private const double DefaultFontSize = 11;

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

        private double? spacingBefore;
        public double SpacingBefore
        {
            get
            {
                if (spacingBefore != null) return spacingBefore.Value;
                if (BasedOn != null) return BasedOn.SpacingBefore;
                return 0;
            }
            set { spacingBefore = value; }
        }

        private double? spacingAfter;
        public double SpacingAfter
        {
            get
            {
                if (spacingAfter != null) return spacingAfter.Value;
                if (BasedOn != null) return BasedOn.SpacingAfter;
                return 0;
            }
            set { spacingAfter = value; }
        }

        private double? lineSpacing;
        public double LineSpacing
        {
            get
            {
                if (lineSpacing != null) return lineSpacing.Value;
                if (BasedOn != null) return BasedOn.LineSpacing;
                return 1;
            }
            set { lineSpacing = value; }
        }

        private double? leftIndentation;
        public double LeftIndentation
        {
            get
            {
                if (leftIndentation != null) return leftIndentation.Value;
                if (BasedOn != null) return BasedOn.LeftIndentation;
                return 0;
            }
            set { leftIndentation = value; }
        }

        private double? rightIndentation;
        public double RightIndentation
        {
            get
            {
                if (rightIndentation != null) return rightIndentation.Value;
                if (BasedOn != null) return BasedOn.RightIndentation;
                return 0;
            }
            set { rightIndentation = value; }
        }

        private bool? pageBreakBefore;
        public bool PageBreakBefore
        {
            get
            {
                if (pageBreakBefore != null) return pageBreakBefore.Value;
                if (BasedOn != null) return BasedOn.PageBreakBefore;
                return false;
            }
            set { pageBreakBefore = value; }
        }

        // margin
        // padding
        // borders
    }
}
