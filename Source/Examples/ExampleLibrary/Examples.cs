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
#if !UNIVERSAL
        public static IEnumerable<T> GetCustomAttributes<T>(this Type type) where T : Attribute
        {
            return type.GetCustomAttributes(typeof(T), true).Cast<T>();
        }

        public static IEnumerable<T> GetCustomAttributes<T>(this MethodInfo info) where T : Attribute
        {
            return info.GetCustomAttributes(typeof(T), true).Cast<T>();
        }
#endif

        /// <summary>
        /// Gets the list of examples.
        /// </summary>
        /// <returns>The list of examples.</returns>
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
#if UNIVERSAL
                    types.Add(baseType.AsType());
                    baseType = baseType.BaseType != null ? baseType.BaseType.GetTypeInfo() : null;
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