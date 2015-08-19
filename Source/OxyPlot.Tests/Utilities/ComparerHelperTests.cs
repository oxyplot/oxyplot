// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComparerHelperTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class ComparerHelperTests
    {
        [Test]
        public void CreateComparerFromDelegate()
        {
            var source = new List<int> { 1, 2, 3, 0, 4, 5, 6, 7 };
            var comparer = ComparerHelper.CreateComparer<int>(
                (x, y) =>
                {
                    if (x == 0) return y == 0 ? 0 : -1;
                    if (y == 0) return 1;

                    return y.CompareTo(x);
                });

            var sorted = source.OrderBy(x => x, comparer).ToList();

            var expected = new List<int> { 0, 7, 6, 5, 4, 3, 2, 1 };
            CollectionAssert.AreEqual(expected, sorted);
        }
    }
}