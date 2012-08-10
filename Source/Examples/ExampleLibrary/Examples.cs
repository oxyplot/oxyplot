// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Examples.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Reflection;

namespace ExampleLibrary
{
    using System;

    public static class Examples
    {
        public static List<ExampleInfo> GetList()
        {
            var list = new List<ExampleInfo>();
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                var examplesAttributes = type.GetCustomAttributes(typeof(ExamplesAttribute), true);
                if (examplesAttributes.Length == 0)
                {
                    continue;
                }

                var examplesAttribute = examplesAttributes[0] as ExamplesAttribute;
                var types = new List<Type>();
                var baseType = type;
                while (baseType != null)
                {
                    Console.WriteLine(baseType);
                    types.Add(baseType);
                    baseType = baseType.BaseType;
                }
                foreach (var t in types)
                {
                    foreach (var method in t.GetMethods(BindingFlags.Public | BindingFlags.Static))
                    {
                        var exampleAttributes = method.GetCustomAttributes(typeof(ExampleAttribute), true);
                        if (exampleAttributes.Length == 0)
                        {
                            continue;
                        }

                        var exampleAttribute = (ExampleAttribute)exampleAttributes[0];
                        list.Add(new ExampleInfo(examplesAttribute.Category, exampleAttribute.Title, method));
                    }
                }
            };
            return list;
        }
    }
}