// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeGenerator.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Generates c# code for the specified PlotModel.
    /// </summary>
    /// <remarks>
    /// This is useful for creating examples or unit tests.
    ///   Press Ctrl+Alt+C in a plot to copy code to the clipboard.
    ///   Usage:
    ///   var cg = new CodeGenerator(myPlotModel);
    ///   Clipboard.SetText(cg.ToCode());
    /// </remarks>
    public class CodeGenerator
    {
        #region Constants and Fields

        /// <summary>
        ///   The sb.
        /// </summary>
        private readonly StringBuilder sb;

        /// <summary>
        ///   The variables.
        /// </summary>
        private readonly HashSet<string> variables;

        /// <summary>
        ///   The indent string.
        /// </summary>
        private string indentString;

        /// <summary>
        ///   The indents.
        /// </summary>
        private int indents;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeGenerator"/> class.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        public CodeGenerator(PlotModel model)
        {
            this.variables = new HashSet<string>();
            this.sb = new StringBuilder();
            this.Indents = 8;
            var title = model.Title ?? "Untitled";
            this.AppendLine("[Example({0})]", title.ToCode());
            string methodName = this.MakeValidVariableName(title);
            this.AppendLine("public static PlotModel {0}()", methodName);
            this.AppendLine("{");
            this.Indents += 4;
            string modelName = this.Add(model);
            this.AddChildren(modelName, "Axes", model.Axes);
            this.AddChildren(modelName, "Series", model.Series);
            this.AddChildren(modelName, "Annotations", model.Annotations);
            this.AppendLine("return {0};", modelName);
            this.Indents -= 4;
            this.AppendLine("}");
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets Indents.
        /// </summary>
        private int Indents
        {
            get
            {
                return this.indents;
            }

            set
            {
                this.indents = value;
                this.indentString = new string(' ', value);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Formats the code.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <returns>
        /// The format code.
        /// </returns>
        public static string FormatCode(string format, params object[] values)
        {
            var encodedValues = new string[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                encodedValues[i] = values[i].ToCode();
            }

            return string.Format(format, encodedValues);
        }

        /// <summary>
        /// Formats a constructor.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="format">
        /// The format of the constructor arguments.
        /// </param>
        /// <param name="values">
        /// The argument values.
        /// </param>
        /// <returns>
        /// The format constructor.
        /// </returns>
        public static string FormatConstructor(Type type, string format, params object[] values)
        {
            return "new " + TypeHelper.GetTypeName(type) + "(" + FormatCode(format, values) + ")";
        }

        /// <summary>
        /// Returns the c# code for this model.
        /// </summary>
        /// <returns>
        /// C# code.
        /// </returns>
        public string ToCode()
        {
            return this.sb.ToString();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the specified object to the generated code.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The variable name.</returns>
        private string Add(object obj)
        {
            Type type = obj.GetType();
            object defaultInstance = Activator.CreateInstance(type);
            string varName = this.GetNewVariableName(type);
            this.variables.Add(varName);
            this.AppendLine("var {0} = new {1}();", varName, TypeHelper.GetTypeName(type));
            this.SetProperties(obj, varName, defaultInstance);
            return varName;
        }

        /// <summary>
        /// The add children.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="collectionName">
        /// The collection name.
        /// </param>
        /// <param name="children">
        /// The children.
        /// </param>
        private void AddChildren(string name, string collectionName, IEnumerable children)
        {
            foreach (var child in children)
            {
                string childName = this.Add(child);
                this.AppendLine("{0}.{1}.Add({2});", name, collectionName, childName);
            }
        }

        /// <summary>
        /// The add items.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="list">
        /// The list.
        /// </param>
        private void AddItems(string name, IList list)
        {
            foreach (var item in list)
            {
                var cgi = item as ICodeGenerating;
                if (cgi != null)
                {
                    this.AppendLine("{0}.Add({1});", name, cgi.ToCode());
                }
            }
        }

        /// <summary>
        /// The append line.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        private void AppendLine(string format, params object[] args)
        {
            if (args.Length > 0)
            {
                this.sb.AppendLine(this.indentString + string.Format(CultureInfo.InvariantCulture, format, args));
            }
            else
            {
                this.sb.AppendLine(this.indentString + format);
            }
        }

        /// <summary>
        /// The are lists equal.
        /// </summary>
        /// <param name="list1">
        /// The list 1.
        /// </param>
        /// <param name="list2">
        /// The list 2.
        /// </param>
        /// <returns>
        /// The are lists equal.
        /// </returns>
        private bool AreListsEqual(IList list1, IList list2)
        {
            if (list1 == null || list2 == null)
            {
                return false;
            }

            if (list1.Count != list2.Count)
            {
                return false;
            }

            for (int i = 0; i < list1.Count; i++)
            {
                if (!list1[i].Equals(list2[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// The get first attribute.
        /// </summary>
        /// <param name="pi">
        /// The pi.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// </returns>
        private T GetFirstAttribute<T>(PropertyInfo pi) where T : Attribute
        {
            foreach (T a in pi.GetCustomAttributes(typeof(CodeGenerationAttribute), true))
            {
                return a;
            }

            return null;
        }

        /// <summary>
        /// The get new variable name.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The get new variable name.
        /// </returns>
        private string GetNewVariableName(Type type)
        {
            string prefix = TypeHelper.GetTypeName(type);
            prefix = char.ToLower(prefix[0]) + prefix.Substring(1);
            int i = 1;
            while (this.variables.Contains(prefix + i))
            {
                i++;
            }

            return prefix + i;
        }

        /// <summary>
        /// Makes a valid variable name of a string. Invalid characters will simply be removed.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <returns>
        /// A valid variable name.
        /// </returns>
        private string MakeValidVariableName(string title)
        {
            if (title == null)
            {
                return null;
            }

            var regex = new Regex("[a-zA-Z_][a-zA-Z0-9_]*");
            var result = new StringBuilder();
            foreach (char c in title)
            {
                string s = c.ToString();
                if (regex.Match(s).Success)
                {
                    result.Append(s);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// The set properties.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="varName">
        /// The var name.
        /// </param>
        /// <param name="defaultValues">
        /// The default values.
        /// </param>
        private void SetProperties(object instance, string varName, object defaultValues)
        {
#if !METRO
            Type type = instance.GetType();
            var listsToAdd = new Dictionary<string, IList>();
            foreach (var pi in type.GetProperties())
            {
                // check the [CodeGeneration] attribute
                var cga = this.GetFirstAttribute<CodeGenerationAttribute>(pi);
                if (cga != null && !cga.GenerateCode)
                {
                    continue;
                }

                string name = varName + "." + pi.Name;
                object value = pi.GetValue(instance, null);
                object defaultValue = pi.GetValue(defaultValues, null);

                // check if lists are equal
                if (this.AreListsEqual(value as IList, defaultValue as IList))
                {
                    continue;
                }

                // only items in List<T>s where T:ICodeGenerating will be added
                var list = value as IList;
                if (list != null)
                {
                    listsToAdd.Add(name, list);
                    continue;
                }

                // only properties with public setters are used
                MethodInfo sm = pi.GetSetMethod();
                if (sm == null || !sm.IsPublic)
                {
                    continue;
                }

                // skip default values
                if ((value != null && value.Equals(defaultValue)) || value == defaultValue)
                {
                    continue;
                }

                this.SetProperty(pi.PropertyType, name, value);
            }

            // Add the items of the lists
            foreach (var kvp in listsToAdd)
            {
                var name = kvp.Key;
                var list = kvp.Value;
                Type listType = list.GetType();
                Type[] gargs = listType.GetGenericArguments();
                if (gargs.Length > 0)
                {
                    bool isCodeGenerating = gargs[0].GetInterfaces().Any(x => x == typeof(ICodeGenerating));
                    if (!isCodeGenerating)
                    {
                        continue;
                    }
                }

                this.AddItems(name, list);
            }

#endif
        }

        /// <summary>
        /// The set property.
        /// </summary>
        /// <param name="propertyType">
        /// The property type.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        private void SetProperty(Type propertyType, string name, object value)
        {
            string code = value.ToCode();
            if (code != null)
            {
                this.AppendLine("{0} = {1};", name, code);
            }
        }

        #endregion
    }
}