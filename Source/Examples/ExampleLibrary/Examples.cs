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
        /// Gets all examples in the specified category.
        /// </summary>
        /// <param name="categoryType">The type of the category.</param>
        public static IEnumerable<ExampleInfo> GetCategory(Type categoryType)
        {
            var typeInfo = categoryType.GetTypeInfo();
            var examplesAttribute = typeInfo.GetCustomAttributes<ExamplesAttribute>().FirstOrDefault();
            var examplesTags = typeInfo.GetCustomAttributes<TagsAttribute>().FirstOrDefault() ?? new TagsAttribute();

            var types = new List<Type>();
            var baseType = typeInfo;
            while (baseType != null)
            {
                types.Add(baseType.AsType());
                baseType = baseType.BaseType?.GetTypeInfo();
            }

            foreach (var t in types)
            {
                foreach (var method in t.GetRuntimeMethods())
                {
                    var exampleAttribute = method.GetCustomAttributes<ExampleAttribute>().FirstOrDefault();
                    if (exampleAttribute != null)
                    {
                        var exampleTags = method.GetCustomAttributes<TagsAttribute>().FirstOrDefault() ?? new TagsAttribute();
                        var tags = new List<string>(examplesTags.Tags);
                        tags.AddRange(exampleTags.Tags);
                        yield return
                            new ExampleInfo(
                                examplesAttribute.Category,
                                exampleAttribute.Title,
                                tags.ToArray(),
                                method,
                                exampleAttribute.ExcludeFromAutomatedTests);
                    }
                }
            }
        }

        /// <summary>
        /// Gets all examples.
        /// </summary>
        public static IEnumerable<ExampleInfo> GetList()
        {
            foreach (var type in typeof(Examples).GetTypeInfo().Assembly.DefinedTypes)
            {
                if (!type.GetCustomAttributes<ExamplesAttribute>().Any())
                {
                    continue;
                }

                foreach (var example in GetCategory(type.AsType()))
                {
                    yield return example;
                }
            }
        }

        /// <summary>
        /// Gets all examples suitable for automated test.
        /// </summary>
        public static IEnumerable<ExampleInfo> GetListForAutomatedTest()
        {
            return GetList().Where(ex => !ex.ExcludeFromAutomatedTests);
        }

        /// <summary>
        /// Gets the first example of each category suitable for automated test.
        /// </summary>
        public static IEnumerable<ExampleInfo> GetFirstExampleOfEachCategoryForAutomatedTest()
        {
            return GetListForAutomatedTest()
                .GroupBy(example => example.Category)
                .Select(group => group.First());
        }

        /// <summary>
        /// Gets the 'rendering capabilities' examples suitable for automated test.
        /// </summary>
        public static IEnumerable<ExampleInfo> GetRenderingCapabilitiesForAutomatedTest()
        {
            return GetCategory(typeof(RenderingCapabilities)).Where(ex => !ex.ExcludeFromAutomatedTests);
        }
    }
}
