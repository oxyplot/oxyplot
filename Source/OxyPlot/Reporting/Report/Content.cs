using System.Collections.Generic;

namespace OxyPlot.Reporting
{
    public class Content : Table
    {
        public List<ContentItem> Contents { get; set; }
        public ReportItem Base { get; set; }

        public Content(ReportItem b)
        {
            this.Base = b;
            Class = "content";
            Contents = new List<ContentItem>();
            Columns.Add(new TableColumn(null, "Chapter"));
            Columns.Add(new TableColumn(null, "Title"));
            Items = Contents;
        }

        public class ContentItem
        {
            public string Chapter { get; set; }
            public string Title { get; set; }
        }

        public override void Update()
        {
            Contents.Clear();
            var hh = new HeaderHelper();
            Search(Base, hh);
            base.Update();
        }

        private void Search(ReportItem item, HeaderHelper hh)
        {
            var h = item as Header;
            if (h != null)
            {
                h.Chapter = hh.GetHeader(h.Level);
                Contents.Add(new ContentItem() { Chapter = h.Chapter, Title = h.Text });
            }
            foreach (var c in item.Children)
                Search(c,hh);
        }
        public override void WriteContent(IReportWriter w)
        {
        }
    }
}