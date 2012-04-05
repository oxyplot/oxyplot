// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OxyAssert.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf.Tests
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using NUnit.Framework;
    using OxyPlot.Wpf;

    public class OxyAssert
    {
        /// <summary>
        /// Asserts that a plot is equal to the plot stored in the "baseline" folder.
        /// 1. Renders the plot to file.png
        /// 2. If the baseline does not exist, the current plot is copied to the baseline folder.
        /// 3. Checks that the png file is equal to a baseline png.
        /// </summary>
        /// <param name="plot">The plot.</param>
        /// <param name="name">The name of the baseline file.</param>
        public static void AreEqual(PlotModel plot, string name)
        {
            // string name = new System.Diagnostics.StackFrame(1).GetMethod().Name;
            string path = name + ".png";
            string baseline = @"baseline\" + path;
            PngExporter.Export(plot, path, 800, 500, OxyColors.White);

            if (!Directory.Exists("baseline"))
            {
                Directory.CreateDirectory("baseline");
            }

            if (!File.Exists(baseline))
            {
                File.Copy(path, baseline);
            }

            var baselineImage = new Bitmap(baseline);
            var plotImage = new Bitmap(path);
            Assert.AreEqual(baselineImage.Width, plotImage.Width, "Image width");
            Assert.AreEqual(baselineImage.Height, plotImage.Height, "Image height");

            for (int x = 0; x < baselineImage.Width; x++)
            {
                for (int y = 0; y < baselineImage.Height; y++)
                {
                    Assert.AreEqual(
                        baselineImage.GetPixel(x, y), plotImage.GetPixel(x, y), string.Format("Pixel ({0},{1})", x, y));
                }
            }
        }

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
                    Console.WriteLine("Property {0} not found in {1}", pd1.Name, o2.GetType());
                    continue;
                }
                var v2 = pd2.GetValue(o2);
                if (v1 != null && v2!=null && v1.ToString().Equals(v2.ToString(), StringComparison.InvariantCultureIgnoreCase)) continue;
                if (v1 == v2) continue;
                Console.WriteLine("The default values of property {0} is different in {1} and {2}.", pd1.Name, o1.GetType(), o2.GetType());
                Console.WriteLine("  {1}.{0} = {3} and {2}.{0} = {4}", pd1.Name, o1.GetType(), o2.GetType(), v1, v2);
                result = false;
            }

            NUnit.Framework.Assert.IsTrue(result);
        }
    }
}