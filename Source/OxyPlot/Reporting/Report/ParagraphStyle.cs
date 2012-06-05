// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParagraphStyle.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    /// <summary>
    /// The paragraph style.
    /// </summary>
    public class ParagraphStyle
    {
        #region Constants and Fields

        /// <summary>
        ///   The default font.
        /// </summary>
        private const string DefaultFont = "Arial";

        /// <summary>
        ///   The default font size.
        /// </summary>
        private const double DefaultFontSize = 11;

        /// <summary>
        ///   The bold.
        /// </summary>
        private bool? bold;

        /// <summary>
        ///   The font family.
        /// </summary>
        private string fontFamily;

        /// <summary>
        ///   The font size.
        /// </summary>
        private double? fontSize;

        /// <summary>
        ///   The italic.
        /// </summary>
        private bool? italic;

        /// <summary>
        ///   The left indentation.
        /// </summary>
        private double? leftIndentation;

        /// <summary>
        ///   The line spacing.
        /// </summary>
        private double? lineSpacing;

        /// <summary>
        ///   The page break before.
        /// </summary>
        private bool? pageBreakBefore;

        /// <summary>
        ///   The right indentation.
        /// </summary>
        private double? rightIndentation;

        /// <summary>
        ///   The spacing after.
        /// </summary>
        private double? spacingAfter;

        /// <summary>
        ///   The spacing before.
        /// </summary>
        private double? spacingBefore;

        /// <summary>
        ///   The text color.
        /// </summary>
        private OxyColor textColor;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets BasedOn.
        /// </summary>
        public ParagraphStyle BasedOn { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether Bold.
        /// </summary>
        public bool Bold
        {
            get
            {
                if (this.bold != null)
                {
                    return this.bold.Value;
                }

                if (this.BasedOn != null)
                {
                    return this.BasedOn.Bold;
                }

                return false;
            }

            set
            {
                this.bold = value;
            }
        }

        /// <summary>
        ///   Gets or sets FontFamily.
        /// </summary>
        public string FontFamily
        {
            get
            {
                if (this.fontFamily != null)
                {
                    return this.fontFamily;
                }

                if (this.BasedOn != null)
                {
                    return this.BasedOn.FontFamily;
                }

                return DefaultFont;
            }

            set
            {
                this.fontFamily = value;
            }
        }

        /// <summary>
        ///   Gets or sets FontSize.
        /// </summary>
        public double FontSize
        {
            get
            {
                if (this.fontSize != null)
                {
                    return this.fontSize.Value;
                }

                if (this.BasedOn != null)
                {
                    return this.BasedOn.FontSize;
                }

                return DefaultFontSize;
            }

            set
            {
                this.fontSize = value;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether Italic.
        /// </summary>
        public bool Italic
        {
            get
            {
                if (this.italic != null)
                {
                    return this.italic.Value;
                }

                if (this.BasedOn != null)
                {
                    return this.BasedOn.Italic;
                }

                return false;
            }

            set
            {
                this.italic = value;
            }
        }

        /// <summary>
        ///   Gets or sets LeftIndentation.
        /// </summary>
        public double LeftIndentation
        {
            get
            {
                if (this.leftIndentation != null)
                {
                    return this.leftIndentation.Value;
                }

                if (this.BasedOn != null)
                {
                    return this.BasedOn.LeftIndentation;
                }

                return 0;
            }

            set
            {
                this.leftIndentation = value;
            }
        }

        /// <summary>
        ///   Gets or sets LineSpacing.
        /// </summary>
        public double LineSpacing
        {
            get
            {
                if (this.lineSpacing != null)
                {
                    return this.lineSpacing.Value;
                }

                if (this.BasedOn != null)
                {
                    return this.BasedOn.LineSpacing;
                }

                return 1;
            }

            set
            {
                this.lineSpacing = value;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether PageBreakBefore.
        /// </summary>
        public bool PageBreakBefore
        {
            get
            {
                if (this.pageBreakBefore != null)
                {
                    return this.pageBreakBefore.Value;
                }

                if (this.BasedOn != null)
                {
                    return this.BasedOn.PageBreakBefore;
                }

                return false;
            }

            set
            {
                this.pageBreakBefore = value;
            }
        }

        /// <summary>
        ///   Gets or sets RightIndentation.
        /// </summary>
        public double RightIndentation
        {
            get
            {
                if (this.rightIndentation != null)
                {
                    return this.rightIndentation.Value;
                }

                if (this.BasedOn != null)
                {
                    return this.BasedOn.RightIndentation;
                }

                return 0;
            }

            set
            {
                this.rightIndentation = value;
            }
        }

        /// <summary>
        ///   Gets or sets SpacingAfter.
        /// </summary>
        public double SpacingAfter
        {
            get
            {
                if (this.spacingAfter != null)
                {
                    return this.spacingAfter.Value;
                }

                if (this.BasedOn != null)
                {
                    return this.BasedOn.SpacingAfter;
                }

                return 0;
            }

            set
            {
                this.spacingAfter = value;
            }
        }

        /// <summary>
        ///   Gets or sets SpacingBefore.
        /// </summary>
        public double SpacingBefore
        {
            get
            {
                if (this.spacingBefore != null)
                {
                    return this.spacingBefore.Value;
                }

                if (this.BasedOn != null)
                {
                    return this.BasedOn.SpacingBefore;
                }

                return 0;
            }

            set
            {
                this.spacingBefore = value;
            }
        }

        /// <summary>
        ///   Gets or sets TextColor.
        /// </summary>
        public OxyColor TextColor
        {
            get
            {
                if (this.textColor != null)
                {
                    return this.textColor;
                }

                if (this.BasedOn != null)
                {
                    return this.BasedOn.TextColor;
                }

                return OxyColors.Black;
            }

            set
            {
                this.textColor = value;
            }
        }

        #endregion

        // margin
        // padding
        // borders
    }
}