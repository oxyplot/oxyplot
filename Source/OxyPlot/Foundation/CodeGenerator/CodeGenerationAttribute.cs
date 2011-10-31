// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeGenerationAttribute.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;

    /// <summary>
    /// Attribute that controls if code should be generated for the property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CodeGenerationAttribute : Attribute
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeGenerationAttribute"/> class.
        /// </summary>
        /// <param name="generateCode">
        /// The generate code.
        /// </param>
        public CodeGenerationAttribute(bool generateCode)
        {
            this.GenerateCode = generateCode;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets a value indicating whether GenerateCode.
        /// </summary>
        public bool GenerateCode { get; set; }

        #endregion
    }
}