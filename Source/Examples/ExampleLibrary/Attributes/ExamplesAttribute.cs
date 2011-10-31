// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExamplesAttribute.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
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