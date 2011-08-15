using System;
using System.Collections;
using System.ComponentModel;

namespace OxyPlot.Reporting
{

    /// <summary>
    /// The PropertyTable autogenerates columns based on reflecting the Items type.
    /// Only [Browsable] properties are included.
    /// </summary>
    public class PropertyTable : ItemsTable
    {
        public PropertyTable(IEnumerable items, bool itemsInRows)
            : base(itemsInRows)
        {
            Alignment = Alignment.Left;
            UpdateFields(items);
            Items = items;
        }

        private void UpdateFields(IEnumerable items)
        {
            Type type = GetItemType(items);
            if (type == null)
                return;
            Columns.Clear();

#if SILVERLIGHT
            foreach (var pi in type.GetProperties())
            {
                var header = pi.Name;
                Fields.Add(new ItemsTableField(header, pi.Name, null, Alignment.Left));
            }
#else
            foreach (PropertyDescriptor p in TypeDescriptor.GetProperties(type))
            {
                if (!p.IsBrowsable)
                    continue;
                var header = p.DisplayName ?? p.Name;
                Fields.Add(new ItemsTableField(header, p.Name));
            }
#endif
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