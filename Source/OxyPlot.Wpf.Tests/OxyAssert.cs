// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyAssert.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf.Tests
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;

    public class OxyAssert
    {
        /// <summary>
        /// Check that all public properties in o1 exists in o2, and that the values are equal.
        /// </summary>
        public static void PropertiesAreEqual(object o1, object o2)
        {
            var p1 = TypeDescriptor.GetProperties(o1);
            var p2 = TypeDescriptor.GetProperties(o2);
            var result = true;
            foreach (PropertyDescriptor pd1 in p1)
            {
                if (pd1.ComponentType != o1.GetType())
                    continue;
                var v1 = pd1.GetValue(o1);
                var pd2 = p2[pd1.Name];
                if (pd2 == null)
                {
                    Debug.WriteLine("Property {0} not found in {1}", pd1.Name, o2.GetType());
                    continue;
                }
                var v2 = pd2.GetValue(o2);
                if (v1 != null && v2!=null && v1.ToString().Equals(v2.ToString(), StringComparison.InvariantCultureIgnoreCase)) continue;
                if (v1 == v2) continue;
                Debug.WriteLine("The default values of property {0} is different in {1} and {2}.", pd1.Name, o1.GetType(), o2.GetType());
                Debug.WriteLine("  {1}.{0} = {3} and {2}.{0} = {4}", pd1.Name, o1.GetType(), o2.GetType(), v1, v2);
                result = false;
            }

            NUnit.Framework.Assert.IsTrue(result);
        }
    }
}