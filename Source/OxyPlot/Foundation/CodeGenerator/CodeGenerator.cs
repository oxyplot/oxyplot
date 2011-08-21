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
    /// This class generates c# code for the specified PlotModel.
    /// This is useful for creating examples or unit tests.
    /// Press Ctrl+Alt+C in a plot to copy code to the clipboard.
    /// Usage:
    ///   var cg = new CodeGenerator(myPlotModel);
    ///   Clipboard.SetText(cg.ToCode());
    /// </summary>
    public class CodeGenerator
    {
        /// <summary>
        /// Formats a constructor.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="format">The format of the constructor arguments.</param>
        /// <param name="values">The argument values.</param>
        /// <returns></returns>
        public static string FormatConstructor(Type type, string format, params object[] values)
        {
            return "new " + type.Name + "(" + FormatCode(format, values) + ")";
        }

        /// <summary>
        /// Formats the code.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static string FormatCode(string format, params object[] values)
        {
            string[] encodedValues = new string[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                encodedValues[i] = values[i].ToCode();
            }
            return String.Format(format, encodedValues);
        }

        #region Constants and Fields

        private readonly StringBuilder sb;

        private readonly HashSet<string> variables;

        private string indentString;

        private int indents;

        #endregion

        #region Constructors and Destructors

        public CodeGenerator(PlotModel model)
        {
            this.variables = new HashSet<string>();
            this.sb = new StringBuilder();
            this.Indents = 8;
            this.AppendLine("[Example({0})]", model.Title.ToCode());
            string methodName = this.MakeValidVariableName(model.Title) ?? "Untitled";
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
        /// Returns the c# code for this model.
        /// </summary>
        /// <returns>C# code.</returns>
        public string ToCode()
        {
            return this.sb.ToString();
        }

        #endregion

        #region Methods

        private string Add(object obj)
        {
            Type type = obj.GetType();
            object defaultInstance = Activator.CreateInstance(type);
            string varName = this.GetNewVariableName(type);
            this.variables.Add(varName);
            this.AppendLine("var {0} = new {1}();", varName, type.Name);
            this.SetProperties(obj, varName, defaultInstance);
            return varName;
        }

        private void AddChildren(string name, string collectionName, IEnumerable children)
        {
            foreach (object child in children)
            {
                string childName = this.Add(child);
                this.AppendLine("{0}.{1}.Add({2});", name, collectionName, childName);
            }
        }

        private void AddItems(string name, IList list)
        {
            foreach (object item in list)
            {
                var cgi = item as ICodeGenerating;
                if (cgi != null)
                {
                    this.AppendLine("{0}.Add({1});", name, cgi.ToCode());
                }
            }
        }

        private void AppendLine(string format, params object[] args)
        {
            if (args.Length > 0)
            {
                this.sb.AppendLine(this.indentString + String.Format(CultureInfo.InvariantCulture, format, args));
            }
            else
            {
                this.sb.AppendLine(this.indentString + format);
            }
        }

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

        private T GetFirstAttribute<T>(PropertyInfo pi) where T : class
        {
            foreach (T a in pi.GetCustomAttributes(typeof(CodeGenerationAttribute), true))
            {
                return a;
            }
            return null;
        }

        private string GetNewVariableName(Type type)
        {
            string prefix = type.Name;
            prefix = Char.ToLower(prefix[0]) + prefix.Substring(1);
            int i = 1;
            while (this.variables.Contains(prefix + i))
            {
                i++;
            }
            return prefix + i;
        }

        /// <summary>
        /// Makes a valid variable of a string.
        /// Invalid characters will simply be removed.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns></returns>
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


        private void SetProperties(object o, string varName, object defaultValues)
        {
            Type type = o.GetType();
            foreach (PropertyInfo pi in type.GetProperties())
            {
                // check the [CodeGeneration] attribute
                var cga = this.GetFirstAttribute<CodeGenerationAttribute>(pi);
                if (cga != null && !cga.GenerateCode)
                {
                    continue;
                }

                string name = varName + "." + pi.Name;
                object value = pi.GetValue(o, null);
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
        }

        private void SetProperty(Type propertyType, string name, object value)
        {
            var code = value.ToCode();
            if (code != null)
                this.AppendLine("{0} = {1};", name, code);
        }

        #endregion
    }
}