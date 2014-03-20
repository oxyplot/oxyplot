// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="OxyPlot">
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
using System;
using System.Collections.ObjectModel;

namespace BindingDemo
{
    public class MainViewModel
    {
        public Collection<Measurement> Measurements { get; private set; }

        public string Subtitle { get; set; }

        public MainViewModel()
        {
            Measurements = new Collection<Measurement>();
            int N = 50000;
            Subtitle = "N = " + N;

            var r = new Random(385);
            double dy = 0;
            double y = 0;
            for (int i = 0; i < N; i++)
            {
                dy += r.NextDouble() * 2 - 1;
                y += dy;
                Measurements.Add(new Measurement
                                     {
                                         Time = 2.5 * i / (N - 1),
                                         Value = y / (N - 1),
                                         Maximum = (y ) / (N - 1)+5,
                                         Minimum = (y ) / (N - 1)-5
                                     });
            }
        }
    }
}