// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyAssert.cs" company="OxyPlot">
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
// <summary>
//   Check that all public properties in t1 exists in t2.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace OxyPlot.Wpf.Tests
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Media;

    public class OxyAssert
    {
        /// <summary>
        /// Check that all public properties in t1 exists in t2.
        /// </summary>
        /// <param name="t1">The source type.</param>
        /// <param name="t2">The type to check.</param>
        /// <param name="failOnMissingProperty">Create an NUnit assertion if a property is missing.</param>
        public static void PropertiesExist(Type t1, Type t2, bool failOnMissingProperty = false)
        {
            var p1 = TypeDescriptor.GetProperties(t1);
            var p2 = TypeDescriptor.GetProperties(t2);
            var result = true;
            foreach (PropertyDescriptor pd1 in p1)
            {
                if (pd1.ComponentType != t1 || pd1.IsReadOnly)
                {
                    continue;
                }

                var pd2 = p2[pd1.Name];
                if (pd2 == null)
                {
                    Console.WriteLine("Property {0} not found in {1}", pd1.Name, t2);
                    if (failOnMissingProperty)
                    {
                        result = false;
                    }
                }
            }

            NUnit.Framework.Assert.IsTrue(result);
        }

        /// <summary>
        /// Check that all public properties in o1 exists in o2, and that the default values are equal.
        /// </summary>
        public static void PropertiesAreEqual(object o1, object o2)
        {
            var p1 = TypeDescriptor.GetProperties(o1);
            var p2 = TypeDescriptor.GetProperties(o2);
            var result = true;
            foreach (PropertyDescriptor pd1 in p1)
            {
                if (pd1.ComponentType != o1.GetType())
                {
                    continue;
                }

                var v1 = pd1.GetValue(o1);
                var pd2 = p2[pd1.Name];
                if (pd2 == null)
                {
                    Console.WriteLine("Property {0} not found in {1}", pd1.Name, o2.GetType());
                    continue;
                }

                var v2 = pd2.GetValue(o2);
                if (AreEqual(v1, v2))
                {
                    continue;
                }

                if (v1 == v2)
                {
                    continue;
                }

                Debug.WriteLine("The default values of property {0} is different in {1} and {2}.", pd1.Name, o1.GetType(), o2.GetType());
                Debug.WriteLine("  {1}.{0} = {3} and {2}.{0} = {4}", pd1.Name, o1.GetType(), o2.GetType(), v1, v2);
                result = false;
            }

            NUnit.Framework.Assert.IsTrue(result);
        }

        private static bool AreEqual(object v1, object v2)
        {
            if (v1 == null || v2 == null)
            {
                return v1 == null && v2 == null;
            }

            if (v1 is Color && v2 is OxyColor)
            {
                return ((OxyColor)v2).ToColor().Equals((Color)v1);
            }

            if (v1 is Vector && v2 is ScreenVector)
            {
                return ((ScreenVector)v2).ToVector().Equals((Vector)v1);
            }

            if (v1 is Thickness && v2 is OxyThickness)
            {
                return ((OxyThickness)v2).ToThickness().Equals((Thickness)v1);
            }

            if (v1 is IList && v2 is IList)
            {
                return AreEqual((IList)v1, (IList)v2);
            }

            return v1.Equals(v2);
        }

        private static bool AreEqual(IList v1, IList v2)
        {
            if (v1.Count != v2.Count)
            {
                return false;
            }

            for (int i = 0; i < v1.Count; i++)
            {
                if (!AreEqual(v1[i], v2[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}