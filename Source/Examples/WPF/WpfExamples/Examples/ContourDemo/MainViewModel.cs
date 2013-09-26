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
// <summary>
//   The main view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ContourDemo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using OxyPlot;
    using OxyPlot.Series;

    /// <summary>
    /// The main view model.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The selected example.
        /// </summary>
        private Example selectedExample;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            this.Examples = new List<Example>();
            this.AddExamples();
            this.SelectedExample = this.Examples[0];
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the examples.
        /// </summary>
        /// <value>The examples.</value>
        public List<Example> Examples { get; set; }

        /// <summary>
        /// Gets or sets the plot model.
        /// </summary>
        /// <value>The plot model.</value>
        public PlotModel PlotModel { get; set; }

        /// <summary>
        /// Gets or sets the selected example.
        /// </summary>
        /// <value>The selected example.</value>
        public Example SelectedExample
        {
            get
            {
                return this.selectedExample;
            }

            set
            {
                this.selectedExample = value;
                this.RaisePropertyChanged("SelectedExample");
            }
        }

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        /// <param name="property">
        /// The property.
        /// </param>
        protected void RaisePropertyChanged(string property)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }

        /// <summary>
        /// Adds the examples.
        /// </summary>
        private void AddExamples()
        {
            Func<double, double, double> peaks =
                (x, y) =>
                3 * (1 - x) * (1 - x) * Math.Exp(-(x * x) - (y + 1) * (y + 1))
                - 10 * (x / 5 - x * x * x - y * y * y * y * y) * Math.Exp(-x * x - y * y)
                - 1.0 / 3 * Math.Exp(-(x + 1) * (x + 1) - y * y);

            this.Examples.Add(new Example("peaks", -3, 3, 0.05, -3, 3, 0.05, -6, 10, 1, peaks));

            this.Examples.Add(
                new Example(
                    "Conrec example 1",
                    -1.5,
                    1.5,
                    0.1,
                    -1.5,
                    1.5,
                    0.1,
                    0,
                    2.25,
                    0.25,
                    (x, y) =>
                    1.0
                    / ((x * x + (y - 0.842) * (y + 0.842)).Squared() + (x * (y - 0.842) + x * (y - 0.842)).Squared())));

            // todo: something wrong here?
            double c = 0;
            this.Examples.Add(
                new Example(
                    "Conrec example 2",
                    -2 * Math.PI,
                    2 * Math.PI,
                    0.25,
                    -2 * Math.PI,
                    2 * Math.PI,
                    0.25,
                    -1,
                    1,
                    0.02,
                    (x, y) =>
                    Math.Sin(Math.Sqrt(x * x + y * y)) + y != 0 ? 1.0 / Math.Sqrt((x - c) * (x - c) + y * y) : 0));

            this.Examples.Add(
                new Example(
                    "sin(x)*cos(y)",
                    -Math.PI,
                    Math.PI,
                    0.1,
                    -Math.PI,
                    Math.PI,
                    0.1,
                    -1,
                    1,
                    0.1,
                    (x, y) => Math.Sin(x) * Math.Cos(y)));

            this.Examples.Add(new Example("x*y", -1, 1, 0.1, -1, 1, 0.1, -1, 1, 0.1, (x, y) => x * y));
            this.Examples.Add(new Example("x^{2}+y^{2}", -1, 1, 0.1, -1, 1, 0.1, -1, 1, 0.1, (x, y) => x * x + y * y));
            this.Examples.Add(
                new Example(
                    "atan2(y,x)",
                    -1,
                    1,
                    0.1,
                    -1,
                    1,
                    0.1,
                    -180,
                    180,
                    10,
                    (x, y) => Math.Atan2(y, x) * 180 / Math.PI,
                    "0°"));

            this.Examples.Add(new Example("y/x", -1, 1, 0.1, -1, 1, 0.1, -1, 1, 0.1, (x, y) => y / x, "0%"));

            // http://en.wikipedia.org/wiki/Sinc_function
            this.Examples.Add(
                new Example(
                    "sinc(r+0.1)",
                    -8,
                    8,
                    0.2,
                    -8,
                    8,
                    0.2,
                    -0.2,
                    1,
                    0.1,
                    (x, y) =>
                        {
                            double r = Math.Sqrt(x * x + y * y);
                            return Math.Sin(r + 0.1) / (r + 0.1);
                        },
                    "0.0"));
        }

        /// <summary>
        /// Represents an example.
        /// </summary>
        public class Example
        {
            /// <summary>
            /// The d.
            /// </summary>
            private readonly double[,] D;

            /// <summary>
            /// The x.
            /// </summary>
            private readonly double[] X;

            /// <summary>
            /// The y.
            /// </summary>
            private readonly double[] Y;

            /// <summary>
            /// The z.
            /// </summary>
            private readonly double[] Z;

            /// <summary>
            /// Initializes a new instance of the <see cref="Example"/> class.
            /// </summary>
            /// <param name="title">
            /// The title.
            /// </param>
            /// <param name="minx">
            /// The minx.
            /// </param>
            /// <param name="maxx">
            /// The maxx.
            /// </param>
            /// <param name="dx">
            /// The dx.
            /// </param>
            /// <param name="miny">
            /// The miny.
            /// </param>
            /// <param name="maxy">
            /// The maxy.
            /// </param>
            /// <param name="dy">
            /// The dy.
            /// </param>
            /// <param name="minz">
            /// The minz.
            /// </param>
            /// <param name="maxz">
            /// The maxz.
            /// </param>
            /// <param name="dz">
            /// The dz.
            /// </param>
            /// <param name="f">
            /// The f.
            /// </param>
            /// <param name="formatString">
            /// The format string.
            /// </param>
            public Example(
                string title,
                double minx,
                double maxx,
                double dx,
                double miny,
                double maxy,
                double dy,
                double minz,
                double maxz,
                double dz,
                Func<double, double, double> f,
                string formatString = null)
            {
                this.Title = title;
                this.X = ArrayHelper.CreateVector(minx, maxx, dx);
                this.Y = ArrayHelper.CreateVector(miny, maxy, dy);
                this.Z = ArrayHelper.CreateVector(minz, maxz, dz);
                this.D = ArrayHelper.Evaluate(f, this.X, this.Y);
                this.FormatString = formatString;
            }

            /// <summary>
            /// Gets or sets the format string.
            /// </summary>
            /// <value>The format string.</value>
            public string FormatString { get; set; }

            /// <summary>
            /// Gets the plot model.
            /// </summary>
            /// <value>The plot model.</value>
            public PlotModel PlotModel
            {
                get
                {
                    var m = new PlotModel(this.Title);
                    var cs = new ContourSeries
                        {
                            ColumnCoordinates = this.X,
                            RowCoordinates = this.Y,
                            Data = this.D,
                            ContourLevels = this.Z,
                            LabelFormatString = this.FormatString
                        };
                    cs.CalculateContours();
                    m.Series.Add(cs);
                    return m;
                }
            }

            /// <summary>
            /// Gets or sets the title.
            /// </summary>
            /// <value>The title.</value>
            public string Title { get; set; }

            /// <summary>
            /// Returns a <see cref="System.String"/> that represents this instance.
            /// </summary>
            /// <returns>
            /// A <see cref="System.String"/> that represents this instance.
            /// </returns>
            public override string ToString()
            {
                return this.Title;
            }

        }
    }
}