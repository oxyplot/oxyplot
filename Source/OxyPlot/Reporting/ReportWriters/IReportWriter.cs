//-----------------------------------------------------------------------
// <copyright file="IReportWriter.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    /// <summary>
    /// Interface for Report writers.
    /// </summary>
    public interface IReportWriter
    {
        #region Public Methods

        /// <summary>
        /// The write drawing.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        void WriteDrawing(DrawingFigure d);

        /// <summary>
        /// The write equation.
        /// </summary>
        /// <param name="equation">
        /// The equation.
        /// </param>
        void WriteEquation(Equation equation);

        /// <summary>
        /// The write header.
        /// </summary>
        /// <param name="h">
        /// The h.
        /// </param>
        void WriteHeader(Header h);

        /// <summary>
        /// The write image.
        /// </summary>
        /// <param name="i">
        /// The i.
        /// </param>
        void WriteImage(Image i);

        /// <summary>
        /// The write paragraph.
        /// </summary>
        /// <param name="p">
        /// The p.
        /// </param>
        void WriteParagraph(Paragraph p);

        /// <summary>
        /// The write plot.
        /// </summary>
        /// <param name="plot">
        /// The plot.
        /// </param>
        void WritePlot(PlotFigure plot);

        /// <summary>
        /// The write report.
        /// </summary>
        /// <param name="report">
        /// The report.
        /// </param>
        /// <param name="style">
        /// The style.
        /// </param>
        void WriteReport(Report report, ReportStyle style);

        /// <summary>
        /// The write table.
        /// </summary>
        /// <param name="t">
        /// The t.
        /// </param>
        void WriteTable(Table t);

        #endregion
    }
}
