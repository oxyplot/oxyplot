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
        /// Gets the first or default attribute of the specified type.
        /// </summary>
        /// <typeparam name="T">The attribute type.</typeparam>
        /// <param name="type">The type to reflect.</param>
        /// <returns>The attribute.</returns>
        public static T FirstOrDefault<T>(this Type type) where T : Attribute
        {
            var attributes = type.GetCustomAttributes(typeof(T), true).ToArray();
            return attributes.Length == 0 ? null : (T)attributes[0];
        }

        /// <summary>
        /// Gets the first or default attribute of the specified type.
        /// </summary>
        /// <typeparam name="T">The attribute type.</typeparam>
        /// <param name="info">The information.</param>
        /// <returns>
        /// The attribute.
        /// </returns>
        public static T FirstOrDefault<T>(this MethodInfo info) where T : Attribute
        {
            var attributes = info.GetCustomAttributes(typeof(T), true).ToArray();
            return attributes.Length == 0 ? null : (T)attributes[0];
        }

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
                var examplesAttribute = type.FirstOrDefault<ExamplesAttribute>();
                if (examplesAttribute == null)
                {
                    continue;
                }

                var examplesTags = type.FirstOrDefault<TagsAttribute>() ?? new TagsAttribute();

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
                            var exampleAttribute = method.FirstOrDefault<ExampleAttribute>();
                            if (exampleAttribute != null)
                            {
                                var exampleTags = method.FirstOrDefault<TagsAttribute>() ?? new TagsAttribute();
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