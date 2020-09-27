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
        /// <param name="excludeFromAutomatedTests">A value indiciating whether the example should be excluded from automated tests.</param>
        public ExampleAttribute(string title = null, bool excludeFromAutomatedTests = false)
        {
            this.Title = title;
            this.ExcludeFromAutomatedTests = excludeFromAutomatedTests;
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; private set; }

        /// <summary>
        /// Gets a value indiciating whether this example should be excluded from automated tests.
        /// </summary>
        /// <value>
        /// <c>true</c> if the example should be excluded from automated tests, otherwise <c>false</c>.
        /// </value>
        public bool ExcludeFromAutomatedTests { get; }
    }
}
