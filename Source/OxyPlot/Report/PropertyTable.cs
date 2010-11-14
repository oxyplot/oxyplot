using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

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
            var type = GetItemType(Items);
            if (type == null)
                return;
            Fields.Clear();
            foreach (var pi in type.GetProperties())
            {
                Fields.Add(new TableColumn(pi.Name, pi.Name));
            }
            base.Update();
        }

        private Type GetItemType(IEnumerable items)
        {
            Type result = null;
            foreach (var item in items)
            {
                var t = item.GetType();
                if (result == null)
                    result = t;
                if (t != result)
                    return null;
            }
            return result;
        }
    }
}