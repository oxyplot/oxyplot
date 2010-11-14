namespace OxyReporter
{
    public class Paragraph : ReportItem
    {
        public string Text { get; set; }
        public override void WriteContent(IReportWriter w)
        {
            w.WriteParagraph(this);
        }
    }
}