//-----------------------------------------------------------------------
// <copyright file="ReportItem.cs" company="OxyPlot">
//     http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
//-----------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// The report item.
    /// </summary>
    public abstract class ReportItem
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportItem"/> class.
        /// </summary>
        protected ReportItem()
        {
            this.Children = new Collection<ReportItem>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Children.
        /// </summary>
        public Collection<ReportItem> Children { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="child">
        /// The child.
        /// </param>
        public void Add(ReportItem child)
        {
            this.Children.Add(child);
        }

        /// <summary>
        /// The add drawing.
        /// </summary>
        /// <param name="content">
        /// The content.
        /// </param>
        /// <param name="text">
        /// The text.
        /// </param>
        public void AddDrawing(string content, string text)
        {
            this.Add(new DrawingFigure { Content = content, FigureText = text });
        }

        /// <summary>
        /// The add equation.
        /// </summary>
        /// <param name="equation">
        /// The equation.
        /// </param>
        /// <param name="caption">
        /// The caption.
        /// </param>
        public void AddEquation(string equation, string caption = null)
        {
            this.Add(new Equation { Content = equation, Caption = caption });
        }

        /// <summary>
        /// The add header.
        /// </summary>
        /// <param name="level">
        /// The level.
        /// </param>
        /// <param name="header">
        /// The header.
        /// </param>
        public void AddHeader(int level, string header)
        {
            this.Add(new Header { Level = level, Text = header });
        }

        /// <summary>
        /// The add image.
        /// </summary>
        /// <param name="src">
        /// The src.
        /// </param>
        /// <param name="text">
        /// The text.
        /// </param>
        public void AddImage(string src, string text)
        {
            this.Add(new Image { Source = src, FigureText = text });
        }

        /// <summary>
        /// The add items table.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="items">
        /// The items.
        /// </param>
        /// <param name="fields">
        /// The fields.
        /// </param>
        public void AddItemsTable(string title, IEnumerable items, IList<ItemsTableField> fields)
        {
            this.Add(new ItemsTable { Caption = title, Items = items, Fields = fields });
        }

        /// <summary>
        /// The add paragraph.
        /// </summary>
        /// <param name="content">
        /// The content.
        /// </param>
        public void AddParagraph(string content)
        {
            this.Add(new Paragraph { Text = content });
        }

        /// <summary>
        /// The add property table.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// </returns>
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
        /// The add table of contents.
        /// </summary>
        /// <param name="b">
        /// The b.
        /// </param>
        public void AddTableOfContents(ReportItem b)
        {
            this.Add(new TableOfContents(b));
        }

        /// <summary>
        /// The update.
        /// </summary>
        public virtual void Update()
        {
        }

        /// <summary>
        /// The write.
        /// </summary>
        /// <param name="w">
        /// The w.
        /// </param>
        public virtual void Write(IReportWriter w)
        {
            this.Update();
            this.WriteContent(w);
            foreach (ReportItem child in this.Children)
            {
                child.Write(w);
            }
        }

        /// <summary>
        /// The write content.
        /// </summary>
        /// <param name="w">
        /// The w.
        /// </param>
        public virtual void WriteContent(IReportWriter w)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// The update figure numbers.
        /// </summary>
        protected void UpdateFigureNumbers()
        {
            var fc = new FigureCounter();
            this.UpdateFigureNumbers(fc);
        }

        /// <summary>
        /// The update figure numbers.
        /// </summary>
        /// <param name="fc">
        /// The fc.
        /// </param>
        private void UpdateFigureNumbers(FigureCounter fc)
        {
            var table = this as Table;
            if (table != null)
            {
                table.TableNumber = fc.TableNumber++;
            }

            var figure = this as Figure;
            if (figure != null)
            {
                figure.FigureNumber = fc.FigureNumber++;
            }

            foreach (ReportItem child in this.Children)
            {
                child.UpdateFigureNumbers(fc);
            }
        }

        #endregion

        /// <summary>
        /// The figure counter.
        /// </summary>
        private class FigureCounter
        {
            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="FigureCounter"/> class.
            /// </summary>
            public FigureCounter()
            {
                this.FigureNumber = 1;
                this.TableNumber = 1;
            }

            #endregion

            #region Public Properties

            /// <summary>
            /// Gets or sets FigureNumber.
            /// </summary>
            public int FigureNumber { get; set; }

            /// <summary>
            /// Gets or sets TableNumber.
            /// </summary>
            public int TableNumber { get; set; }

            #endregion
        }
    }
}
