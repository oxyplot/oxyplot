namespace OxyPlot.Reporting
{
    public class Header : ReportItem
    {
        public int Level { get; set; }
        public string Text { get; set; }
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