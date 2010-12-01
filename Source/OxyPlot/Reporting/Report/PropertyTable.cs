using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace OxyPlot.Reporting
{
    public class PropertyTable : Table
    {
        public PropertyTable()
        {
            Transposed = true;
        }

        public override void Update()
        {
            Type type = GetItemType(Items);
            if (type == null)
                return;
            Columns.Clear();

            foreach (PropertyDescriptor p in TypeDescriptor.GetProperties(type))
            {
                if (!p.IsBrowsable)
                    continue;
                var header = p.DisplayName ?? p.Name;
                Columns.Add(new TableColumn(header, p.Name));
            }
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