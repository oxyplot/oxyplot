// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotControllerTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Tests the <see cref="PlotController" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System.Linq;

    using NUnit.Framework;

    /// <summary>
    /// Tests the <see cref="PlotController" /> class.
    /// </summary>
    [TestFixture]
    public class PlotControllerTests
    {
        /// <summary>
        /// Tests the <see cref="PlotController.Unbind(OxyInputGesture)" /> method.
        /// </summary>
        public class Unbind
        {
            /// <summary>
            /// When unbinding a gesture, the gesture should be removed from the InputCommandBindings.
            /// </summary>
            [Test]
            public void UnbindLeftMouseButton()
            {
                var c = new PlotController();
                c.Unbind(new OxyMouseDownGesture(OxyMouseButton.Left));
                Assert.IsFalse(c.InputCommandBindings.Any(b => b.Gesture.Equals(new OxyMouseDownGesture(OxyMouseButton.Left))));
            }

            /// <summary>
            /// When unbinding a command, the command should be removed from the InputCommandBindings.
            /// </summary>
            [Test]
            public void UnbindPlotCommand()
            {
                var c = new PlotController();
                c.Unbind(PlotCommands.SnapTrack);
                Assert.IsFalse(c.InputCommandBindings.Any(b => b.Command == PlotCommands.SnapTrack));
            }

            /// <summary>
            /// When unbinding all gestures, the InputCommandBindings collection should be empty.
            /// </summary>
            [Test]
            public void UnbindAll()
            {
                var c = new PlotController();
                c.UnbindAll();
                Assert.AreEqual(0, c.InputCommandBindings.Count);
            }
        }
    }
}