using System.Collections.Generic;
using System.Linq;

namespace OxyPlot.Tests
{
    using System.Collections;

    using NUnit.Framework;

    using OxyPlot.Series;

    [TestFixture]
    public class DataPointSeriesTest
    {
        [Test]
        public void CachedItemsSourcePointsAreUsedByGetItem()
        {
            var itemsSource = new MockItemsSource();
            var series = new TestDataPointSeries { ItemsSource = itemsSource };
            series.TestUpdateData();
            var itemA = (DataPoint) series.TestGetItem(2);
            var itemB = (DataPoint) series.TestGetItem(2);

            Assert.That(itemA.X, Is.EqualTo(2));
            Assert.That(itemB.X, Is.EqualTo(2));
            Assert.That(itemsSource.IterationCount, Is.EqualTo(1));
        }

        class MockItemsSource : IEnumerable<DataPoint>
        {
            public int IterationCount;

            public IEnumerator<DataPoint> GetEnumerator()
            {
                ++this.IterationCount;

                return Enumerable.Range(0, 5).Select(x => new DataPoint(x, x)).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        class TestDataPointSeries : DataPointSeries
        {
            public override void Render(IRenderContext rc)
            {
                // Do nothing
            }

            public object TestGetItem(int i)
            {
                return this.GetItem(i);
            }
            public void TestUpdateData()
            {
                this.UpdateData();
            }
        }
    }
}
