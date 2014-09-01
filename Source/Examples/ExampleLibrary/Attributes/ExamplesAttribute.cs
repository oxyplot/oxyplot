// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExamplesAttribute.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace ExampleLibrary
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ExamplesAttribute : Attribute
    {
        public string Category { get; set; }
        public ExamplesAttribute(string category = null)
        {
            this.Category = category;
        }
    }
}