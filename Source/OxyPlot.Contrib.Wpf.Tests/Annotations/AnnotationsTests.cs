// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnnotationsTests.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf.Tests
{
    using System.Diagnostics.CodeAnalysis;

    using NUnit.Framework;

    // ReSharper disable InconsistentNaming
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestFixture]
    public class AnnotationsTests
    {
        public class DefaultValues
        {
            [Test]
            public void LineAnnotation()
            {
                var s1 = new LineAnnotation();
                var s2 = new Annotations.LineAnnotation();
                OxyAssert.PropertiesAreEqual(s2, s1);
            }

            [Test]
            public void PointAnnotation()
            {
                var s1 = new PointAnnotation();
                var s2 = new Annotations.PointAnnotation();
                OxyAssert.PropertiesAreEqual(s2, s1);
            }

            [Test]
            public void EllipseAnnotation()
            {
                var s1 = new EllipseAnnotation();
                var s2 = new Annotations.EllipseAnnotation();
                OxyAssert.PropertiesAreEqual(s2, s1);
            }

            [Test]
            public void FunctionAnnotation()
            {
                var s1 = new FunctionAnnotation();
                var s2 = new Annotations.FunctionAnnotation();
                OxyAssert.PropertiesAreEqual(s2, s1);
            }

            [Test]
            public void RectangleAnnotation()
            {
                var s1 = new RectangleAnnotation();
                var s2 = new Annotations.RectangleAnnotation();
                OxyAssert.PropertiesAreEqual(s2, s1);
            }

            [Test]
            public void ArrowAnnotation()
            {
                var s1 = new ArrowAnnotation();
                var s2 = new Annotations.ArrowAnnotation();
                OxyAssert.PropertiesAreEqual(s2, s1);
            }

            [Test]
            public void PolygonAnnotation()
            {
                var s1 = new PolygonAnnotation();
                var s2 = new Annotations.PolygonAnnotation();
                OxyAssert.PropertiesAreEqual(s2, s1);
            }

            [Test]
            public void PolylineAnnotation()
            {
                var s1 = new PolylineAnnotation();
                var s2 = new Annotations.PolylineAnnotation();
                OxyAssert.PropertiesAreEqual(s2, s1);
            }

            [Test]
            public void TextAnnotation()
            {
                var s1 = new TextAnnotation();
                var s2 = new Annotations.TextAnnotation();
                OxyAssert.PropertiesAreEqual(s2, s1);
            }
        }
    }
}
