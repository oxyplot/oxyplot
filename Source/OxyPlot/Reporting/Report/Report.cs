namespace OxyPlot.Reporting
{
    public class Report : ReportItem
    {
        public override void Write(IReportWriter w)
        {
            UpdateFigureNumbers();
            base.Write(w);
        }
    }
}