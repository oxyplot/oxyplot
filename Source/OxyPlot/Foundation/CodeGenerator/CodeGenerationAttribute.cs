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

        public CodeGenerationAttribute(bool generateCode)
        {
            this.GenerateCode = generateCode;
        }

        #endregion

        #region Public Properties

        public bool GenerateCode { get; set; }

        #endregion
    }
}