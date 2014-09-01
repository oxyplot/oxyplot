// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExamplesBase.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   The examples base class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// The examples base class.
    /// </summary>
    public abstract class ExamplesBase
    {
        /////// <summary>
        /////// Gets the title from the ExampleAttribute of the calling method.
        /////// </summary>
        /////// <param name="frameIndex">Index of the stack frame.</param>
        /////// <returns>
        /////// The title.
        /////// </returns>
        ////protected static string GetTitle(int frameIndex = 1)
        ////{
        //// var st = new StackTrace();
        //// var sf = st.GetFrame(frameIndex);
        //// var m = sf.GetMethod();
        //// var ea = m.GetCustomAttributes(typeof(ExampleAttribute), false).First() as ExampleAttribute;
        //// return ea.Title;
        ////}
    }
}