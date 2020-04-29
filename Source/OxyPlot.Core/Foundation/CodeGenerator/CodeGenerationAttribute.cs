// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeGenerationAttribute.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Specifies whether code should be generated for the property.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// Specifies whether code should be generated for the property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CodeGenerationAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CodeGenerationAttribute" /> class.
        /// </summary>
        /// <param name="generateCode">The generate code.</param>
        public CodeGenerationAttribute(bool generateCode)
        {
            this.GenerateCode = generateCode;
        }

        /// <summary>
        /// Gets or sets a value indicating whether GenerateCode.
        /// </summary>
        public bool GenerateCode { get; set; }
    }
}