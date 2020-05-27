// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotModelTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Tests the <see cref="PlotModel.Render" /> method.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using ExampleLibrary;

    using NSubstitute;

    using NUnit.Framework;

    using OxyPlot.Axes;
    using OxyPlot.Series;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class PlotModelTests
    {
        [Test]
        public void Update_AllExamples_ThrowsNoExceptions()
        {
            foreach (var example in Examples.GetList())
            {
                ((IPlotModel)example.PlotModel)?.Update(true);

                var first = 1; // skip the 'none', since we do that above for clarity
                var all = (int)(ExampleFlags.Transpose | ExampleFlags.Reverse);

                for (int flags = first; flags < all; flags++)
                {
                    ((IPlotModel)example.GetModel((ExampleFlags)flags))?.Update(true);
                }
            }
        }

        [Test]
        public void B11_Backgrounds()
        {
            var plot = new PlotModel { Title = "Backgrounds" };
            plot.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "X-axis" });
            var yaxis1 = new LinearAxis { Position = AxisPosition.Left, Title = "Y1", Key = "Y1", StartPosition = 0, EndPosition = 0.5 };
            var yaxis2 = new LinearAxis { Position = AxisPosition.Left, Title = "Y2", Key = "Y2", StartPosition = 0.5, EndPosition = 1 };
            plot.Axes.Add(yaxis1);
            plot.Axes.Add(yaxis2);

            Action<LineSeries> addExamplePoints = ls =>
                {
                    ls.Points.Add(new DataPoint(3, 13));
                    ls.Points.Add(new DataPoint(10, 47));
                    ls.Points.Add(new DataPoint(30, 23));
                    ls.Points.Add(new DataPoint(40, 65));
                    ls.Points.Add(new DataPoint(80, 10));
                };

            var ls1 = new LineSeries { Background = OxyColors.LightSeaGreen, YAxisKey = "Y1" };
            addExamplePoints(ls1);
            plot.Series.Add(ls1);

            var ls2 = new LineSeries { Background = OxyColors.LightSkyBlue, YAxisKey = "Y2" };
            addExamplePoints(ls2);
            plot.Series.Add(ls2);

            // OxyAssert.AreEqual(plot, "B11");
        }

        [Test]
        public void PlotControl_CollectedPlotControl_ReferenceShouldNotBeAlive()
        {
            var plot = Substitute.For<IPlotView>();
            var pm = new PlotModel();
            ((IPlotModel)pm).AttachPlotView(plot);
            Assert.IsNotNull(pm.PlotView);

            // ReSharper disable once RedundantAssignment
            plot = null;
            GC.Collect();

            // Verify that the reference is lost
            Assert.IsNull(pm.PlotView);
        }

        /// <summary>
        /// Tests rendering on a collapsed output surface.
        /// </summary>
        [Test]
        public void Collapsed()
        {
            var model = new PlotModel();
            var rc = Substitute.For<IRenderContext>();
            ((IPlotModel)model).Render(rc, new OxyRect(0, 0, 0, 0));
        }

        /// <summary>
        /// Tests rendering on a small output surface.
        /// </summary>
        [Test]
        public void NoPadding()
        {
            var model = new PlotModel { Padding = new OxyThickness(0) };
            var rc = Substitute.For<IRenderContext>();
            ((IPlotModel)model).Render(rc, new OxyRect(0, 0, double.Epsilon, double.Epsilon));
        }

        /// <summary>
        /// The same axis cannot be added more than once.
        /// </summary>
        [Test]
        public void AddAxisTwice()
        {
            var model = new PlotModel();
            var axis = new LinearAxis();
            model.Axes.Add(axis);
            Assert.Throws<InvalidOperationException>(() => model.Axes.Add(axis));
        }

        /// <summary>
        /// The same axis cannot be added to different PlotModels.
        /// </summary>
        [Test]
        public void AddAxisToDifferentModels()
        {
            var model1 = new PlotModel();
            var model2 = new PlotModel();
            var axis = new LinearAxis();
            model1.Axes.Add(axis);
            Assert.Throws<InvalidOperationException>(() => model2.Axes.Add(axis));
        }

        /// <summary>
        /// An exception should be thrown when the axis key is invalid.
        /// </summary>
        [Test]
        public void InvalidAxisKey()
        {
            var model = new PlotModel();
            model.Axes.Add(new LinearAxis());
            model.Series.Add(new LineSeries { XAxisKey = "invalidKey" });
            ((IPlotModel)model).Update(true);
            Assert.IsNotNull(model.GetLastPlotException() as InvalidOperationException);
        }

        /// <summary>
        /// When PlotMargins is not set, the ActualPlotMargins should use the desired size of the axes.
        /// </summary>
        [Test]
        public void AutoPlotMargins()
        {
            var plot = new PlotModel { Title = "Auto PlotMargins" };
            var verticalAxis = new LinearAxis { Position = AxisPosition.Left };
            var horizontalAxis = new LinearAxis { Position = AxisPosition.Bottom };
            plot.Axes.Add(verticalAxis);
            plot.Axes.Add(horizontalAxis);
            plot.UpdateAndRenderToNull(800, 600);
            Assert.That(plot.ActualPlotMargins.Left, Is.EqualTo(26).Within(1), "left");
            Assert.That(plot.ActualPlotMargins.Top, Is.EqualTo(5).Within(1), "top");
            Assert.That(plot.ActualPlotMargins.Right, Is.EqualTo(7.5).Within(1), "right");
            Assert.That(plot.ActualPlotMargins.Bottom, Is.EqualTo(21).Within(1), "bottom");
        }

        /// <summary>
        /// When PlotMargins is not set, the ActualPlotMargins should have the same value.
        /// </summary>
        [Test]
        public void FixedPlotMargins()
        {
            var plot = new PlotModel { PlotMargins = new OxyThickness(23, 19, 17, 31) };
            plot.UpdateAndRenderToNull(800, 600);
            Assert.That(plot.ActualPlotMargins.Left, Is.EqualTo(plot.PlotMargins.Left), "left");
            Assert.That(plot.ActualPlotMargins.Top, Is.EqualTo(plot.PlotMargins.Top), "top");
            Assert.That(plot.ActualPlotMargins.Right, Is.EqualTo(plot.PlotMargins.Right), "right");
            Assert.That(plot.ActualPlotMargins.Bottom, Is.EqualTo(plot.PlotMargins.Bottom), "bottom");
        }

        /// <summary>
        /// When GetAxis is called with an unknown key, an InvalidOperationException should be thrown.
        /// </summary>
        [Test]
        public void GetAxis()
        {
            var plot = new PlotModel { Title = "Get Axis Or Default" };
            var verticalAxis = new LinearAxis { Key = "YAxis", Position = AxisPosition.Left };
            var horizontalAxis = new LinearAxis { Key = "XAxis", Position = AxisPosition.Bottom };

            plot.Axes.Add(verticalAxis);
            plot.Axes.Add(horizontalAxis);

            Assert.That(() => plot.GetAxis("ThisIsAnInvalidKey"), Throws.InvalidOperationException);
        }

        /// <summary>
        /// When GetAxisOrDefault is called with an unknown key, the provided default value should
        /// be returned.
        /// </summary>
        [Test]
        public void GetAxisOrDefault()
        {
            var plot = new PlotModel { Title = "Get Axis Or Default" };
            var verticalAxis = new LinearAxis { Key = "YAxis", Position = AxisPosition.Left };
            var horizontalAxis = new LinearAxis { Key = "XAxis", Position = AxisPosition.Bottom };

            plot.Axes.Add(verticalAxis);
            plot.Axes.Add(horizontalAxis);

            var defaultValue = new LinearAxis { Key = "DefaultAxis" };

            Assert.That(plot.GetAxisOrDefault("ThisIsAnInvalidKey", defaultValue), Is.EqualTo(defaultValue));
        }
    }
}
