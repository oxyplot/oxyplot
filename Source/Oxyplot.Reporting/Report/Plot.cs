namespace OxyPlot.Reporting
{
    public class Plot : Figure
    {
        public PlotModel PlotModel { get; set; }
        
        public override void WriteContent(IReportWriter w)
        {
            // w.WritePlot(this);
        }
    }
}