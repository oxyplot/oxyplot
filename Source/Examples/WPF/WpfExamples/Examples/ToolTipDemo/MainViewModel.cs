// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using OxyPlot;
using OxyPlot.Series;

namespace ToolTipDemo
{
    using System;
    using System.Collections.ObjectModel;

    public class MainViewModel
    {
        Random r = new Random(13);

        public MainViewModel()
        {
            this.Model = GenerateRandomPlotModel("Random plot model");
            this.Model.TitleToolTip = "Tool tip set via the model";

            this.Measurements = new Collection<Measurement>();
            var r = new Random(31);
            double p1 = 0;
            double p2 = 5;
            double v1 = 0;
            double v2 = 0;
            for (int i = 0; i < 100; i++)
            {
                v1 += (r.NextDouble() - 0.5) * 0.7;
                v2 += (r.NextDouble() - 0.5) * 0.1;
                double y1 = p1 + v1;
                double y2 = p2 + v2;
                p1 = y1;
                p2 = y2;
                this.Measurements.Add(new Measurement
                                     {
                                         Time = i * 2.5,
                                         Value = y1,
                                         Maximum = y1 + y2,
                                         Minimum = y1 - y2
                                     });
            }
        }

        public Collection<Measurement> Measurements { get; private set; }

        public PlotModel Model { get; private set; }

        private PlotModel GenerateRandomPlotModel(string title, int numberOfPoints = 50)
        {
            var plotModel = new PlotModel
            {
                Title = title,
                TitleToolTip = title
            };
            var lineSeries = new LineSeries();

            for (int i = 0; i < numberOfPoints; i++)
            {
                lineSeries.Points.Add(new DataPoint(i, this.r.NextDouble()));
            }

            plotModel.Series.Add(lineSeries);

            return plotModel;
        }
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