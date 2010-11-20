namespace OxyPlot.Reporting
{
    public class Plot : Figure
    {
        public PlotModel PlotModel { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public override void WriteContent(IReportWriter w)
        {
            w.WritePlot(this);
        }
    }
}