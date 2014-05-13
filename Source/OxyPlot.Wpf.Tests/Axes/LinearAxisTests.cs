// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinearAxisTests.cs" company="OxyPlot">
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
//   Provides unit tests for the <see cref="LinearAxis" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf.Tests
{
    using NUnit.Framework;

    /// <summary>
    /// Provides unit tests for the <see cref="LinearAxis" /> class.
    /// </summary>
    public class LinearAxisTests
    {
        /// <summary>
        /// Asserts that default values in the <see cref="LinearAxis" /> equal the default values in <see cref="Axes.LinearAxis" />.
        /// </summary>
        public class DefaultValues
        {
            /// <summary>
            /// Compares the properties related to the axis title.
            /// </summary>
            [Test]
            public void CompareTitleProperties()
            {
                var axis = new Axes.LinearAxis();
                var wpfAxis = new LinearAxis();
                Assert.AreEqual(axis.TitleColor, wpfAxis.TitleColor.ToOxyColor(), "TitleColor");
                Assert.AreEqual(axis.Title, wpfAxis.Title, "Title");
                Assert.AreEqual(axis.TitleClippingLength, wpfAxis.TitleClippingLength, "TitleClippingLength");
                Assert.AreEqual(axis.TitleFont, wpfAxis.TitleFont, "TitleFont");
                Assert.AreEqual(axis.TitleFontSize, wpfAxis.TitleFontSize, "TitleFontSize");
                Assert.AreEqual(axis.TitleFontWeight, wpfAxis.TitleFontWeight.ToOpenTypeWeight(), "TitleFontWeight");
                Assert.AreEqual(axis.TitleFormatString, wpfAxis.TitleFormatString, "TitleFormatString");
                Assert.AreEqual(axis.TitlePosition, wpfAxis.TitlePosition, "TitlePosition");
            }
        }
    }
}