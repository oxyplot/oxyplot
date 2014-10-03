// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExamplesAttribute.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    /// <summary>
    /// Specifies the category for a class containing examples.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ExamplesAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExamplesAttribute"/> class.
        /// </summary>
        /// <param name="category">The category.</param>
        public ExamplesAttribute(string category = null)
        {
            this.Category = category;
        }

        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public string Category { get; private set; }
    }
}