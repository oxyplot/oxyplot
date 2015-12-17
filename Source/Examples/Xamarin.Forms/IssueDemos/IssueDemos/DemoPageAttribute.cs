namespace IssueDemos
{
    using System;

    public sealed class DemoPageAttribute : Attribute
    {
        public DemoPageAttribute(string title, string details)
        {
            this.Title = title;
            this.Details = details;
        }
        public string Title { get; private set; }
        public string Details { get; private set; }
    }
}