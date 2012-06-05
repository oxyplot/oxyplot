// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReportWriter.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    /// <summary>
    /// Interface for Report writers.
    /// </summary>
    public interface IReportWriter
    {
        #region Public Methods

        /// <summary>
        /// Writes the drawing.
        /// </summary>
        /// <param name="drawing">The drawing.</param>
        void WriteDrawing(DrawingFigure drawing);

        /// <summary>
        /// Writes the equation.
        /// </summary>
        /// <param name="equation">The equation.</param>
        void WriteEquation(Equation equation);

        /// <summary>
        /// Writes the header.
        /// </summary>
        /// <param name="header">The header.</param>
        void WriteHeader(Header header);

        /// <summary>
        /// Writes the image.
        /// </summary>
        /// <param name="image">The image.</param>
        void WriteImage(Image image);

        /// <summary>
        /// Writes the paragraph.
        /// </summary>
        /// <param name="paragraph">The paragraph.</param>
        void WriteParagraph(Paragraph paragraph);

        /// <summary>
        /// Writes the plot.
        /// </summary>
        /// <param name="plot">The plot.</param>
        void WritePlot(PlotFigure plot);

        /// <summary>
        /// Writes the report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="reportStyle">The style.</param>
        void WriteReport(Report report, ReportStyle reportStyle);

        /// <summary>
        /// Writes the table.
        /// </summary>
        /// <param name="table">The table.</param>
        void WriteTable(Table table);

        #endregion
    }
}