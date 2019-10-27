// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportStyle.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a report style.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    /// <summary>
    /// Represents a report style.
    /// </summary>
    public class ReportStyle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportStyle" /> class.
        /// </summary>
        /// <param name="titleFontFamily">The title font family.</param>
        /// <param name="bodyTextFontFamily">The body text font family.</param>
        /// <param name="tableTextFontFamily">The table text font family.</param>
        public ReportStyle(
            string titleFontFamily = "Arial",
            string bodyTextFontFamily = "Verdana",
            string tableTextFontFamily = "Courier New")
        {
            this.DefaultStyle = new ParagraphStyle { FontFamily = bodyTextFontFamily, FontSize = 11, SpacingAfter = 10 };

            this.HeaderStyles = new ParagraphStyle[5];
            this.HeaderStyles[0] = new ParagraphStyle
                {
                   BasedOn = this.DefaultStyle, FontFamily = titleFontFamily, SpacingBefore = 12, SpacingAfter = 3
                };
            for (int i = 1; i < this.HeaderStyles.Length; i++)
            {
                this.HeaderStyles[i] = new ParagraphStyle { BasedOn = this.HeaderStyles[i - 1] };
            }

            foreach (var t in this.HeaderStyles)
            {
                t.Bold = true;
            }

            this.HeaderStyles[0].FontSize = 16;
            this.HeaderStyles[1].FontSize = 14;
            this.HeaderStyles[2].FontSize = 13;
            this.HeaderStyles[3].FontSize = 12;
            this.HeaderStyles[4].FontSize = 11;

            this.HeaderStyles[0].PageBreakBefore = true;
            this.HeaderStyles[1].PageBreakBefore = false;

            this.BodyTextStyle = new ParagraphStyle { BasedOn = this.DefaultStyle };
            this.FigureTextStyle = new ParagraphStyle { BasedOn = this.DefaultStyle, Italic = true };

            this.TableTextStyle = new ParagraphStyle
                {
                    BasedOn = this.DefaultStyle,
                    FontFamily = tableTextFontFamily,
                    SpacingAfter = 0,
                    LeftIndentation = 3,
                    RightIndentation = 3
                };
            this.TableHeaderStyle = new ParagraphStyle { BasedOn = this.TableTextStyle, Bold = true };
            this.TableCaptionStyle = new ParagraphStyle
                {
                   BasedOn = this.DefaultStyle, Italic = true, SpacingBefore = 10, SpacingAfter = 3
                };

            this.Margins = new OxyThickness(25);

            this.FigureTextFormatString = "Figure {0}. {1}";
            this.TableCaptionFormatString = "Table {0}. {1}";
        }

        /// <summary>
        /// Gets or sets the body text style.
        /// </summary>
        public ParagraphStyle BodyTextStyle { get; set; }

        /// <summary>
        /// Gets or sets the default style.
        /// </summary>
        public ParagraphStyle DefaultStyle { get; set; }

        /// <summary>
        /// Gets or sets the figure text format string.
        /// </summary>
        public string FigureTextFormatString { get; set; }

        /// <summary>
        /// Gets or sets figure text style.
        /// </summary>
        public ParagraphStyle FigureTextStyle { get; set; }

        /// <summary>
        /// Gets or sets header styles.
        /// </summary>
        public ParagraphStyle[] HeaderStyles { get; set; }

        /// <summary>
        /// Gets or sets the page margins (mm).
        /// </summary>
        public OxyThickness Margins { get; set; }

        // todo: should the FormatStrings be in the Report class?

        /// <summary>
        /// Gets or sets the table caption format string.
        /// </summary>
        public string TableCaptionFormatString { get; set; }

        /// <summary>
        /// Gets or sets the table caption style.
        /// </summary>
        public ParagraphStyle TableCaptionStyle { get; set; }

        /// <summary>
        /// Gets or sets the table header style.
        /// </summary>
        public ParagraphStyle TableHeaderStyle { get; set; }

        /// <summary>
        /// Gets or sets the table text style.
        /// </summary>
        public ParagraphStyle TableTextStyle { get; set; }
    }
}