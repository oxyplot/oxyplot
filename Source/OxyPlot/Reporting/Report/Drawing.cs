namespace OxyPlot.Reporting
{
    /// <summary>
    /// Drawing currently only supports SVG format.
    /// </summary>
    public class Drawing : Figure
    {
        public enum DrawingFormat { Svg }
        public DrawingFormat Format { get; set; }
        public string Content { get; set; }

        public override void WriteContent(IReportWriter w)
        {
            w.WriteDrawing(this);
        }
    }
}