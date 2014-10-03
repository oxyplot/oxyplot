// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagsAttribute.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    /// <summary>
    /// Specifies tags.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TagsAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagsAttribute" /> class.
        /// </summary>
        /// <param name="tags">The tags.</param>
        public TagsAttribute(params string[] tags)
        {
            this.Tags = tags;
        }

        /// <summary>
        /// Gets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public string[] Tags { get; private set; }
    }
}