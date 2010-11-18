namespace OxyPlot.Reporting
{
    public class Image : Figure
    {
        public string Source { get; set; }
        public override void WriteContent(IReportWriter w)
        {
            w.WriteImage(this);
        }
    }
}