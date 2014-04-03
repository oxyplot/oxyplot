// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="OxyPlot">
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
//   The performance test main program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PerformanceTest
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using OxyPlot;

    /// <summary>
    /// The performance test main program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The program entry point.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            var t0 = TestDrawClippedLine(10000, 1000, false);
            var t1 = TestDrawClippedLine(10000, 1000, true);
            Console.WriteLine("{0:P1}", (t0 - t1) / t0);
            Console.ReadKey();
        }

        /// <summary>
        /// Tests the <see cref="RenderingExtensions.DrawClippedLine" /> method.
        /// </summary>
        /// <param name="n">The number of points.</param>
        /// <param name="m">The number of repetitions.</param>
        /// <param name="useOutputBuffer"><c>true</c> to use an output buffer.</param>
        /// <returns>The elapsed time in milliseconds.</returns>
        public static double TestDrawClippedLine(int n, int m, bool useOutputBuffer)
        {
            var points = new ScreenPoint[n];
            for (int i = 0; i < n; i++)
            {
                points[i] = new ScreenPoint((double)i / n, Math.Sin(40d * i / n));
            }

            var clippingRectangle = new OxyRect(0.3, -0.5, 0.5, 1);
            var rc = new EmptyRenderContext();
            var outputBuffer = useOutputBuffer ? new List<ScreenPoint>(n) : null;
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < m; i++)
            {
                rc.DrawClippedLine(
                    points,
                    clippingRectangle,
                    1,
                    OxyColors.Black,
                    1,
                    null,
                    OxyPenLineJoin.Miter,
                    false,
                    outputBuffer);
            }

            stopwatch.Stop();
            Console.WriteLine((double)stopwatch.ElapsedMilliseconds);
            return stopwatch.ElapsedMilliseconds;
        }
    }
}
