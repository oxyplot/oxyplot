namespace OxyPlot.Reporting
{
    public class ReportStyle
    {
        public ReportStyle(string titleFontFamily = "Arial", string bodyTextFontFamily = "Verdana")
        {
            DefaultStyle = new ParagraphStyle { FontFamily = bodyTextFontFamily, FontSize = 12 };

            HeaderStyles = new ParagraphStyle[5];
            HeaderStyles[0] = new ParagraphStyle { BasedOn = DefaultStyle, FontFamily = titleFontFamily };
            for (int i = 1; i < HeaderStyles.Length; i++)
                HeaderStyles[i] = new ParagraphStyle { BasedOn = HeaderStyles[i - 1] };
            for (int i = 0; i < HeaderStyles.Length; i++)
            {
                HeaderStyles[i].FontSize = 22 - i * 2;
                HeaderStyles[i].Bold = true;
            }

            BodyTextStyle = new ParagraphStyle { BasedOn = DefaultStyle };
            FigureTextStyle = new ParagraphStyle { BasedOn = DefaultStyle, Italic = true };
            TableTextStyle = new ParagraphStyle { BasedOn = DefaultStyle };
            TableHeaderStyle = new ParagraphStyle { BasedOn = DefaultStyle, Bold = true };
        }

        public ParagraphStyle DefaultStyle { get; set; }
        public ParagraphStyle[] HeaderStyles { get; set; }
        public ParagraphStyle BodyTextStyle { get; set; }
        public ParagraphStyle FigureTextStyle { get; set; }
        public ParagraphStyle TableTextStyle { get; set; }
        public ParagraphStyle TableHeaderStyle { get; set; }
    }
}