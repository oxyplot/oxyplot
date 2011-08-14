using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace OxyPlot
{
    public static class ReflectionHelper
    {
        public static void FillValues<T>(IEnumerable source, string field, IList<T> list)
        {
            PropertyInfo pi = null;
            Type t = null;
            foreach (var o in source)
            {
                if (pi == null || o.GetType() != t)
                {
                    t = o.GetType();
                    pi = t.GetProperty(field);
                    if (pi == null)
                    {
                        throw new InvalidOperationException(string.Format("Could not find field {0} on type {1}", field, t));
                    }

                }
                var value = (T)Convert.ChangeType(pi.GetValue(o, null), typeof(T), CultureInfo.InvariantCulture);
                list.Add(value);
            }

        }
    }
}
