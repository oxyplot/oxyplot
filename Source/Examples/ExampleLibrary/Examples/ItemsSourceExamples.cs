// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemsSourceExamples.cs" company="OxyPlot">
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
using OxyPlot;

namespace ExampleLibrary
{
    [Examples("ItemsSource")]
    public class ItemsSourceExamples : ExamplesBase
    {
        [Example("Using IDataPointProvider")]
        public static PlotModel UsingIDataPointProvider()
        {
            var measurements = new[] { new MeasurementType1(0, 10), new MeasurementType1(10, 8), new MeasurementType1(15, 12) };
            var model = new PlotModel("Using IDataPointProvider");
            model.Series.Add(new LineSeries { ItemsSource = measurements });
            return model;
        }

        [Example("Using IDataPoint")]
        public static PlotModel UsingIDataPoint()
        {
            var measurements = new[] { new MeasurementType2(0, 8), new MeasurementType2(10, 10), new MeasurementType2(15, 12) };
            var model = new PlotModel("Using IDataPoint");
            model.Series.Add(new LineSeries { ItemsSource = measurements });
            return model;
        }

        [Example("Using reflection")]
        public static PlotModel UsingReflection()
        {
            var measurements = new[] { new MeasurementType3(0, 12), new MeasurementType3(10, 8), new MeasurementType3(15, 10) };
            var model = new PlotModel("Using reflection");
            model.Series.Add(new LineSeries { ItemsSource = measurements, DataFieldX = "Abscissa", DataFieldY = "Ordinate" });
            return model;
        }

        [Example("Using Mapping property")]
        public static PlotModel UsingMappingProperty()
        {
            var measurements = new[] { new MeasurementType3(0, 12), new MeasurementType3(10, 8), new MeasurementType3(15, 10) };
            var model = new PlotModel("Using Mapping property");
            model.Series.Add(
                new LineSeries
                    {
                        ItemsSource = measurements,
                        Mapping =
                            item => new DataPoint(((MeasurementType3)item).Abscissa, ((MeasurementType3)item).Ordinate)
                    });
            return model;
        }

        private class MeasurementType1 : IDataPointProvider
        {
            public double Abscissa { get; set; }
            public double Ordinate { get; set; }

            public MeasurementType1(double abscissa, double ordinate)
            {
                this.Abscissa = abscissa;
                this.Ordinate = ordinate;
            }

            public DataPoint GetDataPoint()
            {
                return new DataPoint(Abscissa, Ordinate);
            }
        }

        private class MeasurementType2 : IDataPoint
        {
            public double Abscissa { get; set; }
            public double Ordinate { get; set; }

            public MeasurementType2(double abscissa, double ordinate)
            {
                this.Abscissa = abscissa;
                this.Ordinate = ordinate;
            }

            public DataPoint GetDataPoint()
            {
                return new DataPoint(Abscissa, Ordinate);
            }

            public string ToCode()
            {
                return CodeGenerator.FormatConstructor(this.GetType(), "{0},{1}", this.Abscissa, this.Ordinate);
            }

            public double X
            {
                get
                {
                    return Abscissa;
                }
                set
                {
                    Abscissa = value;
                }
            }

            public double Y
            {
                get
                {
                    return Ordinate;
                }
                set
                {
                    Ordinate = value;
                }
            }
        }

        private class MeasurementType3
        {
            public double Abscissa { get; set; }

            public double Ordinate { get; set; }

            public MeasurementType3(double abscissa, double ordinate)
            {
                this.Abscissa = abscissa;
                this.Ordinate = ordinate;
            }
        }
    }
}