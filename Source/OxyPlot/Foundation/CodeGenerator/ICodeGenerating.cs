// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICodeGenerating.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides functionality to generate C# code of an object.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    /// <summary>
    /// Provides functionality to generate C# code of an object.
    /// </summary>
    public interface ICodeGenerating
    {
        /// <summary>
        /// Returns C# code that generates this instance.
        /// </summary>
        /// <returns>The C# code.</returns>
        string ToCode();
    }
}