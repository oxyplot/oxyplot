namespace OxyPlot.Reporting
{
    public interface IReportWriter
    {
        void WriteHeader(Header h);
        void WriteParagraph(Paragraph p);
        void WriteTable(Table t);
        void WriteImage(Image i);
        void WriteDrawing(Drawing d);
        void WritePlot(Plot plot);
    }
}