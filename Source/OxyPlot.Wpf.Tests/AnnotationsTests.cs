// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnnotationsTests.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
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
        [Test]
        public void LineAnnotation()
        {
            var s1 = new LineAnnotation();
            var s2 = new Annotations.LineAnnotation();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }
    
        [Test]
        public void EllipseAnnotation()
        {
            var s1 = new EllipseAnnotation();
            var s2 = new Annotations.EllipseAnnotation();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }

        [Test]
        public void RectangleAnnotation()
        {
            var s1 = new RectangleAnnotation();
            var s2 = new Annotations.RectangleAnnotation();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }

        [Test]
        public void ArrowAnnotation()
        {
            var s1 = new ArrowAnnotation();
            var s2 = new Annotations.ArrowAnnotation();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }

        [Test]
        public void PolygonAnnotation()
        {
            var s1 = new PolygonAnnotation();
            var s2 = new Annotations.PolygonAnnotation();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }

        [Test]
        public void TextAnnotation()
        {
            var s1 = new TextAnnotation();
            var s2 = new Annotations.TextAnnotation();
            OxyAssert.PropertiesAreEqual(s1, s2);
        }
    }
}