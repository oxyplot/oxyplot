// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemsSourceExamples.cs" company="OxyPlot">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 OxyPlot contributors
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

namespace ExampleLibrary
{
    using System;
    using System.Collections.Generic;

    using OxyPlot;
    using OxyPlot.Series;

    [Examples("ItemsSource")]
    public class ItemsSourceExamples : ExamplesBase
    {
        private static int n = 100000;
        [Example("List<DataPoint>")]
        public static PlotModel UsingIDataPoint()
        {
            var points = new List<DataPoint>(n);
            for (int i = 0; i < n; i++)
            {
                var x = (double)i / (n - 1);
                points.Add(new DataPoint(x, y(x)));
            }

            var model = new PlotModel { Title = "Using IDataPoint" };
            model.Series.Add(new LineSeries { ItemsSource = points });
            return model;
        }

        [Example("Items implementing IDataPointProvider")]
        public static PlotModel UsingIDataPointProvider()
        {
            var points = new List<PointType1>(n);
            for (int i = 0; i < n; i++)
            {
                var x = (double)i / (n - 1);
                points.Add(new PointType1(x, y(x)));
            }

            var model = new PlotModel { Title = "Items implementing IDataPointProvider" };
            model.Series.Add(new LineSeries { ItemsSource = points });
            return model;
        }

        [Example("Mapping property")]
        public static PlotModel UsingMappingProperty()
        {
            var points = new List<PointType2>(n);
            for (int i = 0; i < n; i++)
            {
                var x = (double)i / (n - 1);
                points.Add(new PointType2(x, y(x)));
            }

            var model = new PlotModel { Title = "Using Mapping property" };
            model.Series.Add(
                new LineSeries
                {
                    ItemsSource = points,
                    Mapping = item => new DataPoint(((PointType2)item).Abscissa, ((PointType2)item).Ordinate)
                });
            return model;
        }

        [Example("Using reflection (slow)")]
        public static PlotModel UsingReflection()
        {
            var points = new List<PointType2>(n);
            for (int i = 0; i < n; i++)
            {
                var x = (double)i / (n - 1);
                points.Add(new PointType2(x, y(x)));
            }

            var model = new PlotModel { Title = "Using reflection (slow)" };
            model.Series.Add(new LineSeries { ItemsSource = points, DataFieldX = "Abscissa", DataFieldY = "Ordinate" });
            return model;
        }

        private class PointType1 : IDataPointProvider, ICodeGenerating
        {
            public PointType1(double abscissa, double ordinate)
            {
                this.Abscissa = abscissa;
                this.Ordinate = ordinate;
            }

            public double Abscissa { get; private set; }

            public double Ordinate { get; private set; }

            public DataPoint GetDataPoint()
            {
                return new DataPoint(Abscissa, Ordinate);
            }

            public string ToCode()
            {
                return CodeGenerator.FormatConstructor(this.GetType(), "{0},{1}", this.Abscissa, this.Ordinate);
            }
        }

        private class PointType2
        {
            public PointType2(double abscissa, double ordinate)
            {
                this.Abscissa = abscissa;
                this.Ordinate = ordinate;
            }

            public double Abscissa { get; private set; }

            public double Ordinate { get; private set; }
        }

        /// <summary>
        /// Evaluates a chaotic function.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <returns>A y value.</returns>
        private static double y(double x)
        {
            // http://computing.dcu.ie/~humphrys/Notes/Neural/chaos.html
            return Math.Sin(3 / x) * Math.Sin(5 / (1 - x));
        }
    }
}