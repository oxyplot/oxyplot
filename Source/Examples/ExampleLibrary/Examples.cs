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

    /// <summary>
    /// Enumerates all examples in the assembly.
    /// </summary>
    public static class Examples
    {
        /// <summary>
        /// Gets the list of examples.
        /// </summary>
        /// <returns>The list of examples.</returns>
        public static List<ExampleInfo> GetList()
        {
            var list = new List<ExampleInfo>();
            var assemblyTypes = typeof(Examples).GetTypeInfo().Assembly.DefinedTypes;

            foreach (var type in assemblyTypes)
            {
                var examplesAttribute = type.GetCustomAttributes<ExamplesAttribute>().FirstOrDefault();
                if (examplesAttribute == null)
                {
                    continue;
                }

                var examplesTags = type.GetCustomAttributes<TagsAttribute>().FirstOrDefault() ?? new TagsAttribute();

                var types = new List<Type>();
                var baseType = type;
                while (baseType != null)
                {
                    types.Add(baseType.AsType());
                    baseType = baseType.BaseType != null ? baseType.BaseType.GetTypeInfo() : null;
                }

                foreach (var t in types)
                {
                    var methods = t.GetRuntimeMethods();

                    foreach (var method in methods)
                    {
                        try
                        {
                            var exampleAttribute = method.GetCustomAttributes<ExampleAttribute>().FirstOrDefault();
                            if (exampleAttribute != null)
                            {
                                var exampleTags = method.GetCustomAttributes<TagsAttribute>().FirstOrDefault() ?? new TagsAttribute();
                                var tags = new List<string>(examplesTags.Tags);
                                tags.AddRange(exampleTags.Tags);
                                list.Add(
                                    new ExampleInfo(
                                        examplesAttribute.Category,
                                        exampleAttribute.Title,
                                        tags.ToArray(),
                                        method));
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }

            return list;
        }
    }
}