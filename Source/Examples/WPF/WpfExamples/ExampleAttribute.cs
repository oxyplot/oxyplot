// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleAttribute.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WpfExamples
{
    using System;

    public class ExampleAttribute : Attribute
    {
        public ExampleAttribute(string description)
            : this(null, description)
        {
        }

        public ExampleAttribute(string title, string description)
        {
            this.Title = title;
            this.Description = description;
        }

        public string Title { get; private set; }

        public string Description { get; private set; }
    }
}