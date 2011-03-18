using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using OxyPlot.Reporting;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;
using Color = DocumentFormat.OpenXml.Wordprocessing.Color;
using Image = OxyPlot.Reporting.Image;

namespace OxyPlot.Pdf
{
    /// <summary>
    /// Word/OpenXML (.docx) report writer using OpenXML SDK 2.0.
    /// </summary>
    public class WordDocumentReportWriter : IDisposable, IReportWriter
    {
        // http://www.codeproject.com/KB/office/OpenXML-SDK-HelloWorld.aspx
        // http://msdn.microsoft.com/en-us/library/documentformat.openxml.wordprocessing.aspx

        protected WordprocessingDocument package;
        protected MainDocumentPart mainPart;
        protected Document document;
        protected Body body;
        private readonly StyleDefinitionsPart stylePart;

        private const string BodyTextID = "Normal";
        private const string BodyTextName = "Normal";
        private const string HeaderID = "Heading{0}";
        private const string HeaderName = "Heading {0}";
        private const string TableTextID = "TableText";
        private const string TableTextName = "Table text";
        private const string TableHeaderID = "TableHeader";
        private const string TableHeaderName = "Table header";
        private const string TableCaptionID = "TableCaption";
        private const string TableCaptionName = "Table caption";
        private const string FigureTextID = "FigureText";
        private const string FigureTextName = "Figure text";

        public WordDocumentReportWriter(string filePath)
        {
            package = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document);

            mainPart = package.AddMainDocumentPart();

            stylePart = mainPart.AddNewPart<StyleDefinitionsPart>();

            // fontTablePart = mainPart.AddNewPart<FontTablePart>();

            document = CreateDocument();
            body = new Body();
        }

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

        private void AddStyles(StyleDefinitionsPart sdp, ReportStyle style)
        {
            sdp.Styles = new Styles();

            sdp.Styles.Append(CreateStyle(style.BodyTextStyle, BodyTextID, BodyTextName, null, null, true, false));
            for (int i = 0; i < style.HeaderStyles.Length; i++)
                sdp.Styles.Append(CreateStyle(style.HeaderStyles[i], String.Format(HeaderID, i + 1),
                                              String.Format(HeaderName, i + 1), "Heading1", BodyTextID, false, false));

            sdp.Styles.Append(CreateStyle(style.TableTextStyle, TableTextID, TableTextName, null, null));
            sdp.Styles.Append(CreateStyle(style.TableHeaderStyle, TableHeaderID, TableHeaderName, null, null));
            sdp.Styles.Append(CreateStyle(style.TableCaptionStyle, TableCaptionID, TableCaptionName, null, null));

            sdp.Styles.Append(CreateStyle(style.FigureTextStyle, FigureTextID, FigureTextName, null, null));
        }

