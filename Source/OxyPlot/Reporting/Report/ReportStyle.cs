using System;

namespace OxyPlot.Reporting
{
    public class ReportStyle
    {
        public ReportStyle(string titleFontFamily = "Arial", string bodyTextFontFamily = "Verdana", string tableTextFontFamily = "Courier New")
        {
            DefaultStyle = new ParagraphStyle { FontFamily = bodyTextFontFamily, FontSize = 11, SpacingAfter = 10 };

            HeaderStyles = new ParagraphStyle[5];
            HeaderStyles[0] = new ParagraphStyle { BasedOn = DefaultStyle, FontFamily = titleFontFamily, SpacingBefore = 12, SpacingAfter = 3 };
            for (int i = 1; i < HeaderStyles.Length; i++)
                HeaderStyles[i] = new ParagraphStyle { BasedOn = HeaderStyles[i - 1] };

            for (int i = 0; i < HeaderStyles.Length; i++)
            {
                HeaderStyles[i].Bold = true;
            }
            HeaderStyles[0].FontSize = 16;
            HeaderStyles[1].FontSize = 14;
            HeaderStyles[2].FontSize = 13;
            HeaderStyles[3].FontSize = 12;
            HeaderStyles[4].FontSize = 11;

            HeaderStyles[0].PageBreakBefore = true;
            HeaderStyles[1].PageBreakBefore = false;

            BodyTextStyle = new ParagraphStyle { BasedOn = DefaultStyle };
            FigureTextStyle = new ParagraphStyle { BasedOn = DefaultStyle, Italic = true };

            TableTextStyle = new ParagraphStyle { BasedOn = DefaultStyle, FontFamily = tableTextFontFamily, SpacingAfter = 0, LeftIndentation = 3, RightIndentation = 3 };
            TableHeaderStyle = new ParagraphStyle { BasedOn = TableTextStyle, Bold = true };
            TableCaptionStyle = new ParagraphStyle { BasedOn = DefaultStyle, Italic = true, SpacingBefore = 10, SpacingAfter = 3 };

            Margins = new OxyThickness(25);

            FigureTextFormatString = "Figure {0}. {1}";
            TableCaptionFormatString = "Table {0}. {1}";

        }

        public ParagraphStyle DefaultStyle { get; set; }
        public ParagraphStyle[] HeaderStyles { get; set; }
        public ParagraphStyle BodyTextStyle { get; set; }
        public ParagraphStyle FigureTextStyle { get; set; }
        public ParagraphStyle TableTextStyle { get; set; }
        public ParagraphStyle TableHeaderStyle { get; set; }
        public ParagraphStyle TableCaptionStyle { get; set; }

        /// <summary>
        /// Gets or sets the page margins (mm).
        /// </summary>
        public OxyThickness Margins { get; set; }

        // todo: should the FormatStrings be in the Report class?
        public string FigureTextFormatString { get; set; }
        public string TableCaptionFormatString { get; set; }
    }
}