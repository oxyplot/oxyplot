// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WordDocumentReportWriter.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Word/OpenXML (.docx) report writer using OpenXML SDK 2.0.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.OpenXml
{
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.IO;

    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Drawing;
    using DocumentFormat.OpenXml.Drawing.Wordprocessing;
    using DocumentFormat.OpenXml.Office2010.Drawing;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Wordprocessing;

    using OxyPlot.Reporting;
    using OxyPlot.Wpf;

    using BlipFill = DocumentFormat.OpenXml.Drawing.Pictures.BlipFill;
    using BottomBorder = DocumentFormat.OpenXml.Wordprocessing.BottomBorder;
    using Color = DocumentFormat.OpenXml.Wordprocessing.Color;
    using GridColumn = DocumentFormat.OpenXml.Wordprocessing.GridColumn;
    using Header = OxyPlot.Reporting.Header;
    using Image = OxyPlot.Reporting.Image;
    using LeftBorder = DocumentFormat.OpenXml.Wordprocessing.LeftBorder;
    using NonVisualDrawingProperties = DocumentFormat.OpenXml.Drawing.Pictures.NonVisualDrawingProperties;
    using NonVisualGraphicFrameDrawingProperties = DocumentFormat.OpenXml.Drawing.Wordprocessing.NonVisualGraphicFrameDrawingProperties;
    using NonVisualPictureDrawingProperties = DocumentFormat.OpenXml.Drawing.Pictures.NonVisualPictureDrawingProperties;
    using NonVisualPictureProperties = DocumentFormat.OpenXml.Drawing.Pictures.NonVisualPictureProperties;
    using Outline = DocumentFormat.OpenXml.Drawing.Outline;
    using Paragraph = OxyPlot.Reporting.Paragraph;
    using ParagraphProperties = DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties;
    using Path = System.IO.Path;
    using Picture = DocumentFormat.OpenXml.Drawing.Pictures.Picture;
    using RightBorder = DocumentFormat.OpenXml.Wordprocessing.RightBorder;
    using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
    using RunProperties = DocumentFormat.OpenXml.Wordprocessing.RunProperties;
    using ShapeProperties = DocumentFormat.OpenXml.Drawing.Pictures.ShapeProperties;
    using Table = OxyPlot.Reporting.Table;
    using TableCell = DocumentFormat.OpenXml.Wordprocessing.TableCell;
    using TableCellBorders = DocumentFormat.OpenXml.Wordprocessing.TableCellBorders;
    using TableCellProperties = DocumentFormat.OpenXml.Wordprocessing.TableCellProperties;
    using TableGrid = DocumentFormat.OpenXml.Wordprocessing.TableGrid;
    using TableProperties = DocumentFormat.OpenXml.Wordprocessing.TableProperties;
    using TableRow = DocumentFormat.OpenXml.Wordprocessing.TableRow;
    using TableStyle = DocumentFormat.OpenXml.Wordprocessing.TableStyle;
    using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
    using TopBorder = DocumentFormat.OpenXml.Wordprocessing.TopBorder;
    using Transform2D = DocumentFormat.OpenXml.Drawing.Transform2D;

    /// <summary>
    /// Provides a report writer for Word/OpenXML (.docx) output using OpenXML SDK 2.0.
    /// </summary>
    public class WordDocumentReportWriter : IDisposable, IReportWriter
    {
        // http://www.codeproject.com/KB/office/OpenXML-SDK-HelloWorld.aspx
        // http://msdn.microsoft.com/en-us/library/documentformat.openxml.wordprocessing.aspx
        /// <summary>
        /// The body text id.
        /// </summary>
        private const string BodyTextID = "Normal";

        /// <summary>
        /// The body text name.
        /// </summary>
        private const string BodyTextName = "Normal";

        /// <summary>
        /// The figure text id.
        /// </summary>
        private const string FigureTextID = "FigureText";

        /// <summary>
        /// The figure text name.
        /// </summary>
        private const string FigureTextName = "Figure text";

        /// <summary>
        /// The header id.
        /// </summary>
        private const string HeaderID = "Heading{0}";

        /// <summary>
        /// The header name.
        /// </summary>
        private const string HeaderName = "Heading {0}";

        /// <summary>
        /// The table caption id.
        /// </summary>
        private const string TableCaptionID = "TableCaption";

        /// <summary>
        /// The table caption name.
        /// </summary>
        private const string TableCaptionName = "Table caption";

        /// <summary>
        /// The table header id.
        /// </summary>
        private const string TableHeaderID = "TableHeader";

        /// <summary>
        /// The table header name.
        /// </summary>
        private const string TableHeaderName = "Table header";

        /// <summary>
        /// The table text id.
        /// </summary>
        private const string TableTextID = "TableText";

        /// <summary>
        /// The table text name.
        /// </summary>
        private const string TableTextName = "Table text";

        /// <summary>
        /// The style part.
        /// </summary>
        private readonly StyleDefinitionsPart stylePart;

        /// <summary>
        /// The body.
        /// </summary>
        private Body body;

        /// <summary>
        /// The disposed flag.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// The document.
        /// </summary>
        private Document document;

        /// <summary>
        /// The is saved flag.
        /// </summary>
        private bool isSaved;

        /// <summary>
        /// The main part.
        /// </summary>
        private MainDocumentPart mainPart;

        /// <summary>
        /// The package.
        /// </summary>
        private WordprocessingDocument package;

        /// <summary>
        /// The style.
        /// </summary>
        private ReportStyle style;

        /// <summary>
        /// Initializes a new instance of the <see cref="WordDocumentReportWriter"/> class.
        /// </summary>
        /// <param name="filePath">
        /// The file path.
        /// </param>
        public WordDocumentReportWriter(string filePath)
        {
            this.FileName = filePath;
            this.package = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document);

            this.mainPart = this.package.AddMainDocumentPart();

            this.stylePart = this.mainPart.AddNewPart<StyleDefinitionsPart>();

            // fontTablePart = mainPart.AddNewPart<FontTablePart>();
            this.document = this.CreateDocument();
            this.body = new Body();
        }

        /// <summary>
        /// Gets or sets Creator.
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// Gets or sets Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; private set; }

        /// <summary>
        /// Gets or sets Keywords.
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// Gets or sets Revision.
        /// </summary>
        public string Revision { get; set; }

        /// <summary>
        /// Gets or sets Subject.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets Title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets Version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Saves this document.
        /// </summary>
        public void Save()
        {
            this.SetPackageProperties(this.package);
            this.document.Append(this.body);
            this.mainPart.Document = this.document;

            this.stylePart.Styles.Save();
            this.mainPart.Document.Save();
            this.isSaved = true;
        }

        /// <summary>
        /// The write drawing.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        public void WriteDrawing(DrawingFigure d)
        {
            this.body.AppendChild(CreateParagraph("DrawingFigures are not yet supported."));
        }

        /// <summary>
        /// The write equation.
        /// </summary>
        /// <param name="equation">
        /// The equation.
        /// </param>
        public void WriteEquation(Equation equation)
        {
            this.body.AppendChild(CreateParagraph("Equations are not yet supported."));
        }

        /// <summary>
        /// The write header.
        /// </summary>
        /// <param name="h">
        /// The h.
        /// </param>
        public void WriteHeader(Header h)
        {
            this.body.AppendChild(CreateParagraph(h.Text, string.Format(HeaderID, h.Level)));
        }

        /// <summary>
        /// The write image.
        /// </summary>
        /// <param name="i">
        /// The i.
        /// </param>
        public void WriteImage(Image i)
        {
            if (i.Source == null)
            {
                return;
            }

            this.AppendImage(i.Source, "Picture " + i.FigureNumber);

            this.body.Append(CreateParagraph(i.GetFullCaption(this.style), FigureTextID));
        }

        /// <summary>
        /// The write paragraph.
        /// </summary>
        /// <param name="pa">
        /// The pa.
        /// </param>
        public void WriteParagraph(Paragraph pa)
        {
            this.body.AppendChild(CreateParagraph(pa.Text));
        }

        /// <summary>
        /// The write plot.
        /// </summary>
        /// <param name="plot">
        /// The plot.
        /// </param>
        public void WritePlot(PlotFigure plot)
        {
            if (this.FileName == null)
            {
                return;
            }

            var directory = Path.GetDirectoryName(this.FileName);
            if (directory == null)
            {
                return;
            }

            var source = string.Format(
                "{0}_Plot{1}.png", Path.GetFileNameWithoutExtension(this.FileName), plot.FigureNumber);
            var sourceFullPath = Path.Combine(directory, source);

            // write to a png
            // todo: write to a stream, not to disk
            PngExporter.Export(plot.PlotModel, sourceFullPath, (int)plot.Width, (int)plot.Height);

            // append the image to the document
            this.AppendImage(sourceFullPath, "Plot" + plot.FigureNumber);

            this.body.Append(CreateParagraph(plot.GetFullCaption(this.style), FigureTextID));
        }

        /// <summary>
        /// The write report.
        /// </summary>
        /// <param name="report">
        /// The report.
        /// </param>
        /// <param name="reportStyle">
        /// The style.
        /// </param>
        public void WriteReport(Report report, ReportStyle reportStyle)
        {
            this.style = reportStyle;
            this.AddStyles(this.stylePart, reportStyle);
            report.Write(this);
        }

        /// <summary>
        /// The write table.
        /// </summary>
        /// <param name="t">
        /// The t.
        /// </param>
        public void WriteTable(Table t)
        {
            this.body.Append(CreateParagraph(t.GetFullCaption(this.style), TableCaptionID));

            var table = new DocumentFormat.OpenXml.Wordprocessing.Table();

            var tableProperties1 = new TableProperties();
            var tableStyle1 = new TableStyle { Val = "TableGrid" };
            var tableWidth1 = new TableWidth { Width = "0", Type = TableWidthUnitValues.Auto };
            var tableLook1 = new TableLook
                {
                    Val = "04A0",
                    FirstRow = true,
                    LastRow = false,
                    FirstColumn = true,
                    LastColumn = false,
                    NoHorizontalBand = false,
                    NoVerticalBand = true
                };

            tableProperties1.Append(tableStyle1);
            tableProperties1.Append(tableWidth1);
            tableProperties1.Append(tableLook1);

            var tableGrid1 = new TableGrid();
            foreach (var tc in t.Columns)
            {
                // tc.Width
                var gridColumn1 = new GridColumn { Width = "3070" };
                tableGrid1.Append(gridColumn1);
            }

            foreach (var row in t.Rows)
            {
                var tr = new TableRow();

                if (row.IsHeader)
                {
                    var trp = new TableRowProperties();
                    var tableHeader1 = new TableHeader();
                    trp.Append(tableHeader1);
                    tr.Append(trp);
                }

                int j = 0;
                foreach (var c in row.Cells)
                {
                    bool isHeader = row.IsHeader || t.Columns[j++].IsHeader;
                    var cell = new TableCell();
                    var tcp = new TableCellProperties();
                    var borders = new TableCellBorders();
                    borders.Append(
                        new BottomBorder
                            {
                                Val = BorderValues.Single,
                                Size = (UInt32Value)4U,
                                Space = (UInt32Value)0U,
                                Color = "auto"
                            });
                    borders.Append(
                        new TopBorder
                            {
                                Val = BorderValues.Single,
                                Size = (UInt32Value)4U,
                                Space = (UInt32Value)0U,
                                Color = "auto"
                            });
                    borders.Append(
                        new LeftBorder
                            {
                                Val = BorderValues.Single,
                                Size = (UInt32Value)4U,
                                Space = (UInt32Value)0U,
                                Color = "auto"
                            });
                    borders.Append(
                        new RightBorder
                            {
                                Val = BorderValues.Single,
                                Size = (UInt32Value)4U,
                                Space = (UInt32Value)0U,
                                Color = "auto"
                            });
                    tcp.Append(borders);

                    cell.Append(tcp);
                    string styleID = isHeader ? "TableHeader" : "TableText";
                    cell.Append(CreateParagraph(c.Content, styleID));
                    tr.Append(cell);
                }

                table.Append(tr);
            }

            this.body.Append(table);
        }

        /// <summary>
        /// The create paragraph.
        /// </summary>
        /// <param name="content">
        /// The content.
        /// </param>
        /// <param name="styleID">
        /// The style id.
        /// </param>
        /// <returns>
        /// </returns>
        private static DocumentFormat.OpenXml.Wordprocessing.Paragraph CreateParagraph(
            string content, string styleID = null)
        {
            var p = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();

            if (styleID != null)
            {
                var pp = new ParagraphProperties { ParagraphStyleId = new ParagraphStyleId { Val = styleID } };
                p.Append(pp);
            }

            var text = new Text(content);
            var run = new Run(text);
            p.Append(run);
            return p;
        }

        /// <summary>
        /// The create style.
        /// </summary>
        /// <param name="ps">
        /// The ps.
        /// </param>
        /// <param name="styleID">
        /// The style id.
        /// </param>
        /// <param name="styleName">
        /// The style name.
        /// </param>
        /// <param name="basedOnStyleID">
        /// The based on style id.
        /// </param>
        /// <param name="nextStyleID">
        /// The next style id.
        /// </param>
        /// <param name="isDefault">
        /// The is default.
        /// </param>
        /// <param name="isCustomStyle">
        /// The is custom style.
        /// </param>
        /// <returns>
        /// </returns>
        private static Style CreateStyle(
            ParagraphStyle ps,
            string styleID,
            string styleName,
            string basedOnStyleID,
            string nextStyleID,
            bool isDefault = false,
            bool isCustomStyle = true)
        {
            // todo: add font to FontTable?
            var rPr = new StyleRunProperties();

            // http://msdn.microsoft.com/en-us/library/documentformat.openxml.wordprocessing.color.aspx
            var color = new Color { Val = ps.TextColor.ToString().Trim('#').Substring(2) };
            rPr.Append(color);

            // http://msdn.microsoft.com/en-us/library/cc850848.aspx
            rPr.Append(new RunFonts { Ascii = ps.FontFamily, HighAnsi = ps.FontFamily });
            rPr.Append(new FontSize { Val = new StringValue((ps.FontSize * 2).ToString(CultureInfo.InvariantCulture)) });
            rPr.Append(
                new FontSizeComplexScript
                    {
                       Val = new StringValue((ps.FontSize * 2).ToString(CultureInfo.InvariantCulture))
                    });

            if (ps.Bold)
            {
                rPr.Append(new Bold());
            }

            if (ps.Italic)
            {
                rPr.Append(new Italic());
            }

            var pPr = new StyleParagraphProperties();
            var spacingBetweenLines2 = new SpacingBetweenLines
                {
                    After = string.Format(CultureInfo.InvariantCulture, "{0}", ps.SpacingAfter * 20),
                    Before = string.Format(CultureInfo.InvariantCulture, "{0}", ps.SpacingBefore * 20),
                    Line = string.Format(CultureInfo.InvariantCulture, "{0}", ps.LineSpacing * 240),
                    LineRule = LineSpacingRuleValues.Auto
                };
            var indentation = new Indentation
                {
                    Left = string.Format(CultureInfo.InvariantCulture, "{0}", ps.LeftIndentation * 20),
                    Right = string.Format(CultureInfo.InvariantCulture, "{0}", ps.RightIndentation * 20)
                };
            var contextualSpacing1 = new ContextualSpacing();

            pPr.Append(spacingBetweenLines2);
            pPr.Append(contextualSpacing1);
            pPr.Append(indentation);

            // StyleRunProperties styleRunProperties7 = new StyleRunProperties();
            // RunFonts runFonts8 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana" };
            // Color color7 = new Color() { Val = "000000" };

            // styleRunProperties7.Append(runFonts8);
            // styleRunProperties7.Append(color7);

            // http://msdn.microsoft.com/en-us/library/documentformat.openxml.wordprocessing.style.aspx
            var style = new Style
                {
                    Default = new OnOffValue(isDefault),
                    CustomStyle = new OnOffValue(isCustomStyle),
                    StyleId = styleID,
                    Type = StyleValues.Paragraph
                };

            style.Append(new Name { Val = styleName });
            if (basedOnStyleID != null)
            {
                style.Append(new BasedOn { Val = basedOnStyleID });
            }

            var rsid = new Rsid();

            // style.Append(rsid);
            var primaryStyle = new PrimaryStyle();
            style.Append(primaryStyle);
            if (nextStyleID != null)
            {
                style.Append(new NextParagraphStyle { Val = nextStyleID });
            }

            style.Append(rPr);
            style.Append(pPr);
            return style;
        }

        /// <summary>
        /// The add styles.
        /// </summary>
        /// <param name="sdp">
        /// The sdp.
        /// </param>
        /// <param name="style">
        /// The style.
        /// </param>
        private void AddStyles(StyleDefinitionsPart sdp, ReportStyle style)
        {
            sdp.Styles = new Styles();

            sdp.Styles.Append(CreateStyle(style.BodyTextStyle, BodyTextID, BodyTextName, null, null, true, false));
            for (int i = 0; i < style.HeaderStyles.Length; i++)
            {
                sdp.Styles.Append(
                    CreateStyle(
                        style.HeaderStyles[i],
                        string.Format(HeaderID, i + 1),
                        string.Format(HeaderName, i + 1),
                        "Heading1",
                        BodyTextID,
                        false,
                        false));
            }

            sdp.Styles.Append(CreateStyle(style.TableTextStyle, TableTextID, TableTextName, null, null));
            sdp.Styles.Append(CreateStyle(style.TableHeaderStyle, TableHeaderID, TableHeaderName, null, null));
            sdp.Styles.Append(CreateStyle(style.TableCaptionStyle, TableCaptionID, TableCaptionName, null, null));

            sdp.Styles.Append(CreateStyle(style.FigureTextStyle, FigureTextID, FigureTextName, null, null));
        }

        /// <summary>
        /// The append image.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        private void AppendImage(string source, string name)
        {
            // http://msdn.microsoft.com/en-us/library/bb497430.aspx
            string ext = Path.GetExtension(source).ToLower();
            ImagePartType ipt = ImagePartType.Jpeg;
            if (ext == ".png")
            {
                ipt = ImagePartType.Png;
            }

            var imagePart = this.mainPart.AddImagePart(ipt);
            using (var stream = new FileStream(source, FileMode.Open))
            {
                imagePart.FeedData(stream);
            }

            using (var bmp = new Bitmap(source))
            {
                double width = bmp.Width / bmp.HorizontalResolution; // inches
                double height = bmp.Height / bmp.VerticalResolution; // inches
                double w = 15 / 2.54;
                double h = height / width * w;
                this.body.Append(this.CreateImageParagraph(this.mainPart.GetIdOfPart(imagePart), name, source, w, h));
            }
        }

        /// <summary>
        /// The create document.
        /// </summary>
        /// <returns>
        /// </returns>
        private Document CreateDocument()
        {
            var d = new Document { MCAttributes = new MarkupCompatibilityAttributes { Ignorable = "w14 wp14" } };
            d.AddNamespaceDeclaration("wpc", "http://schemas.microsoft.com/office/word/2010/wordprocessingCanvas");
            d.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            d.AddNamespaceDeclaration("o", "urn:schemas-microsoft-com:office:office");
            d.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            d.AddNamespaceDeclaration("m", "http://schemas.openxmlformats.org/officeDocument/2006/math");
            d.AddNamespaceDeclaration("v", "urn:schemas-microsoft-com:vml");
            d.AddNamespaceDeclaration("wp14", "http://schemas.microsoft.com/office/word/2010/wordprocessingDrawing");
            d.AddNamespaceDeclaration("wp", "http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing");
            d.AddNamespaceDeclaration("w10", "urn:schemas-microsoft-com:office:word");
            d.AddNamespaceDeclaration("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
            d.AddNamespaceDeclaration("w14", "http://schemas.microsoft.com/office/word/2010/wordml");
            d.AddNamespaceDeclaration("wpg", "http://schemas.microsoft.com/office/word/2010/wordprocessingGroup");
            d.AddNamespaceDeclaration("wpi", "http://schemas.microsoft.com/office/word/2010/wordprocessingInk");
            d.AddNamespaceDeclaration("wne", "http://schemas.microsoft.com/office/word/2006/wordml");
            d.AddNamespaceDeclaration("wps", "http://schemas.microsoft.com/office/word/2010/wordprocessingShape");
            return d;
        }

        /// <summary>
        /// The create image paragraph.
        /// </summary>
        /// <param name="relationshipId">
        /// The relationship id.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        /// <returns>
        /// </returns>
        private DocumentFormat.OpenXml.Wordprocessing.Paragraph CreateImageParagraph(
            string relationshipId, string name, string description, double width, double height)
        {
            // http://msdn.microsoft.com/en-us/library/documentformat.openxml.drawing.extents.aspx
            // http://polymathprogrammer.com/2009/10/22/english-metric-units-and-open-xml/

            // cx (Extent Length)
            // Specifies the length of the extents rectangle in EMUs. This rectangle shall dictate the size of the object as displayed (the result of any scaling to the original object).
            // Example: Consider a DrawingML object specified as follows:
            // <… cx="1828800" cy="200000"/>
            // The cx attributes specifies that this object has a height of 1828800 EMUs (English Metric Units). end example]
            // The possible values for this attribute are defined by the ST_PositiveCoordinate simple type (§20.1.10.42).

            // cy (Extent Width)
            // Specifies the width of the extents rectangle in EMUs. This rectangle shall dictate the size of the object as displayed (the result of any scaling to the original object).
            // Example: Consider a DrawingML object specified as follows:
            // < … cx="1828800" cy="200000"/>
            // The cy attribute specifies that this object has a width of 200000 EMUs (English Metric Units). end example]
            // The possible values for this attribute are defined by the ST_PositiveCoordinate simple type (§20.1.10.42).
            var paragraph1 = new DocumentFormat.OpenXml.Wordprocessing.Paragraph
                {
                   RsidParagraphAddition = "00D91137", RsidRunAdditionDefault = "00AC08EB"
                };

            var run1 = new Run();

            var runProperties1 = new RunProperties();
            var noProof1 = new NoProof();

            runProperties1.Append(noProof1);

            var drawing1 = new Drawing();

            var inline1 = new Inline
                {
                   DistanceFromTop = 0U, DistanceFromBottom = 0U, DistanceFromLeft = 0U, DistanceFromRight = 0U
                };
            var extent1 = new Extent { Cx = 5753100L, Cy = 3600450L };
            extent1.Cx = (long)(width * 914400);
            extent1.Cy = (long)(height * 914400);

            var effectExtent1 = new EffectExtent { LeftEdge = 0L, TopEdge = 0L, RightEdge = 0L, BottomEdge = 0L };
            var docProperties1 = new DocProperties { Id = 1U, Name = name, Description = description };

            var nonVisualGraphicFrameDrawingProperties1 = new NonVisualGraphicFrameDrawingProperties();

            var graphicFrameLocks1 = new GraphicFrameLocks { NoChangeAspect = true };
            graphicFrameLocks1.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");

            nonVisualGraphicFrameDrawingProperties1.Append(graphicFrameLocks1);

            var graphic1 = new Graphic();
            graphic1.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");

            var graphicData1 = new GraphicData { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" };

            var picture1 = new Picture();
            picture1.AddNamespaceDeclaration("pic", "http://schemas.openxmlformats.org/drawingml/2006/picture");

            var nonVisualPictureProperties1 = new NonVisualPictureProperties();
            var nonVisualDrawingProperties1 = new NonVisualDrawingProperties
                {
                   Id = 0U, Name = name, Description = description
                };

            var nonVisualPictureDrawingProperties1 = new NonVisualPictureDrawingProperties();
            var pictureLocks1 = new PictureLocks { NoChangeAspect = true, NoChangeArrowheads = true };

            nonVisualPictureDrawingProperties1.Append(pictureLocks1);

            nonVisualPictureProperties1.Append(nonVisualDrawingProperties1);
            nonVisualPictureProperties1.Append(nonVisualPictureDrawingProperties1);

            var blipFill1 = new BlipFill();

            var blip1 = new Blip { Embed = relationshipId };

            var blipExtensionList1 = new BlipExtensionList();

            var blipExtension1 = new BlipExtension { Uri = "{28A0092B-C50C-407E-A947-70E740481C1C}" };

            var useLocalDpi1 = new UseLocalDpi { Val = false };
            useLocalDpi1.AddNamespaceDeclaration("a14", "http://schemas.microsoft.com/office/drawing/2010/main");

            blipExtension1.Append(useLocalDpi1);

            blipExtensionList1.Append(blipExtension1);

            blip1.Append(blipExtensionList1);
            var sourceRectangle1 = new SourceRectangle();

            var stretch1 = new Stretch();
            var fillRectangle1 = new FillRectangle();

            stretch1.Append(fillRectangle1);

            blipFill1.Append(blip1);
            blipFill1.Append(sourceRectangle1);
            blipFill1.Append(stretch1);

            var shapeProperties1 = new ShapeProperties { BlackWhiteMode = BlackWhiteModeValues.Auto };

            var transform2D1 = new Transform2D();
            var offset1 = new Offset { X = 0L, Y = 0L };
            var extents1 = new Extents { Cx = extent1.Cx, Cy = extent1.Cy };

            transform2D1.Append(offset1);
            transform2D1.Append(extents1);

            var presetGeometry1 = new PresetGeometry { Preset = ShapeTypeValues.Rectangle };
            var adjustValueList1 = new AdjustValueList();

            presetGeometry1.Append(adjustValueList1);
            var noFill1 = new NoFill();

            var outline1 = new Outline();
            var noFill2 = new NoFill();

            outline1.Append(noFill2);

            shapeProperties1.Append(transform2D1);
            shapeProperties1.Append(presetGeometry1);
            shapeProperties1.Append(noFill1);
            shapeProperties1.Append(outline1);

            picture1.Append(nonVisualPictureProperties1);
            picture1.Append(blipFill1);
            picture1.Append(shapeProperties1);

            graphicData1.Append(picture1);

            graphic1.Append(graphicData1);

            inline1.Append(extent1);
            inline1.Append(effectExtent1);
            inline1.Append(docProperties1);
            inline1.Append(nonVisualGraphicFrameDrawingProperties1);
            inline1.Append(graphic1);

            drawing1.Append(inline1);

            run1.Append(runProperties1);
            run1.Append(drawing1);

            paragraph1.Append(run1);

            return paragraph1;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        /// </param>
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (!this.isSaved)
                    {
                        this.Save();
                    }

                    this.package.Close();
                }
            }

            this.disposed = true;
        }

        /// <summary>
        /// The set package properties.
        /// </summary>
        /// <param name="p">
        /// The p.
        /// </param>
        private void SetPackageProperties(OpenXmlPackage p)
        {
            p.PackageProperties.Creator = this.Creator;
            p.PackageProperties.Title = this.Title;
            p.PackageProperties.Subject = this.Subject;
            p.PackageProperties.Description = this.Description;
            p.PackageProperties.Keywords = this.Keywords;
            p.PackageProperties.Version = this.Version;
            p.PackageProperties.Revision = this.Revision;

            p.PackageProperties.Created = DateTime.Now;
            p.PackageProperties.Modified = DateTime.Now;
            p.PackageProperties.LastModifiedBy = this.Creator;
        }

    }
}