// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ElementCollectionTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides unit tests for the <see cref="ElementCollection{T}"/> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Tests.Graphics
{
    using NUnit.Framework;
    using OxyPlot.Axes;

    /// <summary>
    /// Provides unit tests for the <see cref="ElementCollection{T}"/> class.
    /// </summary>
    public class ElementCollectionTests
    {
        /// <summary>
        /// The add method unit tests.
        /// </summary>
        [TestFixture]
        public class TheAddMethod
        {
            /// <summary>
            /// Tests whether the collection changed event is raised.
            /// </summary>
            [TestCase]
            public void RaisesCollectionChangedEvent()
            {
                var collection = new ElementCollection<Axis>(new PlotModel());

                ElementCollectionChangedEventArgs<Axis> eventArgs = null;
                var raisedCount = 0;

                collection.CollectionChanged += (sender, e) =>
                {
                    eventArgs = e;
                    raisedCount++;
                };

                var axis = new LinearAxis();
                collection.Add(axis);

                Assert.AreEqual(1, raisedCount);
                Assert.AreEqual(1, eventArgs.AddedItems.Count);
                Assert.IsTrue(ReferenceEquals(axis, eventArgs.AddedItems[0]));
            }
        }

        /// <summary>
        /// The insert method unit tests.
        /// </summary>
        [TestFixture]
        public class TheInsertMethod
        {
            /// <summary>
            /// Tests whether the collection changed event is raised.
            /// </summary>
            [TestCase]
            public void RaisesCollectionChangedEvent()
            {
                var collection = new ElementCollection<Axis>(new PlotModel());

                collection.Add(new LinearAxis());
                collection.Add(new LinearAxis());
                collection.Add(new LinearAxis());

                ElementCollectionChangedEventArgs<Axis> eventArgs = null;
                var raisedCount = 0;

                collection.CollectionChanged += (sender, e) =>
                {
                    eventArgs = e;
                    raisedCount++;
                };

                var axis = new LinearAxis();
                collection.Insert(1, axis);

                Assert.AreEqual(1, raisedCount);
                Assert.AreEqual(1, eventArgs.AddedItems.Count);
                Assert.IsTrue(ReferenceEquals(axis, eventArgs.AddedItems[0]));
            }
        }

        /// <summary>
        /// The clear method unit tests.
        /// </summary>
        [TestFixture]
        public class TheClearMethod
        {
            /// <summary>
            /// Tests whether the collection changed event is raised.
            /// </summary>
            [TestCase]
            public void RaisesCollectionChangedEvent()
            {
                var collection = new ElementCollection<Axis>(new PlotModel());

                var axis = new LinearAxis();
                collection.Add(axis);

                ElementCollectionChangedEventArgs<Axis> eventArgs = null;
                var raisedCount = 0;

                collection.CollectionChanged += (sender, e) =>
                {
                    eventArgs = e;
                    raisedCount++;
                };

                collection.Clear();

                Assert.AreEqual(1, raisedCount);
                Assert.AreEqual(1, eventArgs.RemovedItems.Count);
                Assert.IsTrue(ReferenceEquals(axis, eventArgs.RemovedItems[0]));
            }
        }

        /// <summary>
        /// The remove method unit tests.
        /// </summary>
        [TestFixture]
        public class TheRemoveMethod
        {
            /// <summary>
            /// Tests whether the collection changed event is raised.
            /// </summary>
            [TestCase]
            public void RaisesCollectionChangedEvent()
            {
                var collection = new ElementCollection<Axis>(new PlotModel());

                var axis = new LinearAxis();
                collection.Add(axis);

                ElementCollectionChangedEventArgs<Axis> eventArgs = null;
                var raisedCount = 0;

                collection.CollectionChanged += (sender, e) =>
                {
                    eventArgs = e;
                    raisedCount++;
                };

                collection.Remove(axis);

                Assert.AreEqual(1, raisedCount);
                Assert.AreEqual(1, eventArgs.RemovedItems.Count);
                Assert.IsTrue(ReferenceEquals(axis, eventArgs.RemovedItems[0]));
            }
        }

        /// <summary>
        /// The remove at method unit tests.
        /// </summary>
        [TestFixture]
        public class TheRemoveAtMethod
        {
            /// <summary>
            /// Tests whether the collection changed event is raised.
            /// </summary>
            [TestCase]
            public void RaisesCollectionChangedEvent()
            {
                var collection = new ElementCollection<Axis>(new PlotModel());

                var axis = new LinearAxis();
                collection.Add(axis);

                ElementCollectionChangedEventArgs<Axis> eventArgs = null;
                var raisedCount = 0;

                collection.CollectionChanged += (sender, e) =>
                {
                    eventArgs = e;
                    raisedCount++;
                };

                collection.RemoveAt(0);

                Assert.AreEqual(1, raisedCount);
                Assert.AreEqual(1, eventArgs.RemovedItems.Count);
                Assert.IsTrue(ReferenceEquals(axis, eventArgs.RemovedItems[0]));
            }
        }
    }
}