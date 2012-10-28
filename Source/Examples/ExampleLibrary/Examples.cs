// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Examples.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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
#if PCL
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
#if PCL
                var types = new List<TypeInfo>();
#else
                var types = new List<Type>();
#endif
                var baseType = type;
                while (baseType != null)
                {
                    types.Add(baseType);
#if PCL
                    baseType = baseType.BaseType == null ? null : baseType.BaseType.GetTypeInfo();
#else
                    baseType = baseType.BaseType;
#endif
                }

                foreach (var t in types)
                {
#if PCL
                    foreach (var method in t.DeclaredMethods)//.GetMethods(BindingFlags.Public | BindingFlags.Static))
#else
                    foreach (var method in t.GetMethods(BindingFlags.Public | BindingFlags.Static))
#endif
                    {
#if PCL
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