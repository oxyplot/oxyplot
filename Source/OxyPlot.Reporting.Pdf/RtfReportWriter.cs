// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RtfReportWriter.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a report writer for rich text format using MigraDoc.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Pdf
{
    using System.IO;

    using MigraDoc.RtfRendering;

    /// <summary>
    /// Provides a report writer for rich text format using MigraDoc.
    /// </summary>
    public class RtfReportWriter : PdfReportWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RtfReportWriter" /> class.
        /// </summary>
        /// <param name="filename">The FileName.</param>
        public RtfReportWriter(string filename)
            : base(filename)
        {
        }

        /// <summary>
        /// The close.
        /// </summary>
        public override void Close()
        {
            var r = new RtfDocumentRenderer();
            r.Render(this.Document, this.Output, Path.GetTempPath());
        }
    }
}