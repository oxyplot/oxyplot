// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleAttribute.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace ExampleLibrary
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ExampleAttribute : Attribute
    {
        public string Title { get; set; }
        public ExampleAttribute(string title = null)
        {
            this.Title = title;
        }
    }
}