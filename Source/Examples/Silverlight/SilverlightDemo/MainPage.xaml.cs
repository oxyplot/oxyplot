// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainPage.xaml.cs" company="OxyPlot">
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

namespace SilverlightDemo
{
    using System;
    using System.Windows.Controls;

    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;

    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            this.InitializeComponent();

            var tmp = new PlotModel("Silverlight demo");
            tmp.Axes.Add(new LinearAxis(AxisPosition.Left) { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot });
            tmp.Axes.Add(new LinearAxis(AxisPosition.Bottom) { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot });
            tmp.Series.Add(new FunctionSeries(x => Math.Sin(x) / x, -10, 10, 0.03, "sin(x)/x"));
            plot1.Model = tmp;
        }
    }
}