// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Equation.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents an equation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Reporting
{
    /// <summary>
    /// Represents an equation.
    /// </summary>
    public class Equation : ReportItem
    {
        /// <summary>
        /// Gets or sets Caption.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets Content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// The write content.
        /// </summary>
        /// <param name="w">The w.</param>
        public override void WriteContent(IReportWriter w)
        {
            w.WriteEquation(this);
        }
    }
}