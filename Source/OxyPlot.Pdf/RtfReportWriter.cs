// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RtfReportWriter.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Pdf
{
    using System.IO;

    using MigraDoc.RtfRendering;

    /// <summary>
    /// RTF report writer using MigraDoc.
    /// </summary>
    public class RtfReportWriter : PdfReportWriter
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RtfReportWriter"/> class.
        /// </summary>
        /// <param name="filename">
        /// The FileName.
        /// </param>
        public RtfReportWriter(string filename)
            : base(filename)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The close.
        /// </summary>
        public override void Close()
        {
            var r = new RtfDocumentRenderer();
            r.Render(this.Document, this.FileName, Path.GetTempPath());
        }

        #endregion
    }
}