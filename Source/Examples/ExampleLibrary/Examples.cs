// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Examples.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class Examples
    {
        public static List<ExampleInfo> GetList()
        {
            var list = new List<ExampleInfo>();
            var assemblyTypes = typeof(Examples).Assembly.GetTypes();

            foreach (var type in assemblyTypes)
            {
                var examplesAttributes = type.GetCustomAttributes(typeof(ExamplesAttribute), true).ToArray();
                if (examplesAttributes.Length == 0)
                {
                    continue;
                }

                var examplesAttribute = examplesAttributes[0] as ExamplesAttribute;
                var types = new List<Type>();
                var baseType = type;
                while (baseType != null)
                {
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
                        if (examplesAttribute != null)
                        {
                            list.Add(new ExampleInfo(examplesAttribute.Category, exampleAttribute.Title, method));
                        }
                    }
                }
            }

            return list;
        }
    }
}