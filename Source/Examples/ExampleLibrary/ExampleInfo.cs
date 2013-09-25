// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleInfo.cs" company="OxyPlot">
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
// --------------------------------------------------------------------------------------------------------------------
using System.Reflection;
using OxyPlot;

namespace ExampleLibrary
{
    public class ExampleInfo
    {
        public string Category { get; set; }
        public string Title { get; set; }
        private MethodInfo Method { get; set; }

        private PlotModel plotModel;

        public PlotModel PlotModel
        {
            get
            {
                if (plotModel == null)
                    plotModel = Method.Invoke(null, null) as PlotModel;
                return plotModel;
            }
        }

        public string Code
        {
            get
            {
                return PlotModel.ToCode();
            }
        }

        public ExampleInfo(string category, string title, MethodInfo method)
        {
            this.Category = category;
            this.Title = title;
            this.Method = method;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}