        public string Creator { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Revision { get; set; }
        public string Version { get; set; }
        public string Keywords { get; set; }

        private void SetPackageProperties(OpenXmlPackage p)
        {
            p.PackageProperties.Creator = Creator;
            p.PackageProperties.Title = Title;
            p.PackageProperties.Subject = Subject;
            p.PackageProperties.Description = Description;
            p.PackageProperties.Keywords = Keywords;
            p.PackageProperties.Version = Version;
            p.PackageProperties.Revision = Revision;

            p.PackageProperties.Created = DateTime.Now;
            p.PackageProperties.Modified = DateTime.Now;
            p.PackageProperties.LastModifiedBy = Creator;
        }

        private static Style CreateStyle(ParagraphStyle ps, string styleID, string styleName, string basedOnStyleID,
                                         string nextStyleID, bool isDefault = false, bool isCustomStyle = true)
        {
            // todo: add font to FontTable?

            var rPr = new StyleRunProperties();

            // http://msdn.microsoft.com/en-us/library/documentformat.openxml.wordprocessing.color.aspx
            Color color = new Color { Val = ps.TextColor.ToString().Trim('#').Substring(2) };
            rPr.Append(color);

            // http://msdn.microsoft.com/en-us/library/cc850848.aspx
            rPr.Append(new RunFonts { Ascii = ps.FontFamily, HighAnsi = ps.FontFamily });
            rPr.Append(new FontSize { Val = new StringValue((ps.FontSize * 2).ToString(CultureInfo.InvariantCulture)) });
            rPr.Append(new FontSizeComplexScript { Val = new StringValue((ps.FontSize * 2).ToString(CultureInfo.InvariantCulture)) });

            if (ps.Bold)
                rPr.Append(new Bold());
            if (ps.Italic)
                rPr.Append(new Italic());


            var pPr = new StyleParagraphProperties();
            var spacingBetweenLines2 = new SpacingBetweenLines
            {
                After = String.Format(CultureInfo.InvariantCulture, "{0}", ps.SpacingAfter * 20),
                Before = String.Format(CultureInfo.InvariantCulture, "{0}", ps.SpacingBefore * 20),
                Line = String.Format(CultureInfo.InvariantCulture, "{0}", ps.LineSpacing * 240),
                LineRule = LineSpacingRuleValues.Auto
            };
            var indentation = new Indentation()
                                  {
                                      Left = String.Format(CultureInfo.InvariantCulture, "{0}", ps.LeftIndentation * 20),
                                      Right = String.Format(CultureInfo.InvariantCulture, "{0}", ps.RightIndentation * 20)
                                  };
            var contextualSpacing1 = new ContextualSpacing();

            pPr.Append(spacingBetweenLines2);
            pPr.Append(contextualSpacing1);
            pPr.Append(indentation);

            //StyleRunProperties styleRunProperties7 = new StyleRunProperties();
            //RunFonts runFonts8 = new RunFonts() { Ascii = "Verdana", HighAnsi = "Verdana" };
            //Color color7 = new Color() { Val = "000000" };

            //styleRunProperties7.Append(runFonts8);
            //styleRunProperties7.Append(color7);






            // http://msdn.microsoft.com/en-us/library/documentformat.openxml.wordprocessing.style.aspx
            Style style = new Style
                              {
                                  Default = new OnOffValue(isDefault),
                                  CustomStyle = new OnOffValue(isCustomStyle),
                                  StyleId = styleID,
                                  Type = StyleValues.Paragraph
                              };

            style.Append(new Name { Val = styleName });
            if (basedOnStyleID != null) style.Append(new BasedOn { Val = basedOnStyleID });
            Rsid rsid = new Rsid();
            //style.Append(rsid);
            var primaryStyle = new PrimaryStyle();
            style.Append(primaryStyle);
            if (nextStyleID != null) style.Append(new NextParagraphStyle { Val = nextStyleID });
            style.Append(rPr);
            style.Append(pPr);
            return style;
        }

        #region IDisposable Members

        public void Dispose()
        {
            SetPackageProperties(package);
            document.Append(body);
            mainPart.Document = document;

            stylePart.Styles.Save();
            mainPart.Document.Save();
            package.Close();
        }

        #endregion

        #region IReportWriter Members

        private ReportStyle style;
        public void WriteReport(Report report, ReportStyle style)
        {
            this.style = style;
            AddStyles(stylePart, style);
            report.Write(this);
        }

        public void WriteHeader(OxyPlot.Reporting.Header h)
        {
            body.AppendChild(CreateParagraph(h.Text, String.Format(HeaderID, h.Level)));
        }

        public void WriteParagraph(OxyPlot.Reporting.Paragraph pa)
        {
            body.AppendChild(CreateParagraph(pa.Text));
        }

        private static DocumentFormat.OpenXml.Wordprocessing.Paragraph CreateParagraph(string content, string styleID = null)
        {
            var p = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();

            if (styleID != null)
            {
                var pp = new ParagraphProperties
                             {
                                 ParagraphStyleId = new ParagraphStyleId { Val = styleID }
                             };
                p.Append(pp);
            }

            var text = new Text(content);
            var run = new Run(text);
            p.Append(run);
            return p;
        }

        public void WriteTable(OxyPlot.Reporting.Table t)
        {
            body.Append(CreateParagraph(t.GetFullCaption(style), TableCaptionID));

            var table = new DocumentFormat.OpenXml.Wordprocessing.Table();

            var tableProperties1 = new TableProperties();
            var tableStyle1 = new TableStyle() { Val = "TableGrid" };
            var tableWidth1 = new TableWidth() { Width = "0", Type = TableWidthUnitValues.Auto };
            var tableLook1 = new TableLook() { Val = "04A0", FirstRow = true, LastRow = false, FirstColumn = true, LastColumn = false, NoHorizontalBand = false, NoVerticalBand = true };

            tableProperties1.Append(tableStyle1);
            tableProperties1.Append(tableWidth1);
            tableProperties1.Append(tableLook1);

            TableGrid tableGrid1 = new TableGrid();
            foreach (var tc in t.Columns)
            {
                // tc.Width
                GridColumn gridColumn1 = new GridColumn() { Width = "3070" };
                tableGrid1.Append(gridColumn1);
            }

            foreach (var row in t.Rows)
            {
                var tr = new DocumentFormat.OpenXml.Wordprocessing.TableRow();
                int j = 0;
                foreach (var c in row.Cells)
                {
                    bool isHeader = row.IsHeader || t.Columns[j++].IsHeader;
                    var cell = new DocumentFormat.OpenXml.Wordprocessing.TableCell();
                    var tcp = new TableCellProperties();
                    var borders = new TableCellBorders();
                    borders.Append(new BottomBorder() { Val = BorderValues.Single, Size = (UInt32Value)4U, Space = (UInt32Value)0U, Color = "auto" });
                    borders.Append(new TopBorder() { Val = BorderValues.Single, Size = (UInt32Value)4U, Space = (UInt32Value)0U, Color = "auto" });
                    borders.Append(new LeftBorder() { Val = BorderValues.Single, Size = (UInt32Value)4U, Space = (UInt32Value)0U, Color = "auto" });
                    borders.Append(new RightBorder() { Val = BorderValues.Single, Size = (UInt32Value)4U, Space = (UInt32Value)0U, Color = "auto" });
                    tcp.Append(borders);

                    cell.Append(tcp);
                    string styleID = isHeader ? "TableHeader" : "TableText";
                    cell.Append(CreateParagraph(c.Content, styleID));
                    tr.Append(cell);
                }
                table.Append(tr);
            }

            body.Append(table);
        }

        public void WriteImage(Image i)
        {
            if (i.Source == null)
                return;

            // http://msdn.microsoft.com/en-us/library/bb497430.aspx
            var ext = Path.GetExtension(i.Source).ToLower();
            var ipt = ImagePartType.Jpeg;
            if (ext == ".png")
                ipt = ImagePartType.Png;

            var imagePart = mainPart.AddImagePart(ipt);
            using (FileStream stream = new FileStream(i.Source, FileMode.Open))
            {
                imagePart.FeedData(stream);
            }
            using (var bmp = new Bitmap(i.Source))
            {
                double width = bmp.Width / bmp.HorizontalResolution; // inches
                double height = bmp.Height / bmp.VerticalResolution; // inches
                double w = 15 / 2.54;
                double h = height / width * w;
                body.Append(CreateImageParagraph(mainPart.GetIdOfPart(imagePart), "Picture " + i.FigureNumber, i.Source, w, h));
            }
            body.Append(CreateParagraph(i.GetFullCaption(style), FigureTextID));
        }

        private DocumentFormat.OpenXml.Wordprocessing.Paragraph CreateImageParagraph(string relationshipId,
                                                                                     string name,
                                                                                     string description, double width, double height)
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




            var paragraph1 = new DocumentFormat.OpenXml.Wordprocessing.Paragraph() { RsidParagraphAddition = "00D91137", RsidRunAdditionDefault = "00AC08EB" };

            var run1 = new Run();

            var runProperties1 = new RunProperties();
            var noProof1 = new NoProof();

            runProperties1.Append(noProof1);

            var drawing1 = new Drawing();

            var inline1 = new DocumentFormat.OpenXml.Drawing.Wordprocessing.Inline() { DistanceFromTop = (UInt32Value)0U, DistanceFromBottom = (UInt32Value)0U, DistanceFromLeft = (UInt32Value)0U, DistanceFromRight = (UInt32Value)0U };
            var extent1 = new DocumentFormat.OpenXml.Drawing.Wordprocessing.Extent() { Cx = 5753100L, Cy = 3600450L };
            extent1.Cx = (long)(width * 914400);
            extent1.Cy = (long)(height * 914400);

            var effectExtent1 = new DocumentFormat.OpenXml.Drawing.Wordprocessing.EffectExtent() { LeftEdge = 0L, TopEdge = 0L, RightEdge = 0L, BottomEdge = 0L };
            var docProperties1 = new DocumentFormat.OpenXml.Drawing.Wordprocessing.DocProperties() { Id = (UInt32Value)1U, Name = name, Description = description };

            var nonVisualGraphicFrameDrawingProperties1 = new DocumentFormat.OpenXml.Drawing.Wordprocessing.NonVisualGraphicFrameDrawingProperties();

            var graphicFrameLocks1 = new DocumentFormat.OpenXml.Drawing.GraphicFrameLocks() { NoChangeAspect = true };
            graphicFrameLocks1.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");

            nonVisualGraphicFrameDrawingProperties1.Append(graphicFrameLocks1);

            var graphic1 = new DocumentFormat.OpenXml.Drawing.Graphic();
            graphic1.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");

            var graphicData1 = new DocumentFormat.OpenXml.Drawing.GraphicData() { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" };

            var picture1 = new DocumentFormat.OpenXml.Drawing.Pictures.Picture();
            picture1.AddNamespaceDeclaration("pic", "http://schemas.openxmlformats.org/drawingml/2006/picture");

            var nonVisualPictureProperties1 = new DocumentFormat.OpenXml.Drawing.Pictures.NonVisualPictureProperties();
            var nonVisualDrawingProperties1 = new DocumentFormat.OpenXml.Drawing.Pictures.NonVisualDrawingProperties() { Id = (UInt32Value)0U, Name = name, Description = description };

            var nonVisualPictureDrawingProperties1 = new DocumentFormat.OpenXml.Drawing.Pictures.NonVisualPictureDrawingProperties();
            var pictureLocks1 = new DocumentFormat.OpenXml.Drawing.PictureLocks() { NoChangeAspect = true, NoChangeArrowheads = true };

            nonVisualPictureDrawingProperties1.Append(pictureLocks1);

            nonVisualPictureProperties1.Append(nonVisualDrawingProperties1);
            nonVisualPictureProperties1.Append(nonVisualPictureDrawingProperties1);

            var blipFill1 = new DocumentFormat.OpenXml.Drawing.Pictures.BlipFill();

            var blip1 = new DocumentFormat.OpenXml.Drawing.Blip() { Embed = relationshipId };

            var blipExtensionList1 = new DocumentFormat.OpenXml.Drawing.BlipExtensionList();

            var blipExtension1 = new DocumentFormat.OpenXml.Drawing.BlipExtension() { Uri = "{28A0092B-C50C-407E-A947-70E740481C1C}" };

            var useLocalDpi1 = new DocumentFormat.OpenXml.Office2010.Drawing.UseLocalDpi() { Val = false };
            useLocalDpi1.AddNamespaceDeclaration("a14", "http://schemas.microsoft.com/office/drawing/2010/main");

            blipExtension1.Append(useLocalDpi1);

            blipExtensionList1.Append(blipExtension1);

            blip1.Append(blipExtensionList1);
            var sourceRectangle1 = new DocumentFormat.OpenXml.Drawing.SourceRectangle();

            var stretch1 = new DocumentFormat.OpenXml.Drawing.Stretch();
            var fillRectangle1 = new DocumentFormat.OpenXml.Drawing.FillRectangle();

            stretch1.Append(fillRectangle1);

            blipFill1.Append(blip1);
            blipFill1.Append(sourceRectangle1);
            blipFill1.Append(stretch1);

            var shapeProperties1 = new DocumentFormat.OpenXml.Drawing.Pictures.ShapeProperties() { BlackWhiteMode = DocumentFormat.OpenXml.Drawing.BlackWhiteModeValues.Auto };

            var transform2D1 = new DocumentFormat.OpenXml.Drawing.Transform2D();
            var offset1 = new DocumentFormat.OpenXml.Drawing.Offset { X = 0L, Y = 0L };
            var extents1 = new DocumentFormat.OpenXml.Drawing.Extents { Cx = extent1.Cx, Cy = extent1.Cy };

            transform2D1.Append(offset1);
            transform2D1.Append(extents1);

            var presetGeometry1 = new DocumentFormat.OpenXml.Drawing.PresetGeometry() { Preset = DocumentFormat.OpenXml.Drawing.ShapeTypeValues.Rectangle };
            var adjustValueList1 = new DocumentFormat.OpenXml.Drawing.AdjustValueList();

            presetGeometry1.Append(adjustValueList1);
            var noFill1 = new DocumentFormat.OpenXml.Drawing.NoFill();

            var outline1 = new DocumentFormat.OpenXml.Drawing.Outline();
            var noFill2 = new DocumentFormat.OpenXml.Drawing.NoFill();

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

        public void WriteDrawing(OxyPlot.Reporting.DrawingFigure d)
        {
            body.AppendChild(CreateParagraph("DrawingFigures are not yet supported."));
        }

        public void WritePlot(PlotFigure plot)
        {
            body.AppendChild(CreateParagraph("PlotFigures are not yet supported."));
        }

        public void WriteEquation(Equation equation)
        {
            body.AppendChild(CreateParagraph("Equations are not yet supported."));
        }

        #endregion
    }
}