// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WordDocumentReportWriter.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a report writer for Word/OpenXML output using OpenXML SDK 2.0.
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
    /// Provides a report writer for Word/OpenXML output using OpenXML SDK 2.0.
    /// </summary>
    public class WordDocumentReportWriter : IDisposable, IReportWriter
    {
        //// http://www.codeproject.com/KB/office/OpenXML-SDK-HelloWorld.aspx
        //// http://msdn.microsoft.com/en-us/library/documentformat.openxml.wordprocessing.aspx

        /// <summary>
        /// The body text id.
        /// </summary>
        private const string BodyTextId = "Normal";

        /// <summary>
        /// The body text name.
        /// </summary>
        private const string BodyTextName = "Normal";

        /// <summary>
        /// The figure text id.
        /// </summary>
        private const string FigureTextId = "FigureText";

        /// <summary>
        /// The figure text name.
        /// </summary>
        private const string FigureTextName = "Figure text";

        /// <summary>
        /// The header id.
        /// </summary>
        private const string HeaderId = "Heading{0}";

        /// <summary>
        /// The header name.
        /// </summary>
        private const string HeaderName = "Heading {0}";

        /// <summary>
        /// The table caption id.
        /// </summary>
        private const string TableCaptionId = "TableCaption";

        /// <summary>
        /// The table caption name.
        /// </summary>
        private const string TableCaptionName = "Table caption";

        /// <summary>
        /// The table header id.
        /// </summary>
        private const string TableHeaderId = "TableHeader";

        /// <summary>
        /// The table header name.
        /// </summary>
        private const string TableHeaderName = "Table header";

        /// <summary>
        /// The table text id.
        /// </summary>
        private const string TableTextId = "TableText";

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
        /// Initializes a new instance of the <see cref="WordDocumentReportWriter" /> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public WordDocumentReportWriter(string filePath)
        {
            this.FileName = filePath;
            this.package = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document);

            this.mainPart = this.package.AddMainDocumentPart();

            this.stylePart = this.mainPart.AddNewPart<StyleDefinitionsPart>();

            // fontTablePart = mainPart.AddNewPart<FontTablePart>();
            this.document = CreateDocument();
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
        /// <value>The name of the file.</value>
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
            this.document.AppendChild(this.body);
            this.mainPart.Document = this.document;

            this.stylePart.Styles.Save();
            this.mainPart.Document.Save();
            this.isSaved = true;
        }

        /// <summary>
        /// The write drawing.
        /// </summary>
        /// <param name="d">The d.</param>
        public void WriteDrawing(DrawingFigure d)
        {
            this.body.AppendChild(CreateParagraph("DrawingFigures are not yet supported."));
        }

        /// <summary>
        /// The write equation.
        /// </summary>
        /// <param name="equation">The equation.</param>
        public void WriteEquation(Equation equation)
        {
            this.body.AppendChild(CreateParagraph("Equations are not yet supported."));
        }

        /// <summary>
        /// The write header.
        /// </summary>
        /// <param name="h">The h.</param>
        public void WriteHeader(Header h)
        {
            this.body.AppendChild(CreateParagraph(h.Text, string.Format(HeaderId, h.Level)));
        }

        /// <summary>
        /// The write image.
        /// </summary>
        /// <param name="i">The i.</param>
        public void WriteImage(Image i)
        {
            if (i.Source == null)
            {
                return;
            }

            this.AppendImage(i.Source, "Picture " + i.FigureNumber);

            this.body.AppendChild(CreateParagraph(i.GetFullCaption(this.style), FigureTextId));
        }

        /// <summary>
        /// The write paragraph.
        /// </summary>
        /// <param name="pa">The pa.</param>
        public void WriteParagraph(Paragraph pa)
        {
            this.body.AppendChild(CreateParagraph(pa.Text));
        }

        /// <summary>
        /// The write plot.
        /// </summary>
        /// <param name="plot">The plot.</param>
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
            PngExporter.Export(plot.PlotModel, sourceFullPath, (int)plot.Width, (int)plot.Height, OxyColors.Automatic);

            // append the image to the document
            this.AppendImage(sourceFullPath, "Plot" + plot.FigureNumber);

            this.body.AppendChild(CreateParagraph(plot.GetFullCaption(this.style), FigureTextId));
        }

        /// <summary>
        /// The write report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="reportStyle">The style.</param>
        public void WriteReport(Report report, ReportStyle reportStyle)
        {
            this.style = reportStyle;
            this.AddStyles(this.stylePart, reportStyle);
            report.Write(this);
        }

        /// <summary>
        /// The write table.
        /// </summary>
        /// <param name="t">The t.</param>
        public void WriteTable(Table t)
        {
            this.body.AppendChild(CreateParagraph(t.GetFullCaption(this.style), TableCaptionId));

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

            tableProperties1.AppendChild(tableStyle1);
            tableProperties1.AppendChild(tableWidth1);
            tableProperties1.AppendChild(tableLook1);

            var tableGrid1 = new TableGrid();
            // ReSharper disable once UnusedVariable
            foreach (var tc in t.Columns)
            {
                // TODO: use tc.Width to set the width of the column
                var gridColumn1 = new GridColumn { Width = "3070" };
                tableGrid1.AppendChild(gridColumn1);
            }

            foreach (var row in t.Rows)
            {
                var tr = new TableRow();

                if (row.IsHeader)
                {
                    var trp = new TableRowProperties();
                    var tableHeader1 = new TableHeader();
                    trp.AppendChild(tableHeader1);
                    tr.AppendChild(trp);
                }

                int j = 0;
                foreach (var c in row.Cells)
                {
                    bool isHeader = row.IsHeader || t.Columns[j++].IsHeader;
                    var cell = new TableCell();
                    var tcp = new TableCellProperties();
                    var borders = new TableCellBorders();
                    borders.AppendChild(
                        new BottomBorder
                            {
                                Val = BorderValues.Single,
                                Size = 4U,
                                Space = 0U,
                                Color = "auto"
                            });
                    borders.AppendChild(
                        new TopBorder
                            {
                                Val = BorderValues.Single,
                                Size = 4U,
                                Space = 0U,
                                Color = "auto"
                            });
                    borders.AppendChild(
                        new LeftBorder
                            {
                                Val = BorderValues.Single,
                                Size = 4U,
                                Space = 0U,
                                Color = "auto"
                            });
                    borders.AppendChild(
                        new RightBorder
                            {
                                Val = BorderValues.Single,
                                Size = 4U,
                                Space = 0U,
                                Color = "auto"
                            });
                    tcp.AppendChild(borders);

                    cell.AppendChild(tcp);
                    string styleId = isHeader ? "TableHeader" : "TableText";
                    cell.AppendChild(CreateParagraph(c.Content, styleId));
                    tr.AppendChild(cell);
                }

                table.AppendChild(tr);
            }

            this.body.AppendChild(table);
        }

        /// <summary>
        /// The create paragraph.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="styleId">The style id.</param>
        /// <returns>The paragraph.</returns>
        private static DocumentFormat.OpenXml.Wordprocessing.Paragraph CreateParagraph(string content, string styleId = null)
        {
            var p = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();

            if (styleId != null)
            {
                var pp = new ParagraphProperties { ParagraphStyleId = new ParagraphStyleId { Val = styleId } };
                p.AppendChild(pp);
            }

            var text = new Text(content);
            var run = new Run(text);
            p.AppendChild(run);
            return p;
        }

        /// <summary>
        /// Creates the document.
        /// </summary>
        /// <returns>The <see cref="Document" />.</returns>
        private static Document CreateDocument()
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
        /// Creates a style.
        /// </summary>
        /// <param name="ps">The paragraph style.</param>
        /// <param name="styleId">The style id.</param>
        /// <param name="styleName">The style name.</param>
        /// <param name="basedOnStyleId">The based on style id.</param>
        /// <param name="nextStyleId">The next style id.</param>
        /// <param name="isDefault"><c>true</c> if the style is default.</param>
        /// <param name="isCustomStyle"><c>true</c> if the style is a custom style.</param>
        /// <returns>The <see cref="Style" />.</returns>
        private static Style CreateStyle(
            ParagraphStyle ps,
            string styleId,
            string styleName,
            string basedOnStyleId,
            string nextStyleId,
            bool isDefault = false,
            bool isCustomStyle = true)
        {
            // todo: add font to FontTable?
            var rPr = new StyleRunProperties();

            // http://msdn.microsoft.com/en-us/library/documentformat.openxml.wordprocessing.color.aspx
            var color = new Color { Val = ps.TextColor.ToString().Trim('#').Substring(2) };
            rPr.AppendChild(color);

            // http://msdn.microsoft.com/en-us/library/cc850848.aspx
            rPr.AppendChild(new RunFonts { Ascii = ps.FontFamily, HighAnsi = ps.FontFamily });
            rPr.AppendChild(new FontSize { Val = new StringValue((ps.FontSize * 2).ToString(CultureInfo.InvariantCulture)) });
            rPr.AppendChild(
                new FontSizeComplexScript
                    {
                        Val = new StringValue((ps.FontSize * 2).ToString(CultureInfo.InvariantCulture))
                    });

            if (ps.Bold)
            {
                rPr.AppendChild(new Bold());
            }

            if (ps.Italic)
            {
                rPr.AppendChild(new Italic());
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

            pPr.AppendChild(spacingBetweenLines2);
            pPr.AppendChild(contextualSpacing1);
            pPr.AppendChild(indentation);

            // StyleRunProperties styleRunProperties7 = new StyleRunProperties();
            // RunFonts runFonts8 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana" };
            // Color color7 = new Color() { Val = "000000" };

            // styleRunProperties7.AppendChild(runFonts8);
            // styleRunProperties7.AppendChild(color7);

            // http://msdn.microsoft.com/en-us/library/documentformat.openxml.wordprocessing.style.aspx
            var style = new Style
                {
                    Default = new OnOffValue(isDefault),
                    CustomStyle = new OnOffValue(isCustomStyle),
                    StyleId = styleId,
                    Type = StyleValues.Paragraph
                };

            style.AppendChild(new Name { Val = styleName });
            if (basedOnStyleId != null)
            {
                style.AppendChild(new BasedOn { Val = basedOnStyleId });
            }

            //// var rsid = new Rsid();

            // style.AppendChild(rsid);
            var primaryStyle = new PrimaryStyle();
            style.AppendChild(primaryStyle);
            if (nextStyleId != null)
            {
                style.AppendChild(new NextParagraphStyle { Val = nextStyleId });
            }

            style.AppendChild(rPr);
            style.AppendChild(pPr);
            return style;
        }

        /// <summary>
        /// Adds a style to the specified part.
        /// </summary>
        /// <param name="sdp">The style definition part.</param>
        /// <param name="styleToAdd">The style.</param>
        private void AddStyles(StyleDefinitionsPart sdp, ReportStyle styleToAdd)
        {
            sdp.Styles = new Styles();

            sdp.Styles.AppendChild(CreateStyle(styleToAdd.BodyTextStyle, BodyTextId, BodyTextName, null, null, true, false));
            for (int i = 0; i < styleToAdd.HeaderStyles.Length; i++)
            {
                sdp.Styles.AppendChild(
                    CreateStyle(
                        styleToAdd.HeaderStyles[i],
                        string.Format(HeaderId, i + 1),
                        string.Format(HeaderName, i + 1),
                        "Heading1",
                        BodyTextId,
                        false,
                        false));
            }

            sdp.Styles.AppendChild(CreateStyle(styleToAdd.TableTextStyle, TableTextId, TableTextName, null, null));
            sdp.Styles.AppendChild(CreateStyle(styleToAdd.TableHeaderStyle, TableHeaderId, TableHeaderName, null, null));
            sdp.Styles.AppendChild(CreateStyle(styleToAdd.TableCaptionStyle, TableCaptionId, TableCaptionName, null, null));

            sdp.Styles.AppendChild(CreateStyle(styleToAdd.FigureTextStyle, FigureTextId, FigureTextName, null, null));
        }

        /// <summary>
        /// The append image.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="name">The name.</param>
        private void AppendImage(string source, string name)
        {
            // http://msdn.microsoft.com/en-us/library/bb497430.aspx
            var ext = (Path.GetExtension(source) ?? string.Empty).ToLower();
            var ipt = ImagePartType.Jpeg;
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
                this.body.AppendChild(this.CreateImageParagraph(this.mainPart.GetIdOfPart(imagePart), name, source, w, h));
            }
        }

        /// <summary>
        /// Creates an image paragraph.
        /// </summary>
        /// <param name="relationshipId">The relationship id.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns>The <see cref="Paragraph" /> containing the image.</returns>
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
                    RsidParagraphAddition = "00D91137",
                    RsidRunAdditionDefault = "00AC08EB"
                };

            var run1 = new Run();

            var runProperties1 = new RunProperties();
            var noProof1 = new NoProof();

            runProperties1.AppendChild(noProof1);

            var drawing1 = new Drawing();

            var inline1 = new Inline
                {
                    DistanceFromTop = 0U,
                    DistanceFromBottom = 0U,
                    DistanceFromLeft = 0U,
                    DistanceFromRight = 0U
                };
            var extent1 = new Extent { Cx = 5753100L, Cy = 3600450L };
            extent1.Cx = (long)(width * 914400);
            extent1.Cy = (long)(height * 914400);

            var effectExtent1 = new EffectExtent { LeftEdge = 0L, TopEdge = 0L, RightEdge = 0L, BottomEdge = 0L };
            var docProperties1 = new DocProperties { Id = 1U, Name = name, Description = description };

            var nonVisualGraphicFrameDrawingProperties1 = new NonVisualGraphicFrameDrawingProperties();

            var graphicFrameLocks1 = new GraphicFrameLocks { NoChangeAspect = true };
            graphicFrameLocks1.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");

            nonVisualGraphicFrameDrawingProperties1.AppendChild(graphicFrameLocks1);

            var graphic1 = new Graphic();
            graphic1.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");

            var graphicData1 = new GraphicData { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" };

            var picture1 = new Picture();
            picture1.AddNamespaceDeclaration("pic", "http://schemas.openxmlformats.org/drawingml/2006/picture");

            var nonVisualPictureProperties1 = new NonVisualPictureProperties();
            var nonVisualDrawingProperties1 = new NonVisualDrawingProperties
                {
                    Id = 0U,
                    Name = name,
                    Description = description
                };

            var nonVisualPictureDrawingProperties1 = new NonVisualPictureDrawingProperties();
            var pictureLocks1 = new PictureLocks { NoChangeAspect = true, NoChangeArrowheads = true };

            nonVisualPictureDrawingProperties1.AppendChild(pictureLocks1);

            nonVisualPictureProperties1.AppendChild(nonVisualDrawingProperties1);
            nonVisualPictureProperties1.AppendChild(nonVisualPictureDrawingProperties1);

            var blipFill1 = new BlipFill();

            var blip1 = new Blip { Embed = relationshipId };

            var blipExtensionList1 = new BlipExtensionList();

            var blipExtension1 = new BlipExtension { Uri = "{28A0092B-C50C-407E-A947-70E740481C1C}" };

            var useLocalDpi1 = new UseLocalDpi { Val = false };
            useLocalDpi1.AddNamespaceDeclaration("a14", "http://schemas.microsoft.com/office/drawing/2010/main");

            blipExtension1.AppendChild(useLocalDpi1);

            blipExtensionList1.AppendChild(blipExtension1);

            blip1.AppendChild(blipExtensionList1);
            var sourceRectangle1 = new SourceRectangle();

            var stretch1 = new Stretch();
            var fillRectangle1 = new FillRectangle();

            stretch1.AppendChild(fillRectangle1);

            blipFill1.AppendChild(blip1);
            blipFill1.AppendChild(sourceRectangle1);
            blipFill1.AppendChild(stretch1);

            var shapeProperties1 = new ShapeProperties { BlackWhiteMode = BlackWhiteModeValues.Auto };

            var transform2D1 = new Transform2D();
            var offset1 = new Offset { X = 0L, Y = 0L };
            var extents1 = new Extents { Cx = extent1.Cx, Cy = extent1.Cy };

            transform2D1.AppendChild(offset1);
            transform2D1.AppendChild(extents1);

            var presetGeometry1 = new PresetGeometry { Preset = ShapeTypeValues.Rectangle };
            var adjustValueList1 = new AdjustValueList();

            presetGeometry1.AppendChild(adjustValueList1);
            var noFill1 = new NoFill();

            var outline1 = new Outline();
            var noFill2 = new NoFill();

            outline1.AppendChild(noFill2);

            shapeProperties1.AppendChild(transform2D1);
            shapeProperties1.AppendChild(presetGeometry1);
            shapeProperties1.AppendChild(noFill1);
            shapeProperties1.AppendChild(outline1);

            picture1.AppendChild(nonVisualPictureProperties1);
            picture1.AppendChild(blipFill1);
            picture1.AppendChild(shapeProperties1);

            graphicData1.AppendChild(picture1);

            graphic1.AppendChild(graphicData1);

            inline1.AppendChild(extent1);
            inline1.AppendChild(effectExtent1);
            inline1.AppendChild(docProperties1);
            inline1.AppendChild(nonVisualGraphicFrameDrawingProperties1);
            inline1.AppendChild(graphic1);

            drawing1.AppendChild(inline1);

            run1.AppendChild(runProperties1);
            run1.AppendChild(drawing1);

            paragraph1.AppendChild(run1);

            return paragraph1;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
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
        /// Sets the package properties.
        /// </summary>
        /// <param name="p">The package.</param>
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