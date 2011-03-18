namespace OxyPlot.Reporting
{
    public class Header : ReportItem
    {
        /// <summary>
        /// Gets or sets the level of the header (1-5).
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets the header text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the chapter number(s).
        /// </summary>
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
            h += Text;
            return h;
        }
    }
}