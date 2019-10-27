// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TableOfContents.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a table of contents.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a table of contents.
    /// </summary>
    public class TableOfContents : ItemsTable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TableOfContents" /> class.
        /// </summary>
        /// <param name="b">The source.</param>
        public TableOfContents(ReportItem b)
        {
            this.Base = b;
            this.Contents = new List<ContentItem>();
            this.Fields.Add(new ItemsTableField(null, "Chapter"));
            this.Fields.Add(new ItemsTableField(null, "Title"));
            this.Items = this.Contents;
        }

        /// <summary>
        /// Gets the source item.
        /// </summary>
        public ReportItem Base { get; private set; }

        /// <summary>
        /// Gets the contents.
        /// </summary>
        public List<ContentItem> Contents { get; private set; }

        /// <summary>
        /// Updates the table of contents.
        /// </summary>
        public override void Update()
        {
            this.Contents.Clear();
            var hh = new HeaderHelper();
            this.AppendHeaders(this.Base, hh);
            base.Update();
        }

        /// <summary>
        /// Appends headers (recursively) to the <see cref="Contents" /> of the object.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="hh">The header formatter.</param>
        private void AppendHeaders(ReportItem item, HeaderHelper hh)
        {
            var h = item as Header;
            if (h != null)
            {
                h.Chapter = hh.GetHeader(h.Level);
                this.Contents.Add(new ContentItem { Chapter = h.Chapter, Title = h.Text });
            }

            foreach (var c in item.Children)
            {
                this.AppendHeaders(c, hh);
            }
        }

        /// <summary>
        /// Represents an item in the table of contents.
        /// </summary>
        public class ContentItem
        {
            /// <summary>
            /// Gets or sets the chapter.
            /// </summary>
            public string Chapter { get; set; }

            /// <summary>
            /// Gets or sets the title.
            /// </summary>
            public string Title { get; set; }
        }
    }
}