// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportItem.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides a base class for report items.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Provides a base class for report items.
    /// </summary>
    public abstract class ReportItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "ReportItem" /> class.
        /// </summary>
        protected ReportItem()
        {
            this.Children = new Collection<ReportItem>();
        }

        /// <summary>
        /// Gets the children.
        /// </summary>
        public Collection<ReportItem> Children { get; private set; }

        /// <summary>
        /// Gets the report.
        /// </summary>
        public Report Report { get; internal set; }

        /// <summary>
        /// Adds a report item to the report.
        /// </summary>
        /// <param name="child">The child.</param>
        public void Add(ReportItem child)
        {
            this.Children.Add(child);
        }

        /// <summary>
        /// Adds a drawing to the report.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="text">The text.</param>
        public void AddDrawing(string content, string text)
        {
            this.Add(new DrawingFigure { Content = content, FigureText = text });
        }

        /// <summary>
        /// Adds a plot to the report.
        /// </summary>
        /// <param name="plot">The plot model.</param>
        /// <param name="text">The text.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public void AddPlot(PlotModel plot, string text, double width, double height)
        {
            this.Add(new PlotFigure { PlotModel = plot, Width = width, Height = height, FigureText = text });
        }

        /// <summary>
        /// Adds an equation to the report.
        /// </summary>
        /// <param name="equation">The equation.</param>
        /// <param name="caption">The caption.</param>
        public void AddEquation(string equation, string caption = null)
        {
            this.Add(new Equation { Content = equation, Caption = caption });
        }

        /// <summary>
        /// Adds a header to the report.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="header">The header.</param>
        public void AddHeader(int level, string header)
        {
            this.Add(new Header { Level = level, Text = header });
        }

        /// <summary>
        /// Adds an image to the report.
        /// </summary>
        /// <param name="src">The image source file.</param>
        /// <param name="text">The text.</param>
        public void AddImage(string src, string text)
        {
            this.Add(new Image { Source = src, FigureText = text });
        }

        /// <summary>
        /// Adds an items table to the report.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="items">The items.</param>
        /// <param name="fields">The fields.</param>
        public void AddItemsTable(string title, IEnumerable items, IList<ItemsTableField> fields)
        {
            this.Add(new ItemsTable { Caption = title, Items = items, Fields = fields });
        }

        /// <summary>
        /// Adds a paragraph to the report.
        /// </summary>
        /// <param name="content">The content.</param>
        public void AddParagraph(string content)
        {
            this.Add(new Paragraph { Text = content });
        }

        /// <summary>
        /// Adds a property table to the report.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="obj">The object.</param>
        /// <returns>A PropertyTable.</returns>
        public PropertyTable AddPropertyTable(string title, object obj)
        {
            var items = obj as IEnumerable;
            if (items == null)
            {
                items = new[] { obj };
            }

            var pt = new PropertyTable(items, false) { Caption = title };
            this.Add(pt);
            return pt;
        }

        /// <summary>
        /// Adds a table of contents.
        /// </summary>
        /// <param name="b">The source for the table of contents.</param>
        public void AddTableOfContents(ReportItem b)
        {
            this.Add(new TableOfContents(b));
        }

        /// <summary>
        /// Updates the item.
        /// </summary>
        public virtual void Update()
        {
        }

        /// <summary>
        /// Writes the item to a <see cref="IReportWriter" />.
        /// </summary>
        /// <param name="w">The target <see cref="IReportWriter" />.</param>
        public virtual void Write(IReportWriter w)
        {
            this.Update();
            this.WriteContent(w);
            foreach (var child in this.Children)
            {
                child.Write(w);
            }
        }

        /// <summary>
        /// Writes the content of the item to the specified <see cref="IReportWriter" />.
        /// </summary>
        /// <param name="w">The target <see cref="IReportWriter" />.</param>
        public virtual void WriteContent(IReportWriter w)
        {
        }

        /// <summary>
        /// Updates the figure numbers.
        /// </summary>
        protected void UpdateFigureNumbers()
        {
            var fc = new FigureCounter();
            this.UpdateFigureNumbers(fc);
        }

        /// <summary>
        /// Updates the Report property.
        /// </summary>
        /// <param name="report">The report.</param>
        protected void UpdateParent(Report report)
        {
            this.Report = report;
            foreach (var child in this.Children)
            {
                child.UpdateParent(report);
            }
        }

        /// <summary>
        /// Updates the figure numbers.
        /// </summary>
        /// <param name="fc">The figure counter.</param>
        private void UpdateFigureNumbers(FigureCounter fc)
        {
            var table = this as Table;
            if (table != null && table.Caption != null)
            {
                table.TableNumber = fc.TableNumber++;
            }

            var figure = this as Figure;
            if (figure != null && figure.FigureText != null)
            {
                figure.FigureNumber = fc.FigureNumber++;
            }

            foreach (var child in this.Children)
            {
                child.UpdateFigureNumbers(fc);
            }
        }

        /// <summary>
        /// Provides a figure and table counter.
        /// </summary>
        private class FigureCounter
        {
            /// <summary>
            /// Initializes a new instance of the <see cref = "FigureCounter" /> class.
            /// </summary>
            public FigureCounter()
            {
                this.FigureNumber = 1;
                this.TableNumber = 1;
            }

            /// <summary>
            /// Gets or sets the current figure number.
            /// </summary>
            public int FigureNumber { get; set; }

            /// <summary>
            /// Gets or sets the current table number.
            /// </summary>
            public int TableNumber { get; set; }
        }
    }
}