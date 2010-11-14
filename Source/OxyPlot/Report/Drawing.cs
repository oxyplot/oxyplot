namespace OxyReporter
{
    public class Drawing : Figure
    {
        public string Content { get; set; }
        public override void WriteContent(IReportWriter w)
        {
            w.WriteDrawing(this);
        }
    }
}