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
            foreach (var type in typeof(Examples).GetTypeInfo().Assembly.DefinedTypes)
            {
                var examplesAttributes = type.GetCustomAttributes(typeof(ExamplesAttribute), true).ToArray();
                if (examplesAttributes.Length == 0)
                {
                    continue;
                }

                var examplesAttribute = examplesAttributes[0] as ExamplesAttribute;
                var types = new List<TypeInfo>();
                var baseType = type;
                while (baseType != null)
                {
                    System.Diagnostics.Debug.WriteLine(baseType);
                    types.Add(baseType);
                    baseType = baseType.BaseType == null ? null : baseType.BaseType.GetTypeInfo();                                        
                }

                foreach (var t in types)
                {
                    foreach (var method in t.DeclaredMethods)//.GetMethods(BindingFlags.Public | BindingFlags.Static))
                    {
                        var exampleAttributes = method.GetCustomAttributes(typeof(ExampleAttribute), true).ToArray();
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