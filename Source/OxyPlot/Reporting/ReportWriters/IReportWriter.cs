// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReportWriter.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Specifies functionality to write <see cref="Report" /> objects.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    /// <summary>
    /// Specifies functionality to write <see cref="Report" /> objects.
    /// </summary>
    public interface IReportWriter
    {
        /// <summary>
        /// Writes the specified drawing.
        /// </summary>
        /// <param name="drawing">The drawing.</param>
        void WriteDrawing(DrawingFigure drawing);

        /// <summary>
        /// Writes the specified equation.
        /// </summary>
        /// <param name="equation">The equation.</param>
        void WriteEquation(Equation equation);

        /// <summary>
        /// Writes the specified header.
        /// </summary>
        /// <param name="header">The header.</param>
        void WriteHeader(Header header);

        /// <summary>
        /// Writes the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        void WriteImage(Image image);

        /// <summary>
        /// Writes the specified paragraph.
        /// </summary>
        /// <param name="paragraph">The paragraph.</param>
        void WriteParagraph(Paragraph paragraph);

        /// <summary>
        /// Writes the specified plot.
        /// </summary>
        /// <param name="plot">The plot.</param>
        void WritePlot(PlotFigure plot);

        /// <summary>
        /// Writes the specified report with the specified style.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="reportStyle">The style.</param>
        void WriteReport(Report report, ReportStyle reportStyle);

        /// <summary>
        /// Writes the specified table.
        /// </summary>
        /// <param name="table">The table.</param>
        void WriteTable(Table table);
    }
}