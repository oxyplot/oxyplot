namespace OxyPlot.Reporting
{
    public class Report : ReportItem
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Author { get; set; }

        public override void Write(IReportWriter w)
        {
            UpdateFigureNumbers();
            base.Write(w);
        }
    }
}