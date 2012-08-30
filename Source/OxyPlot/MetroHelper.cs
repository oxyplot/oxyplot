// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetroHelper.cs" company="OxyPlot">
//   http://oxyplot.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace OxyPlot
{
    public static class MetroHelper
    {
        public static PropertyInfo GetProperty(this Type type, string name)
        {
            return type.GetTypeInfo().GetDeclaredProperty(name);
        }
        public static FieldInfo[] GetFields(this Type type, BindingFlags flags) {
            return null;
        }
        public static string ToShortDateString(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }              
        
    }
    public enum BindingFlags { Public, Static }

    public static class Path {
        public static string GetFileName(string path) {
            return path;
        }
    }
    public static class Assembly
    {
        public static AssemblyName GetExecutingAssembly()
        {
            return new AssemblyName();
        }
    }
    public class TypeDescriptor
    {
        public static PropertyInfo[] GetProperties(Type type)
        {
            return null;
        }
    }
}