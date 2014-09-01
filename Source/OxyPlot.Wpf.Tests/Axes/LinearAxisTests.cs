// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinearAxisTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
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