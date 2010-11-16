using System;
using System.Collections;
using System.Reflection;

namespace OxyReporter
{
    public class PropertyTable : Table
    {
        public PropertyTable()
        {
            Flipped = true;
        }

        public override void Update()
        {
            Type type = GetItemType(Items);
            if (type == null)
                return;
            Fields.Clear();
            foreach (PropertyInfo pi in type.GetProperties())
            {
                Fields.Add(new TableColumn(pi.Name, pi.Name));
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