namespace OxyPlot.Reporting
{
    public class Header : ReportItem
    {
        /// <summary>
        /// Gets or sets the level of the header (1-5).
        /// </summary>
        /// <value>The level.</value>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the chapter number(s).
        /// </summary>
        /// <value>The chapter.</value>
        public string Chapter { get; set; }

        public override void WriteContent(IReportWriter w)
        {
            w.WriteHeader(this);
        }

        public override string ToString()
        {
            string h = "";
            if (Chapter != null)
                h += Chapter + " ";
            h += Text.ToUpper();
            return h;
        }
    }
}