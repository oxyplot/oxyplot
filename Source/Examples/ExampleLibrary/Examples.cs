// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Examples.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace ExampleLibrary
{
    using System;
    using OxyPlot;

    public static class Examples
    {
        public static List<ExampleInfo> GetList()
        {
            var list = new List<ExampleInfo>();
#if METRO
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
#if METRO
                var types = new List<TypeInfo>();
#else
                var types = new List<Type>();
#endif
                var baseType = type;
                while (baseType != null)
                {
                    types.Add(baseType);
#if METRO
                    baseType = baseType.BaseType == null ? null : baseType.BaseType.GetTypeInfo();
#else
                    baseType = baseType.BaseType;
#endif
                }

                foreach (var t in types)
                {
#if METRO
                    foreach (var method in t.DeclaredMethods)//.GetMethods(BindingFlags.Public | BindingFlags.Static))
#else
                    foreach (var method in t.GetMethods(BindingFlags.Public | BindingFlags.Static))                   
#endif
                    {
#if METRO
                        var exampleAttributes = method.GetCustomAttributes(typeof(ExampleAttribute), true).ToArray();
#else
                        var exampleAttributes = method.GetCustomAttributes(typeof(ExampleAttribute), true);
#endif
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