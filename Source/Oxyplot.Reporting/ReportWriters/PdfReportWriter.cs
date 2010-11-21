using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;

namespace OxyPlot.Reporting
{
    public class PdfReportWriter : IDisposable, IReportWriter
    {
        private Document doc;
        private string filename;

        public PdfReportWriter(string filename)
        {
            this.filename = filename;
            doc = new Document();
        }

        public void Close()
        {
            var r = new PdfDocumentRenderer();
            r.Document = doc;
            r.RenderDocument();
            r.PdfDocument.Save(filename);
        }

        private Section currentSection;
        public void WriteHeader(Header h)
        {
            if (h.Text == null)
                return;
            currentSection = doc.AddSection();
            var p = currentSection.AddParagraph(h.Text);
            p.AddFormattedText("Hello, World!", TextFormat.Bold);
        }

        public void WriteParagraph(Paragraph p)
        {
            var pa = currentSection.AddParagraph(p.Text);
            pa.AddFormattedText("Hello, World!", TextFormat.Bold);
        }

        int tableCounter = 0;

        public void WriteTable(Table t)
        {
            if (t.Items == null)
                return;
            tableCounter++;

            var table = currentSection.AddTable();
        }

        int FigureCounter;

        public void WriteStartFigure(Figure f)
        {
            FigureCounter++;
        }
        public void WriteEndFigure(string text)
        {
            //            String.Format("Fig {0}. {1}", FigureCounter, text));
        }

        public void WriteImage(Image i)
        {
            //Source);
            //FigureText);
        }

        public void WriteDrawing(Drawing d)
        {
            //d.Content);
        }

        public void WritePlot(Plot plot)
        {
            // use pdf renderer to draw the plot
        }

        public void Dispose()
        {
            Close();
        }
    }
}