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
#if UNIVERSAL
            var assemblyTypes = typeof(Examples).GetTypeInfo().Assembly.DefinedTypes;
#else
            var assemblyTypes = typeof(Examples).Assembly.GetTypes();
#endif

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
#if UNIVERSAL
                    types.Add(baseType.AsType());
                    baseType = null;
#else
                    types.Add(baseType);
                    baseType = baseType.BaseType;
#endif
                }

                foreach (var t in types)
                {
#if UNIVERSAL
                    var methods = t.GetRuntimeMethods();
#else
                    var methods = t.GetMethods(BindingFlags.Public | BindingFlags.Static);
#endif

                    foreach (var method in methods)
                    {
                        try
                        {
                            var exampleAttributes = method.GetCustomAttributes(typeof(ExampleAttribute), true).ToList();
                            if (!exampleAttributes.Any())
                            {
                                continue;
                            }

                            var exampleAttribute = exampleAttributes.ElementAtOrDefault(0) as ExampleAttribute;
                            if (exampleAttribute != null && examplesAttribute != null)
                            {
                                list.Add(new ExampleInfo(examplesAttribute.Category, exampleAttribute.Title, method));
                            }
                        }
                        catch(Exception) { }
                    }
                }
            }

            return list;
        }
    }
}