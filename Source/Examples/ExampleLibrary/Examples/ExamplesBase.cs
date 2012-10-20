// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExamplesBase.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   The examples base class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ExampleLibrary
{
    using System.Diagnostics;
    using System.Linq;

    using OxyPlot;

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