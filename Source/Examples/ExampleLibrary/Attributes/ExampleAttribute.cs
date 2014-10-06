// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleAttribute.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    /// <summary>
    /// Specifies the title for an example.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ExampleAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleAttribute"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        public ExampleAttribute(string title = null)
        {
            this.Title = title;
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; private set; }
    }
}