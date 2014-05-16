// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotViewTests.cs" company="OxyPlot">
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
// <summary>
//   Provides unit tests for the <see cref="PlotView" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf.Tests
{
    using System.Threading.Tasks;
    using System.Windows;

    using NUnit.Framework;

    /// <summary>
    /// Provides unit tests for the <see cref="PlotView" /> class.
    /// </summary>
    [TestFixture]
    public class PlotViewTests
    {
        /// <summary>
        /// Provides unit tests for the <see cref="PlotView.ActualModel" /> property.
        /// </summary>
        public class ActualModel
        {
            /// <summary>
            /// Gets the actual model when model is not set.
            /// </summary>
            [Test, Ignore]
            public void GetDefault()
            {
                var w = new Window();
                var plotView = new PlotView();
                w.Content = plotView;
                w.Show();
                Assert.IsNotNull(plotView.ActualModel);
            }

            /// <summary>
            /// Gets the actual model from the same thread that created the <see cref="PlotView" />.
            /// </summary>
            [Test]
            public void GetFromSameThread()
            {
                var model = new PlotModel();
                var plotView = new PlotView { Model = model };
                Assert.AreEqual(model, plotView.ActualModel);
            }

            /// <summary>
            /// Gets the actual model from a thread different from the one that created the <see cref="PlotView" />.
            /// </summary>
            [Test]
            public void GetFromOtherThread()
            {
                var model = new PlotModel();
                var plotView = new PlotView { Model = model };
                PlotModel actualModel = null;
                Task.Factory.StartNew(() => actualModel = plotView.ActualModel).Wait();
                Assert.AreEqual(model, actualModel);
            }
        }

        /// <summary>
        /// Provides unit tests for the <see cref="PlotView.InvalidatePlot" /> method.
        /// </summary>
        public class InvalidatePlot
        {
            /// <summary>
            /// Invalidates the plotView from the same thread that created the <see cref="PlotView" />.
            /// </summary>
            [Test]
            public void InvalidateFromSameThread()
            {
                var model = new PlotModel();
                var plotView = new PlotView { Model = model };
                plotView.InvalidatePlot();
            }

            /// <summary>
            /// Invalidates the plotView from a thread different from the one that created the <see cref="PlotView" />.
            /// </summary>
            [Test]
            public void InvalidateFromOtherThread()
            {
                var model = new PlotModel();
                var plotView = new PlotView { Model = model };
                Task.Factory.StartNew(() => plotView.InvalidatePlot()).Wait();
            }
        }

        /// <summary>
        /// Provides unit tests for the default values of the <see cref="PlotView" /> class.
        /// </summary>
        public class DefaultValues
        {
            /// <summary>
            /// Asserts that the default values are equal to the default values in the <see cref="PlotModel" />.
            /// </summary>
            [Test]
            public void PlotModelVsPlot()
            {
                var model = new PlotModel();
                var view = new PlotView();
                OxyAssert.PropertiesAreEqual(model, view);
            }
        }
    }
}