using System.IO;
using MigraDoc.RtfRendering;

namespace OxyPlot.Pdf
{
    /// <summary>
    /// RTF report writer using MigraDoc.
    /// </summary>
    public class RtfReportWriter : PdfReportWriter
    {
        public RtfReportWriter(string filename) : base(filename)
        {
        }

        public override void Close()
        {
            var r = new RtfDocumentRenderer();
            r.Render(doc, filename, Path.GetTempPath());
        }
    }
}