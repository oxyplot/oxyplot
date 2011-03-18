namespace OxyPlot.Reporting
{
    /// <summary>
    /// Interface for Report writers.
    /// </summary>
    public interface IReportWriter
    {
        void WriteReport(Report report, ReportStyle style);

        void WriteHeader(Header h);
        void WriteParagraph(Paragraph p);
        void WriteTable(Table t);
        void WriteImage(Image i);
        void WriteDrawing(DrawingFigure d);
        void WritePlot(PlotFigure plot);
        void WriteEquation(Equation equation);
    }
}