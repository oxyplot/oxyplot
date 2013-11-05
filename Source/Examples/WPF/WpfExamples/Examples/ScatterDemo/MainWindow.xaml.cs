// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
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
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System.Windows;
using OxyPlot;

namespace ScatterDemo
{
    using OxyPlot.Series;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PlotModel ScatterModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            var tmp = new PlotModel("Scatter plot","Barnsley fern (IFS)");
            var s1 = new LineSeries
                         {
                             StrokeThickness = 0,
                             MarkerSize = 3,
                             // MarkerFill = OxyColors.Blue,
                             MarkerStroke=OxyColors.ForestGreen,
                             MarkerType = MarkerType.Plus
                         };

            foreach (var pt in Fern.Generate(2000))
                s1.Points.Add(new DataPoint(pt.X, -pt.Y));

            //var r = new Random();
            //for (int i = 0; i < 1000; i++)
            //    s1.Points.Add(new DataPoint(r.NextDouble(), r.NextDouble()));

            tmp.Series.Add(s1);
            ScatterModel = tmp;
        }
    }
}