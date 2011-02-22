using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace OxyPlot.Reporting
{
    /// <summary>
    /// The PropertyTable autogenerates columns based on reflecting the Items type.
    /// Only [Browsable] properties are included.
    /// </summary>
    public class PropertyTable : Table
    {
        public PropertyTable()
        {
            ItemsInColumns = true;
        }

        public override void Update()
        {
            Type type = GetItemType(Items);
            if (type == null)
                return;
            Columns.Clear();

#if SILVERLIGHT
            foreach (var pi in type.GetProperties())
            {
                var header = pi.Name;
                Columns.Add(new TableColumn(header, pi.Name));
            }
#else
            foreach (PropertyDescriptor p in TypeDescriptor.GetProperties(type))
            {
                if (!p.IsBrowsable)
                    continue;
                var header = p.DisplayName ?? p.Name;
                Columns.Add(new TableColumn(header, p.Name));
            }
#endif

            base.Update();
        }

        private Type GetItemType(IEnumerable items)
        {
            Type result = null;
            foreach (object item in items)
            {
                Type t = item.GetType();
                if (result == null)
                    result = t;
                if (t != result)
                    return null;
            }
            return result;
        }
    }
}