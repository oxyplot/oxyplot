namespace OxyPlot.Reporting
{
    /// <summary>
    /// Equations (Tex or MathML)
    /// </summary>
    public class Equation : ReportItem
    {
        public string Content { get; set; }
        public override void WriteContent(IReportWriter w)
        {
            // w.WriteEquation(this);
        }
    }
}