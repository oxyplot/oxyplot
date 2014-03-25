// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotControllerTests.cs" company="OxyPlot">
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