// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HelpersTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using NUnit.Framework;
    using OxyPlot.Utilities;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class HelpersTests
    {
        private readonly struct LinearInterpolationTestCase
        {

            public LinearInterpolationTestCase(double x0, double y0, double x1, double y1, double x, double y) : this()
            {
                this.X0 = x0;
                this.X1 = x1;
                this.Y0 = y0;
                this.Y1 = y1;
                this.X = x;
                this.Y = y;
            }

            public double X0 { get; }
            public double X1 { get; }
            public double Y0 { get; }
            public double Y1 { get; }
            public double X { get; }
            public double Y { get; }
        }

        [Test]
        public void LinearInterpolation()
        {
            var testCases = new LinearInterpolationTestCase[]
            {
                //   x0   y0   x1   y1    x      y
                new(  0,   0,   1,   1,   0.5,   0.5),
                new(  0,   0,   1,   1,   0  ,   0  ),
                new(  0,   0,   1,   1,   1  ,   1  ),
                new(  0,   0,   1,   1,   0.1,   0.1),
                new(  0,   0,   1,   1,   0.9,   0.9),
                new(  0, -10,   1,  10,   0  , -10  ),
                new(  0, -10,   1,  10,   0.5,   0  ),
                new(  0, -10,   1,  10,   1  ,  10  ),
                new(-10,   0,  10,   1, -10  ,   0  ),
                new(-10,   0,  10,   1,   0  ,   0.5),
                new(-10,   0,  10,   1,  10  ,   1  ),
            };

            foreach (var tc in testCases)
            {
                var result = Helpers.LinearInterpolation(tc.X0, tc.Y0, tc.X1, tc.Y1, tc.X);
                Assert.AreEqual(tc.Y, result);
            }
        }
    }
}
