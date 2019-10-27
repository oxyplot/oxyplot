// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyAssert.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Provides specialized unit test assertion methods.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;

    using NUnit.Framework;

    /// <summary>
    /// Provides specialized unit test assertion methods.
    /// </summary>
    internal class OxyAssert
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
                    Console.WriteLine(@"Property {0} not found in {1}", pd1.Name, t2);
                    if (failOnMissingProperty)
                    {
                        result = false;
                    }
                }
            }

            Assert.IsTrue(result);
        }

        /// <summary>
        /// Check that all public properties in o1 exists in o2, and that the default values are equal.
        /// </summary>
        /// <param name="o1">The first object.</param>
        /// <param name="o2">The second object.</param>
        /// <param name="testType1">Type of the assertion.</param>
        public static void PropertiesAreEqual(object o1, object o2, Type testType1 = null)
        {
            if (testType1 == null)
            {
                testType1 = o1.GetType();
            }

            var p1 = TypeDescriptor.GetProperties(testType1);
            var p2 = TypeDescriptor.GetProperties(o2);
            var result = true;
            foreach (PropertyDescriptor pd1 in p1)
            {
                if (pd1.IsReadOnly)
                {
                    continue;
                }

                var propertyName = pd1.Name;
                if (propertyName == "IsVisible")
                {
                    propertyName = "Visibility";
                }

                if (propertyName == "FontSize")
                {
                    continue;
                }

                var v1 = pd1.GetValue(o1);
                var pd2 = p2[propertyName];
                if (pd2 == null)
                {
                    Console.WriteLine(@"{0}: missing in {1}", propertyName, o2.GetType());
                    continue;
                }

                var v2 = pd2.GetValue(o2);
                v2 = ConvertToOxyPlotObject(v2, pd2.PropertyType);

                var type1 = v1 != null ? v1.GetType() : null;
                var type2 = v2 != null ? v2.GetType() : null;
                if (!AreEqual(type1, type2))
                {
                    Console.WriteLine(@"{0}: {1} / {2}", pd1.Name, type1, type2);
                }

                var list1 = v1 as IList;
                var list2 = v2 as IList;
                if (list1 != null)
                {
                    if (list1.Count != list2.Count)
                    {
                        Console.WriteLine(@"{0}: {1} / {2}", pd1.Name, list1.Count, list2.Count);
                        result = false;
                        continue;
                    }

                    for (int i = 0; i < list1.Count; i++)
                    {
                        if (!AreEqual(list1[i], list2[i]))
                        {
                            Console.WriteLine(@"{0}[{1}]: {2} / {3}", pd1.Name, i, list1[i], list2[i]);
                            result = false;
                        }
                    }

                    continue;
                }

                if (AreEqual(v1, v2))
                {
                    continue;
                }

                if (v1 == v2)
                {
                    continue;
                }

                Console.WriteLine(@"{0}: {1} / {2}", pd1.Name, v1, v2);
                result = false;
            }

            Assert.IsTrue(result);
        }

        /// <summary>
        /// Converts the specified object to the corresponding OxyPlot object.
        /// </summary>
        /// <param name="value">The object to convert.</param>
        /// <param name="type">The type.</param>
        /// <returns>
        /// A converted <see cref="Object"/>.
        /// </returns>
        private static object ConvertToOxyPlotObject(object value, Type type)
        {
            if (value is Color)
            {
                return ((Color)value).ToOxyColor();
            }

            var listOfColor = value as IList<Color>;
            if (listOfColor != null)
            {
                return listOfColor.Select(c => c.ToOxyColor()).ToList();
            }

            if (typeof(Brush).IsAssignableFrom(type))
            {
                return ((Brush)value).ToOxyColor();
            }

            if (value is Visibility)
            {
                return ((Visibility)value) == Visibility.Visible;
            }

            if (value is Vector)
            {
                return ((Vector)value).ToScreenVector();
            }

            if (value is Thickness)
            {
                return ((Thickness)value).ToOxyThickness();
            }

            if (value is HorizontalAlignment)
            {
                return ((HorizontalAlignment)value).ToHorizontalAlignment();
            }

            if (value is VerticalAlignment)
            {
                return ((VerticalAlignment)value).ToVerticalAlignment();
            }

            if (value is FontWeight)
            {
                return (double)((FontWeight)value).ToOpenTypeWeight();
            }

            return value;
        }

        /// <summary>
        /// Determines if the specified objects are equal. Converts from Wpf to OxyPlot types if necessary.
        /// </summary>
        /// <param name="v1">The first object to compare.</param>
        /// <param name="v2">The second object to compare.</param>
        /// <returns><c>true</c> if the objects are equal.</returns>
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

        /// <summary>
        /// Determines if the specified lists are equal. Converts from Wpf to OxyPlot types if necessary.
        /// </summary>
        /// <param name="v1">The first list to compare.</param>
        /// <param name="v2">The second list to compare.</param>
        /// <returns><c>true</c> if the lists are equal.</returns>
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
