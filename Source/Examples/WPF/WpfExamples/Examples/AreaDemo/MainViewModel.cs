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

namespace AreaDemo
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            Measurements = new Collection<Measurement>();
            var r = new Random();
            double p1 = 0;
            double p2 = 5;
            double v1 = 0;
            double v2 = 0;
            for (int i = 0; i < 100; i++)
            {
                v1 += (r.NextDouble() - 0.5)*0.7;
                v2 += (r.NextDouble() - 0.5)*0.1;
                double y1 = p1 + v1;
                double y2 = p2 + v2;
                p1 = y1;
                p2 = y2;
                Measurements.Add(new Measurement
                                     {
                                         Time = i*2.5,
                                         Value = y1,
                                         Maximum = y1 + y2,
                                         Minimum = y1 - y2
                                     });
            }
        }

        public Collection<Measurement> Measurements { get; private set; }
    }

    public class Measurement
    {
        public double Time { get; set; }
        public double Value { get; set; }
        public double Minimum { get; set; }
        public double Maximum { get; set; }

        public override string ToString()
        {
            return String.Format("{0:#0.0} {1:##0.0}", Time, Value);
        }
    }
}