// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TableOfContents.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a table of contents.
    /// </summary>
    public class TableOfContents : ItemsTable
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TableOfContents"/> class.
        /// </summary>
        /// <param name="b">
        /// The b.
        /// </param>
        public TableOfContents(ReportItem b)
        {
            this.Base = b;
            this.Contents = new List<ContentItem>();
            this.Fields.Add(new ItemsTableField(null, "Chapter"));
            this.Fields.Add(new ItemsTableField(null, "Title"));
            this.Items = this.Contents;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets Base.
        /// </summary>
        public ReportItem Base { get; set; }

        /// <summary>
        ///   Gets or sets Contents.
        /// </summary>
        public List<ContentItem> Contents { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The update.
        /// </summary>
        public override void Update()
        {
            this.Contents.Clear();
            var hh = new HeaderHelper();
            this.Search(this.Base, hh);
            base.Update();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The search.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <param name="hh">
        /// The hh.
        /// </param>
        private void Search(ReportItem item, HeaderHelper hh)
        {
            var h = item as Header;
            if (h != null)
            {
                h.Chapter = hh.GetHeader(h.Level);
                this.Contents.Add(new ContentItem { Chapter = h.Chapter, Title = h.Text });
            }

            foreach (var c in item.Children)
            {
                this.Search(c, hh);
            }
        }

        #endregion

        /// <summary>
        /// The content item.
        /// </summary>
        public class ContentItem
        {
            #region Public Properties

            /// <summary>
            ///   Gets or sets Chapter.
            /// </summary>
            public string Chapter { get; set; }

            /// <summary>
            ///   Gets or sets Title.
            /// </summary>
            public string Title { get; set; }

            #endregion
        }
    }
}